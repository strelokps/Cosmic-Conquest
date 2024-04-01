using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseObjectSelection : MonoBehaviour
{

    private InputControls _controls;

    // Слой объектов, которые могут быть выбраны мышью
    public LayerMask selectableLayer;

    // Расстояние, на котором происходит луч для выбора объекта
    public float raycastDistance = 1000f;

    // Материал для подсветки выбранного объекта
    public Material highlightMaterial;

    // Текущий выбранный объект
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
            print($"Выбери меня!");

            // Создаем луч из позиции мыши в мировом пространстве
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Отрисовываем луч в Scene View
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);
            // Проверяем пересечение луча с объектами на слое selectableLayer
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                print($"hit 1 {hit.transform.name} {hit.transform.gameObject.layer}");
                if (selectableLayer == hit.transform.gameObject.layer)
                    print($"hit 2 {hit.transform.name} {hit.transform.gameObject.layer}");

            }


            // Проверяем пересечение луча с объектами на слое selectableLayer
            if (Physics.Raycast(ray, out hit, raycastDistance, selectableLayer))
            {
                print($"hit 3 {hit.transform.name}");
                // Получаем выбранный объект
                GameObject planet = hit.collider.gameObject;

                print($"Планета: {planet.GetComponent<ParametrPlanet_mono>().prop_ParentTransformFromPlanet.name}");

                // Подсвечиваем выбранный объект (если есть материал подсветки)
                HighlightObject(planet);

                // Сохраняем выбранный объект
                selectedObject = planet;

                // Здесь можно добавить дополнительную логику для выбранного объекта,
                // например, обновление интерфейса или выполнение определенного действия.
            }
            else
            {
                // Если не выбран никакой объект, сбрасываем выбор
                ClearSelection();
            }
        }
    }

    // Подсветка выбранного объекта
    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && highlightMaterial != null)
        {
            // Применяем материал подсветки
            renderer.material = highlightMaterial;
        }
    }

    // Сброс выбранного объекта и снятие подсветки
    void ClearSelection()
    {
        if (selectedObject != null)
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Возвращаем исходный материал объекта
                renderer.material = null;
            }
            selectedObject = null;
        }
    }

}
