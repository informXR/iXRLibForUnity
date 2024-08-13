using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using UnityEngine;

public class iXRLog
{
    public static iXRResult DebugSynchronous(string bstrText)
	{
		return iXRSend.LogDebugSynchronous(bstrText);
	}
	public static iXRResult Debug(string bstrText)
	{
		return iXRSend.LogDebug(bstrText);
	}
	public static iXRResult InfoSynchronous(string bstrText)
	{
		return iXRSend.LogInfoSynchronous(bstrText);
	}
	public static iXRResult Info(string bstrText)
	{
		return iXRSend.LogInfo(bstrText);
	}
	public static iXRResult WarnSynchronous(string bstrText)
	{
		return iXRSend.LogWarnSynchronous(bstrText);
	}
	public static iXRResult Warn(string bstrText)
	{
		return iXRSend.LogWarn(bstrText);
	}
	public static iXRResult ErrorSynchronous(string bstrText)
	{
		return iXRSend.LogErrorSynchronous(bstrText);
	}
	public static iXRResult Error(string bstrText)
	{
		return iXRSend.LogError(bstrText);
	}
	public static iXRResult CriticalSynchronous(string bstrText)
	{
		return iXRSend.LogCriticalSynchronous(bstrText);
	}
	public static iXRResult Critical(string bstrText)
	{
		return iXRSend.LogCritical(bstrText);
	}
	// ---
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
		meta += $"x={gameObject.transform.position.x}";
		meta += $"y={gameObject.transform.position.y}";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, string meta, GameObject gameObject)
	{
		meta += $"x={gameObject.transform.position.x}";
		meta += $"y={gameObject.transform.position.y}";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.Event(message, meta);
	}
	// ---
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
			result += $"{kvp.Key},{kvp.Value}";
		}

		return result;
	}
}