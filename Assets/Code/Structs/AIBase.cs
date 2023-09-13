using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    [Header("SO AI"), SerializeField] private SetUpAI _setupAI;

    [Header("Self AI settings")]
    public string nameAI;
    public int idAI;
    public Color colorAI;
    public int lvlTech;

    [Header("Team")]
    public List<AIBase> friends;
    public List<AIBase> enemy;
    public List<AIBase> neutral;


    private void Start()
    {
        if (_setupAI == null)
            Debug.Log("No have load SO SetUpAI");

    }

    public void SetColor(int locValue)
    {
        lvlTech = locValue;
        
    }
}
