using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetManager : MonoBehaviour
{
    private Color _materialFleet_L;
    private Color _materialFleet_R;
    private MeshRenderer _meshRendererFleet;
    [SerializeField] private Image _imageFleet_L;
    [SerializeField] private Image _imageFleet_R;

    private void Start()
    {
        _materialFleet_L = _imageFleet_L.GetComponent<Image>().color;
        _materialFleet_R = _imageFleet_R.GetComponent<Image>().color;
    }

    public void SetColorFleet(Color locColorFleet)
    {
        print($" Цвет флота 0 {locColorFleet}");
        //_materialFleet_L.color = locColorFleet;
        //_materialFleet_L.SetColor("_EmissionColor", locColorFleet * 1);
        _imageFleet_L.GetComponent<Image>().color = locColorFleet;
        _imageFleet_R.GetComponent<Image>().color = locColorFleet;
    }
}
