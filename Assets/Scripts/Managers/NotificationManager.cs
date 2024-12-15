using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public UINotificationBox UIManagementScreen;
    public static NotificationManager Instance { get; private set; }
    public List<Notification> Notifications = new List<Notification>();
    private Notification DisplayedNotification;
    private NotificationManagerListener uiListener;
    private bool IsDisplayingNotification = false;

    public void Awake() {
        Instance = this;
        Instance.uiListener = UIManagementScreen;
    }

    private void EvaluateNextNotificationToDisplay() {
        if (IsDisplayingNotification == false && Notifications.Count > 0) {
            DisplayedNotification = Notifications.FirstOrDefault();
            IsDisplayingNotification = true;
            Debug.Log("Displaying notification");
            Debug.Log(uiListener);
            uiListener.DisplayedNotificationAdded(DisplayedNotification, this);
        }
    }

    public static void AddNotification(Notification notification) {
        Instance.Notifications.Add(notification);
        Instance.EvaluateNextNotificationToDisplay();
        Debug.Log("notification added");
    }

    public void FinishDisplayingNotification(Notification notification) {
        uiListener.DisplayedNotificationRemoved();
        Notifications.Remove(notification);
        IsDisplayingNotification = false;
        EvaluateNextNotificationToDisplay();
    }
}

public class Notification {
    public int FramesToDisplay;
    public string Title;
    public string Text;

    public Notification(string title, string text, int framesToDisplay = 200) {
        Title = title;
        Text = text;
        FramesToDisplay = framesToDisplay;
    }
}
