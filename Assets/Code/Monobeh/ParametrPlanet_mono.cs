using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParametrPlanet_mono : MonoBehaviour 
{
    private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    private Material _materialPlanet;
    private MeshRenderer _meshRendererPlanet;
    private Transform _parentTransform;
    [HideInInspector] public Transform selfTransform;
    public int timerGenSolarium;

    //Test
    [SerializeField] private Transform _fleetTransform;
    private FleetManager _fleetManager;


    [Header("[ Fleet ]")]
    [SerializeField] private Transform _pointSpawnFleet;

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
        selfTransform = transform;
        if (gameObject.GetComponent<MeshRenderer>())
        {
            _materialPlanet = GetComponent<MeshRenderer>().material;
            _colorPlanet = _materialPlanet.color;

        }
        else
        {
            Debug.Log($"Not found MeshRenderer in gameObject {gameObject.name}  {prop_IdPlanet}");
        }

        _fleetManager = _fleetTransform.GetComponent<FleetManager>();
    }

    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * 1);
        GenerationFleet(locColorPlanet);

    }

    public void SetParentTransform(Transform locParentTransform)
    {
        _parentTransform = locParentTransform;
        transform.SetParent(_parentTransform);
    }

    private void GenerationFleet(Color locColorFleet)
    {
        _fleetManager.SetColorFleet(locColorFleet);
        _fleetTransform.SetParent(transform);
        _fleetTransform.position = _pointSpawnFleet.position;
        
    }

}
