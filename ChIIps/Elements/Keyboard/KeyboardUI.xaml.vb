Partial Public Class KeyboardUI

    Public keyPressed As WindowsInput.VirtualKeyCode

    Private Sub Button1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Button1.KeyUp
        keyPressed = e.Key + 21

        Button1.Content = keyPressed.ToString
    End Sub
End Class
