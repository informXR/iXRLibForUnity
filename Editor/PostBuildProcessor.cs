using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEngine;

public class PostBuildProcessor
{
    private const string PackageName = "com.informxr.unity";
    private const string FileToCopy = "Assets/Plugins/WebGL/ixrlib.min.js";

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.WebGL) return;
        
        // Get the resolved package path (editor-only)
        string packagePath = GetPackagePath(PackageName);
        if (string.IsNullOrEmpty(packagePath)) return;
        
        string sourceFilePath = Path.Combine(packagePath, FileToCopy);
        string destinationFilePath = Path.Combine(pathToBuiltProject, Path.GetFileName(FileToCopy));

        // Copy the file to the WebGL build
        if (File.Exists(sourceFilePath))
        {
            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
            Debug.Log($"Copied {sourceFilePath} to {destinationFilePath}");
        }
        else
        {
            Debug.LogError($"Source file not found: {sourceFilePath}");
        }
    }

    private static string GetPackagePath(string packageName)
    {
        // Use Package Manager to get the resolved package path (editor-only)
        var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath($"Packages/{packageName}");
        return packageInfo?.resolvedPath;
    }
}
