using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventBus 
{
    public static event Action<int> OnSolariumUpdate;


    private static EventBus _instans;

    public static EventBus Instans
    {
        get
        {
            if (_instans == null) _instans = new EventBus();
            return _instans;
        }
        set => _instans = value;
    }

    public void InvokeSolarium(int locSolarium)
    {
        OnSolariumUpdate?.Invoke(locSolarium);
    }
}
