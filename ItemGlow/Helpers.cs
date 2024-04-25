using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using MEC;
using UnityEngine;

namespace ItemGlow;

public static class Helpers
{
    public static Vector3 GetHandPosition(this Player player, bool compensateVelocity = true, bool compensateLatency = true)
    {
        var handPosition = player.Position + player.CameraTransform.rotation * new Vector3(0.07f * player.Scale.x, 0.28f * player.Scale.y, 0.36f * player.Scale.z);

        handPosition += player.CameraTransform.rotation * new Vector3(0, 0.06f, 0);

        if (compensateVelocity)
            handPosition += player.Velocity * 0.15f;

        if (compensateLatency)
            handPosition += player.Velocity * (player.Ping / 1000f);

        return handPosition;
    }

    public static void AttachDynamically(this MapEditorObject child, Func<Vector3> getPosition, Func<Quaternion> getRotation, float updateFrequency = 0.2f, Func<bool> whileCondition = null)
    {
        whileCondition ??= () => true;
        Timing.RunCoroutine(AttachObject(child, getPosition, getRotation, updateFrequency, whileCondition));
    }

    private static IEnumerator<float> AttachObject(MapEditorObject child, Func<Vector3> positionGetter, Func<Quaternion> rotationGetter, float updateFrequency, Func<bool> whileCondition)
    {
        while (whileCondition())
        {
            try
            {
                var update = false;
                
                var position = positionGetter();
                if (Vector3.Distance(child.Position, position) > 0.01f)
                {
                    child.transform.position = position;
                    update = true;
                }

                var rotation = rotationGetter();
                if (Quaternion.Angle(child.transform.rotation, rotation) > 0.01f)
                {
                    child.transform.rotation = rotation;
                    update = true;
                }

                if (update)
                {
                    child.UpdateObject();
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