using Meadow.Peripherals.Sensors.Distance;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.Foundation
{
    public class DistanceSensorSimulated : IRangeFinder
    {
        protected List<IObserver<IChangeResult<Length>>> observers = new();

        protected object samplingLock = new();

        protected CancellationTokenSource? SamplingTokenSource { get; set; }

        public Length? Distance { get; protected set; } = new Length(0);

        public TimeSpan UpdateInterval { get; protected set; }

        public bool IsSampling { get; protected set; } = false;

        public event EventHandler<IChangeResult<Length>> DistanceUpdated;

        public Length? MinLength { get; protected set; }

        public Length? MaxLength { get; protected set; }

        public DistanceSensorSimulated(Length initialLength, Length? minLength = null, Length? maxLength = null)
        {
            Distance = initialLength;

            MinLength = minLength;
            MaxLength = maxLength;
        }

        public void MeasureDistance()
        {
            _ = Read();
        }

        public Task<Length> Read()
        {
            var random = new Random();

            var value = random.Next((int)(Distance.Value.Centimeters - 3), (int)(Distance.Value.Centimeters + 3));

            var distance = new Length(value, Length.UnitType.Centimeters);

            if (MinLength is { } min && distance < min)
            {
                distance = min;
            }

            if (MaxLength is { } max && distance > max)
            {
                distance = max;
            }

            return Task.FromResult(distance);
        }

        public void StartUpdating(TimeSpan? updateInterval = null)
        {
            lock (samplingLock)
            {
                if (IsSampling) { return; }

                IsSampling = true;

                // if an update interval has been passed in, override the default
                if (updateInterval is { } ui) { UpdateInterval = ui; }

                SamplingTokenSource = new CancellationTokenSource();
                CancellationToken ct = SamplingTokenSource.Token;

                Length oldConditions;
                ChangeResult<Length> result;

                var t = new Task(async () =>
                {
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            observers.ForEach(x => x.OnCompleted());
                            IsSampling = false;
                            break;
                        }
                        oldConditions = Distance.Value;

                        Distance = await Read();

                        result = new ChangeResult<Length>(Distance.Value, oldConditions);

                        RaiseEventsAndNotify(result);

                        await Task.Delay(UpdateInterval);
                    }
                }, SamplingTokenSource.Token, TaskCreationOptions.LongRunning);
                t.Start();
            }
        }

        public void StopUpdating()
        {
            lock (samplingLock)
            {
                if (!IsSampling) { return; }

                SamplingTokenSource?.Cancel();

                IsSampling = false;
            }
        }

        protected void RaiseEventsAndNotify(IChangeResult<Length> changeResult)
        {
            DistanceUpdated?.Invoke(this, changeResult);

            NotifyObservers(changeResult);
        }

        protected void NotifyObservers(IChangeResult<Length> changeResult)
        {
            observers.ForEach(x => x.OnNext(changeResult));
        }
    }
}