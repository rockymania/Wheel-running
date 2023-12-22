using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static Main instance;

    public static Main Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Main>();
            }
            return instance;
        }
    }

    public delegate void StartWheelEvent();
    public event StartWheelEvent OnStart;

    public delegate void StopWheelEvent();
    public event StopWheelEvent OnStop;

    public void onStart()
    {
        OnStart?.Invoke();
    }

    public void onStop()
    {
        OnStop?.Invoke();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
