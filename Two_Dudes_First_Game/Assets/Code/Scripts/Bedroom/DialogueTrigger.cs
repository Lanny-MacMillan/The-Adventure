using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("After it's done")]
    [SerializeField] private string doneFlag;
    [SerializeField] private string doneKnot = "after_key";

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        DialogueManager manager = DialogueManager.GetInstance();
        if (playerInRange && manager != null && !manager.dialogueIsPlaying && !PauseMenu.IsPaused)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                bool done = !string.IsNullOrEmpty(doneFlag) && PlayerPrefs.GetInt(doneFlag) == 1;
                manager.EnterDialogueMode(inkJSON, done ? doneKnot : null);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
