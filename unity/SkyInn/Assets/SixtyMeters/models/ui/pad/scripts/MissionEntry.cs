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

        public void SetMission(Mission mission)
        {
            _mission = mission;
            titleElement.text = mission.title;
            descriptionElement.text = mission.description;
            percentageCompleteElement.value = mission.GetPercentageCompleted();
            rewardCoinAmountElement.text = mission.rewardAmount + "";

            if (mission.IsComplete())
            {
                claimButtonEnabled.SetActive(true);
                claimButtonDisabled.SetActive(false);
            }
            else
            {
                claimButtonEnabled.SetActive(false);
                claimButtonDisabled.SetActive(true);
            }
        }

        public void ClaimReward()
        {
            _mission.Claim();
            Destroy(gameObject);
        }
    }
}