using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Assets.Code
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlanetsSystem))]
    public sealed class PlanetsSystem : UpdateSystem
    {
        private Filter _filterPlanets;
        private Stash<PlanetComponent> _stashPlanet;
        public override void OnAwake()
        {
            this._filterPlanets = this.World.Filter.With<PlanetComponent>();
            this._stashPlanet = this.World.GetStash<PlanetComponent>();
            GetPlanetFromFiltr();
        }

        public override void OnUpdate(float deltaTime)
        {

        }

        private void GetPlanetFromFiltr()
        {
            foreach (var entity in _filterPlanets)
            {
                ref var filterPlanetComponents = ref _stashPlanet.Get(entity);
                Debug.Log($"{filterPlanetComponents.colorPlanet}");
            }
        }
    }
}