using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    [CreateAssetMenu(fileName = "ExcrementStatistics", menuName = "ScriptableObjects/ExcrementStatistics")]
    public class ExcrementStatistics : ScriptableObject
    {
        [SerializeField] [Range(1, 3600)] private int _smallMinEnergy = 90;
        [SerializeField] [Range(1, 3600)] private int _smallMaxEnergy = 120;
        [SerializeField] [Range(1, 3600)] private int _averageMinEnergy = 150;
        [SerializeField] [Range(1, 3600)] private int _averageMaxEnergy = 210;
        [SerializeField] [Range(1, 3600)] private int _largeMinEnergy = 240;
        [SerializeField] [Range(1, 3600)] private int _largeMaxEnergy = 330;
        [SerializeField] [Range(1, 3600)] private int _smallEatingTime = 20;
        [SerializeField] [Range(1, 3600)] private int _averageEatingTime = 30;
        [SerializeField] [Range(1, 3600)] private int _largeEatingTime = 40;
        [SerializeField] private GameObject _smallExcrementPrefab;
        [SerializeField] private GameObject _averageExcrementPrefab;
        [SerializeField] private GameObject _largeExcrementPrefab;

        public int SmallEnergy { get => _smallMaxEnergy; }//Random.Range(_smallMinEnergy, _smallMaxEnergy+1); }
        public int AverageEnergy { get => _averageMaxEnergy; }// Random.Range(_averageMinEnergy, _averageMaxEnergy + 1); }
        public int LargeEnergy { get => _largeMaxEnergy; }// Random.Range(_largeMinEnergy, _largeMaxEnergy + 1); }

        public int GetEnergyByTypeOfExcrement(ItemTypes excrementType)
        {
            switch (excrementType)
            {
                default:
                    return SmallEnergy;
                case ItemTypes.FRUIT_SMALL:
                    return SmallEnergy;
                case ItemTypes.FRUIT_AVERAGE:
                    return AverageEnergy;
                case ItemTypes.FRUIT_LARGE:
                    return LargeEnergy;
            }
        }

        public int GetFruitEatingTime(ItemTypes fruitType)
        {
            switch (fruitType)
            {
                default:
                    return _smallEatingTime;
                case ItemTypes.FRUIT_SMALL:
                    return _smallEatingTime;
                case ItemTypes.FRUIT_AVERAGE:
                    return _averageEatingTime;
                case ItemTypes.FRUIT_LARGE:
                    return _largeEatingTime;
            }
        }
        public GameObject GetExcrementPrefab(ItemTypes fruitType)
        {
            switch (fruitType)
            {
                default:
                    return _smallExcrementPrefab;
                case ItemTypes.FRUIT_SMALL:
                    return _smallExcrementPrefab;
                case ItemTypes.FRUIT_AVERAGE:
                    return _averageExcrementPrefab;
                case ItemTypes.FRUIT_LARGE:
                    return _largeExcrementPrefab;
            }
        }

    }
}