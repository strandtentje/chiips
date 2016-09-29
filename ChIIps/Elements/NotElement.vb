Public Class NotElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("in")

        Outputs.Add("uit")

        ProcessChanges()

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Not"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()
        Outputs(0).State = Not Inputs(0).State
    End Sub


    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Boolean Logic"
        End Get
    End Property
End Class
