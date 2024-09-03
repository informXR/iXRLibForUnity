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
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            Debug.LogError("Failed to get current package version.");
            return;
        }

        bool updateAvailable = await IsUpdateAvailable(currentVersion);
        
        if (!updateAvailable)
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

    private static async Task<bool> IsUpdateAvailable(string currentVersion)
    {
        ListRequest listRequest = Client.List(true); // true to include packages from the registry

        while (!listRequest.IsCompleted)
        {
            await Task.Delay(100);
        }

        if (listRequest.Status == StatusCode.Success)
        {
            foreach (var package in listRequest.Result)
            {
                if (package.name == PackageName)
                {
                    return package.version != currentVersion;
                }
            }
            Debug.LogWarning($"Package {PackageName} not found in the registry.");
            return false;
        }
        else
        {
            Debug.LogError($"Failed to check for updates: {listRequest.Error.message}");
            return false;
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
        AddRequest addRequest = Client.Add(PackageUrl);
        EditorApplication.update += ProgressUpdate;

        void ProgressUpdate()
        {
            if (addRequest.IsCompleted)
            {
                if (addRequest.Status == StatusCode.Success)
                    Debug.Log("Package updated successfully");
                else if (addRequest.Status >= StatusCode.Failure)
                    Debug.LogError($"Package update failed: {addRequest.Error.message}");

                EditorApplication.update -= ProgressUpdate;
            }
        }
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
