Public Class Manuel

    Private Sub Manuel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WebBrowser1.ScriptErrorsSuppressed = True
        WebBrowser1.Navigate("http://ame.astria-serv.com/index.php?manuel")
        Me.TopMost = True
    End Sub

End Class