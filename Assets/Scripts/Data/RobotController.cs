using UnityEngine;

public class RobotController : MonoBehaviour
{
    private RobotBuilder builder;
    private RobotBuild robot;
    private RobotPartMaterial[] robotPartMaterials;
    
    public RobotBuild Robot => robot;

    private void Awake()
    {
        builder = new RobotBuilder(transform);
        builder.LoadAndSortPrefabs();
        robot = builder.BuildDefault();
        robotPartMaterials = Resources.LoadAll<RobotPartMaterial>("RobotMaterials");
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
    }

    
    public void ChangeMaterial(PartType selectedType, RobotPartMaterial selectedMaterial)
    {
        if (robot == null || selectedMaterial == null)
        {
            return;
        }

        foreach (RobotPart part in robot.Parts)
        {
            if (part.Type == selectedType)
            {
                part.SetMaterial(selectedMaterial);
            }
        }

    }

    public void ChangeMaterial(PartType selectedType, int materialIndex)
    {
        if (robotPartMaterials == null || materialIndex < 0 || materialIndex >= robotPartMaterials.Length)
        {
            return;
        }

        ChangeMaterial(selectedType, robotPartMaterials[materialIndex]);
    }

    private void StopAnimations()
    {
        foreach (RobotPartMotion motion in robot.Motions)
        {
            motion.StopAnimations();
        }
    }
}
