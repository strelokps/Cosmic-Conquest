using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TestPlanet : MonoBehaviour
{

    public event Action<int> eInitChange;
    public event Action eChangeOwenerPlanet;

    private int _value = 0;

    private float _timer1 = 2f;
    private float _timer2 = 4f;
    private float _tempTimer = 0;

    private bool _flag;

    // Start is called before the first frame update
    private void Start()
    {
        print($"<color=red> start </color>");

    }

    private void Update()
    {
        _tempTimer += Time.deltaTime;
        print($"<color=red> _flag {_flag}  {_value} </color>");

        if (_flag)
        {
            if (_tempTimer > _timer1)
            {
                eChangeOwenerPlanet?.Invoke();
                print($"<color=red>             eChangeOwenerPlanet?.Invoke();\r\n </color>");

            }


            if (_tempTimer > _timer2)
            {
                eInitChange?.Invoke(_value);
                print($"<color=red>             eInitChange \r\n </color>");

            }
        }


    }

    public void Initialize(int value)
    {
        _flag = true;
        _value = value;
        print($"<color=red>             Initialize {_flag} </color>");

    }

    IEnumerator Fade()
    {
        print($"<color=red> 1 </color>");
        yield return new WaitForSeconds(2f);
        
        print($"<color=red> 2 </color>");


        yield return new WaitForSeconds(2f);

        print($"<color=red> 3 </color>");

        _value++;
        eInitChange?.Invoke(_value);

    }
}
