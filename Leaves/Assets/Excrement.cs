using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class Excrement : Interactable
    {
        public override bool Highlight()
        {
            if (PlayerController.CarriedItem == null)
                return true;

            return false;
        }

        public override bool Interact()
        {
            if (base.Interact())
            {
                return Carry();
            }

            return false;
        }

        public override bool Carry()
        {
            if (base.Carry())
            {
                PlayerController.SetCarriedItem(this);
                return true;
            }

            return false;
        }
    }
}