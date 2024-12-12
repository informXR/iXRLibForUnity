using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;
using XRDM.SDK.External.Unity;

public interface IAuthenticationService
{
    void Initialize(IConfigurationService configService, IIxrService ixrService);
    void Authenticate();
    void ReAuthenticate();
    Task KeyboardAuthenticate(string keyboardInput = null);
    void SetSessionData();
}

public class AuthenticationService : MonoBehaviour, IAuthenticationService
{
    private IConfigurationService _configService;
    private IIxrService _ixrService;

    private string _orgId;
    private string _deviceId;
    private string _authSecret;
    private string _userId;
    private string _appId;
    private Partner _partner = Partner.eNone;
    private int _failedAuthAttempts;

    public void Initialize(IConfigurationService configService, IIxrService ixrService)
    {
        _configService = configService;
        _ixrService = ixrService;

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

    private void CheckArborInfo()
    {
        if (Callback.Service == null) return;

        _partner = Partner.eArborXR;
        _orgId = Callback.Service.GetOrgId();
        _deviceId = Callback.Service.GetDeviceId();
        _authSecret = Callback.Service.GetFingerprint();
        _userId = Callback.Service.GetAccessToken();
    }

    private bool GetDataFromConfig()
    {
        var config = _configService.GetConfiguration();
        const string appIdPattern = "^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$";
        if (string.IsNullOrEmpty(config.appID) || !Regex.IsMatch(config.appID, appIdPattern))
        {
            Debug.LogError("iXRLib - Invalid Application ID. Cannot authenticate.");
            return false;
        }

        _appId = config.appID;

        if (_partner == Partner.eArborXR) return true;

        _orgId = config.orgID;
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

        _authSecret = config.authSecret;
        if (string.IsNullOrEmpty(_authSecret))
        {
            Debug.LogError("iXRLib - Missing Auth Secret. Cannot authenticate.");
            return false;
        }

        _deviceId = SystemInfo.deviceUniqueIdentifier;

        return true;
    }

    public async Task KeyboardAuthenticate(string keyboardInput = null)
    {
        if (keyboardInput != null)
        {
            Dictionary<string, string> localAuthMechanism = iXRAuthentication.AuthMechanism;
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
        _ixrService.PresentKeyboard(prompt, iXRAuthentication.AuthMechanism["type"], emailDomain);
        _failedAuthAttempts++;
    }

    public void Authenticate()
    {
        var result = iXRInit.Authenticate(_appId, _orgId, _deviceId, _authSecret, _partner);
        if (result == iXRResult.Ok)
        {
            Debug.Log("iXRLib - Authenticated successfully");
            return;
        }

        Debug.LogError($"iXRLib - Authentication failed : {result}");
    }

    public void ReAuthenticate()
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

    public void SetSessionData()
    {
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

        iXRAuthentication.AppVersion = Application.version;
        iXRAuthentication.UnityVersion = Application.unityVersion;

        SetIPAddress();
    }

    private void SetIPAddress()
    {
        try
        {
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
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
