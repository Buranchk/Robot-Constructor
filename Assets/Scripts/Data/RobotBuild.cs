using System.Collections.Generic;

public class RobotBuild
{
    public RobotPart Legs { get; }
    public RobotPart Torso { get; }
    public RobotPart Head { get; }

    public List<RobotPart> Parts { get; }
    public List<RobotPartMotion> Motions { get; } = new();

    public int TotalPower { get; }
    public float TotalWeight { get; }

    public RobotBuild(RobotPart legs, RobotPart torso, RobotPart head)
    {
        Legs = legs;
        Torso = torso;
        Head = head;
        Parts = new List<RobotPart> { Legs, Torso, Head };
        Parts.RemoveAll(part => part == null);

        foreach (RobotPart part in Parts)
        {
            TotalPower += part.Power;
            TotalWeight += part.Weight;

            RobotPartMotion motion = part.GetComponent<RobotPartMotion>();
            if (motion != null)
            {
                Motions.Add(motion);
            }
        }
    }

    public RobotPart GetPart(PartType type)
    {
        foreach (RobotPart part in Parts)
        {
            if (part.Type == type)
            {
                return part;
            }
        }

        return null;
    }
}
