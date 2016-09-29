Public Class SqrtElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("X")

        Inputs.Add("n")

        Outputs.Add("Y")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "SQRT"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Try
            Outputs(0).State = Math.Pow(Inputs(0).State, 1 / Inputs(1).State)
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Maths"
        End Get
    End Property
End Class
