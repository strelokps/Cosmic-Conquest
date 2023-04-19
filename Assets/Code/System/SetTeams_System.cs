using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Assets.Code.ScriptableObject;
using Assets.Code.Component;
using Scellecs.Morpeh;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SetTeams_System))]
public sealed class SetTeams_System : UpdateSystem 
{
    private SetUpPlayer _setUpPlayer;
    private SetUpAI _setUpAI;
    private Filter _filterAIComponent;
    private Stash<AIComponent> _stashAIComponents;


    public override void OnAwake()
    {
        _setUpPlayer = Resources.Load<SetUpPlayer>("SetUpPlayerSO");
        _setUpAI = Resources.Load<SetUpAI>("SetUpAISO");
        _filterAIComponent = this.World.Filter.With<AIComponent>();
        _stashAIComponents = this.World.GetStash<AIComponent>();
        SetTeams();
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
                    if (_locStashAIComponent.friendOrEnemy.Count < 0)
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

    public override void OnUpdate(float deltaTime) {
    }
}