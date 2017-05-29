# Cvdm.Scheduler
Schedules execution of a C# callback using `System.Threading.Timer`. Compatible with .NET Standard ≥1.2.

The library consists of an interface (`IScheduler`) and an implementation (`Scheduler`). The interface makes the scheduler easy to use with dependency injection. The implementation keeps internal references to all created timers, so you don't have to do that yourself if the scheduler lives at least as long as the timer (e.g. if the scheduler is a singleton component).

The following methods/overloads are provided:

### Schedule recurring execution of code

```c#
// State-accepting callback, starting at a fixed time
Timer ScheduleRecurring(TimerCallback callback, object state, DateTimeOffset startTime, TimeSpan interval);

// State-accepting callback, starting after a delay
Timer ScheduleRecurring(TimerCallback callback, object state, TimeSpan startAfter, TimeSpan interval);

// Simple action, starting at a fixed time
Timer ScheduleRecurring(Action callback, DateTimeOffset startTime, TimeSpan interval);

// Simple action, starting after a delay
Timer ScheduleRecurring(Action callback, TimeSpan startAfter, TimeSpan interval);
```

### Schedule one-time execution of code

```c#
// State-accepting callback, invoked at a fixed time
Timer ScheduleOneTime(TimerCallback callback, object state, DateTimeOffset startTime)
  
// State-accepting callback, invoked after a delay
Timer ScheduleOneTime(TimerCallback callback, object state, TimeSpan startAfter)

// Simple action, invoked at a fixed time
Timer ScheduleOneTime(Action callback, DateTimeOffset startTime)

// Simple action, invoked after a delay
Timer ScheduleOneTime(Action callback, TimeSpan startAfter)
```

