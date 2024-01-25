using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyShip : MonoBehaviour
{
    private ParametrPlanet_mono selfParamPlanet_mono;
    public void InitBuyShip()
    {
        selfParamPlanet_mono = GetComponent<ParametrPlanet_mono>();
    }
}
