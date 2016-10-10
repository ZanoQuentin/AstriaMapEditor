Imports System.IO

Public Class MusicManager

    Dim MyParent As MapEditor

    Public Sub New(ByRef _Parent As MapEditor)
        InitializeComponent()
        Me.TopMost = True
        MyParent = _Parent

        ' Affichage des musiques
        Dim InfosDirectory1 As New DirectoryInfo(Main.DirectoryApply & "\Musiques")
        For Each FileName As FileInfo In InfosDirectory1.GetFiles
            KryptonListBox1.Items.Add(FileName.Name.Split(".")(0))
        Next

        KryptonListBox1.Refresh()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Valide.Click
        MyParent.MyDatas.Musique = KryptonListBox1.SelectedItem.Split("-")(0)
        MyParent.MyDatas.MusiqueName = KryptonListBox1.SelectedItem
        Main.MenuMap_RefreshControls()
        Me.Dispose()
    End Sub

End Class