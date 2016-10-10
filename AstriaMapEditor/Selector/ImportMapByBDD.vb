Imports System.Net

Public Class ImportMapByBDD

    Dim ListMaps As New List(Of MapInfos)

    Structure MapInfos
        Dim id As Integer
        Dim name As String
    End Structure

    Private Sub ImportMapByBDD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If ListView2.Items.Count = 0 Then
            Dim Client As New WebClient
            Dim maps As String = System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(Client.DownloadString(Main.Link_PHP & "?get=maps&hwid=" & Main.Get_HWID())))
            For Each amap As String In maps.Split("|")
                If amap = "" Then Continue For
                If Not amap.Contains(",") Then Continue For

                Dim NewMap As MapInfos
                NewMap.id = amap.Split(",")(0)
                NewMap.name = amap.Split(",")(1)

                ListView2.Items.Add(NewMap.id).SubItems.Add(NewMap.name)

                ListMaps.Add(NewMap)
            Next
        End If
    End Sub

    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        KryptonButton1.Enabled = True
    End Sub

    Private Sub ToolStripTextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripTextBox2.KeyPress
        ' Search Date
        If ToolStripTextBox2.Text = "" Then
            For Each NewMap As MapInfos In ListMaps
                ListView2.Items.Add(NewMap.id).SubItems.Add(NewMap.name)
            Next
            Exit Sub
        End If
        ListView2.Items.Clear()
        For Each NewMap As MapInfos In ListMaps
            If NewMap.name.ToUpper().Contains(ToolStripTextBox2.Text.ToUpper) Then
                ListView2.Items.Add(NewMap.id).SubItems.Add(NewMap.name)
            End If
        Next
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        ' Search ID
        If ToolStripTextBox1.Text = "" Then
            For Each NewMap As MapInfos In ListMaps
                ListView2.Items.Add(NewMap.id).SubItems.Add(NewMap.name)
            Next
            Exit Sub
        End If
        If Not IsNumeric(ToolStripTextBox1.Text) Then
            ToolStripTextBox1.Text = ""
            Exit Sub
        End If

        ListView2.Items.Clear()
        For Each NewMap As MapInfos In ListMaps
            If NewMap.id = ToolStripTextBox1.Text Then
                ListView2.Items.Add(NewMap.id).SubItems.Add(NewMap.name)
            End If
        Next
    End Sub

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton1.Click, ListView2.DoubleClick
        ' Importer

        For Each item As ListViewItem In ListView2.SelectedItems
            Dim client As New WebClient

            ' Importation SQL
            Dim maps As String = System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(client.DownloadString(Main.Link_PHP & "?get=map&hwid=" & Main.Get_HWID() & "&id=" & item.Text)))
            Dim map() As String = maps.Split("!")

            ' Importation SWF
            Try
                Dim datemap As String = item.SubItems(1).Text
                If CInt(item.Text) < 12263 Then datemap &= "X"
                If Not IO.Directory.Exists("tmp") Then IO.Directory.CreateDirectory("tmp")
                client.DownloadFile(Options.MyOptions.LinkSWF & item.Text & "_" & datemap & ".swf", "tmp/" & item.Text & "_" & datemap & ".swf")
                Main.OpenSWFProject("tmp/" & item.Text & "_" & datemap & ".swf", map(1))
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Dim aMap As MapEditor = Main.ListOfMapEditors.Item(Main.ListOfMapEditors.Count - 1)

            ' Importation SQL
            aMap.MyDatas.FightPlaces = map(0)
            aMap.MyDatas.LoadFightPlaces()
            aMap.MyDatas.Mobs = map(2)
            If map(3).Contains(",") Then
                aMap.MyDatas.X = map(3).Split(",")(0)
                aMap.MyDatas.Y = map(3).Split(",")(1)
                aMap.MyDatas.Area = map(3).Split(",")(2)
            End If
            aMap.MyDatas.NbGroups = map(4)
            aMap.MyDatas.GroupMaxSize = map(5)
            aMap.RefreshDraw()
            aMap.Save()
        Next

        Me.Close()
    End Sub

End Class
