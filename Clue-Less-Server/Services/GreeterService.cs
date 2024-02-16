using Clue_Less_Server;
using Clue_Less_Server.Managers;
using Clue_Less_Server.Managers.Interfaces;
using Grpc.Core;

namespace Clue_Less_Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IBoardManager _boardManager;
        public GreeterService(ILogger<GreeterService> logger)
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

        //public override Task<IntegerResponse> GetPlayerLocation()
        //{
        //    return Task.FromResult(_boardManager.GetPlayerLocation()).Result;
        //}
    }
}
