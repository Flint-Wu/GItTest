using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UIManager : MonoBehaviour
{
    public GameObject CurrentUI;

    [Header("UIs")]
    public GameObject GamePlay;
    public GameObject GameMenu;
    public GameObject Dialogue;

    public StarterAssetsInputs _playerInput;
    public bool _menuEnabled;

    void Start()
    {
        GameMenu = FindObjectOfType<GameMenu>().gameObject;
        GameMenu.SetActive(false);
        Dialogue = FindObjectOfType<Dialogue>().gameObject;
        Dialogue.SetActive(false);
        _playerInput = FindObjectOfType<StarterAssetsInputs>();

    }

    void Update()
    {
        MenuEnable();
    }

    public void MenuEnable()
    {
        if (_playerInput.menu)
        {
            GameMenu.SetActive(!GameMenu.activeSelf);
            Debug.Log("Menu");
            _playerInput.menu = false;

            if (!GameMenu.activeInHierarchy)
                CurrentUI = GameMenu;
        }
    }

    public void DialogueEnable()
    {
        Dialogue.SetActive(true);
        CurrentUI = Dialogue;
    }
}
