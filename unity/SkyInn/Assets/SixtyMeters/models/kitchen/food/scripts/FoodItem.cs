using SixtyMeters.characters.adventurers.scripts;
using UnityEngine;

namespace SixtyMeters.models.kitchen.food.scripts
{
    public class FoodItem : MonoBehaviour
    {
        // Settings
        public float cookTimeSec = 30;
        public bool canBurn = true;
        public AudioClip cookingSound;

        // Internals
        private bool _isCooking = false;
        private float _timeCooked = 0;
        private float _edibleRangeStartSec;
        private float _edibleRangeEndSec;

        // Components
        private AudioSource _audioSource;
        private Renderer _renderer;

        // Internal statics
        private static readonly int CookRate = Shader.PropertyToID("_cookRate");
        private static readonly int BurnRate = Shader.PropertyToID("_burnRate");
        private const float CookingGracePercentage = 0.25f; // 25% more or less cooked and it's still edible

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = cookingSound;
            _edibleRangeStartSec = cookTimeSec - (cookTimeSec * CookingGracePercentage);
            _edibleRangeEndSec = cookTimeSec + (cookTimeSec * CookingGracePercentage);
        }

        // Update is called once per frame
        void Update()
        {
            if (_isCooking)
            {
                var cookValue = Mathf.Lerp(0, 1, _timeCooked / cookTimeSec);
                if (cookValue > 1)
                {
                    cookValue = 1;
                }

                _renderer.material.SetFloat(CookRate, cookValue);

                if (_timeCooked > cookTimeSec)
                {
                    var burnValue = Mathf.Lerp(0, 1, _timeCooked / (cookTimeSec * 2));
                    if (burnValue > 1)
                    {
                        burnValue = 1;
                    }

                    _renderer.material.SetFloat(BurnRate, burnValue);
                }

                _timeCooked += Time.deltaTime;
            }
        }

        public bool IsEdible()
        {
            return _timeCooked > _edibleRangeStartSec && _timeCooked < _edibleRangeEndSec;
        }

        public void StartCooking()
        {
            _isCooking = true;
            _audioSource.Play();
        }

        public void StopCooking()
        {
            _isCooking = false;
            _audioSource.Stop();
        }
    }
}