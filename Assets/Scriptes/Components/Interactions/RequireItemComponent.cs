using UnityEngine;
using UnityEngine.Events;
using PixelCrew.Model;
using PixelCrew.Model.Data;

namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _requiredItems;
        [SerializeField] private bool _removeAfterUse;
        [SerializeField] private UnityEvent _onSussess;
        [SerializeField] private UnityEvent _onFail;

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            bool areAllRequirementsMet = true;
            foreach (var item in _requiredItems)
            { 
                if(session.Data.Inventory.Count(item.Id) < item.Value)
                {
                    areAllRequirementsMet = false;
                    break;
                }
            }

            if (areAllRequirementsMet)
            {
                if(_removeAfterUse)
                {
                    foreach (var item in _requiredItems)
                        session.Data.Inventory.Remove(item.Id, item.Value);
                }
                _onSussess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}

