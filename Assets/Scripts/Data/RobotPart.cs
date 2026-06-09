using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotPart : MonoBehaviour
{
    private static RobotPartMaterial[] robotPartMaterials;

    [SerializeField] private float weight;
    [SerializeField] private int power;
    [SerializeField] private PartType type;
    [SerializeField] private float height;
    [SerializeField] private List<Renderer> partRenderers;

    public float Weight => weight;
    public int Power => power;
    public PartType Type => type;
    public float Height => height;

    public void SetMaterial(RobotPartMaterial partMaterial)
    {
        if (partMaterial == null || partRenderers == null)
        {
            return;
        }

        foreach (Renderer materialRenderer in partRenderers)
        {
            if (materialRenderer == null)
            {
                continue;
            }

            materialRenderer.material = partMaterial.Material;
        }
    }

    private void Awake()
    {
        SetMaterial(GetMaterial("Default"));
    }

    private static IReadOnlyList<RobotPartMaterial> GetMaterials()
    {
        robotPartMaterials ??= Resources.LoadAll<RobotPartMaterial>("RobotMaterials");
        return robotPartMaterials;
    }

    private static RobotPartMaterial GetMaterial(string materialName)
    {
        return GetMaterials().FirstOrDefault(material => material.name == materialName);
    }
}
