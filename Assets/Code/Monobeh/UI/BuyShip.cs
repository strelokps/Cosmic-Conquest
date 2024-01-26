using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject.Asteroids;
using static ShipType;

public class BuyShip : MonoBehaviour
{

    private DataShip _ship;
    private Shipyard _shipyard;


    public void InitBuyShip(Shipyard locShipyard)
    {
        _ship = new DataShip();
        _shipyard = locShipyard;

    }

    [Button("Light")]
    private void BuildShipLight()
    {
        _ship = new DataShip();
        _ship.damageShipMin = 1f;
        _ship.damageShipMax = 3f;
        _ship.armorShip = 10;
        _ship.shieldShip = 5;
        _ship.regenShield = 1f;
        _ship.speedShip = 4.5f;
        _ship.coastShip = 5;
        _ship.timeToBuild = 2f;
        _ship.typeShip = eShipType.light;

        SetShipToBuildInShipyard();
        print($"Корабль отправлен на верфь");
    }


    [Button("Medium")]
    private void BuildShipMedium()
    {
        _ship = new DataShip();
        _ship.damageShipMin = 3f;
        _ship.damageShipMax = 7f;
        _ship.armorShip = 20;
        _ship.shieldShip = 10;
        _ship.regenShield = 2f;
        _ship.speedShip = 4f;
        _ship.coastShip = 10;
        _ship.timeToBuild = 5f;
        _ship.typeShip = eShipType.medium;

        SetShipToBuildInShipyard();
        print($"Корабль отправлен на верфь");
    }

    private void SetShipToBuildInShipyard()
    {
        _shipyard.BuildShipInShipyard(_ship);
        _ship = new DataShip();
    }
}
