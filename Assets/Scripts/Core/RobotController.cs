using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class RobotController : MonoBehaviour
{
    [SerializeField] private RobotConfig robotConfig;
    
    private RobotBuilder builder;
    private RobotBuild robot;
    private RobotPartMaterial[] robotPartMaterials;
    private Sequence jumpSequence;
    
    public RobotBuild Robot => robot;
    public event Action<RobotBuild> RobotChanged;

    private void Awake()
    {
        builder = new RobotBuilder(transform, robotConfig);
        robot = builder.BuildDefault();
        robotPartMaterials = Resources.LoadAll<RobotPartMaterial>("RobotMaterials");
        RobotChanged?.Invoke(robot);
    }

    public void PlayAnimation()
    {
        StopAnimations();
        int animationIndex = Random.Range(0, 3);

        foreach (RobotPartMotion motion in robot.Motions)
        {
            switch (animationIndex)
            {
                case 0:
                    motion.MoveAnimation();
                    break;
                case 1:
                    motion.JumpAnimation();
                    LocalJumpAnimation();
                    break;
                case 2:
                    motion.RotateAnimation();
                    break;
            }
        }
    }

    public void SwitchPart(PartType type, bool next)
    {
        StopAnimations();
        robot = builder.SwitchPart(type, robot, next);
        RobotChanged?.Invoke(robot);
    }


    public void ChangeMaterial(PartType selectedType, int materialIndex)
    {
        if (robotPartMaterials == null || materialIndex < 0 || materialIndex >= robotPartMaterials.Length)
        {
            return;
        }

        RobotPartMaterial selectedMaterial = robotPartMaterials[materialIndex];

        foreach (RobotPart part in robot.Parts)
        {
            if (part.Type == selectedType)
            {
                part.SetMaterial(selectedMaterial);
            }
        }

    }

    private void StopAnimations()
    {
        jumpSequence?.Kill();
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        foreach (RobotPartMotion motion in robot.Motions)
        {
            motion.StopAnimations();
        }
    }

    private void LocalJumpAnimation()
    {
        RobotPartMotion motion = robot.Motions[0];
        jumpSequence = DOTween.Sequence();
        jumpSequence.Append(transform.DOScaleY(0.85f, motion.anticipationDuration));
        jumpSequence.Append(transform.DOLocalMoveY(0.5f, motion.followThroughDuration));
        jumpSequence.Join(transform.DOScaleY(1f, motion.followThroughDuration));
        jumpSequence.Append(transform.DOLocalMoveY(0f, motion.followThroughDuration));
    }
}
