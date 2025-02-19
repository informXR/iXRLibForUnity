using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Newtonsoft.Json;
using UnityEngine;

public class iXR
{
#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern void JSLogDebug(string text, string meta);
	[DllImport("__Internal")]
	private static extern void JSLogInfo(string text, string meta);
	[DllImport("__Internal")]
	private static extern void JSLogWarn(string text, string meta);
	[DllImport("__Internal")]
	private static extern void JSLogError(string text, string meta);
	[DllImport("__Internal")]
	private static extern void JSLogCritical(string text, string meta);
	[DllImport("__Internal")]
	private static extern void JSEvent(string name, string meta);
	[DllImport("__Internal")]
	private static extern void JSAddTelemetryEntry(string name, string meta);
	[DllImport("__Internal")]
	private static extern void JSStorageGetDefaultEntry();
	[DllImport("__Internal")]
	private static extern void JSStorageGetEntry(string name);
	[DllImport("__Internal")]
	private static extern void JSStorageSetDefaultEntry(string entry, bool keepLatest, string origin, bool sessionData);
	[DllImport("__Internal")]
	private static extern void JSStorageSetEntry(string name, string entry, bool keepLatest, string origin, bool sessionData);
	[DllImport("__Internal")]
	private static extern void JSStorageRemoveDefaultEntry();
	[DllImport("__Internal")]
	private static extern void JSStorageRemoveEntry(string name);
	[DllImport("__Internal")]
	private static extern void JSStorageRemoveMultipleEntries(bool sessionOnly);
	[DllImport("__Internal")]
	private static extern void JSAddAIProxy0(string prompt, string llmProvider);
	[DllImport("__Internal")]
	private static extern void JSAddAIProxy1(string prompt, string pastMessages, string llmProvider);
	[DllImport("__Internal")]
	private static extern void JSEventAssessmentStart(string assessmentName, string meta);
	[DllImport("__Internal")]
	private static extern void JSEventAssessmentComplete(string assessmentName, string score, string meta, int resultOption);
	[DllImport("__Internal")]
	private static extern void JSEventObjectiveStart(string objectiveName, string meta);
	[DllImport("__Internal")]
	private static extern void JSEventObjectiveComplete(string objectiveName, string score, string meta, int resultOption);
	[DllImport("__Internal")]
	private static extern void JSEventInteractionStart(string interactionName, string meta);
	[DllImport("__Internal")]
	private static extern void JSEventInteractionComplete(string interactionName, string result, string resultDetails, int interactionType, string dictMeta);
	[DllImport("__Internal")]
	private static extern void JSEventLevelStart(string levelName, string meta);
	[DllImport("__Internal")]
	private static extern void JSEventLevelComplete(string levelName, string score, string meta);
#endif
	
	// Alias the ResultOptions enum from iXRLib.iXRLibInterop
	public enum ResultOptions
	{
		//Null = iXRLib.ResultOptions.Null,
		Pass = iXRLib.ResultOptions.Pass,
		Fail = iXRLib.ResultOptions.Fail,
		Complete = iXRLib.ResultOptions.Complete,
		Incomplete = iXRLib.ResultOptions.Incomplete
	}
	// Alias the InteractionType enum from iXRLib.iXRLibInterop
	public enum InteractionType
	{
		Null = iXRLib.InteractionType.Null,
		Bool = iXRLib.InteractionType.Bool,
		Select = iXRLib.InteractionType.Select,
		Text = iXRLib.InteractionType.Text,
		Rating = iXRLib.InteractionType.Rating,
		Number = iXRLib.InteractionType.Number
	}

	private static Dictionary<string, string> StringToDict(string input)
	{
		var dict = new Dictionary<string, string>();
		var pairs = input.Split(',');
		foreach (var pair in pairs)
		{
			var parts = pair.Split('=');
			string key = parts[0].Trim();
			string value = parts[1].Trim();
			dict[key] = value;
		}
		return dict;
	}

    // Logging
    public static iXRResult LogDebug(string text, string meta = "")
    {
#if UNITY_WEBGL
	    Dictionary<string, string> metaDict = StringToDict(meta);
	    string metaString = JsonConvert.SerializeObject(metaDict);
	    JSLogDebug(text, metaString);
	    return iXRResult.Ok;
#else
	    return iXRSend.LogDebug(text, meta);
#endif
    }

