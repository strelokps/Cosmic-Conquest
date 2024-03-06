using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using UnityEngine;

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



    private void Update()
    {
        FleetStateMeth();
    }


    public void SetState
        (FleetStateStruct.enumFleetState locStateFleet, ParametrPlanet_mono locDistPlanetMono, ParametrPlanet_mono selfPlanetMono)
    {
        speedMove = 5.5f;

        _stopBefore = _tempStopBefore = 16f; // ��������� ��������� ����� �������� ��� �����

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
                SetParamScale(0.8f, 1f, 0.002f, transform.localScale);
                CheckOtherAttackersFleetFromOnePlanetToJoin();
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

                case FleetStateStruct.enumFleetState.FoundTargetForAttackingFleet:
                _fleetShootingSystem.SetTarget(TakeTargetForAttackingFleet()); //�������� � ������������� ���� ��� ����� �����
                break;
            
            case FleetStateStruct.enumFleetState.FoundTargetForDefenceFleet:
                //print($"�� ��� ������, ������");
                _fleetShootingSystem.SetTarget(TakeTargetForDefenceFleet()); //�������� � ������������� ���� ��� ����� ���������

                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet:
                break;
            
            case FleetStateStruct.enumFleetState.StartForDefence:
                StartDefence();
                break;
            
            case FleetStateStruct.enumFleetState.OrbitDefence:
                ScaleForDescent(_startScale, _endScale);
                Movement();
                if (scaleModifier >= 1)
                {
                    _stateFleet = FleetStateStruct.enumFleetState.FoundTargetForDefenceFleet;
                }
                break;

            case FleetStateStruct.enumFleetState.Idle:
                break;
          
        }
    }


    private GameObject TakeTargetForDefenceFleet()
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

    private GameObject TakeTargetForAttackingFleet()
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
            RefundDefenceFleet();
        }
        return go;
    }

    //������� ����� ������ �� �������
    private void RefundDefenceFleet()
    {
        _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;
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
        _stopBefore = 1.5f;
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
            flagChkDistance = true;

        return flagChkDistance;
    }

    private void StartDefence()
    {
        _targetToMove = _distParametrPlanetMono._spawnPointDefenceFleet.position;
        Vector3 tempPosition = transform.position;
        tempPosition = _distParametrPlanetMono.SelfTransform.position;
        transform.position = tempPosition;
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
                _stateFleet = FleetStateStruct.enumFleetState.FoundTargetForAttackingFleet;

                _distParametrPlanetMono.AddToListAttackerFleet(gameObject);


                print($"<color=red> � �����!!! </color>");
            }
            else
            {
                if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
                {
                    _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;
                    print($"<color=aqua>�� �������!!!</color>");
                    _fleetManager.CapturePlanet();
                    SetStopBeforeForDescent();
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
                SetStopBeforeForDescent();
            }
        }
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
                        ._selfPlanetTransform == _fleetManager._selfPlanetTransform 
                        & _distParametrPlanetMono.attackingFleet_LGO[i] != gameObject)
                {
                    _distParametrPlanetMono
                        .attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetListDataFleet());
                    _fleetManager.DestroyAttackingFleet();
                    return;
                }
            }
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
            _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;
        }
    }

    private void Attack()
    {
        _fleetShootingSystem.Fire();
    }


    private void Shooting()
    {

    }


}
