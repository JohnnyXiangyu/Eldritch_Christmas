using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Billboard : HoveringTooltip
{
    [SerializeField] string textToShow;
    [SerializeField] float emergingDuration = 0.1f;
    [SerializeField] float stayingDuration = 5.0f;

    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] CanvasGroup canvasGroup;

    InputActions input;

    private void Awake()
    {
        input = new InputActions();

        input.Gameplay.Interact.performed += ctx =>
        {
            if (showingTooltip)
                StartCoroutine(BillboardTextRoutine());
        };
    }

    private void Start()
    {
        textBox.text = textToShow;

        canvasGroup.alpha = 0;
    }

    private IEnumerator BillboardTextRoutine()
    {
        showingTooltip = false;
        tooltip.SetActive(false);

        float progress = 0;
        canvasGroup.alpha = 0;

        while (progress < 1)
        {
            progress += 1 / emergingDuration * Time.deltaTime;
            canvasGroup.alpha = Mathf.Min(1, progress);

            yield return null;
        }

        while (entered)
        {
            yield return null;
        }

        float waitTime = 0;
        while (waitTime < stayingDuration)
        {
            waitTime += Time.deltaTime;
            yield return null;
        }

        progress = 1;
        while (progress > 0)
        {
            progress -= 1 / emergingDuration * Time.deltaTime;
            canvasGroup.alpha = Mathf.Max(0, progress);

            yield return null;
        }

        showingTooltip = true;
        if (entered)
            tooltip.SetActive(true);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
