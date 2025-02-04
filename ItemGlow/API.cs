using System;
using System.Reflection;
using Exiled.API.Features;
using UnityEngine;

namespace ItemGlow;

public class API
{
    /// <summary>
    ///     Adds glow to your custom item. You only need to call this method once.
    /// </summary>
    /// <param name="itemCheck">A method that checks that a given serial is of your custom item. If you are using Exiled's CustomItems, you can use the Check method.</param>
    /// <param name="updateInterval">The delay between glow position updates in seconds. With lower values you get smoother glow movement at the cost of performance.</param>
    /// <param name="glowInHands">Whether the glow should be visible when the item is in the player's hands.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when updateInterval is less than 0.</exception>
    public static void AddGlow(Func<ushort, bool> itemCheck, Color color, float intensity = 0.7f, float radius = 0.5f, float updateInterval = 0.2f, bool glowInHands = true)
    {
        if (updateInterval < 0)
            throw new ArgumentOutOfRangeException(nameof(updateInterval), "ItemGlow.API.AddGlow: updateInterval cannot be less than 0.");

        string callingAssembly = Assembly.GetCallingAssembly().GetName().Name;

        GlowHandler.GlowGetters.Add((ushort serial, out Glow glow) =>
        {
            try
            {
                if (itemCheck(serial))
                {
                    glow = new Glow(color, intensity, radius, updateInterval, glowInHands);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error($"ItemGlow.API.AddGlow: Exception in itemCheck method provided by [{callingAssembly}]: {e}");
            }

            glow = null;
            return false;
        });
    }
}