using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyType type = EnemyType.Broccoli;
    // Start is called before the first frame update
    void Start()
    {
        if (type == EnemyType.Broccoli)
            GetComponent<Animator>().SetInteger("type", 1);
        else if (type == EnemyType.Carrot)
            GetComponent<Animator>().SetInteger("type", 2);
        else if (type == EnemyType.Corn)
            GetComponent<Animator>().SetInteger("type", 3);
        else if (type == EnemyType.Cauliflower)
            GetComponent<Animator>().SetInteger("type", 4);
        else if (type == EnemyType.Eggplant)
            GetComponent<Animator>().SetInteger("type", 5);
        else if (type == EnemyType.Tomato)
            GetComponent<Animator>().SetInteger("type", 6);
    }
}

public enum EnemyType
{
    Broccoli,
    Carrot,
    Corn,
    Cauliflower,
    Eggplant,
    Tomato
}
