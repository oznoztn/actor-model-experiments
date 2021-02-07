using System;
using Akka.Actor;
using AkkaNet.Nascence.Irrelevant;
using AkkaNet.Nascence.Messages;

namespace AkkaNet.Nascence.Actors
{
    public class UserActor : ReceiveActor
    {
        private readonly int _userId;
        private string _currentlyWatching;

        public UserActor(int userId)
        {
            _userId = userId;
            
            Console.WriteLine($"UserActor:{userId} Constructor");
            ColorConsole.WriteLine("Setting initial behavior to 'Stopped'", ConsoleColor.DarkCyan);
            
            Stopped();
        }

        // Represents the 'playing' behavior
        private void Playing()
        {
            Receive<PlayMovieMessage>(message =>
                ColorConsole.WriteLineRed(
                    $"ERROR: User:{_userId} Cannot start playing another movie ({message}) before stopping the existing one."));
            Receive<StopMovieMessage>(message => StopPlayingMovie());

            ColorConsole.WriteLine($"UserActor:{_userId} has now become 'Playing'.", ConsoleColor.DarkCyan);
        }

        // Represents the 'stopped' behavior
        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.Title));
            Receive<StopMovieMessage>(message => ColorConsole.WriteLineRed($"ERROR: User:{_userId} Cannot stop if nothing is playing."));

            ColorConsole.WriteLine($"UserActor:{_userId} has now become 'Stopped'.", ConsoleColor.DarkCyan);
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;
            ColorConsole.WriteLineGreen($"User:{_userId} is watching {title}.");

            Context
                .ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));

            Become(Playing);
        }

        private void StopPlayingMovie()
        {
            string temp = _currentlyWatching;
            _currentlyWatching = null;
            ColorConsole.WriteLineGreen($"User:{_userId} has stopped watching {temp}.");

            Become(Stopped);
        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow($"UserActor:{_userId} PreStart");
            base.PreStart();
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow($"UserActor:{_userId} PostStop");
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow($"UserActor:{_userId} PreRestart");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow($"UserActor:{_userId} PostRestart");
            base.PostRestart(reason);
        }
    }
}
