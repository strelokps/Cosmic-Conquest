using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ParentManager : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] public SceneMembersData _memberSceneDatasParent;
    public List<ParametrPlanet_mono> _planetList;
    private Transform _parentTransform;
    private int numChild;
    private bool _flagPlayer;
    private int _idFleet;
    private int _idPlanet;

    [SerializeField] private Transform enemyTransform;

    [Header("Display solarium")]
    [SerializeField] private int _solarium;

    public int prop_id { get => id; }

  



    public void SetMembersSceneData(SceneMembersData locAISceneData)
    {
        _flagPlayer = locAISceneData.flagPlayer;
        locAISceneData.parentTransform = transform;
        _memberSceneDatasParent = locAISceneData;
        _parentTransform = transform; //кеширование нужно для оптимизации

        numChild = CheckNumChild();
        _planetList = new List<ParametrPlanet_mono>();

        SearchAllChildren(transform);

        //for (int i = 0; i < numChild; i++)
        //{
        //    if (transform.GetChild(i).GetComponent<ParametrPlanet_mono>() 
        //        & gameObject.activeSelf 
        //        & transform.GetChild(i).gameObject.activeSelf)
        //    {

        //        var pl = _parentTransform.GetChild(i).GetComponent<ParametrPlanet_mono>();
        //        _planetList.Add(pl);
        //        pl.StartetConfig(_memberSceneDatasParent, _parentTransform);
        //    }
        //}

    }

    public void AddSolarium(int locAddSolarium)
    {
        if (locAddSolarium >0)
            _solarium += locAddSolarium;
        DisplaySolarium();
    }

    public void RemoveSolarium(int locRemoveSolarium)
    {
        if (locRemoveSolarium <= _solarium)
            _solarium -= locRemoveSolarium;
        if (_solarium < 0)
            _solarium = 0;
        DisplaySolarium();
    }

    public int TakeAmountSolarium()
    {
        return _solarium;
    }

    private int CheckNumChild() 
    {
        return _parentTransform.childCount;
    }

    //Запускаем обновление отображения солариума на UI
    private void DisplaySolarium()
    {
        if (_flagPlayer)
            EventBus.Instans.InvokeSolarium(_solarium);
    }

    //создаем ID название для флота
    public string GetIdForFleet()
    {
        if (_idFleet > 999999)
            _idFleet = 0;
        return "Fleet_" + transform.name + "_" + _idFleet++.ToString();
    }

    //создаем ID название для планеты
    public string GetIdForPlanet()
    {
        if (_idPlanet > 999999)
            _idPlanet = 0;
        return "Planet_" + transform.name + "_" + _idPlanet++.ToString();
    }

    private void SearchAllChildren(Transform parenTransform)
    {
        foreach (Transform child in parenTransform)
        {

            if (child.GetComponent<ParametrPlanet_mono>() 
                & gameObject.activeSelf 
                & child.gameObject.activeSelf
                )
            {
                if (child.GetComponent<ParametrPlanet_mono>().pParentManager == null)
                {
                    var pl = child.GetComponent<ParametrPlanet_mono>();
                    _planetList.Add(pl);
                    pl.StartetConfig(_memberSceneDatasParent, _parentTransform);
                }
            }

            // Рекурсивно вызываем функцию для всех дочерних объектов
            SearchAllChildren(child);
        }
    }
}
