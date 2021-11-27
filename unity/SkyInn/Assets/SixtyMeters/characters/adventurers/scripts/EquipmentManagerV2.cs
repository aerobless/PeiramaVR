using System;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;

namespace SixtyMeters.characters.adventurers.scripts
{
    public class EquipmentManagerV2 : MonoBehaviour
    {
        public PropMuscle rightHand;
        public PropMuscle leftHand;

        private Dictionary<int, Vector3> _itemOriginPositions;

        // Start is called before the first frame update
        void Start()
        {
            _itemOriginPositions = new Dictionary<int, Vector3>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Equip(PuppetMasterProp item, EquipmentSlot slot)
        {
            _itemOriginPositions.Add(item.GetInstanceID(), item.transform.position);
            switch (slot)
            {
                case EquipmentSlot.RightHand:
                    rightHand.currentProp = item;
                    break;
                case EquipmentSlot.LeftHand:
                    leftHand.currentProp = item;
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
                    var prop = rightHand.currentProp;
                    if (prop != null)
                    {
                        var itemOriginPostion = _itemOriginPositions[rightHand.currentProp.GetInstanceID()];
                        rightHand.currentProp = null;
                        prop.transform.position = itemOriginPostion;
                    }
                    break;
                case EquipmentSlot.LeftHand:
                    //TODO: fixme
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }
    }
}
