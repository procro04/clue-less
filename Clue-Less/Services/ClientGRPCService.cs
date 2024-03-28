﻿using Grpc.Net.Client;
using System;
using Greet;
using System.Collections.Generic;
using Managers;

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

        public HeartbeatResponse Heartbeat()
        {
            if (ClientTokenManager.Instance.LoggedInPlayer != null)
            {
                return networkService.Heartbeat(new HeartbeatRequest { PlayerId = ClientTokenManager.Instance.LoggedInPlayer.PlayerId });
            }
            return new HeartbeatResponse { Response = ServerHeartbeatResponse.NoPendingMessages };
        }

        public void AdvancePlayerTurn()
        {
            networkService.AdvancePlayerTurn(new Empty());
        }
    }
}
