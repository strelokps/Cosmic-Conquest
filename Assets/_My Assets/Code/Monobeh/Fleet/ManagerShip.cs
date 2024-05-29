using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ManagerShip : MonoBehaviour
{
    [ShowInInspector] public List<DataShip> shipList = new List<DataShip>();
    [ShowInInspector] private ShipType.eShipType _shipType;
    [ShowInInspector] private GameObject _prefabBullet;

    public void Init()
    {
        if (gameObject.tag.Contains("Light"))
        {
            _shipType = ShipType.eShipType.light;
            print($"Light");
        }
        else if (gameObject.tag.Contains("Medium"))
        {
            _shipType = ShipType.eShipType.medium;
            print($"Medium");
        }
        else if (gameObject.tag.Contains("Heavy"))
        {
            _shipType = ShipType.eShipType.heavy;
            print($"Heavy");
        }

        foreach (Transform child in gameObject.transform) 
        {
            if (child.tag.Contains("Bullet"))
            {
                _prefabBullet = child.gameObject;
                print($"<color=green> name bullet {_shipType} {gameObject.name} || {child.gameObject.name}");
            }
        }
    }
    
}
