using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    private float _speedBullet;

    private float _damageBullet;


    private float _lifeTime = 5f;
    private float _templifeTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        _speedBullet = 10f;
        _damageBullet = 15;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _templifeTime += Time.deltaTime;

        transform.Translate(Vector3.forward * _speedBullet * Time.deltaTime);

        if (gameObject.activeInHierarchy)
        {
            if (_templifeTime >= _lifeTime)
            {

                print($"BAH-H-H !!!");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли у объекта компонент, способный получать урон
        var target = other.GetComponent<FleetManager>();
        print($"Toch {other.name}");
        if (target != null)
        {
            target.TakeDamage(_damageBullet);
        }

        // Уничтожаем пулю после столкновения
        Destroy(gameObject);
    }
}
