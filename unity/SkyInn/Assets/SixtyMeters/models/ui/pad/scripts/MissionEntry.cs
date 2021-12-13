using System;
using SixtyMeters.scripts.level.missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class MissionEntry : MonoBehaviour
    {
        //TODO: different icons for different missions
        //TODO: different reward types, exp?

        //TODO: think about live updates - what happens when the pad is open and a mission is completed?

        public TextMeshProUGUI titleElement;
        public TextMeshProUGUI descriptionElement;
        public Slider percentageCompleteElement;
        public TextMeshProUGUI rewardCoinAmountElement;

        public GameObject claimButtonEnabled;
        public GameObject claimButtonDisabled;

        private Mission _mission;

        private MagipadBehavior _magipadBehavior;

        private bool oneShotComplete = false;

        private void Start()
        {
            _magipadBehavior = GetComponentInParent<MagipadBehavior>();
        }

        private void Update()
        {
            percentageCompleteElement.value = _mission.GetPercentageCompleted();
            if (_mission.IsComplete() && oneShotComplete == false)
            {
                oneShotComplete = true; //prevents re-triggering this on every update
                EnableClaimButton();
                _magipadBehavior.PlayNotificationAudio();
            }
        }

        public void SetMission(Mission mission)
        {
            _mission = mission;
            titleElement.text = _mission.title;
            descriptionElement.text = _mission.description;
            percentageCompleteElement.value = _mission.GetPercentageCompleted();
            rewardCoinAmountElement.text = _mission.rewardAmount + "";

            if (_mission.IsComplete())
            {
                EnableClaimButton();
            }
            else
            {
                DisableClaimButton();
            }
        }
        
        private void EnableClaimButton()
        {
            claimButtonEnabled.SetActive(true);
            claimButtonDisabled.SetActive(false);
        }

        private void DisableClaimButton()
        {
            claimButtonEnabled.SetActive(false);
            claimButtonDisabled.SetActive(true);
        }

        public void ClaimReward()
        {
            _mission.Claim();
            _magipadBehavior.PlayCoinRewardAudio();
            Destroy(gameObject, 0.1f);
        }
    }
}