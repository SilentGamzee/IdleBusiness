using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OLS.Editor.FeatureStructure
{
    public class FeatureStructureCreator
    {
        private const string FeaturesFolderPath = "Assets/Scripts/Features";

        internal class DoCreateFeatureFolderStructure : EndNameEditAction, DeepCopyAssets.ICheckFileAndReplace, DeepCopyAssets.IFileNameChange
        {
            private const string FeatureStructureTemplatePath = "Assets/Scripts/Editor/FeatureStructure/FeatureTemplate";
            private const string featureTemplateSuffix = "FeatureTemplate";
            
            private string replaceFeatureName = string.Empty;
            
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var featureName = Path.GetFileName(pathName);
                ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(
                    AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(Path.GetDirectoryName(pathName), Path.GetFileName(pathName))), typeof(Object)));
                replaceFeatureName = featureName;
                DeepCopyAssets.DuplicateSelectedWithParameters(FeatureStructureTemplatePath, pathName, 
                    null,
                    new List<DeepCopyAssets.ICheckFileAndReplace> { this }, 
                    this);
                
                AssetDatabase.ImportAsset(pathName, ImportAssetOptions.ForceUpdate);
            }

            public bool IsFileForReplace(string filePath)
            {
                if (filePath.EndsWith(".cs"))
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
                return content.Replace(featureTemplateSuffix, replaceFeatureName);
            }

            public bool IsFileForRename(string filePath)
            {
                return filePath.Contains(featureTemplateSuffix);
            }

            public string RenameFile(string filePath)
            {
                return filePath.Replace(featureTemplateSuffix, replaceFeatureName);
            }
        }

        [MenuItem("Assets/Create/Folder Feature Structure", true, 20)]
        public static bool CreateFeatureStructureValidator(MenuCommand command)
        {
            if (Selection.assetGUIDs.Length != 1)
            {
                return false;
            }
            
            string folderPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            folderPath = folderPath.Replace("\\", "/");
            if (!folderPath.StartsWith(FeaturesFolderPath))
            {
                return false;
            }
            
            bool isValidFolder = AssetDatabase.IsValidFolder(folderPath);
            Debug.Log($"{folderPath}; {isValidFolder}");
            return true;
        }

        [MenuItem("Assets/Create/Folder Feature Structure", false, 20)]
        public static void CreateFeatureStructure()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreateFeatureFolderStructure>(), "FeatureName",
                EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D, null);
        }
    }
}