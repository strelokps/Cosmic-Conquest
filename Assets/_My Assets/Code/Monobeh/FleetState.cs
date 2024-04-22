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
    [ShowInInspector] public GameObject _targetForDefenceFleet;
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


    private bool flagChkDistance; // флаг для проверки дистанции
    //test

    private int count = 0;

    private void Update()
    {
        FleetStateMeth();
    }


    public void SetState
        (FleetStateStruct.enumFleetState locStateFleet, ParametrPlanet_mono locDistPlanetMono, ParametrPlanet_mono selfPlanetMono)
    {
        speedMove = 5.5f;

        _stopBefore = _tempStopBefore = 30f; // дистанция остановки перед объектом для атаки

        _fleetManager = transform.GetComponent<FleetManager>();
        _fleetShootingSystem = GetComponent<FleetShootingSystem>();

        _stateFleet = locStateFleet;

        _distParametrPlanetMono = locDistPlanetMono;
        _selfParametrPlanetMono = selfPlanetMono;

        //вычисляем расстояние от вражеской планеты до точки вызова флота защитны с планеты атакующим флотом, в зависимости от 
        //точки защиты и точки атаки. 
        //расстояние между орбитой вызова деф флота и орбитой атаки равна растоянию между планетой и точкой спауна деф флота
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
                CallDefFleet();
                _stopBefore = _stopDistForCallDefendefFleet;
                _stateFleet = FleetStateStruct.enumFleetState.OrbitCallDefendefFleet;
                _distParametrPlanetMono.attackingFleet_LGO.Add(gameObject); //добавляемся в список атакующих планету

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
                print($"<color=yellow> idle {this.name}</color>");
                break;
          
        }
    }


    private void FoundTarget()
    {
        if (_fleetManager.isDefenceFleet)
        {
            print($"Scaning for new target for Def fleet");

            _fleetShootingSystem.SetTarget(TakeTargetForDefenceFleet()); //выбираем и устанавливаем цель для флота защитника
        }
        else
        {
            print($"Scaning for new target for Attacking fleet");

            _fleetShootingSystem.SetTarget(TakeTargetForAttackingFleet()); //выбираем и устанавливаем цель для флота атаки
        }
    }

    public GameObject TakeTargetForDefenceFleet()
    {
        _targetForDefenceFleet = null;
        if (_selfParametrPlanetMono.attackingFleet_LGO.Count > 0)
        {
            _targetForDefenceFleet = _selfParametrPlanetMono.attackingFleet_LGO[0];
            _stateFleet = FleetStateStruct.enumFleetState.Attack;
            _targetForDefenceFleet.GetComponent<FleetState>()._stateFleet = FleetStateStruct.enumFleetState.Attack;

        }
        if (_targetForDefenceFleet == null)
        {
            //TODO сделать анимацию посадки на планету с уничтожеием флота
            RefundDefenceFleet();
        }
        return _targetForDefenceFleet;
    }

    public GameObject TakeTargetForAttackingFleet()
    {
        GameObject go = null;
        if (_distParametrPlanetMono.defFleetOnOrbitPlanet_GO != null)
        {
            go = _distParametrPlanetMono.defFleetOnOrbitPlanet_GO;
            GameObject targetDefFleet = go.GetComponent<FleetState>()._targetForDefenceFleet;

         
                _stateFleet = FleetStateStruct.enumFleetState.Idle;
            
            if (targetDefFleet != null & targetDefFleet != this)
            {
                print($"target 1 {this.name}");
                _stateFleet = FleetStateStruct.enumFleetState.Attack;

            }
            //_enemyStateFleet = go.GetComponent<FleetState>()._stateFleet;
        }
        if (go == null)
        {
            //TODO сделать анимацию посадки на планету с уничтожеием флота
            _stateFleet = FleetStateStruct.enumFleetState.OrbitAttack;
        }
        return go;
    }

    //возврат флота защиты на планету
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
    /// true если паренты флота и планеты цели одинаковые
    /// </summary>
    /// <returns></returns>
    //Проверяем явлется ли планета дружественной
    private bool CheckDistPlanetIsEnemy()
    {
        bool flagCheckDistPlanetIsEnemy = _distParametrPlanetMono.CompareParents(_fleetManager.GetParentTransform());

        return flagCheckDistPlanetIsEnemy;
    }

    private void SetStopBeforeForDescent()
    {
        _stopBefore = 2.5f;
    }

    //маштабирование для анимации взлета и посадки на планету.
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
    //атакующий флот вызывает флот защиты на орбиту
    private void CallDefFleet()
    {
        //проверка является ли планета все еще врагом через сравнение родителей
        if (!CheckDistPlanetIsEnemy())
        {
           GameObject go = _distParametrPlanetMono.CallDefenderFleet(transform);

            if (go != null & _stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.FoundTarget;

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
            //если планета дружественная и флот на орбите атаки, то летим присоединяться к флоту зашиты на планете
            if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDescent;

                print($"<color=bisque> DescentOnPlanet  2 {transform.name} || {count}</color>");

            }
        }
    }

  


    //проверяем наличие у нападающих флотов соотвествие по вылету из одной и тойже планеты, если совпало, то добавляем к фл
    private bool CheckOtherAttackersFleetFromOnePlanetToJoin()
    {
        bool flagToMerg = false;
        //уже есть атакующий флот
        if (_distParametrPlanetMono.attackingFleet_LGO.Count > 0)
        {
            for (int i = 0; i < _distParametrPlanetMono.attackingFleet_LGO.Count; i++)
            {
                //проверяем наличие флотов вылетивших с той же планеты что и данный флот, если есть, то добавляемся.
                if (_distParametrPlanetMono.attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        ._selfPlanetTransform //берем трансформ с атакующего флота
                        == 
                        _fleetManager
                        ._selfPlanetTransform //берем тарнсформ с подлетающего флота
                        & 
                        _distParametrPlanetMono.attackingFleet_LGO[i] != gameObject) //убираем из списка сравнения подлетющей флот
                {
                    _distParametrPlanetMono
                        .attackingFleet_LGO[i]
                        .GetComponent<FleetManager>()
                        .MergFleets(_fleetManager.GetListDataFleet());

                    _distParametrPlanetMono.RemoveToListAttackerFleet(gameObject);

                    count++;

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
    }

}
