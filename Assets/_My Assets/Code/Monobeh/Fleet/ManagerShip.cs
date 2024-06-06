using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ManagerShip : MonoBehaviour
{
    [ShowInInspector] public List<DataShip> shipList = new List<DataShip>();
    [ShowInInspector] private ShipType.eShipType _shipType;
    [ShowInInspector] private GameObject _prefabBullet;
    [ShowInInspector] private GameObject _pointToFire;
    //[ShowInInspector] private GameObject _pointToHit;
    [ShowInInspector] public List<Transform> _listPointToHit;

    private BoxCollider[] _colladerPointToHit;

    private void OnDisable()
    {
        
    }

    public void Init()
    {
        shipList = new List<DataShip>();
        _colladerPointToHit = gameObject.GetComponents<BoxCollider>();
        _listPointToHit = new List<Transform>();

        if (gameObject.tag.Contains("Light"))
        {
            _shipType = ShipType.eShipType.light;
        }
        else if (gameObject.tag.Contains("Medium"))
        {
            _shipType = ShipType.eShipType.medium;
        }
        else if (gameObject.tag.Contains("Heavy"))
        {
            _shipType = ShipType.eShipType.heavy;
        }

        foreach (Transform child in gameObject.transform) 
        {
            if (child.tag.Contains("prefBullet"))
            {
                _prefabBullet = child.gameObject;
            }
            else if (child.tag.Contains("PointToFire"))
            {
                _pointToFire = child.gameObject;
            }
            else if ((child.tag.Contains("PointToHit")))
            {
                _listPointToHit.Add(child.transform);
            }
        }


        if (_listPointToHit.Count == _colladerPointToHit.Length)
        {
            for (int i = 0; i < _colladerPointToHit.Length; i++)
            {
                _colladerPointToHit[i].center = _listPointToHit[i].localPosition;
            }
        }
    }


    public List<Transform> TakeListTransformsPointToHit()
    {
        Init();
        return _listPointToHit;
    }
}
