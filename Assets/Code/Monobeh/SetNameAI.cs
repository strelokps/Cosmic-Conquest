using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNameAI : MonoBehaviour
{
    private GameObject textNameAI;
    private string tempNameAI;
    private GeneralConfig _generalConfig;

    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        //��������� ����� ����� gameObject � ����� ��������� ������ ��� ���������� ����������� ������ AI � ����������� � ������ ����.
        tempNameAI = gameObject.name;
        if (gameObject.name.Length > 0)
        {

            tempNameAI = tempNameAI.Substring(tempNameAI.Length - 1);
        }
        else
        {
            Debug.Log("Length name AI < 1 char");
        }

        // ���������� ���� �����, ���� ���� ��� ����� ����� AI, � ����������� ��c��������� ����� � ������ ���� � _generalConfig 
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name.Contains("Text Name AI"))
            {
                try
                {
                    var x = gameObject.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                    x.text = _generalConfig.nameAI[int.Parse(tempNameAI)];
                }
                catch (Exception e)
                {
                    Debug.Log($"Text mash pro fo name AI not found {e}");
                    throw;
                }
            }
            else
            {
                Debug.Log($"Component for name AI TextMeshProUGUI not found");
            }
        }
    }
}