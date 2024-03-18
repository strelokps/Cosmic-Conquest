using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "VersionBuild_SO", menuName = "CosmicCon/Config/VersionBuildSO", order = 51)]
public class VersionBuildSO : ScriptableObject
{
    [SerializeField] private int _major;
    [SerializeField] private int _minor;
    [SerializeField] private int _buildMinor;
    [SerializeField] private int _build;
    [Header("[ Test bug ]")]
    [SerializeField] private int _tempMinor;
    [SerializeField] private int _tempMinorDvaPlusa;


    public int prop_Major => _major;
    public int prop_Minor => _minor;
    public int BuildMinor => _buildMinor;
    public int prop_Build => _build;


    public VersionBuildSO()
    {
    }

    public void Increase()
    {
 
        _build++;
        if (_tempMinor != _minor)
        {
            _tempMinor = _minor;
            _buildMinor = 0;
            _tempMinorDvaPlusa++;
            Debug.LogError("Ver");

        }
        else
        {
            _buildMinor++;
        }
    }

    public void ShowBuild(TMP_Text _loctextVersion)
    {
        string str = String.Format($"ver.: {_major}.{_minor}.{_buildMinor}.{_build}");
        
        _loctextVersion.text = str ;
    }
}
