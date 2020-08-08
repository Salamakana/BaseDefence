using System;
using UnityEngine;

public class TargetingState : IState
{  
    private Transform currentTarget;
    private Transform rotatingPart;
    private float rotationSpeed;
    private Action targetInSightCallback;

    public TargetingState
        (
        Transform currentTarget,
        Transform rotatingPart,
        float rotationSpeed,
        Action targetInSightCallback
        )
    {
        this.currentTarget = currentTarget;
        this.rotatingPart = rotatingPart;
        this.rotationSpeed = rotationSpeed;
        this.targetInSightCallback = targetInSightCallback;
    }

    public void EnterState()
    {
       
    }

    public void ExecuteState()
    {   
        LockOnTarget(currentTarget);       
    }

    public void ExitState()
    {

    }

    private void LockOnTarget(Transform currentTarget)
    {
        var direction = currentTarget.position - rotatingPart.position;
        Quaternion desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
        rotatingPart.rotation = Quaternion.RotateTowards(rotatingPart.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        targetInSightCallback();
    }
}
