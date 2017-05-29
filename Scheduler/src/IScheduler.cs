namespace Cvdm.Scheduler
{
    using System;
    using System.Threading;

    /// <summary>Schedules execution of code.</summary>
    public interface IScheduler
    {
        /// <summary>Schedules a state-accepting callback to execute at regular intervals, starting at a fixed time.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="state">An object containing information to be used by the callback method.</param>
        /// <param name="startTime">When to first execute the callback. Must be in the future.</param>
        /// <param name="interval">How often to execute the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="state" /> is <see langword="null" /> (use the stateless overload instead).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startTime" /> is in the past.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval" /> is not positive.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleRecurring(TimerCallback callback, object state, DateTimeOffset startTime, TimeSpan interval);

        /// <summary>
        ///     Schedules a state-accepting callback to execute at regular intervals, starting after an initial
        ///     delay.
        /// </summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="state">An object containing information to be used by the callback method.</param>
        /// <param name="startAfter">The amount of time to delay before first executing the callback.</param>
        /// <param name="interval">How often to execute the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="state" /> is <see langword="null" /> (use the stateless overload instead).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startAfter" /> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval" /> is not positive.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleRecurring(TimerCallback callback, object state, TimeSpan startAfter, TimeSpan interval);

        /// <summary>Schedules an action to execute at regular intervals, starting at a fixed time.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="startTime">When to first execute the callback. Must be in the future.</param>
        /// <param name="interval">How often to execute the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startTime" /> is in the past.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval" /> is not positive.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleRecurring(Action callback, DateTimeOffset startTime, TimeSpan interval);

        /// <summary>Schedules an action to execute at regular intervals, starting after an initial delay.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="startAfter">The amount of time to delay before first executing the callback.</param>
        /// <param name="interval">How often to execute the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startAfter" /> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval" /> is not positive.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleRecurring(Action callback, TimeSpan startAfter, TimeSpan interval);

        /// <summary>Schedules a state-accepting callback to execute once at a fixed time in the future.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="state">An object containing information to be used by the callback method.</param>
        /// <param name="startTime">When to execute the callback. Must be in the future.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="state" /> is <see langword="null" /> (use the stateless overload instead).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startTime" /> is in the past.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleOneTime(TimerCallback callback, object state, DateTimeOffset startTime);

        /// <summary>Schedules a state-accepting callback to execute once after a delay.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="state">An object containing information to be used by the callback method.</param>
        /// <param name="startAfter">The amount of time to delay before first executing the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="state" /> is <see langword="null" /> (use the stateless overload instead).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startAfter" /> is negative.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleOneTime(TimerCallback callback, object state, TimeSpan startAfter);

        /// <summary>Schedules an action to execute once at a fixed time in the future.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="startTime">When to execute the callback. Must be in the future.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startTime" /> is in the past.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleOneTime(Action callback, DateTimeOffset startTime);

        /// <summary>Schedules an action to execute once after a delay.</summary>
        /// <param name="callback">The callback to execute.</param>
        /// <param name="startAfter">The amount of time to delay before first executing the callback.</param>
        /// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startAfter" /> is negative.</exception>
        /// <returns>The <see cref="Timer" /> that was created to execute the callback.</returns>
        Timer ScheduleOneTime(Action callback, TimeSpan startAfter);
    }
}