using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;
using MEC;
using Handlers = Exiled.Events.Handlers;

namespace ItemGlow;

public class Plugin : Plugin<Config>
{
    public override string Prefix => "ItemGlow";
    public override string Name => Prefix;
    public override string Author => "Banalny_Banan";
    public override Version Version { get; } = new (1, 0, 0);
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