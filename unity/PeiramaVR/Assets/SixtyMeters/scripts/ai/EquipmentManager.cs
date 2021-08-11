using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.scripts.items;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;

    private GameObject _itemInLeftHand;
    private GameObject _itemInRightHand;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipRightHand(GameObject item)
    {
        Debug.Log("Equiping "+item.name+" in right hand");
        _itemInRightHand = item;
        Equip(rightHand, _itemInRightHand);
    }

    public void EquipLefHand(GameObject item)
    {
        _itemInLeftHand = item;
        Equip(leftHand, _itemInLeftHand);
    }

    private static void Equip(GameObject slot, GameObject item)
    {
        //Copying the item prevents it from flying through the world and be placed at a weird position
        var npcItem = Instantiate(item, slot.transform, true);
        npcItem.transform.position = slot.transform.position;
        npcItem.GetComponent<Rigidbody>().isKinematic = true;
    }
}
