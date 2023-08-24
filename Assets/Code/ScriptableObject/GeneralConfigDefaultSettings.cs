using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig_DefaultSettings_SO", menuName = "CosmicCon/Config/GeneralConfig_DefaultSettings", order = 51)]
public class GeneralConfigDefaultSettings : ScriptableObject
{

    public int numberAI;
    public Color[] arrColor_SO = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public Color colorPlayer = Color.blue;
    public AIBase ai;

    void Start()
    {
     
        Debug.Log("Это GeneralConfigDefaultSettings");
    }


}
