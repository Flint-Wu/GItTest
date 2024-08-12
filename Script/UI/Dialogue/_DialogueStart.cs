using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _DialogueStart : MonoBehaviour
{
    public DialogueEntry _dialogueEntry;

    private UIManager UIManager;

    public TextAsset _dialogueText;

    private StarterAssetsInputs _playerInput;

    private void OnEnable()
    {
        EventManager.DialogueFinishEvent += DialogueFinish;
    }

    private void OnDisable()
    {
        EventManager.DialogueFinishEvent -= DialogueFinish;
    }

    private void Start()
    {
        UIManager = FindObjectOfType<UIManager>();
        _playerInput = FindObjectOfType<StarterAssetsInputs>();
    }

    private void DialogueFinish()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (UIManager.Dialogue.activeSelf)
        {
            Debug.Log("nope");
            return;
        }
        
        if (other.CompareTag("Player") && _playerInput.inter )
        {
            Debug.Log("talk");
            GetComponent<Collider>().enabled = false;
            UIManager.DialogueEnable();
            EventManager.CallDialogutBeginEvent(_dialogueText, _dialogueEntry);
            _playerInput.inter = false;
        }
    }
}
