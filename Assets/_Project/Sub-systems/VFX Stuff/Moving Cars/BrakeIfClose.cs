using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveViaPath))]
public class BrakeIfClose : MonoBehaviour
{
    [SerializeField] private MoveViaPath moveScript;

    public string tagToCheck = "MeshHolder";

    private void Awake()
    {
        moveScript = GetComponent<MoveViaPath>();
    }

    public void Brake(MoveViaPath otherMoveScript)
    {
        Debug.Log($"{gameObject.name} is braking because it is close to {otherMoveScript.gameObject.name}");
        moveScript.speed = otherMoveScript.speed;
    }
}
