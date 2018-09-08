using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIFlib
{
    /*
    public class TifTag
    {
        // the tag ID and name indicates what this data means
        public int tagID;
        private string tagName { get { return GetTagName(tagID); } }

        // these describe the shape of the data
        public int dataByteSize;
        public int dataCount;
        public int dataFirstByte;

        // the data type defines how this block of data is formated
        public string dataTypeDesc;
        private int _dataType;
        public int dataType
        {
            set { SetDataType(value); }
            get { return _dataType; }
        }

        public string Info()
        {
            return $"{tagName} [{dataCount}x {dataTypeDesc}] @{dataFirstByte}";
        }

        private void SetDataType(int value)
        {
            switch (value)
            {
                case 1:
                    dataTypeDesc = "BYTE";
                    dataByteSize = 1;
                    break;
                case 2:
                    dataTypeDesc = "ASCII";
                    dataByteSize = 1;
                    break;
                case 3:
                    dataTypeDesc = "SHORT";
                    dataByteSize = 2;
                    break;
                case 4:
                    dataTypeDesc = "LONG";
                    dataByteSize = 4;
                    break;
                case 5:
                    dataTypeDesc = "RATIONAL";
                    dataByteSize = 8;
                    break;
                default:
                    throw new Exception($"unsupported data type: {dataType}");
            }
            _dataType = value;
        }

        private string GetTagName(int tagID)
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
    */
}
