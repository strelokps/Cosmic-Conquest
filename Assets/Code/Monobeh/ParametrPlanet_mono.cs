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
    private List<DataFleet> _listDefenderFleet = new List<DataFleet>();
    private List<DataFleet> _listAttackFleet = new List<DataFleet>();
    private FleetManager _fleetManager = new FleetManager();
    private Transform _targetToFleet;
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet;

    //test
    DataFleet locDataFleet = new DataFleet();
    private int _randomCountFleetToAttack;
    [SerializeField] private float  _percentForAttackFleet;
    [SerializeField] private int _numShipsInFleet;


    
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
        _percentForAttackFleet = 70f;
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
            AddShipsToDefPlanetFleet(locDataFleet);
            //test
            if (_randomCountFleetToAttack > _listDefenderFleet.Count)
                AttackFleet(_percentForAttackFleet);

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

        //test
        _randomCountFleetToAttack = Random.Range(2, 10);
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
    private void GenerationFleet(FleetStateStruct.enumFleetState locStateFleet, List<DataFleet> locAttackFleet)
    {
        if (locAttackFleet.Count > 0)
        {
            SetSpawnPoint(SetTarget());

            //создаем флот
            GameObject fl =
                Instantiate(_prefabFleet, _pointSpawnFleet.position, _pointSpawnFleet.rotation) as GameObject;
            //fl.transform.SetParent(_pointSpawnFleet); //устанавливаем парент для флота
            //проводим первичные настройки флота
            if (fl.GetComponent<FleetManager>())
            {
                _fleetManager = fl.GetComponent<FleetManager>();
                _fleetManager.InitiateFleet(locAttackFleet, _materialPlanet);
                _fleetManager.SetTarget(_targetToFleet, locStateFleet);

            }
            Clear();
        }
    }

    private void AddShipsToDefPlanetFleet(DataFleet locDataFleet)
    {
        _listDefenderFleet.Add(locDataFleet);  // добавляем в список защитников планеты

    }

    public Transform SetTarget(Transform locTrarget = null)
    {
        //для теста устанавливаем цель 
        if (_memberSceneDatasParent.enemy.Count > 0)
        {
            int locNumberEnemy = Random.Range(0, _memberSceneDatasParent.enemy.Count);
            var x = _memberSceneDatasParent.enemy[locNumberEnemy];
            var y1 = x.parentTransform;
            var y = y1.GetComponent<ParentManager>();
            _targetToFleet  = y._planetList[0].selfTransform;
            return _targetToFleet;
        }

        return locTrarget;
    }

    private void SetSpawnPoint(Transform locSpawnPointToTarget)
    {
        float y = _pointSpawnFleet.localPosition.y;
        Vector3 tempVector = (locSpawnPointToTarget.position - transform.position).normalized * 2f;
        _pointSpawnFleet.localPosition = new Vector3(tempVector.x, y, tempVector.z);
        _pointSpawnFleet.rotation = Quaternion.LookRotation(tempVector);
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
    //формирование атакующего флота
    private void AttackFleet(float percentageOfTheDefenderFleet)
    {
        float locPercent = Mathf.Clamp(percentageOfTheDefenderFleet, 0f, 100f);
        _stateFleet = FleetStateStruct.enumFleetState.Movement;
        GenerationFleet(_stateFleet, CalculationPercentageOfTheFleet(locPercent));
    }

    public void DefenderFleet()
    {
        _stateFleet = FleetStateStruct.enumFleetState.Defence;
        GenerationFleet(_stateFleet, _listDefenderFleet);
    }

    //какой процент кораблей из флота защиты перейдет во флот атаки
    private List<DataFleet> CalculationPercentageOfTheFleet(float locPercent)
    {
        List <DataFleet> tempAttackFleet = new List <DataFleet>();
        float tempPercent = _listDefenderFleet.Count * (locPercent / 100);
       
        print($"процент  {locPercent} кол-во кораблей  {_listDefenderFleet.Count} сколько получилось {tempPercent}" +
              $" меньшую сторону {Mathf.Floor(tempPercent)}");

        for (int i = 0; i < _listDefenderFleet.Count; i++)
        {
            //доделать расчет  процент кораблей которые перейдут из флота защиты во флот атаки

        }

        return tempAttackFleet;
    }

    //добавляем корабли во флот защиты
    private void AddShipToDefFleet(DataFleet loDataFleetToAddToFleet)
    {
        _listDefenderFleet.Add(loDataFleetToAddToFleet);
    }


    private void Clear()
    {
        _stateFleet = FleetStateStruct.enumFleetState.Idle;
        _targetToFleet = new RectTransform();
    }
}
