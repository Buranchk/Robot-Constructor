using UnityEngine;

public class RobotCustomizationUI : MonoBehaviour
{
    [SerializeField] private RobotController robotController;

    public RobotBuild Robot => robotController.Robot;

    public void NextPart(int partType)
    {
        robotController.SwitchPart(ToPartType(partType), true);
    }

    public void PreviousPart(int partType)
    {
        robotController.SwitchPart(ToPartType(partType), false);
    }

    public void ChangeMaterial(int partType, int materialIndex)
    {
        robotController.ChangeMaterial(ToPartType(partType), materialIndex);
    }

    public void PlayAnimation()
    {
        robotController.PlayAnimation();
    }

    private PartType ToPartType(int partType)
    {
        return (PartType)(partType - 1);
    }
}
