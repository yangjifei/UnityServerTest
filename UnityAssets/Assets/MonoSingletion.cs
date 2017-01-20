using UnityEngine;
using System.Collections;

public class MonoSingletion<T> : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = GetComponent<T>();
    }
}
