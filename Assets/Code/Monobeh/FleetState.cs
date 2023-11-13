using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetState : MonoBehaviour
{
    private FleetStateStruct.enumFleetState _stateFleet = FleetStateStruct.enumFleetState.Idle;
    private Vector3 _tragetToMove;



    private void Update()
    {
        FleetStateMeth();
    }

    public void SetTargetToMove(Vector3 locTargetPosition)
    {
        _tragetToMove = locTargetPosition;
        _stateFleet = FleetStateStruct.enumFleetState.Movement;
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_tragetToMove.x, 0, _tragetToMove.z)
            , 1f * Time.deltaTime);
    }

    private void FleetStateMeth()
    {
        switch (_stateFleet)
        {
            case FleetStateStruct.enumFleetState.Movement:
                Movement();
                //print($"move");
                break;
            case FleetStateStruct.enumFleetState.Idle:
                //print($"idle");
                break;
        }
    }
}
