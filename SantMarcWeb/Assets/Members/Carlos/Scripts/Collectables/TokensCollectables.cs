using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokensCollectables : MonoBehaviour
{
    [SerializeField] private float angle;
    [SerializeField] private GameObject token;

    private void Awake()
    {
        angle = 0.5f;
    }

    private void Update()
    {
        TokenMovement();
    }

    private void TokenMovement()
    {
        //ROTATION
        var TokenVector = transform.up;
        
        gameObject.transform.RotateAround(token.transform.position, TokenVector, angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        var CollectableSpawner = GetComponentInParent<CollectablesSpawner>();
        
        if (other.CompareTag("Car"))
        {
            if (gameObject.CompareTag("CarToken"))
            {
                CollectableSpawner.CarSpawnCollectablesL.Remove(gameObject);
                CollectableSpawner.CollectablesCompleted();
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Plane"))
        {
            if (gameObject.CompareTag("PlaneToken"))
            {
                CollectableSpawner.PlaneSpawnCollectablesL.Remove(gameObject);
                CollectableSpawner.CollectablesCompleted();
                Destroy(gameObject);
            }
        }
    }
}
