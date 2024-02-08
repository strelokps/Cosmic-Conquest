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
        _ship.armorShip = 10f;
        _ship.maxShieldShip = 5f;
        _ship.shieldShip = 5f;
        _ship.regenShield = 0.1f;
        _ship.speedShip = 4.5f;
        _ship.coastShip = 5;
        _ship.timeToBuild = 0.2f;
        _ship.typeShip = eShipType.light;
        _ship.typeShipIncreasedDamage = eShipType.heavy;
        _ship.increasedDamage = 1.3f;

        SetShipToBuildInShipyard();
        print($"������� ��������� �� �����");
    }


    [Button("Medium")]
    private void BuildShipMedium()
    {
        _ship = new DataShip();
        _ship.damageShipMin = 3;
        _ship.damageShipMax = 7;
        _ship.armorShip = 20;
        _ship.maxShieldShip = 10;
        _ship.shieldShip = 10;
        _ship.regenShield = 2;
        _ship.speedShip = 4f;
        _ship.coastShip = 10;
        _ship.timeToBuild = 5f;
        _ship.typeShip = eShipType.medium;
        _ship.typeShipIncreasedDamage = eShipType.light;
        _ship.increasedDamage = 1.3f;

        SetShipToBuildInShipyard();
        print($"������� ��������� �� �����");
    }


    [Button("Heavy")]
    private void BuildShipHeavy()
    {
        _ship = new DataShip();
        _ship.damageShipMin = 5;
        _ship.damageShipMax = 13;
        _ship.armorShip = 50;
        _ship.maxShieldShip = 20;
        _ship.shieldShip = 20;
        _ship.regenShield = 5;
        _ship.speedShip = 3.5f;
        _ship.coastShip = 15;
        _ship.timeToBuild = 8f;
        _ship.typeShip = eShipType.heavy;
        _ship.typeShipIncreasedDamage = eShipType.medium;
        _ship.increasedDamage = 1.3f;

        SetShipToBuildInShipyard();
        print($"������� ��������� �� �����");
    }

    private void SetShipToBuildInShipyard()
    {
        _shipyard.BuildShipInShipyard(_ship);
        _ship = new DataShip();
    }
}