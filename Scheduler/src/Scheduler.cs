namespace Cvdm.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Cvdm.Scheduler.Helpers;

    using JetBrains.Annotations;

    /// <inheritdoc />
    /// <remarks>
    ///     This class keeps internal references to all created timers, so you don't have to do that yourself if
    ///     the scheduler lives at least as long as the timer (e.g. if the scheduler is a singleton component in your
    ///     application).
    /// </remarks>
    public class Scheduler : IScheduler
    {
        // Keep a reference to all timers to avoid garbage collection
        [UsedImplicitly] private readonly IList<Timer> timers = new List<Timer>();

        private readonly ITimerFactory timerFactory = new TimerFactory();
        private readonly ITimeProvider time = new TimeProvider();

        public Scheduler()
        {
        }

        internal Scheduler(ITimerFactory timerFactory, ITimeProvider time)
        {
            this.timerFactory = timerFactory;
            this.time = time;
        }

        /// <inheritdoc />
        public Timer ScheduleRecurring(TimerCallback callback, object state, DateTimeOffset startTime, TimeSpan interval)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (interval.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval must be positive.");

            return this.ScheduleInner(callback, state, startTime, interval);
        }

        /// <inheritdoc />
        public Timer ScheduleRecurring(TimerCallback callback, object state, TimeSpan startAfter, TimeSpan interval)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state), "State cannot be null. Use the stateless overload instead.");

            if (interval.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval must be positive.");

            return this.ScheduleInner(callback, state, startAfter, interval);
        }

        /// <inheritdoc />
        public Timer ScheduleRecurring(Action callback, DateTimeOffset startTime, TimeSpan interval)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (interval.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval must be positive.");

            return this.ScheduleInner(_ => callback(), null, startTime, interval);
        }

        /// <inheritdoc />
        public Timer ScheduleRecurring(Action callback, TimeSpan startAfter, TimeSpan interval)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (interval.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Interval must be positive.");

            return this.ScheduleInner(_ => callback(), null, startAfter, interval);
        }

        /// <inheritdoc />
        public Timer ScheduleOneTime(TimerCallback callback, object state, DateTimeOffset startTime)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state), "State cannot be null. Use the stateless overload instead.");

            return this.ScheduleInner(callback, state, startTime, TimeSpan.FromMilliseconds(-1));
        }

        /// <inheritdoc />
        public Timer ScheduleOneTime(TimerCallback callback, object state, TimeSpan startAfter)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state), "State cannot be null. Use the stateless overload instead.");

            return this.ScheduleInner(callback, state, startAfter, TimeSpan.FromMilliseconds(-1));
        }

        /// <inheritdoc />
        public Timer ScheduleOneTime(Action callback, DateTimeOffset startTime)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            return this.ScheduleInner(_ => callback(), null, startTime, TimeSpan.FromMilliseconds(-1));
        }

        /// <inheritdoc />
        public Timer ScheduleOneTime(Action callback, TimeSpan startAfter)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            return this.ScheduleInner(_ => callback(), null, startAfter, TimeSpan.FromMilliseconds(-1));
        }

        private Timer ScheduleInner(
            [NotNull] TimerCallback callback,
            [CanBeNull] object state,
            TimeSpan startAfter,
            TimeSpan interval)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            if (startAfter.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(startAfter), "Delay must not be negative.");

            Timer timer = this.timerFactory.Create(callback, state, startAfter, interval);
            this.timers.Add(timer);
            return timer;
        }

        private Timer ScheduleInner(
            [NotNull] TimerCallback callback,
            [CanBeNull] object state,
            DateTimeOffset startTime,
            TimeSpan interval)
        {
            TimeSpan startAfter = startTime - this.time.Now;
            if (startAfter.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(startTime), "Start time must be in the future.");

            return this.ScheduleInner(callback, state, startAfter, interval);
        }
    }
}