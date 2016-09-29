Public Class ValInUI
    Public Event Changed()

    Private Sub txtVal_TextChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles txtVal.TextChanged
        RaiseEvent Changed()
    End Sub
End Class
