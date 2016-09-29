Public Class ServoUI
    Public Event Run()
    Sub GoRun() Handles chkRun.Checked, chkRun.Unchecked
        RaiseEvent Run()
    End Sub
End Class
