using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using iXRLib;
using UnityEngine;
using XRDM.SDK.External.Unity;

[DefaultExecutionOrder(1)]
public class Authentication : SdkBehaviour
{
    private static Authentication _instance;
    private string _arborOrgId;
    private string _arborDeviceId;
    private string _arborAuthSecret;
    private string _arborUserToken;
    private Partner _partner = Partner.eNone;
    private DateTime _lostFocus = DateTime.MaxValue;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("Authentication");
        _instance = singletonObject.AddComponent<Authentication>();
        DontDestroyOnLoad(singletonObject);
    }
    
    protected override void OnEnable()
    {
#if UNITY_ANDROID
        base.OnEnable();
        var callBack = new Callback();
        Connect(callBack);
#endif
    }

    private void CheckArborInfo()
    {
        _arborOrgId = Callback.Service.GetOrgId();
        _arborDeviceId = Callback.Service.GetDeviceId();
        _arborAuthSecret = Callback.Service.GetFingerprint();
        _arborUserToken = Callback.Service.getAccessToken();
    }
    
    private sealed class Callback : IConnectionCallback
    {
        public static ISdkService Service;
        
        public void OnConnected(ISdkService service) => Service = service;

        public void OnDisconnected(bool isRetrying) => Service = null;
    }
    
    private void Start()
    {
        iXRAuthentication.Partner = Partner.eNone;
#if UNITY_ANDROID
        CheckArborInfo();
        if (!string.IsNullOrEmpty(_arborOrgId))
        {
            _partner = Partner.eArborXR;
            iXRAuthentication.Partner = Partner.eArborXR;
        }
#endif
        Authenticate();
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
			if (iXRAuthentication.TokenExpirationImminent())
            {
                Authenticate();
            }
        }
        else
        {
            iXRInit.ForceSendUnsentSynchronous();
            _lostFocus = DateTime.UtcNow;
        }
    }

    private void Authenticate()
    {
        const string appIdPattern = "^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$";
        if (!Regex.IsMatch(Configuration.instance.appID, appIdPattern))
        {
            Debug.LogError("iXRLib - Invalid Application ID. Cannot authenticate.");
            return;
        }
        
        string orgId = _arborOrgId;
        if (string.IsNullOrEmpty(orgId)) orgId = Configuration.instance.orgID;
        if (string.IsNullOrEmpty(orgId))
        {
            Debug.LogError("iXRLib - Missing Organization ID. Cannot authenticate.");
            return;
        }
        
        const string orgIdPattern = "^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$";
        if (!Regex.IsMatch(orgId, orgIdPattern))
        {
            Debug.LogError("iXRLib - Invalid Organization ID. Cannot authenticate.");
            return;
        }

        string deviceId = _arborDeviceId;
        if (string.IsNullOrEmpty(deviceId)) deviceId = SystemInfo.deviceUniqueIdentifier;
        if (string.IsNullOrEmpty(deviceId))
        {
            Debug.LogError("iXRLib - Missing Device ID. Cannot authenticate.");
            return;
        }

        string authSecret = _arborAuthSecret;
        if (string.IsNullOrEmpty(authSecret)) authSecret = Configuration.instance.authSecret;
        if (string.IsNullOrEmpty(authSecret))
        {
            Debug.LogError("iXRLib - Missing Auth Secret. Cannot authenticate.");
            return;
        }
        string userId = _arborUserToken;
        if (string.IsNullOrEmpty(userId)) userId = Configuration.instance.userId;
        SetAuthTelemetry();
        var result = iXRInit.Authenticate(Configuration.instance.appID, orgId, deviceId, authSecret, _partner);
        if (result == iXRResult.Ok)
        {
            Debug.Log("iXRLib - Authenticated successfully");
        }
        else
        {
            Debug.LogError($"iXRLib - Authentication failed : {result}");
        }
    }

    private static void SetAuthTelemetry()
    {
        //TODO Device Type
        
        iXR.TelemetryEntry("OS Version", $"Version={SystemInfo.operatingSystem}");
        iXRAuthentication.OsVersion = SystemInfo.operatingSystem;
        
        var currentAssembly = Assembly.GetExecutingAssembly();
        AssemblyName[] referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (AssemblyName assemblyName in referencedAssemblies)
        {
            if (assemblyName.Name == "XRDM.SDK.External.Unity")
            {
                iXR.TelemetryEntry("XRDM Version", $"Version={assemblyName.Version}");
                iXRAuthentication.XrdmVersion = assemblyName.Version.ToString();
                break;
            }
        }
        
        //TODO Geolocation

        iXR.TelemetryEntry("Application Version", $"Version={Application.version}");
        iXRAuthentication.AppVersion = Application.version;
        
        iXR.TelemetryEntry("Unity Version", $"Version={Application.unityVersion}");
        iXRAuthentication.UnityVersion = Application.unityVersion;

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
