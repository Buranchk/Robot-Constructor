using System.Collections.Generic;
using UnityEngine;

public class RobotBuilder
{
    private readonly Transform root;
    private readonly Dictionary<PartType, List<GameObject>> prefabsByType = new();

    public RobotBuilder(Transform root)
    {
        this.root = root;
    }

    public void LoadAndSortPrefabs()
    {
        prefabsByType.Clear();
        
        GameObject[] prefabs = Resources.LoadAll<GameObject>("RobotParts");

        foreach (GameObject prefab in prefabs)
        {
            RobotPart part = prefab.GetComponent<RobotPart>();

            if (!prefabsByType.ContainsKey(part.Type))
            {
                prefabsByType[part.Type] = new List<GameObject>();
            }

            prefabsByType[part.Type].Add(prefab);
        }
    }
    
    private GameObject GetNextPrefab(PartType type, string currentPartId, bool next)
    {
        List<GameObject> prefabs = prefabsByType[type];

        int currentIndex = prefabs.FindIndex(prefab => prefab.GetComponent<RobotPart>().PartId == currentPartId);

        int direction = next ? 1 : -1;
        int nextIndex = currentIndex + direction;

        if (nextIndex >= prefabs.Count)
        {
            nextIndex = 0;
        }
        else if (nextIndex < 0)
        {
            nextIndex = prefabs.Count - 1;
        }

        return prefabs[nextIndex];
    }

    public RobotBuild BuildDefault()
    {
        return Build(
            prefabsByType[PartType.Legs][0],
            prefabsByType[PartType.Body][0],
            prefabsByType[PartType.Head][0]);
    }

    public RobotBuild SwitchPart(PartType type, RobotBuild currentRobot, bool next)
    {
        RobotPart currentPart = currentRobot.GetPart(type);
        GameObject legsPrefab = GetPrefab(currentRobot.Legs);
        GameObject torsoPrefab = GetPrefab(currentRobot.Torso);
        GameObject headPrefab = GetPrefab(currentRobot.Head);
        GameObject nextPrefab = GetNextPrefab(type, currentPart.PartId, next);

        switch (type)
        {
            case PartType.Legs:
                legsPrefab = nextPrefab;
                break;
            case PartType.Body:
                torsoPrefab = nextPrefab;
                break;
            case PartType.Head:
                headPrefab = nextPrefab;
                break;
        }

        DestroyRobot(currentRobot);
        return Build(legsPrefab, torsoPrefab, headPrefab);
    }

    public RobotBuild Build(
        GameObject legsPrefab,
        GameObject torsoPrefab,
        GameObject headPrefab)
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

    private GameObject GetPrefab(RobotPart part)
    {
        return prefabsByType[part.Type].Find(prefab => prefab.GetComponent<RobotPart>().PartId == part.PartId);
    }

    private void DestroyRobot(RobotBuild robot)
    {
        foreach (RobotPart part in robot.Parts)
        {
            Object.Destroy(part.gameObject);
        }
    }

    private RobotPart SpawnPart(GameObject prefab, Transform
        parent)
    {
        GameObject instance = Object.Instantiate(prefab, parent);
        return instance.GetComponent<RobotPart>();
    }

}
