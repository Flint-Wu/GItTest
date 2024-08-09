using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueEntry
{
    public Sprite characterPortrait;  // ��ɫͷ��
    public string characterName;       // ��ɫ����
}

public class Dialogue : MonoBehaviour
{
    public Image portraitImage;  // ������ʾͷ���UI���
    public TextMeshProUGUI dialogueText;    // ������ʾ�Ի���UI���
    public TextAsset dialogueTextAsset;

    public DialogueEntry[] DialogueEntries;

    private Dictionary<string, Sprite> characterPortraits = new Dictionary<string, Sprite>();
    private List<(string characterName, string dialogueText)> dialogues = new List<(string characterName, string dialogueText)>();
    private int currentDialogueIndex = 0;

    void Start()
    {
        // ���ؽ�ɫͷ��
        LoadCharacterPortraits();

        // ��ȡ�������Ի��ļ�
        //LoadDialogueFile(dialogueTextAsset);

        // ��ʾ��һ�ζԻ�
        ShowDialogue(currentDialogueIndex);
    }

    private void OnEnable()
    {
        EventManager.DialogueEvent += DialogueBegin;
    }

    private void OnDisable()
    {
        EventManager.DialogueEvent -= DialogueBegin;
    }

    private void DialogueBegin(TextAsset asset)
    {
        LoadDialogueFile(asset);
    }

    void LoadCharacterPortraits()
    {
        // �������ÿ����ɫ��ͷ�񣬸�����Ҫ��Ӹ����ɫ
        characterPortraits[DialogueEntries[0].characterName] = DialogueEntries[0].characterPortrait;
        characterPortraits[DialogueEntries[1].characterName] = DialogueEntries[1].characterPortrait;
    }

    void LoadDialogueFile(TextAsset filePath)
    {
        if (filePath != null)
        {
            string[] lines = filePath.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (line.Contains(":"))
                {
                    string[] splitLine = line.Split(new[] { ':' }, 2);
                    string characterName = splitLine[0].Trim();
                    string dialogueText = splitLine[1].Trim().Trim('"');

                    dialogues.Add((characterName, dialogueText));
                }
            }
        }
        else
        {
            Debug.LogError("�޷��ҵ��Ի��ļ�");
        }
    }

    void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Count)
        {
            Debug.LogWarning("�Ի�����������Χ��");
            return;
        }

        var dialogue = dialogues[index];
        dialogueText.text = dialogue.characterName + ": " + dialogue.dialogueText;

        if (characterPortraits.TryGetValue(dialogue.characterName, out Sprite portrait))
        {
            portraitImage.sprite = portrait;
            portraitImage.SetNativeSize();
        }
        else
        {
            Debug.LogWarning("δ�ҵ���ɫͷ��: " + dialogue.characterName);
        }
    }

    public void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Count)
        {
            ShowDialogue(currentDialogueIndex);
        }
        else
        {
            currentDialogueIndex = 0;
            this.gameObject.SetActive(false);
        }
    }
}

