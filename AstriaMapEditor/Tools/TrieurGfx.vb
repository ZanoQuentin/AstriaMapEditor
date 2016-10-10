Public Class TrieurGfx

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Me.AllowDrop = True
    End Sub

    Private Sub Me_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Me_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        For Each PassedFile In e.Data.GetData(DataFormats.FileDrop)
            Dim myfile As New IO.FileInfo(PassedFile)
            If myfile.Name = "GfxID utilisés.txt" Then
                KryptonRichTextBox1.Text &= IO.File.ReadAllText(myfile.FullName)
            Else
                MsgBox("Vous ne pouvez joindre que des fichiers ""GfxID utilisés.txt"".")
            End If
        Next
    End Sub

    Private Sub Conv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Conv.Click
        If KryptonRichTextBox1.Text = "" Then Exit Sub
        Dim txt As String() = KryptonRichTextBox1.Text.Split(vbLf)
        Dim nb As New List(Of Integer)
        For Each atxt As String In txt
            If Not atxt = "" AndAlso Not nb.Contains(atxt) AndAlso IsNumeric(atxt) Then
                nb.Add(CInt(atxt))
            End If
        Next
        nb.Sort()
        KryptonRichTextBox2.Text = ""
        For Each anb As Integer In nb
            KryptonRichTextBox2.Text &= anb & vbCrLf
        Next
    End Sub

End Class
