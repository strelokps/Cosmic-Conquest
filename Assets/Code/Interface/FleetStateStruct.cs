using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FleetStateStruct
{
    public enum enumFleetState
    {
        Idle = 0,
        Attack = 1,
        Movement = 2,
        SetTarget = 3,
    }
}