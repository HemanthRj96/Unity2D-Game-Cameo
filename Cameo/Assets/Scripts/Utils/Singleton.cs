using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance;
    protected bool doNotDestroyOnLoad = false;

    protected void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
        {
            if (doNotDestroyOnLoad)
                DontDestroyOnLoad(this);
            instance = this as T;
        }
    }
}
