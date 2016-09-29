Public Class CounterElement
    Inherits Chip.ChIIpsSDK.Chip

    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property

    Public Sub New()

        MyBase.New()

        Inputs.Add("clock")

        Inputs.Add("reset")

        For i As Integer = 0 To 9
            Outputs.Add(New Chip.ChIIpsSDK.Connection(i))
        Next

    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Outputs(Index).State = True
            Return "Teller"
        End Get
    End Property

    Dim Index As Integer

    Dim Sandboxmode As Boolean

    Public Overrides Sub ProcessChanges()
        If Not Sandboxmode Then
            Sandboxmode = True
            If (Inputs(0).State = True) Then
                Index += 1

                If Index = 10 Then
                    Outputs(9).State = False
                    Index = 0
                Else
                    Outputs(Index - 1).State = False
                End If


                Outputs(Index).State = True

            End If

            If (Inputs(1).State = True) Then
                Outputs(Index).State = False
                Index = 0

                Outputs(0).State = True
            End If
            Sandboxmode = False
        End If
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Chips"
        End Get
    End Property
End Class
