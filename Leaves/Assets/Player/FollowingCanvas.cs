using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gomma
{
    public enum WhenToUpdate { UPDATE = 0, FIXED_UPDATE = 1 }

    public class FollowingCanvas : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private WhenToUpdate _whenToUpdate = WhenToUpdate.UPDATE;

        private void Update()
        {
            if (_whenToUpdate == WhenToUpdate.UPDATE)
                transform.position = _target.position + _offSet;
        }

        private void FixedUpdate()
        {
            if (_whenToUpdate == WhenToUpdate.FIXED_UPDATE)
                transform.position = _target.position + _offSet;
        }
    }
}