using System;
using System.Collections.Generic;
using UnityEngine;

public class ExitPollHandler : MonoBehaviour
{
    public enum PollType
    {
        Thumbs,
        Rating
    }
    
    private static ExitPollHandler _instance;
    private static readonly List<Tuple<string, PollType>> Polls = new();
    private static bool _isProcessing;
    private static IIxrService _ixrService;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("ExitPollHandler");
        _instance = singletonObject.AddComponent<ExitPollHandler>();
        DontDestroyOnLoad(singletonObject);
        _ixrService = ServiceLocator.GetService<IIxrService>();
    }
    
    public static void AddPoll(string prompt, PollType pollType)
    {
        Polls.Add(new Tuple<string, PollType>(prompt, pollType));
        
        if (!_isProcessing) ProcessPoll();
    }

    private static void CreatePoll(PollType pollType)
    {
        string pollPath = "";
        if (pollType == PollType.Rating) pollPath = "Prefabs/iXRExitPollRating";
        else if (pollType == PollType.Thumbs) pollPath = "Prefabs/iXRExitPollThumbs";
        GameObject exitPoll = Resources.Load<GameObject>(pollPath);
        if (exitPoll != null)
        {
            Instantiate(exitPoll, Camera.main.transform);
        }
        else
        {
            Debug.LogError("Failed to load exit poll prefab");
        }
    }

    private static void ProcessPoll()
    {
        _isProcessing = true;

        Tuple<string, PollType> poll = Polls[0];
        CreatePoll(poll.Item2);
        ExitPoll.Instance.prompt.text = poll.Item1;
        WireButtonHandlers();
        Polls.RemoveAt(0);
    }

    private static void WireButtonHandlers()
    {
        ExitPoll.Instance.OnThumbsUp += HandleThumbsUp;
        ExitPoll.Instance.OnThumbsDown += HandleThumbsDown;
        ExitPoll.Instance.OnRating += HandleRating;
    }
    
    private static void HandleThumbsUp(object sender, EventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, "answer=up");
        if (Polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
    
    private static void HandleThumbsDown(object sender, EventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, "answer=down");
        if (Polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
    
    private static void HandleRating(object sender, ExitPoll.RatingEventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, $"answer={e.rating}");
        if (Polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
}