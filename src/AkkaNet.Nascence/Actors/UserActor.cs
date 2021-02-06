using System;
using Akka.Actor;
using AkkaNet.Nascence.Irrelevant;
using AkkaNet.Nascence.Messages;

namespace AkkaNet.Nascence.Actors
{
    public class UserActor : ReceiveActor
    {
        private string _currentlyWatching;

        public UserActor()
        {
            Console.WriteLine("UserActor Constructor");

            ColorConsole.WriteLine("Setting initial behavior to 'stopped'", ConsoleColor.DarkCyan);
            Stopped();
        }

        // Represents the playing behavior
        private void Playing()
        {
            Receive<PlayMovieMessage>(message =>
                ColorConsole.WriteLineRed(
                    $"ERROR: Cannot start playing another movie ({message}) before stopping the existing one."));
            Receive<StopMovieMessage>(message => StopPlayingMovie());

            ColorConsole.WriteLine("UserActor has now become 'Playing'.", ConsoleColor.DarkCyan);
        }

        // Represents the stopped behavior
        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.Title));
            Receive<StopMovieMessage>(message => ColorConsole.WriteLineRed("ERROR: Cannot stop if nothing is playing."));

            ColorConsole.WriteLine("UserActor has now become 'Stopped'.", ConsoleColor.DarkCyan);
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;
            ColorConsole.WriteLineGreen($"User is watching {title}.");

            Become(Playing);
        }

        private void StopPlayingMovie()
        {
            string temp = _currentlyWatching;
            _currentlyWatching = null;
            ColorConsole.WriteLineGreen($"User has stopped watching {temp}.");

            Become(Stopped);
        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow("UserActor:PreStart");
            base.PreStart();
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow("UserActor:PostStop");
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow("UserActor:PreRestart");
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow("UserActor:PostRestart");
            base.PostRestart(reason);
        }
    }
}
