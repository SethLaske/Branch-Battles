using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float miningTimeMultiplier;
    public float radius;

    public void IncreaseMultiplier(float increase) {
        miningTimeMultiplier += increase;
    }
}
