using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciTIFlib
{
    /// <summary>
    /// tools related to file and folder operations
    /// </summary>
    public class Path
    {
        public Path()
        {

        }
        /// <summary>
        /// return the filename of the next or previous image
        /// </summary>
        public string AdjacentFilename(string currentFilePath, string extension=".tif", bool reverse = false)
        {
            string currentBasename = System.IO.Path.GetFileName(currentFilePath);
            string imageFolder = System.IO.Path.GetDirectoryName(currentFilePath);
            string[] imageFiles = System.IO.Directory.GetFiles(imageFolder);
            Array.Sort(imageFiles);

            // not multiple files in this folder so dont do anything
            if (imageFiles.Length < 2)
                return null;

            // step through all the rest
            string imageFirst = imageFiles[0];
            string imageLast = imageFiles[imageFiles.Length - 1];

            string imageNext;
            string imagePrevious;

            for (int i = 0; i < imageFiles.Length; i++)
            {
                string maybeBasename = System.IO.Path.GetFileName(imageFiles[i]);
                System.Console.WriteLine($"{currentBasename.ToUpper()} {maybeBasename.ToUpper()}");
                if (currentBasename.ToUpper() != maybeBasename.ToUpper())
                    continue;

                if (i == 0)
                    imagePrevious = imageLast;
                else
                    imagePrevious = imageFiles[i - 1];

                if (i == imageFiles.Length - 1)
                    imageNext = imageFirst;
                else
                    imageNext = imageFiles[i + 1];

                if (reverse)
                    return imagePrevious;
                else
                    return imageNext;
            }

            return null;

        }
    }
}
