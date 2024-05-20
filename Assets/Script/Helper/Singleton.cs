using UnityEngine;

// Singleton class can be inherited by other classes when needed
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            // Can use for unity error logs 
            // Debug.LogErrorFormat("[Singleton] Trying to instantiate a second instance of singleton class {0} from {1}", GetType().Name,  this.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}