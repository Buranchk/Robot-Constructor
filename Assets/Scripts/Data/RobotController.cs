using UnityEngine;

public class RobotController : MonoBehaviour
{
    private RobotBuilder builder;
    private RobotBuild robot;
    private RobotPartMaterial[] robotPartMaterials;
    
    public RobotBuild Robot => robot;

    private void Awake()
    {
        builder = new RobotBuilder();
        builder.LoadAndSortPrefabs();

        RebuildRobot();
        robotPartMaterials = Resources.LoadAll<RobotPartMaterial>("RobotMaterials");
    }

    public void PlayAnimation()
    {
        foreach (RobotPartMotion motion in robot.Motions)
        {
            motion.MoveAnimation();
        }
    }

    public void SwitchPart(PartType type, bool next)
    {
        robot = builder.SwitchPart(type, robot, next, transform);
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

    private void RebuildRobot()
    {
        DestroyCurrentRobot();


        robot = builder.Build(robot.Legs.gameObject, robot.Torso.gameObject, robot.Head.gameObject, transform);
    }

    private void DestroyCurrentRobot()
    {
        if (robot == null)
        {
            return;
        }

        foreach (RobotPart part in robot.Parts)
        {
            if (part != null && part.transform.parent == transform)
            {
                Destroy(part.gameObject);
            }
        }

        robot = null;
    }

    private RobotPart GetPart(PartType type)
    {
        if (robot == null)
        {
            return null;
        }

        foreach (RobotPart part in robot.Parts)
        {
            if (part.Type == type)
            {
                return part;
            }
        }

        return null;
    }
}
