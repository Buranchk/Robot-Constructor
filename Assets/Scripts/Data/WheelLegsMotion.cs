using UnityEngine;
using DG.Tweening;

public class WheelLegsMotion : RobotPartMotion
{
    public Transform AnchorPoint;
    public Transform Wheel;

    [SerializeField] private float wheelJumpSquashX = 0.4f;
    [SerializeField] private float wheelJumpStretchX = 0.6f;
    [SerializeField] private float anchorJumpStretchY = 1.1f;
    [SerializeField] private float anchorJumpSquashY = 0.9f;
    [SerializeField] private Ease moveEase = Ease.Linear;
    [SerializeField] private Ease jumpEase = Ease.InOutSine;
    [SerializeField] private Ease rotateEase = Ease.InOutSine;

    private Vector3 anchorStartRotation;
    private Vector3 anchorStartScale;
    private Vector3 wheelStartRotation;
    private Vector3 wheelStartScale;
    private Sequence currentSequence;
    private bool hasStartTransforms;

    private void Awake()
    {
        CacheStartTransforms();
    }

    public override void MoveAnimation()
    {
        StopAnimations();

        if (Wheel == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence().Join(Wheel
            .DOLocalRotate(new Vector3(0f, 360f, 0f), anticipationDuration + followThroughDuration, RotateMode.LocalAxisAdd)
            .SetEase(moveEase));
    }

    public override void JumpAnimation()
    {
        StopAnimations();

        if (AnchorPoint == null || Wheel == null)
        {
            return;
        }

        Vector3 wheelSquashScale = wheelStartScale;
        wheelSquashScale.x = wheelJumpSquashX;

        Vector3 wheelStretchScale = wheelStartScale;
        wheelStretchScale.x = wheelJumpStretchX;

        Vector3 anchorStretchScale = anchorStartScale;
        anchorStretchScale.y = anchorJumpStretchY;

        Vector3 anchorSquashScale = anchorStartScale;
        anchorSquashScale.y = anchorJumpSquashY;

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Wheel.DOScale(wheelSquashScale, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(AnchorPoint.DOScale(anchorStretchScale, anticipationDuration).SetEase(jumpEase));
        currentSequence.Append(Wheel.DOScale(wheelStretchScale, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(AnchorPoint.DOScale(anchorSquashScale, followThroughDuration).SetEase(jumpEase));
        currentSequence.Append(Wheel.DOScale(wheelStartScale, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(AnchorPoint.DOScale(anchorStartScale, followThroughDuration).SetEase(jumpEase));
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
            anchorStartScale = AnchorPoint.localScale;
        }

        if (Wheel != null)
        {
            wheelStartRotation = Wheel.localEulerAngles;
            wheelStartScale = Wheel.localScale;
        }

        hasStartTransforms = true;
    }

    private void ResetTransforms()
    {
        if (AnchorPoint != null)
        {
            AnchorPoint.localEulerAngles = anchorStartRotation;
            AnchorPoint.localScale = anchorStartScale;
        }

        if (Wheel != null)
        {
            Wheel.localEulerAngles = wheelStartRotation;
            Wheel.localScale = wheelStartScale;
        }
    }
}
