namespace Cvdm.Scheduler.Helpers
{
    using System;
    using System.Threading;

    using JetBrains.Annotations;

    internal interface ITimerFactory
    {
        /// <summary>Initializes a new instance of the <see cref="Timer" /> class.</summary>
        /// <param name="callback">A delegate representing a method to be executed.</param>
        /// <param name="state">An object containing information to be used by the callback method, or null.</param>
        /// <param name="dueTime">
        ///     The amount of time to delay before the <paramref name="callback" /> parameter invokes
        ///     its methods. Specify negative one (-1) milliseconds to prevent the timer from starting. Specify zero (0)
        ///     to start the timer immediately.
        /// </param>
        /// <param name="period">
        ///     The time interval between invocations of the methods referenced by
        ///     <paramref name="callback" />. Specify negative one (-1) milliseconds to disable periodic signaling.
        /// </param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     The number of milliseconds in the value of
        ///     <paramref name="dueTime" /> or <paramref name="period" /> is negative and not equal to
        ///     <see cref="F:System.Threading.Timeout.Infinite" />, or is greater than
        ///     <see cref="F:System.Int32.MaxValue" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="callback" /> parameter is null.</exception>
        Timer Create(TimerCallback callback, [CanBeNull] object state, TimeSpan dueTime, TimeSpan period);
    }
}