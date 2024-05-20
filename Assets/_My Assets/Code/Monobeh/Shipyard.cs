using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//��������� ���������� �������, ������������ ������ �������� �����, ��������� ������� ������������� �������� ��� ���������
public class Shipyard : MonoBehaviour
{
    private float timerToBuild;
    private float tempTimerToBuild;
    private ParametrPlanet_mono _selfParametrPlanetMono;
    [ShowInInspector]

    private List<DataShip> _listDataShip; //������� �������� �� ���������
    private int _countShipLight; //������� �������� �� ���������
    private int _countShipMedium; //������� �������� �� ���������
    private int _countShipHeavy; //������� �������� �� ���������

    private Image _progressBarBuildShip;
    private TMP_Text _textCountShips;

    private enum StateBuildShip
    {
        idle,
        build,
        clear
    }

    private StateBuildShip stateBuildShip = StateBuildShip.idle;


    public void InitShipyard(ParametrPlanet_mono locParametrPlanetMono)
    {
        _selfParametrPlanetMono = locParametrPlanetMono;

        _listDataShip = new List<DataShip>();

        _textCountShips = new TextMeshPro();

        tempTimerToBuild = 0;

    }

    void FixedUpdate()
    {
        SwitchStateBuild();
    }

    private bool CheckEnoughSolariumForBuildSHip(int locCostShip)
    {
        //false ���� ��������� �� ������� �� ��������� �������
        bool flagChkSolariumForCostShip = _selfParametrPlanetMono.pParentManager.TakeAmountSolarium() > locCostShip;

        return flagChkSolariumForCostShip;
    }

    public void BuildShipInShipyard(DataShip locDataShip)
    {
        if (!CheckEnoughSolariumForBuildSHip(locDataShip.coastShip)) //�� ����� test. ����������� � TODO 
        {
            print("�����, ����� ������ ��������� ����������");
            return;
        }


        locDataShip.startPlanet = _selfParametrPlanetMono.name;
        locDataShip.damageShip = (float)Math.Round(UnityEngine.Random.Range(locDataShip.damageShipMin, locDataShip.damageShipMax), 2); 
        _selfParametrPlanetMono.pParentManager.RemoveSolarium(locDataShip.coastShip);
        _listDataShip.Add(locDataShip);
        
        //timerToBuild = locDataShip.timeToBuild;
        stateBuildShip = StateBuildShip.build;

        AddCountTypeShips(locDataShip);
        ProgressBarToBuildShips(_listDataShip[0]);

    }

    //������ �������������. �������� �� ������� � ����� if, ����� ������ �� ������, ���� ������ ���� - ������ �����
    private void CalcTimerToBuild()
    {
        tempTimerToBuild += Time.deltaTime;

        _progressBarBuildShip.fillAmount = tempTimerToBuild / _listDataShip[0].timeToBuild;

        if (tempTimerToBuild >= _listDataShip[0].timeToBuild & _listDataShip.Count > 0)
        {
            print($"<color=green> ������� � ����� {_listDataShip.Count}</color>");

            _selfParametrPlanetMono.AddShipsToDefenderFleetOnPlanet(_listDataShip[0]);

            RemoveCountTypeShips(_listDataShip[0]);
            ProgressBarToBuildShips(_listDataShip[0]);

            _listDataShip.RemoveAt(0);
            if (_listDataShip.Count == 0)
                stateBuildShip = StateBuildShip.clear;
            else
            {
                tempTimerToBuild = 0;
                timerToBuild = _listDataShip[0].timeToBuild;
                ProgressBarToBuildShips(_listDataShip[0]);

            }
        }
    }


    private void Clear()
    {
        timerToBuild = 0;
        tempTimerToBuild = 0;
        _listDataShip = new List<DataShip>();
        stateBuildShip = StateBuildShip.idle;
    }


    private void SwitchStateBuild()
    {
        switch (stateBuildShip)
        {
            case StateBuildShip.idle:
                break;
            case StateBuildShip.build:
                CalcTimerToBuild();
                break;
            case StateBuildShip.clear:
                Clear();
                break;

            default:
                stateBuildShip = StateBuildShip.idle;
                break;
        }
    }

    //�������������� ��������� � ���������� � ������ ������ ��� �������� ���� ������������� �������
    //Display on planet progress bar for build ships
    private void AddCountTypeShips(DataShip locDataShip)
    {
        if (locDataShip.typeShip == ShipType.eShipType.light)
        {
            _countShipLight++;
            _selfParametrPlanetMono.progressBuild_GO_light.SetActive(true);

            _selfParametrPlanetMono.progressBuild_Image_light.fillAmount = 0;

            _textCountShips = _selfParametrPlanetMono._textUICountShips_InBuild_Light ;
            _selfParametrPlanetMono._textUICountShips_InBuild_Light.text = _countShipLight.ToString();
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.medium)
        {
            _countShipMedium++;
            _selfParametrPlanetMono.progressBuild_GO_Medium.gameObject.SetActive(true);

            _selfParametrPlanetMono.progressBuild_Image_Medium.fillAmount = 0;

            _textCountShips = _selfParametrPlanetMono._textUICountShips_InBuild_Medium;
            _selfParametrPlanetMono._textUICountShips_InBuild_Medium.text = _countShipMedium.ToString();
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.heavy)
        {
            _countShipHeavy++;
            _selfParametrPlanetMono.progressBuild_GO_Heavy.gameObject.SetActive(true);

            _selfParametrPlanetMono.progressBuild_Image_Heavy.fillAmount = 0;

            _textCountShips = _selfParametrPlanetMono._textUICountShips_InBuild_Heavy;
            _selfParametrPlanetMono._textUICountShips_InBuild_Heavy.text = _countShipHeavy.ToString();
        }
    }


    private void RemoveCountTypeShips(DataShip locDataShip)
    {
        if (locDataShip.typeShip == ShipType.eShipType.light)
        {
            _countShipLight--;
            _selfParametrPlanetMono._textUICountShips_InBuild_Light.text = _countShipLight.ToString();

            if (_countShipLight <= 0)
                _selfParametrPlanetMono.progressBuild_GO_light.gameObject.SetActive(false);
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.medium)
        {
            _countShipMedium--;
            _selfParametrPlanetMono._textUICountShips_InBuild_Medium.text = _countShipMedium.ToString();
            
            if (_countShipMedium <= 0)
            _selfParametrPlanetMono.progressBuild_GO_Medium.gameObject.SetActive(false);
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.heavy)
        {
            _countShipHeavy--;
            _selfParametrPlanetMono._textUICountShips_InBuild_Heavy.text = _countShipHeavy.ToString();

            if (_countShipHeavy <= 0)
            _selfParametrPlanetMono.progressBuild_GO_Heavy.gameObject.SetActive(false);
        }
    }

    private void ProgressBarToBuildShips(DataShip locDataShip)
    {
        if (locDataShip.typeShip == ShipType.eShipType.light)
        {
            _progressBarBuildShip = _selfParametrPlanetMono.progressBuild_Image_light;
            _progressBarBuildShip.fillAmount = 0f;
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.medium)
        {
            _progressBarBuildShip = _selfParametrPlanetMono.progressBuild_Image_Medium;
            _progressBarBuildShip.fillAmount = 0f;
        }

        else

        if (locDataShip.typeShip == ShipType.eShipType.heavy)
        {
            _progressBarBuildShip = _selfParametrPlanetMono.progressBuild_Image_Heavy;
            _progressBarBuildShip.fillAmount = 0f;
        }
    }
}
