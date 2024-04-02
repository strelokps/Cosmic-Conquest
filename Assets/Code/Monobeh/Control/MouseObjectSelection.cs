using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseObjectSelection : MonoBehaviour
{

    private InputControls _controls;

    // ���� ��������, ������� ����� ���� ������� �����
    public LayerMask selectableLayer;

    // ����������, �� ������� ���������� ��� ��� ������ �������
    public float raycastDistance = 1000f;

    // �������� ��� ��������� ���������� �������
    public Material highlightMaterial;

    // ������� ��������� ������
    private GameObject selectedObject;

    private Transform _transformSpriteSelectForRotate;

    [SerializeField] private float rotationSpeedY = 70f;
    private Vector3 currentRotation;

    private void Awake()
    {
        _controls = new InputControls();
        _controls.PC.Select.performed += _ => SelectPlanet();
        //selectableLayer = LayerMask.NameToLayer("PlayerPlanet");
        selectableLayer = LayerMask.GetMask("PlayerPlanet");
        currentRotation = transform.rotation.eulerAngles;
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
        //Rotate sprite select
        if (_transformSpriteSelectForRotate != null)
        {
            currentRotation.y += rotationSpeedY * Time.deltaTime;
            _transformSpriteSelectForRotate.rotation = Quaternion.Euler(90f, currentRotation.y , 0f);
        }
    }


    private void SelectPlanet()
    {
        if (_controls.PC.Select.triggered)
        {
            print($"������ ����!");

            // ������� ��� �� ������� ���� � ������� ������������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ������������ ��� � Scene View
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);
            // ��������� ����������� ���� � ��������� �� ���� selectableLayer
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                print($"hit 1 {hit.transform.name} {hit.transform.gameObject.layer}");
                if (selectableLayer == hit.transform.gameObject.layer)
                    print($"hit 2 {hit.transform.name} {hit.transform.gameObject.layer}");

            }


            // ��������� ����������� ���� � ��������� �� ���� selectableLayer
            if (Physics.Raycast(ray, out hit, raycastDistance, selectableLayer))
            {
                print($"hit 3 {hit.transform.name}");
                // �������� ��������� ������
                GameObject planet = hit.collider.gameObject;

                print($"�������: {planet.GetComponent<ParametrPlanet_mono>().prop_ParentTransformFromPlanet.name}");

                // ������������ ��������� ������ (���� ���� �������� ���������)
                HighlightObject(planet);

                // ��������� ��������� ������
                selectedObject = planet;

                // ����� ����� �������� �������������� ������ ��� ���������� �������,
                // ��������, ���������� ���������� ��� ���������� ������������� ��������.
            }
            else
            {
                // ���� �� ������ ������� ������, ���������� �����
                ClearSelection();
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

        if (selectedObject != null)
        {
            print($"Select 2 ");

            selectedObject.GetComponent<ParametrPlanet_mono>().SelectPlanet(false);
            selectedObject = null;
            _transformSpriteSelectForRotate = null;
        }
    }

}