    public static iXRResult LogDebugAsync(string text, string meta = "")
    {
#if UNITY_WEBGL
	    return iXRResult.EventNotEnabled;
#else
	    return iXRSend.LogDebugDeferred(text, meta);
#endif
    }
    
    public static iXRResult LogInfo(string text, string meta = "")
    {
#if UNITY_WEBGL
	    Dictionary<string, string> metaDict = StringToDict(meta);
	    string metaString = JsonConvert.SerializeObject(metaDict);
	    JSLogInfo(text, metaString);
	    return iXRResult.Ok;
#else
	    return iXRSend.LogInfo(text, meta);
#endif
    }

    public static iXRResult LogInfoAsync(string text, string meta = "")
    {
#if UNITY_WEBGL
	    return iXRResult.EventNotEnabled;
#else
	    return iXRSend.LogInfoDeferred(text, meta);
#endif
    }
    
    public static iXRResult LogWarn(string text, string meta = "")
    {
#if UNITY_WEBGL
	    Dictionary<string, string> metaDict = StringToDict(meta);
	    string metaString = JsonConvert.SerializeObject(metaDict);
	    JSLogWarn(text, metaString);
	    return iXRResult.Ok;
#else
	    return iXRSend.LogWarn(text, meta);
#endif
    }

    public static iXRResult LogWarnAsync(string text, string meta = "")
    {
#if UNITY_WEBGL
	    return iXRResult.EventNotEnabled;
#else
	    return iXRSend.LogWarnDeferred(text, meta);
#endif
    }
    
    public static iXRResult LogError(string text, string meta = "")
    {
#if UNITY_WEBGL
	    Dictionary<string, string> metaDict = StringToDict(meta);
	    string metaString = JsonConvert.SerializeObject(metaDict);
	    JSLogError(text, metaString);
	    return iXRResult.Ok;
#else
	    return iXRSend.LogError(text, meta);
#endif
    }

    public static iXRResult LogErrorAsync(string text, string meta = "")
    {
#if UNITY_WEBGL
	    return iXRResult.EventNotEnabled;
#else
	    return iXRSend.LogErrorDeferred(text, meta);
#endif
    }
    
    public static iXRResult LogCritical(string text, string meta = "")
    {
#if UNITY_WEBGL
	    Dictionary<string, string> metaDict = StringToDict(meta);
	    string metaString = JsonConvert.SerializeObject(metaDict);
	    JSLogCritical(text, metaString);
	    return iXRResult.Ok;
#else
	    return iXRSend.LogCritical(text, meta);
#endif
    }

    public static iXRResult LogCriticalAsync(string text, string meta = "")
    {
#if UNITY_WEBGL
	    return iXRResult.EventNotEnabled;
#else
	    return iXRSend.LogCriticalDeferred(text, meta);
#endif
    }

    // ---
	public static iXRResult Event(string name, Dictionary<string, string> meta)
	{
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSEvent(name, metaString);
		return iXRResult.EventNotEnabled;
#else
	    return iXRSend.Event(name, meta);
#endif
	}

	public static iXRResult EventAsync(string message, Dictionary<string, string> meta)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
	    return iXRSend.EventDeferred(message, meta);
#endif
	}

	public static iXRResult Event(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return Event(name, meta);
	}
	public static iXRResult EventAsync(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return EventAsync(name, meta);
	}
	public static iXRResult Event(string name, string meta)
	{
#if UNITY_WEBGL
		JSEvent(name, meta);
		return iXRResult.Ok;
#else
		return iXRSend.Event(name, meta);
#endif
	}

	public static iXRResult EventAsync(string name, string meta)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
	    return iXRSend.EventDeferred(name, meta);
#endif
	}

	public static iXRResult Event(string name, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return Event(name, meta);
	}
	public static iXRResult EventAsync(string name, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return EventAsync(name, meta);
	}
	// ---
	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> meta)
	{
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSAddTelemetryEntry(name, metaString);
		return iXRResult.Ok;
#else
		return iXRSend.AddTelemetryEntry(name, meta);
#endif
	}

	public static iXRResult TelemetryEntryAsync(string name, Dictionary<string, string> meta)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
		return iXRSend.AddTelemetryEntryDeferred(name, meta);
#endif
	}

	public static iXRResult TelemetryEntry(string name, string meta)
	{
#if UNITY_WEBGL
		JSAddTelemetryEntry(name, meta);
		return iXRResult.Ok;
#else
		return iXRLibInterop.AddTelemetryEntry(name, meta);
#endif
	}

	public static iXRResult TelemetryEntryAsync(string name, string meta)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
		return iXRLibInterop.AddTelemetryEntryDeferred(name, meta);
