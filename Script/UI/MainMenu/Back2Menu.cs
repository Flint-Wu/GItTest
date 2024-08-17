using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back2Menu : MonoBehaviour
{
    public GameObject current, menu;

    private void Start()
    {
        current = gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            current.SetActive(false);
        }
    }
}
