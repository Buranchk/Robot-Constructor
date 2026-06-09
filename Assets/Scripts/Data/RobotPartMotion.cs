using UnityEngine;

[RequireComponent(typeof(RobotPart))]
public abstract class RobotPartMotion : MonoBehaviour
{
    public float anticipationDuration = 0.5f;
    public float followThroughDuration = 0.3f;

    public abstract void MoveAnimation();

    public abstract void JumpAnimation();

    public abstract void RotateAnimation();

    public abstract void StopAnimations();
    
}
