using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOption : MonoBehaviour
{
    [SerializeField] private GameObject[] _tableAIInOPtion;

    private GeneralConfig _generalConfig;

    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        SetDeActiveAIWindow();
        SetActiveTableAIWindow(_generalConfig.numberAI);
    }


    public void SetActiveTableAIWindow(int locNumAI)
    {
        SetDeActiveAIWindow();
        for (int i = 0; i < locNumAI + 1; i++)
        {
            if (_tableAIInOPtion != null)
            {
                _tableAIInOPtion[i].gameObject.SetActive(true);
            }
            else
            {
                //

            }
        }
    }

    //вырубаем все таблицы ИИ, что бы в SetActiveTableAIWindow включить только то кол-во которое соответствует _generalConfig.numberAI
    private void SetDeActiveAIWindow()
    {
        for (int i = 0; i < _tableAIInOPtion.Length; i++)
        {
            if (_tableAIInOPtion != null)
            {
                _tableAIInOPtion[i].gameObject.SetActive(false);
            }
            else
            {
                //
                
            }
        }
    }
}
