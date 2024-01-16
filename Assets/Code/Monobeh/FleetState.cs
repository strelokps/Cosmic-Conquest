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
    [SerializeField] private float _tempStopBefore;
    [SerializeField] private float  _distanceSqr;
    private FleetManager _fleetManager;

    private float _stopDistForCallDefendefFleet;
    private float distanceToMoveForJoin;
    [SerializeField] private string _targetTransformName;

    //test



    private void Update()
    {

        FleetStateMeth();

    }


    public void SetState(FleetStateStruct.enumFleetState locStateFleet, ParametrPlanet_mono locDistPlanetMono)
    {
        distanceToMoveForJoin = 2f; //дистанция остановки перед объектом для join
        speedMove = 2.5f;

        _stopBefore = _tempStopBefore = 16f; // дистанция остановки перед объектом для атаки
        _fleetManager = transform.GetComponent<FleetManager>();

        _stateFleet = locStateFleet;
        _distParametrPlanetMono = locDistPlanetMono;
        //вычисляем расстоние от вражеской планеты до точки вызова флота защитны с планеты атакующим флотом, в зависимости от 
        //точки защиты и точки атаки
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
                _stateFleet = FleetStateStruct.enumFleetState.Idle;
                break;
            
            case FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent:
                Movement();
                break;
            
            case FleetStateStruct.enumFleetState.OrbitJoinToDefenderFleet:
                //print($"На нас напали, Милорд");
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

    //если планета враждебна, то добавляемся в список атакующего флота, если своя, то в дружественный  на подлете к планете 
   


   

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
            _distParametrPlanetMono.CallDefenderFleet(transform);
        }
        else
        {
            //если планета дружественная и флот на орбите атаки, то летим присоединяться к флоту зашиты на планете
            if (_stateFleet == FleetStateStruct.enumFleetState.OrbitAttack)
            {
                _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent;
            }
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
       MovingTowardsPlanetForDecent();
       }
       }
       else
       {
       MovingTowardsPlanetForDecent();
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

    //private void MovingTowardsPlanetForDecent()
    //{
    //    _stopBefore = distanceToMoveForJoin;
    //    _stateFleet = FleetStateStruct.enumFleetState.MovingTowardsPlanetForDecent;
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
