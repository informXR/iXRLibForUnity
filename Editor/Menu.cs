using UnityEngine;
using UnityEditor;

public class Menu
{
    private static Configuration _config;
    
    [MenuItem("informXR/Configuration", priority = 1)]
    private static void Configuration()
    {
        Selection.activeObject = Core.GetConfig();
    }
    
    [MenuItem("informXR/Documentation", priority = 2)]
    private static void Documentation()
    {
        Application.OpenURL("https://www.informxr.com/about");
    }
}
