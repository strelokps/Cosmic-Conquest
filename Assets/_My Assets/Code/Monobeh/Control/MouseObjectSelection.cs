using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MouseObjectSelection : MonoBehaviour
{

    private InputControls _controls;

    // ���� ��������, ������� ����� ���� ������� �����
    public LayerMask selectPlayerLayer;
    public LayerMask selectAILayer;
    public LayerMask selectUI;

    // ����������, �� ������� ���������� ��� ��� ������ �������
    public float raycastDistance = 1000f;

    // �������� ��� ��������� ���������� �������
    public Material highlightMaterial;

    // ������� ��������� ������
    private GameObject selectedPlayerPlanet;
    private GameObject selectedTargetPlanet;

    private ParametrPlanet_mono _palyerParametrPlanetMono;

    private Transform _transformSpriteSelectForRotate;

    [SerializeField] private float rotationSpeedY = 70f;
    private Vector3 currentRotation;

    private void Awake()
    {
        _controls = new InputControls();
        _controls.PC.Select.performed += _ => HitToPlanet();
        //selectPlayerLayer = LayerMask.NameToLayer("PlayerPlanet");
        selectPlayerLayer = LayerMask.GetMask("PlayerPlanet");
        selectAILayer = LayerMask.GetMask("Planet");
        selectUI = LayerMask.GetMask("UI");
        currentRotation = transform.rotation.eulerAngles;

        _palyerParametrPlanetMono = GetComponent<ParametrPlanet_mono>();

    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void Update()
    {
        RotationSprite();
    }


    private void HitToPlanet()
    {
        if (_controls.PC.Select.triggered)
        {


            //choose player planet 
            if (selectedPlayerPlanet == null)
            {
                SelectPlanet();

            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, selectUI) )
                {
                    if (selectedPlayerPlanet.gameObject.layer == hit.transform.gameObject.layer)
                    print($"UI select {selectedPlayerPlanet.gameObject.layer} == {hit.transform.gameObject.layer}");
                }

                if (Physics.Raycast(ray, out hit, raycastDistance, selectPlayerLayer | selectAILayer) & selectedPlayerPlanet != null)
                {
                    selectedTargetPlanet = hit.collider.gameObject;
                    print($"UI {hit.transform.name}");

                    //если планета игрока, есть корабли на отправку и попали в другую планету, то отправляем корабли
                    //if ib player planet ships to attack and hit to other planet -> send fleet
                    if (_palyerParametrPlanetMono._listDefenderFleet.Count > 0)
                        _palyerParametrPlanetMono.CreateAttackerFleet(_palyerParametrPlanetMono.percentForAttackFleet, selectedTargetPlanet.transform);

                    else
                    {
                        ClearSelection();

                        SelectPlanet();
                    }
                }
                else
                {
                    ClearSelection();
                }
            }
            
        }
    }

    //включаем спрайт обводки планеты, показываем что планета выбрана
    //switch on sprite shoose planet
    void HighlightObject(GameObject obj)
    {
        _transformSpriteSelectForRotate = obj.GetComponent<ParametrPlanet_mono>().SelectPlanet(true);
    }

    void ClearSelection()
    {

        if (selectedPlayerPlanet != null)
        {
            selectedPlayerPlanet.GetComponent<ParametrPlanet_mono>().SelectPlanet(false);
            selectedPlayerPlanet.GetComponent<UIPlanetBuyShip>().ShowUIPlanetBuyShip();

            selectedPlayerPlanet = null;

            selectedTargetPlanet = null;

            _transformSpriteSelectForRotate = null;
        }
    }

    //Rotate sprite select
    private void RotationSprite()
    {
        if (_transformSpriteSelectForRotate != null)
        {
            currentRotation.y += rotationSpeedY * Time.deltaTime;

            _transformSpriteSelectForRotate.rotation = Quaternion.Euler(90f, currentRotation.y, 0f);
        }
    }

    private void SelectPlanet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, selectPlayerLayer))
        {

            GameObject planet = hit.collider.gameObject;

            HighlightObject(planet);

            selectedPlayerPlanet = planet;

            _palyerParametrPlanetMono = selectedPlayerPlanet.GetComponent<ParametrPlanet_mono>();
            selectedPlayerPlanet.GetComponent<UIPlanetBuyShip>().ShowUIPlanetBuyShip();
        }
        else
        {
            ClearSelection();
        }
    }
}
