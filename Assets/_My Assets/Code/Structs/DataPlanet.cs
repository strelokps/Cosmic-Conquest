using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataPlanet
{

    public int lvlGoldGen;
    [Range(1,3)]private int _currentLvlPlanet;


    /// <summary>
    /// lvlGoldGen and _currentLvlPlanet: var must have range (1,3)
    /// </summary>
    /// /// <param name="count">range (1,3)</param>
    /// <returns></returns>
    public int SetPlanetLvl(int locIntPlanetLvl)
    {
        switch (locIntPlanetLvl)
        {
            case 1:
                lvlGoldGen = 1;
                break;
            case 2:
                lvlGoldGen = 2;
                break;
            case 3:
                lvlGoldGen = 3;
                break;
            default:
                lvlGoldGen = 1;
                break;
        }
        return lvlGoldGen;
    }
}
