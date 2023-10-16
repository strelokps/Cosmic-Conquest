using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private SceneMembersData _memberSceneDatasParent;
    [SerializeField] private List<ParametrPlanet_mono> _planetList;
    private Transform _parentTransform;
    private int numChild;

    public int prop_id { get => id; }

    private void Start()
    {
        
    }

    public void SetMembersSceneData(SceneMembersData locAISceneData)
    {
        _memberSceneDatasParent = locAISceneData;

        _parentTransform = transform; //кеширование нужно для оптимизации

        numChild = CheckNimChild();
        _planetList = new List<ParametrPlanet_mono>();

        for (int i = 0; i < numChild; i++)
        {
            if (transform.GetChild(i).GetComponent<ParametrPlanet_mono>())
            {
                var pl = _parentTransform.GetChild(i).GetComponent<ParametrPlanet_mono>();
                _planetList.Add(pl);
                pl.SetColorPlanet(_memberSceneDatasParent.colorMembers);
                print($"{_memberSceneDatasParent.nameMembers} : {_memberSceneDatasParent.colorMembers.ToHexString()}");
            }
        }
        print($"{_memberSceneDatasParent.nameMembers} : {_memberSceneDatasParent.colorMembers.ToHexString()}");
    }

    public void Show()
    {
        Debug.Log($"{_memberSceneDatasParent.nameMembers}(Techlvl): {_memberSceneDatasParent.lvlTech}");
       
    }

    public int CheckNimChild() 
    {
        return _parentTransform.childCount;
    }

    private void Set(string msg) { }
}
