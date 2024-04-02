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

            // ������� ��� �� ������� ���� � ������� ������������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (selectedPlayerPlanet == null)
            {
                // ��������� ����������� ���� � ��������� �� ���� selectPlayerLayer
                if (Physics.Raycast(ray, out hit, raycastDistance, selectPlayerLayer))
                {
                   SelectPlanet(hit);
                }
                else
                {
                    // ���� �� ������ ������� ������, ���������� �����
                    ClearSelection();
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit, raycastDistance, selectPlayerLayer | selectAILayer) & selectedPlayerPlanet != null)
                {
                    selectedTargetPlanet = hit.collider.gameObject;
                    if (_palyerParametrPlanetMono._listDefenderFleet.Count > 0)
                    _palyerParametrPlanetMono.CreateAttackerFleet(100f, selectedTargetPlanet.transform);
                    else
                    {
                        ClearSelection();
                        SelectPlanet(hit);
                    }
                }
                else
                {
                    // ���� �� ������ ������� ������, ���������� �����
                    ClearSelection();
                }
            }
            
        }
    }

    // ��������� ���������� �������
    void HighlightObject(GameObject obj)
    {
    _transformSpriteSelectForRotate = obj.GetComponent<ParametrPlanet_mono>().SelectPlanet(true);
       
    }

    // ����� ���������� ������� � ������ ���������
    void ClearSelection()
    {
        print($"Select 1 ");

        if (selectedPlayerPlanet != null)
        {
            print($"Select 2 ");

            selectedPlayerPlanet.GetComponent<ParametrPlanet_mono>().SelectPlanet(false);
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

    private void SelectPlanet(RaycastHit locHit)
    {
        print($"hit 3 {locHit.transform.name}");
        // �������� ��������� ������
        GameObject planet = locHit.collider.gameObject;


        // ������������ ��������� ������ (���� ���� �������� ���������)
        HighlightObject(planet);

        // ��������� ��������� ������
        selectedPlayerPlanet = planet;

        _palyerParametrPlanetMono = selectedPlayerPlanet.GetComponent<ParametrPlanet_mono>();

        // ����� ����� �������� �������������� ������ ��� ���������� �������,
        // ��������, ���������� ���������� ��� ���������� ������������� ��������.
    }
}
