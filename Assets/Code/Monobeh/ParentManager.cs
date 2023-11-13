using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] public SceneMembersData _memberSceneDatasParent;
    public List<ParametrPlanet_mono> _planetList;
    private Transform _parentTransform;
    private int numChild;

    public int prop_id { get => id; }

    [Header("Enemy")] public string[] strEnemy;



    public void SetMembersSceneData(SceneMembersData locAISceneData)
    {
        _memberSceneDatasParent = locAISceneData;
        _memberSceneDatasParent.selfTransform = _parentTransform;
        _parentTransform = transform; //кеширование нужно для оптимизации

        numChild = CheckNumChild();
        _planetList = new List<ParametrPlanet_mono>();

        for (int i = 0; i < numChild; i++)
        {
            if (transform.GetChild(i).GetComponent<ParametrPlanet_mono>() & gameObject.activeSelf)
            {
                
                var pl = _parentTransform.GetChild(i).GetComponent<ParametrPlanet_mono>();
                _planetList.Add(pl);
                pl.StartetConfig(_memberSceneDatasParent, transform);
            }
        }
        print($"Имя {locAISceneData.nameMembers}");
        print($"Кол-во врагов {locAISceneData.enemy.Count}");
        print("******************");
        strEnemy = new string[locAISceneData.enemy.Count];
        for (int i = 0; i < locAISceneData.enemy.Count; i++)
        {
            print($"{locAISceneData.enemy[i].nameMembers}");
            strEnemy[i] = locAISceneData.enemy[i].nameMembers;

        }
        print("******************");
        print("");
        print("");
    }

    public void Show()
    {
        Debug.Log($"{_memberSceneDatasParent.nameMembers}(Techlvl): {_memberSceneDatasParent.lvlTech}");
       
    }

    private int CheckNumChild() 
    {
        return _parentTransform.childCount;
    }

    
    
}
