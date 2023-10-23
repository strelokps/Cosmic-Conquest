using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fleet_SO", menuName = "CosmicCon/Config/VersionBuildSO", order = 51)]
public class FleetSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab_Fleet;
}
