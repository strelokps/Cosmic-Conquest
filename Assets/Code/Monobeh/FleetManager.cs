using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FleetState))]
public class FleetManager : MonoBehaviour
{
    [SerializeField] private Transform _selfTransform;
    [SerializeField] private Transform _pointToFire;
    [SerializeField] private Image _imageFleet_L;
    [SerializeField] private Image _imageFleet_R;
    [SerializeField] private TMP_Text _textNumShipInFleet;
    [SerializeField] private TMP_Text _attackShipInFleetText;
    [SerializeField] private TMP_Text _defenceShipInFleetText;
    private int _numShipInFleet;
    [SerializeField] private int _attack;
    [SerializeField] private int _defence;
    private DataFleet _dataFleet ;
    [SerializeField] private List<DataFleet> _dataFleetList = new List<DataFleet>();
    private FleetState _fleetState;



    private void Awake()
    {
        _attack = 0;
        _defence = 0;
        _numShipInFleet = 0;
        // _colorFon = _ColorObjTest.gameObject.GetComponent<Renderer>().material.color;
        _fleetState = GetComponent<FleetState>();
    }

    

    public void AddNumShipInFleet()
    {
        _numShipInFleet ++;
        DisplayNumShipInFleet(_numShipInFleet);
    }

    public void RemoveNumShipInFleet()
    {
        _numShipInFleet --;
        DisplayNumShipInFleet(_numShipInFleet);
    }

    public void AddAttackAndDefence(DataFleet locDatafleet)
    {
        _dataFleetList.Add(locDatafleet);
        //_attack += locDatafleet.attack;
        //_defence += locDatafleet.defence;
        DisplayAttackAndDefenceFleet();
    }

    public void RemoveAttackAndDefence(DataFleet locDatafleet)
    {
        //_attack -= locDatafleet.attack;
        //_defence -= locDatafleet.defence;
        DisplayAttackAndDefenceFleet();
    }

    private void DisplayNumShipInFleet(int locNumShipToText)
    {
        _textNumShipInFleet.text = _dataFleetList.Count.ToString();
    }

    private void DisplayAttackAndDefenceFleet()
    {
        ClearParamFleetAnd();
        foreach (var idx in _dataFleetList)
        {
            _attack += idx.attack;
            _defence += idx.defence;
        }
        _attackShipInFleetText.text = _attack.ToString();
        _defenceShipInFleetText.text = _defence.ToString();
    }

    public void ClearParamFleetAndDisplay()
    {
        ClearParamFleetAnd();
        DisplayAttackAndDefenceFleet();
    }

    public void ClearParamFleetAnd()
    {
        _attack = 0;
        _defence = 0;
    }

    public void InitiateFleet(DataFleet locDataFleet, Material locMaterial)
    {
        _dataFleetList = new List<DataFleet>{new DataFleet() { attack = 0, defence = 0} };

        _dataFleet = new DataFleet();
        _dataFleet = locDataFleet;
        Color locColor = new Color(locDataFleet.colorFleet.r, locDataFleet.colorFleet.g, locDataFleet.colorFleet.r, 1f);
        _imageFleet_R.GetComponent<Image>().color = locColor;
        _imageFleet_R.GetComponent<Image>().material = new Material(locMaterial) ;
        _imageFleet_R.GetComponent<Image>().material.SetColor("_EmissionColor", locDataFleet.colorFleet * 1.9f);
    }

    public void SetTarget(Vector3 locTargetPosition)
    {
        _fleetState.SetTargetToMove(locTargetPosition);
    }
}
