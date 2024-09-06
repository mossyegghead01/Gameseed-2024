// Floating away in the endless abbyss
// Forgotten while everything amiss
// As my soul slowly fades away
// WIth my little voice I say horay...
// Anyway, Scaling time!
using UnityEngine;

public static class ScalingFunctions
{
    // Enemy be chonky
    public static float EnemyScalling(float seconds)
    {
        return (0.0135f * Mathf.Pow(seconds, 2) + 5f);
    }

    // I definetly going to need to change the tuple typing.
    // Maybe migrate the Bounds class here?
    public static WeaponRolling.StatBounds WeaponDamageScalling(float seconds)
    {
        return new((0.0125f * Mathf.Pow(seconds, 2) + 10f), (0.0125f * Mathf.Pow(seconds, 2) + 15f));
    }
}
