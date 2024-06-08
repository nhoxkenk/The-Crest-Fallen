using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIPopup : MonoBehaviour
{
    [Header("Interaction Pop Up")]
    [SerializeField] private TextMeshProUGUI interactionMessageText;
    [SerializeField] private GameObject interactionUIGameObject;

    [Header("YOU DIED Pop Up")]
    [SerializeField] private GameObject youDiedPopUpGameObject;
    [SerializeField] private TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] private TextMeshProUGUI youDiedPopUpText;
    [SerializeField] private CanvasGroup youDiedPopUpCanvasGroup;

    [Header("Boss Defeated Pop Up")]
    [SerializeField] private GameObject bossDefeatedPopUpGameObject;
    [SerializeField] private TextMeshProUGUI bossDefeatedPopUpBackgroundText;
    [SerializeField] private TextMeshProUGUI bossDefeatedPopUpText;
    [SerializeField] private CanvasGroup bossDefeatedPopUpCanvasGroup;

    public void SendYouDiedPopUp()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8f, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(FadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage)
    {
        bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;
        bossDefeatedPopUpText.text = bossDefeatedMessage;
        bossDefeatedPopUpGameObject.SetActive(true);
        bossDefeatedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(bossDefeatedPopUpBackgroundText, 8f, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(bossDefeatedPopUpCanvasGroup, 5));
        StartCoroutine(FadeOutPopUpOverTime(bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    public void SendMessageFromInteractToPlayer(string message)
    {
        PlayerUI.Instance.popUpWindowIsOpen = true;
        interactionMessageText.text = message;
        interactionUIGameObject.SetActive(true);
    }

    public void ClosePopUpMessageFromInteract()
    {
        PlayerUI.Instance.popUpWindowIsOpen = false;
        interactionUIGameObject.SetActive(false);
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        CountdownTimer timer = new CountdownTimer(duration);

        if (duration > 0)
        {
            text.characterSpacing = 0;
            timer.Start();

            yield return null;

            while (!timer.IsFinished())
            {
                timer.Tick(Time.deltaTime);
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
            
        }
    }

    private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        CountdownTimer timer = new CountdownTimer(duration);

        if (duration > 0)
        {
            canvas.alpha = 0;
            timer.Start();

            yield return null;

            while (!timer.IsFinished())
            {
                timer.Tick(Time.deltaTime);
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 1;
        yield return null;
    }

    private IEnumerator FadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        CountdownTimer timer = new CountdownTimer(duration);

        if (duration > 0)
        {
            while(delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1;
            timer.Start();

            yield return null;

            while (!timer.IsFinished())
            {
                timer.Tick(Time.deltaTime);
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0;
        yield return null;
    }
}
