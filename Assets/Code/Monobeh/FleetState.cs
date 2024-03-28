using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;
using Color = System.Drawing.Color;

public class FleetState : MonoBehaviour
{
    [SerializeField] public FleetStateStruct.enumFleetState _stateFleet = FleetStateStruct.enumFleetState.Idle;
    [ShowInInspector] private Vector3 _targetToMove;
    [ShowInInspector] private Transform _targetTransform;
    [ShowInInspector] private Vector3 _targetPosition;
    private FleetStateStruct.enumFleetState _enemyStateFleet;

    private Transform ownFleetPlanet;

    [ShowInInspector] private ParametrPlanet_mono _distParametrPlanetMono;
    private ParametrPlanet_mono _selfParametrPlanetMono;
    public float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float _tempStopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private float _stopDistForCallDefendefFleet;
    [SerializeField] private string _targetTransformName;

    [Header("Scale")]
    [SerializeField] private float _timeToScale;

    private float scaleModifier;

    private float _endScale;
    private float _startScale;
    private Vector3 _originalScale;
    private float elapsedTime;

    private FleetShootingSystem _fleetShootingSystem;


    private bool flagChkDistance; // ���� ��� �������� ���������
    //test

    private int count = 0;

    private void Update()
    {
        FleetStateMeth();
        print($"fleet  U {transform.rotation}");

    }


    public void SetState
        (FleetStateStruct.enumFleetState locStateFleet, ParametrPlanet_mono locDistPlanetMono, ParametrPlanet_mono selfPlanetMono)
    {
        speedMove = 5.5f;

        _stopBefore = _tempStopBefore = 30f; // ��������� ��������� ����� �������� ��� �����

        _fleetManager = transform.GetComponent<FleetManager>();
        _fleetShootingSystem = GetComponent<FleetShootingSystem>();

        _stateFleet = locStateFleet;

        _distParametrPlanetMono = locDistPlanetMono;
        _selfParametrPlanetMono = selfPlanetMono;

        //��������� ���������� �� ��������� ������� �� ����� ������ ����� ������� � ������� ��������� ������, � ����������� �� 
        //����� ������ � ����� �����. 
        //���������� ����� ������� ������ ��� ����� � ������� ����� ����� ��������� ����� �������� � ������ ������ ��� �����
        _targetToMove = _distParametrPlanetMono.SelfTransform.position;
        
        var posDefPOint = _distParametrPlanetMono._spawnPointDefenceFleet.position;
        var locSQRDistCallDefendefFleet = (posDefPOint - _targetToMove).sqrMagnitude;
        
        _stopDistForCallDefendefFleet = _stopBefore + locSQRDistCallDefendefFleet;

        _targetTransformName = locDistPlanetMono.SelfTransform.name + "    " + locDistPlanetMono.prop_ParentTransformFromPlanet.name; //test

        //scale
        elapsedTime = 0f;
    }

    private void SetParamScale(float locTimeToScale, float locStartScale, float locEndScale, Vector3 locOriginalScale)
    {
        _timeToScale = locTimeToScale;
        _startScale = locStartScale;
        _endScale = locEndScale;
        _originalScale = locOriginalScale;
    }

