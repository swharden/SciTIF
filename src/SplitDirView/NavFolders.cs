using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SplitDirView
{

    class NavFolders
    {
        public NavFolders(string pathFolder)
        {
            DirectoryNavigatorSetup(pathFolder);
        }

        /// <summary>
        /// This object represents paths in the file navigator
        /// </summary>
        public class FolderPath
        {
            public string path = "path";
            public string label = "label";
        }

        public List<FolderPath> folders = new List<FolderPath>();

        /// <summary>
        /// scan a folder and populate that path in the folder navigator
        /// </summary>
        private void DirectoryNavigatorSetup(string pathFolder)
        {
            folders.Clear();
            if (!Directory.Exists(pathFolder))
            {
                Console.WriteLine($"folder does not exist: {pathFolder}");
                return;
            }
            while (pathFolder != null)
            {
                FolderPath nv = new FolderPath();
                nv.path = pathFolder;
                nv.label = Path.GetFileName(pathFolder);
                if (nv.label == "")
                    nv.label = Directory.GetDirectoryRoot(pathFolder);
                folders.Add(nv);
                pathFolder = Path.GetDirectoryName(pathFolder);
            }
            folders.Reverse();
        }

        /// <summary>
        /// Return a list of labels suitable for display in a listbox
        /// </summary>
        public string[] GetLabels(string padWith = " ")
        {
            string[] labels = new string[folders.Count];
            for (int i = 0; i < folders.Count; i++)
            {
                string pad = String.Concat(Enumerable.Repeat(padWith, i));
                labels[i] = pad + folders[i].label;
            }
            return labels;
        }
    }
}
