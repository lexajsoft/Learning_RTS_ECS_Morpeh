using ECS.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ECS
{
    public class SpawnButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMPro.TextMeshProUGUI _name;
        [SerializeField] private TMPro.TextMeshProUGUI _creationTime;
        private EntityDescriptionScriptableObject _entityDescriptionScriptableObject;
        
        public UnityAction<EntityDescriptionScriptableObject> OnClick;

        public void SetData(EntityDescriptionScriptableObject data)
        {
            _entityDescriptionScriptableObject = data;

            _name.text = _entityDescriptionScriptableObject.GetEntityDescriptionData().Name;
            _creationTime.text = ((int)_entityDescriptionScriptableObject.GetEntityDescriptionData().CreationTime).ToString() + "Сек.";
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);            
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
        }

        private void Click()
        {
            OnClick?.Invoke(_entityDescriptionScriptableObject);
        }
    }
}