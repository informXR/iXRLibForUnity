using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class EventBatcher : MonoBehaviour
{
	private static float _sendInterval = 10f;
	private const string UrlPath = "/v1/collect/event";
	private static readonly List<Payload> Payloads = new();
	private static readonly object Lock = new();
	
	private void Start()
	{
		_sendInterval = Configuration.instance.sendNextBatchWaitSeconds;
		StartCoroutine(SendLoop());
	}

	public static void SendNow()
	{
		CoroutineRunner.Instance.StartCoroutine(Send());
	}

	private static IEnumerator SendLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(_sendInterval);
			yield return Send();
		}
	}

	private static IEnumerator Send()
	{
		_sendInterval = Configuration.instance.sendNextBatchWaitSeconds;
		lock (Lock)
		{
			if (Payloads.Count > 0) yield return SendEvents();
		}
	}
	
	public static void Add(string name, Dictionary<string, string> meta)
    {
	    long eventTime = Utils.GetUnityTime();
	    var payload = new Payload
	    {
		    preciseTimestamp = eventTime.ToString(),
		    name = name,
		    meta = meta
	    };
	    
	    lock (Lock) Payloads.Add(payload);
    }
	
	private static IEnumerator SendEvents()
	{
		List<Payload> eventsToSend;
		lock (Lock)
        {
            // Copy current list and leave original untouched
            eventsToSend = new List<Payload>(Payloads);
            foreach (var evt in eventsToSend) Payloads.Remove(evt);
        }
				
		var wrapper = new PayloadWrapper { data = eventsToSend };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.instance.restUrl), UrlPath);
		using var request = new UnityWebRequest(fullUri, "POST");
		Utils.BuildRequest(request, json);
		Authentication.SetAuthHeaders(request, json);
		
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("AbxrLib - Event POST Request successful");
		}
		else
		{
			Debug.LogError($"AbxrLib - Event POST Request failed : {request.error} - {request.downloadHandler.text}");
			_sendInterval = Configuration.instance.sendRetryIntervalSeconds;
			lock (Lock)
			{
				Payloads.InsertRange(0, eventsToSend);
			}
		}
	}

	private class Payload
    {
	    public string preciseTimestamp;
	    public string name;
	    public Dictionary<string, string> meta;
    }
	private class PayloadWrapper
    {
	    public List<Payload> data;
    }
}