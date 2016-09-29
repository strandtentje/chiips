Public Class MotorUI
    Public Event Emergency()

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdEmergency.Click
        RaiseEvent Emergency()
    End Sub
End Class
