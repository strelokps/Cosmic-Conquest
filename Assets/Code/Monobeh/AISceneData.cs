using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public struct AISceneData 
{
    [Header("SO AI"), SerializeField] private SetUpAI _setupAI;

    [Header("Self AI settings")]
    public string nameAI;
    public int idAI;
    public Color colorAI;
    public int lvlTech;

    [Header("Team")]
    public List<AISceneData> friends;
    public List<AISceneData> enemy;
    public List<AISceneData> neutral;




    private void Start()
    {


    }

   
}
