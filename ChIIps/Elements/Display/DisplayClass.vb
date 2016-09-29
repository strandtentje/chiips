Public Class DisplayClass
    Inherits Chip.ChIIpsSDK.Chip

    Public Sub New()

        MyBase.New()

        Inputs.Add("tick")
        Inputs.Add("reset")

        Outputs.Add("3")
        Outputs.Add("4")
        Outputs.Add("6")
        Outputs.Add("rst")
        Outputs.Add("2")
    End Sub
    Public Overrides ReadOnly Property Bool As Boolean
        Get
            Return MyBase.Bool
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Tellerdisplay"
        End Get
    End Property

    Dim MyUI As New DisplayUI

    Public Overrides ReadOnly Property UI() As System.Windows.UIElement
        Get
            Return MyUI
        End Get
    End Property

    Dim Index As Integer

    Dim Sandboxmode As Boolean

    Dim Cijfers As String() = {"1111110", "0110000", "1101101", "1111001", "0110011", "1011011", "1011111", "1110000", "1111111", "1111011"}
    Dim strCur As String
    Dim segCur As Rectangle

    Public Overrides Sub ProcessChanges()
        If Not Sandboxmode Then
            Sandboxmode = True
            OnlyOutput(-1)

            If (Inputs(0).State = True) Then
                Index += 1

                If Index = 10 Then
                    Index = 0
                    OnlyOutput(3)
                End If
            End If



            If (Inputs(1).State = True) Then
                Index = 0
                OnlyOutput(-1)
            Else
                If Index = 3 Then OnlyOutput(0)
                If Index = 4 Then OnlyOutput(1)
                If Index = 6 Then OnlyOutput(2)
                If Index = 2 Then OnlyOutput(4)
            End If

            strCur = Cijfers(Index)

            For i As Integer = 0 To 6
                segCur = MyUI.segments.Children(i)
                If strCur(i) = "0" Then segCur.Fill = New SolidColorBrush(Colors.White)
                If strCur(i) = "1" Then segCur.Fill = New SolidColorBrush(Colors.Black)
            Next

            Sandboxmode = False
        End If
    End Sub

    Private Sub OnlyOutput(ByVal index As Integer)
        For i As Integer = 0 To 4
            If i <> index Then Outputs(i).State = False
            If i = index Then Outputs(i).State = True
        Next
    End Sub

    Public Overrides ReadOnly Property Classification() As String
        Get
            Return "Output"
        End Get
    End Property
End Class
