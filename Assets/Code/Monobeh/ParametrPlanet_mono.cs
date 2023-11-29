using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParametrPlanet_mono : MonoBehaviour 
{
    [SerializeField] private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    private SceneMembersData _memberSceneDatasParent = new SceneMembersData();
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
    [SerializeField] private Transform _spawnPointAttackFleet;
    [SerializeField] private Transform _spawnPointDefenceFleet;
    [SerializeField] private GameObject _prefabFleet;
    private List<DataFleet> _listDefenderFleet = new List<DataFleet>();     //список кораблей для защиты внутри планеты 
    public List<GameObject> _listAttackersFleet = new List<GameObject>();    //список нападающших на планету флотов
    private FleetManager _fleetManager = new FleetManager();
    private Transform _targetToFleet;
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet;
    public List<GameObject> goDefFleet;                                     //Флот защиты на орбите
    private FleetManager _defFleetManager;

    //test
    DataFleet locDataFleet = new DataFleet();
    private int _randomCountFleetToAttack;
    [SerializeField] private float  _percentForAttackFleet;
    [SerializeField] private int _numShipsInFleet;
    private int _numRandomShipsForAttack;
    bool testFlag = false;




    [Header("[ Lvl Planet ]")]
    [SerializeField] private int _currentLvlPlanet;
    private DataPlanet _dataPlanet = new DataPlanet();


    [Header("[ Input Controls ]")] 
    private InputControls _controls;

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
        _controls = new InputControls();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void Start()
    {
        //StartCoroutine("TestGenerationFleet");

        _timer = 1f;
        _tempTimer = 0;

    }
                                                /// <summary>
                                                ///              Update
                                                /// </summary>
                                                
    private void Update()
    {
        //Vector2 move = _controls.Main.NewA.ReadValue<Vector2>();
        print($"move {testFlag}");

        if (_controls.Main.Mouse.triggered & _idPlanet == 19)
        {
            print($" ноль есть ноль");
            testFlag = true;

        }

        //if (_controls.Main.NewA.WasPressedThisFrame())
        //{
        //    print($" ноль есть ноль 1");

        //}

        //if (_controls.Main.NewA.WasReleasedThisFrame())
        //{
        //    print($" ноль есть ноль 2");

        //}

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
            if (_randomCountFleetToAttack < _listDefenderFleet.Count)
            {
                if (_idPlanet == 19)
                {
                    print(" атака  19");
                    //print($"количетство кораблей защиты: {_listDefenderFleet.Count} > {_randomCountFleetToAttack}  " + $" id:  {_idPlanet}");

                    //
                    if (testFlag)
                    {
                        print($" {_randomCountFleetToAttack} < {_listDefenderFleet.Count}  {_idPlanet}");

                        AttackFleet(_percentForAttackFleet);
                        testFlag = false;

                    }
                }

                if (_idPlanet != 19)
                {
                    //print($" Атака не 19 {_idPlanet}");
                    if (_controls.Main.NewA.IsPressed())
                        //print($" {_randomCountFleetToAttack} < {_listDefenderFleet.Count}  {_idPlanet}");
                    {
                        AttackFleet(_percentForAttackFleet);
                    }
                }

            }


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
            SetPrefabFleet(_memberSceneDatasParent.prefabFleet); //подгружаеи соответствующий префаб флота
        _materialFleet = _memberSceneDatasParent.fleet_Material;
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransform.GetComponent<ParentManager>();
        SetGoldRepSecond(locMemberSceneDatasParent.lvlTech, _dataPlanet.SetPlanetLvl(_currentLvlPlanet));
        goDefFleet = new List<GameObject>();
        //test
        _randomCountFleetToAttack = Random.Range(2, 10);
    }



    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        if (gameObject.GetComponent<Material>())
            _materialPlanet = new Material(gameObject.GetComponent<Material>());
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * (0.65f)); //цифра обозначает интенсивность свечения
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

                                                /// <summary>
                                                /// создание флота
                                                /// </summary>
    private void GenerationFleet(FleetStateStruct.enumFleetState locStateFleet, List<DataFleet> locListAttackedOrDefenderFleet
        ,Transform locSpawnPosition, Transform locTarget, bool flagDefFleet )
    {
        if (locListAttackedOrDefenderFleet.Count > 0)
        {

            //создаем флот
            GameObject fl =
                Instantiate(_prefabFleet, locSpawnPosition.position, locSpawnPosition.rotation) as GameObject;
            //проводим первичные настройки флота
            if (fl.GetComponent<FleetManager>())
            {
                _fleetManager = fl.GetComponent<FleetManager>();
                _fleetManager.InitiateFleet(locListAttackedOrDefenderFleet, _materialPlanet, transform);
                _fleetManager.SetTarget(locTarget, locStateFleet);
                if (flagDefFleet )
                {
                    goDefFleet.Add(fl);
                    _defFleetManager = _fleetManager;
                }
            }

            
        }
    }

    private void AddShipsToDefPlanetFleet(DataFleet locDataFleet)
    {
        if (goDefFleet.Count > 0)
        {
            _defFleetManager.AddShipToFleet(locDataFleet);// добавляем во флот защиты, который на данный момент активен
        }
        else
        {
            _listDefenderFleet.Add(locDataFleet); // добавляем в список защитников планеты
        }

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
            if (y._planetList.Count == 0)
            {
                Debug.LogError("error");
            }

            _targetToFleet  = y._planetList[0].selfTransform;
            return _targetToFleet;
        }

        return locTrarget;
    }

    private void SetSpawnPointToAttack(Transform locSpawnPointToTarget)
    {
        float y = _spawnPointAttackFleet.localPosition.y;
        Vector3 tempVector = (locSpawnPointToTarget.position - transform.position).normalized * 2f;
        _spawnPointAttackFleet.localPosition = new Vector3(tempVector.x, y, tempVector.z);
        _spawnPointAttackFleet.rotation = Quaternion.LookRotation(tempVector);
    }

    private void SetSpawnPointToDefence(Transform locDefTransform)
    {
        Vector3 setDefSpawnPointPosition = (locDefTransform.position - transform.position).normalized * 2f;
        _spawnPointDefenceFleet.localPosition = 
            new Vector3(setDefSpawnPointPosition.x, locDefTransform.position.y, setDefSpawnPointPosition.z);
        _spawnPointDefenceFleet.rotation = Quaternion.LookRotation(setDefSpawnPointPosition);

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
        float locPercent = Mathf.Clamp(percentageOfTheDefenderFleet, 0f, 100f); // устанавливаем диапазон процентного значения 
        _stateFleet = FleetStateStruct.enumFleetState.PreAttack;
        var locTarget = SetTarget();
        SetSpawnPointToAttack(locTarget);

        GenerationFleet(_stateFleet, CalculationPercentageOfTheFleet(locPercent), 
            _spawnPointAttackFleet, _targetToFleet, false );
        Clear();
    }

    public void DefenderFleet(Transform locTransformAttackerFleet)
    {
        if (_listDefenderFleet.Count > 0)
        {
            SetSpawnPointToDefence(locTransformAttackerFleet);
            _stateFleet = FleetStateStruct.enumFleetState.Defence;
            GenerationFleet(_stateFleet, _listDefenderFleet, _spawnPointDefenceFleet, 
                locTransformAttackerFleet, true);
            _listDefenderFleet.Clear(); //очищаем список флота на планете, т.к. все корабли были переданы в деф флот
            Clear();
        }
    }

    //какой процент кораблей из флота защиты перейдет во флот атаки
    private List<DataFleet> CalculationPercentageOfTheFleet(float locPercent)
    {
        List<DataFleet> tempAttackFleet = new List<DataFleet>();

        if (_listDefenderFleet.Count > 0)
        {
            float tempPercent = Mathf.Floor(_listDefenderFleet.Count * (locPercent / 100));
            float tempCountShipsToAttack = 0;

            if (tempPercent > _listDefenderFleet.Count)
                tempCountShipsToAttack = _listDefenderFleet.Count;
            else
                tempCountShipsToAttack = tempPercent;
            var tempCountDefFleet = _listDefenderFleet.Count;
            for (int i = 0; tempPercent >= tempAttackFleet.Count; i++)
            {

                tempAttackFleet.Add(_listDefenderFleet[0]);
                _listDefenderFleet.Remove(_listDefenderFleet[0]);
                tempCountShipsToAttack--;
                if (tempCountShipsToAttack <= 0)
                {
                    return tempAttackFleet;
                }
            }
        }
        return tempAttackFleet;
    }

    public void AddAttackerFleetToList(GameObject locGOArrivalAttackerFleet)
    {
            _listAttackersFleet.Add(locGOArrivalAttackerFleet);
    }


    private void Clear()
    {
        _stateFleet = FleetStateStruct.enumFleetState.Idle;
        _targetToFleet = new RectTransform();
    }
}
