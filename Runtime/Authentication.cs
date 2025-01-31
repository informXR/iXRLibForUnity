using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;
using XRDM.SDK.External.Unity;

[DefaultExecutionOrder(1)]
public class Authentication : SdkBehaviour
{
    private static string _orgId;
    private static string _deviceId;
    private static string _authSecret;
    private static string _userId;
    private static string _appId;
    private static Partner _partner = Partner.eNone;
    private static int _failedAuthAttempts;
    
    protected override void OnEnable()
    {
#if UNITY_ANDROID
        base.OnEnable();
        var callBack = new Callback();
        Connect(callBack);
#endif
    }

    private static void CheckArborInfo()
    {
        if (Callback.Service == null) return;

        _partner = Partner.eArborXR;
        _orgId = Callback.Service.GetOrgId();
        _deviceId = Callback.Service.GetDeviceId();
        _authSecret = Callback.Service.GetFingerprint();
        _userId = Callback.Service.GetAccessToken();
    }
    
    private sealed class Callback : IConnectionCallback
    {
        public static ISdkService Service;
        
        public void OnConnected(ISdkService service) => Service = service;

        public void OnDisconnected(bool isRetrying) => Service = null;
    }
    
    private void Start()
    {
#if UNITY_ANDROID
        CheckArborInfo();
#endif
        if (GetDataFromConfig())
        {
            SetSessionData();
            Authenticate();
            if (iXRAuthentication.AuthMechanism.ContainsKey("prompt"))
            {
                KeyboardAuthenticate();
            }
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
			if (iXRAuthentication.TokenExpirationImminent())
            {
                ReAuthenticate();
            }
        }
        else
        {
            iXRInit.ForceSendUnsentSynchronous();
        }
    }

    private static bool GetDataFromConfig()
    {
        const string appIdPattern = "^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$";
        if (string.IsNullOrEmpty(Configuration.Instance.appID) || !Regex.IsMatch(Configuration.Instance.appID, appIdPattern))
        {
            Debug.LogError("iXRLib - Invalid Application ID. Cannot authenticate.");
            return false;
        }

        _appId = Configuration.Instance.appID;

        if (_partner == Partner.eArborXR) return true; // the rest of the values are set by Arbor
        
        _orgId = Configuration.Instance.orgID;
        if (string.IsNullOrEmpty(_orgId))
        {
            Debug.LogError("iXRLib - Missing Organization ID. Cannot authenticate.");
            return false;
        }
        
        const string orgIdPattern = "^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$";
        if (!Regex.IsMatch(_orgId, orgIdPattern))
        {
            Debug.LogError("iXRLib - Invalid Organization ID. Cannot authenticate.");
            return false;
        }

        _authSecret = Configuration.Instance.authSecret;
        if (string.IsNullOrEmpty(_authSecret))
        {
            Debug.LogError("iXRLib - Missing Auth Secret. Cannot authenticate.");
            return false;
        }
        
        _deviceId = SystemInfo.deviceUniqueIdentifier;

        return true;
    }

    public static async Task KeyboardAuthenticate(string keyboardInput = null)
    {
        if (keyboardInput != null)
        {
			System.Collections.Generic.Dictionary<string, string> localAuthMechanism = iXRAuthentication.AuthMechanism;
			string originalPrompt = localAuthMechanism["prompt"];
            localAuthMechanism["prompt"] = keyboardInput;
			iXRAuthentication.SetAuthMechanism(localAuthMechanism);
            iXRResult result = await Task.Run(iXRInit.FinalAuthenticate);
			if (result == iXRResult.Ok)
            {
                NonNativeKeyboard.Instance.Close();
                _failedAuthAttempts = 0;
                return;
            }

            localAuthMechanism["prompt"] = originalPrompt;
			iXRAuthentication.SetAuthMechanism(localAuthMechanism);
        }
        
        iXRAuthentication.AuthMechanism.TryGetValue("domain", out string emailDomain);
        string prompt = _failedAuthAttempts > 0 ? $"Authentication Failed ({_failedAuthAttempts})\n" : "";
        prompt += iXRAuthentication.AuthMechanism["prompt"];
        iXR.PresentKeyboard(prompt, iXRAuthentication.AuthMechanism["type"], emailDomain);
        _failedAuthAttempts++;
    }

    private static void Authenticate()
    {
        var result = iXRInit.Authenticate(_appId, _orgId, _deviceId, _authSecret, _partner);
        if (result == iXRResult.Ok)
        {
            Debug.Log("iXRLib - Authenticated successfully");
            return;
        }

        Debug.LogError($"iXRLib - Authentication failed : {result}");
    }

    private static void ReAuthenticate()
    {
        var result = iXRInit.ReAuthenticate(false);
        if (result == iXRResult.Ok)
        {
            Debug.Log("iXRLib - ReAuthenticated successfully");
        }
        else
        {
            Debug.LogError($"iXRLib - ReAuthentication failed : {result}");
        }
    }

    private static void SetSessionData()
    {
#if UNITY_ANDROID
        if (!string.IsNullOrEmpty(DeviceModel.deviceModel)) iXRAuthentication.DeviceModel = DeviceModel.deviceModel;
#endif
        iXRAuthentication.Partner = _partner;
        if (!string.IsNullOrEmpty(_userId)) iXRAuthentication.UserId = _userId;
        
        iXRAuthentication.OsVersion = SystemInfo.operatingSystem;
        
        var currentAssembly = Assembly.GetExecutingAssembly();
        AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (AssemblyName assemblyName in referencedAssemblies)
        {
            if (assemblyName.Name == "XRDM.SDK.External.Unity")
            {
                iXRAuthentication.XrdmVersion = assemblyName.Version.ToString();
                break;
            }
        }
        
        //TODO Geolocation
        
        iXRAuthentication.AppVersion = Application.version;
        iXRAuthentication.UnityVersion = Application.unityVersion;
        iXRAuthentication.DataPath = Application.persistentDataPath;

        SetIPAddress();
    }

    private static void SetIPAddress()
    {
        try
        {
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // Check for IPv4 addresses
                {
                    iXRAuthentication.IpAddress = ip.ToString();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("iXRLib - Failed to get local IP address: " + ex.Message);
        }
    }
}