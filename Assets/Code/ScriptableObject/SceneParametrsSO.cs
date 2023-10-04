using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//тестовая сцена. AI = 2, начальный уровень.

[CreateAssetMenu(fileName = "SceneParametrs_SO", menuName = "CosmicCon/Config/SceneParametrsSO", order = 51)]
public class SceneParametrsSO : ScriptableObject
{
    [SerializeField] private int numAI;
    public int prop_numAI  {
        get { return numAI; }
    }

    [SerializeField] private List<AISceneData> _listAISceneData = new List<AISceneData>();
    public List<AISceneData> prop_ListAiSceneData
    {
        get => _listAISceneData;
        //set => _listAISceneData = value;
    }

    [SerializeField] private Color _color1;
    [SerializeField] private Color _color2;
    [SerializeField] private Color _color3;
    [SerializeField] private Color _color4;

    public Color prop_color1 => _color1;
    public Color prop_color2 => _color2;
    public Color prop_color3 => _color3;
    public Color prop_color4 => _color4;

    


    public void TestScene()
    {
        _listAISceneData.Clear();
        AISceneData AI1 = new AISceneData{nameAI = "Red Evil", colorAI = Color.red, idAI = 0, lvlTech = 0};
        AISceneData AI2 = new AISceneData { nameAI = "Green Evil", colorAI = Color.green, idAI = 1, lvlTech = 0 };
        AISceneData AINeutral1 = new AISceneData { nameAI = "Grey1", colorAI = Color.gray, idAI = 2, lvlTech = 0 };
        AI1.enemy = new List<AISceneData>();
        AI2.enemy = new List<AISceneData>();
        AI1.friends = new List<AISceneData>();
        AI2.friends = new List<AISceneData>();
        AI1.friends.Add(AI2);
        AI1.enemy.Add(AINeutral1);
        AI2.friends.Add(AI1);
        AI2.enemy.Add(AINeutral1);
        _listAISceneData.Add(AI1);
        _listAISceneData.Add(AI2);
        _listAISceneData.Add(AINeutral1);
    }

    public  SceneParametrsSO()
    {

    }



}
