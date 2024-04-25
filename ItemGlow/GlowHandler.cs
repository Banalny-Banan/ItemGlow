using System.Collections.Generic;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using Handlers = Exiled.Events.Handlers;

namespace ItemGlow;

internal static class GlowHandler
{
    public delegate bool GlowGetter(ushort serial, out Glow glow);
    public static bool IsEnabled { get; set; } = false;

    internal static void Enable()
    {
        IsEnabled = true;
        Handlers.Player.ChangedItem += OnChangedItem;
        Handlers.Map.PickupAdded += OnMapPickupAdded;
    }

    internal static void Disable()
    {
        IsEnabled = false;
        Handlers.Player.ChangedItem -= OnChangedItem;
        Handlers.Map.PickupAdded -= OnMapPickupAdded;
    }

    internal static readonly List<GlowGetter> GlowGetters = new();

    internal static bool TryGetGlow(ushort serial, out Glow glow)
    {
        foreach (var glowGetter in GlowGetters)
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
        var player = ev.Player;
        if (ev.Item is not Item item) return;

        if (TryGetGlow(item.Serial, out var glow) && glow.GlowInHands)
        {
            var lightSource = MapEditorReborn.API.Features.ObjectSpawner.SpawnLightSource(new(glow.Color.ToHex(), glow.Intensity * 2, glow.Radius * 2.5f, true), player.GetHandPosition());
            lightSource.AttachDynamically(() => player.GetHandPosition(), () => Quaternion.identity, 0.05f, () => player?.CurrentItem == item);
        }
    }

    internal static void OnMapPickupAdded(PickupAddedEventArgs ev)
    {
        if (TryGetGlow(ev.Pickup.Serial, out var glow))
        {
            var lightSource = MapEditorReborn.API.Features.ObjectSpawner.SpawnLightSource(new(glow.Color.ToHex(), glow.Intensity, glow.Radius, false), ev.Pickup.Position);
            lightSource.AttachDynamically(() => ev.Pickup.Position + Vector3.up * 0.05f, null, glow.UpdateFrequency);
        }
    }
}