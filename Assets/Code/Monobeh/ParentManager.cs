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

    public int prop_id { get => id; }

    private void Start()
    {
        _parentTransform = transform;
        var numChild = _parentTransform.childCount;
        _planetList = new List<ParametrPlanet_mono>();
        Debug.Log($"Name: {id}  {numChild}");
        for (int i = 0; i < numChild; i++)
        {
            if (_parentTransform.GetChild(i).GetComponent<ParametrPlanet_mono>())
            {
                var pl = _parentTransform.GetChild(i);
                _planetList.Add(pl.GetComponent<ParametrPlanet_mono>());
                pl.GetComponent<ParametrPlanet_mono>().SetColorPlanet(_memberSceneDatasParent.colorMembers);
                print($"{_memberSceneDatasParent.colorMembers.ToHexString()}");
            }
        }
    }

    public void SetMembersSceneData(SceneMembersData locAISceneData)
    {
        _memberSceneDatasParent = locAISceneData;
    }

    public void Show()
    {
        Debug.Log($"{_memberSceneDatasParent.nameMembers}(Techlvl): {_memberSceneDatasParent.lvlTech}");
       
    }
}
