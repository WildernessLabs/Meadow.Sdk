
Option Explicit On
Option Strict On

Imports System
Imports System.Threading


Public Class Program
	
	Public Shared Sub Main()
		Console.WriteLine("Hello very modern world!")

		Dim app as New MeadowApp

		Thread.Sleep(Timeout.Infinite)
	End Sub
End Class
