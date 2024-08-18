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

    //public DialogueEntry[] DialogueEntries;
    public List<DialogueEntry> DialogueEntries;

    private Dictionary<string, Sprite> characterPortraits = new Dictionary<string, Sprite>();
    private List<string> dialogues = new List<string>();
    private int currentDialogueIndex = 0;

    public int talkCount;

    void Start()
    {
        // ���ؽ�ɫͷ��
        //LoadCharacterPortraits();

        // ��ȡ�������Ի��ļ�
        //LoadDialogueFile(dialogueTextAsset);

        // ��ʾ��һ�ζԻ�
        //ShowDialogue(currentDialogueIndex);
    }

    private void OnEnable()
    {
        EventManager.DialogueBeginEvent += DialogueBegin;
    }

    private void OnDisable()
    {
        EventManager.DialogueBeginEvent -= DialogueBegin;
    }

    private void Update()
    {
        talkCount = dialogues.Count;
    }

    private void DialogueBegin(TextAsset asset)
    {
        dialogues.Clear();
        LoadCharacterPortraits();

        characterPortraits[DialogueEntries[1].characterName] = DialogueEntries[1].characterPortrait;

        LoadDialogueFile(asset);

        ShowDialogue(currentDialogueIndex);
    }

    void LoadCharacterPortraits()
    {
        // �������ÿ����ɫ��ͷ�񣬸�����Ҫ��Ӹ����ɫ
        for (int i = 0; i < DialogueEntries.Count; i++)
        {
            characterPortraits[DialogueEntries[i].characterName] = DialogueEntries[i].characterPortrait;
        }
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

                    dialogues.Add(dialogueText);
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
        dialogueText.text =  dialogue;

        if (characterPortraits.TryGetValue(dialogue, out Sprite portrait))
        {
            portraitImage.sprite = portrait;
            portraitImage.SetNativeSize();
        }
        else
        {
            Debug.LogWarning("δ�ҵ���ɫͷ��: " + dialogue);
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
            gameObject.SetActive(false);
            DialogueEntries.RemoveAt(1);
            dialogues.Clear();
            EventManager.CallDialogutFinishEvent();
        }
    }
}

