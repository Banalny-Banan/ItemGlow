using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using Handlers = Exiled.Events.Handlers;
using Light = Exiled.API.Features.Toys.Light;

namespace ItemGlow;

internal static class GlowHandler
{
    public delegate bool GlowGetter(ushort serial, out Glow glow);

    internal static readonly List<GlowGetter> GlowGetters = [];

    static bool isEnabled;

    public static void EnsureEnabled()
    {
        if (isEnabled) return;
        isEnabled = true;

        Handlers.Player.ChangedItem += OnChangedItem;
        Handlers.Map.PickupAdded += OnMapPickupAdded;
    }

    internal static bool TryGetGlow(ushort serial, out Glow glow)
    {
        foreach (GlowGetter glowGetter in GlowGetters)
        {
            if (glowGetter(serial, out glow))
            {
                return true;
            }
        }

        glow = null;
        return false;
    }

    internal static void OnChangedItem(ChangedItemEventArgs ev)
    {
        if (ev.Item is not Item item)
            return;

        Player player = ev.Player;

        if (TryGetGlow(item.Serial, out Glow glow) && glow.GlowInHands)
        {
            var light = Light.Create(player.GetHandPosition());
            light.Intensity = glow.Intensity * 2;
            light.Range = glow.Radius * 2.5f;
            light.ShadowType = LightShadows.Soft;
            light.Attach(() => player.GetHandPosition(), () => Quaternion.identity, 0.05f, () => player?.CurrentItem == item);
        }
    }

    internal static void OnMapPickupAdded(PickupAddedEventArgs ev)
    {
        if (ev.Pickup != null && TryGetGlow(ev.Pickup.Serial, out Glow glow))
        {
            var light = Light.Create(ev.Pickup.Position);
            light.Color = glow.Color;
            light.Intensity = glow.Intensity;
            light.Range = glow.Radius;
            light.ShadowType = LightShadows.None;
            light.Attach(() => ev.Pickup.Position + Vector3.up * 0.05f, () => Quaternion.identity, glow.UpdateFrequency);
        }
    }
}