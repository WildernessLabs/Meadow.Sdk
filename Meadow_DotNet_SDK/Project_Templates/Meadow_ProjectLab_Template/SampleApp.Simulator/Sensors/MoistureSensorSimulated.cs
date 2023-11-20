using Meadow.Peripherals.Sensors.Moisture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Meadow.Foundation
{
    public class MoistureSensorSimulated : IMoistureSensor
    {
        protected List<IObserver<IChangeResult<double>>> observers = new();

        protected object samplingLock = new();

        protected CancellationTokenSource? SamplingTokenSource { get; set; }

        public double? Moisture { get; protected set; } = 20;

        public TimeSpan UpdateInterval { get; protected set; }

        public bool IsSampling { get; protected set; } = false;

        public event EventHandler<IChangeResult<double>> HumidityUpdated;

        public double? MinMoisture { get; protected set; }

        public double? MaxMoisture { get; protected set; }

        public MoistureSensorSimulated(double initialValue,
            double? minValue = null,
            double? maxValue = null)
        {
            Moisture = initialValue;

            MinMoisture = minValue;
            MaxMoisture = maxValue;
        }

        public void MeasureMoisture()
        {
            _ = Read();
        }

        public Task<double> Read()
        {
            var random = new Random();

            double value = random.Next((int)(Moisture.Value - 1), (int)(Moisture.Value + 1));

            if (MinMoisture is { } min && value < min)
            {
                value = min;
            }

            if (MaxMoisture is { } max && value > max)
            {
                value = max;
            }

            return Task.FromResult(value);
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

                double oldConditions;
                ChangeResult<double> result;

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
                        oldConditions = Moisture.Value;

                        Moisture = await Read();

                        result = new ChangeResult<double>(Moisture.Value, oldConditions);

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

        protected void RaiseEventsAndNotify(IChangeResult<double> changeResult)
        {
            HumidityUpdated?.Invoke(this, changeResult);

            NotifyObservers(changeResult);
        }

        protected void NotifyObservers(IChangeResult<double> changeResult)
        {
            observers.ForEach(x => x.OnNext(changeResult));
        }
    }
}