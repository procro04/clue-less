using Clue_Less;
using Grpc.Net.Client;
using System;
using Greet;
using System.Collections.Generic;

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
        public Location MovePlayerLocation(int playerId, Location playerMoveLocation)
        {
            return networkService.MovePlayerLocation(new PlayerMoveRequest
            {
                PlayerId = playerId,
                MoveToLocation = playerMoveLocation

            }).PlayerLocation;
        }

        public LoginReply AttemptLogin(string username, PlayerCharacterOptions character)
        {
            return networkService.AttemptLogin(new LoginRequest
            {
                Name = username,
                Character = character
            });
        }

        public List<int> GetPlayerTurnOrder()
        {
            var result = networkService.GetPlayerTurnOrder(new Empty());
            List<int> playerOrder = new List<int>();
            foreach (int player in result.PlayerIdsInOrder)
            {
                playerOrder.Add(player);
            }

            return playerOrder;
        }

        public void StartGame()
        {
            networkService.StartGame(new Empty());
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
