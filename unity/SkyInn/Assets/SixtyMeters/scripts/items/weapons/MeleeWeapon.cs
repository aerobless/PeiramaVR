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
                Vector3 dir = other.contacts[0].point - transform.position;
                dir.Normalize();
                
                var magnitude = 5000;

                // We then get the opposite (-Vector3) and normalize it
                //dir = -dir.normalized;
                // And finally we add force in the direction of dir and multiply it by force. 
                // This will push back the player
                
                other.gameObject.GetComponent<Rigidbody>().AddForce(dir * magnitude);
                //GetComponent<Rigidbody>().AddForce(dir*force);
            }
        }
    }
}
