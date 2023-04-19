using Assets.Code.Component;
using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;


[System.Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct Teg_For_parentGO_AI_Component : IComponent
{
    //��� ������ ���� ������ ����. ������������ �� ������ �� AI
    public Transform _transform;
    public AIComponent[] arrAI;
    public int numAI;
    public int numTeamAI;


}