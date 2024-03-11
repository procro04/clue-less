using Clue_Less_Server;
using Clue_Less_Server.Managers;
using Clue_Less_Server.Managers.Interfaces;
using Grpc.Core;

namespace Clue_Less_Server.Services
{
    public class GRPCService : Greeter.GreeterBase
    {
        private readonly ILogger<GRPCService> _logger;
        private readonly IBoardManager _boardManager;
        public GRPCService(ILogger<GRPCService> logger)
        {
            _logger = logger;
            _boardManager = new BoardManager();
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
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
    }
}
