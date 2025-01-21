using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public interface IIxrService
{
    iXRResult LogDebug(string text);
    iXRResult LogDebugSynchronous(string text);
    iXRResult LogInfo(string text);
    iXRResult LogInfoSynchronous(string text);
    iXRResult LogWarn(string text);
    iXRResult LogWarnSynchronous(string text);
    iXRResult LogError(string text);
    iXRResult LogErrorSynchronous(string text);
    iXRResult LogCritical(string text);
    iXRResult LogCriticalSynchronous(string text);
    iXRResult Event(string name, Dictionary<string, string> meta);
    iXRResult EventSynchronous(string name, Dictionary<string, string> meta);
    iXRResult Event(string name, string meta);
    iXRResult EventSynchronous(string name, string meta);
    void PresentKeyboard(string promptText, string keyboardType, string emailDomain);

    iXRResult Event(string name, Dictionary<string, string> meta, GameObject gameObject);
    iXRResult EventSynchronous(string name, Dictionary<string, string> meta, GameObject gameObject);
    iXRResult Event(string name, string meta, GameObject gameObject);
    iXRResult EventSynchronous(string name, string meta, GameObject gameObject);

    iXRResult TelemetryEntry(string name, Dictionary<string, string> data);
    iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> data);
    iXRResult TelemetryEntry(string name, string data);
    iXRResult TelemetryEntrySynchronous(string name, string data);

    string StorageGetDefaultEntry();
    string StorageGetEntry(string name);
    iXRResult StorageSetDefaultEntry(string storageEntry, bool keepLatest, string origin, bool sessionData);
    iXRResult StorageSetEntry(string name, string storageEntry, bool keepLatest, string origin, bool sessionData);
    iXRResult StorageRemoveDefaultEntry();
    iXRResult StorageRemoveEntry(string name);
    iXRResult StorageRemoveMultipleEntries(bool sessionOnly);

    iXRResult AIProxySynchronous(string prompt, string lMMProvider);
    iXRResult AIProxySynchronous(string prompt, string pastMessages, string lMMProvider);
    iXRResult AIProxy(string prompt, string lMMProvider);
    iXRResult AIProxy(string prompt, string pastMessages, string lMMProvider);

    iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null);
    iXRResult EventAssessmentStart(string assessmentName, string metaString);
    iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, iXR.ResultOptions result = iXR.ResultOptions.Complete);
    iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString, iXR.ResultOptions result = iXR.ResultOptions.Complete);
    iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null);
    iXRResult EventObjectiveStart(string objectiveName, string metaString);
    iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, iXR.ResultOptions result = iXR.ResultOptions.Complete);
    iXRResult EventObjectiveComplete(string objectiveName, string score, string metaString, iXR.ResultOptions result = iXR.ResultOptions.Complete);
    iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null);
    iXRResult EventInteractionStart(string interactionName, string metaString);
    iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, iXR.InteractionType eInteractionType = iXR.InteractionType.Null, Dictionary<string, string> meta = null);
    iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, iXR.InteractionType eInteractionType = iXR.InteractionType.Null, string metaString = null);
    iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null);
    iXRResult EventLevelStart(string levelName, string metaString) => iXRSend.EventLevelStart(levelName, metaString);
    iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null);
    iXRResult EventLevelComplete(string levelName, string score, string metaString);

    //void PollUser(string prompt, ExitPollHandler.PollType pollType);
}

public class IxrService : IIxrService
{
    //private readonly IExitPollHandler _exitPollHandler;

    public IxrService()
    {
        //_exitPollHandler = exitPollHandler;
    }
    
    public void PresentKeyboard(string promptText = null, string keyboardType = null, string emailDomain = null)
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
    
    public iXRResult LogDebugSynchronous(string text) => iXRSend.LogDebugSynchronous(text);
    public iXRResult LogDebug(string text) => iXRSend.LogDebug(text);

    public iXRResult LogInfoSynchronous(string text) => iXRSend.LogInfoSynchronous(text);
    public iXRResult LogInfo(string text) => iXRSend.LogInfo(text);

