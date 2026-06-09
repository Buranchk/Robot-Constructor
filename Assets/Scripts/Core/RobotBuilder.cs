using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotBuilder
{
    private readonly Transform root;
    private readonly RobotConfig robotConfig;

    private readonly Dictionary<PartType, RobotPart[]> prefabsByType;

    public RobotBuilder(Transform root, RobotConfig robotConfig)
    {
        this.root = root;
        this.robotConfig = robotConfig;

        prefabsByType = new Dictionary<PartType, RobotPart[]>();

        foreach (PartType type in Enum.GetValues(typeof(PartType)))
        {
            var parts = robotConfig.GetParts(type);
            if (parts.Length > 0)
                prefabsByType.Add(type, parts);
        }
    }

    private RobotPart GetNextPrefab(PartType type, string currentPartId, bool next)
    {
        var prefabs = prefabsByType[type];
        int currentIndex = Array.FindIndex(prefabs, prefab => prefab.PartId == currentPartId);

        int direction = next ? 1 : -1;
        int nextIndex = currentIndex + direction;

        if (nextIndex >= prefabs.Length)
        {
            nextIndex = 0;
        }
        else if (nextIndex < 0)
        {
            nextIndex = prefabs.Length - 1;
        }

        return prefabs[nextIndex];
    }

    public RobotBuild BuildDefault()
    {
        return Build(
            robotConfig.GetDefaultPart(PartType.Legs),
            robotConfig.GetDefaultPart(PartType.Torso),
            robotConfig.GetDefaultPart(PartType.Head));
    }

    private RobotBuild Build(
        RobotPart legsPrefab,
        RobotPart torsoPrefab,
        RobotPart headPrefab)
    {
        RobotPart legs = SpawnPart(legsPrefab, root);
        RobotPart torso = SpawnPart(torsoPrefab, root);

        Vector3 torsoPosition = torso.transform.localPosition;
        torsoPosition.y += legs.Height;
        torso.transform.localPosition = torsoPosition;

        RobotPart head = SpawnPart(headPrefab, torso.transform);

        Vector3 headPosition = head.transform.localPosition;
        headPosition.y += torso.Height;
        head.transform.localPosition = headPosition;

        return new RobotBuild(legs, torso, head);
    }

    public RobotBuild SwitchPart(PartType type, RobotBuild currentRobot, bool next)
    {
        RobotPart currentPart = currentRobot.GetPart(type);
        RobotPart legsPrefab = GetPrefab(currentRobot.Legs);
        RobotPart torsoPrefab = GetPrefab(currentRobot.Torso);
        RobotPart headPrefab = GetPrefab(currentRobot.Head);
        RobotPart nextPrefab = GetNextPrefab(type, currentPart.PartId, next);

        switch (type)
        {
            case PartType.Legs:
                legsPrefab = nextPrefab;
                break;
            case PartType.Torso:
                torsoPrefab = nextPrefab;
                break;
            case PartType.Head:
                headPrefab = nextPrefab;
                break;
        }

        RobotBuild newRobot = Build(legsPrefab, torsoPrefab, headPrefab);
        Recolor(currentRobot, newRobot);
        DestroyRobot(currentRobot);
        return newRobot;
    }

    private void Recolor(RobotBuild from, RobotBuild to)
    {
        to.Legs.SetMaterial(from.Legs.CurrentMaterial);
        to.Torso.SetMaterial(from.Torso.CurrentMaterial);
        to.Head.SetMaterial(from.Head.CurrentMaterial);
    }

    private RobotPart GetPrefab(RobotPart part)
    {
        return Array.Find(prefabsByType[part.Type], prefab => prefab.GetComponent<RobotPart>().PartId == part.PartId);
    }

    private void DestroyRobot(RobotBuild robot)
    {
        foreach (RobotPart part in robot.Parts)
        {
            GameObject.Destroy(part.gameObject);
        }
    }

    private RobotPart SpawnPart(RobotPart prefab, Transform parent)
    {
        return GameObject.Instantiate(prefab, parent);
    }
}
