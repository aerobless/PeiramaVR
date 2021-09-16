using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyJar : MonoBehaviour
{

    public int money;

    public AudioSource audioSource;
    public AudioClip coinDropped;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (IsCoin(collision))
        {
            money += collision.gameObject.GetComponent<Coin>().coinValue;
            Destroy(collision.gameObject);
            audioSource.PlayOneShot(coinDropped);
        }
    }

    private bool IsCoin(Collision col)
    {
        var coin = col.gameObject.GetComponent<Coin>();
        return coin != null;
    }
}
