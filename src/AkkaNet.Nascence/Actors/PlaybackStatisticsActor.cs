using System;
using Akka.Actor;
using AkkaNet.Nascence.Exceptions;
using AkkaNet.Nascence.Irrelevant;

namespace AkkaNet.Nascence.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(Decider);
        }

        private Directive Decider(Exception exception)
        {
            if (exception is SimulatedCorruptStateException)
            {
                return Directive.Restart;
            }

            if (exception is SimulatedInvalidMovieException)
            {
                return Directive.Resume;
            }

            return Directive.Restart;
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLine("PlaybackStatisticsActor PreStart", ConsoleColor.White);
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLine("PlaybackStatisticsActor PostStop", ConsoleColor.White);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLine($"PlaybackStatisticsActor PreRestart because: {reason.Message}", ConsoleColor.White);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLine($"PlaybackStatisticsActor PostRestart because: {reason.Message}", ConsoleColor.White);

            base.PostRestart(reason);
        }
        #endregion
    }
}