using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.Foundation
{
    public class TemperatureSensorSimulated : ITemperatureSensor
    {
        protected List<IObserver<IChangeResult<Temperature>>> observers = new();

        protected object samplingLock = new();

        protected CancellationTokenSource? SamplingTokenSource { get; set; }

        public Temperature? Temperature { get; protected set; } = new Temperature(20);

        public TimeSpan UpdateInterval { get; protected set; }

        public bool IsSampling { get; protected set; } = false;

        public event EventHandler<IChangeResult<Temperature>> TemperatureUpdated;

        public Temperature? MinTemperature { get; protected set; }

        public Temperature? MaxTemperature { get; protected set; }

        public TemperatureSensorSimulated(Temperature initialValue,
            Temperature? minValue = null,
            Temperature? maxValue = null)
        {
            Temperature = initialValue;

            MinTemperature = minValue;
            MaxTemperature = maxValue;
        }

        public void MeasureTemperature()
        {
            _ = Read();
        }

        public Task<Temperature> Read()
        {
            var random = new Random();

            var value = random.Next((int)(Temperature.Value.Celsius - 3), (int)(Temperature.Value.Celsius + 3));

            var temperature = new Temperature(value, Meadow.Units.Temperature.UnitType.Celsius);

            if (MinTemperature is { } min && temperature < min)
            {
                temperature = min;
            }

            if (MaxTemperature is { } max && temperature > max)
            {
                temperature = max;
            }

            return Task.FromResult(temperature);
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

                Temperature oldConditions;
                ChangeResult<Temperature> result;

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
                        oldConditions = Temperature.Value;

                        Temperature = await Read();

                        result = new ChangeResult<Temperature>(Temperature.Value, oldConditions);

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

        protected void RaiseEventsAndNotify(IChangeResult<Temperature> changeResult)
        {
            TemperatureUpdated?.Invoke(this, changeResult);

            NotifyObservers(changeResult);
        }

        protected void NotifyObservers(IChangeResult<Temperature> changeResult)
        {
            observers.ForEach(x => x.OnNext(changeResult));
        }
    }
}