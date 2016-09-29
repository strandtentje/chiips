Imports System.Windows.Forms

Public Class TimerClass
    Inherits Chip.ChIIpsSDK.Chip
    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property


    Public Sub New()

        MyBase.New()

        Outputs.Add("uit")
        Inputs.Add("aan")

        AddHandler timInterval.Tick, AddressOf Tick
        AddHandler MyUI.sldInterval.ValueChanged, AddressOf SliderMove

        timInterval.Interval = MyUI.sldInterval.Value
    End Sub

    Private Sub SliderMove(ByVal sender As Object, ByVal e As EventArgs)
        timInterval.Interval = MyUI.sldInterval.Value
        MyUI.lblStatus.Content = "Interval: " & timInterval.Interval & " ms"
    End Sub

    Dim timInterval As New Timer

    Private Sub Tick()

        Outputs(0).State = Not Outputs(0).State

    End Sub

    Public Overrides Sub ProcessChanges()

        timInterval.Enabled = Inputs(0).State

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timer"
        End Get
    End Property

    Dim MyUI As New TimerUI

    Public Overrides ReadOnly Property UI() As System.Windows.UIElement
        Get
            Return MyUI
        End Get
    End Property

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Chips"
        End Get
    End Property
End Class
