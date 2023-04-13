using System.Collections.Generic;
using Assets.Code.Component;
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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SetUpAI_System))]
    public sealed class SetUpAI_System : UpdateSystem
    {
        private SetUpPlayer _setUpPlayer;
        private SetUpAI _setUpAI;
        private Filter _filterAIParentGO;
        private Stash<Teg_For_parentGO_AI_Component> _stashAI_teg_Component;
        private Transform _transformParentAI;
        private Filter _filterAIComponent;


        public override void OnAwake()
        {
            //игрока подгружаем что бы исключить цвет игрока при назначении AI
            _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
            _setUpAI = Resources.Load<SetUpAI>("SetUpAISO");
            _filterAIParentGO = this.World.Filter.With<Teg_For_parentGO_AI_Component>();
            _stashAI_teg_Component = this.World.GetStash<Teg_For_parentGO_AI_Component>();
            //берем с парент ГО AI тег компоненту (должна быть одна) и устаналиваем для вновь созданных ГО AI данный трансформ как парент
             foreach (var index in _filterAIParentGO)
            {
                ref var _locStashAI_teg = ref _stashAI_teg_Component.Get(index);
                if (_locStashAI_teg._transform != null)
                 _transformParentAI = _locStashAI_teg._transform;
                _locStashAI_teg.arrAI = new AIComponent[_setUpAI.numberAI];
            }

            for (int i = 0; i < _setUpAI.numberAI; i++)
            {
                var entity = this.World.CreateEntity();
                ref var addedAIComponent = ref entity.AddComponent<AIComponent>();

                GameObject newObject = new GameObject();
                addedAIComponent.goAI = newObject;
                newObject.name = "AI_" + i;
                addedAIComponent.nameAI = newObject.name;
                newObject.gameObject.transform.SetParent(_transformParentAI) ;
                addedAIComponent.friendOrEnemy = new Dictionary<string, bool>();

            }
            //в данном цикле устанавливаем кто кому приходится врагом.
            for (int i = 0; i < _setUpAI.numberAI; i++)
            {

                //addedAIComponent.friendOrEnemy.Add();



            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }
    }
}