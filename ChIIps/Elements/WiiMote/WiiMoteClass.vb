Imports System.Windows.Threading

Public Class WiiMoteClass
    Inherits Chip.ChIIpsSDK.Chip

    Dim WithEvents MyUI As New WiiMote
    Dim WithEvents Refresher As New DispatcherTimer With {.Interval = New TimeSpan(0, 0, 0, 0, 50)}
    Dim Status As WiimoteLib.WiimoteState
    Dim Mote As New WiimoteLib.Wiimote

    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property

    Public Sub New()

        MyBase.New()

        Inputs.Add("L1")
        Inputs.Add("L2")
        Inputs.Add("L3")
        Inputs.Add("L4")

        Outputs.Add("aX")
        Outputs.Add("aY")
        Outputs.Add("aZ")
        Outputs.Add("nX")
        Outputs.Add("nY")
        Outputs.Add("nX")
        Outputs.Add("jX")
        Outputs.Add("jY")

        Outputs.Add("A")
        Outputs.Add("B")
        Outputs.Add("C")
        Outputs.Add("Z")

        Outputs.Add("L")    ' 12
        Outputs.Add("R")
        Outputs.Add("U")
        Outputs.Add("D")

        Outputs.Add("-")
        Outputs.Add("H")
        Outputs.Add("+")

        Outputs.Add("1")
        Outputs.Add("2")
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Wiimote"
        End Get
    End Property

    Sub LoadMote() Handles MyUI.Connect
        Dim WML As New WiimoteLib.WiimoteCollection
        WML.FindAllWiimotes()

        Mote = WML(0)
        LMM()
    End Sub

    Sub LM2() Handles MyUI.C2
        Dim WML As New WiimoteLib.WiimoteCollection
        WML.FindAllWiimotes()

        Mote = WML(1)
        LMM()
    End Sub

    Sub LMM()
        Mote.Connect()

        If Mote.WiimoteState.ExtensionType = WiimoteLib.ExtensionType.BalanceBoard Then ThisIsBoard = True
        If Not ThisIsBoard Then Mote.SetReportType(WiimoteLib.InputReport.ExtensionAccel, True)

        If ThisIsBoard Then MyUI.lblBoard.Visibility = Visibility.Visible

        Refresher.Start()

        AddHandler Mote.WiimoteChanged, AddressOf Change
    End Sub

    Sub UnloadMote() Handles MyUI.Disconnect
        Mote.Disconnect()
    End Sub

    Sub Change(ByVal Sender As Object, ByVal Arg As WiimoteLib.WiimoteChangedEventArgs)
        Status = Arg.WiimoteState
    End Sub

    Public Overrides ReadOnly Property UI() As System.Windows.UIElement
        Get
            Return MyUI
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Mote.SetLEDs(Inputs(0).State, _
                     Inputs(1).State, _
                     Inputs(2).State, _
                     Inputs(3).State)
    End Sub

    Dim ThisIsBoard As Boolean

    Sub Passval() Handles Refresher.Tick
        If ThisIsBoard Then
            With Status.BalanceBoardState.SensorValuesKg
                Outputs(0).State = .BottomLeft * 10
                Outputs(1).State = .BottomRight * 10
                Outputs(2).State = .TopLeft * 10
                Outputs(3).State = .TopRight * 10
            End With
        Else
            With Status.AccelState.Values
                Outputs(0).State = Int(.X * 10000) / 100
                Outputs(1).State = Int(.Y * 10000) / 100
                Outputs(2).State = Int(.Z * 10000) / 100
            End With

            With Status.NunchukState.AccelState.Values
                Outputs(3).State = Int(.X * 100)
                Outputs(4).State = Int(.Y * 100)
                Outputs(5).State = Int(.Z * 100)
            End With

            With Status.NunchukState.Joystick
                Outputs(6).State = Int(.X * 100)
                Outputs(7).State = Int(.Y * 100)
            End With

            With Status.ButtonState
                Outputs(8).State = .A
                Outputs(9).State = .B

                Outputs(12).State = .Left
                Outputs(13).State = .Right
                Outputs(14).State = .Up
                Outputs(15).State = .Down

                Outputs(16).State = .Minus
                Outputs(17).State = .Home
                Outputs(18).State = .Plus

                Outputs(19).State = .One
                Outputs(20).State = .Two
            End With

            With Status.NunchukState
                Outputs(10).State = .C
                Outputs(11).State = .Z
            End With
        End If



    End Sub
    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Input"
        End Get
    End Property
End Class
