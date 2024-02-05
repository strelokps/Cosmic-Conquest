using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FleetState : MonoBehaviour
{
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet = FleetStateStruct.enumFleetState.Idle;
    [ShowInInspector] private Vector3 _targetToMove;
    [ShowInInspector] private Transform _targetTransform;
    [ShowInInspector] private Vector3 _targetPosition;

    private Transform ownFleetPlanet;

    [ShowInInspector] private ParametrPlanet_mono _distParametrPlanetMono;
    public float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float _tempStopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private float _stopDistForCallDefendefFleet;
    private float distanceToMoveForJoin;
    [SerializeField] private string _targetTransformName;

    private bool flagChkDistance; // ���� ��� �������� ���������
    //test



    private void Update()
    {
        FleetStateMeth();
    }


    public void SetState(FleetStateStruct.enumFleetState locStateFleet, ParametrPlanet_mono locDistPlanetMono)
    {
        distanceToMoveForJoin = 2f; //��������� ��������� ����� �������� ��� join
        speedMove = 5.5f;

        _stopBefore = _tempStopBefore = 16f; // ��������� ��������� ����� �������� ��� �����
        _fleetManager = transform.GetComponent<FleetManager>();

        _stateFleet = locStateFleet;
        _distParametrPlanetMono = locDistPlanetMono;
        //��������� ��������� �� ��������� ������� �� ����� ������ ����� ������� � ������� ��������� ������, � ����������� �� 
        //����� ������ � ����� �����
        _targetToMove = _distParametrPlanetMono.SelfTransform.position;
        var posDefPOint = _distParametrPlanetMono._spawnPointDefenceFleet.position;
        var locSQRDistCallDefendefFleet = (posDefPOint - _targetToMove).sqrMagnitude;
        _stopDistForCallDefendefFleet = _stopBefore + locSQRDistCallDefendefFleet;

        _targetTransformName = locDistPlanetMono.SelfTransform.name + "    " + locDistPlanetMono.prop_ParentTransformFromPlanet.name; //test

    }

    private void FleetStateMeth()
    {
        switch (_stateFleet)
        {
            case FleetStateStruct.enumFleetState.StartForAttack: //I.

                _stopBefore = _stopDistForCallDefendefFleet;
                _stateFleet = FleetStateStruct.enumFleetState.OrbitCallDefendefFleet;
                break;

            case FleetStateStruct.enumFleetState.OrbitCallDefendefFleet: //II.

                Movement();
                if (CheckDistanceToAttack())
                {
                    _stopBefore = 16f;
                    _stateFleet = FleetStateStruct.enumFleetState.MoveToOrbitAttack;
                    _fleetManager.StartRegenShield();
                    CallDefFleet();
                }
                break;

            case FleetStateStruct.enumFleetState.MoveToOrbitAttack: //III.

                Movement();
                if (CheckDistanceToAttack())
                {
                    _stateFleet = FleetStateStruct.enumFleetState.OrbitAttack;
                   
                }
                break;

            case FleetStateStruct.enumFleetState.OrbitAttack:   //IV.
                CallDefFleet();
                CheckOtherAttackersFleetFromOnePlanetToJoin();
                break;

            case FleetStateStruct.enumFleetState.Attack:
                Attack();
                break;

            case FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent:
                Movement();
                break;
            
            case FleetStateStruct.enumFleetState.OrbitJoinToDefenderFleet:
                //print($"�� ��� ������, ������");
                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet:
                break;
            
            case FleetStateStruct.enumFleetState.StartForDefence:

                break;
            
            case FleetStateStruct.enumFleetState.OrbitDefence:
                Movement();
                break;

            case FleetStateStruct.enumFleetState.Idle:
                break;
          
        }
    }

    //���� ������� ���������, �� ����������� � ������ ���������� �����, ���� ����, �� � �������������  �� ������� � ������� 
   


   

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_targetToMove.x, 0, _targetToMove.z)
            , speedMove * Time.deltaTime);

    }
    /// <summary>
    /// true ���� ������� ����� � ������� ���� ����������
    /// </summary>
    /// <returns></returns>
    //��������� ������� �� ������� �������������
    private bool CheckDistPlanetIsEnemy()
    {
        bool flagCheckDistPlanetIsEnemy = _distParametrPlanetMono.CompareParents(_fleetManager.GetParentTransform());

        return flagCheckDistPlanetIsEnemy;
    }

    public bool CheckDistanceToAttack()
    {
        flagChkDistance = false;
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;

        if (_distanceSqr < _stopBefore)
            flagChkDistance = true;

        return flagChkDistance;
    }

    //��������� ���� �������� ���� ������ �� ������
    private void CallDefFleet()
    {
        //�������� �������� �� ������� ��� ��� ������ ����� ��������� ���������
        if (!CheckDistPlanetIsEnemy())
        {
            _distParametrPlanetMono.CallDefenderFleet(transform);
        }
        else
        {
            //���� ������� ������������� � ���� �� ������ �����, �� ����� �������������� � ����� ������ �� �������
            if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent;
            }
        }
    }

    private void StartForDefence()
    {
    }


    //��������� ������� � ���������� ������ ����������� �� ������ �� ����� � ����� �������, ���� �������, �� ��������� � ��
    private void CheckOtherAttackersFleetFromOnePlanetToJoin()
    {
        //��� ���� ��������� ����
        if (_distParametrPlanetMono.attackingFleet_LGO.Count > 0)
        {
            for (int i = 0; i < _distParametrPlanetMono.attackingFleet_LGO.Count; i++)
            {
                //��������� ������� ������ ���������� � ��� �� ������� ��� � ������ ����, ���� ����, �� �����������.
                if (_distParametrPlanetMono.attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        ._selfPlanetTransform == _fleetManager._selfPlanetTransform)
                {
                    _distParametrPlanetMono
                        .attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetListDataFleet());
                    _fleetManager.DestroyAttackingFleet();
                }
            }
        }
        else
        {
            _distParametrPlanetMono.AddToListAttackerFleet(gameObject);
        }
    }

    private void PreparingAttack()
    {
        _distParametrPlanetMono.GetTransformDefenceFleet(ref _targetTransform);
        if (_targetTransform != null)
        {
            _stateFleet = FleetStateStruct.enumFleetState.Attack;
        }
        else
        {
            _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent;
        }
    }

    private void Attack()
    {
        if (_distParametrPlanetMono.)
        {
            //attack
            print($"<color=green>Attack</color>");
        }
        else
        {
            //captur planet
        }
    }


    private void Shooting()
    {

    }


}
