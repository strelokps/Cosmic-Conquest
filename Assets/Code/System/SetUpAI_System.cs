using System.Collections.Generic;
using System.Linq;
using Assets.Code.Component;
using Assets.Code.ScriptableObject;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
        private Stash<AIComponent> _stashAIComponents;


        public override void OnAwake()
        {
            //игрока подгружаем что бы исключить цвет игрока при назначении AI
            _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
            _setUpAI = Resources.Load<SetUpAI>("SetUpAISO");
            _filterAIParentGO = this.World.Filter.With<Teg_For_parentGO_AI_Component>();
            _stashAI_teg_Component = this.World.GetStash<Teg_For_parentGO_AI_Component>();

            CreatAI();
            _filterAIComponent = this.World.Filter.With<AIComponent>();
            _stashAIComponents = this.World.GetStash<AIComponent>();


            SetTeams();
            
        }

        //Создаем AI
        private void CreatAI()
        {
            //берем с парент ГО AI тег компоненту (должна быть одна) и устаналиваем для вновь созданных ГО AI данный трансформ как парент
            foreach (var index in _filterAIParentGO)
            {
                ref var _locStashAI_teg = ref _stashAI_teg_Component.Get(index);
                if (_locStashAI_teg._transform != null)
                    _transformParentAI = _locStashAI_teg._transform;
                _locStashAI_teg.arrAI = new AIComponent[_setUpAI.numAI_SO]; // инициируем массив AI
            }

            for (int i = 0; i < _setUpAI.numAI_SO; i++)
            {
                var entity = this.World.CreateEntity();
                ref var addedAIComponent = ref entity.AddComponent<AIComponent>();

                GameObject newObject = new GameObject();
                addedAIComponent.goAI = newObject;
                newObject.name = "AI_" + i.ToString();
                addedAIComponent.nameAI = newObject.name;
                newObject.gameObject.transform.SetParent(_transformParentAI);
                addedAIComponent.friendOrEnemy = new Dictionary<string, bool>();

            }
        }

        private void SetTeams()
        {
            if (_setUpPlayer.havePlayerTeam)
            {
                if (_setUpPlayer.howManyAIinTeam < _setUpAI.numAI_SO)
                {
                    int countTeam = _setUpPlayer.howManyAIinTeam;

                    foreach (var index in this._filterAIComponent)
                    {
                        
                        ref var _locStashAIComponent = ref _stashAIComponents.Get(index);
                        if (_locStashAIComponent.friendOrEnemy.Count() < 0)
                        {
                            _locStashAIComponent.friendOrEnemy.Add(_setUpPlayer.playerName, true);
                        }

                        _locStashAIComponent.isFriendPlayer = true;
                        countTeam--;
                        Debug.Log($"{_locStashAIComponent.nameAI} {_locStashAIComponent.isFriendPlayer}");
                        if (countTeam <= 0)
                            return;
                    }
                }
                else
                    return;
                        
            }
        }


        public override void OnUpdate(float deltaTime)
        {
            SetTeams();
        }
    }
}