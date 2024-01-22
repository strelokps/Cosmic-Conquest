using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShootingSystem : MonoBehaviour
{
    [Header("Target")]
    private Vector3 _directShooting;            //����������� ��������
    private float _directShootingDistance;      //��������� ��� ������� ����� �������

    [Header("Shooting")]
    private float _projectileSpeed;             //�������� �������
    private float _projectileLifeTime;          //����� ����� �������, ������ ������������� ������ �� ��������� �� ����
    private float _fireRate;              //������������� ��������
    private GameObject _projectile;



    // Update is called once per frame
    void Update()
    {
        
    }


}
