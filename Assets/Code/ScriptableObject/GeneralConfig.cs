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

    //    public SceneMembersData SetPlayerData()
    //    {
    //        _playerData.friends = new List<SceneMembersData>();
    //        _playerData.enemy = new List<SceneMembersData>();
    //        _playerData.neutral = new List<SceneMembersData>();

    //        _playerData.nameMembers = playerName;
    //        _playerData.membersID = playerID;
    //        _playerData.colorMembers = colorPlayer;
    //        _playerData.lvlTech = _lvlTechPlayer;

    //        return _playerData;
    //    }
    public Material prop_material_AI1_Planet => _material_AI1_planet;

    public Material prop_material_AI2_Planet => _material_AI2_planet;

    public Material prop_material_AI3_Planet => _material_AI3_planet;

    public Material prop_material_AI4_Planet => _material_AI4_planet;

    public Material prop_material_Neutral_Planet => _material_Neutral_planet;

    public Material prop_material_Player_Planet => _material_Player_planet;

    public Material prop_material_AI1_Fleet => _material_AI1_fleet;

    public Material prop_material_AI2_Fleet => _material_AI2_fleet;

    public Material prop_material_AI3_Fleet => _material_AI3_fleet;

    public Material prop_material_AI4_Fleet => _material_AI4_fleet;

    public Material prop_material_Neutral_Fleet => _material_Neutral_fleet;

    public Material prop_material_Player_Fleet => _material_Player_fleet;
}