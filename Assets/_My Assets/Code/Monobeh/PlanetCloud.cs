using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCloud : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private float x;
    private float y;
    private float z;
    private float speedRotate;


    private void Start()
    {
        x = Random.Range(1f, 360f);
        y = Random.Range(1f, 360f);
        z = Random.Range(1f, 360f);
        speedRotate = 0.01f;
    }


    private void Update()
    {

        _gameObject.transform.Rotate(new Vector3(x, y, z) * speedRotate * Time.deltaTime);


    }
}
