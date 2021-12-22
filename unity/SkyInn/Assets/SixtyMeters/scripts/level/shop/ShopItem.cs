using UnityEngine;

namespace SixtyMeters.scripts.level.shop
{
    public class ShopItem : MonoBehaviour
    {
        public string itemName;
        public int itemPrice;

        public GameObject spawnable;

        private InnLevelManager _innLevelManager;
        private PlayerStatManager _playerStatManager;

        // Start is called before the first frame update
        void Start()
        {
            _innLevelManager = FindObjectOfType<InnLevelManager>();
            _playerStatManager = FindObjectOfType<PlayerStatManager>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void PayAndDeliver()
        {
            if (_playerStatManager.VerifyAndCharge(itemPrice))
            {
                var deliveryTransform = _innLevelManager.deliveryWaypoint.transform;
                Instantiate(spawnable, deliveryTransform.position, deliveryTransform.rotation);
                //TODO: sound effect for successful payment & delivery
            }
            else
            {
                //TODO: sound effect for payment failure
            }

        }
    }
}