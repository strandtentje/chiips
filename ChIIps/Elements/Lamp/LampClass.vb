Public Class LampClass
    Inherits Chip.ChIIpsSDK.Chip
    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property


    Public Sub New()

        MyBase.New()

        Inputs.Add("in")
    End Sub

    Public Overrides Sub ProcessChanges()

        If Inputs(0).State = True Then

            MyUI.BeginStoryboard(MyUI.Resources("On"))

        Else

            MyUI.BeginStoryboard(MyUI.Resources("Off"))

        End If

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Lamp"
        End Get
    End Property

    Dim MyUI As New LampUI

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
