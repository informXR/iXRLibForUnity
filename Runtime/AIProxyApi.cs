using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class AIProxyApi : MonoBehaviour
{
    private const string UrlPath = "/v1/services/llm";
    private static readonly List<string> PastMessages = new();
    private static string _url;
    private static readonly Dictionary<string, string> Responses = new();

    private void Start()
    {
        _url = new Uri(new Uri(Configuration.instance.restUrl), UrlPath).ToString();
    }
    
    public static async Task<string> SendPrompt(string prompt, string llmProvider, List<string> pastMessages = null)
    {
        await CoroutineRunner.CoroutineToTask(Send(prompt, llmProvider, pastMessages));
        return Responses.GetValueOrDefault(prompt, "");
    }
    
    private static IEnumerator Send(string prompt, string llmProvider, List<string> pastMessages = null)
    {
        pastMessages = pastMessages == null ? PastMessages : pastMessages.Union(PastMessages).ToList();
        
        var payload = new AIPromptPayload
        {
            prompt = prompt,
            llmProvider = llmProvider,
            pastMessages = pastMessages
        };
        
        string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
        
        using var request = new UnityWebRequest(_url, "POST");
        Utils.BuildRequest(request, json);
        Authentication.SetAuthHeaders(request, json);
		
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            PastMessages.Add(prompt);
            Responses[prompt] = request.downloadHandler.text;
            Debug.Log("iXRLib - AI POST Request successful");
        }
        else
        {
            Debug.LogError($"iXRLib - AI POST Request failed : {request.error}");
            //TODO retry logic
        }
    }
    
    private class AIPromptPayload
    {
        public string prompt;
        public string llmProvider;
        public List<string> pastMessages;
    }
}