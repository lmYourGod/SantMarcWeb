using System.Collections;
using System.Collections.Generic;
using Members.Carlos.Scripts.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Members.Carlos.Scripts.Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private TaskManager taskManager;

        [Header("--- DIALOGUES ---")]
        [Space(10)]
        [SerializeField] private Animator dialogueAnimator;
        [SerializeField] private DialogueObject firstDialogueObject;
        [SerializeField] private DialogueObject[] dialogueObjects;
        [SerializeField] private TextMeshProUGUI tmpName;
        [SerializeField] private TextMeshProUGUI tmpSentence;
        [SerializeField] private Image avatarImg;

        private Queue<string> _sentences;
        private static readonly int DialogueIsOn = Animator.StringToHash("DialogueIsOn");

        // Start is called before the first frame update
        void Start()
        {
            _sentences = new Queue<string>();

            StartDialogue(firstDialogueObject);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                DisplayNextSentence();
            }
        }

        public void StartDialogue(DialogueObject dialogueObject)
        {
            gameManager.isDialogue = true;
     
            dialogueAnimator.SetBool(DialogueIsOn, true);

            tmpName.text = dialogueObject.nameTxt;
            avatarImg.sprite = dialogueObject.spriteImg;
        
            _sentences.Clear();
        
            foreach (string sentence in dialogueObject.sentenceTxt)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        private void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
        
            string sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            tmpSentence.text = "";
            foreach (char letter in sentence)
            {
                tmpSentence.text += letter;
                yield return null;
            }
        }

        private void EndDialogue()
        {
            gameManager.isDialogue = false;
            dialogueAnimator.SetBool(DialogueIsOn, false);

            taskManager.TasksBoolCheck();
        
            if (taskManager.teacherFound && !taskManager.teacherFound1Vz)
            {
                taskManager.teacherFound1Vz = true;
                StartDialogue(dialogueObjects[0]);
                return;
            }

            if (taskManager.teacherFound1Vz)
            {
                GameObject.FindWithTag("Profe").GetComponent<DialogueTrigger>().dialogueObject = dialogueObjects[1];
            }
        }
    }
}
