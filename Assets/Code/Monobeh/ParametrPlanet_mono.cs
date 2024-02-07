using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlanetCapturing))]
[RequireComponent(typeof(BuyShip))]
[RequireComponent(typeof(Shipyard))]

public class ParametrPlanet_mono : MonoBehaviour
{
    [SerializeField] private int _idPlanet;
    private int _lvlTechPlanet;
    [SerializeField] private Color _colorPlanet;
    private SceneMembersData _memberSceneData = new SceneMembersData();
    private Material _materialPlanet;
    private Material _materialFleet;
    private MeshRenderer _meshRendererPlanet;
    [SerializeField] private Transform _parentTransformFromPlanet;
    private ParentManager _parentManager;
    private PlanetCapturing _planetCapturing;
    private BuyShip _buyShip;
    private Shipyard _shipyard;


    [HideInInspector] private Transform selfTransform;

    [Header("[ Gold ]")] [FoldoutGroup("Gold")] [SerializeField]
    private float _timerForGenGold = 1f;

    [FoldoutGroup("Gold")] private float _tempTimerForGenGold;

    [FoldoutGroup("Gold")] [SerializeField]
    private int _genGoldPerSecond;



    [Header("[ Fleet ]")] [SerializeField] private Transform _spawnPointAttackFleet;
    [SerializeField] public Transform _spawnPointDefenceFleet;
    [SerializeField] private GameObject _prefabFleet;
    [ShowInInspector]
    private List<DataShip> _listDefenderFleet = new List<DataShip>(); //список кораблей для защиты внутри планеты 
    public List<GameObject> attackingFleet_LGO = new List<GameObject>(); //список нападающших на планету флотов
    private List<GameObject> _friendlyFleet_LGO = new List<GameObject>(); //список подлетающего дружественного флота
    private FleetManager _fleetManager = new FleetManager();
    private Transform _targetToFleet;
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet;
    private GameObject defFleetOnOrbitPlanet_GO; //Флот защиты на орбите

    private FleetManager _defFleetManager;

    //test
    DataShip _locDataShipTest = new DataShip();
    [SerializeField] private float _percentForAttackFleet;
    [SerializeField] private int _numShipsInDefenderFleet;
    [SerializeField] private int _numShipsInDefenceFleet;
    [SerializeField] private int _numShipsInAttackingFleet;
    private int _numRandomShipsForAttack;




    [Header("[ Lvl Planet ]")] [SerializeField]
    private int _currentLvlPlanet;

    private DataPlanet _dataPlanet = new DataPlanet();


    [Header("[ Input Controls ]")] private InputControls _controls;

    //Test
    private float _timer;
    private float _tempTimer;

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

    public Transform SelfTransform => selfTransform;

    public Transform prop_ParentTransformFromPlanet => _parentTransformFromPlanet;

    public ParentManager pParentManager
    {
        get { return _parentManager; }
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
        _planetCapturing = gameObject.GetComponent<PlanetCapturing>();
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

        _timer = 5f;
        _tempTimer = 4f;
    }

    [Button("Push Ships")]
    private void PushShips()
    {
        //test
        CreateAttackerFleet(_percentForAttackFleet);
    }



    /// <summary>
    ///              Update
    /// </summary>

