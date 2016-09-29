Public Class Motor
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("Speed")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Motor"
        End Get
    End Property

    Dim WithEvents _UI As MotorUI

    Public Overrides ReadOnly Property UI As System.Windows.UIElement
        Get
            If _UI Is Nothing Then _UI = New MotorUI
            Return _UI
        End Get
    End Property

    Dim LPTStream As IO.FileStream

    Declare Sub Output Lib "inpout32.dll" Alias "Out32" (ByVal address As Integer, ByVal value As Integer)

    Sub Emergency() Handles _UI.Emergency
        Output(888, 0)
    End Sub

    Dim Sp, Dir As Double
    Dim Cnv As Byte

    Public Overrides Sub ProcessChanges()

        Sp = Inputs(0).State

        If Sp >= 100 Then
            Sp = 100
        End If

        If Sp > 0 Then Dir = 0
        If Sp < 0 Then Dir = 64

        Cnv = (Math.Abs(Sp) / 100) * 63 + Dir

        Output(888, Cnv)

    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property
End Class
