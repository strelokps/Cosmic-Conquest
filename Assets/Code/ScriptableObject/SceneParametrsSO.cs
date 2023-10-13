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
    public int prop_numAI  {
        get { return numAI; }
    }

    [SerializeField] private List<SceneMembersData> _listAISceneData = new List<SceneMembersData>();
    public List<SceneMembersData> prop_ListAiSceneData
    {
        get => _listAISceneData;
    }

    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;
    [SerializeField] private Color _color3;
    [SerializeField] private Color _color4;

    public Color prop_color1 => _color1;
    public Color prop_color2 => _color2;
    public Color prop_color3 => _color3;
    public Color prop_color4 => _color4;

    private GeneralConfig _generalConfig;


    public SceneParametrsSO()
    {
    }

    public void TestScene()
    {
        _listAISceneData.Clear();
        SceneMembersData AI1 = new SceneMembersData{ nameMembers = "Red Evil", colorMembers = Color.red, membersID = 0, lvlTech = 0};
        SceneMembersData AI2 = new SceneMembersData { nameMembers = "Green Evil", colorMembers = Color.green, membersID = 1, lvlTech = 0 };
        SceneMembersData AI3 = new SceneMembersData { nameMembers = "Yellow Evil", colorMembers = Color.yellow, membersID = 2, lvlTech = 0 };
        SceneMembersData neutral1 = new SceneMembersData { nameMembers = "Grey1", colorMembers = Color.gray, membersID = 100, lvlTech = 0 };

        InitAI(ref AI1);
        InitAI(ref AI2);
        InitAI(ref neutral1);


        AI1.friends.Add(AI2);
        AI1.neutral.Add(neutral1);
        
        AI2.friends.Add(AI1);
        AI2.neutral.Add(neutral1);

        neutral1.enemy.Add(AI1);
        neutral1.enemy.Add(AI2);
        

        _listAISceneData.Add(AI1);
        _listAISceneData.Add(AI2);
        _listAISceneData.Add(neutral1);

        _generalConfig = Resources.Load<GeneralConfig>("GeneralConfig_SO");
        _generalConfig._lvlTechPlayer = 4;
    }

    private void InitAI(ref SceneMembersData locAI)
    {
        locAI.enemy = new List<SceneMembersData>();
        locAI.friends = new List<SceneMembersData>();
        locAI.neutral = new List<SceneMembersData>();
    }



}
