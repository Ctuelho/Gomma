using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gomma
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance = null;

        [SerializeField] private PlantStatistics _plantStatistics;
        [SerializeField] private ExcrementStatistics _excrementStatistics;

        [SerializeField] private Text _energyFarmed;
        [SerializeField] private Text _energyUsed;

        private int _currentEnergyFarmed = 0;
        private int _currentEnergyUsed = 0;

        private void Awake()
        {
            Instance = this;
            UpdateScores();
        }

        private void UpdateScores()
        {
            _energyFarmed.text = "Collected: " + _currentEnergyFarmed.ToString();
            _energyUsed.text = "Used: " + _currentEnergyUsed.ToString();
        }

        public static PlantStatistics PlantStatistics => Instance._plantStatistics;

        public static ExcrementStatistics ExcrementStatistics => Instance._excrementStatistics;
        
        public static void AddFarmedEnergy(int amount)
        {
            Instance._currentEnergyFarmed += amount;
            Instance.UpdateScores();
        }

        public static void AddUsedEnergy(int amount)
        {
            Instance._currentEnergyUsed += amount;
            Instance.UpdateScores();
        }

        public static int Profit => Instance._currentEnergyFarmed - Instance._currentEnergyUsed;
    }
}