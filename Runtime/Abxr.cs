using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class Abxr
{
	private static readonly Dictionary<string, DateTime> AssessmentStartTimes = new();
	private static readonly Dictionary<string, DateTime> ObjectiveStartTimes = new();
	private static readonly Dictionary<string, DateTime> InteractionStartTimes = new();
	private static readonly Dictionary<string, DateTime> LevelStartTimes = new();
	
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

	public enum StoragePolicy
	{
		keepLatest,
		appendHistory
	}

	public enum StorageScope
	{
		device,
		user
	}

    // Logging
    public static void LogDebug(string text, string meta = "")
    {
	    var metaDict = Utils.StringToDict(meta);
	    AddSceneData(metaDict);
	    LogBatcher.Add("debug", text, metaDict);
    }
    
    public static void LogInfo(string text, string meta = "")
    {
	    var metaDict = Utils.StringToDict(meta);
	    AddSceneData(metaDict);
	    LogBatcher.Add("info", text, metaDict);
    }
    
    public static void LogWarn(string text, string meta = "")
    {
	    var metaDict = Utils.StringToDict(meta);
	    AddSceneData(metaDict);
	    LogBatcher.Add("warn", text, metaDict);
    }
    
    public static void LogError(string text, string meta = "")
    {
	    var metaDict = Utils.StringToDict(meta);
	    AddSceneData(metaDict);
	    LogBatcher.Add("error", text, metaDict);
    }
    
    public static void LogCritical(string text, string meta = "")
    {
	    var metaDict = Utils.StringToDict(meta);
	    AddSceneData(metaDict);
	    LogBatcher.Add("critical", text, metaDict);
    }

    // ---
	public static void Event(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		EventBatcher.Add(name, meta);
	}

	public static void Event(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddPositionData(meta, gameObject);
		Event(name, meta);
	}
	
	public static void Event(string name, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		Event(name, metaDict);
	}

	public static void Event(string name, string meta, GameObject gameObject)
	{
		var metaDict = Utils.StringToDict(meta);
		AddPositionData(metaDict, gameObject);
		Event(name, metaDict);
	}
	// ---
	public static void TelemetryEntry(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		TelemetryBatcher.Add(name, meta);
	}

	public static void TelemetryEntry(string name, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		TelemetryEntry(name, metaDict);
	}

	// Storage
	public static async Task<StorageBatcher.PayloadWrapper> StorageGetDefaultEntry(StorageScope scope = StorageScope.user)
	{
		return await StorageBatcher.Get("state", scope);
	}

	public static async Task<StorageBatcher.PayloadWrapper> StorageGetEntry(string name, StorageScope scope = StorageScope.user)
	{
		return await StorageBatcher.Get(name, scope);
	}

	public static void StorageSetDefaultEntry(Dictionary<string, string> entry, StorageScope scope, StoragePolicy policy = StoragePolicy.keepLatest)
	{
		StorageBatcher.Add("state", entry, scope, policy);
	}
	
	public static void StorageSetEntry(string name, Dictionary<string, string> entry, StorageScope scope, StoragePolicy policy = StoragePolicy.keepLatest)
	{
		StorageBatcher.Add(name, entry, scope, policy);
	}

	public static async Task StorageRemoveDefaultEntry(StorageScope scope = StorageScope.user)
	{
		await StorageBatcher.Delete(scope, "state");
	}

	public static async Task StorageRemoveEntry(string name, StorageScope scope = StorageScope.user)
	{
		await StorageBatcher.Delete(scope, name);
	}

	public static async Task StorageRemoveMultipleEntries(StorageScope scope = StorageScope.user)
	{
		await StorageBatcher.Delete(scope);
	}

	// AI
	public static async Task<string> AIProxy(string prompt, string llmProvider)
	{
		return await AIProxyApi.SendPrompt(prompt, llmProvider);
	}

	public static async Task<string> AIProxy(string prompt, List<string> pastMessages, string llmProvider)
	{
		return await AIProxyApi.SendPrompt(prompt, llmProvider, pastMessages);
	}

	// Event wrapper functions.
	public static void EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		meta["verb"] = "started";
		meta["assessment_name"] = assessmentName;
		AssessmentStartTimes[assessmentName] = DateTime.UtcNow;
		Event("assessment_start", meta);
	}
	public static void EventAssessmentStart(string assessmentName, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		EventAssessmentStart(assessmentName, metaDict);
	}
	
	public static void EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		meta["verb"] = "completed";
		meta["assessment_name"] = assessmentName;
		meta["score"] = score;
		meta["result_options"] = result.ToString();
		AddDuration(AssessmentStartTimes, assessmentName, meta);
		Event("assessment_complete", meta);
	}
	public static void EventAssessmentComplete(string assessmentName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		var metaDict = Utils.StringToDict(meta);
		EventAssessmentComplete(assessmentName, score, metaDict, result);
	}
	
	public static void EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		meta["verb"] = "started";
		meta["objective_name"] = objectiveName;
		ObjectiveStartTimes[objectiveName] = DateTime.UtcNow;
		Event("objective_start", meta);
	}
	public static void EventObjectiveStart(string objectiveName, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		EventObjectiveStart(objectiveName, metaDict);
	}
	
	public static void EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		meta["verb"] = "completed";
		meta["objective_name"] = objectiveName;
		meta["score"] = score;
		meta["result_options"] = result.ToString();
		AddDuration(ObjectiveStartTimes, objectiveName, meta);
		Event("objective_complete", meta);
	}
	public static void EventObjectiveComplete(string objectiveName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		var metaDict = Utils.StringToDict(meta);
		EventObjectiveComplete(objectiveName, score, metaDict, result);
	}
	
	public static void EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        meta["verb"] = "started";
        meta["interaction_name"] = interactionName;
        InteractionStartTimes[interactionName] = DateTime.UtcNow;
        Event("interaction_start", meta);
    }
	public static void EventInteractionStart(string interactionName, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		EventInteractionStart(interactionName, metaDict);
	}
	
	public static void EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType interactionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        meta["verb"] = "completed";
        meta["interaction_name"] = interactionName;
        meta["result"] = result;
        meta["result_details"] = resultDetails;
        meta["lms_type"] = interactionType.ToString();
        AddDuration(InteractionStartTimes, interactionName, meta);
        
        // Add assessment_name if there's only one AssessmentStartTimes value
        if (AssessmentStartTimes.Count == 1)
        {
	        meta["assessment_name"] = AssessmentStartTimes.First().Key;
        }
        
        Event("interaction_complete", meta);
    }
	public static void EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType interactionType = InteractionType.Null, string meta = null)
	{
		var metaDict = Utils.StringToDict(meta);
		EventInteractionComplete(interactionName, result, resultDetails, interactionType, metaDict);
	}
	
	public static void EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        meta["verb"] = "started";
        meta["level_name"] = levelName;
        LevelStartTimes[levelName] = DateTime.UtcNow;
        Event("level_start", meta);
    }
	public static void EventLevelStart(string levelName, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		EventLevelStart(levelName, metaDict);
	}
	
	public static void EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        meta["verb"] = "completed";
        meta["level_name"] = levelName;
        meta["score"] = score;
        AddDuration(LevelStartTimes, levelName, meta);
        Event("level_complete", meta);
    }
	public static void EventLevelComplete(string levelName, string score, string meta)
	{
		var metaDict = Utils.StringToDict(meta);
		EventLevelComplete(levelName, score, metaDict);
	}
	
	private static void AddDuration(Dictionary<string, DateTime> startTimes, string name, Dictionary<string, string> meta)
	{
		if (startTimes.ContainsKey(name))
		{
			double duration = (DateTime.UtcNow - startTimes[name]).TotalSeconds; //TODO do we want seconds?
			meta["duration"] = duration.ToString();
			startTimes.Remove(name);
		}
		else
		{
			meta["duration"] = "0";
		}
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