﻿using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using ___SafeProjectName___.Core;

namespace ___SafeProjectName___.F7;

public class MeadowF7FeatherApp : App<F7FeatherV2>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new ___SafeProjectName___F7FeatherHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}