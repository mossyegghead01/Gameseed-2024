// Floating away in the endless abbyss
// Forgotten while everything amiss
// As my soul slowly fades away
// With my little voice I say horay...
// Anyway, Scaling time!
using UnityEngine;

public static class ScalingFunctions
{
    // Enemy be chonky
    public static float EnemyScalling(int score)
    {
        return (0.0135f * Mathf.Pow(score, 2) + 5f);
    }

    // I definetly going to need to change the tuple typing.
    // Maybe migrate the Bounds class here?
    public static WeaponRolling.StatBounds WeaponDamageScalling(int score)
    {
        return new((0.0125f * Mathf.Pow(score, 2) + 10f), (0.0125f * Mathf.Pow(score, 2) + 15f));
    }
}
