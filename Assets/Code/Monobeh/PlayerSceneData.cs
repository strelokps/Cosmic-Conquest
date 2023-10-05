using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneData
{
    public Transform playerParentTransform;
    public Color playerColor;
    public int playerID;

    [Header("Team")]
    public List<SceneMembersData> friends;
    public List<SceneMembersData> enemy;
    public List<SceneMembersData> neutral;
}
