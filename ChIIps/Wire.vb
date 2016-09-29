Public Class Wire

    Public Sub New(ByVal newSource As connectionUI, ByVal newTarget As connectionUI, ByVal newLine As Line, ByVal newParentGrid As Grid)

        Source = newSource
        Target = newTarget
        Line = newLine
        Parentgrid = newParentGrid

        AddHandler Source.Connection.Change, AddressOf Update

        AddHandler Line.MouseMove, AddressOf Remove

        Active = True

        Line.Stroke = New SolidColorBrush(Colors.Red)

        Update(Source.Connection)

    End Sub

    Public Source As connectionUI
    Public Target As connectionUI
    Public Line As Line
    Dim Parentgrid As Grid
    Public Active As Boolean

    Private Sub Update(ByVal Pin As Chip.ChIIpsSDK.Connection)
        If Active Then
            Target.Connection.State = Pin.State

            If Pin.State = True Then
                Line.Stroke = New SolidColorBrush(Colors.Green)
            Else
                Line.Stroke = New SolidColorBrush(Colors.Red)
            End If
        End If
    End Sub

    Public Event WireRemoved(ByVal Wire As Wire)

    Public Sub RemoveIfYours(ByVal Element As Chip.ChIIpsSDK.Chip)

        If Element.Outputs.Contains(Source.Connection) Or Element.Inputs.Contains(Target.Connection) Then

            RaiseEvent WireRemoved(Me)

            REM Parentgrid.Children.Remove(Line)
            REM Active = False

        End If

    End Sub

    Private Sub Remove(ByVal Sender As Object, ByVal e As MouseEventArgs)
        If e.RightButton Then

            RaiseEvent WireRemoved(Me)

            REM Parentgrid.Children.Remove(Line)
            REM Active = False
        End If
    End Sub

    Public Sub AdjustPos()
        If Active Then
            Dim tmpLine As Line = Line
            Dim GenTrans As GeneralTransform = Source.ellipse.TransformToAncestor(Parentgrid)
            Dim spoint As Point = GenTrans.Transform(New Point(10, 10))

            GenTrans = Target.ellipse.TransformToAncestor(Parentgrid)
            Dim tpoint As Point = GenTrans.Transform(New Point(10, 10))

            tmpLine.X1 = spoint.X
            tmpLine.X2 = tpoint.X
            tmpLine.Y1 = spoint.Y
            tmpLine.Y2 = tpoint.Y
        End If

    End Sub

End Class
