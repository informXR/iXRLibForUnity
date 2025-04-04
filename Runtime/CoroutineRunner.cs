using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            var go = new GameObject("CoroutineRunner");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<CoroutineRunner>();
            return _instance;
        }
    }
    
    // Ideally we just use async/await for all the async calls, but it doesn't play nicely with WebGL
    // This is the workaround
    public static Task<string> CoroutineToTask(IEnumerator coroutine)
    {
        var tcs = new TaskCompletionSource<string>();
        _instance.StartCoroutine(Wrapper());
        return tcs.Task;

        // Wrap coroutine to complete Task
        IEnumerator Wrapper()
        {
            yield return coroutine;
            tcs.SetResult("Finished");
        }
    }
    
    public class CoroutineResult<T>
    {
        public T Value;
    }
}