using UnityEngine;

namespace SixtyMeters.characters.adventurers.scripts
{
    public class InnCustomerStats : MonoBehaviour
    {

        public int hunger = 0;
        public int thirst = 100;
        public int coins = 100;

        public GameObject coin;
        
        // Start is called before the first frame update
        void Start()
        {
            //TODO: randomize stats when a new InnCustomer is spawned
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool IsHungry()
        {
            return hunger > 0;
        }
        
        public bool IsThirsty()
        {
            return thirst > 0;
        }

        public void Drink(int amount)
        {
            thirst -= amount;
        }

        public void PayCoins(int amount, Transform paymentLocation)
        {
            coins -= amount;
            for (var c = 0; c < amount; c++)
            {
                var paymentLocTransform = paymentLocation.transform;
                Instantiate(coin, paymentLocTransform.position, paymentLocTransform.rotation);
            }
        }
    }
}
