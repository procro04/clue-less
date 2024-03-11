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
    public class GRPCService
    {
        private static Greeter.GreeterClient networkService;
        private static GrpcChannel channel;
        private static readonly Lazy<GRPCService> lazy = new Lazy<GRPCService>(() => new GRPCService());
        public static GRPCService Instance { get { return lazy.Value; } }

        public GRPCService()
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
        public string SayHello(string message)
        {
            return networkService.SayHello(new HelloRequest { Name = message }).Message;
        }
    }
}
