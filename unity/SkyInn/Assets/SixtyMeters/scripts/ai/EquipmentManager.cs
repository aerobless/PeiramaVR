using System.Collections;
using SixtyMeters.scripts.items;
using UnityEngine;

namespace SixtyMeters.scripts.ai
{
    public class EquipmentManager : MonoBehaviour
    {
        private IkControl _ikControl;

        // Hands
        public GameObject leftHand;
        public GameObject rightHand;

        // Items in Hand
        public GameObject itemInLeftHand;
        public GameObject itemInRightHand;

        // Position where items were picked up, so they can be dropped there again after use
        private Vector3 _leftHandDropPosition;
        private Quaternion _leftHandDropRotation;
        private Vector3 _rightHandDropPosition;
        private Quaternion _rightHandDropRotation;

        // Start is called before the first frame update
        void Start()
        {
            _ikControl = GetComponent<IkControl>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void EquipRightHand(GameObject item)
        {
            Debug.Log("Equiping " + item.name + " in right hand");
            item.GetComponent<UsableByNpc>().isEquipped = true;
            Vector3 startPos = _ikControl.rightHandControl.position;

            _ikControl.RightHandFollow(item.transform);
            _rightHandDropPosition = item.transform.position;
            _rightHandDropRotation = item.transform.rotation;

            StartCoroutine(MoveRightHandBack(item, startPos));
        }

        IEnumerator MoveRightHandBack(GameObject item, Vector3 startPos)
        {
            yield return new WaitForSeconds(5); //Gives the system enough time to move to item
            Equip(rightHand, item);
            _ikControl.RightHandStopFollowing();
        }

        IEnumerator MoveLeftHandBack(GameObject item, Vector3 startPos)
        {
            yield return new WaitForSeconds(5); //Gives the system enough time to move to item
            Equip(leftHand, item);
            _ikControl.LeftHandStopFollowing();
        }

        public void EquipLefHand(GameObject item)
        {
            Debug.Log("Equiping " + item.name + " in left hand");
            item.GetComponent<UsableByNpc>().isEquipped = true;
            Vector3 startPos = _ikControl.leftHandControl.position;

            _ikControl.LeftHandFollow(item.transform);
            _leftHandDropPosition = item.transform.position;
            _leftHandDropRotation = item.transform.rotation;

            StartCoroutine(MoveLeftHandBack(item, startPos));
        }

        private void Equip(GameObject slot, GameObject item)
        {
            //Copying the item prevents it from flying through the world and be placed at a weird position
            var npcItem = Instantiate(item, slot.transform, true);

            //Disable components that are not needed while equipped by an NPC
            npcItem.GetComponent<UsableByNpc>().isEquipped = true;
            npcItem.GetComponent<PointOfInterest>().enabled = false;
            npcItem.GetComponent<Rigidbody>().isKinematic = true;

            //Destroy the original item after creating a copy
            Destroy(item);

            //TODO: fix for left hand
            var rightHandOrientation = npcItem.GetComponent<UsableByNpc>().rightHandRotation;

            npcItem.transform.position = slot.transform.position;
            npcItem.transform.localRotation = Quaternion.Euler(rightHandOrientation);
            itemInRightHand = npcItem; //TODO: fix for left hand
        }

        public void DropRightHand()
        {
            if (itemInRightHand != null)
            {
                _ikControl.RightHandDestination(_rightHandDropPosition);
                StartCoroutine(DropItemsAndMoveHandBack(itemInRightHand));
            }
        }

        private IEnumerator DropItemsAndMoveHandBack(GameObject item)
        {
            yield return new WaitForSeconds(5); //Gives the system enough time to move to the drop position
            
            item.SetActive(false);
            var droppedItem = Instantiate(item, _rightHandDropPosition, _rightHandDropRotation);
            droppedItem.GetComponent<UsableByNpc>().isEquipped = false;
            droppedItem.GetComponent<PointOfInterest>().enabled = true;
            droppedItem.GetComponent<Rigidbody>().isKinematic = false;
            droppedItem.SetActive(true);
            //Destroy the original item after creating a copy
            Destroy(item.gameObject);
            _ikControl.RightHandStopFollowing();
        }
    }
}