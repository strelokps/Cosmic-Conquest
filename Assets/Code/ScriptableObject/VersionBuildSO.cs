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
    [SerializeField] private int _patch;
    [SerializeField] private int _build;

    public int prop_Major => _major;
    public int prop_Minor => _minor;
    public int prop_Patch => _patch;
    public int prop_Build => _build;

    public VersionBuildSO()
    {
    }

    public void Increase()
    {
        _build++;
    }

    public void ShowBuild(TMP_Text _loctextVersion)
    {
        string str = String.Format($"ver.: {_major}.{_minor}.{_build}.{_patch}");
        
        _loctextVersion.text = str ;
    }
}
