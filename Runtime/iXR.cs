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
	public static iXRResult EventSynchronous(string name, Dictionary<string, string> meta) =>
		iXRSend.EventSynchronous(name, meta);
	public static iXRResult Event(string message, Dictionary<string, string> meta) => iXRSend.Event(message, meta);

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
	public static iXRResult EventSynchronous(string name, string meta) => iXRSend.EventSynchronous(name, meta);
	public static iXRResult Event(string message, string meta) => iXRSend.Event(message, meta);

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
	public static iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> data) =>
		iXRSend.AddTelemetryEntrySynchronous(name, data);

	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> data) =>
		iXRSend.AddTelemetryEntry(name, data);

	public static iXRResult TelemetryEntrySynchronous(string name, string data) =>
		iXRLibInterop.AddTelemetryEntrySynchronous(name, data);
	public static iXRResult TelemetryEntry(string name, string data) => iXRLibInterop.AddTelemetryEntry(name, data);

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
		return iXRSend.EventAssessmentStart(assessmentName, meta);
	}
	public static iXRResult EventAssessmentStart(string assessmentName, string metaString) =>
		iXRSend.EventAssessmentStart(assessmentName, metaString);

	// ---
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString, ResultOptions result = ResultOptions.Complete)
	{
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, metaString);
	}
	// ---
	public static iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		return iXRSend.EventObjectiveStart(objectiveName, meta);
	}
	public static iXRResult EventObjectiveStart(string objectiveName, string metaString) =>
		iXRSend.EventObjectiveStart(objectiveName, metaString);

	// ---
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, string metaString, ResultOptions result = ResultOptions.Complete)
	{
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, metaString);
	}
	// ---
	public static iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		return iXRSend.EventInteractionStart(interactionName, meta);
    }
	public static iXRResult EventInteractionStart(string interactionName, string metaString) =>
		iXRSend.EventInteractionStart(interactionName, metaString);

	// Modified EventInteractionComplete methods.
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
    }
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, string metaString = null)
	{
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, metaString);
	}
	// ---
	public static iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		return iXRSend.EventLevelStart(levelName, meta);
    }
	public static iXRResult EventLevelStart(string levelName, string metaString) =>
		iXRSend.EventLevelStart(levelName, metaString);

	// ---
	public static iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		return iXRSend.EventLevelComplete(levelName, score, meta);
    }
	public static iXRResult EventLevelComplete(string levelName, string score, string metaString) =>
		iXRSend.EventLevelComplete(levelName, score, metaString);

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