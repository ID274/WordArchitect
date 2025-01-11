using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDetector : MonoBehaviour
{
    private BrakeIfClose brakeScript;

    private void Awake()
    {
        brakeScript = GetComponentInParent<BrakeIfClose>();
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag(brakeScript.tagToCheck))
        {
            brakeScript.Brake(other.GetComponentInParent<MoveViaPath>());
        }
    }
}
