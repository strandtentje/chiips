Public Class IfElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("If")

        Inputs.Add("Then")

        Outputs.Add("Out")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "If"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Try
            If Inputs(0).State Then
                Outputs(0).State = Inputs(1).State
            End If
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Conditioning"
        End Get
    End Property

End Class
