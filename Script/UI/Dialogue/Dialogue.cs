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
    public Sprite characterPortrait;  // 角色头像
    public string characterName;       // 角色名字
}

public class Dialogue : MonoBehaviour
{
    public Image portraitImage;  // 用于显示头像的UI组件
    public TextMeshProUGUI dialogueText;    // 用于显示对话的UI组件
    public TextAsset dialogueTextAsset;

    //public DialogueEntry[] DialogueEntries;
    public List<DialogueEntry> DialogueEntries;

    private Dictionary<string, Sprite> characterPortraits = new Dictionary<string, Sprite>();
    private List<(string characterName, string dialogueText)> dialogues = new List<(string characterName, string dialogueText)>();
    private int currentDialogueIndex = 0;

    public int talkCount;

    void Start()
    {
        // 加载角色头像
        //LoadCharacterPortraits();

        // 读取并解析对话文件
        //LoadDialogueFile(dialogueTextAsset);

        // 显示第一段对话
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

    private void DialogueBegin(TextAsset asset, DialogueEntry dialogueEntry)
    {
        dialogues.Clear();
        LoadCharacterPortraits();

        DialogueEntries.Add(dialogueEntry);
        characterPortraits[DialogueEntries[1].characterName] = DialogueEntries[1].characterPortrait;

        LoadDialogueFile(asset);

        ShowDialogue(currentDialogueIndex);
    }

    void LoadCharacterPortraits()
    {
        // 这里加载每个角色的头像，根据需要添加更多角色
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

                    dialogues.Add((characterName, dialogueText));
                }
            }
        }
        else
        {
            Debug.LogError("无法找到对话文件");
        }
    }

    void ShowDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Count)
        {
            Debug.LogWarning("对话索引超出范围！");
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
            Debug.LogWarning("未找到角色头像: " + dialogue.characterName);
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

