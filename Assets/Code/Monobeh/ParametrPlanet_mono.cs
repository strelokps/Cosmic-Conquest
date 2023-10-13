using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametrPlanet_mono : MonoBehaviour 
{
    private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    private Material _materialPlanet;
    private MeshRenderer _meshRendererPlanet;
    private Transform _parentTransform;

    public int prop_IdPlanet
    {
        get => _idPlanet;
        set => _idPlanet = value;
    }
    public int prop_LvlTechPlanet
    {
        get => _lvlTechPlanet;
        set => _lvlTechPlanet = value;
    }


    private void Start()
    {
        if (gameObject.GetComponent<MeshRenderer>())
        {
            _materialPlanet = GetComponent<MeshRenderer>().material;
            _colorPlanet = _materialPlanet.color;
        }
        else
        {
            Debug.Log($"Not found MeshRenderer in gameObject {gameObject.name}  {prop_IdPlanet}");
        }
    }

    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        _materialPlanet.color = _colorPlanet;
    }

    public void SetParentTransform(Transform locParentTransform)
    {
        _parentTransform = locParentTransform;
        transform.SetParent(_parentTransform);
    }


}
