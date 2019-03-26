using System.Linq;

namespace WebApp.Common
{
    public class FileCommon
    {
        public static void TranferFolder(string folderName, string sourcePath, string targetPath)
        {
            if (!string.IsNullOrEmpty(folderName) && !string.IsNullOrEmpty(sourcePath) && !string.IsNullOrEmpty(targetPath))
            {
                // To copy all the files in one directory to another directory.
                if (System.IO.Directory.Exists(sourcePath))
                {
                    //string _targetPath = targetPath + folderName;

                    // Create a new target folder, if necessary.
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    // Get files and folders in source path
                    string[] files = System.IO.Directory.GetFiles(sourcePath);
                    string[] folders = System.IO.Directory.GetDirectories(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string item in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(item);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(item, destFile, true);
                    }

                    CopyFile(folders, sourcePath, targetPath);
                }
            }
        }

        public static void CopyFile(string[] folders, string sourcePath, string targetPath)
        {
            if (!string.IsNullOrEmpty(targetPath) && folders.Any())
            {
                for (int i = 0; i < folders.Count(); i++)
                {
                    string folderName = folders[i].Split('\\').LastOrDefault();
                    if (!string.IsNullOrEmpty(folderName) && folderName != "uploads")
                    {
                        string newSourcePath = folders[i];
                        string newTargetPath = targetPath + "\\" + folderName;

                        if (System.IO.Directory.Exists(newSourcePath))
                        {
                            // Create a new target folder, if necessary.
                            if (!System.IO.Directory.Exists(newTargetPath))
                            {
                                System.IO.Directory.CreateDirectory(newTargetPath);
                            }

                            // Get files and folders in source path
                            string[] files = System.IO.Directory.GetFiles(newSourcePath);
                            string[] subFolders = System.IO.Directory.GetDirectories(newSourcePath);

                            // Copy the files and overwrite destination files if they already exist.
                            foreach (string item in files)
                            {
                                // Use static Path methods to extract only the file name from the path.
                                string fileName = System.IO.Path.GetFileName(item);
                                string destFile = System.IO.Path.Combine(newTargetPath, fileName);
                                System.IO.File.Copy(item, destFile, true);
                            }

                            CopyFile(subFolders, newSourcePath, newTargetPath);
                        }
                    }
                }
            }
        }

        public static void DeleteFolder(string sourcePath)
        {
            if (System.IO.Directory.Exists(sourcePath))
            {
                System.IO.Directory.Delete(sourcePath, true);
            }
        }
    }
}
