
using Greet;

namespace Clue_Less_Server.Managers
{
    public class NotificationManager
    {
        private static readonly Lazy<NotificationManager> lazy = new Lazy<NotificationManager>(() => new NotificationManager());
        public static NotificationManager Instance { get { return lazy.Value; } }

        public NotificationManager() { }
        
        Dictionary<int, Stack<HeartbeatResponse> > PlayerNotificationQueue = new Dictionary<int, Stack<HeartbeatResponse>>();

        public void NewQueueForPlayer(int playerId)
        {
            PlayerNotificationQueue[playerId] = new Stack<HeartbeatResponse>();
        }
        public HeartbeatResponse Heartbeat(int playerId)
        {            
            //We handle globals before we handle specific player queue stuff.
            //One message out per heartbeat.            
            if(PlayerNotificationQueue.ContainsKey(playerId))
            {
                HeartbeatResponse playerResponse = new HeartbeatResponse();
                
                if (PlayerNotificationQueue[playerId].TryPop(out playerResponse))
                {
                    return playerResponse;
                }
                else
                {
                    HeartbeatResponse noNotificationResponse = new HeartbeatResponse();
                    noNotificationResponse.Response = ServerHeartbeatResponse.NoPendingMessages;
                    return noNotificationResponse;
                }
            }

            HeartbeatResponse response = new HeartbeatResponse();
            response.Response = ServerHeartbeatResponse.Error; //Player id not registered to the queue            
            return response;
        }

        public void SendGlobalMessage(HeartbeatResponse message)
        {
            foreach (var item in PlayerNotificationQueue)
            {
                item.Value.Push(message);
            }
        }

        public string SendGlobalPlayerNotification(string message)
        {
            HeartbeatResponse heartbeatResponse = new HeartbeatResponse();
            heartbeatResponse.Response = ServerHeartbeatResponse.GlobalPlayerNotification;
            heartbeatResponse.GlobalPlayerNotification = new GlobalPlayerNotificationResponse();
            heartbeatResponse.GlobalPlayerNotification.Notification = message; 
            foreach (var item in PlayerNotificationQueue)
            {
                item.Value.Push(heartbeatResponse);
            }
            return message;
        }
    }
}
