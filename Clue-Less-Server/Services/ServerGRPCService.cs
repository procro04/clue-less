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

        public override Task<PlayerMoveResponse> MovePlayerLocation(PlayerMoveRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PlayerMoveResponse
            {
                PlayerLocation = BoardManager.Instance.MovePlayer(request.PlayerId, request.MoveToLocation)
            });
        }

        public override Task<PlayerActionResponse> ValidatePlayerAction(PlayerActionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PlayerActionResponse
            {
                ValidPlayerAction = ValidationManager.Instance.ValidatePlayerAction(request.ValidPlayerAction)
            });
        }

        public override Task<GlobalPlayerNotificationResponse> SendGlobalPlayerNotification(GlobalPlayerNotificationRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GlobalPlayerNotificationResponse
            {
                Notification = NotificationManager.Instance.SendGlobalPlayerNotification(request.Notification)
            });
        }

        public override Task<HeartbeatResponse> Heartbeat(HeartbeatRequest request, ServerCallContext context)
        {
            return Task.FromResult(NotificationManager.Instance.Heartbeat(request.PlayerId));
        }

        public override Task<LoginReply> AttemptLogin(LoginRequest request, ServerCallContext context)
        {
            return Task.FromResult(BoardManager.Instance.AttemptLogin(request.Name, request.Character));
        }

        public override Task<PlayerTurnOrderResult> GetPlayerTurnOrder(Empty request, ServerCallContext context)
        {
            var result = BoardManager.Instance.GetPlayerTurnOrder();
            PlayerTurnOrderResult turnOrder = new PlayerTurnOrderResult();
            foreach (int playerId in result)
            {
                turnOrder.PlayerIdsInOrder.Add(playerId);
            }            
            return Task.FromResult(turnOrder);
        }

        public override Task<SolutionResponse> GetSolution(SolutionRequest request, ServerCallContext context)
        {
            return Task.FromResult(BoardManager.Instance.GetSolution(request.RequestingPlayerId, request.SuspectedLocation, request.SuspectedCharacter, request.SuspectedWeapon));
        }

        public override Task<MovementButtonResponse> GetMovementButtonOptions(MovementButtonRequest request, ServerCallContext context)
        {
            var result = BoardManager.Instance.GetValidMoveLocations(BoardManager.Instance.GetPlayerFromId(request.PlayerId));
            MovementButtonResponse buttonMovementOptions = new MovementButtonResponse();
            foreach (var option in result)
            {
                buttonMovementOptions.ButtonOptions.Add(option);
            }
            return Task.FromResult(buttonMovementOptions);
        }

        public override Task<Empty> StartGame(Empty request, ServerCallContext context)
        {
            BoardManager.Instance.StartGame();
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> AdvancePlayerTurn(Empty request, ServerCallContext context)
        {
            BoardManager.Instance.AdvancePlayerTurn();
            return Task.FromResult(new Empty());
        }
    }
}
