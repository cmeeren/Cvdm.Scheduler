namespace Cvdm.Scheduler.Helpers
{
    using System;

    /// <summary>Provides time.</summary>
    internal interface ITimeProvider
    {
        /// <summary>Gets the current local time.</summary>
        DateTimeOffset Now { get; }
    }
}