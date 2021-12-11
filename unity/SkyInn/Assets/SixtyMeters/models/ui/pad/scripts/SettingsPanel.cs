using System;
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

        public Toggle sitting;
        public Toggle standing;
        public Toggle height;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize UI controls
            snapTurningRateSlider.value = playerController.SnapAmount / 100;
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
            playerController.SnapAmount = snapTurningRateSlider.value * 100;
        }

        public void UpdateSittingStandingHeight(String type)
        {
            switch (type)
            {
                case "Sitting":
                    if (sitting.isOn)
                    {
                        cameraRig.SetSitStandMode(HVRSitStand.Sitting);
                        standing.isOn = false;
                        height.isOn = false;
                    }

                    break;
                case "Standing":
                    if (standing.isOn)
                    {
                        cameraRig.SetSitStandMode(HVRSitStand.Standing);
                        sitting.isOn = false;
                        height.isOn = false;
                    }

                    break;
                case "PlayerHeight":
                    if (height.isOn)
                    {
                        cameraRig.SetSitStandMode(HVRSitStand.PlayerHeight);
                        sitting.isOn = false;
                        standing.isOn = false;
                    }

                    break;
            }
        }
    }
}