using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class Fan : Interactable
    {
        public static bool Working;

        [SerializeField] private Animator _animator;

        [SerializeField] private ParticleSystem _purify;
        [SerializeField] private ParticleSystem _toxicCloud;

        private void Start()
        {
            SetCanInteract(false);
        }

        public override bool Highlight()
        {
            return false;
        }

        public override void SetCanInteract(bool canInteract)
        {
            //base.SetCanInteract(canInteract);
            CanInteract = canInteract;
            Working = CanInteract;
            OnStateChanged(canInteract);
            if (CanInteract)
            {
                _animator.SetInteger("State", 1);
                _purify.Play();
                _toxicCloud.Stop();
            }
            else
            {
                _animator.SetInteger("State", 0);
                _purify.Stop();
                _toxicCloud.Play();
            }
        }
    }
}