using UnityEngine;

namespace SixtyMeters.scripts.ai
{
    public class InnCustomerSentiment : MonoBehaviour
    {

        public int maxFoodInStomach;
        public int maxDrinksInStomach;

        /*
     * For the purpose of this simulation we're considering food and drink in the stomach of this npc
     * as separate storage locations. So a npc can still drink even if they've already eaten to the maxFoodInStomach
     * capacity.
     */
        public int currentFoodInStomach;
        public int currentDrinksInStomach;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool WantsToDrink()
        {
            return currentDrinksInStomach < maxDrinksInStomach;
        }


        public bool WantsToEat()
        {
            return currentFoodInStomach < maxFoodInStomach;
        }
    }
}
