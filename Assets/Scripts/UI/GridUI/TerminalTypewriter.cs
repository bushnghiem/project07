using System.Collections;
using TMPro;
using UnityEngine;

public class TMPTerminalTypewriter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text cursor;

    [Header("Typing")]
    [SerializeField] private float charactersPerSecond = 30f;
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float endDelay = 1f;

    [Header("Cursor")]
    [SerializeField] private float cursorBlinkRate = 0.25f;
    [SerializeField] private Vector3 cursorOffset = new Vector3(10f, -10f);

    [Header("Sound")]
    [SerializeField] private bool playSound = true;
    [SerializeField] private SoundType typingSound = SoundType.BUTTON;
    [SerializeField] private float soundVolume = 0.35f;


    private int visibleCharacters;
    private int totalCharacters;

    private Coroutine cursorRoutine;

    private void Awake()
    {
        cursor.gameObject.SetActive(false);
    }


    public IEnumerator TypeText(string message)
    {
        StopTyping();

        yield return new WaitForSeconds(startDelay);


        text.text = message;
        text.maxVisibleCharacters = 0;
        text.ForceMeshUpdate();


        totalCharacters = text.textInfo.characterCount;
        visibleCharacters = 0;


        cursor.gameObject.SetActive(true);
        cursorRoutine = StartCoroutine(CursorBlink());


        float delay = 1f / charactersPerSecond;


        while (visibleCharacters < totalCharacters)
        {
            visibleCharacters++;

            text.maxVisibleCharacters = visibleCharacters;

            UpdateCursorPosition();


            if (playSound)
            {
                SoundManager.PlaySound(
                    typingSound,
                    soundVolume
                );
            }


            yield return new WaitForSeconds(delay);
        }


        UpdateCursorPosition();

        yield return new WaitForSeconds(endDelay);

        StopTyping();
    }



    private IEnumerator CursorBlink()
    {
        while (true)
        {
            cursor.enabled = !cursor.enabled;

            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }



    private void LateUpdate()
    {
        if (cursor.gameObject.activeSelf)
        {
            UpdateCursorPosition();
        }
    }



    private void UpdateCursorPosition()
    {
        text.ForceMeshUpdate();

        if (visibleCharacters <= 0)
        {
            cursor.rectTransform.position =
                text.rectTransform.position;

            return;
        }


        int index = Mathf.Clamp(
            visibleCharacters - 1,
            0,
            text.textInfo.characterCount - 1
        );


        TMP_CharacterInfo character =
            text.textInfo.characterInfo[index];


        if (!character.isVisible)
            return;


        // Use the baseline position after the character
        Vector3 worldPosition =
            text.transform.TransformPoint(
                character.bottomRight
            );


        cursor.rectTransform.position = worldPosition + cursorOffset;


        cursor.fontSize = character.pointSize;
    }



    public void StopTyping()
    {
        if (cursorRoutine != null)
        {
            StopCoroutine(cursorRoutine);
            cursorRoutine = null;
        }


        cursor.gameObject.SetActive(false);

        text.maxVisibleCharacters = int.MaxValue;
    }
}