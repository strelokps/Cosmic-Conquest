using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowOption : MonoBehaviour
{
    [SerializeField] private GameObject[] _tableAIInOPtion;
    [SerializeField] private GameObject dropDownNumberAI;


    private GeneralConfig _generalConfig;
    private SetUpAI[] _setUpAi;
    private DropDownChooseNumAI _dropDownChooseNumAi;

    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        _dropDownChooseNumAi = new DropDownChooseNumAI();
        SetDeActiveAIWindow();
        SetActiveTableAIWindow_InStart(_generalConfig.numberAI);
        dropDownNumberAI.gameObject.GetComponent<TMP_Dropdown>().value = _generalConfig.numberAI - 1; // устанавливаем отображаемое значение у dropDownNumberAI в соответтвии скол-вом ИИ в numberAI от 1 до 4, в value от 0 до 3
        
    }

    public void SetActiveTableAIWindow_InStart(int locNumAI)
    {
        for (int i = 0; i < locNumAI; i++)
        {
            _tableAIInOPtion[i].gameObject.SetActive(true);
        }
    }

    //запускается из dropDown ChooseNumAI
    public void SetActiveTableAIWindow(int locNumAI)
    {
        _dropDownChooseNumAi.InputDrowDown(locNumAI, _generalConfig);
       
        _setUpAi = new SetUpAI[locNumAI];

        SetDeActiveAIWindow();

        for (int i = 0; i < locNumAI + 1; i++)
        {
            if (_tableAIInOPtion != null)
            {
                _tableAIInOPtion[i].gameObject.SetActive(true);

            }
            else
            {
                Debug.Log($"_tableAIInOPtion = null {_tableAIInOPtion }  {gameObject.name}");
            }

        }

        for (int i = 0; i < locNumAI; i++)
        {
            string str = "SetUpAISO_" + i.ToString();
            _setUpAi[i] = Resources.Load<SetUpAI>(str);
            _setUpAi[i].nameAI_SO = "??? 1" + i.ToString();
            _setUpAi[i].colorAI_SO = _generalConfig.arrColor_SO[i];
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
        }
    }

    private void CreatAI(int locNumAI, int index)
    {

        


    }
}
