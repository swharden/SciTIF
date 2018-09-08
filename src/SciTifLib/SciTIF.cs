using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SciTIFlib
{
    public class TifFile
    {
        public string filePath;
        public long fileSize;
        public bool validTif;

        public Logger log;

        private bool littleEndian = true;

        public TifFile(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            this.filePath = filePath;
            log = new Logger("SciTIF");
            log.Info($"Loading abf file: {filePath}");

            if (!File.Exists(filePath))
                throw new Exception($"file does not exist: {filePath}");

            fileSize = new System.IO.FileInfo(filePath).Length;
            validTif = true;
            FileOpen();
            ReadHeader();
            FileClose();
            if (validTif)
                log.Debug("TIF read successfully");
            else
                log.Warn("!!!FILE DID NOT READ SUCCESSFULLY!!!");
        }

        public string Info()
        {
            string msg = $"File: {System.IO.Path.GetFileName(filePath)}\n";
            msg += $"Full Path: {filePath}\n";
            msg += $"Valid TIF: {validTif}\n";
            if (littleEndian)
                msg += $"Byte Order: little endian (LSB last)\n";
            else
                msg += $"Byte Order: big endian (LSB first)\n";
            return msg;
        }

        /* 
         * 
         * FILE OPERATIONS
         * 
         */

        BinaryReader br;
        private void FileOpen()
        {
            log.Debug("opening file");
            br = new BinaryReader(File.Open(filePath, FileMode.Open));
            br.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        private void FileSeek(int byteLocation)
        {
            br.BaseStream.Seek(byteLocation, SeekOrigin.Begin);
        }

        private void FileClose()
        {
            log.Debug("file closed");
            br.Close();
        }

        private byte[] FileReadBytes(int charCount, int bytePosition = -1)
        {
            if (bytePosition >= 0)
                br.BaseStream.Seek(bytePosition, SeekOrigin.Begin);
            byte[] bytes = br.ReadBytes(charCount);
            if (!littleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        private int FileReadUInt16(int bytePosition = -1)
        {
            byte[] bytes = FileReadBytes(2, bytePosition);
            return (int)BitConverter.ToUInt16(bytes, 0);
        }

        private int FileReadUInt32(int bytePosition = -1)
        {
            byte[] bytes = FileReadBytes(4, bytePosition);
            return (int)BitConverter.ToUInt32(bytes, 0);
        }

        private string BytesToString(byte[] bytes)
        {
            string s = System.Text.Encoding.Default.GetString(bytes);
            return s;
        }

        private string BytesToHexstring(byte[] bytes)
        {
            string hexString = "";
            foreach (byte b in bytes)
                hexString += String.Format("{0:X2}", Convert.ToInt32(b));
            return hexString;
        }

        private string BytesToPretty(byte[] bytes)
        {
            string[] strBytes = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                strBytes[i] = string.Format("{0}", bytes[i]);
            }
            string s = string.Join(", ", strBytes);
            return $"[{s}]";
        }

        /* 
         * 
         * DATA READING
         * 
         */

        private void ReadHeader()
        {
            // create a hex string from the first four bytes
            FileSeek(0);
            string firstFour = BytesToHexstring(FileReadBytes(4));
            log.Debug($"First four bytes (hex): {firstFour}");

            // the first four bytes confirm TIF is valid and indicate endianness
            if (firstFour == "49492A00")
            {
                littleEndian = true;
            }
            else if (firstFour == "4D4D002A")
            {
                littleEndian = false;
            }
            else
            {
                validTif = false;
                log.Critical($"Invalid TIF identifier and/or version");
                return;
            }

            // the next 4 bytes are a Uint32 indicating where the first IDF is
            int nextIfdLocation = FileReadUInt32();
            log.Debug($"first IFD location: {nextIfdLocation}");

            // read IFDs until the last one is reached
            while (nextIfdLocation > 0)
            {
                nextIfdLocation = ReadIFD(nextIfdLocation);
            }

            return;
        }

        private int ReadIFD(int ifdLocation)
        {
            // An Image File Directory (IFD) is a collection of information similar to a header, 
            // and it is used to describe the bitmapped data to which it is attached.
            // It contains enteries called "tags", each 12 bytes long. Each tif tag describes
            // a little bit about some data in the file, such as what format its in (SHORT, LONG, etc),
            // how many values it has, and where the first byte is.

            // ensure this byte position is valid
            if (ifdLocation > fileSize)
            {
                validTif = false;
                log.Critical($"ReadIFD() called on invalid byte: {ifdLocation}");
                return 0;
            }

            FileSeek(ifdLocation);
            int entryCount = FileReadUInt16();
            log.Debug($"IFD at byte {ifdLocation} has {entryCount} tags");

            // read each tig tag and add it to the list
            List<TifTag> tifTags = new List<TifTag>();
            for (int i = 0; i < entryCount; i++)
            {
                TifTag tag = ReadTifTag();
                log.Debug("  " + tag.Info());
                tifTags.Add(tag);
            }

            // return the position of the next IFD
            int nextIfdLocation = FileReadUInt32();

            // now you're free to load the data
            log.Debug("Reading TIF tag data...");
            foreach (TifTag tag in tifTags)
            {
                byte[] bytes = FileReadBytes(tag.dataCount, tag.dataFirstByte);

                string vals;
                if (tag.dataType == 2)
                {
                    //vals = System.Text.Encoding.Default.GetString(bytes);
                    vals = $"string of length {bytes.Length}";
                }
                else if (bytes.Length==1)
                {
                    vals = bytes[0].ToString();
                }
                else
                {
                    vals = BytesToPretty(bytes);
                }

                log.Debug($"  {tag.idName} = {vals}");
            }
            return nextIfdLocation;
        }

        private TifTag ReadTifTag()
        {
            int id = FileReadUInt16();
            int dataType = FileReadUInt16();
            int dataCount = FileReadUInt32();
            int dataFirstByte = FileReadUInt32();
            return new TifTag(id, dataType, dataCount, dataFirstByte);
        }

        public class TifTag
        {
            private int id;
            public string idName;
            public int dataType;
            public bool dataTypeKnown = true;
            public int dataCount;
            public int dataFirstByte;
            public bool dataIsText = false;
            public int dataByteSize;

            public TifTag(int id, int dataType, int dataCount, int dataFirstByte)
            {
                this.id = id;
                this.dataType = dataType;
                this.dataCount = dataCount;
                this.dataFirstByte = dataFirstByte;
                idName = NameFromID(id);

                // determine how many bytes the tag data is
                if (dataType == 1) dataByteSize = 1;
                else if (dataType == 2) dataByteSize = 1;
                else if (dataType == 3) dataByteSize = 2;
                else if (dataType == 4) dataByteSize = 4;
                else if (dataType == 8) dataByteSize = 8;
                else dataTypeKnown = false;
                if (dataType == 2) dataIsText = true;
            }

            public string Info()
            {
                string msg = $"{idName} ({dataCount}x) dataType {dataType} at {dataFirstByte}";
                msg = msg.Replace(" (1x)", "");
                return msg;
            }

            private string NameFromID(int tagID)
            {
                if (tagID == 254) return "NewSubfileType";
                if (tagID == 255) return "SubfileType";
                if (tagID == 256) return "ImageWidth";
                if (tagID == 257) return "ImageLength";
                if (tagID == 258) return "BitsPerSample";
                if (tagID == 259) return "Compression";
                if (tagID == 262) return "PhotometricInterpretation";
                if (tagID == 263) return "Threshholding";
                if (tagID == 264) return "CellWidth";
                if (tagID == 265) return "CellLength";
                if (tagID == 266) return "FillOrder";
                if (tagID == 270) return "ImageDescription";
                if (tagID == 271) return "Make";
                if (tagID == 272) return "Model";
                if (tagID == 273) return "StripOffsets";
                if (tagID == 274) return "Orientation";
                if (tagID == 277) return "SamplesPerPixel";
                if (tagID == 278) return "RowsPerStrip";
                if (tagID == 279) return "StripByteCounts";
                if (tagID == 280) return "MinSampleValue";
                if (tagID == 281) return "MaxSampleValue";
                if (tagID == 282) return "XResolution";
                if (tagID == 283) return "YResolution";
                if (tagID == 284) return "PlanarConfiguration";
                if (tagID == 288) return "FreeOffsets";
                if (tagID == 289) return "FreeByteCounts";
                if (tagID == 290) return "GrayResponseUnit";
                if (tagID == 291) return "GrayResponseCurve";
                if (tagID == 296) return "ResolutionUnit";
                if (tagID == 305) return "Software";
                if (tagID == 306) return "DateTime";
                if (tagID == 315) return "Artist";
                if (tagID == 316) return "HostComputer";
                if (tagID == 320) return "ColorMap";
                if (tagID == 338) return "ExtraSamples";
                if (tagID == 33432) return "Copyright";
                return $"unknown tagID ({tagID})";
            }
        }



    }
}
