using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanetBuyShip : MonoBehaviour
{
    [SerializeField] private Canvas _uiPlanetBuyShip;

    private ParametrPlanet_mono _parametrPlanetMono;

    [SerializeField] private bool _flagSwitchEnebleUI;


    void Start()
    {
        _parametrPlanetMono = GetComponent<ParametrPlanet_mono>();
        _flagSwitchEnebleUI = _uiPlanetBuyShip.gameObject.activeInHierarchy;
    }

    //Switch HUD planet canvas
    [Button("Switch off")]
    public void ShowUIPlanetBuyShip()
    {
         _flagSwitchEnebleUI = !_flagSwitchEnebleUI;

        _uiPlanetBuyShip.gameObject.SetActive(!_uiPlanetBuyShip.gameObject.activeInHierarchy);
    }
  
}
