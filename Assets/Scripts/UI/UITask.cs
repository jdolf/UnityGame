using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITask : MonoBehaviour, TaskListener
{
    public Task Task;
    public Image Background;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI ProgressText;
    public TextMeshProUGUI DifficultyText;
    public RectTransform ProgressBar;
    public Image DifficultyIcon;
    public Image CategoryIcon;
    public Image CompletedIcon;
    public Sprite CompletedSprite;
    public Sprite UncompletedSprite;

    public void ProgressChanged(int progress)
    {
        AlterBar(progress, Task.MaxProgress);
    }

    public void TaskCompleted()
    {
        AlterCompleted(true);
    }

    public void Initialize(Task.TaskDifficultyClassification difficulty) {
        AlterBar(Task.Progress, Task.MaxProgress);
        AlterCompleted(Task.Completed);

        Color colorTop = new Color(0f, 0f, 0f);
        Color colorBottom = new Color(0f, 0f, 0f);
        
        if (difficulty == Task.TaskDifficultyClassification.Easy) {
            colorTop = new Color(0.365f, 0.62f, 1f);
            colorBottom = new Color(0.494f, 0.902f, 1f);
        } else if (difficulty == Task.TaskDifficultyClassification.Medium) {
            colorTop = new Color(0.82f, 0.89f, 0f);
            colorBottom = new Color(1f, 1f, 0.569f);
        } else if (difficulty == Task.TaskDifficultyClassification.Hard) {
            colorTop = new Color(0.769f, 0f, 0f);
            colorBottom = new Color(1f, 0.376f, 0.376f);
        } else if (difficulty == Task.TaskDifficultyClassification.Veteran) {
            colorTop = new Color(0.447f, 0f, 0.471f);
            colorBottom = new Color(0.863f, 0f, 0.91f);
        } else if (difficulty == Task.TaskDifficultyClassification.Elite) {
            colorTop = new Color(0f, 0f, 0f);
            colorBottom = new Color(0.671f, 0f, 0f);
        }




        DifficultyText.colorGradient = new VertexGradient(colorTop, colorTop, colorBottom, colorBottom);
    }

    private void AlterBar(int progress, int maxProgress) {
        ProgressText.text = progress + "/" + maxProgress;
        float progressBarWidthFactor = 0;

        if (progress < maxProgress) {
            progressBarWidthFactor = (float) progress / (float) maxProgress;
            
        } else {
            progressBarWidthFactor = 1f;
        }

        ProgressBar.sizeDelta = new Vector2(progressBarWidthFactor * 600, ProgressBar.sizeDelta.y);
    }

    private void AlterCompleted(bool completed) {

        if (completed) {
            CompletedIcon.sprite = CompletedSprite;
            Background.color = new Color(0f, 1f, 0f, 0.392f);
        } else {
            CompletedIcon.sprite = UncompletedSprite;
        }
    }
}
