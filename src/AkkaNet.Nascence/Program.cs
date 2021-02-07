using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaNet.Nascence.Actors;
using AkkaNet.Nascence.Irrelevant;
using AkkaNet.Nascence.Messages;

namespace AkkaNet.Nascence
{
    class Program
    {
        private static ActorSystem _movieStreamingActorSystem;

        private static void Main(string[] args)
        {
            Console.WriteLine("Creating MovieStreamingActorSystem");
            _movieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            Console.WriteLine("Creating actor supervisory hierarchy");
            _movieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");


            do
            {
                ShortPause();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("enter a command and hit enter");

                var command = Console.ReadLine();

                if (command.StartsWith("play"))
                {
                    int userId = int.Parse(command.Split(',')[1]);
                    string movieTitle = command.Split(',')[2];

                    var message = new PlayMovieMessage(movieTitle, userId);
                    _movieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command.StartsWith("stop"))
                {
                    int userId = int.Parse(command.Split(',')[1]);

                    var message = new StopMovieMessage(userId);
                    _movieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command == "exit")
                {
                    _movieStreamingActorSystem.Terminate().Wait();
                    Console.WriteLine("Actor system shutdown");
                    Console.ReadKey();

                    Environment.Exit(1);
                    return;
                }

            } while (true);
        }

        private static void ShortPause()
        {
            Thread.Sleep(450);
        }
    }
}
