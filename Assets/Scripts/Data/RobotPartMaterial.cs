using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Part Material")]
public class RobotPartMaterial : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Material material;

    public string DisplayName => displayName;
    public Material Material => material;
}

