using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class Fruit : Interactable
    {
        public override bool Highlight()
        {
            return PlayerController.CarriedItem == null;
        }

        public override bool Interact()
        {
            if (base.Interact())
            {
                PlayerController.DownlightItem();
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