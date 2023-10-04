using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropDownChooseNumAI : MonoBehaviour
{
   


    public void InputDrowDown(int locValue, GeneralConfig _locGeneralConfig)
    {
        if (_locGeneralConfig.numberAI <= 0)
            _locGeneralConfig.numberAI = 1;
        locValue += 1; //с кнопки значение передается с нуля
        _locGeneralConfig.numberAI = locValue;
        _locGeneralConfig.SetDirty();
    }
}
