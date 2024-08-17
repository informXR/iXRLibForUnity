using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using UnityEngine;

public class iXR
{
	// Logging
    public static iXRResult LogDebugSynchronous(string bstrText)
	{
		return iXRSend.LogDebugSynchronous(bstrText);
	}
	public static iXRResult LogDebug(string bstrText)
	{
		return iXRSend.LogDebug(bstrText);
	}
	public static iXRResult LogInfoSynchronous(string bstrText)
	{
		return iXRSend.LogInfoSynchronous(bstrText);
	}
	public static iXRResult LogInfo(string bstrText)
	{
		return iXRSend.LogInfo(bstrText);
	}
	public static iXRResult LogWarnSynchronous(string bstrText)
	{
		return iXRSend.LogWarnSynchronous(bstrText);
	}
	public static iXRResult LogWarn(string bstrText)
	{
		return iXRSend.LogWarn(bstrText);
	}
	public static iXRResult LogErrorSynchronous(string bstrText)
	{
		return iXRSend.LogErrorSynchronous(bstrText);
	}
	public static iXRResult LogError(string bstrText)
	{
		return iXRSend.LogError(bstrText);
	}
	public static iXRResult LogCriticalSynchronous(string bstrText)
	{
		return iXRSend.LogCriticalSynchronous(bstrText);
	}
	public static iXRResult LogCritical(string bstrText)
	{
		return iXRSend.LogCritical(bstrText);
	}
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta)
	{
		return iXRSend.EventSynchronous(name, DictToString(meta));
	}
	public static iXRResult Event(string message, Dictionary<string, string> meta)
	{
		return iXRSend.Event(message, DictToString(meta));
	}
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.EventSynchronous(name, DictToString(meta));
	}
	public static iXRResult Event(string message, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.Event(message, DictToString(meta));
	}
	public static iXRResult EventSynchronous(string name, string meta)
	{
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, string meta)
	{
		return iXRSend.Event(message, meta);
	}
	public static iXRResult EventSynchronous(string name, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.Event(message, meta);
	}
	public static iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> data)
	{
		return iXRSend.AddTelemetryEntrySynchronous(name, DictToString(data));
	}
	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> data)
	{
		return iXRSend.AddTelemetryEntry(name, DictToString(data));
	}
	public static iXRResult TelemetryEntrySynchronous(string name, string data)
	{
		return iXRSend.AddTelemetryEntrySynchronous(name, data);
	}
	public static iXRResult TelemetryEntry(string name, string data)
	{
		return iXRSend.AddTelemetryEntry(name, data);
	}

	private static string DictToString(Dictionary<string, string> dict)
	{
		string result = "";
		foreach (KeyValuePair<string, string> kvp in dict)
		{
			if (!string.IsNullOrEmpty(result)) result += ",";
			result += $"{kvp.Key}={kvp.Value}";
		}

		return result;
	}

	// Storage
	public static string StorageGetDefaultEntry()
	{
		return iXRLibInterop.MarshalString(() => iXRLibInterop.GetDefaultStorageEntryAsString());
	}
	public static string StorageGetEntry(string bstrName)
	{
		return iXRLibInterop.MarshalString(() => iXRLibInterop.GetStorageEntryAsString(bstrName));
	}
	public static iXRResult StorageSetDefaultEntry(string bstrStorageEntry, bool bKeepLatest, string bstrOrigin, bool bSessionData)
	{
		return iXRLibInterop.SetDefaultStorageEntryFromString(bstrStorageEntry, bKeepLatest, bstrOrigin, bSessionData);
	}
	public static iXRResult StorageSetEntry(string bstrName, string bstrStorageEntry, bool bKeepLatest, string bstrOrigin, bool bSessionData)
	{
		return iXRLibInterop.SetStorageEntryFromString(bstrName, bstrStorageEntry, bKeepLatest, bstrOrigin, bSessionData);
	}
	public static iXRResult StorageRemoveDefaultEntry()
	{
		return iXRLibInterop.RemoveDefaultStorageEntry();
	}
	public static iXRResult StorageRemoveEntry(string bstrName)
	{
		return iXRLibInterop.RemoveStorageEntry(bstrName);
	}
	public static iXRResult StorageRemoveMultipleEntries(bool bSessionOnly)
	{
		return iXRLibInterop.RemoveMultipleStorageEntries(bSessionOnly);
	}
	
	// AI
	public static iXRResult AIProxySynchronous(string bstrPrompt, string bstrLMMProvider)
	{
		return iXRLibInterop.AddAIProxySynchronous(bstrPrompt, "", bstrLMMProvider);
	}
	public static iXRResult AIProxySynchronous(string bstrPrompt, string bstrPastMessages, string bstrLMMProvider)
	{
		return iXRLibInterop.AddAIProxySynchronous(bstrPrompt, bstrPastMessages, bstrLMMProvider);
	}
	public static iXRResult AIProxy(string bstrPrompt, string bstrLMMProvider)
	{
		return iXRLibInterop.AddAIProxy(bstrPrompt, "", bstrLMMProvider);
	}
	public static iXRResult IProxy(string bstrPrompt, string bstrPastMessages, string bstrLMMProvider)
	{
		return iXRLibInterop.AddAIProxy(bstrPrompt, bstrPastMessages, bstrLMMProvider);
	}
}