using System.Collections.Generic;
using System.Globalization;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public interface IIxrService
{
    iXRResult LogDebug(string text);
    iXRResult LogInfo(string text);
    iXRResult LogWarn(string text);
    iXRResult LogError(string text);
    iXRResult LogCritical(string text);
    iXRResult Event(string message, Dictionary<string, string> meta);
    iXRResult Event(string message, string meta);
    iXRResult TelemetryEntry(string name, Dictionary<string, string> data);
    iXRResult TelemetryEntry(string name, string data);
    void PresentKeyboard(string promptText, string keyboardType, string emailDomain);
    void PollUser(string prompt, ExitPollHandler.PollType pollType);
}

public class IxrService : IIxrService
{
    private readonly IExitPollHandler _exitPollHandler;

    public IxrService(IExitPollHandler exitPollHandler)
    {
        _exitPollHandler = exitPollHandler;
    }

    public iXRResult LogDebug(string text) => iXRSend.LogDebug(text);
    public iXRResult LogInfo(string text) => iXRSend.LogInfo(text);
    public iXRResult LogWarn(string text) => iXRSend.LogWarn(text);
    public iXRResult LogError(string text) => iXRSend.LogError(text);
    public iXRResult LogCritical(string text) => iXRSend.LogCritical(text);
    
    public iXRResult Event(string message, Dictionary<string, string> meta) => iXRSend.Event(message, meta);
    public iXRResult Event(string message, string meta) => iXRSend.Event(message, meta);
    
    public iXRResult TelemetryEntry(string name, Dictionary<string, string> data) => iXRSend.AddTelemetryEntry(name, data);
    public iXRResult TelemetryEntry(string name, string data) => iXRLibInterop.AddTelemetryEntry(name, data);
    
    public void PresentKeyboard(string promptText = null, string keyboardType = null, string emailDomain = null)
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

    public void PollUser(string prompt, ExitPollHandler.PollType pollType)
    {
        _exitPollHandler.AddPoll(prompt, pollType);
    }
}

// Keep the original iXR class as a facade for backward compatibility
// public class iXR
// {
//     private static readonly IIxrService Service = new IxrService();
    
//     private static Dictionary<string, float> assessmentStartTimes = new Dictionary<string, float>();
//     private static Dictionary<string, float> interactionStartTimes = new Dictionary<string, float>();
//     private static Dictionary<string, float> levelStartTimes = new Dictionary<string, float>();

//     // Original enums
//     public enum ResultOptions
//     {
//         Pass = iXRLib.ResultOptions.Pass,
//         Fail = iXRLib.ResultOptions.Fail,
//         Complete = iXRLib.ResultOptions.Complete,
//         Incomplete = iXRLib.ResultOptions.Incomplete
//     }

//     public enum InteractionType
//     {
//         Null = iXRLib.InteractionType.Null,
//         Bool = iXRLib.InteractionType.Bool,
//         Select = iXRLib.InteractionType.Select,
//         Text = iXRLib.InteractionType.Text,
//         Rating = iXRLib.InteractionType.Rating,
//         Number = iXRLib.InteractionType.Number
//     }

//     // Original static methods now delegate to the service
//     public static iXRResult LogDebug(string text) => Service.LogDebug(text);
//     public static iXRResult LogInfo(string text) => Service.LogInfo(text);
//     public static iXRResult LogWarn(string text) => Service.LogWarn(text);
//     public static iXRResult LogError(string text) => Service.LogError(text);
//     public static iXRResult LogCritical(string text) => Service.LogCritical(text);
    
//     public static iXRResult Event(string message, Dictionary<string, string> meta) => Service.Event(message, meta);
//     public static iXRResult Event(string message, string meta) => Service.Event(message, meta);
    
//     public static iXRResult TelemetryEntry(string name, Dictionary<string, string> data) => Service.TelemetryEntry(name, data);
//     public static iXRResult TelemetryEntry(string name, string data) => Service.TelemetryEntry(name, data);

//     // Keep all other original methods, delegating to the service where appropriate
//     // ... (rest of the original code)
// }