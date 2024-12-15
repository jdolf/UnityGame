using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.Rendering;
using static GameData;
using System;

public abstract class Task : IdentifiableScriptableObject, AttributeListener, SavableGameData
{
    private Dictionary<TaskDifficultyClassification, int> DifficultyXPDictionary = new Dictionary<TaskDifficultyClassification, int>
    {
        [TaskDifficultyClassification.Rookie] = 1,
        [TaskDifficultyClassification.Easy] = 3,
        [TaskDifficultyClassification.Medium] = 10,
        [TaskDifficultyClassification.Hard] = 25,
        [TaskDifficultyClassification.Veteran] = 75,
        [TaskDifficultyClassification.Elite] = 150,
    };

    public static List<Task> AllTasks;
    public string Description;
    [HideInInspector]
    [NonSerialized]
    public int Progress = 0;
    public int MaxProgress;
    public TaskDifficultyClassification Difficulty;
    public TaskCategoryClassification Category;
    [HideInInspector]
    [NonSerialized]
    public bool Completed = false;
    protected TaskListener listenerUI;

    public static void LoadTasks() {
        if (AllTasks == null) {
            AllTasks = new List<Task>(Resources.LoadAll<Task>("ScriptableObjects/Tasks")).OrderBy(e => e.Difficulty).ToList();
        }
    }

    public void SetListener(TaskListener listenerUI) {
        this.listenerUI = listenerUI;
    }

    public void SetProgress(int progress) {
        Debug.Log("shouldnt happen");
        this.Progress = progress;

        if (Completed == false && this.Progress >= this.MaxProgress) {
            NotificationManager.AddNotification(new Notification("TASK COMPLETED", this.Description));
            this.Completed = true;
            listenerUI.TaskCompleted();
            CrewManager.IncreaseXP(DifficultyXPDictionary[Difficulty]);
        }

        if (listenerUI != null) {
            listenerUI.ProgressChanged(this.Progress);
        }
    }

    public enum TaskDifficultyClassification {
        Rookie,
        Easy,
        Medium,
        Hard,
        Veteran,
        Elite
    }

    public enum TaskCategoryClassification {
        Combat,
        Mining,
        Farming,
        Crafting,
        Cooking,
        Doctoring,
        Charisma,
        Miscellaneous
    }

    public enum TaskFilterClassification {
        Everything,
        Rookie,
        Easy,
        Medium,
        Hard,
        Veteran,
        Elite,
        Combat,
        Mining,
        Farming,
        Crafting,
        Cooking,
        Doctoring,
        Charisma,
        Miscellaneous
    }

    public abstract void AttributeChanged(int value);

    public void PopulateGameData(GameData gameData)
    {
        TaskData existingTaskData = gameData.Tasks.Where(x => x.ID == ID).FirstOrDefault();

        TaskData taskData = new TaskData();
        taskData.ID = this.ID;
        taskData.Progress = this.Progress;
        taskData.Completed = this.Completed;

        if (existingTaskData != null) {
            gameData.Tasks.Remove(existingTaskData);
        }

        gameData.Tasks.Add(taskData);
    }

    public void LoadFromGameData(GameData gameData)
    {
        TaskData existingTaskData = gameData.Tasks.Where(x => x.ID == ID).FirstOrDefault();

        if (existingTaskData != null) {
            this.Progress = existingTaskData.Progress;
            this.Completed = existingTaskData.Completed;
        }
    }
}