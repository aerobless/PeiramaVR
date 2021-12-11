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
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetDetails(string title, string description, int percentageComplete, int rewardCoinAmount)
        {
            titleElement.text = title;
            descriptionElement.text = description;
            percentageCompleteElement.value = percentageComplete;
            rewardCoinAmountElement.text = rewardCoinAmount+"";

            if (percentageComplete >= 100)
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
    
    }
}
