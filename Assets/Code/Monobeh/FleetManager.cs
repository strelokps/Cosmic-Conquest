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
    private DataFleet _dataFleet ;



    private void Awake()
    {
        _attack = 0;
        _defence = 0;
        _numShipInFleet = 0;
       // _colorFon = _ColorObjTest.gameObject.GetComponent<Renderer>().material.color;
    }

    public void SetColorFleet(Color locColorFleet, int locID)
    {



        _imageFleet_R.GetComponent<Image>().color = new Color(locColorFleet.r, locColorFleet.g, locColorFleet.b, locColorFleet.a);

        var i = 0;
        var trans = transform;
        while (i <= 5)
        {//The counter in not really needed because there will at some point be no more parents.
            if (trans.root != null)
            {
                trans = trans.root;//
                i++;
            }
            else
            {
                i = 6;//End the loop when we find the root
            }
        }

        print($"id fleet: {locID}   par {trans.name} color: {locColorFleet}  Image{_imageFleet_R.GetComponent<Image>().color}");


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

    public void AddAttackAndDefence(DataFleet locDatafleet)
    {
        _attack += locDatafleet.attack;
        _defence += locDatafleet.defence;
        DisplayAttackAndDefenceFleet();
    }

    public void RemoveAttackAndDefence(DataFleet locDatafleet)
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

    public void ClearParamFleet()
    {
        _attack = 0;
        _defence = 0;
        DisplayAttackAndDefenceFleet();
    }

    public void InitiateFleet(DataFleet locDataFleet)
    {
        _dataFleet = new DataFleet();
        _dataFleet = transform.GetComponent<DataFleet>();
        _dataFleet = locDataFleet;
        _dataFleet.attack = 12;
        _imageFleet_R.GetComponent<Image>().color = _dataFleet.colorFleet;
    }
}
