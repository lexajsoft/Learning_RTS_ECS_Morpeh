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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
    public sealed class MovementSystem : UpdateSystem 
    {
        private Filter _filter;
    
        public override void OnAwake() 
        {
            World = WorldManager.WorldDefault;
            _filter = World.Filter
                .With<MovementComponent>()
                .With<PositionComponent>()
                .With<TransformComponent>()
                .Build();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in _filter)
            {
                ref var movementComponent = ref entity.GetComponent<MovementComponent>();
                ref var positionComponent = ref entity.GetComponent<PositionComponent>();
                ref var transformComponent = ref entity.GetComponent<TransformComponent>();

                positionComponent.Pos = movementComponent.Transform.position;
                
                if (entity.Has<RotationComponent>())
                {
                    ref var rotationComponent = ref entity.GetComponent<RotationComponent>();
                    // доп проверка, если направления нет то и двигаться не будем
                    if (movementComponent.Direct.magnitude > 0.01f)
                    {
                        //var offset = rotationComponent.DirectToLook * (movementComponent.Speed*500 * deltaTime);
                        //transformComponent.Transform.position = positionComponent.Pos;
                        
                        var offset = rotationComponent.DirectToLook * (movementComponent.Speed);
                        movementComponent.Rigidbody.AddForce(offset);
                    }
                }
                else
                {
                    //var offset = movementComponent.Direct * (movementComponent.Speed*500 * deltaTime);
                    var offset = movementComponent.Direct * (movementComponent.Speed);
                    // positionComponent.Pos += offset;
                    // transformComponent.Transform.position = positionComponent.Pos;
                    movementComponent.Rigidbody.AddForce(offset);
                }

                
            }
        }
    }
}