using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Robot/Robot Config")]
public sealed class RobotConfig : ScriptableObject
{
    [SerializeField] private RobotPart[] parts;
    public IReadOnlyList<RobotPart> Parts => parts;
    
    public RobotPart[] GetParts(PartType type)
    {
        return parts == null ? Array.Empty<RobotPart>() : parts.Where(part => part != null && part.Type == type).ToArray();
    }
    public RobotPart GetDefaultPart(PartType type)
    {
        var filteredParts = GetParts(type);
        if (filteredParts.Length == 0)
            throw new InvalidOperationException($"Robot config does not contain parts of type '{type}'.");
        return filteredParts[0];
    }
    
}