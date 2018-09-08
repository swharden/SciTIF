using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SplitDirView
{

    class NavFiles
    {
        public NavFiles(string pathFolder)
        {
            ScanFolder(pathFolder);
        }

        public string[] paths;
        private string[] pathFolders;
        private string[] pathFiles;
        private void ScanFolder(string pathFolder)
        {
            if (!Directory.Exists(pathFolder))
            {
                Console.WriteLine($"folder does not exist: {pathFolder}");
                return;
            }
            try
            {
                pathFolders = System.IO.Directory.GetDirectories(pathFolder);
                pathFiles = System.IO.Directory.GetFiles(pathFolder, "*.*");
            }
            catch
            {
                Console.WriteLine($"we do not have access to: {pathFolder}");
                return;
            }
            paths = new string[pathFiles.Length + pathFolders.Length];
            paths = pathFolders.Union(pathFiles).ToArray();
        }

        public string[] GetPathNames()
        {
            if (pathFolders == null || pathFiles == null)
                return new string[] { };
            string[] pathNames = new string[pathFolders.Length + pathFiles.Length];

            // first prepare list of folders
            for (int i = 0; i < pathFolders.Length; i++)
            {
                pathNames[i] = Path.GetFileName(pathFolders[i]) + "/";
            }

            // then prepare list of files
            for (int i = 0; i < pathFiles.Length; i++)
            {
                pathNames[i + pathFolders.Length] = Path.GetFileName(pathFiles[i]);
            }

            return pathNames;
        }

    }
}
