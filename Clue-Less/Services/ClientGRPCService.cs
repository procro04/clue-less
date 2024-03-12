using Clue_Less_Server;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Services
{
    public class ClientGRPCService
    {
        private static Greeter.GreeterClient networkService;
        private static GrpcChannel channel;
        private static readonly Lazy<ClientGRPCService> lazy = new Lazy<ClientGRPCService>(() => new ClientGRPCService());
        public static ClientGRPCService Instance { get { return lazy.Value; } }

        public ClientGRPCService()
        {
            channel = GrpcChannel.ForAddress("https://localhost:7052");
            networkService = new Greeter.GreeterClient(channel);
        }
        public int MovePlayerLocation(int playerId, int playerMoveLocation)
        {
            return networkService.MovePlayerLocation(new PlayerMoveRequest
            {
                PlayerId = playerId,
                MoveToLocation = playerMoveLocation

            }).PlayerLocation;
        }

        public bool ValidatePlayerAction(bool isValidPlayerAction)
        {
            return networkService.ValidatePlayerAction(new PlayerActionRequest
            {
                ValidPlayerAction = isValidPlayerAction
            }).ValidPlayerAction;
        }

        public string SendGlobalPlayerNotification(string message)
        {
            return networkService.SendGlobalPlayerNotification(new GlobalPlayerNotificationRequest
            {
                Notification = message
            }).Notification;
        }

        public string SayHello(string message)
        {
            return networkService.SayHello(new HelloRequest { Name = message }).Message;
        }
    }
}
