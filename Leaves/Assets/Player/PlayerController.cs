using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gomma
{
    public enum PlayerStates { IDLE = 0, WALKING = 1, JUMPING = 2, FALLING = 3 }

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        [SerializeField] [Range(1, 300)] int _maxLife = 60;
        [SerializeField] [Range(0.1f, 100f)] float _walkSpeed = 2f;
        [SerializeField] [Range(0.1f, 100f)] float _jumpForce = 10f;
        [SerializeField] [Range(0f, 1f)] float _jumpSpeedReduction = 0.5f;

        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private GameObject _hand;

        [SerializeField] private Animator _lifeBarAnimator;
        [SerializeField] private Image _lifeBarFill;
        [SerializeField] private Animator _airBarAnimator;
        [SerializeField] private Image _airBarFill;

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private float _originalScaleX;
        private PlayerStates _state;
        private int _currentLife;
        private int _currentAir;
        private float _lifeCounter;
        private float _negativeGravity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _originalScaleX = transform.localScale.x;
            Instance = this;
            _currentLife = _maxLife;
            _currentAir = _maxLife;
            _lamps.Clear();
            _negativeGravity = 0;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E) && _canInteract)
            {
                Interact();
            }

            if (Input.GetKey(KeyCode.Space) && _groundChecker.Grounded)
            {
                _jumping = true;
            }
            else
            {
                _jumping = false;
            }

            if (HighlightedItem == null)
                DownlightItem();

            _lifeCounter += Time.deltaTime;
            if (_lifeCounter >= 1)
            {
                _lifeCounter -= 1;
                if (_lamps != null && _lamps.Count > 0)
                {
                    _currentLife = Mathf.Clamp(_currentLife + 4, 0, _maxLife);
                }
                else
                {
                    _currentLife = Mathf.Clamp(_currentLife - 1, 0, _maxLife);
                }

                _lifeBarFill.fillAmount = (float)_currentLife / (float)_maxLife;
                if(_lifeBarFill.fillAmount <= 0.35f)
                {
                    _lifeBarAnimator.SetInteger("State", 1);
                }
                else
                {
                    _lifeBarAnimator.SetInteger("State", 0);
                }

                if(_currentLife <= 0)
                {
                    EndMenu.Instance.Show("You passed out of hypothermia, but the rescue team saved you.", false);
                    return;
                }

                if (_currentAir <= 0)
                {
                    EndMenu.Instance.Show("You passed out of dyspnea, but the rescue team saved you.", false);
                    return;
                }

                //fan
                if (Fan.Working)
                {
                    _currentAir = Mathf.Clamp(_currentAir + 1, 0, _maxLife);
                }
                else
                {
                    _currentAir = Mathf.Clamp(_currentAir - 1, 0, _maxLife);
                }

                _airBarFill.fillAmount = (float)_currentAir / (float)_maxLife;
                if (_airBarFill.fillAmount <= 0.35f)
                {
                    _airBarAnimator.SetInteger("State", 1);
                }
                else
                {
                    _airBarAnimator.SetInteger("State", 0);
                }
            }
        }

        private bool _jumping;
        private void FixedUpdate()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), 0);

            //flip
            if (input.x < 0)
                transform.localScale = new Vector3(-_originalScaleX, transform.localScale.y, transform.localScale.z);
            else if (input.x > 0)
                transform.localScale = new Vector3(_originalScaleX, transform.localScale.y, transform.localScale.z);

            //if (Input.GetKey(KeyCode.Space) && _groundChecker.Grounded)
            //{
            //    _rigidbody.velocity = new Vector2(input.x * _walkSpeed, _jumpForce);
            //}
            //else
            //{
            //    _rigidbody.velocity = new Vector2(input.x * _walkSpeed, _rigidbody.velocity.y);
            //}

            if (_jumping)
            {
                _rigidbody.velocity = 
                    new Vector2(input.x * _walkSpeed * (_groundChecker.Grounded ? 1f : 1f - _jumpSpeedReduction), _jumpForce);
            }
            else
            {
                _rigidbody.velocity = 
                    new Vector2(input.x * _walkSpeed * (_groundChecker.Grounded ? 1f : 1f - _jumpSpeedReduction), _rigidbody.velocity.y);
            }

            if (_negativeGravity != 0)
            {
                _rigidbody.velocity = new Vector2 (_rigidbody.velocity.x,  _negativeGravity);
            }


                if (_groundChecker.Grounded)
            {
                if (input.x != 0)
                    SetState(PlayerStates.WALKING);
                else
                    SetState(PlayerStates.IDLE);
            }
            else
            {
                if (_rigidbody.velocity.y > 0)
                    SetState(PlayerStates.JUMPING);
                else
                    SetState(PlayerStates.FALLING);
            }
        }

        private void StartInteractionDelay()
        {
            _canInteract = false;

            if (_canInteractCoroutine != null)
                StopCoroutine(_canInteractCoroutine);

            _canInteractCoroutine = StartCoroutine(AllowInteractionDelay());
        }

        private bool _canInteract = true;
        Coroutine _canInteractCoroutine;
        IEnumerator AllowInteractionDelay()
        {
            yield return new WaitForSeconds(0.3f);
            _canInteract = true;
        }

        private void SetState(PlayerStates state)
        {
            _state = state;
            _animator.SetInteger("State", (int)_state);
        }

        #region interactables

        [SerializeField] private Interactor _interactor;
        public static Interactable HighlightedItem;
        public static Interactable CarriedItem;

        public static void TryHighlighItem(Interactable candidate)
        {
            if (HighlightedItem != null)
                return;

            if (candidate.Highlight())
            {
                HighlightedItem = candidate;
                Instance._interactor.SetAction(HighlightedItem.Action);
            }
        }

        public static void DownlightItem()
        {
            if (HighlightedItem != null)
            {
                HighlightedItem.Downlight();
                HighlightedItem = null;
            }
            Instance._interactor.ClearAction();
        }

        public static void SetHighlightedItem(Interactable highlighted)
        {
            HighlightedItem = highlighted;
            Instance._interactor.SetAction(HighlightedItem.Action);
        }

        public static void Interact()
        {
            if (HighlightedItem == null && CarriedItem == null)
                return;

            if (HighlightedItem != null && HighlightedItem.Interact())
            {
                DownlightItem();
                Instance.StartInteractionDelay();
                return;
            }

            if (CarriedItem != null)
            {
                Instance.StartInteractionDelay();
                CarriedItem.Drop();
                CarriedItem.transform.SetParent(null);
                CarriedItem.SetCanInteract(true);
                CarriedItem = null;
            }
        }

        public static void SetCarriedItem(Interactable carriedItem)
        {
            //var scale = carriedItem.gameObject.transform.localScale;
            CarriedItem = carriedItem;
            carriedItem.SetCanInteract(false);
            carriedItem.transform.SetParent(Instance._hand.transform);
            carriedItem.transform.localPosition = Vector3.zero;
            //carriedItem.transform.localScale = scale;
        }

        private static List<Interactable> _lamps = new List<Interactable>();
        public static void AddLamp(Interactable lamp)
        {
            if (_lamps.Contains(lamp))
                return;

            _lamps.Add(lamp);
        }

        public static void RemoveLamp(Interactable lamp)
        {
            if (_lamps.Contains(lamp))
                _lamps.Remove(lamp);
        }


        public static void SetNegativeGravity(float negativeGravity)
        {
            Instance._negativeGravity = negativeGravity;
        }
        #endregion interactables
    }
}