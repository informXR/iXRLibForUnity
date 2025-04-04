using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using XRDM.SDK.External.Unity;

[DefaultExecutionOrder(1)]
public class Authentication : SdkBehaviour
{
    private static string _orgId;
    private static string _deviceId;
    private static string _authSecret;
    private static string _userId;
    private static string _appId;
    private static Partner _partner = Partner.None;
    private static string _deviceModel;
    private static string _osVersion;
    private static string _xrdmVersion;
    private static string _appVersion;
    private static string _unityVersion;
    private static string _dataPath;
    private static string _ipAddress;
    private static int _failedAuthAttempts;

    public static string Token;
    public static string Secret;
    
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

        _partner = Partner.ArborXR;
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
    
    private async void Start()
    {
#if UNITY_ANDROID
        CheckArborInfo();
#endif
        if (GetDataFromConfig())
        {
            SetSessionData();
            await AuthenticateAsync();
            //if (iXRAuthentication.AuthMechanism.ContainsKey("prompt"))
            {
            //    KeyboardAuthenticate();
            }
        }
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
			//if (iXRAuthentication.TokenExpirationImminent())
            {
            //    ReAuthenticate();
            }
        }
        else
        {
            //iXRInit.ForceSendUnsent();
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

        if (_partner == Partner.ArborXR) return true; // the rest of the values are set by Arbor
        
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
        return;
        /*if (keyboardInput != null)
        {
			System.Collections.Generic.Dictionary<string, string> localAuthMechanism = iXRAuthentication.AuthMechanism;
			string originalPrompt = localAuthMechanism["prompt"];
            localAuthMechanism["prompt"] = keyboardInput;
			iXRAuthentication.SetAuthMechanism(localAuthMechanism);
            iXR.iXRResult result = await Task.Run(iXRInit.FinalAuthenticate);
			if (result == iXR.iXRResult.Ok)
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
        _failedAuthAttempts++;*/
    }

    /*private static void ReAuthenticate()
    {
        var result = iXRInit.ReAuthenticate(false);
        if (result == iXR.iXRResult.Ok)
        {
            Debug.Log("iXRLib - ReAuthenticated successfully");
        }
        else
        {
            Debug.LogError($"iXRLib - ReAuthentication failed : {result}");
        }
    }*/

    private static void SetSessionData()
    {
#if UNITY_ANDROID
        if (!string.IsNullOrEmpty(DeviceModel.deviceModel)) _deviceModel = DeviceModel.deviceModel;
#endif
        _osVersion = SystemInfo.operatingSystem;
        
        var currentAssembly = Assembly.GetExecutingAssembly();
        AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (AssemblyName assemblyName in referencedAssemblies)
        {
            if (assemblyName.Name == "XRDM.SDK.External.Unity")
            {
                _xrdmVersion = assemblyName.Version.ToString();
                break;
            }
        }
        
        //TODO Geolocation
        
        _appVersion = Application.version;
        _unityVersion = Application.unityVersion;
        _dataPath = Application.persistentDataPath;

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
                    _ipAddress = ip.ToString();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("iXRLib - Failed to get local IP address: " + ex.Message);
        }
    }

    private static async Task AuthenticateAsync()
    {
        var data = new AuthPayload
        {
            appId = _appId,
            orgId = _orgId,
            authSecret = _authSecret,
            deviceId = _deviceId,
            userId = _userId,
            tags = new string[] { },
            sessionId = "someSessionId",
            partner = _partner.ToString().ToLower(),
            ipAddress = _ipAddress,
            deviceModel = _deviceModel,
            geolocation = new Dictionary<string, string>(),
            osVersion = _osVersion,
            xrdmVersion = _xrdmVersion,
            appVersion = _appVersion,
            authMechanism = new Dictionary<string, string>()
        };
        
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), "/v1/auth/token");
        using var request = new UnityWebRequest(fullUri.ToString(), "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield(); // let Unity continue rendering while we wait
            
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("iXRLib - Authenticated successfully");
            AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(request.downloadHandler.text);
            Token = response.Token;
            Secret = response.Secret;
        }
        else
        {
            Debug.LogError($"iXRLib - Authentication failed : {request.error}");
        }
    }

    private class AuthPayload
    {
        public string appId;
        public string orgId;
        public string authSecret;
        public string deviceId;
        public string userId;
        public string[] tags;
        public string sessionId;
        public string partner;
        public string ipAddress;
        public string deviceModel;
        public Dictionary<string, string> geolocation;
        public string osVersion;
        public string xrdmVersion;
        public string appVersion;
        public Dictionary<string, string> authMechanism;
    }

    private class AuthResponse
    {
        public string Token;
        public string Secret;
    }

    private enum Partner
    {
        None,
        ArborXR
    }
}