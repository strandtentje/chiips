Public Class CompareElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("A")

        Inputs.Add("B")

        Outputs.Add("UIT")

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "A > B"
        End Get
    End Property

    Public Overrides Sub ProcessChanges()

        If Inputs(0).State > Inputs(1).State Then
            Outputs(0).State = True
        Else
            Outputs(0).State = False
        End If

    End Sub


    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Maths Logic"
        End Get
    End Property
End Class
