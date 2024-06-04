using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


[RequireComponent(typeof(FleetState))]
[RequireComponent(typeof(FleetShootingSystem))]

public class FleetManager : MonoBehaviour
{

    [SerializeField] private Transform _selfTransform;
    [SerializeField] private Transform _pointToFire;
    public Transform _selfPlanetTransform;
    public bool isDefenceFleet = false;

    [ShowInInspector] private GameObject prefabBullet;

    [SerializeField] private Image _imageFleet_L;
    [SerializeField] private Image _imageFleet_R;
    [SerializeField] private TMP_Text _textNumShipInFleet;
    [SerializeField] private TMP_Text _attackShipInFleetText;
    [SerializeField] private TMP_Text _defenceShipInFleetText;
    private int _numShipInFleet;
    [SerializeField] private int _attackFleet;
    [SerializeField] private int _armorFleet;
    
    [ShowInInspector] private List<DataShip> _dataFleetList ; //список кораблей во флоту

    [ShowInInspector]
    private Dictionary<ShipType.eShipType, GameObject> _objectShipInPrefabFleet 
        = new Dictionary<ShipType.eShipType, GameObject>()
    {
        {ShipType.eShipType.heavy, null},
        { ShipType.eShipType.medium, null},
        { ShipType.eShipType.light, null},
    };

    private Vector3[] locPositionGOShipInFleet;

[ShowInInspector]
    private Dictionary<ShipType.eShipType, List<DataShip>> dicShips = new Dictionary<ShipType.eShipType, List<DataShip>>
    {
        {ShipType.eShipType.heavy, new List<DataShip>()},
        {ShipType.eShipType.medium, new List<DataShip>()},
        {ShipType.eShipType.light, new List<DataShip>()},
    };

    [ShowInInspector] public List<DataShip> _listFleet_light = new List<DataShip>(); 
    [ShowInInspector] public List<DataShip> _listFleet_medium = new List<DataShip>(); 
    [ShowInInspector] public List<DataShip> _listFleet_heavy = new List<DataShip>(); 


    private FleetState _fleetState;
    private Transform _target;
    private SceneMembersData _membersDataInFleet;
    [SerializeField] private Transform _parentTransformInFleet;
    [SerializeField] private Transform _distParentTransform;
    private ParametrPlanet_mono _distParametrPlanetMono;
    private ParametrPlanet_mono _selfParametrPlanetMono;


    [Header("Health | Damage")] 
    private HealthSystem _healthSystem;
    public FleetShootingSystem _fleetShootingSystem;
    [SerializeField] private GameObject _prefabBullet;


    public ParametrPlanet_mono prop_DistParametrPlanetMono => _distParametrPlanetMono;

    public Transform prop_DistParentTransform => _distParentTransform;

    [Header("Time")] 
    private float _timer;
    private float _tempTimer;

    //Test

    [SerializeField] private bool flagDestroy;

    private void Awake()
    {
        _attackFleet = 0;
        _armorFleet = 0;
        _numShipInFleet = 0;

    }

    private void OnDisable()
    {
        Destroy();
        //print($"disable {_distParametrPlanetMono.attackingFleet_LGO.Count}  {_parentTransformInFleet.name}");
    }

    private void Update()
    {
            CallRegenShield();

    }

    public void AddShipToFleet(DataShip locDataShip)
    {
        _dataFleetList.Add(locDataShip);

        DisplayAttackAndDefenceFleet();
    }


    public SceneMembersData GetMembersData()
    {
        return _membersDataInFleet;
    }

    public Transform GetParentTransform()
    {
        return _parentTransformInFleet;
    }

    public List<DataShip> GetListDataFleet()
    {
        return _dataFleetList;
    }

    public void RemoveAttackAndDefence(DataShip locDatafleet)
    {
        DisplayAttackAndDefenceFleet();
    }

    private void DisplayNumShipInFleet()
    {
        _textNumShipInFleet.text = _dataFleetList.Count.ToString();
    }

    private void DisplayAttackAndDefenceFleet()
    {
        ClearParamFleetAnd();
        foreach (var idx in _dataFleetList)
        {
            _attackFleet += (int)idx.damageShipMin;
            _armorFleet += (int)idx.armorShip;
        }
        _attackShipInFleetText.text = _attackFleet.ToString();
        _defenceShipInFleetText.text = _armorFleet.ToString();
    }