    public iXRResult LogWarnSynchronous(string text) => iXRSend.LogWarnSynchronous(text);
    public iXRResult LogWarn(string text) => iXRSend.LogWarn(text);

    public iXRResult LogErrorSynchronous(string text) => iXRSend.LogErrorSynchronous(text);
    public iXRResult LogError(string text) => iXRSend.LogError(text);

    public iXRResult LogCriticalSynchronous(string text) => iXRSend.LogCriticalSynchronous(text);
    public iXRResult LogCritical(string text) => iXRSend.LogCritical(text);

    // ---
	public iXRResult EventSynchronous(string name, Dictionary<string, string> meta) => iXRSend.EventSynchronous(name, meta);
	public iXRResult Event(string message, Dictionary<string, string> meta) => iXRSend.Event(message, meta);

	public iXRResult EventSynchronous(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.EventSynchronous(name, meta);
	}
	public iXRResult Event(string message, Dictionary<string, string> meta, GameObject gameObject)
	{
		meta["x"] = gameObject.transform.position.x.ToString(CultureInfo.InvariantCulture);
		meta["y"] = gameObject.transform.position.y.ToString(CultureInfo.InvariantCulture);
		meta["z"] = gameObject.transform.position.z.ToString(CultureInfo.InvariantCulture);
		return iXRSend.Event(message, meta);
	}
	public iXRResult EventSynchronous(string name, string meta) => iXRSend.EventSynchronous(name, meta);
	public iXRResult Event(string name, string meta) => iXRSend.Event(name, meta);

	public iXRResult EventSynchronous(string name, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.EventSynchronous(name, meta);
	}
	public iXRResult Event(string name, string meta, GameObject gameObject)
	{
		if (!string.IsNullOrEmpty(meta)) meta += ",";
		meta += $"x={gameObject.transform.position.x},";
		meta += $"y={gameObject.transform.position.y},";
		meta += $"z={gameObject.transform.position.z}";
		return iXRSend.Event(name, meta);
	}
	// ---
	public iXRResult TelemetryEntrySynchronous(string name, Dictionary<string, string> data) =>
		iXRSend.AddTelemetryEntrySynchronous(name, data);
	public iXRResult TelemetryEntry(string name, Dictionary<string, string> data) => iXRSend.AddTelemetryEntry(name, data);

	public iXRResult TelemetryEntrySynchronous(string name, string data) => iXRLibInterop.AddTelemetryEntrySynchronous(name, data);
	public iXRResult TelemetryEntry(string name, string data) => iXRLibInterop.AddTelemetryEntry(name, data);

