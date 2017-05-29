namespace Cvdm.Scheduler.Helpers
{
    using System;
    using System.Threading;

    using JetBrains.Annotations;

    internal sealed class TimerFactory : ITimerFactory
    {
        /// <inheritdoc />
        public Timer Create(TimerCallback callback, [CanBeNull] object state, TimeSpan dueTime, TimeSpan period)
        {
            return new Timer(callback, state, dueTime, period);
        }
    }
}