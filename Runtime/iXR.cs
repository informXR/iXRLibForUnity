using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class iXR
{
	public enum iXRResult
	{
		Ok,                         // Analytics API result: Success.
		NotInitialized,             // Analytics API result: Analytics not initialized.
		AnalyticsDisabled,          // Analytics API result: Analytics is disabled.
		TooManyItems,               // Analytics API result: Too many parameters.
		SizeLimitReached,           // Analytics API result: Argument size limit.
		TooManyRequests,            // Analytics API result: Too many requests.
		InvalidData,                // Analytics API result: Invalid argument value.
		UnsupportedPlatform,        // Analytics API result: This platform doesn't support Analytics.
	}
	
	public enum ResultOptions
	{
		Null,
		Pass,
		Fail,
		Complete,
		Incomplete
	}
	
	public enum InteractionType
	{
		Null,
		Bool,
		Select,
		Text,
		Rating,
		Number
	}

	private static Dictionary<string, string> StringToDict(string input)
	{
		var dict = new Dictionary<string, string>();
		if (string.IsNullOrEmpty(input)) return dict;
		
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
    public static void LogDebug(string text, string meta = "")
    {
	    var metaDict = StringToDict(meta);
	    AddSceneData(metaDict);
	    DataBatcher.AddLog("debug", text, metaDict);
    }
    
    public static void LogInfo(string text, string meta = "")
    {
	    var metaDict = StringToDict(meta);
	    AddSceneData(metaDict);
	    DataBatcher.AddLog("info", text, metaDict);
    }
    
    public static void LogWarn(string text, string meta = "")
    {
	    var metaDict = StringToDict(meta);
	    AddSceneData(metaDict);
	    DataBatcher.AddLog("warn", text, metaDict);
    }
    
    public static void LogError(string text, string meta = "")
    {
	    var metaDict = StringToDict(meta);
	    AddSceneData(metaDict);
	    DataBatcher.AddLog("error", text, metaDict);
    }
    
    public static void LogCritical(string text, string meta = "")
    {
	    var metaDict = StringToDict(meta);
	    AddSceneData(metaDict);
	    DataBatcher.AddLog("critical", text, metaDict);
    }

    // ---
	public static void Event(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		DataBatcher.AddEvent(name, meta);
	}

	public static void Event(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddPositionData(meta, gameObject);
		Event(name, meta);
	}
	
	public static void Event(string name, string meta)
	{
		var metaDict = StringToDict(meta);
		Event(name, metaDict);
	}

	public static void Event(string name, string meta, GameObject gameObject)
	{
		var metaDict = StringToDict(meta);
		AddPositionData(metaDict, gameObject);
		Event(name, metaDict);
	}
	// ---
	public static void TelemetryEntry(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		DataBatcher.AddTelemetry(name, meta);
	}

	public static void TelemetryEntry(string name, string meta)
	{
		var metaDict = StringToDict(meta);
		TelemetryEntry(name, metaDict);
	}

	// Storage
	/*public static string StorageGetDefaultEntry()
	{
		return iXRLibInterop.MarshalString(() => { return iXRLibInterop.StorageGetDefaultEntryAsString(); });
	}

	public static string StorageGetEntry(string name)
	{
		return iXRLibInterop.MarshalString(() => { return iXRLibInterop.StorageGetEntryAsString(name); });
	}

	public static iXRResult StorageSetDefaultEntry(string storageEntry, bool keepLatest, string origin, bool sessionData)
	{
		return iXRLibInterop.StorageSetDefaultEntryFromString(storageEntry, keepLatest, origin, sessionData);
	}

	public static iXRResult StorageSetEntry(string name, string storageEntry, bool keepLatest, string origin, bool sessionData)
	{
		return iXRLibInterop.StorageSetEntryFromString(name, storageEntry, keepLatest, origin, sessionData);
	}

	public static iXRResult StorageRemoveDefaultEntry()
	{
		return iXRLibInterop.StorageRemoveDefaultEntry();
	}

	public static iXRResult StorageRemoveEntry(string name)
	{
		return iXRLibInterop.StorageRemoveEntry(name);
	}

	public static iXRResult StorageRemoveMultipleEntries(bool sessionOnly)
	{
		return iXRLibInterop.StorageRemoveMultipleEntries(sessionOnly);
	}

	// AI
	public static iXRResult AIProxy(string prompt, string llmProvider)
	{
		return iXRLibInterop.AddAIProxy(prompt, "", llmProvider);
	}

	public static iXRResult AIProxy(string prompt, string pastMessages, string llmProvider)
	{
		return iXRLibInterop.AddAIProxy(prompt, pastMessages, llmProvider);
	}*/

	// Event wrapper functions.
	public static async Task EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		//return iXRResult.Ok;//return iXRSend.EventAssessmentStart(assessmentName, meta);
	}
	public static async Task EventAssessmentStart(string assessmentName, string meta)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		//return iXRResult.Ok;//return iXRSend.EventAssessmentStart(assessmentName, meta);
	}

	// ---
	public static async Task EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		//return iXRResult.Ok;//return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	public static async Task EventAssessmentComplete(string assessmentName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		//return iXRResult.Ok;//return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, metaString);
	}
	// ---
	public static async Task EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		//return iXRResult.Ok;//return iXRSend.EventObjectiveStart(objectiveName, meta);
	}
	public static async Task EventObjectiveStart(string objectiveName, string meta)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		//return iXRResult.Ok;//return iXRSend.EventObjectiveStart(objectiveName, meta);
	}

	// ---
	public static async Task EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		//return iXRResult.Ok;//return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	public static async Task EventObjectiveComplete(string objectiveName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		//return iXRResult.Ok;//return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, metaString);
	}
	// ---
	public static async Task EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        //return iXRResult.Ok;//return iXRSend.EventInteractionStart(interactionName, meta);
    }
	public static async Task EventInteractionStart(string interactionName, string meta)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		//return iXRResult.Ok;//return iXRSend.EventInteractionStart(interactionName, meta);
	}

	// Modified EventInteractionComplete methods.
	public static async Task EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
		// Convert the InteractionType enum to iXRLib.InteractionType
        //iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        //return iXRResult.Ok;//return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
    }
	public static async Task EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, string meta = null)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		// Convert the InteractionType enum to iXRLib.InteractionType
        //iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        //return iXRResult.Ok;//return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
	}
	// ---
	public static async Task EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        //return iXRResult.Ok;//return iXRSend.EventLevelStart(levelName, meta);
    }
	public static async Task EventLevelStart(string levelName, string meta)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		//return iXRResult.Ok;//return iXRSend.EventLevelStart(levelName, meta);
	}

	// ---
	public static async Task EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        //return iXRResult.Ok;//return iXRSend.EventLevelComplete(levelName, score, meta);
    }
	public static async Task EventLevelComplete(string levelName, string score, string meta)
	{
		var metaDict = StringToDict(meta);
		AddSceneData(metaDict);
		//return iXRResult.Ok;//return iXRSend.EventLevelComplete(levelName, score, meta);
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
	
	private static void AddPositionData(Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
	}
	
	private static void AddSceneData(Dictionary<string, string> meta)
	{
		meta["sceneName"] = SceneChangeDetector.CurrentSceneName;
	}
}