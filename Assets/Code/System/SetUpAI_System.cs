using Assets.Code.ScriptableObject;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Assets.Code.System
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SetUpAI_System))]
    public sealed class SetUpAI_System : UpdateSystem
    {
        private SetUpPlayer _setUpPlayer;
        private SetUpAI _setUpAI;
        public override void OnAwake()
        {
            _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
            _setUpAI = Resources.Load<SetUpAI>("SetUpAISO");

            for (int i = 0; i < _setUpAI.numberAI; i++)
            {
                GameObject newObject = new GameObject();
                newObject.name = "AI_" + i;
            
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}