using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FleetState : MonoBehaviour
{
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet = FleetStateStruct.enumFleetState.Idle;
    [ShowInInspector] private Vector3 _targetToMove;
    [ShowInInspector] private Transform _targetTransform;
    private Transform ownFleetPlanet;

    [ShowInInspector] private ParametrPlanet_mono _managerTheAttackedPlanet;
    [SerializeField] private float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private void Start()
    {
        _stopBefore = 16; // дистанция остановки перед объектом
        speedMove = 2.5f;
        _fleetManager = transform.GetComponent<FleetManager>();

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

        CheckDistanceToAttack(_targetToMove);
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
            case FleetStateStruct.enumFleetState.PreAttack:
                PreAttack();
                break;
            case FleetStateStruct.enumFleetState.Attack:
                print($"На абордаж!!!!");
                FireToTarget();
                break;
            case FleetStateStruct.enumFleetState.Defence:
                print($"На нас напали, Милорд");
                break;
            case FleetStateStruct.enumFleetState.JoinToDefender:
                break;
        }
    }

    private void CheckDistanceToAttack(Vector3 locTargetToMove)
    {

        if (_distanceSqr < _stopBefore)
        {
            CheckOtherAttackersFleet();
            _stateFleet = FleetStateStruct.enumFleetState.Attack;
        }
    }

    private void CheckDistanceToJoin(Vector3 locTargetToMove)
    {

        if (_distanceSqr < _stopBefore)
        {
            CheckOtherAttackersFleet();
            _stateFleet = FleetStateStruct.enumFleetState.JoinToDefender;
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
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;

    }

    private void PreAttack()
    {

        _managerTheAttackedPlanet = _targetTransform?.GetComponent<ParametrPlanet_mono>();
        //если у планеты нет флота защитника, то генерим новый
        if (_managerTheAttackedPlanet.goDefFleet.Count <= 0)
             _managerTheAttackedPlanet.DefenderFleet(transform);

        _stateFleet = FleetStateStruct.enumFleetState.Movement;
    }

    private void FireToTarget()
    {
        CheckHaveAPlanetDefFllet();
    }

    //проверяем наличиу у нападающих флотов соотвествие по transform планеты, если совпало, то добавляем к фл
    private void CheckOtherAttackersFleet()
    {
        if (_managerTheAttackedPlanet._listAttackersFleet.Count > 0)
        {
            for (int i = 0; i < _managerTheAttackedPlanet._listAttackersFleet.Count; i++)
            {
                print($"dist: {_managerTheAttackedPlanet._listAttackersFleet[i].GetComponent<FleetManager>().planetIsOwenerFleet}" +
                      $"self: {_fleetManager.planetIsOwenerFleet}");
                if (_managerTheAttackedPlanet._listAttackersFleet[i].GetComponent<FleetManager>().planetIsOwenerFleet
                    == _fleetManager.planetIsOwenerFleet)
                {
                    _managerTheAttackedPlanet
                        ._listAttackersFleet[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetDataFleetList());
                    _fleetManager.DestroyFleet();
                }
            }
        }
        else
        {
            _managerTheAttackedPlanet._listAttackersFleet.Add(gameObject);
        }

    }

    private void CheckHaveAPlanetDefFllet()
    {
        if(_managerTheAttackedPlanet.ChangeOwnerPlanet(_fleetManager.GetMembersData(), _fleetManager.GetParentTransform()))
        {
            
        }

    }

}
