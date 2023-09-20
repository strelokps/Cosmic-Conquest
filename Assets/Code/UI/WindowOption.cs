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
        dropDownNumberAI.gameObject.GetComponent<TMP_Dropdown>().value = _generalConfig.numberAI - 1; // ������������� ������������ �������� � dropDownNumberAI � ����������� ����-��� �� � numberAI �� 1 �� 4, � value �� 0 �� 3
        
    }

    public void SetActiveTableAIWindow_InStart(int locNumAI)
    {
        for (int i = 0; i < locNumAI; i++)
        {
            _tableAIInOPtion[i].gameObject.SetActive(true);
        }
    }

    //����������� �� dropDown ChooseNumAI
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

    //�������� ��� ������� ��, ��� �� � SetActiveTableAIWindow �������� ������ �� ���-�� ������� ������������� _generalConfig.numberAI
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
