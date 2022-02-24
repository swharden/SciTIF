using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIF.Tests
{
    public class SampleData
    {
        public static string DataFolder = GetDataFolder();

        public static string[] TifFiles = Directory.GetFiles(DataFolder, "*.tif");

        private static string GetDataFolder()
        {
            string testDirectory = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
            string repoDirectory = Path.GetFullPath(Path.Combine(testDirectory, "../../../../../"));
            string sampleDataFilePath = Path.Combine(repoDirectory, "data/images/Lenna_(test_image).png");
            if (File.Exists(sampleDataFilePath))
                return Path.GetDirectoryName(sampleDataFilePath)!;
            else
                throw new FileNotFoundException(sampleDataFilePath);
        }

        [Test]
        public void Test_DataFolder_Exists()
        {
            Assert.That(Directory.Exists(DataFolder));
        }

        [Test]
        public void Test_DataFolder_HasImages()
        {
            Assert.IsNotEmpty(TifFiles);
        }

        public static List<(string, int, int, double)> ExpectedResults()
        {
            /* Made with ImageJ macro:
             *   print("\\Clear");
             *   titles = getList("image.titles");
             *   for (i=0; i<titles.length; i++){
	         *       selectWindow(titles[i]);
             *       print(titles[i] + ",0,0," + getValue(0,0));
             *       print(titles[i] + ",1,0," + getValue(1,0));
             *       print(titles[i] + ",7,3," + getValue(7,3));
             *   }
             */

            List<(string, int, int, double)> pixelValues = new();

            pixelValues.Add(("2018_08_14_DIC2_0000 4x.tif", 0, 0, 12));
            pixelValues.Add(("2018_08_14_DIC2_0000 4x.tif", 1, 0, 12));
            pixelValues.Add(("2018_08_14_DIC2_0000 4x.tif", 7, 3, 12));
            pixelValues.Add(("2018_08_14_DIC2_0000a.tif", 0, 0, 214));
            pixelValues.Add(("2018_08_14_DIC2_0000a.tif", 1, 0, 214));
            pixelValues.Add(("2018_08_14_DIC2_0000a.tif", 7, 3, 207));
            pixelValues.Add(("2018_08_14_DIC2_0000i.tif", 0, 0, 13));
            pixelValues.Add(("2018_08_14_DIC2_0000i.tif", 1, 0, 13));
            pixelValues.Add(("2018_08_14_DIC2_0000i.tif", 7, 3, 12));
            pixelValues.Add(("2018_08_14_DIC2_0000r.tif", 0, 0, 41));
            pixelValues.Add(("2018_08_14_DIC2_0000r.tif", 1, 0, 40));
            pixelValues.Add(("2018_08_14_DIC2_0000r.tif", 7, 3, 41));
            pixelValues.Add(("16923029b-after.tif", 0, 0, 49456));
            pixelValues.Add(("16923029b-after.tif", 1, 0, 48928));
            pixelValues.Add(("16923029b-after.tif", 7, 3, 49664));
            pixelValues.Add(("16923029-f10.tif", 0, 0, 133));
            pixelValues.Add(("16923029-f10.tif", 1, 0, 131));
            pixelValues.Add(("16923029-f10.tif", 7, 3, 138));
            pixelValues.Add(("16923029-f20.tif", 0, 0, 250));
            pixelValues.Add(("16923029-f20.tif", 1, 0, 247));
            pixelValues.Add(("16923029-f20.tif", 7, 3, 237));
            pixelValues.Add(("17418028_MMStack_Pos0.ome.tif", 0, 0, 379));
            pixelValues.Add(("17418028_MMStack_Pos0.ome.tif", 1, 0, 371));
            pixelValues.Add(("17418028_MMStack_Pos0.ome.tif", 7, 3, 348));
            pixelValues.Add(("18622000.tif", 0, 0, 187));
            pixelValues.Add(("18622000.tif", 1, 0, 187));
            pixelValues.Add(("18622000.tif", 7, 3, 189));
            pixelValues.Add(("1536355916.608.tif", 0, 0, 9));
            pixelValues.Add(("1536355916.608.tif", 1, 0, 7));
            pixelValues.Add(("1536355916.608.tif", 7, 3, 9));
            pixelValues.Add(("C3Z4F5.tif", 0, 0, 0));
            pixelValues.Add(("C3Z4F5.tif", 1, 0, 0));
            pixelValues.Add(("C3Z4F5.tif", 7, 3, 0));

            //pixelValues.Add(("calibration-20x-ruler-0.32365.tif", 0, 0, 1896)); // TODO: support this type
            //pixelValues.Add(("calibration-20x-ruler-0.32365.tif", 1, 0, 1896));
            //pixelValues.Add(("calibration-20x-ruler-0.32365.tif", 7, 3, 1929));

            //pixelValues.Add(("fluo-3ch-8bitColor.tif", 0, 0, 49.6667)); // TODO: support this type
            //pixelValues.Add(("fluo-3ch-8bitColor.tif", 1, 0, 15.3333));
            //pixelValues.Add(("fluo-3ch-8bitColor.tif", 7, 3, 24.6667));

            //pixelValues.Add(("fluo-3ch-8bit-composite.tif", 0, 0, 54)); // TODO: support this type
            //pixelValues.Add(("fluo-3ch-8bit-composite.tif", 1, 0, 9));
            //pixelValues.Add(("fluo-3ch-8bit-composite.tif", 7, 3, 29));

            //pixelValues.Add(("fluo-3ch-16bit.tif", 0, 0, 91)); // TODO: support this type
            //pixelValues.Add(("fluo-3ch-16bit.tif", 1, 0, 76));
            //pixelValues.Add(("fluo-3ch-16bit.tif", 7, 3, 206));

            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch1_000001.ome.tif", 0, 0, 247));
            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch1_000001.ome.tif", 1, 0, 139));
            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch1_000001.ome.tif", 7, 3, 154));
            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch2_000001.ome.tif", 0, 0, 98));
            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch2_000001.ome.tif", 1, 0, 98));
            pixelValues.Add(("LineScan-06092017-1414-623_Cycle00001_Ch2_000001.ome.tif", 7, 3, 96));

            //pixelValues.Add(("LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif", 0, 0, 7)); // TODO: support this type
            //pixelValues.Add(("LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif", 1, 0, 7));
            //pixelValues.Add(("LineScan-06092017-1414-623-Cycle00002-Window2-Ch1-8bit-Reference.tif", 7, 3, 4));

            //pixelValues.Add(("proj.tif", 0, 0, 260)); // TODO: support this type
            //pixelValues.Add(("proj.tif", 1, 0, 273));
            //pixelValues.Add(("proj.tif", 7, 3, 283));

            pixelValues.Add(("SingleImage-04142017-1215-126_Cycle00001_Ch2_000001.ome.tif", 0, 0, 163));
            pixelValues.Add(("SingleImage-04142017-1215-126_Cycle00001_Ch2_000001.ome.tif", 1, 0, 154));
            pixelValues.Add(("SingleImage-04142017-1215-126_Cycle00001_Ch2_000001.ome.tif", 7, 3, 124));

            pixelValues.Add(("Substack (1-81-4)0012.tif", 0, 0, 7));
            pixelValues.Add(("Substack (1-81-4)0012.tif", 1, 0, 7));
            pixelValues.Add(("Substack (1-81-4)0012.tif", 7, 3, 7));

            pixelValues.Add(("TSeries-04142017-1215-1341_Cycle00001_Ch1_000082.ome.tif", 0, 0, 100));
            pixelValues.Add(("TSeries-04142017-1215-1341_Cycle00001_Ch1_000082.ome.tif", 1, 0, 84));
            pixelValues.Add(("TSeries-04142017-1215-1341_Cycle00001_Ch1_000082.ome.tif", 7, 3, 98));

            pixelValues.Add(("TSeries-04142017-1215-1342_Cycle00001_Ch1_000007.ome.tif", 0, 0, 137));
            pixelValues.Add(("TSeries-04142017-1215-1342_Cycle00001_Ch1_000007.ome.tif", 1, 0, 76));
            pixelValues.Add(("TSeries-04142017-1215-1342_Cycle00001_Ch1_000007.ome.tif", 7, 3, 118));

            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch1_000007.ome.tif", 0, 0, 890));
            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch1_000007.ome.tif", 1, 0, 520));
            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch1_000007.ome.tif", 7, 3, 382));

            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch2_000007.ome.tif", 0, 0, 302));
            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch2_000007.ome.tif", 1, 0, 410));
            pixelValues.Add(("TSeries-12022016-1322-1168_Cycle00001_Ch2_000007.ome.tif", 7, 3, 203));

            pixelValues.Add(("video-gcamp.tif", 0, 0, 7));
            pixelValues.Add(("video-gcamp.tif", 1, 0, 7));
            pixelValues.Add(("video-gcamp.tif", 7, 3, 7));

            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch1_MIP.tif", 0, 0, 139));
            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch1_MIP.tif", 1, 0, 146));
            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch1_MIP.tif", 7, 3, 135));

            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch2_MIP.tif", 0, 0, 203));
            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch2_MIP.tif", 1, 0, 218));
            pixelValues.Add(("ZSeries-06082017-1213-682_Cycle00001_Ch2_MIP.tif", 7, 3, 189));

            return pixelValues;
        }
    }
}
