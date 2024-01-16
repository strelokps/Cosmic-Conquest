using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FleetStateStruct
{
    public enum enumFleetState
    {
        Idle,
        StartForAttack,
        MoveToOrbitAttack,
        OrbitCallDefendefFleet,
        OrbitAttack,
        OrbitJoinToDefenderFleet,
        PreMovingTowardsPlanet,
        MovingTowardsPlanetForDecent ,
        MovingTowardsDefenceFleet,
        StartForDefence,
        OrbitDefence
    }
}