    public void ClearParamFleetAndDisplay()
    {
        ClearParamFleetAnd();
        DisplayAttackAndDefenceFleet();
    }

    public void ClearParamFleetAnd()
    {
        _attackFleet = 0;
        _armorFleet = 0;
        _numShipInFleet = 0;
    }

    //test
    private DataBullet SetDataBullet()
    {
        DataBullet bullet = new DataBullet();
        bullet.damageBullet = _dataFleetList;
        bullet.speedBullet = 3f;
        bullet.fireRateBullet = 1f;
        bullet.lifaTimeBullet = 0;
        return bullet;
    }

    public void InitiateFleet(List<DataShip> locDataFleet, Material locMaterial 
        ,Transform locPlanetIsOwnerFleet, Transform locParentTransform 
        ,ParametrPlanet_mono locTargetPlanetMono, SceneMembersData locMembersDataInFleet
        ,FleetStateStruct.enumFleetState _locFleetState)
    {
        locPositionGOShipInFleet = new Vector3[Enum.GetValues(typeof(ShipType.eShipType)).Length];

        FindSpaceshipsInChildren(transform);

        _dataFleetList = new List<DataShip> ( locDataFleet );

        _fleetShootingSystem = GetComponent<FleetShootingSystem>();
        _fleetShootingSystem.InitShootingSystem(_prefabBullet, SetDataBullet(), _dataFleetList);

        ClearParamFleetAndDisplay();

        Color locColor = new Color(locMaterial.color.r, locMaterial.color.g, locMaterial.color.r, 1f);

        _imageFleet_R.GetComponent<Image>().color = locColor;
        _imageFleet_R.GetComponent<Image>().material = new Material(locMaterial) ;
        _imageFleet_R.GetComponent<Image>().material.SetColor("_EmissionColor", locMaterial.color * 1.9f);

        _selfPlanetTransform = locPlanetIsOwnerFleet;
        _parentTransformInFleet = locParentTransform;

        _distParametrPlanetMono = locTargetPlanetMono;
        _membersDataInFleet = locMembersDataInFleet;

        _selfParametrPlanetMono = locPlanetIsOwnerFleet.GetComponent<ParametrPlanet_mono>();

        _fleetState = GetComponent<FleetState>();
        _fleetState.SetState( _locFleetState, locTargetPlanetMono, _selfParametrPlanetMono);
        _fleetState.speedMove = GetMinSpeedFleet(locDataFleet);


        _timer = 1f;
        _tempTimer = 0;

        ParseTypeShipInFleet(locDataFleet); //парсим тип кораблей для дальнейшего отображения ГО в префабе(для каждого типа свой ГО)


        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();
    }


    //присоединение кораблей другого флота к себе при атаке на планету, если оба флота были отправленны с одной и той же планеты
    public void MergFleets(List<DataShip> locListDataFleetToMerg)
    {

        _dataFleetList.AddRange(locListDataFleetToMerg);
        ParseTypeShipInFleet(locListDataFleetToMerg);
        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();
    }

    public void JoinToDefenderFleet()
    {
        _healthSystem.SetMaxArmorAndShield(_dataFleetList);
        _distParametrPlanetMono.AddFleetToDefenceFleetOnPlanet(_dataFleetList);
        Destroy();
    }

    private void RemoveAttackingFleetFromListOnPlanet()
    {
        for (int i = 0; i < _distParametrPlanetMono.attackingFleet_LGO.Count; i++)
        {
            if (_distParametrPlanetMono.attackingFleet_LGO[i] == gameObject)
            {
                _distParametrPlanetMono.attackingFleet_LGO.RemoveAt(i);
            }
        }
    }

    public void Destroy()
    {
        if (isDefenceFleet)
        {
            DestroyDefenceFleet();
        }
        else
        {
            DestroyAttackingFleet();
        }
    }

    public void DestroyAttackingFleet()
    {
        RemoveAttackingFleetFromListOnPlanet();
        Destroy(gameObject);

    }

    public void DestroyDefenceFleet()
    {
        _selfParametrPlanetMono.ClearDefenceFleet();
        Destroy(gameObject);

    }

