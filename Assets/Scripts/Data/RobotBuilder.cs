using UnityEngine;
public class RobotBuilder
{
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