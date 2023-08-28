using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float miningTimeMultiplier;
    public float radius;

    private Vector3 particleCenter;

    [SerializeField] private ParticleSystem miningEffect;

    private void Start()
    {
        particleCenter = miningEffect.transform.position;    
    }

    public void IncreaseMultiplier(float increase) {
        miningTimeMultiplier += increase;

        
    }

    public void MineHit(Vector3 minerPosition) {
        ParticleSystem.ShapeModule shapeModule = miningEffect.shape;

        // Set the new shape position
        //shapeModule.position = newPosition;

        Vector3 direction = minerPosition - particleCenter;
        float distance = Random.Range(.1f, 1);
        shapeModule.position = direction.normalized * distance;

        
        miningEffect.Play();
    }
}
