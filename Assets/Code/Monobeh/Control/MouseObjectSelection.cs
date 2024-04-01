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

    private void Awake()
    {
        _controls = new InputControls();
        _controls.PC.Select.performed += _ => SelectPlanet();
        //selectableLayer = LayerMask.NameToLayer("PlayerPlanet");
        selectableLayer = LayerMask.GetMask("PlayerPlanet");
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnEnable()
    {
        _controls.Enable();
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
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && highlightMaterial != null)
        {
            // ��������� �������� ���������
            renderer.material = highlightMaterial;
        }
    }

    // ����� ���������� ������� � ������ ���������
    void ClearSelection()
    {
        if (selectedObject != null)
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // ���������� �������� �������� �������
                renderer.material = null;
            }
            selectedObject = null;
        }
    }

}
