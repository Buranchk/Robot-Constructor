using UnityEngine;
using DG.Tweening;

public class TallTorsoMotion : RobotPartMotion
{
    public Transform AnchorPoint;
    public Transform Gyroscope;
    public Transform Arm1;
    public Transform Arm2;

    [SerializeField] private float gyroscopeMoveRotation = 15f;
    [SerializeField] private float armMoveRotation = 20f;
    [SerializeField] private float gyroscopeJumpDistance = 0.1f;
    [SerializeField] private float armJumpRotation = 35f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;
    [SerializeField] private Ease jumpEase = Ease.OutQuad;
    [SerializeField] private Ease rotateEase = Ease.InOutSine;

    private Vector3 anchorStartRotation;
    private Vector3 gyroscopeStartPosition;
    private Vector3 gyroscopeStartRotation;
    private Vector3 arm1StartRotation;
    private Vector3 arm2StartRotation;
    private Sequence currentSequence;
    private bool hasStartTransforms;

    private void Awake()
    {
        CacheStartTransforms();
    }

    public override void MoveAnimation()
    {
        StopAnimations();

        if (Gyroscope == null || Arm1 == null || Arm2 == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Gyroscope.DOLocalRotate(gyroscopeStartRotation + Vector3.up * gyroscopeMoveRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation + Vector3.up * armMoveRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation - Vector3.up * armMoveRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Append(Gyroscope.DOLocalRotate(gyroscopeStartRotation - Vector3.up * gyroscopeMoveRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation - Vector3.up * armMoveRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation + Vector3.up * armMoveRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Append(Gyroscope.DOLocalRotate(gyroscopeStartRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation, followThroughDuration).SetEase(moveEase));
    }

    public override void JumpAnimation()
    {
        StopAnimations();

        if (Gyroscope == null || Arm1 == null || Arm2 == null)
        {
            return;
        }

        float jumpDistance = Mathf.Min(gyroscopeJumpDistance, 0.1f);

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Gyroscope.DOLocalMoveY(gyroscopeStartPosition.y - jumpDistance, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation - Vector3.right * armJumpRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation - Vector3.right * armJumpRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Append(Gyroscope.DOLocalMoveY(gyroscopeStartPosition.y + jumpDistance, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation + Vector3.right * armJumpRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation + Vector3.right * armJumpRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Append(Gyroscope.DOLocalMoveY(gyroscopeStartPosition.y, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Arm1.DOLocalRotate(arm1StartRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Arm2.DOLocalRotate(arm2StartRotation, followThroughDuration).SetEase(jumpEase));
    }

    public override void RotateAnimation()
    {
        StopAnimations();

        if (AnchorPoint == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence().Join(AnchorPoint
            .DOLocalRotate(new Vector3(0f, 360f, 0f), anticipationDuration + followThroughDuration, RotateMode.LocalAxisAdd)
            .SetEase(rotateEase));
    }

    public override void StopAnimations()
    {
        currentSequence?.Kill();
        ResetTransforms();
    }

    private void CacheStartTransforms()
    {
        if (hasStartTransforms)
        {
            return;
        }

        if (AnchorPoint != null)
        {
            anchorStartRotation = AnchorPoint.localEulerAngles;
        }

        if (Gyroscope != null)
        {
            gyroscopeStartPosition = Gyroscope.localPosition;
            gyroscopeStartRotation = Gyroscope.localEulerAngles;
        }

        if (Arm1 != null)
        {
            arm1StartRotation = Arm1.localEulerAngles;
        }

        if (Arm2 != null)
        {
            arm2StartRotation = Arm2.localEulerAngles;
        }

        hasStartTransforms = true;
    }

    private void ResetTransforms()
    {
        if (AnchorPoint != null)
        {
            AnchorPoint.localEulerAngles = anchorStartRotation;
        }

        if (Gyroscope != null)
        {
            Gyroscope.localPosition = gyroscopeStartPosition;
            Gyroscope.localEulerAngles = gyroscopeStartRotation;
        }

        if (Arm1 != null)
        {
            Arm1.localEulerAngles = arm1StartRotation;
        }

        if (Arm2 != null)
        {
            Arm2.localEulerAngles = arm2StartRotation;
        }
    }
}
