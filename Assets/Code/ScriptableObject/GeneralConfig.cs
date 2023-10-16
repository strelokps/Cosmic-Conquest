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
}