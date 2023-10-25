using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fleet_SO", menuName = "CosmicCon/Config/FleetSO", order = 51)]
public class FleetSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab_HumanFleet;
    [SerializeField] private GameObject _prefab_ProtosFleet;
    [SerializeField] private GameObject _prefab_UFOFleet;

    public GameObject GetHumanFleet()
    { 
        return _prefab_HumanFleet;
    }
    public GameObject GetProtosFleet() 
    {
        return _prefab_ProtosFleet;
    }
    public GameObject GetUFOFleet()
    {
        return _prefab_UFOFleet;
    }

}
