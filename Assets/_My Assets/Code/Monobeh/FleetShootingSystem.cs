using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShootingSystem : MonoBehaviour
{
    [Header("Target")]
    private GameObject _targetFleet;
    private Vector3 _directShooting;            //направление стрельбы
    private float _directShootingDistance;      //дистанция для расчета жизни снаряда

    [Header("Shooting")] 
    private DataBullet _dataBullet;       
    private GameObject _prefabBullet;

    [Header("Sefl fleet")]
    private List<DataShip> _selfFleet;
    private FleetState _selfFleetStateFleet = new FleetState();
    private FleetManager _selfFleetManager = new FleetManager();


    public void InitShootingSystem(GameObject locPrefabBullet, DataBullet locDataBullet, List<DataShip> locSelfFleet)
    {
        _dataBullet = locDataBullet;
        _prefabBullet = locPrefabBullet;
        _selfFleet = locSelfFleet;
        _selfFleetStateFleet = GetComponent<FleetState>();
        _selfFleetManager = GetComponent<FleetManager>();
    }

    public void SetTarget(GameObject locTarget)
    {
        if (locTarget == null)
            return;
        _targetFleet = locTarget;
        CalculationDirectionAndDistance();
    }

    private void CalculationDirectionAndDistance()
    {
        _directShooting = (_targetFleet.transform.position - transform.position).normalized;
        var dist = (_targetFleet.transform.position - transform.position).sqrMagnitude;
        _directShootingDistance = dist / _dataBullet.speedBullet;
        print($"{transform.name}");
        print($"<color=magenta>кэп, вот расстояние до цели {dist}  а это время жизни пули {_directShootingDistance}</color>");
    }

  

    public void Fire()
    {
        CheckAndSetForTargetTypeShip();
        if (_targetFleet == null)
        {
            print($"<color=magenta> Нужна новая цель </color> ");
            if (_selfFleetManager)
            {
                print("поиск цели для def fleet from shooting system   " +
                      _selfFleetManager._selfPlanetTransform.name);

                _selfFleetStateFleet._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
            }
            else
            {
                print("поиск цели для attacking fleet from shooting system   " +
                      _selfFleetManager._selfPlanetTransform.name);

                _selfFleetStateFleet._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
            }
        }
        //fleet shoot
        else
        {
            //test
            _targetFleet.GetComponent<FleetManager>().TakeDamageFleet(_selfFleet);

            var fleet = _targetFleet.GetComponent<FleetManager>().GetListDataFleet();



            if (fleet.Count == 0)
            {
                _targetFleet.GetComponent<FleetManager>().Destroy();
                _targetFleet = null;
                GetComponent<FleetState>()._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
            }
        }
    }

    private void CheckAndSetForTargetTypeShip()
    {
        var n = Enum.GetValues(typeof(ShipType.eShipType)).Length;
        int count = 0;
        
        int[] ships = new int[n] ;

        //for (int i = 0; i < n - 1; i++)
        //{
        //    ships[i] = 0;
        //}

        var locHasActivGOTypeShip = _selfFleetManager._objectShipInPrefabFleet;

        foreach (ShipType.eShipType shipType in Enum.GetValues(typeof(ShipType.eShipType)))
        {
            ships[count] = 0;
            if (locHasActivGOTypeShip[shipType].activeInHierarchy)
            {
                ships[count] = 1;
                print($"<color=green> type ship: {locHasActivGOTypeShip[shipType]} </color>");
            }

        }


    }
}
