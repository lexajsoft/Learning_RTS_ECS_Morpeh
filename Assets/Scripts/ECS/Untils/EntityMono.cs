using Scellecs.Morpeh;
using UnityEngine;

namespace ECS.Factories
{
    public class EntityMono : MonoBehaviour
    {
        [SerializeField] protected Entity _entity;
        
        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        public Entity GetEntity()
        {
            return _entity;
        }
    }
}