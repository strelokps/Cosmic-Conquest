using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct PlanetComponent : IComponent
{
    public string namePlanet;
    public Transform transformPlanet;
    public Color colorPlanet;
    public int lvlPlanet;
    public int genMoneyPlanet;
}