    private void FleetStateMeth()
    {
        switch (_stateFleet)
        {
            case FleetStateStruct.enumFleetState.StartForAttack: //I.

                _stopBefore = _stopDistForCallDefendefFleet;
                _stateFleet = FleetStateStruct.enumFleetState.OrbitCallDefendefFleet;
                SetParamScale(0.8f, 0.02f, 1f, new Vector3(1f, 1f, 1f));


                break;

            case FleetStateStruct.enumFleetState.OrbitCallDefendefFleet: //II.
                ScaleForDescent(_startScale, _endScale);

                Movement();
                if (CheckDistanceToAttack())
                {
                    _stopBefore *= 0.7f;
                    _stateFleet = FleetStateStruct.enumFleetState.MoveToOrbitAttack;
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
                SetParamScale(0.8f, 1f, 0.2f, transform.localScale);
                elapsedTime = 0f;
                if(CheckOtherAttackersFleetFromOnePlanetToJoin())
                    break;
                CallDefFleet();
                break;

            case FleetStateStruct.enumFleetState.Attack:
                Attack();
                break;

            case FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent:
                Movement();
                ScaleForDescent(_startScale, _endScale);
                if (CheckDistanceToAttack())
                {
                    _fleetManager.JoinToDefenderFleet();
                }
                break;
            case FleetStateStruct.enumFleetState.RefundDefenceFleet:
                Movement();
                ScaleForDescent(_startScale, _endScale);
                if (CheckDistanceToAttack())
                {
                    _fleetManager.JoinToDefenderFleet();
                }
                break;

            case FleetStateStruct.enumFleetState.FoundTarget:
                FoundTarget();
                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet:
                break;
            
            case FleetStateStruct.enumFleetState.StartForDefence:
                StartDefence();
                print($"fleet {transform.rotation}");
                break;
            
            case FleetStateStruct.enumFleetState.OrbitDefence:
                ScaleForDescent(_startScale, _endScale);
                Movement();
                if (scaleModifier >= 1)
                {
                    _stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
                }
                break;

            case FleetStateStruct.enumFleetState.Idle:
                break;
          
        }
    }


    private void FoundTarget()
    {
        if (_fleetManager.isDefenceFleet)
        {
            print($"Scaning for new target for Def fleet");

            _fleetShootingSystem.SetTarget(TakeTargetForDefenceFleet()); //�������� � ������������� ���� ��� ����� ���������
        }
        else
        {
            print($"Scaning for new target for Attacking fleet");

            _fleetShootingSystem.SetTarget(TakeTargetForAttackingFleet()); //�������� � ������������� ���� ��� ����� �����
        }
    }

    public GameObject TakeTargetForDefenceFleet()
    {
        GameObject go = null;
        if (_selfParametrPlanetMono.attackingFleet_LGO.Count > 0)
        {
            go = _selfParametrPlanetMono.attackingFleet_LGO[0];
            _stateFleet = FleetStateStruct.enumFleetState.Attack;
            _enemyStateFleet = go.GetComponent<FleetState>()._stateFleet = FleetStateStruct.enumFleetState.Attack;

        }
        if (go == null)
        {
            //TODO ������� �������� ������� �� ������� � ����������� �����
            RefundDefenceFleet();
        }
        return go;
    }

    public GameObject TakeTargetForAttackingFleet()
    {
        GameObject go = null;
        if (_distParametrPlanetMono.defFleetOnOrbitPlanet_GO != null)
        {
            go = _distParametrPlanetMono.defFleetOnOrbitPlanet_GO;
            _stateFleet = FleetStateStruct.enumFleetState.Idle;
            _enemyStateFleet = go.GetComponent<FleetState>()._stateFleet;
        }
        if (go == null)
        {
            //TODO ������� �������� ������� �� ������� � ����������� �����
            _stateFleet = FleetStateStruct.enumFleetState.OrbitAttack;
        }
        return go;
    }

    //������� ����� ������ �� �������
    private void RefundDefenceFleet()
    {
        _stateFleet = FleetStateStruct.enumFleetState.RefundDefenceFleet;
        _targetToMove = _selfParametrPlanetMono.SelfTransform.position;
    }

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

    private void SetStopBeforeForDescent()
    {
        _stopBefore = 2.5f;
    }

    //�������������� ��� �������� ������ � ������� �� �������.
    private void ScaleForDescent(float locStart, float locEnd)
    {
        scaleModifier = Mathf.Lerp(locStart, locEnd, elapsedTime / _timeToScale);
       
        elapsedTime += Time.deltaTime;

        transform.localScale = _originalScale * scaleModifier;
    }

    public bool CheckDistanceToAttack()
    {
        flagChkDistance = false;
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;

        if (_distanceSqr < _stopBefore)
        {
            flagChkDistance = true;
        }

        return flagChkDistance;
    }

    private void StartDefence()
    {
        _fleetManager.isDefenceFleet = true;

        _targetToMove = _distParametrPlanetMono._spawnPointDefenceFleet.position;

        //Vector3 tempPosition = transform.position;

        //tempPosition = _distParametrPlanetMono.SelfTransform.position;

        //transform.position = tempPosition;

        _stateFleet = FleetStateStruct.enumFleetState.OrbitDefence;

        speedMove = 3f;

        SetParamScale(0.8f, 0.002f, 1f, new Vector3(1f, 1f, 1f));
    }
    //��������� ���� �������� ���� ������ �� ������
    private void CallDefFleet()
    {
        //�������� �������� �� ������� ��� ��� ������ ����� ��������� ���������
        if (!CheckDistPlanetIsEnemy())
        {
           GameObject go = _distParametrPlanetMono.CallDefenderFleet(transform);

            if (go != null & _stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.FoundTarget;

                _distParametrPlanetMono.attackingFleet_LGO.Add(gameObject);
                count++;
            }
            else
            {
                if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
                {
                    _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;
                    
                    _fleetManager.CapturePlanet();
                    
                    print($"<color=aquamarine> DescentOnPlanet  1 {transform.name} || {count}</color>");
                    
                    _distParametrPlanetMono.RemoveToListAttackerFleet(gameObject);
                }
            }
        }
        else
        {
            //���� ������� ������������� � ���� �� ������ �����, �� ����� �������������� � ����� ������ �� �������
            if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;

                print($"<color=bisque> DescentOnPlanet  2 {transform.name} || {count}</color>");

            }
        }
    }

  


