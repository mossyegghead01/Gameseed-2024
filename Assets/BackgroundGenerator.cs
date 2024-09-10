using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject BackgroundPrefab;
    public GameObject Grid;
    // Start is called before the first frame update
    void Start()
    {
        var clone = GameObject.Instantiate(BackgroundPrefab);
        clone.transform.parent = Grid.transform;
    }
}
