using SixtyMeters.scripts.level.shop;
using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class ShopPanel : MonoBehaviour
    {
        public ShopManager shopManager;
        public GameObject itemEntryPrototype;
        public GameObject layout;

        private bool _initialized = false;
    
        // Start is called before the first frame update
        void Start()
        {
            AddShopEntries();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_initialized)
            {
                AddShopEntries();
                _initialized = true;
            }
        
        }

        private void AddShopEntries()
        {
            foreach (var shopItem in shopManager.GetItemsForSale())
            {
                GameObject entry = Instantiate(itemEntryPrototype, layout.transform);
                entry.GetComponent<ShopItemEntry>().SetShopItem(shopItem);
            }
        }
    }
}
