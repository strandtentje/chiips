Public Class AddElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("A")

        Inputs.Add("B")

        Outputs.Add("C")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Add"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Try
            Outputs(0).State = Inputs(0).State + Inputs(1).State
        Catch : End Try
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Maths"
        End Get
    End Property

End Class
