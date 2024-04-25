using System;
using System.Reflection;
using Exiled.API.Features;
using UnityEngine;
using static ItemGlow.GlowHandler;

namespace ItemGlow;

public class API
{
    public static void AddGlow(Func<ushort, bool> itemCheck, Color color, float intensity = 0.7f, float radius = 0.5f, float updateFrequency = 0.2f, bool glowInHands = true)
    {
        if (updateFrequency <= 0) throw new ArgumentOutOfRangeException(nameof(updateFrequency), "ItemGlow.API.AddGlow: updateFrequency must be greater than 0");
        
        string callingAssembly = Assembly.GetCallingAssembly().GetName().Name;
        
        GlowGetters.Add((ushort serial, out Glow glow) =>
        {
            try
            {
                if (itemCheck(serial))
                {
                    glow = new Glow(color, intensity, radius, updateFrequency, glowInHands);
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