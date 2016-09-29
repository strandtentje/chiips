Imports System.Windows.Threading

Public Class IncElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("d")

        Inputs.Add("O")

        Inputs.Add("t")

        Outputs.Add("R")

    End Sub

    Public Value As Integer
    Public FO As Integer

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Increment"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        If FO <> Inputs(1).State Then
            FO = Inputs(1).State
            Value = FO
        End If

        If Inputs(2).State > 20 Then
            timAdd.Interval = New TimeSpan(0, 0, 0, 0, Inputs(2).State)
        End If

        If Not timAdd.IsEnabled Then timAdd.Start()
    End Sub

    Dim WithEvents timAdd As New DispatcherTimer With {.Interval = New TimeSpan(0, 0, 0, 0, 50)}

    Sub Add() Handles timAdd.Tick
        Value += Inputs(0).State
        Outputs(0).State = Value
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Maths"
        End Get
    End Property
End Class
