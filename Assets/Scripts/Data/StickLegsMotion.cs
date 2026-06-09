using UnityEngine;
using DG.Tweening;

public class StickLegsMotion : RobotPartMotion
{
    public Transform Leg1;
    public Transform Leg2;
    public Transform AnchorPoint;

    [SerializeField] private float moveDistance = 0.25f;
    [SerializeField] private float jumpSideRotation = 20f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;
    [SerializeField] private Ease jumpEase = Ease.OutQuad;
    [SerializeField] private Ease rotateEase = Ease.InOutSine;

    private Vector3 leg1StartPosition;
    private Vector3 leg2StartPosition;
    private Vector3 leg1StartRotation;
    private Vector3 leg2StartRotation;
    private Vector3 anchorStartRotation;
    private Sequence currentSequence;
    private bool hasStartTransforms;

    private void Awake()
    {
        CacheStartTransforms();
    }
    
    public override void MoveAnimation()
    {
        StopAnimations();

        if (Leg1 == null || Leg2 == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Leg1.DOLocalMoveZ(leg1StartPosition.z + moveDistance, anticipationDuration).SetEase(moveEase));
        currentSequence.Join(Leg2.DOLocalMoveZ(leg2StartPosition.z - moveDistance, anticipationDuration).SetEase(moveEase));
        currentSequence.Append(Leg1.DOLocalMoveZ(leg1StartPosition.z - moveDistance, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Leg2.DOLocalMoveZ(leg2StartPosition.z + moveDistance, followThroughDuration).SetEase(moveEase));
        currentSequence.Append(Leg1.DOLocalMoveZ(leg1StartPosition.z, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Leg2.DOLocalMoveZ(leg2StartPosition.z, followThroughDuration).SetEase(moveEase));
    }

    public override void JumpAnimation()
    {
        StopAnimations();

        if (Leg1 == null || Leg2 == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Leg1.DOLocalRotate(leg1StartRotation - Vector3.forward * jumpSideRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Leg2.DOLocalRotate(leg2StartRotation + Vector3.forward * jumpSideRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Append(Leg1.DOLocalRotate(leg1StartRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Leg2.DOLocalRotate(leg2StartRotation, followThroughDuration).SetEase(jumpEase));
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

        if (Leg1 != null)
        {
            leg1StartPosition = Leg1.localPosition;
            leg1StartRotation = Leg1.localEulerAngles;
        }

        if (Leg2 != null)
        {
            leg2StartPosition = Leg2.localPosition;
            leg2StartRotation = Leg2.localEulerAngles;
        }

        if (AnchorPoint != null)
        {
            anchorStartRotation = AnchorPoint.localEulerAngles;
        }

        hasStartTransforms = true;
    }

    private void ResetTransforms()
    {
        if (Leg1 != null)
        {
            Leg1.localPosition = leg1StartPosition;
            Leg1.localEulerAngles = leg1StartRotation;
        }

        if (Leg2 != null)
        {
            Leg2.localPosition = leg2StartPosition;
            Leg2.localEulerAngles = leg2StartRotation;
        }

        if (AnchorPoint != null)
        {
            AnchorPoint.localEulerAngles = anchorStartRotation;
        }
    }
}
