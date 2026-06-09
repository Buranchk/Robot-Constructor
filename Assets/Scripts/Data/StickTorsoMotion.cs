using UnityEngine;
using DG.Tweening;

public class StickTorsoMotion : RobotPartMotion
{
    public Transform Hand1;
    public Transform Hand2;
    public Transform AnchorPoint;

    [SerializeField] private float handSwingRotation = 25f;
    [SerializeField] private float anchorMoveRotation = 6f;
    [SerializeField] private float jumpLeanRotation = 10f;
    [SerializeField] private float jumpHandsDownDistance = 0.18f;
    [SerializeField] private float jumpHandsUpDistance = 0.14f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;
    [SerializeField] private Ease jumpEase = Ease.OutQuad;
    [SerializeField] private Ease rotateEase = Ease.InOutSine;

    private Vector3 hand1StartPosition;
    private Vector3 hand2StartPosition;
    private Vector3 hand1StartRotation;
    private Vector3 hand2StartRotation;
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

        if (Hand1 == null || Hand2 == null || AnchorPoint == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(Hand1.DOLocalRotate(hand1StartRotation + Vector3.forward * handSwingRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Join(Hand2.DOLocalRotate(hand2StartRotation - Vector3.forward * handSwingRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation - Vector3.forward * anchorMoveRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Append(Hand1.DOLocalRotate(hand1StartRotation - Vector3.forward * handSwingRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Hand2.DOLocalRotate(hand2StartRotation + Vector3.forward * handSwingRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation + Vector3.forward * anchorMoveRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Append(Hand1.DOLocalRotate(hand1StartRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(Hand2.DOLocalRotate(hand2StartRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation, followThroughDuration).SetEase(moveEase));
    }

    public override void JumpAnimation()
    {
        StopAnimations();

        if (Hand1 == null || Hand2 == null || AnchorPoint == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation + Vector3.right * jumpLeanRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Hand1.DOLocalMoveY(hand1StartPosition.y - jumpHandsDownDistance, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Hand2.DOLocalMoveY(hand2StartPosition.y - jumpHandsDownDistance, anticipationDuration).SetEase(jumpEase));
        currentSequence.Append(AnchorPoint.DOLocalRotate(anchorStartRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Hand1.DOLocalMoveY(hand1StartPosition.y + jumpHandsUpDistance, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Hand2.DOLocalMoveY(hand2StartPosition.y + jumpHandsUpDistance, followThroughDuration).SetEase(jumpEase));
        currentSequence.Append(Hand1.DOLocalMoveY(hand1StartPosition.y, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Hand2.DOLocalMoveY(hand2StartPosition.y, followThroughDuration).SetEase(jumpEase));
    }

    public override void RotateAnimation()
    {
        StopAnimations();

        if (AnchorPoint == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence().Join(AnchorPoint
            .DOLocalRotate(new Vector3(0f, 180f, 0f), anticipationDuration + followThroughDuration, RotateMode.LocalAxisAdd)
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

        if (Hand1 != null)
        {
            hand1StartPosition = Hand1.localPosition;
            hand1StartRotation = Hand1.localEulerAngles;
        }

        if (Hand2 != null)
        {
            hand2StartPosition = Hand2.localPosition;
            hand2StartRotation = Hand2.localEulerAngles;
        }

        if (AnchorPoint != null)
        {
            anchorStartRotation = AnchorPoint.localEulerAngles;
        }

        hasStartTransforms = true;
    }

    private void ResetTransforms()
    {
        if (Hand1 != null)
        {
            Hand1.localPosition = hand1StartPosition;
            Hand1.localEulerAngles = hand1StartRotation;
        }

        if (Hand2 != null)
        {
            Hand2.localPosition = hand2StartPosition;
            Hand2.localEulerAngles = hand2StartRotation;
        }

        if (AnchorPoint != null)
        {
            AnchorPoint.localEulerAngles = anchorStartRotation;
        }
    }
}
