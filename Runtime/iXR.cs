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
    public static async Task LogDebug(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    var metaDict = StringToDict(meta);
	    await LogAsync("debug", text, metaDict);
    }
    
    public static async Task LogInfo(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    var metaDict = StringToDict(meta);
	    await LogAsync("info", text, metaDict);
    }
    
    public static async Task LogWarn(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    var metaDict = StringToDict(meta);
	    await LogAsync("warn", text, metaDict);
    }
    
    public static async Task LogError(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    var metaDict = StringToDict(meta);
	    await LogAsync("error", text, metaDict);
    }
    
    public static async Task LogCritical(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    var metaDict = StringToDict(meta);
	    await LogAsync("critical", text, metaDict);
    }

    // ---
	public static async Task Event(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		await EventAsync(name, meta);
	}

	public static async Task Event(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddSceneData(meta);
		AddPositionData(meta, gameObject);
		await Event(name, meta);
	}
	
	public static async Task Event(string name, string meta)
	{
		meta = AddSceneData(meta);
		var metaDict = StringToDict(meta);
		await Event(name, metaDict);
	}

	public static async Task Event(string name, string meta, GameObject gameObject)
	{
		meta = AddSceneData(meta);
		meta = AddPositionData(meta, gameObject);
		var metaDict = StringToDict(meta);
		await Event(name, metaDict);
	}
	// ---
	public static async Task TelemetryEntry(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		await TelemetryAsync(name, meta);
	}

	public static async Task TelemetryEntry(string name, string meta)
	{
		meta = AddSceneData(meta);
		var metaDict = StringToDict(meta);
		await TelemetryAsync(name, metaDict);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
		meta = AddSceneData(meta);
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
	
	private static async Task LogAsync(string logLevel, string text, Dictionary<string, string> meta)
	{
		long logTime = (long)(Time.time * 1000f) + Initialize.StartTimeMs;
	    
		var payloadList = new List<LogPayload>
		{
			new LogPayload
			{
				preciseTimestamp = logTime.ToString(),
				logLevel = logLevel,
				text = text,
				meta = meta
			}
		};
        
		var wrapper = new LogPayloadWrapper { data = payloadList };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), "/v1/collect/log");
		await RequestAsync(fullUri.ToString(), json);
	}
	
	private static async Task TelemetryAsync(string name, Dictionary<string, string> meta)
	{
		long telemetryTime = (long)(Time.time * 1000f) + Initialize.StartTimeMs;
	    
		var payloadList = new List<TelemetryPayload>
		{
			new TelemetryPayload
			{
				preciseTimestamp = telemetryTime.ToString(),
				name = name,
				meta = meta
			}
		};
        
		var wrapper = new TelemetryPayloadWrapper { data = payloadList };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), "/v1/collect/telemetry");
		await RequestAsync(fullUri.ToString(), json);
	}
	
	private static async Task EventAsync(string name, Dictionary<string, string> meta)
    {
	    long eventTime = (long)(Time.time * 1000f) + Initialize.StartTimeMs;
	    
	    var payloadList = new List<EventPayload>
	    {
		    new EventPayload
		    {
			    preciseTimestamp = eventTime.ToString(),
			    name = name,
			    meta = meta
		    }
	    };
        
	    var wrapper = new EventPayloadWrapper { data = payloadList };
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

        var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), "/v1/collect/event");
        await RequestAsync(fullUri.ToString(), json);
    }

	private static async Task RequestAsync(string url, string json)
	{
		using var request = new UnityWebRequest(url, "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
        
		while (string.IsNullOrEmpty(Authentication.Token)) // TODO don't wait forever
			await Task.Yield(); // let Unity continue rendering while we wait
        
		request.SetRequestHeader("Authorization", "Bearer " + Authentication.Token);
        
		string unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
		request.SetRequestHeader("x-ixrlib-timestamp", unixTimeSeconds);

		uint crc = Utils.ComputeCRC(json);
		string hashString = Authentication.Token + Authentication.Secret + unixTimeSeconds + crc;
		request.SetRequestHeader("x-ixrlib-hash", Utils.ComputeSha256Hash(hashString));

		var operation = request.SendWebRequest();

		while (!operation.isDone) await Task.Yield(); // let Unity continue rendering while we wait
            
		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("iXRLib - Request successful");
		}
		else
		{
			Debug.LogError($"iXRLib - Request failed : {request.error}");
		}
	}

	private class EventPayload
    {
	    public string preciseTimestamp;
	    public string name;
	    public Dictionary<string, string> meta;
    }
	private class EventPayloadWrapper
    {
	    public List<EventPayload> data;
    }
	
	private class TelemetryPayload
	{
		public string preciseTimestamp;
		public string name;
		public Dictionary<string, string> meta;
	}
	private class TelemetryPayloadWrapper
	{
		public List<TelemetryPayload> data;
	}
	
	private class LogPayload
	{
		public string preciseTimestamp;
		public string logLevel;
		public string text;
		public Dictionary<string, string> meta;
	}
	private class LogPayloadWrapper
	{
		public List<LogPayload> data;
	}
}