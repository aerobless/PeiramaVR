using SixtyMeters.scripts.level.shop;
using TMPro;
using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class ShopItemEntry : MonoBehaviour
    {
    
        //TODO: different icon for different items
        
        public TextMeshProUGUI itemNameElement;
        public TextMeshProUGUI itemCostElement;

        private ShopItem _shopItem;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetShopItem(ShopItem shopItem)
        {
            itemNameElement.SetText(shopItem.itemName);
            itemCostElement.SetText(shopItem.itemPrice+"");
            _shopItem = shopItem;
        }

        public void BuyItem()
        {
            _shopItem.PayAndDeliver();
        }
    }
}
