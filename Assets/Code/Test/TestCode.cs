using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private GameObject _gameObject;
    private InputControls _controls;

    private void Awake()
    {
        _controls = new InputControls();

    }

    private void Start()
    {
        
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _material.color = Color.blue;
        _material.SetColor("_EmissionColor", Color.blue * 1);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        
        transform.Rotate(new Vector3(0.5f,1f,1f) * 2f);

        if (_controls.Main.Mouse.triggered)
        {
            print($" ноль есть ноль");

        }
    }
}
