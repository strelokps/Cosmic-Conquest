using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShootingSystem : MonoBehaviour
{
    [Header("Target")]
    private Vector3 _directShooting;            //направление стрельбы
    private float _directShootingDistance;      //дистанция для расчета жизни снаряда

    [Header("Shooting")]
    private float _projectileSpeed;             //Скорость снаряда
    private float _projectileLifeTime;          //время жизни снаряда, должно расчитываться исходя из дистанции до цели
    private float _fireRate;              //периодичность стрельбы
    private GameObject _projectile;



    // Update is called once per frame
    void Update()
    {
        
    }


}
