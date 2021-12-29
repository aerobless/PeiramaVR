using System;
using UnityEngine;

namespace SixtyMeters.models.kitchen.food.scripts
{
    public class StoveBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            var food = other.GetComponent<FoodItem>();
            if (food)
            {
                food.StartCooking();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var food = other.GetComponent<FoodItem>();
            if (food)
            {
                food.StopCooking();
            }
        }
    }
}
