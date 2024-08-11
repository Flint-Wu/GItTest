using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _DialogueStart : MonoBehaviour
{
    public DialogueEntry _dialogueEntry;

    public UIManager UIManager;

    public TextAsset _dialogueText;

    private StarterAssetsInputs _playerInput;

    private void Awake()
    {
        UIManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        _playerInput = FindObjectOfType<StarterAssetsInputs>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerInput.inter)
        {
            UIManager.Dialogue.SetActive(true);

            EventManager.CallDialogutEvent(_dialogueText);
        }
    }
}
