using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class ToolTip : Interactable
    {
        [SerializeField] private bool _destroyAfterInteraction;

        public override bool Interact()
        {
            return false;
        }

        public override void Downlight()
        {
            base.Downlight();

            if (_destroyAfterInteraction)
                Destroy(gameObject);
        }
    }
}