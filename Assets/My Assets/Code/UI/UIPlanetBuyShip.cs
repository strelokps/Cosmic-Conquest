using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanetBuyShip : MonoBehaviour
{
    [SerializeField] private Canvas _uiPlanetBuyShip;

    private ParametrPlanet_mono _parametrPlanetMono;


    // Start is called before the first frame update
    void Start()
    {
        _parametrPlanetMono = GetComponent<ParametrPlanet_mono>();
    }


    public void UIPlanetBuyShip_ON()
    {
        _uiPlanetBuyShip.enabled = !_uiPlanetBuyShip.enabled;
    }
    public void UIPlanetBuyShip_OFF()
    {
    }
}
