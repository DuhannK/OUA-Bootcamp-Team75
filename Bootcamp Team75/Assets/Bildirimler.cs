using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class Bildirimler : MonoBehaviour
{
    private bool isPaused;
    private bool notificationsScheduled = false;

    // Bildirim kimlikleri
    private int firstNotificationId;
    private int secondNotificationId;

    // Start is called before the first frame update
    void Start()
    {
        // Bildirim izni kontrol� ve iste�i
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
        else
        {
            // �zin verilmi�se bildirim kanal�n� olu�tur
            CreateNotificationChannel();
        }
    }

    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void ScheduleNotifications()
    {
        if (notificationsScheduled) return;

        // �lk bildirim
        var firstNotification = new AndroidNotification
        {
            Title = "Sulama Vakti!",
            Text = "�i�eklerinin suya ihtiyac� var gibi g�r�n�yor!",
            FireTime = System.DateTime.Now.AddSeconds(10),
            LargeIcon = "logo", // Resources klas�r�nde bulunan logo.png
            SmallIcon = "logo"  // K���k simgeyi de benzer �ekilde ayarlayabilirsiniz, varsay�lan ikon kullan�labilir.
        };

        firstNotificationId = AndroidNotificationCenter.SendNotification(firstNotification, "channel_id");

        // �kinci bildirim
        var secondNotification = new AndroidNotification
        {
            Title = "Kontrol Zaman�!",
            Text = "Bah�enizin durumunu kontrol edin!",
            FireTime = System.DateTime.Now.AddSeconds(15),
            LargeIcon = "logo", // Resources klas�r�nde bulunan logo.png
            SmallIcon = "logo"  // K���k simgeyi de benzer �ekilde ayarlayabilirsiniz, varsay�lan ikon kullan�labilir.
        };

        secondNotificationId = AndroidNotificationCenter.SendNotification(secondNotification, "channel_id");

        notificationsScheduled = true;
    }

    private void CancelScheduledNotifications()
    {
        // Sadece planlanan bildirimleri iptal et
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(firstNotificationId) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelNotification(firstNotificationId);
        }

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(secondNotificationId) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelNotification(secondNotificationId);
        }

        notificationsScheduled = false;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;

        if (isPaused)
        {
            ScheduleNotifications();
        }
        else
        {
            CancelScheduledNotifications();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            ScheduleNotifications();
        }
        else
        {
            CancelScheduledNotifications();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
