using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.scripts.level.shop
{
    public class ShopManager : MonoBehaviour
    {

        private List<ShopItem> _shopItems = new();
    
        // Start is called before the first frame update
        void Start()
        {
            LoadShopItems();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void LoadShopItems()
        {
            _shopItems = new List<ShopItem>(GetComponentsInChildren<ShopItem>());
        }

        //TODO: keep limited supply
        //TODO: change items over playtime
        public List<ShopItem> GetItemsForSale()
        {
            return _shopItems;
        }
    }
}