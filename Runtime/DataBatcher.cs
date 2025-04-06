using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DataBatcher
{
	private const string LogUrlPath = "/v1/collect/log";
	private const string TelemetryUrlPath = "/v1/collect/telemetry";
	private const string EventUrlPath = "/v1/collect/event";
	
	private static readonly List<LogPayload> LogPayloads = new();
	private static readonly List<TelemetryPayload> TelemetryPayloads = new();
	private static readonly List<EventPayload> EventPayloads = new();
	
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

	public static async Task SendLogs()
	{
		var wrapper = new LogPayloadWrapper { data = LogPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), LogUrlPath);
		await SendRequestAsync(fullUri.ToString(), json);
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
	
	public static async Task SendTelemetries()
	{
		var wrapper = new TelemetryPayloadWrapper { data = TelemetryPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), TelemetryUrlPath);
		await SendRequestAsync(fullUri.ToString(), json);
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
	
	public static async Task SendEvents()
	{
		var wrapper = new EventPayloadWrapper { data = EventPayloads };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.Instance.restUrl), EventUrlPath);
		await SendRequestAsync(fullUri.ToString(), json);
	}

	private static async Task SendRequestAsync(string url, string json)
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