using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FleetState : MonoBehaviour
{
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet = FleetStateStruct.enumFleetState.Idle;
    [ShowInInspector] private Vector3 _targetToMove;
    [ShowInInspector] private Transform _targetTransform;

    [ShowInInspector] private ParametrPlanet_mono _managerAttackPlanet;
    [SerializeField] private float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float  _distanceSqr;

    private void Start()
    {
        _stopBefore = 16; // дистанция остановки перед объектом
        speedMove = 0.5f;
    }

    private void Update()
    {
        FleetStateMeth();
    }

    //string нужен для теста
    //Устанавливаем цель и переключаем sate на моve
    private void SetTargetToMove(Vector3 locTargetPosition)
    {
        _targetToMove = locTargetPosition;
        _stateFleet = FleetStateStruct.enumFleetState.Movement;
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_targetToMove.x, 0, _targetToMove.z)
            , speedMove * Time.deltaTime);

        CheckDistance(_targetToMove);
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
            case FleetStateStruct.enumFleetState.Attack:
                break;
            case FleetStateStruct.enumFleetState.Defence:

                break;
        }
    }

    private void CheckDistance(Vector3 locTargetToMove)
    {
        _distanceSqr = (locTargetToMove - transform.position).sqrMagnitude;

        if (_distanceSqr < _stopBefore)
        {
            _stateFleet = FleetStateStruct.enumFleetState.PreAttack;
        }
    }

    private void ClearParam()
    {
        _distanceSqr = 0f;
        _targetToMove = new Vector3();
    }

    public void SetState(Transform locTargetPosition, FleetStateStruct.enumFleetState locStateFleet)
    {
        _stateFleet = locStateFleet;
        _targetTransform = locTargetPosition;
        _targetToMove = locTargetPosition.position;
    }

    private void PreAttack()
    {
        //TODO Сделать спавн точку для деф флота, который встает перед атакующем флотом
        _managerAttackPlanet = _targetTransform?.GetComponent<ParametrPlanet_mono>();
    }
}
