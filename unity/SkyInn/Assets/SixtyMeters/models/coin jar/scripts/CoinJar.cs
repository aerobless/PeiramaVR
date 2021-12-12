using SixtyMeters.scripts;
using SixtyMeters.scripts.level;
using UnityEngine;

namespace SixtyMeters.models.coin_jar.scripts
{
    public class CoinJar : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip coinDropped;
        public TextDisplay textDisplay;
        
        private PlayerStatManager _playerStatManager;

        // Start is called before the first frame update
        void Start()
        {
            _playerStatManager = FindObjectOfType<PlayerStatManager>();
        }

        // Update is called once per frame
        void Update()
        {
            textDisplay.SetText(_playerStatManager.coinsInJar + " Coins");
        }

        void OnCollisionEnter(Collision collision)
        {
            if (IsCoin(collision))
            {
                _playerStatManager.coinsInJar += collision.gameObject.GetComponent<Coin>().coinValue;
                Destroy(collision.gameObject);
                audioSource.PlayOneShot(coinDropped);
                textDisplay.SetText(_playerStatManager.coinsInJar + " Coins");
            }
        }

        private bool IsCoin(Collision col)
        {
            var coin = col.gameObject.GetComponent<Coin>();
            return coin != null;
        }
    }
}