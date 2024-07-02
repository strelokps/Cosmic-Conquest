using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    private float _templifeTime = 0f;

    private DataBullet _bullet = new DataBullet();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _templifeTime += Time.deltaTime;

        transform.Translate(Vector3.forward * _bullet.speedBullet * Time.deltaTime);

        if (gameObject.activeInHierarchy)
        {
            if (_templifeTime >= _bullet.lifeTimeBullet)
            {

                print($"BAH-H-H !!! {gameObject.transform.name}");
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли у объекта компонент, способный получать урон
        var target = other.GetComponent<HealthSystem>();
        if (target != null)
        {
            target.TakeDamage(_bullet.damageBullet );
            //print($"Toch {other.name}");

        }

        // Уничтожаем пулю после столкновения
        Destroy(gameObject);
    }

    public void SetDataBullet(DataBullet locDataBullet, List<DataShip> locDamage)
    {
        _bullet = locDataBullet;
        _bullet.damageBullet = locDamage;
    }
}
