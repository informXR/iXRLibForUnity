using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using UnityEngine;

public class iXR
{
    private static Dictionary<string, DateTime> assessmentStartTimes = new Dictionary<string, DateTime>();
    private static Dictionary<string, DateTime> interactionStartTimes = new Dictionary<string, DateTime>();

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
			result += $"{kvp.Key}={kvp.Value}";
		}

		return result;
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

	// Event wrapper functions
	public static iXRResult EventAssessmentStart(string assessmentName, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		meta["verb"] = "started";
		meta["assessment_name"] = assessmentName;
		
		// Store the start time
		assessmentStartTimes[assessmentName] = DateTime.Now;
		
		return Event("assessment_start", meta);
	}

	public static iXRResult EventAssessmentStart(string assessmentName, string metaString)
	{
		var meta = new Dictionary<string, string>
		{
			["verb"] = "started",
			["assessment_name"] = assessmentName
		};
		if (!string.IsNullOrEmpty(metaString))
		{
			foreach (var pair in metaString.Split(','))
			{
				var keyValue = pair.Split('=');
				if (keyValue.Length == 2)
				{
					meta[keyValue[0]] = keyValue[1];
				}
			}
		}
		
		// Store the start time
		assessmentStartTimes[assessmentName] = DateTime.Now;
		
		return Event("assessment_start", meta);
	}

	public static iXRResult EventAssessmentComplete(string assessmentName, string score, Dictionary<string, string> meta = null)
	{
		meta = meta ?? new Dictionary<string, string>();
		meta["verb"] = "completed";
		meta["assessment_name"] = assessmentName;
		meta["score"] = score;
		
		// Calculate and add duration if start time exists, otherwise use "0"
		if (assessmentStartTimes.TryGetValue(assessmentName, out DateTime startTime))
		{
			TimeSpan duration = DateTime.Now - startTime;
			meta["duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
			assessmentStartTimes.Remove(assessmentName);
		}
		else
		{
			meta["duration"] = "0";
		}
		
		return Event("assessment_complete", meta);
	}

	public static iXRResult EventAssessmentComplete(string assessmentName, string score, string metaString)
	{
		var meta = new Dictionary<string, string>
		{
			["verb"] = "completed",
			["assessment_name"] = assessmentName,
			["score"] = score
		};
		
		// Calculate and add duration if start time exists, otherwise use "0"
		if (assessmentStartTimes.TryGetValue(assessmentName, out DateTime startTime))
		{
			TimeSpan duration = DateTime.Now - startTime;
			meta["duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
			assessmentStartTimes.Remove(assessmentName);
		}
		else
		{
			meta["duration"] = "0";
		}
		
		if (!string.IsNullOrEmpty(metaString))
		{
			foreach (var pair in metaString.Split(','))
			{
				var keyValue = pair.Split('=');
				if (keyValue.Length == 2)
				{
					meta[keyValue[0]] = keyValue[1];
				}
			}
		}
		
		return Event("assessment_complete", meta);
	}


    public static iXRResult EventInteractionStart(string interactionId, string interactionName, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        meta["verb"] = "started";
        meta["interaction_id"] = interactionId;
        meta["interaction_name"] = interactionName;
        
        interactionStartTimes[interactionId] = DateTime.Now;
        
        return Event("interaction_start", meta);
    }

    public static iXRResult EventInteractionStart(string interactionId, string interactionName, string metaString)
    {
        var meta = new Dictionary<string, string>
        {
            ["verb"] = "started",
            ["interaction_id"] = interactionId,
            ["interaction_name"] = interactionName
        };
        if (!string.IsNullOrEmpty(metaString))
        {
            foreach (var pair in metaString.Split(','))
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    meta[keyValue[0]] = keyValue[1];
                }
            }
        }
        
        interactionStartTimes[interactionId] = DateTime.Now;

        return Event("interaction_start", meta);
    }

    // Modified EventInteractionComplete methods
    public static iXRResult EventInteractionComplete(string interactionId, string interactionName, string score, Dictionary<string, string> meta = null)
    {
        meta = meta ?? new Dictionary<string, string>();
        meta["verb"] = "completed";
        meta["interaction_id"] = interactionId;
        meta["interaction_name"] = interactionName;
        meta["score"] = score;
        
        if (interactionStartTimes.TryGetValue(interactionId, out DateTime startTime))
        {
            TimeSpan duration = DateTime.Now - startTime;
            meta["duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
            interactionStartTimes.Remove(interactionId);
        }
        else
        {
            meta["duration"] = "0";
        }

        // Add assessment_name if there's only one assessmentStartTimes value
        if (assessmentStartTimes.Count == 1)
        {
            meta["assessment_name"] = assessmentStartTimes.Keys.First();
        }
        
        return Event("interaction_complete", meta);
    }

    public static iXRResult EventInteractionComplete(string interactionId, string interactionName, string score, string metaString)
    {
        var meta = new Dictionary<string, string>
        {
            ["verb"] = "completed",
            ["interaction_id"] = interactionId,
            ["interaction_name"] = interactionName,
            ["score"] = score
        };
        
        if (interactionStartTimes.TryGetValue(interactionId, out DateTime startTime))
        {
            TimeSpan duration = DateTime.Now - startTime;
            meta["duration"] = duration.TotalSeconds.ToString(CultureInfo.InvariantCulture);
            interactionStartTimes.Remove(interactionId);
        }
        else
        {
            meta["duration"] = "0";
        }
        
        if (!string.IsNullOrEmpty(metaString))
        {
            foreach (var pair in metaString.Split(','))
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    meta[keyValue[0]] = keyValue[1];
                }
            }
        }

        // Add assessment_name if there's only one assessmentStartTimes value
        if (assessmentStartTimes.Count == 1)
        {
            meta["assessment_name"] = assessmentStartTimes.Keys.First();
        }
        
        return Event("interaction_complete", meta);
    }
}