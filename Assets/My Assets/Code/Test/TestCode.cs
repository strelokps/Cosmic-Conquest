using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private GameObject _gameObject;
    private InputControls _controls;
    private float x;
    private float y;
    private float z;

    private void Awake()
    {
        _controls = new InputControls();

    }

    private void Start()
    {
        x = Random.Range(0.1f, 0.9f);
        y = Random.Range(0.1f, 0.9f);
        z = Random.Range(0.1f, 0.9f);
        if (_material != null)
        {
            _material = gameObject.GetComponent<MeshRenderer>().material;
            //_material.color = Color.blue;
            _material.SetColor("_EmissionColor", _material.color * 1f);
        }
        //print($"{x}  {y}  {z}");
    }

    private void OnEnable()
    {
        _controls.Enable();
        //print($"OnEnable in test");
    }

    private void OnDisable()
    {
        _controls.Disable();
        //print($"OnDisable in test");

    }

    private void Update()
    {
        
        transform.Rotate(new Vector3(x,y,z) * 0.2f);

       
    }
}
