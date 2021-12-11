using HurricaneVR.Framework.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class SettingsPanel : MonoBehaviour
    {
        public HVRCameraRig cameraRig;
        public HVRPlayerController playerController;

        public Slider snapTurningRateSlider;

        public ToggleBehaviour smoothTurningToggle;
    
        // Start is called before the first frame update
        void Start()
        {
            // Initialize UI controls
            snapTurningRateSlider.value = playerController.SnapAmount;
            smoothTurningToggle.setValue(playerController.RotationType == RotationType.Smooth);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void CalibrateHeight()
        {
            if (cameraRig)
                cameraRig.Calibrate();
        }
        
        public void UpdateSmoothTurning()
        {
            playerController.RotationType = smoothTurningToggle.getValue() ? RotationType.Smooth : RotationType.Snap;
        }

        public void UpdateSnapTurnRate()
        {
            playerController.SnapAmount = snapTurningRateSlider.value*100;
        }

    }
}
