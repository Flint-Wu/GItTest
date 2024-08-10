using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _DialogueStart : MonoBehaviour
{
    public DialogueEntry _dialogueEntry;

    public GameObject _dialogue;

    public TextAsset _dialogueText;

    private void Awake()
    {
        _dialogue = FindObjectOfType<Dialogue>().gameObject;
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            EventManager.CallDialogutEvent(_dialogueText);

            _dialogue.SetActive(true);
        }
    }
}
