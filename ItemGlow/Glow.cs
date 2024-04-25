using UnityEngine;

namespace ItemGlow;

public class Glow
{
    public Glow(Color color, float intensity = 0.7f, float radius = 0.5f, float updateFrequency = 0.2f, bool glowInHands = true)
    {
        Color = color;
        Intensity = intensity;
        Radius = radius;
        UpdateFrequency = updateFrequency;
        GlowInHands = glowInHands;
    }

    public Color Color;
    public float Intensity;
    public float Radius;
    public float UpdateFrequency;
    public bool GlowInHands;
}