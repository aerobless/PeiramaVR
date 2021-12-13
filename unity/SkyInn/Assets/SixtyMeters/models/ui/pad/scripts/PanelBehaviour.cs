using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class PanelBehaviour : MonoBehaviour
    {

        public bool activeAtStart = false;

        private MagipadBehavior _magipadBehavior;
        
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(activeAtStart);
            _magipadBehavior = GetComponentInParent<MagipadBehavior>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowPanel()
        {
            gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            _magipadBehavior.PlayClickAudio();
            gameObject.SetActive(false);
        }
    }
}
