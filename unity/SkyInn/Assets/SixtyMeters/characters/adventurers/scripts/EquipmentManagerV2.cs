using System;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace SixtyMeters.characters.adventurers.scripts
{
    public class EquipmentManagerV2 : MonoBehaviour
    {
        private InteractionSystem _interactionSystem;

        private Dictionary<EquipmentSlot, Vector3> _itemOriginPositions;
        private Dictionary<EquipmentSlot, InteractionObject> _equippedItems;

        // Start is called before the first frame update
        void Start()
        {
            _itemOriginPositions = new Dictionary<EquipmentSlot, Vector3>();
            _equippedItems = new Dictionary<EquipmentSlot, InteractionObject>();
            _interactionSystem = GetComponentInChildren<InteractionSystem>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Equip(InteractionObject item, EquipmentSlot slot)
        {
            _itemOriginPositions.Add(slot, item.transform.position);
            _equippedItems.Add(slot, item);
            switch (slot)
            {
                case EquipmentSlot.RightHand:
                    _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, item, true);
                    break;
                case EquipmentSlot.LeftHand:
                    _interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, item, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        public void Drop(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.RightHand:
                    _interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
                    break;
                case EquipmentSlot.LeftHand:
                    _interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
            var itemOriginPosition = _itemOriginPositions[slot];
            var interactionObject = _equippedItems[slot];
            interactionObject.transform.parent = null;
            interactionObject.GetComponent<Rigidbody>().isKinematic = false;
            interactionObject.transform.position = itemOriginPosition;
            _equippedItems.Remove(slot);
            _itemOriginPositions.Remove(slot);
        }
        
        public InteractionObject GetInteractionObject(EquipmentSlot slot)
        {
            return _equippedItems[slot];
        }
        
    }
}
