using System;
using UnityEngine;
using UnityEditor;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public class UpdateCheck
{
    private const string PackageUrl = "https://github.com/informXR/iXRLibUnitySDK.git";
    private const string PackageName = "com.informxr.unity-sdk";
    private const string SkippedVersionKey = "SkippedPackageVersion";
    private const int UpdateCheckFrequencyDays = 1;
    public const string UpdateCheckPref = "iXR_updateCheck";

    [MenuItem("informXR/Check For Updates")]
    public static async void CheckForUpdatesMenu()
    {
        EditorPrefs.DeleteKey(SkippedVersionKey);
        EditorPrefs.DeleteKey(UpdateCheckPref);
        await CheckForUpdates(true);
    }

    public static async Task CheckForUpdates(bool forced = false)
    {
        string currentVersion = await CheckPackageVersion();
        
        // Remove the HTTP client and GitHub API check
        // Instead, we'll check if the package is up to date using Unity's package manager
        
        bool isUpToDate = await IsPackageUpToDate(currentVersion);
        
        if (isUpToDate)
        {
            if (forced)
            {
                EditorUtility.DisplayDialog("Up-to-date", "You have the latest version of the package.", "OK");
            }
        }
        else
        {
            ShowUpdateDialog(currentVersion);
        }
        
        EditorPrefs.SetString(UpdateCheckPref, DateTime.UtcNow.AddDays(UpdateCheckFrequencyDays).ToString("G"));
    }

    private static async Task<bool> IsPackageUpToDate(string currentVersion)
    {
        AddRequest addRequest = Client.Add(PackageUrl);
        
        while (!addRequest.IsCompleted)
        {
            await Task.Delay(100);
        }

        if (addRequest.Status == StatusCode.Success)
        {
            string latestVersion = addRequest.Result.version;
            return currentVersion == latestVersion;
        }
        else
        {
            Debug.LogError($"Failed to check for updates: {addRequest.Error.message}");
            return true; // Assume up to date to avoid unnecessary prompts
        }
    }

    private static void ShowUpdateDialog(string currentVersion)
    {
        int option = EditorUtility.DisplayDialogComplex("Update Available",
            $"A new version is available. Your current version is {currentVersion}. Would you like to update?",
            "Update", "Cancel", "Skip This Version");

        if (option == 0) // Update
        {
            UpdatePackage();
        }
        else if (option == 2) // Skip this version
        {
            EditorPrefs.SetString(SkippedVersionKey, currentVersion);
        }
    }

    private static void UpdatePackage()
    {
        Client.Add(PackageUrl);
    }

    private static async Task<string> CheckPackageVersion()
    {
        ListRequest listRequest = Client.List(); // List all packages installed in the project

        while (!listRequest.IsCompleted)
        {
            await Task.Delay(100); // Wait for the request to complete
        }

        if (listRequest.Status == StatusCode.Success)
        {
            foreach (var package in listRequest.Result)
            {
                if (package.name == PackageName)
                {
                    return package.version;
                }
            }
            
            Debug.LogWarning($"Package {PackageName} not found.");
        }
        else if (listRequest.Status >= StatusCode.Failure)
        {
            Debug.LogError(listRequest.Error.message);
        }

        return "";
    }

    [Serializable]
    private class GitHubRelease
    {
        public string tag_name;
        public string html_url;
    }
}
