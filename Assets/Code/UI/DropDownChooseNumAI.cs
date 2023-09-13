using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownChooseNumAI : MonoBehaviour
{
    //private GeneralConfig _generalConfig;

    //private void Start()
    //{
    //    _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");


    //}


    public void InputDrowDown(int locValue, GeneralConfig _locGeneralConfig)
    {
        if (_locGeneralConfig.numberAI <= 0)
            _locGeneralConfig.numberAI = 1;
        locValue += 1; //с кнопки значение передается с нуля
        _locGeneralConfig.numberAI = locValue;
    }
}
