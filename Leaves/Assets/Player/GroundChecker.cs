using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class GroundChecker : MonoBehaviour
    {
        public bool Grounded { get; private set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Grounded = true;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            Grounded = true;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            Grounded = false;
        }
    }
}