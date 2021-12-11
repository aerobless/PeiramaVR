using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class PanelBehaviour : MonoBehaviour
    {

        
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
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
            gameObject.SetActive(false);
        }
    }
}
