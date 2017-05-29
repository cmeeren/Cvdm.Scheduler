namespace Cvdm.Scheduler.Helpers
{
    using System;

    /// <inheritdoc />
    internal sealed class TimeProvider : ITimeProvider
    {
        /// <inheritdoc />
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}