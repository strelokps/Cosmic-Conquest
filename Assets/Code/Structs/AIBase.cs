using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    [Header("Self Ai settings")]
    public string nameAI;
    public int idAI;
    public Color colorAI;
    public int lvlTech;

    [Header("Team")]
    public List<AIBase> friends;
    public List<AIBase> enemy;
    public List<AIBase> neutral;

    public void SetLvlTech(int locValue)
    {
        lvlTech = locValue;
    }
}
