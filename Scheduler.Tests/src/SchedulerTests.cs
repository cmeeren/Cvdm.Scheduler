namespace Cvdm.Scheduler.Tests
{
    using System;
    using System.Threading;

    using Cvdm.Scheduler.Helpers;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class SchedulerTests
    {
        private Mock<ITimerFactory> timerFactory;
        private Mock<ITimeProvider> timeProvider;

        private Scheduler scheduler;

        [SetUp]
        public void SetUp()
        {
            this.timerFactory = new Mock<ITimerFactory>();
            this.timeProvider = new Mock<ITimeProvider>();

            this.scheduler = new Scheduler(
                this.timerFactory.Object,
                this.timeProvider.Object);
        }

        [Test]
        public void ScheduleRecurringWithStateAndStartTime_Should_CreateCorrectTimer()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            var callback = new TimerCallback(_ => { });
            var state = new object();
            var startTime = new DateTimeOffset(2001, 2, 3, 4, 5, 6, TimeSpan.Zero);
            var interval = new TimeSpan(1, 2, 3);

            // Act
            this.scheduler.ScheduleRecurring(callback, state, startTime, interval);

            // Assert
            this.timerFactory.Verify(_ => _.Create(callback, state, startTime - currentTime, interval));
        }

        [Test]
        public void ScheduleRecurringWithStateAndDelay_Should_CreateCorrectTimer()
        {
            // Arrange
            var callback = new TimerCallback(_ => { });
            var state = new object();
            var startAfter = new TimeSpan(2, 3, 4);
            var interval = new TimeSpan(1, 2, 3);

            // Act
            this.scheduler.ScheduleRecurring(callback, state, startAfter, interval);

            // Assert
            this.timerFactory.Verify(_ => _.Create(callback, state, startAfter, interval));
        }

        [Test]
        public void ScheduleRecurringWithStateAndStartTime_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(null, new object(), DateTimeOffset.Now.AddDays(1), TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithStateAndDelay_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(null, new object(), TimeSpan.Zero, TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithStateAndStartTime_When_StateIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(_ => { }, null, DateTimeOffset.Now.AddDays(1), TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithStateAndDelay_When_StateIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(_ => { }, null, TimeSpan.Zero, TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithStateAndStartTime_When_StartTimeIsPast_Should_Throw()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            DateTimeOffset startTime = currentTime.AddSeconds(-1);

            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(_ => { }, new object(), startTime, TimeSpan.FromDays(1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithStateAndDelay_When_DelayIsNegative_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(
                    _ => { },
                    new object(),
                    TimeSpan.FromMilliseconds(-1),
                    TimeSpan.FromDays(1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithStateAndStartTime_When_IntervalIsNotPositive_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(
                    _ => { },
                    new object(),
                    DateTimeOffset.Now.AddDays(1),
                    TimeSpan.Zero),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithStateAndDelay_When_IntervalIsNotPositive_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(_ => { }, new object(), TimeSpan.Zero, TimeSpan.Zero),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndStartTime_Should_CreateCorrectTimer()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            var actionWasCalled = false;
            var callback = new Action(() => actionWasCalled = true);
            var startTime = new DateTimeOffset(2001, 2, 3, 4, 5, 6, TimeSpan.Zero);
            var interval = new TimeSpan(1, 2, 3);

            this.timerFactory.Setup(_ => _.Create(It.IsAny<TimerCallback>(), null, startTime - currentTime, interval))
                .Callback((TimerCallback cb, object _, TimeSpan __, TimeSpan ___) => cb(null));

            // Act
            this.scheduler.ScheduleRecurring(callback, startTime, interval);

            // Assert
            this.timerFactory.Verify(_ => _.Create(It.IsAny<TimerCallback>(), null, startTime - currentTime, interval));
            Assert.That(actionWasCalled);
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithDelay_Should_CreateCorrectTimer()
        {
            // Arrange
            var actionWasCalled = false;
            var callback = new Action(() => actionWasCalled = true);
            var startAfter = new TimeSpan(2, 3, 4);
            var interval = new TimeSpan(1, 2, 3);

            this.timerFactory.Setup(_ => _.Create(It.IsAny<TimerCallback>(), null, startAfter, interval))
                .Callback((TimerCallback cb, object _, TimeSpan __, TimeSpan ___) => cb(null));

            // Act
            this.scheduler.ScheduleRecurring(callback, startAfter, interval);

            // Assert
            this.timerFactory.Verify(_ => _.Create(It.IsAny<TimerCallback>(), null, startAfter, interval));
            Assert.That(actionWasCalled);
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithStartTime_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(null, DateTimeOffset.Now.AddDays(1), TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithDelay_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(null, TimeSpan.Zero, TimeSpan.FromDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithStartTime_When_StartTimeIsPast_Should_Throw()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            DateTimeOffset startTime = currentTime.AddSeconds(-1);

            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(() => { }, startTime, TimeSpan.FromDays(1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithStartTime_When_DelayIsNegative_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(
                    () => { },
                    TimeSpan.FromMilliseconds(-1),
                    TimeSpan.FromDays(1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithStartTime_When_IntervalIsNotPositive_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(
                    () => { },
                    DateTimeOffset.Now.AddDays(1),
                    TimeSpan.Zero),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleRecurringWithoutStateAndWithDelay_When_IntervalIsNotPositive_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleRecurring(
                    () => { },
                    TimeSpan.Zero,
                    TimeSpan.Zero),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleOneTimeWithStateAndStartTime_Should_CreateCorrectTimer()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            var callback = new TimerCallback(_ => { });
            var state = new object();
            var startTime = new DateTimeOffset(2001, 2, 3, 4, 5, 6, TimeSpan.Zero);

            // Act
            this.scheduler.ScheduleOneTime(callback, state, startTime);

            // Assert
            this.timerFactory.Verify(_ => _.Create(callback, state, startTime - currentTime, TimeSpan.FromMilliseconds(-1)));
        }

        [Test]
        public void ScheduleOneTimeWithStateAndDelay_Should_CreateCorrectTimer()
        {
            // Arrange
            var callback = new TimerCallback(_ => { });
            var state = new object();
            var startAfter = new TimeSpan(1, 2, 3);

            // Act
            this.scheduler.ScheduleOneTime(callback, state, startAfter);

            // Assert
            this.timerFactory.Verify(_ => _.Create(callback, state, startAfter, TimeSpan.FromMilliseconds(-1)));
        }

        [Test]
        public void ScheduleOneTimeWithStateAndStartTime_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(null, new object(), DateTimeOffset.Now.AddDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithStateAndDelay_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(null, new object(), TimeSpan.Zero),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithStateAndStartTime_When_StateIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(_ => { }, null, DateTimeOffset.Now.AddDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithStateAndDelay_When_StateIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(_ => { }, null, TimeSpan.Zero),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithStateAndStartTime_When_StartTimeIsPast_Should_Throw()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            DateTimeOffset startTime = currentTime.AddSeconds(-1);

            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(_ => { }, new object(), startTime),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleOneTimeWithStateAndDelay_When_DelayIsNegative_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(
                    _ => { },
                    new object(),
                    TimeSpan.FromMilliseconds(-1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithStartTime_Should_CreateCorrectTimer()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            var actionWasCalled = false;
            var callback = new Action(() => actionWasCalled = true);
            var startTime = new DateTimeOffset(2001, 2, 3, 4, 5, 6, TimeSpan.Zero);

            this.timerFactory.Setup(
                    _ => _.Create(It.IsAny<TimerCallback>(), null, startTime - currentTime, TimeSpan.FromMilliseconds(-1)))
                .Callback((TimerCallback cb, object _, TimeSpan __, TimeSpan ___) => cb(null));

            // Act
            this.scheduler.ScheduleOneTime(callback, startTime);

            // Assert
            this.timerFactory.Verify(
                _ => _.Create(It.IsAny<TimerCallback>(), null, startTime - currentTime, TimeSpan.FromMilliseconds(-1)));
            Assert.That(actionWasCalled);
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithDelay_Should_CreateCorrectTimer()
        {
            // Arrange
            var actionWasCalled = false;
            var callback = new Action(() => actionWasCalled = true);
            var startAfter = new TimeSpan(1, 2, 3);

            this.timerFactory.Setup(_ => _.Create(It.IsAny<TimerCallback>(), null, startAfter, TimeSpan.FromMilliseconds(-1)))
                .Callback((TimerCallback cb, object _, TimeSpan __, TimeSpan ___) => cb(null));

            // Act
            this.scheduler.ScheduleOneTime(callback, startAfter);

            // Assert
            this.timerFactory.Verify(_ => _.Create(It.IsAny<TimerCallback>(), null, startAfter, TimeSpan.FromMilliseconds(-1)));
            Assert.That(actionWasCalled);
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithStartTime_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(null, DateTimeOffset.Now.AddDays(1)),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithDelay_When_CallbackIsNull_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(null, TimeSpan.Zero),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithStartTime_When_StartTimeIsPast_Should_Throw()
        {
            // Arrange
            var currentTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            this.timeProvider.Setup(_ => _.Now).Returns(currentTime);

            DateTimeOffset startTime = currentTime.AddSeconds(-1);

            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(() => { }, startTime),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ScheduleOneTimeWithoutStateAndWithDelay_When_DelayIsNegative_Should_Throw()
        {
            // Act/Assert
            Assert.That(
                () => this.scheduler.ScheduleOneTime(
                    () => { },
                    TimeSpan.FromMilliseconds(-1)),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Test_Default_Constructor()
        {
            // Arrange
            var actionWasRaised = true;

            // Act
            var realScheduler = new Scheduler();
            realScheduler.ScheduleOneTime(() => actionWasRaised = true, TimeSpan.Zero);

            // Assert
            Assert.That(actionWasRaised, Is.True.After(1000, 10));
        }
    }
}