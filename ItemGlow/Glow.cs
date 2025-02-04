using UnityEngine;

namespace ItemGlow;

public class Glow(Color color, float intensity = 0.7f, float radius = 0.5f, float updateFrequency = 0.2f, bool glowInHands = true)
{
    public readonly float Intensity = intensity;
    public readonly float Radius = radius;
    public readonly float UpdateFrequency = updateFrequency;
    public readonly bool GlowInHands = glowInHands;
    public readonly Color Color = color;
}