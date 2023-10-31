using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject _ColorObjTest;
    private Color _colorObj;


    private void Awake()
    {
        _attack = 0;
        _defence = 0;
        _numShipInFleet = 0;
        _colorObj = _ColorObjTest.gameObject.GetComponent<Renderer>().material.color;
    }

    public void SetColorFleet(Color locColorFleet)
    {
        print($" Цвет флота 0 {locColorFleet}");
        //_materialFleet_L.color = locColorFleet;
        //_materialFleet_L.SetColor("_EmissionColor", locColorFleet * 1);
        _imageFleet_L.GetComponent<Image>().color = locColorFleet;
        _imageFleet_R.GetComponent<Image>().color = locColorFleet;
        _colorObj = locColorFleet;
        _colorObj.a = 20f;

    }

    public void AddNumShipInFleet(int locNumShip)
    {
        _numShipInFleet += locNumShip;
        DisplayNumShipInFleet(_numShipInFleet);
    }

    public void RemoveNumShipInFleet(int locNumShip)
    {
        _numShipInFleet -= locNumShip;
        DisplayNumShipInFleet(_numShipInFleet);
    }

    public void AddAttackAndDefence(structDataFleet locDatafleet)
    {
        _attack += locDatafleet.attack;
        _defence += locDatafleet.defence;
        DisplayAttackAndDefenceFleet();
    }

    public void RemoveAttackAndDefence(structDataFleet locDatafleet)
    {
        _attack -= locDatafleet.attack;
        _defence -= locDatafleet.defence;
        DisplayAttackAndDefenceFleet();
    }

    private void DisplayNumShipInFleet(int locNumShipToText)
    {
        _textNumShipInFleet.text = locNumShipToText.ToString();
    }

    private void DisplayAttackAndDefenceFleet()
    {
        _attackShipInFleetText.text = _attack.ToString();
        _defenceShipInFleetText.text = _defence.ToString();
    }
}
