
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenMessageController : MonoBehaviour
{
    public static ScreenMessageController Instance { get; private set; }

    [System.Serializable]
    public class Message
    {
        [TextArea(1, 4)]
        public string text;

        public Color color = Color.white;
        public float typingSpeed = 30f;
        public float holdTime = 2f;
        public float fadeInTime = 0.5f;
        public float fadeOutTime = 1f;
        public int priority = 0;
    }

    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMPTerminalTypewriter typewriter;

    private readonly List<Message> queue = new();
    private Coroutine queueRoutine;
    private bool isPlaying;
    private int currentPriority = int.MinValue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        canvasGroup.alpha = 0f;
    }

    public void ShowMessage(
        string message,
        Color color,
        float typingSpeed = 30f,
        float holdTime = 2f,
        int priority = 0,
        float fadeInTime = 0.5f,
        float fadeOutTime = 1f)
    {
        Message newMessage = new Message
        {
            text = message,
            color = color,
            typingSpeed = typingSpeed,
            holdTime = holdTime,
            priority = priority,
            fadeInTime = fadeInTime,
            fadeOutTime = fadeOutTime
        };

        queue.Add(newMessage);

        // Highest priority messages are played first.
        queue.Sort((a, b) => b.priority.CompareTo(a.priority));

        if (queueRoutine == null)
            queueRoutine = StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        while (queue.Count > 0)
        {
            Message message = queue[0];
            queue.RemoveAt(0);

            isPlaying = true;
            currentPriority = message.priority;

            yield return StartCoroutine(PlayMessage(message));

            isPlaying = false;
            currentPriority = int.MinValue;
        }

        queueRoutine = null;
    }

    private IEnumerator PlayMessage(Message message)
    {
        titleText.color = message.color;

        // Fade in.
        canvasGroup.alpha = 0f;

        float t = 0f;

        if (message.fadeInTime > 0f)
        {
            while (t < message.fadeInTime)
            {
                t += Time.deltaTime;
                canvasGroup.alpha =
                    Mathf.Clamp01(t / message.fadeInTime);

                yield return null;
            }
        }

        canvasGroup.alpha = 1f;

        // Type the message using the shared typewriter.
        yield return StartCoroutine(
            typewriter.TypeText(message.text, message.typingSpeed)
        );

        yield return new WaitForSeconds(message.holdTime);

        // Fade out.
        t = 0f;

        if (message.fadeOutTime > 0f)
        {
            while (t < message.fadeOutTime)
            {
                t += Time.deltaTime;
                canvasGroup.alpha =
                    1f - Mathf.Clamp01(t / message.fadeOutTime);

                yield return null;
            }
        }

        canvasGroup.alpha = 0f;
        typewriter.StopTyping();
    }

    public bool IsPlaying => isPlaying;

    public void ClearQueue()
    {
        queue.Clear();
    }
}
