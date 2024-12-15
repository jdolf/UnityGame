using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINotificationBox : MonoBehaviour, NotificationManagerListener
{
    public const int FadeInTimeTotal = 30;
    public const int FadeOutTimeTotal = 30;
    public const int NotificationBoxHeight = 200;
    public RectTransform RectTransform;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Text;
    private Notification Notification;
    private NotificationManager Manager;
    private bool HasNotification = false;
    private int FadedInToGo = 0;
    private int FadedOutToGo = 0; 
    private int DisplayedFramesToGo = 0;

    public void DisplayedNotificationAdded(Notification notification, NotificationManager manager)
    {
        this.Notification = notification;
        HasNotification = true;
        FadedInToGo = FadeInTimeTotal;
        FadedOutToGo = FadeOutTimeTotal;
        DisplayedFramesToGo = notification.FramesToDisplay;
        Title.text = notification.Title;
        Text.text = notification.Text;
        Manager = manager;
    }

    public void DisplayedNotificationRemoved()
    {
        HasNotification = false;
        Notification = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HasNotification) {
            if (FadedInToGo > 0) {
                FadedInToGo--;
                RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, (float)FadedInToGo / (float)FadeInTimeTotal * NotificationBoxHeight);
            } else if (DisplayedFramesToGo > 0) {
                DisplayedFramesToGo--;
            } else if (FadedOutToGo > 0) {
                FadedOutToGo--;
                RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, (1f - (float)FadedOutToGo / (float)FadeInTimeTotal) * NotificationBoxHeight);
            } else {
                Manager.FinishDisplayingNotification(Notification);
            }
        }
    }
}
