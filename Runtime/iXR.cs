using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class iXR
{
    private static Dictionary<string, float> assessmentStartTimes = new Dictionary<string, float>();
    private static Dictionary<string, float> interactionStartTimes = new Dictionary<string, float>();
    private static Dictionary<string, float> levelStartTimes = new Dictionary<string, float>();

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
	// ---
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta)
	{
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, Dictionary<string, string> meta)
	{
		return iXRSend.Event(message, meta);
	}
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.Event(message, meta);
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
	// ---
	public static iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> data)
	{
		return iXRSend.AddTelemetryEntrySynchronous(name, data);
	}
	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> data)
	{
		return iXRSend.AddTelemetryEntry(name, data);
	}
	public static iXRResult TelemetryEntrySynchronous(string name, string data)
	{
		return iXRLibInterop.AddTelemetryEntrySynchronous(name, data);
	}
	public static iXRResult TelemetryEntry(string name, string data)
	{
		return iXRLibInterop.AddTelemetryEntry(name, data);
	}
	// Storage
	public static string StorageGetDefaultEntry()
	{
		return iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetDefaultEntryAsString());
	}
	public static string StorageGetEntry(string bstrName)
	{
		return iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetEntryAsString(bstrName));
	}
	public static iXRResult StorageSetDefaultEntry(string bstrStorageEntry, bool bKeepLatest, string bstrOrigin, bool bSessionData)
	{
		return iXRLibInterop.StorageSetDefaultEntryFromString(bstrStorageEntry, bKeepLatest, bstrOrigin, bSessionData);
	}
	public static iXRResult StorageSetEntry(string bstrName, string bstrStorageEntry, bool bKeepLatest, string bstrOrigin, bool bSessionData)
	{
		return iXRLibInterop.StorageSetEntryFromString(bstrName, bstrStorageEntry, bKeepLatest, bstrOrigin, bSessionData);
	}
	public static iXRResult StorageRemoveDefaultEntry()
	{
		return iXRLibInterop.StorageRemoveDefaultEntry();
	}
	public static iXRResult StorageRemoveEntry(string bstrName)
	{
		return iXRLibInterop.StorageRemoveEntry(bstrName);
	}
	public static iXRResult StorageRemoveMultipleEntries(bool bSessionOnly)
	{
		return iXRLibInterop.StorageRemoveMultipleEntries(bSessionOnly);
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
	public static iXRResult AIProxy(string bstrPrompt, string bstrPastMessages, string bstrLMMProvider)
	{
		return iXRLibInterop.AddAIProxy(bstrPrompt, bstrPastMessages, bstrLMMProvider);
	}

	// Event wrapper functions.

	public static iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventAssessmentStart(assessmentName, meta);
	}

	public static iXRResult EventAssessmentStart(string assessmentName, string metaString)
	{
		return iXRSend.EventAssessmentStart(assessmentName, metaString);
	}

	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventAssessmentComplete(assessmentName, score, meta);
	}

	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString)
	{
		return iXRSend.EventAssessmentComplete(assessmentName, score, metaString);
	}

	public static iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventInteractionStart(interactionName, meta);
    }

	public static iXRResult EventInteractionStart(string interactionName, string metaString)
	{
		return iXRSend.EventInteractionStart(interactionName, metaString);
	}

	// Modified EventInteractionComplete methods.

	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails, LMSType eLmsType = LMSType.Null, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, eLmsType, meta);
    }

	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails, LMSType eLmsType = LMSType.Null, string metaString = null)
	{
		return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, eLmsType, metaString);
	}

	public static iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventLevelStart(levelName, meta);
    }

	public static iXRResult EventLevelStart(string levelName, string metaString)
	{
		return iXRSend.EventLevelStart(levelName, metaString);
	}

	public static iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventLevelComplete(levelName, score, meta);
    }

	public static iXRResult EventLevelComplete(string levelName, string score, string metaString)
	{
		return iXRSend.EventLevelComplete(levelName, score, metaString);
	}

	public static void PresentKeyboard(string promptText = null, string keyboardType = null, string emailDomain = null)
	{
		if (keyboardType is "text" or null)
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Enter Login";
			NonNativeKeyboard.Instance.PresentKeyboard();
		}
		else if (keyboardType == "assessmentPin")
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Enter PIN";
			NonNativeKeyboard.Instance.PresentKeyboard(NonNativeKeyboard.LayoutType.Symbol);
		}
		else if (keyboardType == "email")
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Enter E-Mail";
			NonNativeKeyboard.Instance.EmailDomain.text = emailDomain;
			NonNativeKeyboard.Instance.PresentKeyboard(NonNativeKeyboard.LayoutType.Email);
		}
	}
}