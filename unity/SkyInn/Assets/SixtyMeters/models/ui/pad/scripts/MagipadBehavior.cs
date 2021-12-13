using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class MagipadBehavior : MonoBehaviour
    {

        public AudioSource audioSource;

        public AudioClip notification;
        public AudioClip click;
        public AudioClip coinReward;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PlayClickAudio()
        {
            audioSource.PlayOneShot(click);
        }
        public void PlayNotificationAudio()
        {
            audioSource.PlayOneShot(notification);
        }
        public void PlayCoinRewardAudio()
        {
            audioSource.PlayOneShot(coinReward);
        }

    }
}
