using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Assets.Code.System
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(test__System))]
    public sealed class test__System : UpdateSystem
    {

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}