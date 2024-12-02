using UnityEngine;

namespace ECS.Factories
{
    public class TriggerEventCollider : EntityMono
    {
        [SerializeField] private bool _onEnterTriggerEvent = false; 
        [SerializeField] private bool _onStayTriggerEvent = false;
        [SerializeField] private bool _onExitTriggerEvent = false;
        

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (_onEnterTriggerEvent)
            {
                var entityMono = otherCollider.gameObject.GetComponent<EntityMono>();
                if (entityMono != null)
                {
                    TriggerFactory.CreateTriggerEventRequest(_entity, gameObject,entityMono.GetEntity(), otherCollider.gameObject);
                }
            }
        }
        
        private void OnTriggerStay(Collider otherCollider)
        {
            if (_onStayTriggerEvent)
            {
                var otherTriggerEventCollider = otherCollider.gameObject.GetComponent<TriggerEventCollider>();
                TriggerFactory.CreateTriggerEventRequest(_entity,gameObject,otherTriggerEventCollider.GetEntity(), otherCollider.gameObject);
            }
        }
        
        private void OnTriggerExit(Collider otherCollider)
        {
            if (_onExitTriggerEvent)
            {
                var otherTriggerEventCollider = otherCollider.gameObject.GetComponent<TriggerEventCollider>();
                TriggerFactory.CreateTriggerEventRequest(_entity,gameObject,otherTriggerEventCollider.GetEntity(), otherCollider.gameObject);
            }
        }
    }
}