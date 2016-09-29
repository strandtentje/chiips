Imports WindowsInput
Public Class KeyboardClass
    Inherits Chip.ChIIpsSDK.Chip
    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property

    Shared InputSimulator As New InputSimulator

    Public Sub New()

        MyBase.New()

        Inputs.Add("En")
    End Sub

    Public Overrides Sub ProcessChanges()

        If Not MyUI.keyPressed = 0 Then
            If Inputs(0).State Then
                InputSimulator.Keyboard.KeyDown(MyUI.keyPressed)
            Else
                InputSimulator.Keyboard.KeyUp(MyUI.keyPressed)
            End If
        End If

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Keyboard"
        End Get
    End Property

    Dim MyUI As New KeyboardUI

    Public Overrides ReadOnly Property UI() As System.Windows.UIElement
        Get
            Return MyUI
        End Get
    End Property

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property
End Class
