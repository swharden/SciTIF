using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TifLib
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
            log = new Logger("TifLib");
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

        BinaryReader br;
        public void FileOpen()
        {
            log.Debug("opening file");
            br = new BinaryReader(File.Open(filePath, FileMode.Open));
            br.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public void FileSeek(int byteLocation)
        {
            br.BaseStream.Seek(byteLocation, SeekOrigin.Begin);
        }

        public void FileClose()
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

        public void ReadHeader()
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

        public int ReadIFD(int ifdLocation)
        {
            // An Image File Directory (IFD) is a collection of information similar to a header, 
            // and it is used to describe the bitmapped data to which it is attached.
            // It contains enteries called "tags", each 12 bytes long.

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

            // each entry contains 12 bytes
            for (int i = 0; i < entryCount; i++)
                ReadTag();

            // determine if another IFD exists and where
            int nextIfdLocation = FileReadUInt32();
            if (nextIfdLocation == 0)
                log.Debug($"  this was the last IFD.");
            else
                log.Debug($"  next IFD location: {nextIfdLocation}");

            // sanity check
            if (nextIfdLocation == ifdLocation)
                throw new Exception("next IFD location same as this one!");

            return nextIfdLocation;
        }

        public void ReadTag()
        {
            string[] typeNames = { "?", "BYTE", "ASCII", "SHORT", "LONG", "RATIONAL",
                "SBYTE", "UNDEFINE", "SSHORT", "SLONG", "SRATIONAL", "FLOAT", "DOUBLE" };

            int[] supportedTypes = { 1, 2, 3, 4, 5 };

            //string tagString = BytesToPretty(bytes);
            //log.Debug($"  tag: {tagString}");
            int ID = FileReadUInt16();
            int dataType = FileReadUInt16();
            string typeName = typeNames[dataType];
            int count = FileReadUInt32();
            int location = FileReadUInt32();
            log.Debug($"  {ID} {typeName} (type {dataType}) {count} @ byte {location}");

            if (!supportedTypes.Contains(dataType))
                throw new Exception($"unsupported tag type {dataType} ({typeNames[dataType]})");

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
    }
}
