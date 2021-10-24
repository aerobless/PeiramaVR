using UnityEngine;

namespace SixtyMeters.models.portal.scripts
{
    public class PortalRuneController : MonoBehaviour
    {

        public GameObject portalPlane;

        private PortalBehaviour _portalBehaviour;
        
        // Start is called before the first frame update
        void Start()
        {
            _portalBehaviour = portalPlane.GetComponent<PortalBehaviour>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var portalRune = other.GetComponent<PortalRune>();
            if (portalRune != null)
            {
                _portalBehaviour.OpenPortal();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var portalRune = other.GetComponent<PortalRune>();
            if (portalRune != null)
            {
                _portalBehaviour.ClosePortal();
            }
        }
    }
}