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

            Receive<PlayMovieMessage>(HandlePlayMovieMessage);
            Receive<StopMovieMessage>(HandleStopMovieMessage);
        }

        private void HandleStopMovieMessage(StopMovieMessage stopMovieMessage)
        {
            if (_currentlyWatching == null)
            {
                ColorConsole.WriteLineRed("ERROR: Cannot stop if nothing is playing.");
            }
            else
            {
                string temp = _currentlyWatching;
                _currentlyWatching = null;
                ColorConsole.WriteLineGreen($"User has stopped watching {temp}.");
            }
        }

        private void HandlePlayMovieMessage(PlayMovieMessage playMovieMessage)
        {
            if (_currentlyWatching != null)
            {
                ColorConsole.WriteLineRed($"ERROR: Cannot start playing another movie ({playMovieMessage.Title}) before stopping the existing one.");
            }
            else
            {
                _currentlyWatching = playMovieMessage.Title;
                ColorConsole.WriteLineGreen($"User is watching {playMovieMessage.Title}.");
            }
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
