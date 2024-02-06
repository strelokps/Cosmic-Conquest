using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FleetStateStruct
{
    public enum enumFleetState
    {
        Idle,
        StartForAttack,
        MoveToOrbitAttack,
        OrbitCallDefendefFleet,
        OrbitAttack,
        Attack,
        OrbitJoinToDefenderFleet,
        PreMovingTowardsPlanet,
        MovingTowardsPlanetForDescent ,
        MovingTowardsDefenceFleet,
        StartForDefence,
        OrbitDefence
    }
}
