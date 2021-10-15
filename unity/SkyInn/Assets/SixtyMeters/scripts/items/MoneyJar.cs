using SixtyMeters.scripts;
using UnityEngine;

public class MoneyJar : MonoBehaviour
{

    public int money;

    private int _internalMoney = 0;

    public AudioSource audioSource;
    public AudioClip coinDropped;
    public TextDisplay textDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (money != _internalMoney)
        {
            _internalMoney = money;
            textDisplay.SetText(_internalMoney+" Coins");
        }
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
