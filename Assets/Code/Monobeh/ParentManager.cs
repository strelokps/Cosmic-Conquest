using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private SceneMembersData _memberSceneDatasParent;

    public int prop_id { get => id; }

    public void SetListAISceneData(SceneMembersData locAISceneData)
    {
        _memberSceneDatasParent = locAISceneData;
    }

    public void Show()
    {
        if (_memberSceneDatasParent.friends.Count > 0)
        {
            Debug.Log($"Friends");
            for (int i = 0; i < _memberSceneDatasParent.friends.Count; i++)
            {
                Debug.Log($"membersID {_memberSceneDatasParent.friends[i].nameAI}");
            }
        }

    }
}
