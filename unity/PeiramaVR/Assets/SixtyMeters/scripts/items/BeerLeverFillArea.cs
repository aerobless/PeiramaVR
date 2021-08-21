using UnityEngine;

namespace SixtyMeters.scripts.items
{
    public class BeerLeverFillArea : MonoBehaviour
    {

        public BeerLever lever;
        
        private float _fillingTimePassed;
        private Mug _mug;

        private float fillIncrement = 0.1f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_mug && lever.isDispensing)
            {
                _fillingTimePassed += Time.deltaTime;
                if (_fillingTimePassed >= fillIncrement)
                {
                    _mug.FillMugByIncrement();
                    _fillingTimePassed = 0;
                }
            }
        }
    
        void OnTriggerEnter(Collider col)
        {
            if (IsMug(col))
            {
                _mug = col.gameObject.GetComponent<Mug>();
            }
        }
    
        void OnTriggerExit(Collider col)
        {
            if (IsMug(col))
            {
                _mug = null;
            }
        }
    
        private static bool IsMug(Collider col)
        {
            return col.gameObject.GetComponent<Mug>() != null;
        }
    }
}
