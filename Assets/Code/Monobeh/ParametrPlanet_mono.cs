using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParametrPlanet_mono : MonoBehaviour 
{
    [SerializeField] private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    [SerializeField] private SceneMembersData _memberSceneDatasParent;
    private Material _materialPlanet;
    private Material _materialFleet;
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
    DataFleet locDataFleet = new DataFleet();

    //Test
    private float _timer;
    private float _tempTimer;
    private int numShip;

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
        

    }

    private void Start()
    {
        //StartCoroutine("TestGenerationFleet");

        _timer = 3f;
        _tempTimer = 0;

    }

    private void Update()
    {
        _tempTimer += Time.deltaTime;
        if (_tempTimer > _timer)
        {
            _tempTimer = 0;
            numShip = 1;
            
            locDataFleet.attack = 2;
            locDataFleet.defence = 10;
            locDataFleet.colorFleet = _colorPlanet;
            locDataFleet.volume = numShip;
            GenerationFleet(locDataFleet);
        }



    }

    public void StartetConfig(SceneMembersData locMemberSceneDatasParent, Transform locParentTransform)
    {
        _memberSceneDatasParent = locMemberSceneDatasParent;
        _idPlanet = _memberSceneDatasParent.membersID;
        //_materialPlanet = locMemberSceneDatasParent.planet_Material;
        SetColorPlanet(locMemberSceneDatasParent.colorMembers);

        if (locMemberSceneDatasParent.prefabFleet != null)
            SetPrefabFleet(locMemberSceneDatasParent.prefabFleet);
        _materialFleet = locMemberSceneDatasParent.fleet_Material;
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransform.GetComponent<ParentManager>();
    }

    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        if (gameObject.GetComponent<Material>())
            _materialPlanet = new Material(gameObject.GetComponent<Material>());
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * (0.85f));
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

    private void GenerationFleet(DataFleet locDataFleet)
    {
        if (_listDefenderPlanet.Count == 0 )
        {
            var fl = Instantiate(_prefabFleet, _pointSpawnFleet.position, Quaternion.identity) as GameObject;
            fl.transform.SetParent(_pointSpawnFleet);
            _listDefenderPlanet.Add(fl);
            if (fl.GetComponent<FleetManager>())
            {
                //fl.AddComponent<DataFleet>();
                _fleetManager = fl.GetComponent<FleetManager>();
                _fleetManager.ClearParamFleetAndDisplay();
                _fleetManager.InitiateFleet(locDataFleet, _materialPlanet);

                //для теста устанавливаем цель 
                if (_memberSceneDatasParent.enemy.Count > 0)
                {
                    print("Имя, сестра, имя! " + _memberSceneDatasParent.enemy[0].selfTransform.name);
                    _fleetManager.SetTarget(_memberSceneDatasParent.enemy[0].selfTransform.position);
                }

            }




            _fleetManager?.AddNumShipInFleet(locDataFleet.volume);
            _fleetManager?.AddAttackAndDefence(locDataFleet);

        }
        else 
        {
            if (_listDefenderPlanet.Count != 0)
            {
                //test
                Debug.Log($"else create Fleet");
                _fleetManager?.AddNumShipInFleet( numShip);
                _fleetManager?.AddAttackAndDefence(locDataFleet);
            }
        }
    }
  




}
