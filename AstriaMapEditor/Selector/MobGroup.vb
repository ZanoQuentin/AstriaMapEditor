Imports System.Xml.Serialization

Public Class MobGroup

    Public Mobs As String = ""

    Public Sub New(Optional ByVal group As String = "")
        InitializeComponent()
        If Not group = "" Then
            Mobs = group
            Dim mobss() As String = Mobs.Split("|")
            For Each Mob As String In mobss
                If Mob <> "" Then
                    Dim MyMonster As Monster = Monster.GetByID(Mob.Split(",")(0))
                    ListView2.Items.Add(MyMonster.Name).SubItems.Add(Mob.Split(",")(1)).Tag = MyMonster.ID
                End If
            Next
        End If
    End Sub

    Private Sub MobGroup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        KryptonListBox1.Items.AddRange(Monster.Monsters)
    End Sub

    Private Sub KryptonListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonListBox1.SelectedIndexChanged
        Dim aMonster As Monster = KryptonListBox1.SelectedItem
        KryptonLabel1.Text = aMonster.Name
        KryptonLabel5.Text = aMonster.MinKamas
        KryptonLabel4.Text = aMonster.MaxKamas
        ListView1.Items.Clear()
        Dim Caracts() As Monster.Caracteristiques = aMonster.Explode
        For Each caract As Monster.Caracteristiques In Caracts
            If caract.Level = 0 Then Exit For
            Dim MyListViewItem As New ListViewItem
            MyListViewItem.Text = caract.Level
            MyListViewItem.SubItems.Add(caract.Pdvs)
            MyListViewItem.SubItems.Add(caract.PA & " / " & caract.PM)
            MyListViewItem.SubItems.Add(caract.Initiative)
            MyListViewItem.SubItems.Add(caract.Experience)
            ListView1.Items.Add(MyListViewItem)
        Next
    End Sub

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton1.Click
        If IsNothing(KryptonListBox1.SelectedItem) Then
            MsgBox("Veuillez selectionner un monstre !")
            Exit Sub
        End If
        If ListView1.SelectedItems.Count = 0 Then
            MsgBox("Veuillez selectionner un level !")
            Exit Sub
        End If
        Dim aMonster As Monster = KryptonListBox1.SelectedItem
        For Each item As ListViewItem In ListView1.SelectedItems
            ListView2.Items.Add(aMonster.Name).SubItems.Add(item.Text).Tag = aMonster.ID
        Next
    End Sub

    Private Sub KryptonButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton2.Click
        If ListView2.SelectedItems.Count = 0 Then Exit Sub
        For Each item As ListViewItem In ListView2.SelectedItems
            ListView2.Items.Remove(item)
        Next
    End Sub

    Private Sub KryptonButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton3.Click
        Mobs = ""
        For Each line As ListViewItem In ListView2.Items
            Mobs &= line.SubItems(1).Tag & "," & line.SubItems(1).Text & "|"
        Next
        Me.Close()
    End Sub

End Class