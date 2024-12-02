using Scellecs.Morpeh;
using UnityEngine;

namespace ECS
{
    public abstract class GameplayControllerBase : MonoBehaviour
    {
        
        // главное здание которым можно управлять (спавнить юнитов)
        protected Entity _entity;
        
        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        public abstract void Init();
    }
}