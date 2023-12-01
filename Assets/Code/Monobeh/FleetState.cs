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

    [ShowInInspector] private ParametrPlanet_mono _distPlanetMonoinState;
    [SerializeField] private float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private float distanceToAttack;
    private float distanceToMoveForJoin;

    private void Start()
    {
        _stopBefore = distanceToAttack = 16f; // ��������� ��������� ����� �������� ��� �����
        distanceToMoveForJoin = 2f; //��������� ��������� ����� �������� ��� join
        speedMove = 2.5f;
        _fleetManager = transform.GetComponent<FleetManager>();

    }

    private void Update()
    {
        FleetStateMeth();
    }

    

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_targetToMove.x, 0, _targetToMove.z)
            , speedMove * Time.deltaTime);

    }



    private void FleetStateMeth()
    {
        switch (_stateFleet)
        {
            case FleetStateStruct.enumFleetState.Movement:
                Movement();
                CheckDistanceToAttack();

                //print($"move");
                break;
            case FleetStateStruct.enumFleetState.Idle:
                //print($"idle");
                break;
            case FleetStateStruct.enumFleetState.PreAttack:
                PreAttack();
                break;
            case FleetStateStruct.enumFleetState.Attack:
                print($"�� �������!!!!");
                FireToTarget();
                break;
            case FleetStateStruct.enumFleetState.Defence:
                print($"�� ��� ������, ������");
                break;
            case FleetStateStruct.enumFleetState.PreTowardsPlanet:
                _stopBefore = 1f;
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanet;
                break;
            case FleetStateStruct.enumFleetState.MovingTowardsPlanet:
                Movement();
                CheckDistanceMovingTowardsPlanet();
                break;
        }
    }

    private void CheckDistanceToAttack()
    {
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;
        if (_distanceSqr < _stopBefore)
        {
            //�������� �������� �� ������� ��� ��� ������
            if (!_distPlanetMonoinState.CompareParents(_fleetManager.GetParentTransform()))
            {

                if (_distPlanetMonoinState.goDefFleet.Count > 0)
                {
                    //TODO ����� ��� ������������� �����
                    print($"Attack def fleet on orbit!!!!!");
                    _stateFleet = FleetStateStruct.enumFleetState.Attack;
                    CheckOtherAttackersFleetFromOnePlanetToJoin();

                }
                else
                if (_distPlanetMonoinState.CallDefenderFleet(transform))
                {
                    print($"Gen def fleet and Attack !!!!!");
                    _stateFleet = FleetStateStruct.enumFleetState.Attack;
                    CheckOtherAttackersFleetFromOnePlanetToJoin();

                }
                else
                {
                    MovingTowardsPlanet();
                }
            }
            else
            {
                MovingTowardsPlanet();
            }
        }
    }

    private void CheckDistanceMovingTowardsPlanet()
    {
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;
        if (_distanceSqr < _stopBefore)
        {
            CheckOtherAttackersFleetFromOnePlanetToJoin();
            _fleetManager.JoinToDefender();
        }
    }

    private void ClearParam()
    {
        _distanceSqr = 0f;
        _targetToMove = new Vector3();
    }

    public void SetState(Transform locTargetPosition, FleetStateStruct.enumFleetState locStateFleet, 
        ParametrPlanet_mono locDistPlanetMono )
    {
        _stateFleet = locStateFleet;
        _targetTransform = locTargetPosition;
        _targetToMove = locDistPlanetMono.selfTransform.position;
        _distPlanetMonoinState = locDistPlanetMono;

    }

    private void PreAttack()
    {
        _stateFleet = FleetStateStruct.enumFleetState.Movement;
    }

    private void FireToTarget()
    {
        CheckHaveAPlanetDefFllet();
    }

    
    //��������� ������� � ���������� ������ ����������� �� transform �������, ���� �������, �� ��������� � ��
    private void CheckOtherAttackersFleetFromOnePlanetToJoin()
    {
        //��� ���� ��������� ����
        if (_distPlanetMonoinState._listAttackersFleet.Count > 0)
        {
            for (int i = 0; i < _distPlanetMonoinState._listAttackersFleet.Count; i++)
            {
                print($"dist: {_distPlanetMonoinState._listAttackersFleet[i].GetComponent<FleetManager>()._selfPlanetTransform}" +
                      $"self: {_fleetManager._selfPlanetTransform}");
                //��������� ������� ������ ���������� � ��� �� ������� ��� � ������ ����, ���� ����, �� �����������.
                if (_distPlanetMonoinState._listAttackersFleet[i].GetComponent<FleetManager>()._selfPlanetTransform
                    == _fleetManager._selfPlanetTransform)
                {
                    _distPlanetMonoinState
                        ._listAttackersFleet[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetDataFleetList());
                    _fleetManager.DestroyFleet();
                }
            }
        }
        else
        {
            _distPlanetMonoinState._listAttackersFleet.Add(gameObject);
        }

    }

    private void CheckHaveAPlanetDefFllet()
    {
        if(_distPlanetMonoinState.ChangeOwnerPlanet(_fleetManager.GetMembersData(), _fleetManager.GetParentTransform()))
        {
            
        }

    }

    private bool CheckDefEnemyFleetonOrbit()
    {
        var flagCheckEnemyDefFleet = false;

        //���� � ������� ��� ����� ���������, �� ������� �����
        if (_distPlanetMonoinState.CallDefenderFleet(transform))
        {
            _distPlanetMonoinState.CallDefenderFleet(transform);
            flagCheckEnemyDefFleet = true;
        }

        return flagCheckEnemyDefFleet;
    }

    private void MovingTowardsPlanet()
    {
        _stopBefore = distanceToMoveForJoin;
        _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanet;
    }

}
