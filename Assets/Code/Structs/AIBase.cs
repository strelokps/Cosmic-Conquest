using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIBase
{
    [Header("Self Ai settings")]
    public string nameAI;
    public int idAI;
    public Color colorAI;

    [Header("Team")]
    public List<AIBase> friends;
    public List<AIBase> enemy;

}
