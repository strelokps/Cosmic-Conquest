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

    [Header("Take target and shot")]
    private TakeTarget _takeTarget = new TakeTarget();

    public bool _flagMayShot;

    private GameObject target;
    private FleetState _fleetState;

    private float rateOfFire = 1;
    private float tempRateOfFire = 0;

    //Test
    [ShowInInspector] private string _name;
    [ShowInInspector] private string _nameTraget;


    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        if (_flagMayShot)
        {
            tempRateOfFire += Time.deltaTime;
            if (tempRateOfFire >= rateOfFire)
            {
                Fire();
                tempRateOfFire = 0;
            }
        }

        if (_fleetState != null)
        {
           // print($"Cap, no target! {_name}  target {_nameTraget}");
            if (_fleetState != null)
                _fleetState._stateFleet = FleetStateStruct.enumFleetState.FoundTarget;

        }
        else
        {
            //print($"<color=red> _fleetState = null {_name}  target {_nameTraget} </color>");
        }

    }

    public void Init()
    {
        _fleetState = new FleetState();

        _colladerPointToHit = gameObject.GetComponents<BoxCollider>();
        _listPointToHit = new List<Transform>();

        if (gameObject.tag.Contains("Light"))
        {
            _shipType = ShipType.eShipType.light;
            rateOfFire = Random.Range(0.3f, 0.7f);
        }
        else if (gameObject.tag.Contains("Medium"))
        {
            _shipType = ShipType.eShipType.medium;
            rateOfFire = Random.Range(0.6f, 1f);

        }
        else if (gameObject.tag.Contains("Heavy"))
        {
            _shipType = ShipType.eShipType.heavy;
            rateOfFire = Random.Range(0.8f, 1.4f);

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
       // print($"<color=green> Init manager ships </color>");
        return _listPointToHit;
    }

    public void PushFire(GameObject locTarget, FleetState locFleetState)
    {
        target = locTarget;
        _flagMayShot = true;
        _fleetState = locFleetState;

        //test
        _name = _fleetState.gameObject.transform.name;
        _nameTraget = target.transform.name;
    }

    private void Fire()
    {
        Vector3 point = _takeTarget.TakeTargetForAttackingFleet( ref _flagMayShot, target);
        
        _pointToFire.transform.LookAt(point);

        GameObject bullet = Instantiate(_prefabBullet, _pointToFire.transform.position, Quaternion.identity);
        bullet.SetActive(true);
        bullet.transform.LookAt(point);
    }

}
