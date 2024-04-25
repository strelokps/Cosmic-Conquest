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
    private DataShipsSO _dataShipsSO;


    public void InitBuyShip(Shipyard locShipyard)
    {
        _ship = new DataShip();
        _shipyard = locShipyard;
        _dataShipsSO = Resources.Load<DataShipsSO>("Fleet\\DataShips_SO");

    }

    [Button("Light")]
    public void BuildShipLight()
    {
        _ship = _dataShipsSO.GetDataShipLight();

        SetShipToBuildInShipyard();
    }


    [Button("Medium")]
    public void BuildShipMedium()
    {
        _ship = _dataShipsSO.GetDataShipMedium();

        SetShipToBuildInShipyard();
    }


    [Button("Heavy")]
    public void BuildShipHeavy()
    {
        _ship = _dataShipsSO.GetDataShipHeavy
            ();

        SetShipToBuildInShipyard();
    }

    private void SetShipToBuildInShipyard()
    {
        _shipyard.BuildShipInShipyard(_ship);
        _ship = new DataShip();
    }
}
