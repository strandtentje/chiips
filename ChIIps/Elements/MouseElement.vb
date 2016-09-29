Imports System.Runtime.InteropServices

Public Class MouseAX
    Inherits Chip.ChIIpsSDK.Chip

    Private Declare Function SetCursorPos Lib "user32.dll" Alias "SetCursorPos" (ByVal X As Integer, ByVal Y As Integer) As Boolean
    Private Declare Sub mouse_event Lib "user32.dll" Alias "mouse_event" (ByVal dwFlags As UInteger, ByVal dx As Integer, ByVal dy As Integer, ByVal dwData As Integer, ByVal dwExtraInfo As Integer)

    Public Enum MouseEventFlags As Integer
        LEFTDOWN = &H2
        LEFTUP = &H4
        MIDDLEDOWN = &H20
        MIDDLEUP = &H40
        MOVE = &H1
        ABSOLUTE = &H8000
        RIGHTDOWN = &H8
        RIGHTUP = &H10
        MOUSEWHEEL = &H800
    End Enum

    ' ...
    ' //Set cursor position  
    ' SetCursorPos(10, 50);
    ' //Mouse Right Down and Mouse Right Up
    ' mouse_event((uint)MouseEventFlags.RIGHTDOWN,0,0,0,0);
    ' mouse_event((uint)MouseEventFlags.RIGHTUP,0,0,0,0); 

    Public Sub New()

        MyBase.New()

        Inputs.Add("AX")

        Inputs.Add("AY")

        Inputs.Add("L")

        Inputs.Add("R")

        Inputs.Add("MUP")
        Inputs.Add("MDN")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "MouseAX"
        End Get
    End Property

    Dim WithEvents timSendMouse As New Timers.Timer With {.AutoReset = 1, .Interval = 10}
    Dim blnL, blnR, blnP, blnM As Boolean
    Dim Keep As Integer

    Public Overrides Sub ProcessChanges()
        timSendMouse.Start()

        Keep = 0
    End Sub

    Sub SendMouse() Handles timSendMouse.Elapsed

        Keep += 1

        If Keep = 500 Then
            timSendMouse.Stop()
            Keep = 0
        End If

        SetCursorPos(Windows.Forms.Cursor.Position.X + Inputs(0).State, Windows.Forms.Cursor.Position.Y + Inputs(1).State)

        If blnL <> Inputs(2).State Then
            blnL = Inputs(2).State

            If blnL Then mouse_event(MouseEventFlags.LEFTDOWN, 0, 0, 0, 0)
            If Not blnL Then mouse_event(MouseEventFlags.LEFTUP, 0, 0, 0, 0)
        End If

        If blnR <> Inputs(3).State Then
            blnR = Inputs(3).State

            If blnR Then mouse_event(MouseEventFlags.RIGHTDOWN, 0, 0, 0, 0)
            If Not blnR Then mouse_event(MouseEventFlags.RIGHTUP, 0, 0, 0, 0)
        End If

        If Inputs(4).State <> blnM Then
            blnM = Inputs(4).State
            If blnM Then mouse_event(MouseEventFlags.MOUSEWHEEL, 0, 0, 120, 0)
        End If

        If Inputs(5).State <> blnP Then
            blnP = Inputs(5).State
            If blnP Then mouse_event(MouseEventFlags.MOUSEWHEEL, 0, 0, -120, 0)
        End If
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property

End Class
