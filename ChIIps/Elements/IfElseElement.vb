Public Class IfElseElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("If")

        Inputs.Add("Then")

        Inputs.Add("Else")

        Outputs.Add("Out")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "IfElse"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Try
            If Inputs(0).State Then
                outputs(0).state = inputs(1).state
            Else
                outputs(0).state = inputs(2).state
            End If
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Conditioning"
        End Get
    End Property

End Class
