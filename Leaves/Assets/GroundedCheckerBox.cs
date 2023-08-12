using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public class GroundedCheckerBox : MonoBehaviour
    {
        public delegate void OnGroundedStateChanged(bool isGrounded);
        public OnGroundedStateChanged GroundedStateChanged;

        public LayerMask _layerMask;
        public Vector2 _boxSize = Vector2.one;
        public float _distance = 0.1f;
        private bool _ignoreFirstTime = true;

        [SerializeField] private bool _grounded = false;
        public bool Grounded => _grounded;

        private void Awake()
        {
            TestGrounded();
        }

        private void FixedUpdate()
        {
            TestGrounded();
        }

        private void TestGrounded()
        {
            bool isGrounded = Physics2D.BoxCast(transform.position, _boxSize, 0, Vector2.down, _distance, _layerMask);

            if (_grounded != isGrounded)
            {
                if (_ignoreFirstTime)
                {
                    _ignoreFirstTime = false;
                }
                else
                {
                    GroundedStateChanged?.Invoke(isGrounded);
                }
                _grounded = isGrounded;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position - transform.up * _distance, _boxSize);
        }
    }
}