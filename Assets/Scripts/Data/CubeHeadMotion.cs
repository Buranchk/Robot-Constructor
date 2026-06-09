using UnityEngine;
using DG.Tweening;

public class CubeHeadMotion : RobotPartMotion
{
    public Transform Eye;
    public Transform AnchorPoint;

    [SerializeField] private float moveLookRotation = 12f;
    [SerializeField] private float jumpLeanRotation = 12f;
    [SerializeField] private float closedEyeScaleY = 0.05f;
    [SerializeField] private Ease moveEase = Ease.InOutSine;
    [SerializeField] private Ease jumpEase = Ease.OutQuad;
    [SerializeField] private Ease rotateEase = Ease.InOutSine;

    private Vector3 eyeStartScale;
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

        if (AnchorPoint == null)
        {
            return;
        }

        currentSequence = DOTween.Sequence();
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation - Vector3.up * moveLookRotation, anticipationDuration).SetEase(moveEase));
        currentSequence.Append(AnchorPoint.DOLocalRotate(anchorStartRotation + Vector3.up * moveLookRotation, followThroughDuration).SetEase(moveEase));
        currentSequence.Append(AnchorPoint.DOLocalRotate(anchorStartRotation, followThroughDuration).SetEase(moveEase));
    }

    public override void JumpAnimation()
    {
        StopAnimations();

        if (AnchorPoint == null || Eye == null)
        {
            return;
        }

        Vector3 closedEyeScale = eyeStartScale;
        closedEyeScale.y = closedEyeScaleY;

        currentSequence = DOTween.Sequence();
        currentSequence.Join(AnchorPoint.DOLocalRotate(anchorStartRotation - Vector3.right * jumpLeanRotation, anticipationDuration).SetEase(jumpEase));
        currentSequence.Join(Eye.DOScale(closedEyeScale, anticipationDuration).SetEase(jumpEase));
        currentSequence.Append(AnchorPoint.DOLocalRotate(anchorStartRotation, followThroughDuration).SetEase(jumpEase));
        currentSequence.Join(Eye.DOScale(eyeStartScale, followThroughDuration).SetEase(jumpEase));
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

        if (Eye != null)
        {
            eyeStartScale = Eye.localScale;
        }

        if (AnchorPoint != null)
        {
            anchorStartRotation = AnchorPoint.localEulerAngles;
        }

        hasStartTransforms = true;
    }

    private void ResetTransforms()
    {
        if (Eye != null)
        {
            Eye.localScale = eyeStartScale;
        }

        if (AnchorPoint != null)
        {
            AnchorPoint.localEulerAngles = anchorStartRotation;
        }
    }
}
