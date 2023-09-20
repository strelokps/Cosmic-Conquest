using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig_SO", menuName = "CosmicCon/Config/GeneralConfig", order = 51)]
public class GeneralConfig : ScriptableObject
{
    public string playerName;
    public Color colorPlayer;

    public int numberAI;
    public Color[] arrColor_SO = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public String[] nameAI = { "Nova", "Nexus", "Neo", "Rogue" };
    public AIBase ai;
}