using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShootingSystem : MonoBehaviour
{
    [Header("Target")]
    private GameObject _targetFleet;
    private Vector3 _directShooting;            //����������� ��������
    private float _directShootingDistance;      //��������� ��� ������� ����� �������

    [Header("Shooting")] 
    private DataBullet _dataBullet;       
    private GameObject _prefabBullet;

    [Header("Sefl fleet")]
    private List<DataShip> _selfFleet;
    private FleetState _fleetStateSelfFleet = new FleetState();
    private FleetManager _selfFleetManager = new FleetManager();


    public void InitShootingSystem(GameObject locPrefabBullet, DataBullet locDataBullet, List<DataShip> locSelfFleet)
    {
        _dataBullet = locDataBullet;
        _prefabBullet = locPrefabBullet;
        _selfFleet = locSelfFleet;
        _fleetStateSelfFleet = GetComponent<FleetState>();
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
        print($"<color=magenta>���, ��� ���������� �� ���� {dist}  � ��� ����� ����� ���� {_directShootingDistance}</color>");
    }

  

    public void Fire()
    {

        if (_targetFleet == null)
        {
            print($"<color=magenta> ����� ����� ���� </color> ");
            if (_selfFleetManager)
            {
                print("����� ���� ��� def fleet from shooting system   " +
                      _selfFleetManager._selfPlanetTransform.name);

                _fleetStateSelfFleet._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
            }
            else
            {
                print("����� ���� ��� attacking fleet from shooting system   " +
                      _selfFleetManager._selfPlanetTransform.name);

                _fleetStateSelfFleet._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;
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
}