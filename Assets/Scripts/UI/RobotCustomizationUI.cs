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

    public void ChangeLegsMaterial(int materialIndex)
    {
        ChangeMaterial(PartType.Legs, materialIndex);
    }

    public void ChangeBodyMaterial(int materialIndex)
    {
        ChangeMaterial(PartType.Torso, materialIndex);
    }

    public void ChangeHeadMaterial(int materialIndex)
    {
        ChangeMaterial(PartType.Head, materialIndex);
    }

    private void ChangeMaterial(PartType partType, int materialIndex)
    {
        robotController.ChangeMaterial(partType, materialIndex);
    }

    public void PlayAnimation()
    {
        robotController.PlayAnimation();
    }

    private PartType ToPartType(int partType)
    {
        return (PartType)partType;
    }

}
