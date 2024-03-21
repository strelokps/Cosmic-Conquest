using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph.Internal;
using static UnityEngine.GraphicsBuffer;
using Object = System.Object;

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
    
    [ShowInInspector] private List<DataShip> _dataFleetList ; //������ �������� �� �����
    private List<GameObject> _arrayShipInPrefabFleet = new List<GameObject>(); //������ �������� �� ������� ����� ��� ����������� ��������� � �������� ��� ���� �������� ( light, medium, heavy) � �� point fire

    [ShowInInspector] private Dictionary<ShipType.eShipType, int> shipCountByType = new Dictionary<ShipType.eShipType, int>
    {
        {ShipType.eShipType.heavy, 0},
        {ShipType.eShipType.medium, 0},
        {ShipType.eShipType.light, 0},
    };
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
        _fleetState = GetComponent<FleetState>();

    }

    private void OnDisable()
    {
        Destroy();
        //print($"disable {_distParametrPlanetMono.attackingFleet_LGO.Count}  {_parentTransformInFleet.name}");
    }

    private void Update()
    {
            CallRegenShield();


            //if (flagDestroy)
            //DestroyAttackingFleet();
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
        
        _fleetState.SetState( _locFleetState, locTargetPlanetMono, _selfParametrPlanetMono);
        _fleetState.speedMove = GetMinSpeedFleet(locDataFleet);


        _timer = 1f;
        _tempTimer = 0;

        ParseTypeShipInFleet(locDataFleet); //������ ��� �������� ��� ����������� ����������� �� � �������(��� ������� ���� ���� ��)

        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();
    }


    //������������� �������� ������� ����� � ���� ��� ����� �� �������, ���� ��� ����� ���� ����������� � ����� � ��� �� �������
    public void MergFleets(List<DataShip> locListDataFleetToMerg)
    {

        _dataFleetList.AddRange(locListDataFleetToMerg);
        ParseTypeShipInFleet(locListDataFleetToMerg);
        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();
    }

    public void JoinToDefenderFleet()
    {
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

    //�������� ����������� �������� �� ���� �������� �����
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
        _healthSystem.TakeDamage(this, locDataShips);
        if (locDataShips.Count == 0)
        {
            print($"kill me");
            Destroy();
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

   private void FindSpaceshipsInChildren(Transform parent)
   {

        if (_arrayShipInPrefabFleet.Count <= 0)
        {
            foreach (Transform child in parent)
            {
                if (child.name.Contains("Spaceships"))
                {
                    _arrayShipInPrefabFleet.Add(child.gameObject);
                    child.gameObject.SetActive(false);
                }

                // ���������� �������� ������� ��� ���� �������� ��������
                FindSpaceshipsInChildren(child);
            }
        }
    }

   private void ParseTypeShipInFleet(List<DataShip> locDataFleet)
   {
       //��������� ����� ���� �������� ���� �� �����
       for (int i = 0; i < locDataFleet.Count; i++)
       {
           if (locDataFleet[i].typeShip == ShipType.eShipType.light)
           {
                shipCountByType[ShipType.eShipType.light]++;
           }
           else
           
           if (locDataFleet[i].typeShip == ShipType.eShipType.medium)
           {
               shipCountByType[ShipType.eShipType.medium]++;

           }
           else
           
           if (locDataFleet[i].typeShip == ShipType.eShipType.heavy)
           {
               shipCountByType[ShipType.eShipType.heavy]++;
           }
       }

       int count = 0;

       foreach (ShipType.eShipType type in Enum.GetValues(typeof(ShipType.eShipType)))
       {
           if (shipCountByType[type] > 0)
           {
               _arrayShipInPrefabFleet[count].SetActive(true);
               _arrayShipInPrefabFleet[count].name = type.ToString();

               count++; 
           }
       }
       
    }
}