    private void Update()
    {
        _controls.Disable();

        _numShipsInDefenderFleet = _listDefenderFleet.Count;
        _numShipsInAttackingFleet = attackingFleet_LGO.Count; //test

       
        
        AddShipsToDefenceFleetOnOrbit(); 
        GenerationGold();

        _tempTimer += Time.deltaTime;
        if (_tempTimer > _timer)
        {
            _tempTimer = 0;

            _locDataShipTest.damageShipMin = 2;
            _locDataShipTest.armorShip = 10;
            
            

                if (_idPlanet != 19)
                {
                    //print($" Атака не 19 {_idPlanet}");
                    if (_controls.Main.NewA.IsPressed())
                    {
                        CreateAttackerFleet(_percentForAttackFleet);
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

        _memberSceneData = locMemberSceneDatasParent;
        _idPlanet = _memberSceneData.membersID;
        SetColorPlanet(_memberSceneData.colorMembers);

        if (_memberSceneData.prefabFleet != null)
            SetPrefabFleet(_memberSceneData.prefabFleet); //подгружаеи соответствующий префаб флота
        _materialFleet = _memberSceneData.fleet_Material;
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransformFromPlanet.GetComponent<ParentManager>();
        SetGoldRepSecond(locMemberSceneDatasParent.lvlTech, _dataPlanet.SetPlanetLvl(_currentLvlPlanet));
        //defFleetOnOrbitPlanet_GO = new GameObject();
        _buyShip = GetComponent<BuyShip>();
        _shipyard = GetComponent<Shipyard>();
        _shipyard.InitShipyard(this);
        _buyShip.InitBuyShip(_shipyard);



        //test
        _parentManager.AddSolarium(100);
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
        _parentTransformFromPlanet = locParentTransform;
        transform.SetParent(_parentTransformFromPlanet);
    }

                                                /// <summary>
                                                /// создание флота
                                                /// </summary>
    private void GenerationFleet(FleetStateStruct.enumFleetState locStateFleet,
        List<DataShip> locListAttackedOrDefenderFleet, Transform locSpawnPosition, 
        Transform locTarget)
    {
        if (locListAttackedOrDefenderFleet.Count > 0 )
        {
            var TargetPlanetMono = new ParametrPlanet_mono();
            var originalScale = transform.localScale;
            if (locTarget.GetComponent<ParametrPlanet_mono>()) 
                TargetPlanetMono = locTarget.GetComponent<ParametrPlanet_mono>(); //для атакующего флота
            if (locStateFleet == FleetStateStruct.enumFleetState.StartForDefence)
            {
                TargetPlanetMono = this.GetComponent<ParametrPlanet_mono>(); //для флота защиты
                originalScale = new Vector3(0.02f, 0.02f, 0.02f);
            }

            if (_prefabFleet.GetComponent<FleetManager>())
            {
                //создаем флот
                GameObject fl =
                    Instantiate(_prefabFleet, transform.position, locSpawnPosition.rotation) as GameObject;
                //проводим первичные настройки флота
                _fleetManager = fl.GetComponent<FleetManager>();
                _fleetManager.InitiateFleet(locListAttackedOrDefenderFleet, _materialPlanet, transform
                    , _parentTransformFromPlanet, TargetPlanetMono, _memberSceneData, locStateFleet);
                fl.name = _parentManager.GetIdForFleet();
                fl.transform.localScale = originalScale; //для флота защиты, что бы сразу формировался уменьшиный
                if (locStateFleet == FleetStateStruct.enumFleetState.StartForDefence &
                    defFleetOnOrbitPlanet_GO  == null)
                {
                    defFleetOnOrbitPlanet_GO = fl;
                    _defFleetManager = defFleetOnOrbitPlanet_GO.GetComponent<FleetManager>();
                }
            }
        }
    }
    //добавляем корабли с верфи к флоту  на планете
    public void AddShipsToDefenderFleetOnPlanet(DataShip locDataShip)
    {
        _listDefenderFleet.Add(locDataShip); // добавляем в список защитников планеты
    }

    //добавление кораблей из флота на планете к флоту на орбите
    private void AddShipsToDefenceFleetOnOrbit()
    {

        if (_listDefenderFleet.Count > 0 & defFleetOnOrbitPlanet_GO != null)
        {

            _defFleetManager.MergFleets(_listDefenderFleet);
            print($"<color=_colorPlanet>{_listDefenderFleet.Count}</color>");

            _listDefenderFleet = new List<DataShip>();
            
        }

    }





    public void DestroyDefenceFleet()
    {
        Destroy(defFleetOnOrbitPlanet_GO); 
    }

    //добавляем внешний флот к флоту планеты или к флоту на орбите
    public void AddFleetToDefPlanetFleet(List<DataShip> locListDataFleet)
    {

        if (defFleetOnOrbitPlanet_GO != null)
        {
                _defFleetManager.MergFleets(locListDataFleet);// добавляем во флот защиты, который на данный момент активен
        }
        else
        {
            for (int i = 0; i < locListDataFleet.Count; i++)
            {
                _listDefenderFleet.Add(locListDataFleet[i]); // добавляем в список защитников планеты
            }
        }

    }


    //вызов на орбиту защитного флота планеты
    public GameObject CallDefenderFleet(Transform locTransformAttackingFleet)
    {
        if (_listDefenderFleet.Count > 0 & defFleetOnOrbitPlanet_GO == null)
        {
            SetSpawnPointToDefence(locTransformAttackingFleet);
            _stateFleet = FleetStateStruct.enumFleetState.StartForDefence;
            GenerationFleet(_stateFleet, _listDefenderFleet, _spawnPointDefenceFleet,
                locTransformAttackingFleet);
            _listDefenderFleet = new List<DataShip>(); //очищаем список флота на планете, т.к. все корабли были переданы в деф флот
            Clear();
        }
        return defFleetOnOrbitPlanet_GO;
    }

    public Transform SetTarget(Transform locTrarget = null)
    {
        //для теста устанавливаем цель 
        if (_memberSceneData.enemy.Count > 0)
        {
            //int locNumberEnemy = Random.Range(0, _memberSceneData.enemy.Count);
            int locNumberEnemy = 1;
            var x = _memberSceneData.enemy[locNumberEnemy];
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

    //точка откуда вылетает корабль у планету
    private void SetSpawnPointToAttack(Transform locSpawnPointToTarget)
    {
        float y = _spawnPointAttackFleet.localPosition.y;
        Vector3 tempVector = (locSpawnPointToTarget.position - transform.position).normalized * 2f;
        _spawnPointAttackFleet.localPosition = new Vector3(tempVector.x, y, tempVector.z);
        _spawnPointAttackFleet.rotation = Quaternion.LookRotation(tempVector);
    }

    private void SetSpawnPointToDefence(Transform locDefTransform)
    {
        Vector3 setDefSpawnPointPosition = (locDefTransform.position - transform.position).normalized * 3f; //считаем точку появления флота защитника на орбите планеты
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
    private void CreateAttackerFleet(float percentageOfTheDefenderFleet)
    {
        float locPercent = Mathf.Clamp(percentageOfTheDefenderFleet, 0f, 100f); // устанавливаем диапазон процентного значения 
        _stateFleet = FleetStateStruct.enumFleetState.StartForAttack;
        var locTarget = SetTarget();
        SetSpawnPointToAttack(locTarget);

        GenerationFleet(_stateFleet, CalculationPercentageOfTheFleet(locPercent), 
            _spawnPointAttackFleet, _targetToFleet );
        Clear();
    }

    /// <summary>
    /// Дружественный флот на подлете
    /// </summary>
    /// <param name="locIncomeFriedlyFleet"></param>
    public void AddToListIncomeFriendlyFleet(GameObject locIncomeFriedlyFleet)
    {
        _friendlyFleet_LGO.Add(locIncomeFriedlyFleet);
    }
    public void RemoveToListIncomeFriendlyFleet(GameObject locIncomeFriedlyFleet)
    {
        _friendlyFleet_LGO.Remove(locIncomeFriedlyFleet);
    }

    public void AddToListAttackerFleet(GameObject locAttackerFleet)
    {
        attackingFleet_LGO.Add(locAttackerFleet);
    }
    public void RemoveToListAttackerFleet(GameObject locAttackerFleet)
    {
        attackingFleet_LGO.Remove(locAttackerFleet);
    }

    //какой процент кораблей из флота защиты перейдет во флот атаки
    private List<DataShip> CalculationPercentageOfTheFleet(float locPercent)
    {
        List<DataShip> tempAttackFleet = new List<DataShip>();

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

   
    //Смена владельца планеты
    public bool ChangeOwnerPlanet(SceneMembersData locNewMembersData, Transform locNewParentTransform)
    {
        bool flagChangeOwnerPlanet = false;
        if (_listDefenderFleet.Count == 0 & defFleetOnOrbitPlanet_GO == null )
        {
            StartetConfig(locNewMembersData, locNewParentTransform);
            flagChangeOwnerPlanet = true;
        }

        return flagChangeOwnerPlanet;
    }

    public bool CompareParents(Transform locOtherParentTransform)
    {
        //print($" CompareParents {_parentTransformFromPlanet} targetParent {locOtherParentTransform}");
        bool flagCompare = _parentTransformFromPlanet == locOtherParentTransform;

        return flagCompare;
    }

    private void Clear()
    {
        _stateFleet = FleetStateStruct.enumFleetState.Idle;
        _targetToFleet = new RectTransform();
    }

    public void ClearDefenceFleet()
    {
        _defFleetManager = null;
        defFleetOnOrbitPlanet_GO = null;
    }

    public void GetTransformDefenceFleet(ref Transform locTransfor)
    {
        Transform target = null;
        if (defFleetOnOrbitPlanet_GO != null)
        {
            target = defFleetOnOrbitPlanet_GO.transform;
        }
    }
}