	// Storage
	public string StorageGetDefaultEntry() => iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetDefaultEntryAsString());

	public string StorageGetEntry(string name) => iXRLibInterop.MarshalString(() => iXRLibInterop.StorageGetEntryAsString(name));

	public iXRResult StorageSetDefaultEntry(string storageEntry, bool keepLatest, string origin, bool sessionData) =>
		iXRLibInterop.StorageSetDefaultEntryFromString(storageEntry, keepLatest, origin, sessionData);

	public iXRResult StorageSetEntry(string name, string storageEntry, bool keepLatest, string origin, bool sessionData) =>
		iXRLibInterop.StorageSetEntryFromString(name, storageEntry, keepLatest, origin, sessionData);

	public iXRResult StorageRemoveDefaultEntry() => iXRLibInterop.StorageRemoveDefaultEntry();

	public iXRResult StorageRemoveEntry(string name) => iXRLibInterop.StorageRemoveEntry(name);

	public iXRResult StorageRemoveMultipleEntries(bool sessionOnly) => iXRLibInterop.StorageRemoveMultipleEntries(sessionOnly);

	// AI
	public iXRResult AIProxySynchronous(string prompt, string lMMProvider) =>
		iXRLibInterop.AddAIProxySynchronous(prompt, "", lMMProvider);

	public iXRResult AIProxySynchronous(string prompt, string pastMessages, string lMMProvider) =>
		iXRLibInterop.AddAIProxySynchronous(prompt, pastMessages, lMMProvider);

	public iXRResult AIProxy(string prompt, string lMMProvider) => iXRLibInterop.AddAIProxy(prompt, "", lMMProvider);

	public iXRResult AIProxy(string prompt, string pastMessages, string lMMProvider) =>
		iXRLibInterop.AddAIProxy(prompt, pastMessages, lMMProvider);

	// Event wrapper functions.
	// ---
	public iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventAssessmentStart(assessmentName, meta);
	}
	public iXRResult EventAssessmentStart(string assessmentName, string metaString) =>
		iXRSend.EventAssessmentStart(assessmentName, metaString);

	// ---
	public iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, iXR.ResultOptions result = iXR.ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	public iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString, iXR.ResultOptions result = iXR.ResultOptions.Complete)
	{
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, metaString);
	}
	// ---
	public iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventObjectiveStart(objectiveName, meta);
	}
	public iXRResult EventObjectiveStart(string objectiveName, string metaString) =>
		iXRSend.EventObjectiveStart(objectiveName, metaString);

	// ---
	public iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, iXR.ResultOptions result = iXR.ResultOptions.Complete)
	{
		meta = meta ?? new Dictionary<string, string>();
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	public iXRResult EventObjectiveComplete(string objectiveName, string score, string metaString, iXR.ResultOptions result = iXR.ResultOptions.Complete)
	{
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, metaString);
	}
	// ---
	public iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventInteractionStart(interactionName, meta);
    }
	public iXRResult EventInteractionStart(string interactionName, string metaString)
	{
		return iXRSend.EventInteractionStart(interactionName, metaString);
	}
	// Modified EventInteractionComplete methods.
	public iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, iXR.InteractionType eInteractionType = iXR.InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
    }
	public iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, iXR.InteractionType eInteractionType = iXR.InteractionType.Null, string metaString = null)
	{
        // Convert the InteractionType enum to iXRLib.InteractionType
        iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, metaString);
	}
	// ---
	public iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventLevelStart(levelName, meta);
    }
	public iXRResult EventLevelStart(string levelName, string metaString) => iXRSend.EventLevelStart(levelName, metaString);

	// ---
	public iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
		// ---
		return iXRSend.EventLevelComplete(levelName, score, meta);
    }
	public iXRResult EventLevelComplete(string levelName, string score, string metaString) =>
		iXRSend.EventLevelComplete(levelName, score, metaString);
}

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
	
    public static void PollUser(string prompt, ExitPollHandler.PollType pollType)
    {
        ExitPollHandler.AddPoll(prompt, pollType);
    }
}

// Keep the original iXR class as a facade for backward compatibility
// public class iXR
// {
//     private static readonly IIxrService Service = new IxrService();
    
//     private static Dictionary<string, float> assessmentStartTimes = new Dictionary<string, float>();
//     private static Dictionary<string, float> interactionStartTimes = new Dictionary<string, float>();
//     private static Dictionary<string, float> levelStartTimes = new Dictionary<string, float>();

//     // Original static methods now delegate to the service
//     public static iXRResult LogDebug(string text) => Service.LogDebug(text);
//     public static iXRResult LogInfo(string text) => Service.LogInfo(text);
//     public static iXRResult LogWarn(string text) => Service.LogWarn(text);
//     public static iXRResult LogError(string text) => Service.LogError(text);
//     public static iXRResult LogCritical(string text) => Service.LogCritical(text);
    
//     public static iXRResult Event(string message, Dictionary<string, string> meta) => Service.Event(message, meta);
//     public static iXRResult Event(string message, string meta) => Service.Event(message, meta);
    
//     public static iXRResult TelemetryEntry(string name, Dictionary<string, string> data) => Service.TelemetryEntry(name, data);
//     public static iXRResult TelemetryEntry(string name, string data) => Service.TelemetryEntry(name, data);

//     // Keep all other original methods, delegating to the service where appropriate
//     // ... (rest of the original code)
// }