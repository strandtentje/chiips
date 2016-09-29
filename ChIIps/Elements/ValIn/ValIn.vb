Public Class ValOut
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Outputs.Add("X")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "ValueInput"
        End Get
    End Property

    Dim WithEvents _UI As ValInUI

    Public Overrides ReadOnly Property UI As System.Windows.UIElement
        Get
            If _UI Is Nothing Then _UI = New ValInUI
            Return _UI
        End Get
    End Property

    Sub Change() Handles _UI.Changed
        Try
            Outputs(0).State = Val(_UI.txtVal.Text)
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Input"
        End Get
    End Property
End Class
