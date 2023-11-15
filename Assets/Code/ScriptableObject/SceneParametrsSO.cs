using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms;

//тестовая сцена. numAI = 2, neutral = 1,  lvlTech = 0.

[CreateAssetMenu(fileName = "SceneParametrs_SO", menuName = "CosmicCon/Config/SceneParametrsSO", order = 51)]
public class SceneParametrsSO : ScriptableObject
{
    [SerializeField] private int numAI;
    public int prop_numAI {
        get { return numAI; }
    }

    [SerializeField] private List<SceneMembersData> _listAISceneData = new List<SceneMembersData>();
    public List<SceneMembersData> prop_ListAiSceneData
    {
        get => _listAISceneData;
    }
    public SceneParametrsSO()
    {
    }

    public void TestScene()
    {
        FleetSO _fleetSO = Resources.Load<FleetSO>("Fleet\\Fleet_SO");

        GeneralConfig _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");

        _listAISceneData.Clear();
        SceneMembersData ai1 = new SceneMembersData
        {
            nameMembers = "Red Evil",
            colorMembers = Color.red,
            membersID = 0,
            lvlTech = 0,
            prefabFleet = _fleetSO.GetProtosFleet(),
            planet_Material = _generalConfig.prop_material_AI1_Planet,
            fleet_Material = _generalConfig.prop_material_AI1_Fleet
            
        };
        SceneMembersData ai2 = new SceneMembersData 
        { 
            nameMembers = "Green Evil", 
            colorMembers = Color.green, 
            membersID = 1, 
            lvlTech = 0,
            prefabFleet = _fleetSO.GetProtosFleet(),
            planet_Material = _generalConfig.prop_material_AI2_Planet,
            fleet_Material = _generalConfig.prop_material_AI2_Fleet
        };
        SceneMembersData ai3 = new SceneMembersData
        {
            nameMembers = "Yellow Evil",
            colorMembers = Color.yellow,
            membersID = 2,
            lvlTech = 0,
            prefabFleet = _fleetSO.GetProtosFleet(),
            planet_Material = _generalConfig.prop_material_AI3_Planet,
            fleet_Material = _generalConfig.prop_material_AI3_Fleet
        };
        SceneMembersData neutral1 = new SceneMembersData 
        { 
            nameMembers = "Grey1", 
            colorMembers = Color.gray, 
            membersID = 100, 
            lvlTech = 0,
            prefabFleet = _fleetSO.GetUFOFleet(),
            planet_Material = _generalConfig.prop_material_Neutral_Planet,
            fleet_Material = _generalConfig.prop_material_Neutral_Fleet,
            flagNeutral = true
        };
        SceneMembersData player = new SceneMembersData 
        { 
            nameMembers = _generalConfig.playerName, 
            colorMembers = _generalConfig.colorPlayer, 
            membersID = _generalConfig.playerID, 
            lvlTech = _generalConfig._lvlTechPlayer,
            prefabFleet = _fleetSO.GetHumanFleet(),
            planet_Material = _generalConfig.prop_material_Player_Planet,
            fleet_Material = _generalConfig.prop_material_Player_Fleet,
            flagPlayer = true
        };

        InitAI(ref ai1);
        InitAI(ref ai2);
        InitAI(ref ai3);
        InitAI(ref neutral1);
        InitAI(ref player);


        ai1.friends.Add(ai2);
        ai1.enemy.Add(neutral1);
        ai1.enemy.Add(ai3);
        ai1.enemy.Add(player);

        ai2.friends.Add(ai1);
        ai2.enemy.Add(neutral1);
        ai2.enemy.Add(ai3);
        ai2.enemy.Add(player);

        ai3.enemy.Add(neutral1);
        ai3.enemy.Add(ai1);
        ai3.enemy.Add(ai2);
        ai3.enemy.Add(player);

        neutral1.enemy.Add(ai1);
        neutral1.enemy.Add(ai2);
        neutral1.enemy.Add(ai3);
        neutral1.enemy.Add(player);

        
        player.enemy.Add(ai1);
        player.enemy.Add(ai2);
        player.enemy.Add(ai3);
        player.enemy.Add(neutral1);


        _listAISceneData.Add(ai1);
        _listAISceneData.Add(ai2);
        _listAISceneData.Add(ai3);
        _listAISceneData.Add(neutral1);
        _listAISceneData.Add(player);


    }

    private void InitAI(ref SceneMembersData locAI)
    {
        locAI.enemy = new List<SceneMembersData>();
        locAI.friends = new List<SceneMembersData>();
        locAI.neutral = new List<SceneMembersData>();
    }



}
