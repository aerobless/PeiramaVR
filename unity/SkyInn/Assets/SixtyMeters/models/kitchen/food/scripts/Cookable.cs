using UnityEngine;

namespace SixtyMeters.models.kitchen.food.scripts
{
    public class Cookable : MonoBehaviour
    {
        private Renderer _renderer;

        public float cookTimeSec = 30;
        public bool canBurn = true;
        public bool isCooking = false;

        public AudioClip cookingSound;

        private AudioSource _audioSource;

        private float _timeCooked = 0;
        private static readonly int CookRate = Shader.PropertyToID("_cookRate");
        private static readonly int BurnRate = Shader.PropertyToID("_burnRate");

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = cookingSound;
        }

        // Update is called once per frame
        void Update()
        {
            if (isCooking)
            {
                var cookValue = Mathf.Lerp(0, 1, _timeCooked / cookTimeSec);
                if (cookValue > 1)
                {
                    cookValue = 1;
                }
                _renderer.material.SetFloat(CookRate, cookValue);

                if (_timeCooked > cookTimeSec)
                {
                    var burnValue = Mathf.Lerp(0, 1, _timeCooked / (cookTimeSec*2));
                    if (burnValue > 1)
                    {
                        burnValue = 1;
                    }
                    _renderer.material.SetFloat(BurnRate, burnValue);
                }
                
                _timeCooked += Time.deltaTime;
            }
        }

        public void StartCooking()
        {
            isCooking = true;
            _audioSource.Play();
        }
        
        public void StopCooking()
        {
            isCooking = false;
            _audioSource.Stop();
        }
    }
}
