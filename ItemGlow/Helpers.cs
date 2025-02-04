using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using MEC;
using UnityEngine;

namespace ItemGlow;

public static class Helpers
{
    public static Vector3 GetHandPosition(this Player player, bool compensateVelocity = true, bool compensateLatency = true)
    {
        Vector3 handPosition = player.Position + player.CameraTransform.rotation * new Vector3(0.07f * player.Scale.x, 0.28f * player.Scale.y, 0.36f * player.Scale.z);

        handPosition += player.CameraTransform.rotation * new Vector3(0, 0.06f, 0);

        if (compensateVelocity)
            handPosition += player.Velocity * 0.15f;

        if (compensateLatency)
            handPosition += player.Velocity * (player.Ping / 1000f);

        return handPosition;
    }

    public static void Attach(this AdminToy child, Func<Vector3> getPosition, Func<Quaternion> getRotation, float updateFrequency = 0.2f, Func<bool> whileCondition = null)
    {
        Timing.RunCoroutine(AttachCoroutine(child, getPosition, getRotation, updateFrequency, whileCondition));
    }

    static IEnumerator<float> AttachCoroutine(AdminToy child, Func<Vector3> positionGetter, Func<Quaternion> getRotation, float updateFrequency, Func<bool> condition)
    {
        while (condition())
        {
            try
            {
                Vector3 position = positionGetter();

                if (child.Position != position)
                {
                    child.Position = position;
                }

                Quaternion rotation = getRotation();

                if (child.Rotation != rotation)
                {
                    child.Rotation = rotation;
                }
            }
            catch
            {
                child?.Destroy();
                yield break;
            }

            yield return Timing.WaitForSeconds(updateFrequency);
        }

        child?.Destroy();
    }
}