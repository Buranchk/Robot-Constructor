using System.Collections.Generic;
using UnityEngine;

public class RobotBuilder
{
    private readonly Dictionary<PartType, List<GameObject>> prefabsByType = new();

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
    
    public GameObject GetNextPrefab(PartType type, string currentPartId, bool next)
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

    public RobotBuild Build(
        GameObject legsPrefab,
        GameObject torsoPrefab,
        GameObject headPrefab,
        Transform root)
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

    private RobotPart SpawnPart(GameObject prefab, Transform
        parent)
    {
        GameObject instance = Object.Instantiate(prefab, parent);
        return instance.GetComponent<RobotPart>();
    }

}
