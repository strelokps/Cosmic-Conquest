using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    [Header("[ Gold ]")] 
    [SerializeField] private float _timerForGenGold = 1f;
    private float _tempTimerForGenGold;
    [SerializeField] private int _genGoldPerSecond;



    [Header("[ Fleet ]")]
    [SerializeField] private Transform _pointSpawnFleet;
    [SerializeField] private GameObject _prefabFleet;
    private List<GameObject> _listDefenderPlanet = new List<GameObject>();
    private FleetManager _fleetManager = new FleetManager();
    private Vector3 _targetToFleet;

    //test
    DataFleet locDataFleet = new DataFleet();



    
    [Header("[ Lvl Planet ]")]
    [SerializeField] private int _currentLvlPlanet;
    private DataPlanet _dataPlanet = new DataPlanet();


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
        //Gold
        _tempTimerForGenGold = _timerForGenGold;

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
        GenerationGold();
        _tempTimer += Time.deltaTime;
        if (_tempTimer > _timer)
        {
            _tempTimer = 0;
            numShip = 1;
            
            locDataFleet.attack = 2;
            locDataFleet.defence = 10;
            locDataFleet.colorFleet = _colorPlanet;
            SetTarget(new Vector3(40, 1, 24));

        }



    }

    public void StartetConfig(SceneMembersData locMemberSceneDatasParent, Transform locParentTransform)
    {
        //Parent Manager
        _parentManager = locMemberSceneDatasParent.parentTransform.GetComponent<ParentManager>();

        //Gold
        _tempTimerForGenGold = _timerForGenGold;

        _memberSceneDatasParent = locMemberSceneDatasParent;
        _idPlanet = _memberSceneDatasParent.membersID;
        SetColorPlanet(_memberSceneDatasParent.colorMembers);

        if (_memberSceneDatasParent.prefabFleet != null)
            SetPrefabFleet(_memberSceneDatasParent.prefabFleet);
        _materialFleet = _memberSceneDatasParent.fleet_Material;
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransform.GetComponent<ParentManager>();
        SetGoldRepSecond(locMemberSceneDatasParent.lvlTech, _dataPlanet.SetPlanetLvl(_currentLvlPlanet));
    }

    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        if (gameObject.GetComponent<Material>())
            _materialPlanet = new Material(gameObject.GetComponent<Material>());
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * (0.85f)); //цифра обозначает интенсивность свечения
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

    //создание флота
    private void GenerationFleet(DataFleet locDataFleet)
    {
        if (_listDefenderPlanet.Count == 0 )
        {
            SetSpawnPoint(_targetToFleet);

            //создаем флот
            var fl = Instantiate(_prefabFleet, _pointSpawnFleet.position, _pointSpawnFleet.rotation) as GameObject;
            fl.transform.SetParent(_pointSpawnFleet); //устанавливаем парент для флота
            _listDefenderPlanet.Add(fl);  // добавляем в список защитников планеты
            //проводим первичные настройки флота
            if (fl.GetComponent<FleetManager>())
            {
                _fleetManager = fl.GetComponent<FleetManager>();
                _fleetManager.ClearParamFleetAndDisplay();
                _fleetManager.InitiateFleet(locDataFleet, _materialPlanet);
            }
            _fleetManager?.AddNumShipInFleet();
            _fleetManager?.AddAttackAndDefence(locDataFleet);
            _fleetManager.SetTarget(_targetToFleet);
            _targetToFleet = new Vector3();
        }
        else 
        {
            if (_listDefenderPlanet.Count > 0)
            {
                //test
                Debug.Log($"else create Fleet");
                _fleetManager?.AddNumShipInFleet();
                _fleetManager?.AddAttackAndDefence(locDataFleet);
            }
        }

    }


    private void GenerationGold()
    {
        if (_parentManager == null)
            return;

        _tempTimerForGenGold -= Time.deltaTime;
        if (_tempTimerForGenGold <= 0)
        {
            _tempTimerForGenGold = _timerForGenGold;
            _parentManager.AddSolarium(_genGoldPerSecond);
        }
    }

    private void SetGoldRepSecond(int locGoldForTechLvl = 0, int locGoldForPlanetLvl = 0)
    {
        _genGoldPerSecond = locGoldForTechLvl + locGoldForPlanetLvl;
    }

    public void SetTarget(Vector3 locTrarget)
    {
        
        //для теста устанавливаем цель 
        if (_memberSceneDatasParent.enemy.Count > 0)
        {
            int locNumberEnemy = Random.Range(0, _memberSceneDatasParent.enemy.Count);
            var x = _memberSceneDatasParent.enemy[locNumberEnemy];
            var y1 = x.parentTransform;
            var y = y1.GetComponent<ParentManager>();
            _targetToFleet  = y._planetList[0].selfTransform.position;
            GenerationFleet(locDataFleet);

        }
    }

    private void SetSpawnPoint(Vector3 locSpawnPointToTarget)
    {
        float y = _pointSpawnFleet.localPosition.y;
        Vector3 tempVector = (locSpawnPointToTarget - transform.position).normalized * 2f;
        _pointSpawnFleet.localPosition = new Vector3(tempVector.x, y, tempVector.z);
        _pointSpawnFleet.rotation = Quaternion.LookRotation(tempVector);
    }
}
