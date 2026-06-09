using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [SerializeField] private List<GameObject> legsPrefabs;
    [SerializeField] private List<GameObject> torsoPrefabs;
    [SerializeField] private List<GameObject> headPrefabs;
    
    private RobotBuild robot;
    private RobotPartMaterial[] robotPartMaterials;
    private int legsIndex;
    private int torsoIndex;
    private int headIndex;

    private void Awake()
    {
        RebuildRobot();
        robotPartMaterials = Resources.LoadAll<RobotPartMaterial>("RobotMaterials");
    }

    public void PlayAnimation()
    {
        if (robot == null)
        {
            return;
        }

        foreach (RobotPartMotion motion in robot.Motions)
        {
            motion.MoveAnimation();
        }
    }

    public void SwitchPart(int type)
    {
        switch (type)
        {
            case 1:
                legsIndex = GetNextIndex(legsIndex, legsPrefabs);
                break;
            case 2:
                torsoIndex = GetNextIndex(torsoIndex, torsoPrefabs);
                break;
            case 3:
                headIndex = GetNextIndex(headIndex, headPrefabs);
                break;
        }

        RebuildRobot();
    }
    
    private void ChangeMaterial(PartType selectedType, RobotPartMaterial selectedMaterial)
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

    private void ChangeMaterial(PartType selectedType, int materialIndex)
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

        if (!HasPrefab(legsPrefabs, legsIndex) || !HasPrefab(torsoPrefabs, torsoIndex) || !HasPrefab(headPrefabs, headIndex))
        {
            return;
        }

        RobotBuilder builder = new RobotBuilder();
        robot = builder.Build(legsPrefabs[legsIndex], torsoPrefabs[torsoIndex], headPrefabs[headIndex], transform);
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

    private int GetNextIndex(int currentIndex, List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0)
        {
            return 0;
        }

        return (currentIndex + 1) % prefabs.Count;
    }

    private bool HasPrefab(List<GameObject> prefabs, int index)
    {
        return prefabs != null && index >= 0 && index < prefabs.Count && prefabs[index] != null;
    }
}
