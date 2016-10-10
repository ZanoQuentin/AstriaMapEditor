Public Class BackgroundManager

    Dim MyParent As MapEditor

    Public Sub New(ByRef _Parent As MapEditor)
        InitializeComponent()
        Me.TopMost = True
        MyParent = _Parent

        ' Affichage des backgrounds
        Dim imageListLarge As New ImageList()
        imageListLarge.ImageSize = New Size(166, 100)
        imageListLarge.ColorDepth = ColorDepth.Depth32Bit

        Dim i As Integer = 0
        For Each ATile As Tile In Main.List_Backgrounds
            If IsNothing(ATile) Then Continue For
            ListView1.Items.Add(ATile.ID.ToString, i)
            imageListLarge.Images.Add(ATile.Image())
            i += 1
        Next
        ListView1.LargeImageList = imageListLarge
        ListView1.Refresh()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick, BT_Valide.Click
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        MyParent.DrawBackground(Main.List_Backgrounds(ListView1.SelectedItems(0).Text))
        ListView1.LargeImageList.Dispose()
        ListView1.Dispose()
        Me.Dispose()
    End Sub

End Class