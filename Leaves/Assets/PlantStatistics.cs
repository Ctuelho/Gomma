using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    [CreateAssetMenu(fileName = "PlantsStatistics", menuName = "ScriptableObjects/PlantsStatistics")]
    public class PlantStatistics : ScriptableObject
    {
        [SerializeField] [Range(1, 3600)] private int _minNourishmentPerFertilization = 60;
        [SerializeField] [Range(1, 3600)] private int _maxNourishmentPerFertilization = 90;
        [SerializeField] [Range(1, 3600)] private int _minSustenancePerStage = 60;
        [SerializeField] [Range(1, 3600)] private int _maxSustenancePerStage = 90;
        [SerializeField] private GameObject _smallFruitPrefab;
        [SerializeField] private GameObject _averageFruitPrefab;
        [SerializeField] private GameObject _largeFruitPrefab;

        public int NourishmentPerFertilization { get => Random.Range(_minNourishmentPerFertilization, _maxNourishmentPerFertilization + 1); }
        public int MinSustenancePerStage { get => _minSustenancePerStage; }
        public int MaxSustenancePerStage { get => _maxSustenancePerStage; }

        public GameObject GetRandomFruit()
        {
            //20 60 20
            var random = Random.Range(1, 101);
            if (random > 80)
            {
                return _largeFruitPrefab;
            }
            else if (random > 20)
            {
                return _averageFruitPrefab;
            }
            else
                return _smallFruitPrefab;
        }
    }
}
