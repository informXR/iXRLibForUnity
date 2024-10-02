using System;
using iXRLib;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine;

public class KeyboardHandler : MonoBehaviour
{
    private static KeyboardHandler _instance;
    
    public static void Initialize()
    {
        if (_instance != null) return;
        
        var singletonObject = new GameObject("KeyboardPositioner");
        _instance = singletonObject.AddComponent<KeyboardHandler>();
        DontDestroyOnLoad(singletonObject);
    }
    
    private void Start()
    {
        GameObject keyboard = Resources.Load<GameObject>("Prefabs/iXRKeyboard");
        if (keyboard != null)
        {
            Instantiate(keyboard, Camera.main.transform);
        }
        else
        {
            Debug.LogError("Failed to load keyboard prefab");
        }
        
        NonNativeKeyboard.Instance.OnTextSubmitted += HandleTextSubmitted;
    }

    private static void HandleTextSubmitted(object sender, EventArgs e)
    {
        NonNativeKeyboard.Instance.Close();
        var keyboard = (NonNativeKeyboard)sender;
        Authentication.KeyboardAuthenticate(keyboard.InputField.text);
    }
}