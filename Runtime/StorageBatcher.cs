using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class StorageBatcher : MonoBehaviour
{
	private static float _sendInterval = 10f;
	private const string UrlPath = "/v1/storage";
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
			if (Payloads.Count > 0) yield return SendStorages();
		}
	}
	
	public static void Add(string name, Dictionary<string, string> entry, iXR.StorageScope scope, iXR.StoragePolicy policy)
	{
		long storageTime = Utils.GetUnityTime();
		string isoTime = DateTimeOffset.FromUnixTimeMilliseconds(storageTime).UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		var payload = new Payload
		{
			timestamp = isoTime,
			keepPolicy = policy.ToString(),
			name = name,
			data = new List<Dictionary<string, string>>
			{
				entry
			},
			scope = scope.ToString()
		};
		
		lock (Lock) Payloads.Add(payload);
	}

	private static IEnumerator SendStorages()
	{
		List<Payload> storagesToSend;
		lock (Lock)
		{
			// Copy current list and leave original untouched
			storagesToSend = new List<Payload>(Payloads);
			foreach (var storage in storagesToSend) Payloads.Remove(storage);
		}
		
		var wrapper = new PayloadWrapper { data = storagesToSend };
		string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);

		var fullUri = new Uri(new Uri(Configuration.instance.restUrl), UrlPath);
		using var request = new UnityWebRequest(fullUri, "POST");
		Utils.BuildRequest(request, json);
		Authentication.SetAuthHeaders(request, json);
		
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("iXRLib - Storage POST Request successful");
		}
		else
		{
			Debug.LogError($"iXRLib - Storage POST Request failed : {request.error} - {request.downloadHandler.text}");
			_sendInterval = Configuration.instance.sendRetryIntervalSeconds;
			lock (Lock)
			{
				Payloads.InsertRange(0, storagesToSend);
			}
		}
	}

	public static async Task<PayloadWrapper> Get(string name, iXR.StorageScope scope)
	{
		var queryParams = new Dictionary<string, string>
		{
			{ "name", name },
			{ "scope", scope.ToString() }
		};
		var result = new CoroutineRunner.CoroutineResult<string>();
		await CoroutineRunner.CoroutineToTask(GetStorageEntries(queryParams, result));
		if (result.Value == "") return null;
		return JsonConvert.DeserializeObject<PayloadWrapper>(result.Value);
	}

	private static IEnumerator GetStorageEntries(Dictionary<string, string> queryParams, CoroutineRunner.CoroutineResult<string> result)
	{
		var fullUri = new Uri(new Uri(Configuration.instance.restUrl), UrlPath);
		string urlWithParams = Utils.BuildUrlWithParams(fullUri.ToString(), queryParams);
		using UnityWebRequest request = UnityWebRequest.Get(urlWithParams);
		request.SetRequestHeader("Accept", "application/json");
		Authentication.SetAuthHeaders(request);

		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			result.Value = request.downloadHandler.text;
		}
		else
		{
			Debug.LogWarning($"iXRLib - GetStorageEntries failed: {request.error} - {request.downloadHandler.text}");
			result.Value = "";
		}
	}
	
	public static async Task Delete(iXR.StorageScope scope, string name = "")
	{
		var queryParams = new Dictionary<string, string>
		{
			{ "scope", scope.ToString() }
		};
		if (string.IsNullOrEmpty(name)) queryParams.Add("name", name);
		await CoroutineRunner.CoroutineToTask(DeleteStorageEntries(queryParams));
	}

	private static IEnumerator DeleteStorageEntries(Dictionary<string, string> queryParams)
	{
		var fullUri = new Uri(new Uri(Configuration.instance.restUrl), UrlPath);
		string urlWithParams = Utils.BuildUrlWithParams(fullUri.ToString(), queryParams);
		using UnityWebRequest request = UnityWebRequest.Delete(urlWithParams);
		request.SetRequestHeader("Accept", "application/json");
		Authentication.SetAuthHeaders(request);

		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("iXRLib - DeleteStorageEntries succeeded");
		}
		else
		{
			Debug.LogWarning($"iXRLib - DeleteStorageEntries failed: {request.error} - {request.downloadHandler.text}");
		}
	}
	
	public class Payload
	{
		public string timestamp;  // 'yyyy-MM-ddTHH:mm:ss.fffZ'
		public string keepPolicy; // 'keepLatest' or 'appendHistory'
		public string name;       // defaults to 'state'
		public List<Dictionary<string, string>> data;
		public string scope;      // 'device' or 'user'
	}
	public class PayloadWrapper
	{
		public List<Payload> data;
	}
}