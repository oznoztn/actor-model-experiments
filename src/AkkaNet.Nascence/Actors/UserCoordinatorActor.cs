using System.Collections.Generic;
using Akka.Actor;
using AkkaNet.MovieStreaming.Messages;

namespace AkkaNet.MovieStreaming.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _userActors;

        public UserCoordinatorActor()
        {
            _userActors = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(message =>
            {
                CreateIfNotExists(message.UserId);

                IActorRef userActorRef = _userActors[message.UserId];
                
                userActorRef.Tell(message);
            });

            Receive<StopMovieMessage>(message =>
            {
                CreateIfNotExists(message.UserId);

                IActorRef actorRef = _userActors[message.UserId];

                actorRef.Tell(message);
            });
        }

        private void CreateIfNotExists(int userId)
        {
            if (!_userActors.ContainsKey(userId))
            {
                IActorRef actorRef = Context.ActorOf(Props.Create(() => new UserActor(userId)));

                _userActors[userId] = actorRef;
            }
        }
    }
}