using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataBullet
{
    public List<DataShip> damageBullet;
    public float lifeTimeBullet;
    public float speedBullet;
    public float rateFireBullet;
    public ShipType.eShipType typeShootingShip;


    public DataBullet GetDataBullet(ShipType.eShipType locShipType)
    {
        DataBullet _bullet = new DataBullet();

        if (locShipType == ShipType.eShipType.light)
        {
            _bullet.speedBullet = 10f;
            _bullet.rateFireBullet = Random.Range(0.3f, 0.6f);
            _bullet.lifeTimeBullet = 1.5f;
            typeShootingShip = ShipType.eShipType.light;

        }
        else if (locShipType == ShipType.eShipType.medium)
        {
            _bullet.speedBullet = 10f;
            _bullet.rateFireBullet = Random.Range(0.6f, 1.0f);
            _bullet.lifeTimeBullet = 1.5f;
            typeShootingShip = ShipType.eShipType.medium;
        }
        else if (locShipType == ShipType.eShipType.heavy)
        {
            _bullet.speedBullet = 10f;
            _bullet.rateFireBullet = Random.Range(1.0f, 1.5f);
            _bullet.lifeTimeBullet = 1.5f;
            typeShootingShip = ShipType.eShipType.heavy;
        }
        return _bullet;
    }

}
