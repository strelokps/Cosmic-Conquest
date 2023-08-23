using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig_SO", menuName = "CosmicCon/Config/GeneralConfig", order = 51)]
public class GeneralConfig : ScriptableObject
{

    public int numberAI;
    public Color[] arrColor_SO = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public Color colorPlayer = Color.blue;
}