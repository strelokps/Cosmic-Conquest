using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject.SpaceFighter;

[RequireComponent(typeof(HealthSystem))]
public class ShipManager : MonoBehaviour
{
    private FleetState _fleetState;
    private FleetManager _fleetManager;

    [ShowInInspector] private List<DataShip> _shipsList = new List<DataShip>();
    [ShowInInspector] private ShipType.eShipType _shipType;
    [ShowInInspector] public List<Transform> _listPointToHit;

    private BoxCollider[] _colladerPointToHit;
    [ShowInInspector]
    [Header("Take target and shot")]
    private TakeTarget _takeTarget = new TakeTarget();

    public bool _flagMayShot;

    private GameObject target;

    private float rateOfFire = 1;
    private float tempRateOfFire = 0;

    [Header("UI")]

    [ShowInInspector] public Image _shieldImage;
    [ShowInInspector] public Image _armorImage;

    [Header("Time")]
    private float _timer;
    private float _tempTimer;

    [Header("Health | Damage")]
    private HealthSystem _healthSystemShips;
    private float _armor;
    private float _armorMax;
    private float _shield;
    private float _shieldMax;
    private DataBullet _dataBullet;
    [ShowInInspector] private GameObject _prefabBullet;
    [ShowInInspector] private GameObject _pointToFire;



    private void OnDisable()
    {
        print($"<color=green> Dis off  </color>");
        if (_fleetManager != null)
            print($"{_fleetManager.transform.name}");
    }

    private void OnEnable()
    {
        print($"<color=blue> Dis On </color>");
        if (_fleetManager != null)
            print($"{_fleetManager.transform.name}");
    }

    private void FixedUpdate()
    {
        if (_flagMayShot)
        {
            tempRateOfFire += Time.deltaTime;
            if (tempRateOfFire >= _dataBullet.rateFireBullet)
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

            CallRegenShield();

    }

    public void Init(List<DataShip> locListTypeShips, FleetManager locFleetManager)
    {
        _fleetManager = new FleetManager();
        _fleetManager = locFleetManager;

        _healthSystemShips = new HealthSystem();
        _healthSystemShips = GetComponent<HealthSystem>();
        _healthSystemShips.InitHealthSystem(this, _shipsList);

        _dataBullet = new DataBullet();

        _shipsList = new List<DataShip>();
        MergeShips(locListTypeShips);

        _fleetState = new FleetState();
        
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

        _dataBullet = _dataBullet.GetDataBullet(_shipType);

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

        //centering the collider position of the hit point position
        if (_listPointToHit.Count == _colladerPointToHit.Length)
        {
            for (int i = 0; i < _colladerPointToHit.Length; i++)
            {
                _colladerPointToHit[i].center = _listPointToHit[i].localPosition;
            }
        }
        _timer = 1f;
        _tempTimer = 0;
    }


    public List<Transform> TakeListTransformsPointToHit()
    {
       // print($"<color=green> Init manager ships </color>");
        return _listPointToHit;
    }

    //************************ shooting ************************
    #region MyRegion 

    public void PushFire(GameObject locTarget, FleetState locFleetState)
    {
        target = locTarget;
        _flagMayShot = true;
        _fleetState = locFleetState;
    }

    private void Fire()
    {
        Vector3 point = _takeTarget.TakeTargetForAttackingFleet( ref _flagMayShot, target);

        if (!_flagMayShot)
            return;

        _pointToFire.transform.LookAt(point);

        GameObject bullet = Instantiate(_prefabBullet, _pointToFire.transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDataBullet(_dataBullet, GetShipsList());
        bullet.SetActive(true);
        bullet.transform.LookAt(point);
    }

    #endregion
    //************************ shooting ************************

    public void MergeShips(List<DataShip> locListTypeShips)
    {
        _shipsList.AddRange(locListTypeShips);
        CalcInInitArmorAndShield(); //calculation of armor and shields at the beginning
    }

    private void CallRegenShield()
    {
        _tempTimer += Time.deltaTime;

        if (_tempTimer > _timer)
        {
            _tempTimer = 0;

            _healthSystemShips.RegenerationShield( _shipsList);
        }
    }

    //calculation of armor and shields at the beginning
    private void CalcInInitArmorAndShield()
    {

        DisplayArmorAndShield();
        _armorMax = _armor;
        _shieldMax = _shield;
    }

    public void DisplayArmorAndShield()
    {
        _healthSystemShips.CalcArmorAndShield(ref _shield, ref _armor, _shipsList);
        _shieldImage.fillAmount = _shield / _shieldMax;
        _armorImage.fillAmount = _armor / _armorMax;
    }

    public List<DataShip> GetShipsList()
    {
        return _shipsList;
    }

    public void JoinToDefenderFleet_ShipManager()
    {
        _healthSystemShips.SetMaxArmorAndShield(_shipsList);
    }

    public void OnOffShipGO(bool flagOnOff)
    {
        gameObject.SetActive(flagOnOff);
    }
}
