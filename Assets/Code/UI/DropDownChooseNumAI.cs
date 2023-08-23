using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownChooseNumAI : MonoBehaviour
{
    private GeneralConfig _generalConfig;

    private void Start()
    {
        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        if (_generalConfig.numberAI <= 0 )
            _generalConfig.numberAI = 1;
    }


    public void InputDrowDown(int locValue)
    {
        locValue += 1;
        _generalConfig.numberAI = locValue;
    }
}