    //получаем минимальную скорость из всех кораблей флота
    public float GetMinSpeedFleet(List<DataShip> locDataShips)
    {
        float minSpeed = locDataShips[0].speedShip;

        for (int i = 0; i < locDataShips.Count; i++)
        {
            if (locDataShips[i].speedShip < minSpeed)
                minSpeed = locDataShips[i].speedShip;
        }

        return minSpeed;
    }

    public void TakeDamageFleet( List<DataShip> locDataShips)
    {
        if (locDataShips.Count == 0)
        {
            print($"kill me");
            Destroy();
        }
        else
        {
            _healthSystem.TakeDamage(this, locDataShips);
        }
    }



    private void CallRegenShield()
    {
        _tempTimer += Time.deltaTime;

        if (_tempTimer > _timer)
        {
            _tempTimer = 0;
            
            _healthSystem.RegenerationShield(_dataFleetList);
        }
    }

    public void CapturePlanet()
    {
        _distParametrPlanetMono.ChangeOwnerPlanet(_membersDataInFleet, _parentTransformInFleet);
    }

    //добавляем в словарь GO кораблей флота для последующего отображения GO по наличию во флоте соответствующих типа кораблей
   private void FindSpaceshipsInChildren(Transform parent)
   {
       foreach (Transform child in parent)
       {

           if (child.tag.Contains("Light"))
           {
               _objectShipInPrefabFleet[ShipType.eShipType.light] = child.gameObject;
               locPositionGOShipInFleet[2] = child.gameObject.transform.localPosition;
                child.gameObject.SetActive(false);
           }
           else if (child.tag.Contains("Medium"))
           {
               _objectShipInPrefabFleet[ShipType.eShipType.medium] = child.gameObject;
               locPositionGOShipInFleet[1] = child.gameObject.transform.localPosition;
                child.gameObject.SetActive(false);
            }
            else if (child.tag.Contains("Heavy"))
           {
               _objectShipInPrefabFleet[ShipType.eShipType.heavy] = child.gameObject;
               locPositionGOShipInFleet[0] = child.gameObject.transform.localPosition;
               child.gameObject.SetActive(false);
            }

            // Рекурсивно вызываем функцию для всех дочерних объектов
            FindSpaceshipsInChildren(child);
       }

      
    }

   private void ParseTypeShipInFleet(List<DataShip> locDataFleet)
   {
       //проверяем какие типы кораблей есть во флоте
       for (int i = 0; i < locDataFleet.Count; i++)
       {
           if (locDataFleet[i].typeShip == ShipType.eShipType.light)
           {
               _listFleet_light.Add(locDataFleet[i]);
           }
           else
           
           if (locDataFleet[i].typeShip == ShipType.eShipType.medium)
           {
               _listFleet_medium.Add(locDataFleet[i]);

            }
           else if (locDataFleet[i].typeShip == ShipType.eShipType.heavy)
           {
               _listFleet_heavy.Add(locDataFleet[i]);
           }


       }
        int countLocTransform = 0;

        //activate those GO's that are in the fleet and set their display in the fleet in order, starting from the head
        foreach (ShipType.eShipType shipType in Enum.GetValues(typeof(ShipType.eShipType)))
       {
           if (_listFleet_heavy.Count > 0 && _listFleet_heavy[0].typeShip == shipType)
           {
               ActivateShipInFleet(ShipType.eShipType.heavy, countLocTransform);
               countLocTransform++;
           }
           else if (_listFleet_medium.Count > 0 && _listFleet_medium[0].typeShip == shipType)
           {
               ActivateShipInFleet(ShipType.eShipType.medium, countLocTransform);
               countLocTransform++;
           }
           else if (_listFleet_light.Count > 0 && _listFleet_light[0].typeShip == shipType)
           {
               ActivateShipInFleet(ShipType.eShipType.light, countLocTransform);
               countLocTransform++;
           }
        }
    }

   private void ActivateShipInFleet(ShipType.eShipType shipType, int countLocTransform)
   {
       _objectShipInPrefabFleet[shipType].SetActive(true);
       _objectShipInPrefabFleet[shipType].name = shipType.ToString();

       _objectShipInPrefabFleet[shipType].transform.localPosition = locPositionGOShipInFleet[countLocTransform];
    }
}
