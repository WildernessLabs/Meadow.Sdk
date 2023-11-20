using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.Foundation
{
    public class HumiditySensorSimulated : IHumiditySensor
    {
        protected List<IObserver<IChangeResult<RelativeHumidity>>> observers = new();

        protected object samplingLock = new();

        protected CancellationTokenSource? SamplingTokenSource { get; set; }

        public RelativeHumidity? Humidity { get; protected set; } = new RelativeHumidity(20);

        public TimeSpan UpdateInterval { get; protected set; }

        public bool IsSampling { get; protected set; } = false;

        public event EventHandler<IChangeResult<RelativeHumidity>> HumidityUpdated;

        public RelativeHumidity? MinHumidity { get; protected set; }

        public RelativeHumidity? MaxHumidity { get; protected set; }

        public HumiditySensorSimulated(RelativeHumidity initialValue,
            RelativeHumidity? minValue = null,
            RelativeHumidity? maxValue = null)
        {
            Humidity = initialValue;

            MinHumidity = minValue;
            MaxHumidity = maxValue;
        }

        public void MeasureHumidity()
        {
            _ = Read();
        }

        public Task<RelativeHumidity> Read()
        {
            var random = new Random();

            var value = random.Next((int)(Humidity.Value.Percent - 1), (int)(Humidity.Value.Percent + 1));

            var humidity = new RelativeHumidity(value, RelativeHumidity.UnitType.Percent);

            if (MinHumidity is { } min && humidity < min)
            {
                humidity = min;
            }

            if (MaxHumidity is { } max && humidity > max)
            {
                humidity = max;
            }

            return Task.FromResult(humidity);
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

                RelativeHumidity oldConditions;
                ChangeResult<RelativeHumidity> result;

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
                        oldConditions = Humidity.Value;

                        Humidity = await Read();

                        result = new ChangeResult<RelativeHumidity>(Humidity.Value, oldConditions);

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

        protected void RaiseEventsAndNotify(IChangeResult<RelativeHumidity> changeResult)
        {
            HumidityUpdated?.Invoke(this, changeResult);

            NotifyObservers(changeResult);
        }

        protected void NotifyObservers(IChangeResult<RelativeHumidity> changeResult)
        {
            observers.ForEach(x => x.OnNext(changeResult));
        }
    }
}