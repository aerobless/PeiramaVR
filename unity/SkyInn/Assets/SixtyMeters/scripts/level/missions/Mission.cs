using System;
using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.scripts.level.missions
{
    public class Mission : MonoBehaviour
    {
        public string title;
        public string description;
        public int rewardAmount;
        public RewardType rewardType;
        public bool rewardWasClaimed;
        public List<MissionObjective> missionObjectives;

        private PlayerStatManager _playerStatManager;

        //TODO: think about preconditions
        void Start()
        {
            _playerStatManager = FindObjectOfType<PlayerStatManager>();
        }

        public int GetPercentageCompleted()
        {
            if (missionObjectives.Count == 0)
            {
                // If the mission doesn't have any objectives we consider it complete
                return 100;
            }

            // Each objective can have a completion of 0-100, in the end we divide the total value by the number of objectives
            // to get an average completion percentage.
            var totalScore = 0;
            foreach (var missionObjective in missionObjectives)
            {
                totalScore += missionObjective.GetPercentageCompleted();
                totalScore /= missionObjectives.Count;
            }

            return totalScore;
        }

        public bool IsComplete()
        {
            return GetPercentageCompleted() >= 100;
        }

        public void Claim()
        {
            switch (rewardType)
            {
                case RewardType.Coin:
                    _playerStatManager.AddCoins(rewardAmount);
                    break;
                case RewardType.Exp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}