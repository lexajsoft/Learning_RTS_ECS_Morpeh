using ECS.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ECS.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DestroyAfterTimeSystem))]
    public sealed class DestroyAfterTimeSystem : UpdateSystem
    {

        private Filter _filter;
        public override void OnAwake()
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter.With<DestroyAfterComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            int countDestoryed = 0;
            foreach (var entity in _filter)
            {
                ref var destoryAfterComponent = ref entity.GetComponent<DestroyAfterComponent>();
                if (destoryAfterComponent.Remain > 0)
                {
                    destoryAfterComponent.Remain -= deltaTime;
                }
                else
                {
                    if(destoryAfterComponent.GameObject != null)
                        Destroy(destoryAfterComponent.GameObject);
                    
                    World.RemoveEntity(entity);
                    countDestoryed++;
                }
            }
            if(countDestoryed > 0)
                World.Commit();
        }
    }
}