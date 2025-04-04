using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
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
    public static iXRResult LogDebug(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    return iXRResult.Ok;//return iXRSend.LogDebug(text, meta);
    }
    
    public static iXRResult LogInfo(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    return iXRResult.Ok;//return iXRSend.LogInfo(text, meta);
    }
    
    public static iXRResult LogWarn(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    return iXRResult.Ok;//return iXRSend.LogWarn(text, meta);
    }
    
    public static iXRResult LogError(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    return iXRResult.Ok;//return iXRSend.LogError(text, meta);
    }
    
    public static iXRResult LogCritical(string text, string meta = "")
    {
	    meta = AddSceneData(meta);
	    return iXRResult.Ok;//return iXRSend.LogCritical(text, meta);
    }

    // ---
	public static async Task Event(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		await EventAsync(name, meta);
	}

	public static iXRResult Event(string name, Dictionary<string, string> meta, GameObject gameObject)
	{
		AddSceneData(meta);
		AddPositionData(meta, gameObject);
		return iXRResult.Ok; //Event(name, meta);
	}
	
	public static iXRResult Event(string name, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.Event(name, meta);
	}

	public static iXRResult Event(string name, string meta, GameObject gameObject)
	{
		meta = AddSceneData(meta);
		meta = AddPositionData(meta, gameObject);
		return Event(name, meta);
	}
	// ---
	public static iXRResult TelemetryEntry(string name, Dictionary<string, string> meta)
	{
		AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.AddTelemetryEntry(name, meta);
	}

	public static iXRResult TelemetryEntry(string name, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRLibInterop.AddTelemetryEntry(name, meta);
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
	public static iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventAssessmentStart(assessmentName, meta);
	}
	public static iXRResult EventAssessmentStart(string assessmentName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventAssessmentStart(assessmentName, meta);
	}

	// ---
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRResult.Ok;//return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, meta);
	}
	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		meta = AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRResult.Ok;//return iXRSend.EventAssessmentComplete(assessmentName, score, iXRLibResult, metaString);
	}
	// ---
	public static iXRResult EventObjectiveStart(string objectiveName, Dictionary<string, string> meta = null)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventObjectiveStart(objectiveName, meta);
	}
	public static iXRResult EventObjectiveStart(string objectiveName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventObjectiveStart(objectiveName, meta);
	}

	// ---
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, Dictionary<string, string> meta = null, ResultOptions result = ResultOptions.Complete)
	{
		meta ??= new Dictionary<string, string>();
		AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRResult.Ok;//return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, meta);
	}
	public static iXRResult EventObjectiveComplete(string objectiveName, string score, string meta, ResultOptions result = ResultOptions.Complete)
	{
		meta = AddSceneData(meta);
		// Convert the ResultOptions enum to iXRLib.ResultOptions
		//iXRLib.ResultOptions iXRLibResult = (iXRLib.ResultOptions)result;
		return iXRResult.Ok;//return iXRSend.EventObjectiveComplete(objectiveName, score, iXRLibResult, metaString);
	}
	// ---
	public static iXRResult EventInteractionStart(string interactionName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        return iXRResult.Ok;//return iXRSend.EventInteractionStart(interactionName, meta);
    }
	public static iXRResult EventInteractionStart(string interactionName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventInteractionStart(interactionName, meta);
	}

	// Modified EventInteractionComplete methods.
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
		// Convert the InteractionType enum to iXRLib.InteractionType
        //iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRResult.Ok;//return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
    }
	public static iXRResult EventInteractionComplete(string interactionName, string result, string resultDetails = null, InteractionType eInteractionType = InteractionType.Null, string meta = null)
	{
		meta = AddSceneData(meta);
		// Convert the InteractionType enum to iXRLib.InteractionType
        //iXRLib.InteractionType iXRLibInteractionType = (iXRLib.InteractionType)eInteractionType;
        return iXRResult.Ok;//return iXRSend.EventInteractionComplete(interactionName, result, resultDetails, iXRLibInteractionType, meta);
	}
	// ---
	public static iXRResult EventLevelStart(string levelName, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        return iXRResult.Ok;//return iXRSend.EventLevelStart(levelName, meta);
    }
	public static iXRResult EventLevelStart(string levelName, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventLevelStart(levelName, meta);
	}

	// ---
	public static iXRResult EventLevelComplete(string levelName, string score, Dictionary<string, string> meta = null)
    {
        meta ??= new Dictionary<string, string>();
        AddSceneData(meta);
        return iXRResult.Ok;//return iXRSend.EventLevelComplete(levelName, score, meta);
    }
	public static iXRResult EventLevelComplete(string levelName, string score, string meta)
	{
		meta = AddSceneData(meta);
		return iXRResult.Ok;//return iXRSend.EventLevelComplete(levelName, score, meta);
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
        
	    var wrapper = new PayloadWrapper { data = payloadList };
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

        var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), "/v1/collect/event");
        using var request = new UnityWebRequest(fullUri.ToString(), "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        while (string.IsNullOrEmpty(Authentication.Token)) // TODO don't wait forever
	        await Task.Yield(); // let Unity continue rendering while we wait
        
        request.SetRequestHeader("Authorization", "Bearer " + Authentication.Token);
        
        string unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        request.SetRequestHeader("x-ixrlib-timestamp", unixTimeSeconds);

        uint crc = ComputeCRC(json);
        string hashString = Authentication.Token + Authentication.Secret + unixTimeSeconds + crc;
        request.SetRequestHeader("x-ixrlib-hash", ComputeSha256Hash(hashString));

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield(); // let Unity continue rendering while we wait
            
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("iXRLib - Event successful");
        }
        else
        {
            Debug.LogError($"iXRLib - Event failed : {request.error}");
            Debug.LogError($"xx = {request.result}");
            Debug.LogError($"response = {request.downloadHandler.text}");
        }
    }
	
	public static string ComputeSha256Hash(string rawData)
	{
		using var sha256 = SHA256.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(rawData);
		byte[] hash = sha256.ComputeHash(bytes);
		return Convert.ToBase64String(hash);
	}
	
	static readonly uint[] Table = GenerateTable();

	public static uint ComputeCRC(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		uint crc = 0xFFFFFFFF;

		foreach (byte b in bytes)
		{
			byte index = (byte)((crc ^ b) & 0xFF);
			crc = (crc >> 8) ^ Table[index];
		}

		return ~crc;
	}

	private static uint[] GenerateTable()
	{
		uint[] retTable = new uint[256];
		const uint polynomial = 0xEDB88320;

		for (uint i = 0; i < retTable.Length; i++)
		{
			uint c = i;
			for (int j = 0; j < 8; j++)
				c = (c & 1) != 0 ? (polynomial ^ (c >> 1)) : (c >> 1);
			retTable[i] = c;
		}

		return retTable;
	}
    
    [Serializable]
    public class EventPayload
    {
	    public string preciseTimestamp;
	    public string name;
	    public Dictionary<string, string> meta;
    }
    
    public class PayloadWrapper
    {
	    public List<EventPayload> data;
    }
}