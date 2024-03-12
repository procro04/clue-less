using Clue_Less_Server;
using Clue_Less_Server.Managers;
using Clue_Less_Server.Managers.Interfaces;
using Grpc.Core;

namespace Clue_Less_Server.Services
{
    public class ServerGRPCService : Greeter.GreeterBase
    {
        private readonly ILogger<ServerGRPCService> _logger;
        private readonly IBoardManager _boardManager;
        public ServerGRPCService(ILogger<ServerGRPCService> logger)
        {
            _logger = logger;
            _boardManager = new BoardManager();
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
    }
}
