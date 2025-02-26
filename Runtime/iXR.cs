using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class iXR
{
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

    // Logging
    public static iXRResult LogDebugSynchronous(string text) => iXRSend.LogDebugSynchronous(text);
    public static iXRResult LogDebug(string text) => iXRSend.LogDebug(text);
	public static iXRResult LogInfoSynchronous(string text) => iXRSend.LogInfoSynchronous(text);
	public static iXRResult LogInfo(string text) => iXRSend.LogInfo(text);
	public static iXRResult LogWarnSynchronous(string text) => iXRSend.LogWarnSynchronous(text);
	public static iXRResult LogWarn(string text) => iXRSend.LogWarn(text);
	public static iXRResult LogErrorSynchronous(string text) => iXRSend.LogErrorSynchronous(text);
	public static iXRResult LogError(string text) => iXRSend.LogError(text);
	public static iXRResult LogCriticalSynchronous(string text) => iXRSend.LogCriticalSynchronous(text);
	public static iXRResult LogCritical(string text) => iXRSend.LogCritical(text);

	// ---
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		return iXRSend.EventSynchronous(name, meta);
	}

	public static iXRResult Event(string message, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		return iXRSend.Event(message, meta);
	}

	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddSceneData(meta);
		AddPositionData(meta, gameObject);
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddSceneData(meta);
		AddPositionData(meta, gameObject);
		return iXRSend.Event(message, meta);
	}
	public static iXRResult EventSynchronous(string name, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventSynchronous(name, meta);
	}

	public static iXRResult Event(string message, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.Event(message, meta);
	}

	public static iXRResult EventSynchronous(string name, string meta, GameObject gameObject)
	{
		AddSceneData(meta);
		meta = AddPositionData(meta, gameObject);
		return iXRSend.EventSynchronous(name, meta);
	}
	public static iXRResult Event(string message, string meta, GameObject gameObject)
	{
		meta = AddSceneData(meta);
		meta = AddPositionData(meta, gameObject);
		return iXRSend.Event(message, meta);
	}
	// ---
	public static iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		return iXRSend.AddTelemetryEntrySynchronous(name, meta);
	}

	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		return iXRSend.AddTelemetryEntry(name, meta);
	}

	public static iXRResult TelemetryEntrySynchronous(string name, string meta)
	{
		AddSceneData(meta);
		return iXRLibInterop.AddTelemetryEntrySynchronous(name, meta);
	}

	public static iXRResult TelemetryEntry(string name, string meta)
	{
		AddSceneData(meta);
		return iXRLibInterop.AddTelemetryEntry(name, meta);
	}

	// Storage
	public static string StorageGetDefaultEntry() =>
		iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetDefaultEntryAsString());

	public static string StorageGetEntry(string name) =>
		iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetEntryAsString(name));

	public static iXRResult StorageSetDefaultEntry(string storageEntry, bool keepLatest, string origin, bool sessionData) =>
		iXRLibInterop.StorageSetDefaultEntryFromString(storageEntry, keepLatest, origin, sessionData);

	public static iXRResult StorageSetEntry(string name, string storageEntry, bool keepLatest, string origin, bool sessionData) =>
		iXRLibInterop.StorageSetEntryFromString(name, storageEntry, keepLatest, origin, sessionData);

	public static iXRResult StorageRemoveDefaultEntry() =>
		iXRLibInterop.StorageRemoveDefaultEntry();

	public static iXRResult StorageRemoveEntry(string name) =>
		iXRLibInterop.StorageRemoveEntry(name);

	public static iXRResult StorageRemoveMultipleEntries(bool sessionOnly) =>
		iXRLibInterop.StorageRemoveMultipleEntries(sessionOnly);

	// AI
	public static iXRResult AIProxySynchronous(string prompt, string lMMProvider) =>
		iXRLibInterop.AddAIProxySynchronous(prompt, "", lMMProvider);

	public static iXRResult AIProxySynchronous(string prompt, string pastMessages, string lMMProvider) =>
		iXRLibInterop.AddAIProxySynchronous(prompt, pastMessages, lMMProvider);

	public static iXRResult AIProxy(string prompt, string lMMProvider) =>
		iXRLibInterop.AddAIProxy(prompt, "", lMMProvider);

	public static iXRResult AIProxy(string prompt, string pastMessages, string lMMProvider) =>
		iXRLibInterop.AddAIProxy(prompt, pastMessages, lMMProvider);

	// Event wrapper functions.
	// ---
	public static iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		AddSceneData(meta);
		return iXRSend.EventAssessmentStart(assessmentName, meta);
	}
	public static iXRResult EventAssessmentStart(string assessmentName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventAssessmentStart(assessmentName, meta);
	}

	// ---
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		meta = AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	// ---
	public static iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		AddSceneData(meta);
		return iXRSend.EventObjectiveStart(objectiveName, meta);
	}
	public static iXRResult EventObjectiveStart(string objectiveName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventObjectiveStart(objectiveName, meta);
	}

	// ---
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		meta = AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	// ---
	public static iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        AddSceneData(meta);
		return iXRSend.EventInteractionStart(interactionName, meta);
    }
	public static iXRResult EventInteractionStart(string interactionName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventInteractionStart(interactionName, meta);
	}

	// Modified EventInteractionComplete methods.
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        AddSceneData(meta);
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
    }
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, string meta = null)
	{
		meta = AddSceneData(meta);
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
	}
	// ---
	public static iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
	{
        meta = meta ?? new Dictionary<string, string>();
        AddSceneData(meta);
		return iXRSend.EventLevelStart(levelName, meta);
    }
	public static iXRResult EventLevelStart(string levelName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventLevelStart(levelName, meta);
	}

	// ---
	public static iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        AddSceneData(meta);
		return iXRSend.EventLevelComplete(levelName, score, meta);
    }
	public static iXRResult EventLevelComplete(string levelName, string score, string meta)
	{
		meta = AddSceneData(meta);
		return iXRSend.EventLevelComplete(levelName, score, meta);
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

	private static string AddPositionData(string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return meta;
	}
	
	private static void AddSceneData(Dictionary<string, string> meta)
	{
		meta["sceneName"] = SceneChangeDetector.CurrentSceneName;
	}

	private static string AddSceneData(string meta)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"sceneName={SceneChangeDetector.CurrentSceneName}";
		return meta;
	}
}