using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArea : MonoBehaviour
{
    public LevelName beforeLevel;
    public LevelName currentLevel;
    public LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("something in");
        if (collision.transform.CompareTag("Player"))
        {
            levelManager.LevelChange(beforeLevel, currentLevel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something in");
        if (other.transform.CompareTag("Player"))
        {
            levelManager.LevelChange(beforeLevel, currentLevel);
        }
    }
}
