Partial Public Class WiiMote

    Public Event Connect()
    Public Event C2()
    Public Event Disconnect()

    Private Sub cmdConnect_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdConnect.Click
        RaiseEvent Connect()
    End Sub

    Private Sub cmdDisconnect_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdDisconnect.Click
        RaiseEvent Disconnect()
    End Sub

    Private Sub cmdC2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles cmdC2.Click
        RaiseEvent C2()
    End Sub
End Class
