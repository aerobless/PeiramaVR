using UnityEngine;

namespace SixtyMeters.characters.monsters.goblins.ai
{
    public class GoblinAnimator  : MonoBehaviour
    {
        private GoblinAI _goblinAI;
        
        void Start()
        {
            _goblinAI = gameObject.GetComponentInParent<GoblinAI>();
        }
        void OnAnimatorMove ()
        {
            _goblinAI.AnimatorMove();
        }
    }
}