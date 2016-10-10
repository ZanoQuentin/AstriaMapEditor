Public Class InputSizeMap

    Public _Width As Integer = 10
    Public _Height As Integer = 10
    Public _X As Integer = 0
    Public _Y As Integer = 0

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton1.Click
        _Width = KryptonNumericUpDown1.Value
        _Height = KryptonNumericUpDown2.Value
        _X = KryptonNumericUpDown4.Value
        _Y = KryptonNumericUpDown3.Value
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class
