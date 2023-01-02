Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds
Imports Meadow.Peripherals.Leds

Public Class MeadowApp
    ' Change F7CoreComputeV2 to F7FeatherV2 (or F7FeatherV1) for Feather boards
    Inherits App(Of F7CoreComputeV2)

    Public Overrides Function Run() As Task
        Resolver.Log.Info("Run... (VB.NET)")

        Resolver.Log.Info("Hello, Meadow Core-Compute!")

        Return MyBase.Run()
    End Function

    Public Overrides Function Initialize() As Task
        Resolver.Log.Info("Initialize... (VB.NET)")

        Return MyBase.Run()
    End Function
End Class