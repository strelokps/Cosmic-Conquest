using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private GameObject _gameObject;


    private void Start()
    {
        _material = gameObject.GetComponent<MeshRenderer>().material;
        _material.color = Color.blue;
        _material.SetColor("_EmissionColor", Color.blue * 1);
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0.5f,1f,1f) * 2f);
    }
}
