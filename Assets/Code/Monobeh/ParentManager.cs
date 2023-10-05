using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private SceneMembersData _memberSceneDatasParent;

    public int prop_id { get => id; }

    public void SetMembersSceneData(SceneMembersData locAISceneData)
    {
        _memberSceneDatasParent = locAISceneData;
    }

    public void Show()
    {
        Debug.Log($"{_memberSceneDatasParent.nameMembers}(Techlvl): {_memberSceneDatasParent.lvlTech}");
       
    }
}
