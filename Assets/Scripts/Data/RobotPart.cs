using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotPart : MonoBehaviour
{
    private static RobotPartMaterial[] robotPartMaterials;

    [SerializeField] private float weight;
    [SerializeField] private int power;
    [SerializeField] private PartType type;
    [SerializeField] private string partId;
    [SerializeField] private float height;
    [SerializeField] private List<Renderer> partRenderers;

    public float Weight => weight;
    public int Power => power;
    public PartType Type => type;
    public string PartId => partId;
    public float Height => height;
    public RobotPartMaterial CurrentMaterial { get; private set; }

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

        CurrentMaterial = partMaterial;
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
