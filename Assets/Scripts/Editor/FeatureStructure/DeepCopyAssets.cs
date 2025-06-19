using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace OLS
{
    public class DeepCopyAssets
    {
        public interface IFileNameChange
        {
            bool IsFileForRename(string filePath);
            string RenameFile(string filePath);
        }
        
        public interface ICheckFileAndReplace
        {
            bool IsFileForReplace(string filePath);
            string ReplaceContent(string content);
        }
        
        //[MenuItem("Assets/Deep Duplicate")]
        public static void DuplicateSelected()
        {
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var targetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath);
            DuplicateSelectedWithParameters(folderPath, targetPath);
        }

        public static void DuplicateSelectedWithParameters(string folderPath, string targetPath,
            Func<string, bool> copyFilesCriteria = null,
            List<ICheckFileAndReplace> checkReplacers = null,
            IFileNameChange fileNameChange = null)
        {
            try
            {
                AssetDatabase.StartAssetEditing();
                CopyDirectoryDeep(folderPath, targetPath, copyFilesCriteria, checkReplacers, fileNameChange);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            AssetDatabase.Refresh();
        }

        private static void CopyDirectoryDeep(string sourcePath,
            string destinationPath, Func<string, bool> copyFilesCriteria = null,
            List<ICheckFileAndReplace> checkReplacers = null,
            IFileNameChange fileNameChange = null)
        {
            CopyFilesRecursively(sourcePath, destinationPath, fileNameChange, copyFilesCriteria);

            if (checkReplacers == null)
            {
                checkReplacers = new(1);
            }
            
            checkReplacers.Insert(0, new CheckMetaFilesAndGenerateNewGuids());

            var allFiles = GetFilesRecursively(destinationPath);
            var filesForReplace = new List<(string filePath, List<ICheckFileAndReplace> replacers)>();

            foreach (string fileToModify in allFiles)
            {
                var replacers = checkReplacers.FindAll(c => c.IsFileForReplace(fileToModify));
                if (replacers.Count == 0)
                {
                    continue;
                }
                filesForReplace.Add((fileToModify, replacers));
            }

            foreach (var fileForReplace in filesForReplace)
            {
                string content = File.ReadAllText(fileForReplace.filePath);
                foreach (var replacer in fileForReplace.replacers)
                {
                    content = replacer.ReplaceContent(content);
                }

                File.WriteAllText(fileForReplace.filePath, content);
            }
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath, IFileNameChange fileNameChange = null, Func<string, bool> criteria = null)
        {
            sourcePath = Path.Combine(Environment.CurrentDirectory, sourcePath);
            targetPath = Path.Combine(Environment.CurrentDirectory, targetPath);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (criteria != null && !criteria(dirPath))
                {
                    continue;
                }

                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string oldPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (criteria != null && !criteria(oldPath))
                {
                    continue;
                }

                var newPath = oldPath.Replace(sourcePath, targetPath);
                if (fileNameChange != null && fileNameChange.IsFileForRename(newPath))
                {
                    newPath = fileNameChange.RenameFile(newPath);
                }

                File.Copy(oldPath, newPath, true);
            }
        }

        private static List<string> GetFilesRecursively(string path, Func<string, bool> criteria = null,
            List<string> files = null)
        {
            if (files == null)
            {
                files = new List<string>();
            }

            files.AddRange(Directory.GetFiles(path).Where(f => criteria == null || criteria(f)));

            foreach (string directory in Directory.GetDirectories(path))
            {
                GetFilesRecursively(directory, criteria, files);
            }

            return files;
        }
    }

    internal class CheckMetaFilesAndGenerateNewGuids : DeepCopyAssets.ICheckFileAndReplace
    {
        private Dictionary<string, string> guidTable = new();
        
        public bool IsFileForReplace(string filePath)
        {
            if (filePath.EndsWith(".meta"))
            {
                GenerateNewGuidsToTable(filePath);
                return true;
            }

            if (filePath.EndsWith(".mat"))
            {
                return true;
            }

            if (filePath.EndsWith(".prefab"))
            {
                return true;
            }

            if (filePath.EndsWith(".mat"))
            {
                return true;
            }

            if (filePath.EndsWith(".asset"))
            {
                return true;
            }

            if (filePath.EndsWith(".asmdef"))
            {
                return true;
            }

            return false;
        }

        public string ReplaceContent(string content)
        {
            foreach (var guidPair in guidTable)
            {
                content = content.Replace(guidPair.Key, guidPair.Value);
            }

            return content;
        }

        private void GenerateNewGuidsToTable(string filePath)
        {
            var file = new StreamReader(filePath);
            file.ReadLine();
            string guidLine = file.ReadLine();
            file.Close();
            string originalGuid = guidLine.Substring(6, guidLine.Length - 6);
            string newGuid = GUID.Generate().ToString().Replace("-", "");
            guidTable.Add(originalGuid, newGuid);
        }
    }
}