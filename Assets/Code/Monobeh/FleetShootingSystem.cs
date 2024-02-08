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


    public void InitShootingSystem(GameObject locPrefabBullet, DataBullet locDataBullet, List<DataShip> locSelfFleet)
    {
        _dataBullet = locDataBullet;
        _prefabBullet = locPrefabBullet;
        _selfFleet = locSelfFleet;
    }

    public void SetTarget(GameObject locTarget)
    {
        _targetFleet = locTarget;
        CalculationDirectionAndDistance();
    }

    private void CalculationDirectionAndDistance()
    {
        _directShooting = (_targetFleet.transform.position - transform.position).normalized;
        var dist = (_targetFleet.transform.position - transform.position).sqrMagnitude;
        _directShootingDistance = dist/_dataBullet.speedBullet;
        print($"{transform.name}");
        print($"<color=magenta>���, ��� ���������� �� ���� {_directShootingDistance}  � ��� ����� ����� ���� {_directShootingDistance}</color>");
        print("***************");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        //test
        _targetFleet.GetComponent<FleetManager>().TakeDamageFleet(_selfFleet);
    }

}
