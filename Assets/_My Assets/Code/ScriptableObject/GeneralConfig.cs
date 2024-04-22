using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfig_SO", menuName = "CosmicCon/Config/GeneralConfig", order = 51)]
public class GeneralConfig : ScriptableObject
{
    public string playerName;
    public Color colorPlayer;
    public int playerID;
    private SceneMembersData _playerData;
    public int _lvlTechPlayer;

    public int numberAI;
    public Color[] arrColor_SO = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan };
    public String[] nameAI = { "Nova", "Helix", "Neo", "Rogue" };

    [SerializeField] private Material _material_AI1_planet;
    [SerializeField] private Material _material_AI1_fleet;
    [SerializeField] private Material _material_AI2_planet;
    [SerializeField] private Material _material_AI2_fleet;
    [SerializeField] private Material _material_AI3_planet;
    [SerializeField] private Material _material_AI3_fleet;
    [SerializeField] private Material _material_AI4_planet;
    [SerializeField] private Material _material_AI4_fleet;
    [SerializeField] private Material _material_Neutral_planet;
    [SerializeField] private Material _material_Neutral_fleet;
    [SerializeField] private Material _material_Player_planet;
    [SerializeField] private Material _material_Player_fleet;

    public GeneralConfig()
    {
        if (playerID != 19)
            playerID = 19;
    }
}