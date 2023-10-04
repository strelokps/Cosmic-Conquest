using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VersionBuild_SO", menuName = "CosmicCon/Config/VersionBuildSO", order = 51)]
public class VersionBuildSO : ScriptableObject
{
    [SerializeField] private int _major;
    [SerializeField] private int _minor;
    [SerializeField] private int _patch;
    [SerializeField] private int _build;

    public void Increase()
    {
        _build++;
    }
}
