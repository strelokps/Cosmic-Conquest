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

    [ShowInInspector] private ParametrPlanet_mono _distParametrPlanetMono;
    [SerializeField] private float speedMove = 1f;

    [SerializeField] private float _stopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private float distanceToAttack;
    private float distanceToMoveForJoin;
    [SerializeField] private string _targetTransformName;

    private void Start()
    {
        _stopBefore = distanceToAttack = 16f; // дистанция остановки перед объектом для атаки
        distanceToMoveForJoin = 2f; //дистанция остановки перед объектом для join
        speedMove = 2.5f;

    }

    private void Update()
    {
        FleetStateMeth();
    }


    public void SetState(FleetStateStruct.enumFleetState locStateFleet,
        ParametrPlanet_mono locDistPlanetMono)
    {
        _fleetManager = transform.GetComponent<FleetManager>();

        _stateFleet = locStateFleet;
        _distParametrPlanetMono = locDistPlanetMono;
        _targetTransformName = locDistPlanetMono.SelfTransform.name + "    " + locDistPlanetMono.prop_ParentTransformFromPlanet.name; //test
    }

    private void FleetStateMeth()
    {
        switch (_stateFleet)
        {
            case FleetStateStruct.enumFleetState.StartForAttack:
                StartedAttack();
                _stopBefore = 36f;
                break;

            case FleetStateStruct.enumFleetState.MoveToOrbitAttack:
                Movement();
                if (CheckDistanceToAttack())
                {
                    _stateFleet = FleetStateStruct.enumFleetState.Idle;

                }
                break;
            
            case FleetStateStruct.enumFleetState.OrbitCallDefendefFleet:
                Movement();
                if (CheckDistanceToAttack())
                {
                    _stopBefore = 16f;
                    CallDefFleet();
                }

                break;
            
            case FleetStateStruct.enumFleetState.OrbitAttack:
               
                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsPlanet:
                Movement();

                break;
            
            case FleetStateStruct.enumFleetState.OrbitJoinToDefenderFleet:
                //print($"На нас напали, Милорд");
                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet:
                MovingTowardsToDefenceFleet();
                break;
            
            case FleetStateStruct.enumFleetState.StartForDefence:
                //CheckDistanceMovingTowardsPlanet();
                break;
            
            case FleetStateStruct.enumFleetState.OrbitDefence:
                Movement();
                CreateDefenceFleet();
                break;

            case FleetStateStruct.enumFleetState.Idle:
                break;
          
        }
    }

    //если планета враждебна, то добавляемся в список атакующего флота, если своя, то в дружественный  на подлете к планете 
    private void StartedAttack()
    {

        _targetToMove = _distParametrPlanetMono.SelfTransform.position;

        if (!CheckDistPlanetIsEnemy())
        {
            _distParametrPlanetMono.AddToListAttackerFleet(gameObject);
        }
        else
        {
            _distParametrPlanetMono.AddToListIncomeFriendlyFleet(gameObject);
        }

        _stateFleet = FleetStateStruct.enumFleetState.OrbitCallDefendefFleet;
    }

    private void CreateDefenceFleet()
    {
        //TODO если на защиту встает несколько флотов друг за другом сделать merg
        _distParametrPlanetMono.CreateDefenceFleet(gameObject, _fleetManager);
    }

    private void MovingTowardsToDefenceFleet()
    {
        Vector3 targetRotation = _targetTransform.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetRotation), 4f * Time.deltaTime);
        float angle = Quaternion.Angle(transform.rotation, _targetTransform.rotation);
        print($"Угол {angle}");

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
        bool flagCheckDistPlanetIsEnemy = false;

        if (_distParametrPlanetMono.CompareParents(_fleetManager.GetParentTransform()))
        {
            flagCheckDistPlanetIsEnemy = true;
        }

        return flagCheckDistPlanetIsEnemy;
    }

    public bool CheckDistanceToAttack()
    {
        var flagChkDistance = false;
        _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;
        if (_distanceSqr < _stopBefore)
            flagChkDistance = true;

        return flagChkDistance;
    }

    //атакующий флот вызывает флот защиты на орбиту
    private void CallDefFleet()
    {
        //проверка является ли планета все еще врагом через сравнение родителей
        if (!CheckDistPlanetIsEnemy())
        {
            //если у планеты есть флот защиты то будем атаковать его
            if (_distParametrPlanetMono.GetCountDefenceFleet() > 0)
            {
                //TODO атака уже существующего флота
                print($"Attack def fleet on orbit!!!!!");
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsDefenceFleet;
                _targetTransform = _distParametrPlanetMono.GetTransformDefenceFleet();
               
            }
            else
            {
                _distParametrPlanetMono.CallDefenderFleet(transform);
                //print($"Gen def fleet and Attack !!!!!");
                _stateFleet = FleetStateStruct.enumFleetState.MoveToOrbitAttack;
            }
        }
        else
        {
            _stateFleet = FleetStateStruct.enumFleetState.MoveToOrbitAttack;
        }
    }

    private void StartForDefence()
    {
    }

    /*
     *
     *  //проверка является ли планета все еще врагом через сравнение родителей
       if (!CheckDistPlanetIsEnemy())
       {
       //если у планеты есть флот защиты 
       if (_distParametrPlanetMono.GetCountDefenceFleet() > 0)
       {
       //TODO атака уже существующего флота
       print($"Attack def fleet on orbit!!!!!");
       _stateFleet = FleetStateStruct.enumFleetState.Attack;
       
       }
       else
       if (_distParametrPlanetMono.CallDefenderFleet(transform))
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
     */


    //private void CheckDistanceMovingTowardsPlanet()
    //{
    //    _distanceSqr = (_targetToMove - transform.position).sqrMagnitude;
    //    if (_distanceSqr < _stopBefore)
    //    {
    //        CheckOtherAttackersFleetFromOnePlanetToJoin();
    //        _fleetManager.JoinToDefenderFleet();
    //    }
    //}

    //private void ClearParam()
    //{
    //    _distanceSqr = 0f;
    //    _targetToMove = new Vector3();
    //}



    //private void PreAttack()
    //{
    //    _stateFleet = FleetStateStruct.enumFleetState.Movement;
    //}

    //private void FireToTarget()
    //{
    //    CheckPlanetHaveDefFleet();
    //}


    ////проверяем наличие у нападающих флотов соотвествие по transform планеты, если совпало, то добавляем к фл
    //private void CheckOtherAttackersFleetFromOnePlanetToJoin()
    //{
    //    //уже есть атакующий флот
    //    if (_distParametrPlanetMono._attackersFleet_LGO.Count > 0)
    //    {
    //        for (int i = 0; i < _distParametrPlanetMono._attackersFleet_LGO.Count; i++)
    //        {
    //            print($"dist: {_distParametrPlanetMono._attackersFleet_LGO[i].GetComponent<FleetManager>()._selfPlanetTransform}" +
    //                  $"self: {_fleetManager._selfPlanetTransform}");
    //            //проверяем наличие флотов вылетивших с той же планеты что и данный флот, если есть, то добавляемся.
    //            if (_distParametrPlanetMono._attackersFleet_LGO[i]
    //                    .GetComponent<FleetManager>()
    //                    ._selfPlanetTransform == _fleetManager._selfPlanetTransform)
    //            {
    //                _distParametrPlanetMono
    //                    ._attackersFleet_LGO[i]
    //                    .GetComponent<FleetManager>()
    //                    .MergFleets(_fleetManager.GetDataFleetList());
    //                _fleetManager.DestroyFleet();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        _distParametrPlanetMono.AddToListAttackerFleet(gameObject);
    //    }

    //}

    //private void CheckPlanetHaveDefFleet()
    //{
    //    if(_distParametrPlanetMono.ChangeOwnerPlanet(_fleetManager.GetMembersData(), _fleetManager.GetParentTransform()))
    //    {

    //    }

    //}

    //private bool CheckDefleetOnOrbit()
    //{
    //    var flagCheckEnemyDefFleet = false;

    //    //если у планеты нет флота защитника, то генерим новый
    //    if (_distParametrPlanetMono.CallDefenderFleet(transform))
    //    {
    //        _distParametrPlanetMono.CallDefenderFleet(transform);
    //        flagCheckEnemyDefFleet = true;
    //    }

    //    return flagCheckEnemyDefFleet;
    //}

    //private void MovingTowardsPlanet()
    //{
    //    _stopBefore = distanceToMoveForJoin;
    //    _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanet;
    //}


    //private void AddAttackerFleetToPlanetAttackOrDefFleet()
    //{
    //    if (!CheckDistPlanetIsEnemy())
    //    {
    //        //добавлемся в список атакующего флота
    //        _distParametrPlanetMono.AddToListAttackerFleet(gameObject);
    //    }
    //    else
    //    {
    //        //добавляемся в список дружественного флота
    //        _distParametrPlanetMono.AddToListIncomeFriendlyFleet(gameObject);
    //    }

    //}
}
