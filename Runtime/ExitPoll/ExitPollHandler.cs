using System;
using System.Collections.Generic;
using UnityEngine;

public interface IExitPollHandler
{
    void Initialize(IIxrService ixrService);
    void AddPoll(string prompt, ExitPollHandler.PollType pollType);
}

public class ExitPollHandler : MonoBehaviour, IExitPollHandler
{
    public enum PollType
    {
        Thumbs,
        Rating
    }
    
    private IIxrService _ixrService;
    private readonly List<Tuple<string, PollType>> _polls = new();
    private bool _isProcessing;
    private IExitPoll _currentPoll;
    
    public void Initialize(IIxrService ixrService)
    {
        _ixrService = ixrService;
    }
    
    public void AddPoll(string prompt, PollType pollType)
    {
        _polls.Add(new Tuple<string, PollType>(prompt, pollType));
        
        if (!_isProcessing) ProcessPoll();
    }

    private void CreatePoll(PollType pollType)
    {
        string pollPath = "";
        if (pollType == PollType.Rating) pollPath = "Prefabs/iXRExitPollRating";
        else if (pollType == PollType.Thumbs) pollPath = "Prefabs/iXRExitPollThumbs";
        
        GameObject exitPollPrefab = Resources.Load<GameObject>(pollPath);
        if (exitPollPrefab != null)
        {
            var exitPollObject = Instantiate(exitPollPrefab, Camera.main.transform);
            _currentPoll = exitPollObject.GetComponent<ExitPoll>();
            if (_currentPoll == null)
            {
                Debug.LogError("Exit poll prefab does not contain ExitPoll component");
                return;
            }
            WireButtonHandlers();
        }
        else
        {
            Debug.LogError("Failed to load exit poll prefab");
        }
    }

    private void ProcessPoll()
    {
        _isProcessing = true;

        var poll = _polls[0];
        CreatePoll(poll.Item2);
        if (_currentPoll != null)
        {
            _currentPoll.prompt.text = poll.Item1;
        }
        _polls.RemoveAt(0);
    }

    private void WireButtonHandlers()
    {
        if (_currentPoll == null) return;
        
        _currentPoll.OnThumbsUp += HandleThumbsUp;
        _currentPoll.OnThumbsDown += HandleThumbsDown;
        _currentPoll.OnRating += HandleRating;
    }
    
    private void HandleThumbsUp(object sender, EventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, "answer=up");
        if (_polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
    
    private void HandleThumbsDown(object sender, EventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, "answer=down");
        if (_polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
    
    private void HandleRating(object sender, ExitPoll.RatingEventArgs e)
    {
        var poll = (ExitPoll)sender;
        _ixrService.Event(poll.prompt.text, $"answer={e.rating}");
        if (_polls.Count > 0) ProcessPoll();
        _isProcessing = false;
    }
}