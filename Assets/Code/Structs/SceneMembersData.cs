using System.Collections;
using System.Collections.Generic;
using Assets.Code.ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

public struct SceneMembersData 
{
    [Header("SO AI"), SerializeField] private SetUpAI _setupAI;

    [Header("Self AI settings")]
    public string nameMembers;
    public int membersID;
    public Color colorMembers;
    public int lvlTech;
    public GameObject prefabFleet;

    [Header("Team")]
    public List<SceneMembersData> friends;
    public List<SceneMembersData> enemy;
    public List<SceneMembersData> neutral;




   
}
