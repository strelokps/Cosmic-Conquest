using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
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



    [Header("[ Fleet ]")] 
    [SerializeField] private Transform _spawnPointAttackFleet;
    [SerializeField] public Transform _spawnPointDefenceFleet;
    [SerializeField] private GameObject _prefabFleet;
    [ShowInInspector]
    public List<DataShip> _listDefenderFleet = new List<DataShip>(); //������ �������� ��� ������ ������ ������� 
    public List<GameObject> attackingFleet_LGO = new List<GameObject>(); //������ ����������� �� ������� ������
    private List<GameObject> _friendlyFleet_LGO = new List<GameObject>(); //������ ������������ �������������� �����
    private FleetManager _fleetManager = new FleetManager();
    //private Transform _targetToFleet;
    [SerializeField] private FleetStateStruct.enumFleetState _stateFleet;
    public GameObject defFleetOnOrbitPlanet_GO; //���� ������ �� ������

    private FleetManager _defFleetManager;

    [Header("[ Select planet ]")] 
    
    [SerializeField] private SpriteRenderer _spriteSelect;


    [Header("[ Test ]")]
    //test
    [SerializeField] private float _percentForAttackFleet;
    //[SerializeField] private int _numShipsInDefenderFleet;
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
    private InputAction.CallbackContext ctx;

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
        _percentForAttackFleet = 100f;
        _controls = new InputControls();
        ctx = new InputAction.CallbackContext();
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

        //_timer = 5f;
        //_tempTimer = 4f;
    }




    /// <summary>
    ///              Update
    /// </summary>

    private void Update()
    {

        _numShipsInAttackingFleet = attackingFleet_LGO.Count; //test



       
        if (_controls.PC.PushFleet.triggered & _idPlanet == 19)
        {
            print($"<color=red> Charg !! {_parentManager.transform.name}</color>");
            //CreateAttackerFleet(_percentForAttackFleet);
        }


        AddShipsToDefenceFleetOnOrbit(); 
        GenerationGold();

        //_tempTimer += Time.deltaTime;
        //if (_tempTimer > _timer)
        //{
        //    _tempTimer = 0;

        //    if (_idPlanet != 19)
        //    {
        //        //print($" ����� �� 19 {_idPlanet}");
        //        if (_controls.PC.PushFleet.IsPressed())
        //        {
        //            CreateAttackerFleet(_percentForAttackFleet);
        //        }
        //    }
        //}

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
            SetPrefabFleet(_memberSceneData.prefabFleet); //���������� ��������������� ������ �����
        _materialFleet = _memberSceneData.fleet_Material;
        SetParentTransform(locParentTransform);
        _parentManager = _parentTransformFromPlanet.GetComponent<ParentManager>();
        SetGoldRepSecond(locMemberSceneDatasParent.lvlTech, _dataPlanet.SetPlanetLvl(_currentLvlPlanet));
        //defFleetOnOrbitPlanet_GO = new GameObject();
        _buyShip = GetComponent<BuyShip>();
        _shipyard = GetComponent<Shipyard>();
        _shipyard.InitShipyard(this);
        _buyShip.InitBuyShip(_shipyard);

        gameObject.name = _parentManager.GetIdForPlanet(); //����������� ���������� ���

        //������������� ���� ������, ��� �������������� � ������������ �����-������ 
        if (_parentManager._flagPlayer)
            gameObject.layer = LayerMask.NameToLayer("PlayerPlanet");
        else
        {
            if (gameObject.layer == LayerMask.NameToLayer("Default"))
                gameObject.layer = LayerMask.NameToLayer("Planet");
        }

        _spriteSelect.transform.gameObject.SetActive(false);


        //test
        _parentManager.AddSolarium(1000);
    }



    public void SetColorPlanet(Color locColorPlanet)
    {
        _colorPlanet = locColorPlanet;
        if (gameObject.GetComponent<Material>())
            _materialPlanet = new Material(gameObject.GetComponent<Material>());
        _materialPlanet.color = _colorPlanet;
        _materialPlanet.SetColor("_EmissionColor", locColorPlanet * (0.65f)); //����� ���������� ������������� ��������

        Material spriteSelectMaterial = _spriteSelect.GetComponent<SpriteRenderer>().material;
        spriteSelectMaterial.color = locColorPlanet;
        spriteSelectMaterial.SetColor("_EmissionColor", locColorPlanet * (0.65f));
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
                                                /// �������� �����
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
                TargetPlanetMono = locTarget.GetComponent<ParametrPlanet_mono>(); //��� ���������� �����

            if (locStateFleet == FleetStateStruct.enumFleetState.StartForDefence)
            {
                TargetPlanetMono = this.GetComponent<ParametrPlanet_mono>(); //��� ����� ������
                originalScale = new Vector3(0.02f, 0.02f, 0.02f);
            }

            if (_prefabFleet.GetComponent<FleetManager>())
            {
                Vector3 spawnPoint = new Vector3(transform.position.x, 0f, transform.position.z);
                //������� ����
                GameObject fl =
                    Instantiate(_prefabFleet, spawnPoint, locSpawnPosition.rotation) as GameObject;

                //�������� ��������� ��������� �����
                _fleetManager = fl.GetComponent<FleetManager>();

                _fleetManager.InitiateFleet(locListAttackedOrDefenderFleet, _materialPlanet, transform
                    , _parentTransformFromPlanet, TargetPlanetMono, _memberSceneData, locStateFleet);

                fl.name = _parentManager.GetIdForFleet();
                fl.transform.localScale = originalScale; //��� ����� ������, ��� �� ����� ������������ ����������

                if (locStateFleet == FleetStateStruct.enumFleetState.StartForDefence &
                    defFleetOnOrbitPlanet_GO  == null)
                {
                    defFleetOnOrbitPlanet_GO = fl;
                    _defFleetManager = defFleetOnOrbitPlanet_GO.GetComponent<FleetManager>();
                }
            }
        }
    }
    //��������� ������� � ����� � �����  �� �������
    public void AddShipsToDefenderFleetOnPlanet(DataShip locDataShip)
    {
        _listDefenderFleet.Add(locDataShip);// ��������� � ������ ���������� �������
    }

    //���������� �������� �� ����� �� ������� � ����� �� ������
    private void AddShipsToDefenceFleetOnOrbit()
    {
        if (_listDefenderFleet.Count > 0 & defFleetOnOrbitPlanet_GO != null)
        {
            _defFleetManager.MergFleets(_listDefenderFleet);
            print($"<color=yellow>{_listDefenderFleet.Count}</color>");

            _listDefenderFleet = new List<DataShip>();
        }
    }


    /// <summary>
    /// ��������� ������� ���� � ����� ������� ��� � ����� �� ������
    /// </summary>
    /// <param name="locListDataFleet"></param>
    
    public void AddFleetToDefenceFleetOnPlanet(List<DataShip> locListDataFleet)
    {
        for (int i = 0; i < locListDataFleet.Count; i++)
        {
            _listDefenderFleet.Add(locListDataFleet[i]); // ��������� � ������ ���������� �������
        }
    }

    //public void AddFleetToDefenderFleetOnOrbit(List<DataShip> locListDataFleet)
    //{
    //    if (defFleetOnOrbitPlanet_GO != null)
    //    {
    //        _defFleetManager.MergFleets(locListDataFleet); // ��������� �� ���� ������, ������� �� ������ ������ �������
    //    }
    //}


    //����� �� ������ ��������� ����� �������
    public GameObject CallDefenderFleet(Transform locTransformAttackingFleet)
    {
        if (_listDefenderFleet.Count > 0 & defFleetOnOrbitPlanet_GO == null)
        {
            SetSpawnPointToDefence(locTransformAttackingFleet);
            _stateFleet = FleetStateStruct.enumFleetState.StartForDefence;


            GenerationFleet(_stateFleet, _listDefenderFleet, SetSpawnPointToDefence(locTransformAttackingFleet), locTransformAttackingFleet);
            _listDefenderFleet = new List<DataShip>(); //������� ������ ����� �� �������, �.�. ��� ������� ���� �������� � ��� ����

            Clear();
        }

        return defFleetOnOrbitPlanet_GO;
    }

    //public Transform SetTarget(Transform locTrarget = null)
    //{
    //    //��� ����� ������������� ���� 
    //    if (_memberSceneData.enemy.Count > 0)
    //    {
    //        //int locNumberEnemy = Random.Range(0, _memberSceneData.enemy.Count);
    //        int locNumberEnemy = 1;

    //        var x = _memberSceneData.enemy[locNumberEnemy];

    //        var y1 = x.parentTransform;

    //        var y = y1.GetComponent<ParentManager>();

    //        if (y._planetList.Count == 0)
    //        {
    //            Debug.LogError("SetTarget error");
    //        }

    //        _targetToFleet  = y._planetList[0].selfTransform;

    //        return _targetToFleet;
    //    }

    //    return locTrarget;
    //}


    //����� ������ �������� ������� � �������
    private Transform SetSpawnPointToAttack(Transform locSpawnPointToTarget)
    {
        float y = _spawnPointAttackFleet.localPosition.y;

        Vector3 tempVector = (locSpawnPointToTarget.position - transform.position).normalized * 2f;
        Transform tempPoint = _spawnPointAttackFleet;
        tempPoint.localPosition = new Vector3(tempVector.x, y, tempVector.z);
        tempPoint.rotation = Quaternion.LookRotation(tempVector, Vector3.up);
        return tempPoint;
    }

    private Transform SetSpawnPointToDefence(Transform locDefTransform)
    {
        Vector3 setDefSpawnPointPosition = (locDefTransform.position - _spawnPointDefenceFleet.position).normalized * 2f; //������� ����� ��������� ����� ��������� �� ������ �������
        
        //_spawnPointDefenceFleet.localPosition = 
        //    new Vector3(setDefSpawnPointPosition.x, 0, setDefSpawnPointPosition.z);
        //_spawnPointDefenceFleet.rotation = Quaternion.LookRotation(setDefSpawnPointPosition);
        
        Transform tempPoint = _spawnPointAttackFleet;
        tempPoint.localPosition = new Vector3(setDefSpawnPointPosition.x, setDefSpawnPointPosition.y, setDefSpawnPointPosition.z);
        tempPoint.rotation = Quaternion.LookRotation(setDefSpawnPointPosition, Vector3.up);
        return tempPoint;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_spawnPointDefenceFleet.position, _spawnPointDefenceFleet.forward * 4f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_spawnPointDefenceFleet.position, 0.3f);

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
    [Button("Push fleet")]
    //������������ ���������� �����
    public void CreateAttackerFleet(float percentageOfTheDefenderFleet, Transform targetPlanet)
    {
        float locPercent = Mathf.Clamp(percentageOfTheDefenderFleet, 0f, 100f); // ������������� �������� ����������� �������� 
        _stateFleet = FleetStateStruct.enumFleetState.StartForAttack;
        //var locTarget = SetTarget();
        var locTarget = targetPlanet;
        SetSpawnPointToAttack(locTarget);

        GenerationFleet(_stateFleet, CalculationPercentageOfTheFleet(locPercent), 
            _spawnPointAttackFleet, targetPlanet);
        Clear();
    }

    /// <summary>
    /// ������������� ���� �� �������
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

    //public void AddToListAttackerFleet(GameObject locAttackerFleet)
    //{
    //    attackingFleet_LGO.Add(locAttackerFleet);
    //}
    public void RemoveToListAttackerFleet(GameObject locAttackerFleet)
    {
        attackingFleet_LGO.Remove(locAttackerFleet);
    }

    //����� ������� �������� �� ����� ������ �������� �� ���� �����
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

   
    //����� ��������� �������
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
        //_targetToFleet = new RectTransform();
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

    public Transform SelectPlanet(bool setSelect)
    {
        _spriteSelect.transform.gameObject.SetActive(setSelect);

        return _spriteSelect.transform;
    }

}