#endif
	}

	// Storage
	public static string StorageGetDefaultEntry()
	{
#if UNITY_WEBGL
		return "";
#else
		return iXRLibInterop.MarshalString(() => { return iXRLibInterop.StorageGetDefaultEntryAsString(); });
#endif
	}

	public static string StorageGetEntry(string name)
	{
#if UNITY_WEBGL
		return "";
#else
		return iXRLibInterop.MarshalString(() => { return iXRLibInterop.StorageGetEntryAsString(name); });
#endif
	}

	public static iXRResult StorageSetDefaultEntry(string storageEntry, bool keepLatest, string origin, bool sessionData)
	{
#if UNITY_WEBGL
		JSStorageSetDefaultEntry(storageEntry, keepLatest, origin, sessionData);
		return iXRResult.Ok;
#else
		return iXRLibInterop.StorageSetDefaultEntryFromString(storageEntry, keepLatest, origin, sessionData);
#endif
	}

	public static iXRResult StorageSetEntry(string name, string storageEntry, bool keepLatest, string origin, bool sessionData)
	{
#if UNITY_WEBGL
		JSStorageSetEntry(name, storageEntry, keepLatest, origin, sessionData);
		return iXRResult.Ok;
#else
		return iXRLibInterop.StorageSetEntryFromString(name, storageEntry, keepLatest, origin, sessionData);
#endif
	}

	public static iXRResult StorageRemoveDefaultEntry()
	{
#if UNITY_WEBGL
		JSStorageRemoveDefaultEntry();
		return iXRResult.Ok;
#else
		return iXRLibInterop.StorageRemoveDefaultEntry();
#endif
	}

	public static iXRResult StorageRemoveEntry(string name)
	{
#if UNITY_WEBGL
		JSStorageRemoveEntry(name);
		return iXRResult.Ok;
#else
		return iXRLibInterop.StorageRemoveEntry(name);
#endif
	}

	public static iXRResult StorageRemoveMultipleEntries(bool sessionOnly)
	{
#if UNITY_WEBGL
		JSStorageRemoveMultipleEntries(sessionOnly);
		return iXRResult.Ok;
#else
		return iXRLibInterop.StorageRemoveMultipleEntries(sessionOnly);
#endif
	}

	// AI
	public static iXRResult AIProxy(string prompt, string llmProvider)
	{
#if UNITY_WEBGL
		JSAddAIProxy0(prompt, llmProvider);
		return iXRResult.Ok;
#else
		return iXRLibInterop.AddAIProxy(prompt, "", llmProvider);
#endif
	}

	public static iXRResult AIProxy(string prompt, string pastMessages, string llmProvider)
	{
#if UNITY_WEBGL
		JSAddAIProxy1(prompt, pastMessages, llmProvider);
		return iXRResult.Ok;
#else
		return iXRLibInterop.AddAIProxy(prompt, pastMessages, llmProvider);
#endif
	}

	public static iXRResult AIProxyAsync(string prompt, string llmProvider)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
		return iXRLibInterop.AddAIProxyDeferred(prompt, "", llmProvider);
#endif
	}

	public static iXRResult AIProxyAsync(string prompt, string pastMessages, string llmProvider)
	{
#if UNITY_WEBGL
		return iXRResult.EventNotEnabled;
#else
		return iXRLibInterop.AddAIProxyDeferred(prompt, pastMessages, llmProvider);
#endif
	}

	// Event wrapper functions.
	// ---
	public static iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSEventAssessmentStart(assessmentName, metaString);
		return iXRResult.Ok;
#else
		return iXRSend.EventAssessmentStart(assessmentName, meta);
#endif
	}
	public static iXRResult EventAssessmentStart(string assessmentName, string meta)
	{
#if UNITY_WEBGL
		JSEventAssessmentStart(assessmentName, meta);
		return iXRResult.Ok;
#else
		return iXRSend.EventAssessmentStart(assessmentName, meta);
#endif
	}

	// ---
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSEventAssessmentComplete(assessmentName, score, metaString, (int)result);
		return iXRResult.Ok;
