Imports System.Threading.Tasks
Imports Meadow
Imports Meadow.Devices

Public Class MeadowApp
    Inherits App(Of F7CoreComputeV2)

    Public Overrides Function Initialize() As Task
        Resolver.Log.Info("Initialize...")

        Return Task.CompletedTask
    End Function

    Public Overrides Function Run() As Task
        Resolver.Log.Info("Run...")

        Resolver.Log.Info("Hello, Meadow Core-Compute!")

        Return Task.CompletedTask
    End Function
End Class