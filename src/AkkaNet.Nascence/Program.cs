using System;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaNet.Nascence.Actors;
using AkkaNet.Nascence.Messages;

namespace AkkaNet.Nascence
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ActorSystem movieActorSystem = ActorSystem.Create(nameof(movieActorSystem));
            Console.WriteLine("Actor system created.");

            Props userActorProps = Props.Create<UserActor>();
            IActorRef userActorRef = movieActorSystem.ActorOf(userActorProps, nameof(UserActor));

            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovieMessage (Anatomy of a Murder)");
            userActorRef.Tell(new PlayMovieMessage("Anatomy of a Murder"));

            Console.ReadKey();
            Console.WriteLine("Sending a PlayMovieMessage (To Kill a Mockingbird)");
            userActorRef.Tell(new PlayMovieMessage("To Kill a Mockingbird"));

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());

            await movieActorSystem.Terminate();
        }
    }
}
