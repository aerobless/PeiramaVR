using SixtyMeters.characters.monsters.goblins.ai;
using UnityEngine;

namespace SixtyMeters.scripts.items.weapons
{
    public class MeleeWeapon : MonoBehaviour
    {

        public int dmgPerHit = 25;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            var otherGameObject = other.transform.root.gameObject;
            if (otherGameObject.tag.Equals("Enemy"))
            {
                var goblinAi = otherGameObject.GetComponentInChildren<GoblinAIv2>();
                goblinAi.TakeDamage(dmgPerHit);
            }
        }
    }
}
