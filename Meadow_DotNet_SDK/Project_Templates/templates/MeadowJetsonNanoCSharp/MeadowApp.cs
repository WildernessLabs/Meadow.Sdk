﻿using Meadow;
using Meadow.Pinouts;
using System.Threading.Tasks;

namespace MeadowApplication.Template
{
    public class MeadowApp : App<Linux<JetsonNano>>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Jetson Nano!");

            return Task.CompletedTask;
        }

        static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}