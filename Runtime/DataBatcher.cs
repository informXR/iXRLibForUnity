using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DataBatcher : MonoBehaviour
{
	private const float SendIntervalSeconds = 10f;
	
	private const string LogUrlPath = "/v1/collect/log";
	private const string TelemetryUrlPath = "/v1/collect/telemetry";
	private const string EventUrlPath = "/v1/collect/event";
	
	private static readonly List<LogPayload> LogPayloads = new();
	private static readonly List<TelemetryPayload> TelemetryPayloads = new();
	private static readonly List<EventPayload> EventPayloads = new();
	
	private void Start()
	{
		StartCoroutine(SendLoop());
	}

	public static void SendNow(MonoBehaviour context)
	{
		context.StartCoroutine(Send());
	}

	private static IEnumerator SendLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(SendIntervalSeconds);
			yield return Send();
		}
	}

	private static IEnumerator Send()
	{
		if (LogPayloads.Count > 0)
		{
			yield return SendLogs();
		}

		if (TelemetryPayloads.Count > 0)
		{
			yield return SendTelemetries();
		}

		if (EventPayloads.Count > 0)
		{
			yield return SendEvents();
		}
	}
	
    public static void AddLog(string logLevel, string text, Dictionary<string, string> meta)
    {
	    long logTime = GetEventTime();
		var payload = new LogPayload
		{
			preciseTimestamp = logTime.ToString(),
			logLevel = logLevel,
			text = text,
			meta = meta
		};
		
		LogPayloads.Add(payload);
	}

	private static IEnumerator SendLogs()
	{
		var wrapper = new LogPayloadWrapper { data = LogPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), LogUrlPath);
		yield return SendRequest(fullUri.ToString(), json);
	}
	
	public static void AddTelemetry(string name, Dictionary<string, string> meta)
	{
		long telemetryTime = GetEventTime();
		var payload = new TelemetryPayload
		{
			preciseTimestamp = telemetryTime.ToString(),
			name = name,
			meta = meta
		};
		
		TelemetryPayloads.Add(payload);
	}
	
	private static IEnumerator SendTelemetries()
	{
		var wrapper = new TelemetryPayloadWrapper { data = TelemetryPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), TelemetryUrlPath);
		yield return SendRequest(fullUri.ToString(), json);
	}
	
	public static void AddEvent(string name, Dictionary<string, string> meta)
    {
	    long eventTime = GetEventTime();
	    var payload = new EventPayload
	    {
		    preciseTimestamp = eventTime.ToString(),
		    name = name,
		    meta = meta
	    };
	    
	    EventPayloads.Add(payload);
    }
	
	private static IEnumerator SendEvents()
	{
		var wrapper = new EventPayloadWrapper { data = EventPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), EventUrlPath);
		yield return SendRequest(fullUri.ToString(), json);
	}

	private static IEnumerator SendRequest(string url, string json)
	{
		using var request = new UnityWebRequest(url, "POST");
		BuildRequest(request, json);
		
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("iXRLib - Request successful");
			if (url.Contains("log")) LogPayloads.Clear();
			if (url.Contains("telemetry")) TelemetryPayloads.Clear();
			if (url.Contains("event")) EventPayloads.Clear();
			//TODO need to lock the list while I'm doing this
		}
		else
		{
			Debug.LogError($"iXRLib - Request failed : {request.error}");
		}
	}

	private static void BuildRequest(UnityWebRequest request, string json)
	{
		byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
        
		request.SetRequestHeader("Authorization", "Bearer " + Authentication.Token);
        
		string unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
		request.SetRequestHeader("x-ixrlib-timestamp", unixTimeSeconds);

		uint crc = Utils.ComputeCRC(json);
		string hashString = Authentication.Token + Authentication.Secret + unixTimeSeconds + crc;
		request.SetRequestHeader("x-ixrlib-hash", Utils.ComputeSha256Hash(hashString));
	}

	private static long GetEventTime() => (long)(Time.time * 1000f) + Initialize.StartTimeMs;

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