#else
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
#endif
	}
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString, ResultOptions result = ResultOptions.Complete)
	{
#if UNITY_WEBGL
		JSEventAssessmentComplete(assessmentName, score, metaString, (int)result);
		return iXRResult.Ok;
#else
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, metaString);
#endif
	}
	// ---
	public static iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSEventObjectiveStart(objectiveName, metaString);
		return iXRResult.Ok;
#else
		return iXRSend.EventObjectiveStart(objectiveName, meta);
#endif
	}
	public static iXRResult EventObjectiveStart(string objectiveName, string meta)
	{
#if UNITY_WEBGL
		JSEventObjectiveStart(objectiveName, meta);
		return iXRResult.Ok;
#else
		return iXRSend.EventObjectiveStart(objectiveName, meta);
#endif
	}

	// ---
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
		string metaString = JsonConvert.SerializeObject(meta);
		JSEventObjectiveComplete(objectiveName, score, metaString, (int)result);
		return iXRResult.Ok;
#else
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
#endif
	}
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, string metaString, ResultOptions result = ResultOptions.Complete)
	{
#if UNITY_WEBGL
		JSEventObjectiveComplete(objectiveName, score, metaString, (int)result);
		return iXRResult.Ok;
#else
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, metaString);
#endif
	}
	// ---
	public static iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
	    string metaString = JsonConvert.SerializeObject(meta);
	    JSEventInteractionStart(interactionName, metaString);
	    return iXRResult.Ok;
#else
		return iXRSend.EventInteractionStart(interactionName, meta);
#endif
    }
	public static iXRResult EventInteractionStart(string interactionName, string meta)
	{
#if UNITY_WEBGL
		JSEventInteractionStart(interactionName, meta);
		return iXRResult.Ok;
#else
		return iXRSend.EventInteractionStart(interactionName, meta);
#endif
	}

	// Modified EventInteractionComplete methods.
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
	    string metaString = JsonConvert.SerializeObject(meta);
	    JSEventInteractionComplete(interactionName, result, resultDetails, (int)eInteractionType, metaString);
	    return iXRResult.Ok;
#else
		// Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
#endif
    }
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, string meta = null)
	{
#if UNITY_WEBGL
		JSEventInteractionComplete(interactionName, result, resultDetails, (int)eInteractionType, meta);
		return iXRResult.Ok;
#else
		// Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
#endif
	}
	// ---
	public static iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
	    string metaString = JsonConvert.SerializeObject(meta);
	    JSEventLevelStart(levelName, metaString);
	    return iXRResult.Ok;
#else
		return iXRSend.EventLevelStart(levelName, meta);
#endif
    }
	public static iXRResult EventLevelStart(string levelName, string meta)
	{
#if UNITY_WEBGL
		JSEventLevelStart(levelName, meta);
		return iXRResult.Ok;
#else
		return iXRSend.EventLevelStart(levelName, meta);
#endif
	}

	// ---
	public static iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
#if UNITY_WEBGL
	    string metaString = JsonConvert.SerializeObject(meta);
	    JSEventLevelComplete(levelName, score, metaString);
	    return iXRResult.Ok;
#else
		return iXRSend.EventLevelComplete(levelName, score, meta);
#endif
    }
	public static iXRResult EventLevelComplete(string levelName, string score, string meta)
	{
#if UNITY_WEBGL
		JSEventLevelComplete(levelName, score, meta);
		return iXRResult.Ok;
#else
		return iXRSend.EventLevelComplete(levelName, score, meta);
#endif
	}

	// ---
	public static void PresentKeyboard(string promptText = null, string keyboardType = null, string emailDomain = null)
	{
		KeyboardHandler.ProcessingSubmit = false;
		if (keyboardType is "text" or null)
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Please Enter Your Login";
			NonNativeKeyboard.Instance.PresentKeyboard();
		}
		else if (keyboardType == "assessmentPin")
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Enter your 6-digit PIN";
			NonNativeKeyboard.Instance.PresentKeyboard(NonNativeKeyboard.LayoutType.Symbol);
		}
		else if (keyboardType == "email")
		{
			NonNativeKeyboard.Instance.Prompt.text = promptText ?? "Enter your email";
			NonNativeKeyboard.Instance.EmailDomain.text = $"@{emailDomain}";
			NonNativeKeyboard.Instance.PresentKeyboard(NonNativeKeyboard.LayoutType.Email);
		}
	}

	public static void PollUser(string prompt, ExitPollHandler.PollType pollType)
	{
		ExitPollHandler.AddPoll(prompt, pollType);
	}
}