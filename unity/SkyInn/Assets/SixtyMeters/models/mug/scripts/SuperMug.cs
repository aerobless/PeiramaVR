using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        Vector3 direction = (this.transform.position - other.transform.position) /
                            (this.transform.position - other.transform.position).magnitude;
        other.transform.GetComponent<Rigidbody> ().AddForce (50, 50, 0, ForceMode.Impulse);
        
    }
}
