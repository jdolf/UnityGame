using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TaskManager : MonoBehaviour, Savable, UICategoryListener
{
    
    public GameObject UITask;
    public Transform TasksParent;
    public List<TaskDifficultySpritePair> DifficultySpritePairs;
    public List<TaskCategorySpritePair> CategorySpritePairs;
    protected List<UITask> uiTasks = new List<UITask>();
    protected List<Task> tasks = new List<Task>();
    public UICategory UICategory;
    private string FilterValue = "everything";

    void Awake()
    {
        UICategory.RegisterListener(this);
    }

    void Start() {
        DisplayTasks();
    }

    protected void DisplayTasks() {
        uiTasks.ForEach(x => Destroy(x.gameObject));
        uiTasks.Clear();

        foreach (Task task in tasks.OrderBy(x => x.Completed).ThenBy(x => x.Difficulty).ThenBy(x => x.Category)) {
            bool valid = false;

            switch (FilterValue) {
                case "rookie":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Rookie;
                    break;
                case "easy":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Easy;
                    break;
                case "medium":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Medium;
                    break;
                case "hard":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Hard;
                    break;
                case "veteran":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Veteran;
                    break;
                case "elite":
                    valid = task.Difficulty == Task.TaskDifficultyClassification.Elite;
                    break;
                case "combat":
                    valid = task.Category == Task.TaskCategoryClassification.Combat;
                    break;
                case "mining":
                    valid = task.Category == Task.TaskCategoryClassification.Mining;
                    break;
                case "cooking":
                    valid = task.Category == Task.TaskCategoryClassification.Cooking;
                    break;
                case "crafting":
                    valid = task.Category == Task.TaskCategoryClassification.Crafting;
                    break;
                case "farming":
                    valid = task.Category == Task.TaskCategoryClassification.Farming;
                    break;
                case "doctoring":
                    valid = task.Category == Task.TaskCategoryClassification.Doctoring;
                    break;
                case "charisma":
                    valid = task.Category == Task.TaskCategoryClassification.Charisma;
                    break;
                case "miscellaneous":
                    valid = task.Category == Task.TaskCategoryClassification.Miscellaneous;
                    break;
                default:
                    valid = true;
                    break;
            }

            if (valid) {
                InstantiateUITask(task);
            }
        }
    }

    private void InstantiateUITask(Task task) {
        UITask uiTask = Instantiate(this.UITask, Vector3.zero, Quaternion.identity).GetComponent<UITask>();
        task.SetListener(uiTask);
        uiTask.Task = task;
        uiTask.Description.text = task.Description;
        uiTask.DifficultyText.text = task.Difficulty.ToString();
        uiTask.DifficultyIcon.sprite = DifficultySpritePairs.Find(element => element.Classification == task.Difficulty).Sprite;
        uiTask.CategoryIcon.sprite = CategorySpritePairs.Find(element => element.Classification == task.Category).Sprite;
        uiTasks.Add(uiTask);
        uiTask.transform.SetParent(TasksParent);
        uiTask.transform.localScale = Vector3.one;
        uiTask.Initialize(task.Difficulty);
    }

    public void Save(GameData gameData)
    {
        foreach (Task task in tasks) {
            task.PopulateGameData(gameData);
        }
    }

    public void Load(GameData gameData)
    {
        tasks = Task.AllTasks;
        foreach (Task task in tasks) {
            task.LoadFromGameData(GameDataManager.GameData);
        }
    }

    public void CategorySelected(string filterValue)
    {
        FilterValue = filterValue;
        DisplayTasks();
    }
}

[System.Serializable]
public class TaskDifficultySpritePair {
    public Task.TaskDifficultyClassification Classification;
    public Sprite Sprite;
}

[System.Serializable]
public class TaskCategorySpritePair {
    public Task.TaskCategoryClassification Classification;
    public Sprite Sprite;
}