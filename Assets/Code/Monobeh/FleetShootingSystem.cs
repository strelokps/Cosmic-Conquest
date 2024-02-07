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


    public void InitShootingSystem(GameObject locPrefabBullet, DataBullet locDataBullet)
    {
        _dataBullet = locDataBullet;
        _prefabBullet = locPrefabBullet;
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
        print($"<color=magenta>���, ��� ���������� �� ���� {_directShootingDistance}  � ��� ����� ����� ���� {_directShootingDistance}</color>");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
