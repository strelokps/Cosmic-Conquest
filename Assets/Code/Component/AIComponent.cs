using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Color = System.Drawing.Color;

namespace Assets.Code.Component
{
    [global::System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AIComponent : IComponent
    {
        public string nameAI;
        public Color colorAI;
        public Dictionary<string, bool> friendOrEnemy;
        public bool isFriendPlayer;
        public GameObject goAI;
    }
}