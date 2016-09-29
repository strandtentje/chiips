Public Class ValIn
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("X")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "ValueDisplay"
        End Get
    End Property

    Dim _UI As ValInUI

    Public Overrides ReadOnly Property UI As System.Windows.UIElement
        Get
            If _UI Is Nothing Then _UI = New ValInUI
            _UI.txtVal.IsReadOnly = True
            Return _UI
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        _UI.txtVal.Text = Inputs(0).State
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property
End Class
