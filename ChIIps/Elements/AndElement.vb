Public Class AndElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("in 1")

        Inputs.Add("in 2")

        Outputs.Add("uit")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "And"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()

        If Inputs(0).State And Inputs(1).State Then
            Outputs(0).State = True
        Else
            Outputs(0).State = False
        End If

    End Sub


    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Boolean Logic"
        End Get
    End Property
End Class
