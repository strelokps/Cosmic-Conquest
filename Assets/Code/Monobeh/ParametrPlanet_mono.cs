using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParametrPlanet_mono : MonoBehaviour 
{
    private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    [SerializeField] private SceneMembersData _memberSceneDatasParent;
    private Material _materialPlanet;
    private MeshRenderer _meshRendererPlanet;
    private Transform _parentTransform;
    private ParentManager _parentManager;

    [HideInInspector] public Transform selfTransform;
    public int timerGenSolarium;



    [Header("[ Fleet ]")]
    [SerializeField] private Transform _pointSpawnFleet;
    [SerializeField] private GameObject _prefabFleet;
    private List<GameObject> _listDefenderPlanet = new List<GameObject>();
    private FleetManager _fleetManager = new FleetManager();

    //Test


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


    private void Awake()
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
        StartCoroutine("TestGenerationFleet");
    }

    //private void Update()
    //{


    //}

    public void StartetConfig(SceneMembersData locMemberSceneDatasParent, Transform locParentTransform)
    {
        _memberSceneDatasParent = locMemberSceneDatasParent;
        SetColorPlanet(locMemberSceneDatasParent.colorMembers);
        if (locMemberSceneDatasParent.prefabFleet != null)
            SetPrefabFleet(locMemberSceneDatasParent.prefabFleet);
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransform.GetComponent<ParentManager>();
        HashRefManagerFleet();
    }

    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * 1);
    }

    public void SetPrefabFleet(GameObject locPrefabFleet)
    {
        _prefabFleet = locPrefabFleet;
    }

    public void SetParentTransform(Transform locParentTransform)
    {
        _parentTransform = locParentTransform;
        transform.SetParent(_parentTransform);
    }

    private void GenerationFleet(structDataFleet locDataFleet, int numShip)
    {
        if (_listDefenderPlanet.Count == 0 )
        {
            var fl = Instantiate(_prefabFleet, _pointSpawnFleet.position, Quaternion.identity, _pointSpawnFleet);
            _listDefenderPlanet.Add(fl);
            if (fl.GetComponent<FleetManager>())
                _fleetManager = _prefabFleet.GetComponent<FleetManager>();


            _fleetManager?.AddNumShipInFleet(numShip);
            _fleetManager?.AddAttackAndDefence(locDataFleet);
            Debug.Log($"create Fleet");

        }
        else 
        {
            if (_listDefenderPlanet.Count != 0)
            {
                Debug.Log($"else create Fleet");
                _fleetManager?.AddNumShipInFleet( numShip);
                _fleetManager?.AddAttackAndDefence(locDataFleet);
            }
        }
    }

    IEnumerator TestGenerationFleet()
    {
        // Test *************************************************************************
        structDataFleet locDataFleet;
        var numShip = 1;
        locDataFleet.attack = 2;
        locDataFleet.defence = 10;
        yield return new WaitForSeconds(2f);
        GenerationFleet(locDataFleet, numShip);
        yield return new WaitForSeconds(4f);
        StartCoroutine("TestGenerationFleet");
    }

    private void HashRefManagerFleet()
    {
        
    }

}
