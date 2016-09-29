Imports System.IO.Ports
Imports System.Windows.Threading

Public Class Servo
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        tx = 1500
        ty = 1500
        tz = 1500

        Inputs.Add("X")
        Inputs.Add("Y")
        Inputs.Add("Z")
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Servo"
        End Get
    End Property

    Dim WithEvents _UI As ServoUI

    Public Overrides ReadOnly Property UI As System.Windows.UIElement
        Get
            If _UI Is Nothing Then _UI = New ServoUI
            Return _UI
        End Get
    End Property

    Dim Port As New SerialPort("COM5", 115200, IO.Ports.Parity.None, _
                               8, IO.Ports.StopBits.One)

    Dim WithEvents timMove As New DispatcherTimer() With {.Interval = New TimeSpan(0, 0, 0, 0, 50)}


    Sub RunCH() Handles _UI.Run
        timMove.IsEnabled = _UI.chkRun.IsChecked
    End Sub

    Dim px, py, pz As Integer
    Dim tx, ty, tz As Integer

    Sub Act() Handles timMove.Tick
        If Not Port.IsOpen Then Port.Open()

        px = 1500 + 9 * Inputs(0).State
        py = 1500 + 9 * Inputs(1).State
        pz = 1500 + 9 * Inputs(2).State

        tx = tx + (px - tx) / 2
        ty = ty + (py - ty) / 2
        tz = tz + (pz - tz) / 2

        Port.Write("#0 P" & tx & " T50")
        Port.Write("#1 P" & ty & " T50")
        Port.Write("#2 P" & tz & " T50")
        Port.Write(vbCr)
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property
End Class
