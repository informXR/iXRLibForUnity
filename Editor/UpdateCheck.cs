using System;
using UnityEngine;
using UnityEditor;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public class UpdateCheck
{
    private const string PackageUrl = "https://api.github.com/repos/informXR/iXRLibUnitySDK/releases/latest";
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
        using var client = new HttpClient();
        try
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
            var response = await client.GetStringAsync(PackageUrl);
            var latestRelease = JsonUtility.FromJson<GitHubRelease>(response);

            string skippedVersion = EditorPrefs.GetString(SkippedVersionKey, "");

            if (latestRelease.tag_name == currentVersion)
            {
                if (forced)
                {
                    EditorUtility.DisplayDialog("Up-to-date", "You have the latest version of the package.", "OK");
                }
            }
            else
            {
                if (latestRelease.tag_name != skippedVersion || forced)
                {
                    ShowUpdateDialog(latestRelease);
                }
            }
            
            EditorPrefs.SetString(UpdateCheckPref, DateTime.UtcNow.AddDays(UpdateCheckFrequencyDays).ToString("G"));
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("Error checking for updates: " + e.Message);
        }
    }

    private static void ShowUpdateDialog(GitHubRelease latestRelease)
    {
        int option = EditorUtility.DisplayDialogComplex("Update Available",
            $"A new version ({latestRelease.tag_name}) is available. Please update your package.",
            "Download", "Cancel", "Skip This Version");

        if (option == 0) // Download
        {
            Application.OpenURL(latestRelease.html_url);
        }
        else if (option == 2) // Skip this version
        {
            EditorPrefs.SetString(SkippedVersionKey, latestRelease.tag_name);
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
