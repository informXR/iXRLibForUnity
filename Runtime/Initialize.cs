using System;
using System.Threading.Tasks;
using iXRLib;
using UnityEngine;

public static class Initialize
{
	public delegate void SetTextMesh(string sz);
	// ---
	static SetTextMesh	m_pfnSetTextMesh = null;
	// ---
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
		string	szOrgId = null,
				szDeviceId = null,
				szAuthSecret = null;

        TestDiagnosticStringCallbackMechanism();
		//ArborInfo.Initialize();
		//ArborInfo.GetFromArbor(out szOrgId, out szDeviceId, out szAuthSecret);blah
		Task.Run(async () => await Initialize.DiagnosticString($"Ey mon, the OrgId from Arbor is {szOrgId}, DeviceId is {szDeviceId}, AuthSecret is {szAuthSecret}"));
		iXRInit.Start();
        SetConfigValues();
        Authentication.Initialize();
        TrackSystemInfo.Initialize();
#if UNITY_ANDROID
        TrackInputDevices.Initialize();
#endif
    }

    private static void SetConfigValues()
    {
        iXRLib.Configuration.restUrl = Configuration.instance.restUrl;
        iXRLib.Configuration.sendRetriesOnFailure = (uint)Configuration.instance.sendRetriesOnFailure;
        iXRLib.Configuration.sendRetryInterval = TimeSpan.FromSeconds(Configuration.instance.sendRetryIntervalSeconds);
        iXRLib.Configuration.sendNextBatchWait = TimeSpan.FromSeconds(Configuration.instance.sendNextBatchWaitSeconds);
        iXRLib.Configuration.stragglerTimeout = TimeSpan.FromSeconds(Configuration.instance.stragglerTimeoutSeconds);
        iXRLib.Configuration.eventsPerSendAttempt = (uint)Configuration.instance.eventsPerSendAttempt;
        iXRLib.Configuration.logsPerSendAttempt = (uint)Configuration.instance.logsPerSendAttempt;
        iXRLib.Configuration.telemetryEntriesPerSendAttempt = (uint)Configuration.instance.telemetryEntriesPerSendAttempt;
        iXRLib.Configuration.storageEntriesPerSendAttempt = (uint)Configuration.instance.storageEntriesPerSendAttempt;
        iXRLib.Configuration.pruneSentItemsOlderThan = TimeSpan.FromHours(Configuration.instance.pruneSentItemsOlderThanHours);
        iXRLib.Configuration.maximumCachedItems = (uint)Configuration.instance.maximumCachedItems;
        iXRLib.Configuration.retainLocalAfterSent = Configuration.instance.retainLocalAfterSent;
    }

    private static void TestDiagnosticStringCallbackMechanism()
    {
        iXRLibAnalytics.SetDiagnosticStringCallback(DiagnosticString);
        iXRLibAnalytics.TestDiagnosticStringCallbackMechanism();
    }

    private static async Task DiagnosticString(string szString)
    {
        await Task.Run(() => Debug.Log($"iXRLibDebug - {szString}"));
		if (m_pfnSetTextMesh != null)
		{
			m_pfnSetTextMesh(szString);
		}
	}
	public static void SetTextMeshCallback(SetTextMesh pfnSetTextMesh)
	{
		m_pfnSetTextMesh = pfnSetTextMesh;
	}
}