    //��������� ������� � ���������� ������ ����������� �� ������ �� ����� � ����� �������, ���� �������, �� ��������� � ��
    private bool CheckOtherAttackersFleetFromOnePlanetToJoin()
    {
        bool flagToMerg = false;
        //��� ���� ��������� ����
        if (_distParametrPlanetMono.attackingFleet_LGO.Count > 0)
        {
            for (int i = 0; i < _distParametrPlanetMono.attackingFleet_LGO.Count; i++)
            {
                //if (_distParametrPlanetMono.attackingFleet_LGO[i] == null)
                //{
                //    print($"_selfParametrPlanetMono = {_selfParametrPlanetMono.SelfTransform.name} ");
                //}

                //if (_distParametrPlanetMono.attackingFleet_LGO[i].activeInHierarchy)
                //{
                //    print($"<color=gold> A name {_distParametrPlanetMono.attackingFleet_LGO[i].name} </color>");
                //}
                //else
                //{
                //    print($"<color=aqua> !!!A name {_distParametrPlanetMono.attackingFleet_LGO[i].name} </color>");
                //}
                //��������� ������� ������ ���������� � ��� �� ������� ��� � ������ ����, ���� ����, �� �����������.
                if (_distParametrPlanetMono.attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        ._selfPlanetTransform //����� ��������� � ���������� �����
                        == 
                        _fleetManager
                        ._selfPlanetTransform //����� ��������� � ������������ �����
                        & 
                        _distParametrPlanetMono.attackingFleet_LGO[i] != gameObject) //������� �� ������ ��������� ���������� ����
                {
                    _distParametrPlanetMono
                        .attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetListDataFleet());

                    _distParametrPlanetMono.RemoveToListAttackerFleet(gameObject);

                    count++;
                    print($"<color=red> merg {transform.name} || {count}</color>");
                    flagToMerg = true;
                    _fleetManager.Destroy();

                }
            }
        }
        return flagToMerg;
    }


    private void Attack()
    {
        _fleetShootingSystem.Fire();
    }


    private void DescentOnPlanet()
    {
        _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;

        _distParametrPlanetMono.AddFleetToDefenceFleetOnPlanet(_fleetManager.GetListDataFleet());

        SetStopBeforeForDescent();
        print($"<color=aqua> DescentOnPlanet  3 {transform.name} || {count}</color>");
       

    }

}
