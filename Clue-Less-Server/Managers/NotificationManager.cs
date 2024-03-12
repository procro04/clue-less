using Clue_Less_Server.Managers.Interfaces;

namespace Clue_Less_Server.Managers
{
    public class NotificationManager : INotificationManager
    {
        private static readonly Lazy<NotificationManager> lazy = new Lazy<NotificationManager>(() => new NotificationManager());
        public static NotificationManager Instance { get { return lazy.Value; } }

        public NotificationManager() { }

        public string SendGlobalPlayerNotification(string message)
        {
            return message;
        }
    }
}
