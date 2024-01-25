using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(FleetState))]
public class FleetManager : MonoBehaviour
{
    [SerializeField] private Transform _selfTransform;
    [SerializeField] private Transform _pointToFire;
    public Transform _selfPlanetTransform;

    [SerializeField] private Image _imageFleet_L;
    [SerializeField] private Image _imageFleet_R;
    [SerializeField] private TMP_Text _textNumShipInFleet;
    [SerializeField] private TMP_Text _attackShipInFleetText;
    [SerializeField] private TMP_Text _defenceShipInFleetText;
    private int _numShipInFleet;
    [SerializeField] private float _attackFleet;
    [SerializeField] private float _armorFleet;
    [SerializeField] private List<DataShip> _dataFleetList ; //список кораблей во флоту
    private FleetState _fleetState;
    private Transform _target;
    private SceneMembersData _membersDataInFleet;
    [SerializeField] private Transform _parentTransformInFleet;
    [SerializeField] private Transform _distParentTransform;
    private ParametrPlanet_mono _distParametrPlanetMono;
    private ParametrPlanet_mono _selfParametrPlanetMono;

    public ParametrPlanet_mono prop_DistParametrPlanetMono => _distParametrPlanetMono;

    public Transform prop_DistParentTransform => _distParentTransform;

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
        DestroyAttackingFleet();
        print($"<color=green> OnDisable</color>");
        //test
        _selfParametrPlanetMono.DestroyDefenceFleet();
    }

    private void Update()
    {
        if (flagDestroy)
            DestroyAttackingFleet();
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
            _attackFleet += idx.damageShip;
            _armorFleet += idx.armorShip;
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

    public void InitiateFleet(List<DataShip> locDataFleet, Material locMaterial, 
        Transform locPlanetIsOwnerFleet, Transform locParentTransform, 
        ParametrPlanet_mono locTargetPlanetMono, SceneMembersData locMembersDataInFleet,
        FleetStateStruct.enumFleetState _locFleetState)
    {
        _dataFleetList = new List<DataShip> { new DataShip() { damageShip = 0, armorShip = 0 } };
        ClearParamFleetAndDisplay();

        _dataFleetList.AddRange(locDataFleet);
        Color locColor = new Color(locMaterial.color.r, locMaterial.color.g, locMaterial.color.r, 1f);
        _imageFleet_R.GetComponent<Image>().color = locColor;
        _imageFleet_R.GetComponent<Image>().material = new Material(locMaterial) ;
        _imageFleet_R.GetComponent<Image>().material.SetColor("_EmissionColor", locMaterial.color * 1.9f);

        _selfPlanetTransform = locPlanetIsOwnerFleet;
        _parentTransformInFleet = locParentTransform;
        _distParametrPlanetMono = locTargetPlanetMono;
        _membersDataInFleet = locMembersDataInFleet;
        _fleetState.SetState( _locFleetState, locTargetPlanetMono);
        _selfParametrPlanetMono = locPlanetIsOwnerFleet.GetComponent<ParametrPlanet_mono>();

        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();


    }


    //присоединение кораблей другого флота к себе при атаке на планету, если оба флота были отправленны с одной и той же планеты
    public void MergFleets(List<DataShip> locListDataFleetToMerg)
    {
        print($"Merg {locListDataFleetToMerg.Count}");
        for (int i = 0; i < locListDataFleetToMerg.Count; i++)
        {
            _dataFleetList.Add(locListDataFleetToMerg[i]);
        }
        DisplayAttackAndDefenceFleet();
        DisplayNumShipInFleet();
    }

    public void JoinToDefenderFleet()
    {
        _distParametrPlanetMono.AddFleetToDefPlanetFleet(_dataFleetList);
        DestroyAttackingFleet();
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

    public void DestroyAttackingFleet()
    {
        print($"<color=green> DestroyAttackingFleet</color>");
        RemoveAttackingFleetFromListOnPlanet();
        Destroy(gameObject);

    }

    public void DestroyDefenceFleet()
    {
        _selfParametrPlanetMono.ClearDefenceFleet();
    }


}
