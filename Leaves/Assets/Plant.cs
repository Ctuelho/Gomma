using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class Plant : Interactable
    {
        public enum PlantStates {SPROUT = 0, GROWING = 1, FLOWERING = 2, FRUITING = 3 }

        public enum FruitSizes { SMALL = 0, AVERAGE = 1, LARGE = 2 }

        public bool Nourished { get; private set; }

        [SerializeField] [Range(1, 3600)] private int _growthTime;
        [SerializeField] private GameObject _fruitPosition;
        [SerializeField] private List<GameObject> _treeStages;
        [SerializeField] private ParticleSystem _nourishedEffect;
        [SerializeField] private AudioSource _fertilize;
        [SerializeField] private AudioSource _fruitfy;

        private PlantStates _state = PlantStates.SPROUT;
        private int _energy = 0;
        private int _sustenanceNeeded;
        private float _counter;
        private Interactable _fruit;

        private void Start()
        {
            SetState(PlantStates.SPROUT);
        }

        private void Update()
        {
            if (Nourished)
            {
                _counter += Time.deltaTime;
                while (_counter > 1)
                {
                    _counter -= 1;
                    _energy = Mathf.Max(_energy - 1, 0);

                    _sustenanceNeeded = Mathf.Max(_sustenanceNeeded - 1, 0);
                    if (_sustenanceNeeded <= 0)
                    {
                        //grow the plant
                        Grow();
                    }
                }
            }
        }

        public void Grow()
        {
            _sustenanceNeeded = Random.Range(GameManager.PlantStatistics.MinSustenancePerStage, GameManager.PlantStatistics.MaxSustenancePerStage + 1);
            _energy = 0;
            //SetNourished(false);

            if (_state == PlantStates.SPROUT)
            {
                SetState(PlantStates.GROWING);
            }
            else if (_state == PlantStates.GROWING)
            {
                SetState(PlantStates.FLOWERING);
            }
            else if (_state == PlantStates.FLOWERING)
            {
                SetState(PlantStates.FRUITING);
            }
            else if (_state == PlantStates.FRUITING)
            {
               SetState(PlantStates.SPROUT);
            }
        }

        private void RefreshEffects()
        {
            if (Nourished)
            {
                _treeStages.ForEach(s => s.GetComponent<Animator>().SetInteger("State", 1));
                _nourishedEffect.Play();
            }
            else
            {
                _treeStages.ForEach(s => s.GetComponent<Animator>().SetInteger("State", 0));
                _nourishedEffect.Stop();
            }
        }

        public void SetNourished(bool nourished)
        {
            Nourished = nourished;

            _energy = GameManager.PlantStatistics.NourishmentPerFertilization;

            RefreshEffects();
        }

        public void SetState(PlantStates state)
        {
            _state = state;

            _treeStages.ForEach(t => t.gameObject.SetActive(false));
            //SetNourished(false);
            Action = "Fertilize";
            MustBeCarryingToInteract = new List<ItemTypes> { ItemTypes.GEL };
            switch (_state)
            {
                case PlantStates.SPROUT:
                    SetCanInteract(true);
                    _sustenanceNeeded = Random.Range(GameManager.PlantStatistics.MinSustenancePerStage, GameManager.PlantStatistics.MaxSustenancePerStage + 1);                 
                    _treeStages[0].gameObject.SetActive(true);
                    SetNourished(false);
                    break;
                case PlantStates.GROWING:
                    //SetCanInteract(true);
                    _treeStages[1].gameObject.SetActive(true);
                    SetNourished(true);
                    break;
                case PlantStates.FLOWERING:
                    //SetCanInteract(true);
                    _treeStages[2].gameObject.SetActive(true);
                    SetNourished(true);
                    break;
                case PlantStates.FRUITING:
                    SetCanInteract(true);
                    _treeStages[3].gameObject.SetActive(true);
                    Action = "Harvest";
                    MustBeCarryingToInteract = null;
                    SetNourished(false);
                    //spawn fruit
                    var fruitObject = Instantiate(GameManager.PlantStatistics.GetRandomFruit());
                    _fruit = fruitObject.GetComponent<Interactable>();
                    fruitObject.transform.SetParent(_fruitPosition.transform);
                    fruitObject.transform.localPosition = Vector3.zero;
                    _fruit.SetCanInteract(false);
                    break;
            }
        }

        public override bool Highlight()
        {
            if (_state == PlantStates.FRUITING)
                return PlayerController.CarriedItem == null;

            return base.Interact();
        }

        public override void SetCanInteract(bool canInteract)
        {
            //base.SetCanInteract(canInteract);
            CanInteract = canInteract;
        }

        public override bool Interact()
        {
            if (base.Interact())
            {
                //_targetInteractable.SetCanInteract(true);
                SetCanInteract(false);

                switch (_state)
                {
                    default:
                        _energy = GameManager.PlantStatistics.NourishmentPerFertilization;
                        SetNourished(true);
                        PlayerController.CarriedItem.Drop();
                        PlayerController.DownlightItem();
                        _fertilize.Play();
                        break;
                    case PlantStates.FRUITING:
                        _treeStages[3].gameObject.SetActive(true);
                        _fruitfy.Play();
                        //drop fruit
                        _fruit.transform.SetParent(null);
                        _fruit.SetCanInteract(true);
                        SetState(PlantStates.SPROUT);
                        break;
                }

                //_isOn = true;
                //_animator.SetInteger("State", 1);
                //_visualEffect.Play();
                //var excrement = PlayerController.CarriedItem;
                //switch (excrement.ItemType)
                //{
                //    case ItemTypes.EXCREMENT_1:
                //        _sustenanceNeeded = GameManager.ExcrementStatistics.SmallEnergy;
                //        break;
                //    case ItemTypes.EXCREMENT_2:
                //        _energy = GameManager.ExcrementStatistics.AverageEnergy;
                //        break;
                //    case ItemTypes.EXCREMENT_3:
                //        _energy = GameManager.ExcrementStatistics.LargeEnergy;
                //        break;
                //    default:
                //        _energy = GameManager.ExcrementStatistics.SmallEnergy;
                //        break;
                //}
                //excrement.SetCanInteract(false);
                //_excrement = excrement.gameObject;
                //_excrement.transform.SetParent(_excrementPlace);
                //_excrement.transform.localPosition = Vector3.zero;
                //Destroy(excrement);
                return true;
            }

            return false;
        }
    }
}
