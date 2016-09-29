Public Class DivElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("t")

        Inputs.Add("n")

        Outputs.Add("X")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Div"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Try
            Outputs(0).State = Inputs(0).State / Inputs(1).State
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Maths"
        End Get
    End Property
End Class
