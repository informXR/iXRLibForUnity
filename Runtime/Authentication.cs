using System.Text.RegularExpressions;
using iXRLib;
using UnityEngine;
using XRDM.SDK.External.Unity;

[DefaultExecutionOrder(1)]
public class Authentication : SdkBehaviour
{
    public static bool Authenticated;
    private static Authentication _instance;
    private string _arborOrgId;
    private string _arborDeviceId;
    private string _arborAuthSecret;
    private Partner _partner = Partner.eNone;
    
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
        _arborAuthSecret = Callback.Service.GetAuthSecret();
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
        if (!string.IsNullOrEmpty(_arborOrgId)) _partner = Partner.eArborXR;
#endif
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
        
        var result = iXRInit.Authenticate(Configuration.instance.appID, orgId, deviceId, authSecret, _partner);
        if (result == iXRResult.Ok)
        {
            Debug.Log("iXRLib - Authenticated successfully");
            Authenticated = true;
        }
        else
        {
            Debug.LogError($"iXRLib - Authentication failed : {result}");
            Authenticated = false;
        }
    }
}