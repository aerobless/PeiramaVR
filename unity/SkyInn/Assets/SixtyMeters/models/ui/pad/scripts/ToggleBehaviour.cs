using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class ToggleBehaviour : MonoBehaviour
    {
        public Toggle toggle;
        public TextMeshProUGUI text;

        private bool _state;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void setValue(bool toggle)
        {
            this.toggle.isOn = toggle;
        }

        public bool getValue()
        {
            return toggle.isOn;
        }

        public void setOn()
        {
            text.text = "ON";
        }
    
        public void setOff()
        {
            text.text = "OFF";
        }
    }
}
