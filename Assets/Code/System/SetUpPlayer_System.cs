using Assets.Code.ScriptableObject;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Assets.Code.System
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SetUpPlayer_System))]
    public sealed class SetUpPlayer_System : UpdateSystem
    {
        private SetUpPlayer _setUpPlayer;
        public override void OnAwake()
        {
            _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
            _setUpPlayer.playerName = "Strelok";
            _setUpPlayer.playerColor = Color.blue;
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}