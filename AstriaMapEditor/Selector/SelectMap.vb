Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing.Image

Public Class SelectMap

    Public SelectedMap As Map

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub SelectMap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TopMost = True
        LoadMaps()
    End Sub

    Private Sub LoadMaps()
        ListView1.Clear()
        ListView1.LargeImageList = Nothing
        ' Chargement des maps
        Dim imageListLarge As New ImageList()
        imageListLarge.ImageSize = New Size(133, 100)
        imageListLarge.ColorDepth = ColorDepth.Depth32Bit

        Dim i As Integer = 0
        For Each aMap As Map In Map.ListOfMaps
            If IsNothing(aMap) Then
                Map.ListOfMaps.Remove(aMap)
                Continue For
            End If
            If aMap.IsEditing Then Continue For
            Dim item As New ListViewItem(aMap.ID.ToString, i)
            item.Tag = aMap
            ListView1.Items.Add(item)

            If Not IsNothing(aMap.Screenshot) Then
                Dim callback As GetThumbnailImageAbort = New Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
                Dim Thumb As Image = aMap.Screenshot.GetThumbnailImage(imageListLarge.ImageSize.Width, imageListLarge.ImageSize.Height, callback, IntPtr.Zero)
                imageListLarge.Images.Add(Thumb)
            Else
                imageListLarge.Images.Add(New Bitmap(imageListLarge.ImageSize.Width, imageListLarge.ImageSize.Height))
            End If

            i += 1
        Next
        ListView1.LargeImageList = imageListLarge
        ListView1.Refresh()
    End Sub

    Public Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick, KryptonButton1.Click
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        SelectedMap = DirectCast(ListView1.SelectedItems(0).Tag, Map)
        ListView1.LargeImageList.Dispose()
        ListView1.Dispose()
        Me.Close()
    End Sub

#Region " ContextMenu "
    Private Sub ImporterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImporterToolStripMenuItem.Click
        Me.Visible = False
        Main.ImporterToolStripMenuItem_Click(sender, e)
        Me.Dispose()
    End Sub

    Private Sub SupprimerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupprimerToolStripMenuItem.Click
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        SelectedMap = DirectCast(ListView1.SelectedItems(0).Tag, Map)
        If Directory.Exists(Main.DirectoryApply & "\Maps\" & SelectedMap.ID) Then
            IO.Directory.Delete(Main.DirectoryApply & "\Maps\" & SelectedMap.ID, True)
            Map.ListOfMaps.Remove(SelectedMap)
        Else
            Map.ListOfMaps.Remove(SelectedMap)
        End If
        LoadMaps()
    End Sub

    Private Sub OuvrirDansWindowsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OuvrirDansWindowsToolStripMenuItem.Click
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        SelectedMap = DirectCast(ListView1.SelectedItems(0).Tag, Map)
        If Not Directory.Exists(Main.DirectoryApply & "\Maps\" & SelectedMap.ID) Then Exit Sub
        Process.Start(Main.DirectoryApply & "\Maps\" & SelectedMap.ID)
    End Sub
#End Region

    Private Sub ListView1_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If ListView1.SelectedItems.Count = 0 Then
                SupprimerToolStripMenuItem.Visible = False
                OuvrirDansWindowsToolStripMenuItem.Visible = False
            Else
                SupprimerToolStripMenuItem.Visible = True
                OuvrirDansWindowsToolStripMenuItem.Visible = True
            End If
            ContextMenuStrip1.Show(Cursor.Position)
        End If
    End Sub

End Class