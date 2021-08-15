using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.scripts.items;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private IkControl _ikControl;
    
    public GameObject leftHand;
    public GameObject rightHand;

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
        Debug.Log("Equiping "+item.name+" in right hand");
        item.GetComponent<UsableByNpc>().isEquipped = true;
        Vector3 startPos = _ikControl.rightHandControl.position;
        
        _ikControl.RightHandFollow(item.transform);
        
        StartCoroutine(MoveHandBack(item, startPos));
    }
    
    IEnumerator MoveHandBack(GameObject item, Vector3 startPos)
    {
        yield return new WaitForSeconds(5);  //Gives the system enough time to move to item
        Equip(rightHand, item);
        _ikControl.RightHandStopFollowing();
    }

    public void EquipLefHand(GameObject item)
    {
        Equip(leftHand, item);
    }

    private static void Equip(GameObject slot, GameObject item)
    {
        //Copying the item prevents it from flying through the world and be placed at a weird position
        var npcItem = Instantiate(item, slot.transform, true);

        //Disable components that are not needed while equipped by an NPC
        npcItem.GetComponent<UsableByNpc>().isEquipped = true;
        npcItem.GetComponent<PointOfInterest>().enabled = false;
        npcItem.GetComponent<Rigidbody>().isKinematic = true;

        //Destroy the original item after creating a copy
        Destroy(item);
        
        var rightHandOrientation = npcItem.GetComponent<UsableByNpc>().rightHandRotation;

        npcItem.transform.position = slot.transform.position;
        npcItem.transform.localRotation = Quaternion.Euler(rightHandOrientation);
    }
}
