using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    [SerializeField] private float _lifeSpam = 5;

    private void Awake()
    {
        Invoke("SelfDesotry", _lifeSpam);
    }

    private void SelfDesotry()
    {
        Destroy(gameObject);
    }
}
