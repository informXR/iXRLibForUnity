using System;
using System.Collections;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;
using iXRLib;

public class KeyboardHandler : MonoBehaviour
{
    private static KeyboardHandler _instance;
    private static IAuthenticationService _authService;
    public static bool ProcessingSubmit;
    private const string ProcessingText = "Processing";
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("KeyboardHandler");
        _instance = singletonObject.AddComponent<KeyboardHandler>();
        _authService = ServiceLocator.GetService<IAuthenticationService>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        GameObject keyboard = Resources.Load<GameObject>("Prefabs/iXRKeyboard");
        if (keyboard != null)
        {
            Instantiate(keyboard, Camera.main.transform);
            Debug.Log("iXRLib - Loaded keyboard prefab");
        }
        else
        {
            Debug.LogError("iXRLib - Failed to load keyboard prefab");
        }
        
        NonNativeKeyboard.Instance.OnTextSubmitted += HandleTextSubmitted;
        
        if (iXRAuthentication.AuthMechanism.ContainsKey("prompt"))
        {
            _authService.KeyboardAuthenticate();
        }
    }

    private async void HandleTextSubmitted(object sender, EventArgs e)
    {
        if (ProcessingSubmit) return;
        
        StartCoroutine(ProcessingVisual());
        var keyboard = (NonNativeKeyboard)sender;
        await _authService.KeyboardAuthenticate(keyboard.InputField.text);
    }
    
    private static IEnumerator ProcessingVisual()
    {
        ProcessingSubmit = true;
        NonNativeKeyboard.Instance.Prompt.text = ProcessingText;
        while (ProcessingSubmit)
        {
            string currentText = NonNativeKeyboard.Instance.Prompt.text;
            NonNativeKeyboard.Instance.Prompt.text = currentText.Length > ProcessingText.Length + 10 ?
                ProcessingText :
                $":{NonNativeKeyboard.Instance.Prompt.text}:";
            
            yield return new WaitForSeconds(0.5f); // Wait before running again
        }
    }
}