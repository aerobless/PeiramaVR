using UnityEngine;

namespace SixtyMeters.scripts.level
{
    public class PlayerStatManager : MonoBehaviour
    {
        public int coinsInJar = 0;

        public void AddCoins(int amount)
        {
            coinsInJar += amount;
        }

        public bool VerifyAndCharge(int amount)
        {
            if (coinsInJar < amount) return false;
            
            coinsInJar -= amount;
            return true;
        }
    }
}