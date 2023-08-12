using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAnimationEvents : MonoBehaviour
{
    public General general;

    public void EndGeneralTaunt() {
        Debug.Log("End General Taunt");
        general.EndTaunt();
    }

    public void EndGeneralAttack()
    {
        Debug.Log("End General Attack");
        general.EndTaunt();
    }
}
