using Exiled.API.Features;
using System;

namespace ItemGlow;

public class Plugin : Plugin<Config>
{
    public override string Prefix => "ItemGlow";
    public override string Name => Prefix;
    public override string Author => "Banalny_Banan";
    public override Version Version { get; } = new(2, 0, 0);
    public static Plugin Instance;

    public override void OnEnabled()
    {
        Instance = this;
        GlowHandler.Enable();
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Instance = null;
        GlowHandler.Disable();
        base.OnDisabled();
    }
}