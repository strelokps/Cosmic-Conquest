using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

//управляет постройкой корабля, отображением нужных прогресс баров, проверкой наличия достаточности ресурсов для постройки
public class Shipyard : MonoBehaviour
{
    private float timerToBuild;
    private float tempTimerToBuild;
    private ParametrPlanet_mono _selfParametrPlanetMono;
    [ShowInInspector]
    private List<DataShip> _listDataShip; //очередь кораблей на постройку
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
    }

    void FixedUpdate()
    {
        SwitchStateBuild();
    }

    private bool CheckEnoughSolariumForBuildSHip(int locCostShip)
    {
        //false если солариума не хватает на постройку корабля
        bool flagChkSolariumForCostShip = _selfParametrPlanetMono.pParentManager.TakeAmountSolarium() > locCostShip;

        return flagChkSolariumForCostShip;
    }

    public void BuildShipInShipyard(DataShip locDataShip)
    {
        if (!CheckEnoughSolariumForBuildSHip(locDataShip.coastShip)) //на время test. подробности в TODO 
        {
            print("Милор, нужно больше блестящих кругляшков");
            return;
        }

        locDataShip.startPlanet = _selfParametrPlanetMono.name;
        locDataShip.damageShip = UnityEngine.Random.Range(locDataShip.damageShipMin, locDataShip.damageShipMax);
        _selfParametrPlanetMono.pParentManager.RemoveSolarium(locDataShip.coastShip);
        _listDataShip.Add(locDataShip);
        timerToBuild = locDataShip.timeToBuild;
        tempTimerToBuild = 0;
        stateBuildShip = StateBuildShip.build;
    }


    private void CalcTimerToBuild()
    {
        tempTimerToBuild += Time.deltaTime;

        if (tempTimerToBuild >= timerToBuild & _listDataShip.Count > 0)
        {
            print($"<color=green> Очередь в верфь {_listDataShip.Count}</color>");

            _selfParametrPlanetMono.AddShipsToDefenderFleetOnPlanet(_listDataShip[0]);
            _listDataShip.RemoveAt(0);

            if (_listDataShip.Count == 0)
                stateBuildShip = StateBuildShip.clear;
            else
            {
                tempTimerToBuild = 0;
                timerToBuild = _listDataShip[0].timeToBuild;
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
}
