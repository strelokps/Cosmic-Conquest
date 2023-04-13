using Assets.Code.ScriptableObject;
using Scellecs.Morpeh;
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
        private Filter _filterPlayerComponent;
        private Stash<PlayerComponent> _stashPlayerComponent;

        private Filter _filterTegPlayer;
        private Stash<Teg_For_parentGO_Player_Component> _stashTegPlayer;

        public override void OnAwake()
        {
            var entity = this.World.CreateEntity();

            _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
            _setUpPlayer.playerName = "Strelok";
            _setUpPlayer.playerColor = Color.blue;
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}