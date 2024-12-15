using TMPro;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    public const float Padding = 5f;
    public string Suffix = "";
    private RectTransform ParentTransform;
    public TextMeshProUGUI ProgressBarText;
    public RectTransform ProgressBar;
    private float TotalProgressWidth;

    public void Initialize(int progress, int targetProgress) {
        ProgressChanged(progress, targetProgress);
    }

    public void ProgressChanged(int progress, int targetProgress)
    {
        string formattedText;
        formattedText = progress + "/" + targetProgress;

        if (Suffix != "") {
            formattedText += " " + Suffix;
        }

        Debug.Log(-((1f - (float) progress / (float) targetProgress) * TotalProgressWidth + Padding));
        Debug.Log(progress);
        Debug.Log(targetProgress);
        Debug.Log("progresswidth" + TotalProgressWidth);

        if (ParentTransform == null) {
            CalcParentTransformAndWidth();
        }

        ProgressBarText.text = formattedText;
        // Make sure to readd parent in inspector
        ProgressBar.offsetMax = new Vector2(-((1f - (float) progress / (float) targetProgress) * TotalProgressWidth + Padding), ProgressBar.offsetMax.y);
    }

    private void CalcParentTransformAndWidth() {
        if (transform.parent.gameObject.TryGetComponent<RectTransform>(out RectTransform rectTransform)) {
            ParentTransform = rectTransform;
        }

        TotalProgressWidth = ParentTransform.sizeDelta.x - Padding * 2;
    }
}
