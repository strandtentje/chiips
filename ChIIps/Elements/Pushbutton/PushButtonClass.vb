Public Class PushButtonClass
    Inherits Chip.ChIIpsSDK.Chip

    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property


    Public Sub New()

        MyBase.New()

        Outputs.Add("uit")

        AddHandler MyUI.ellipse.MouseDown, AddressOf ButtonDown
        AddHandler MyUI.ellipse.MouseLeftButtonUp, AddressOf ButtonUp
    End Sub

    Private Sub ButtonDown(ByVal sender As Object, ByVal e As EventArgs)
        Outputs(0).State = True
    End Sub

    Private Sub ButtonUp(ByVal Sender As Object, ByVal e As EventArgs)
        Outputs(0).State = False
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Drukknop"
        End Get
    End Property

    Dim MyUI As New Pushbutton

    Public Overrides ReadOnly Property UI() As System.Windows.UIElement
        Get
            Return MyUI
        End Get
    End Property
    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Input"
        End Get
    End Property
End Class
