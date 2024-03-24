using Managers;
using Grpc.Core;
using Greet;
using Models.GameplayObjects;
using System;
using Clue_Less_Server.Managers;

namespace Clue_Less_Server.Services
{
    public class ServerGRPCService : Greeter.GreeterBase
    {
        private readonly ILogger<ServerGRPCService> _logger;
        public ServerGRPCService(ILogger<ServerGRPCService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine("Hi, I'm the Server~! I've received a request!");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<PlayerMoveResponse> MovePlayerLocation(PlayerMoveRequest request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call - MovePlayerLocation - Lets Move a Player! ");
            return Task.FromResult(new PlayerMoveResponse
            {
                PlayerLocation = BoardManager.Instance.MovePlayer(request.PlayerId, request.MoveToLocation)
            });
        }

        public override Task<PlayerActionResponse> ValidatePlayerAction(PlayerActionRequest request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call - Validate Player Action - Is this a valid player action? " + request.ValidPlayerAction.ToString());
            return Task.FromResult(new PlayerActionResponse
            {
                ValidPlayerAction = ValidationManager.Instance.ValidatePlayerAction(request.ValidPlayerAction)
            });
        }

        public override Task<GlobalPlayerNotificationResponse> SendGlobalPlayerNotification(GlobalPlayerNotificationRequest request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call global message! " + request.Notification); 
            return Task.FromResult(new GlobalPlayerNotificationResponse
            {
                Notification = NotificationManager.Instance.SendGlobalPlayerNotification(request.Notification)
            });
        }

        public override Task<HeartbeatResponse> Heartbeat(HeartbeatRequest request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call Heartbeat from player " + request.PlayerId);
            return Task.FromResult(NotificationManager.Instance.Heartbeat(request.PlayerId));
        }

        public override Task<LoginReply> AttemptLogin(LoginRequest request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call Attempt Login! " + request.Name + " " + request.Character.ToString());
            return Task.FromResult(BoardManager.Instance.AttemptLogin(request.Name, request.Character));
        }

        public override Task<PlayerTurnOrderResult> GetPlayerTurnOrder(Empty request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call GetPlayerTurnOrder! ");
            var result = BoardManager.Instance.GetPlayerTurnOrder();
            PlayerTurnOrderResult turnOrder = new PlayerTurnOrderResult();
            foreach(int playerId in result)
            {
                turnOrder.PlayerIdsInOrder.Add(playerId);
            }            
            return Task.FromResult(turnOrder);
        }

        public override Task<SolutionResponse> GetSolution(SolutionRequest request, ServerCallContext context)
        {
            //add some validation here
            Console.WriteLine("Server gRPC call - Get Card Solution!");
            return Task.FromResult(BoardManager.Instance.GetSolution(request.RequestingPlayerId, request.SuspectedLocation, request.SuspectedCharacter, request.SuspectedWeapon));

        }

        public override Task<Empty> StartGame(Empty request, ServerCallContext context)
        {
            Console.WriteLine("Server gRPC call - Get Card Solution!");
            BoardManager.Instance.StartGame();
            return Task.FromResult(new Empty());
        }
    }
}
