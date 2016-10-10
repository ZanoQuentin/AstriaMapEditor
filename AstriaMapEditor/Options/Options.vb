Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Public Class Options

    Public FileName As String = Main.DirectoryApply & "\config"
    Public ListOptions As New List(Of MyOptions)
    Public MyOptions As New MyOptions

    Public Sub New()
        InitializeComponent()
        LoadFile()
        LoadConfig()
        Me.TopMost = True
    End Sub

    Private Sub OnClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Save()
        Dim FluxDeFichier As FileStream = File.Create(FileName)
        Dim Serialiseur As New BinaryFormatter
        Serialiseur.Serialize(FluxDeFichier, ListOptions)
        FluxDeFichier.Close()
        Me.Visible = False
    End Sub

#Region " Load "

    Public Sub LoadFile()
        If IO.File.Exists(FileName) Then
            Dim FluxDeFichier As Stream = File.OpenRead(FileName)
            Dim Deserialiseur As New BinaryFormatter()
            ListOptions = CType(Deserialiseur.Deserialize(FluxDeFichier), List(Of MyOptions))
            MyOptions = ListOptions.Item(0)
            FluxDeFichier.Close()
            For Each aOption As MyOptions In ListOptions
                List_Profils.Items.Add(aOption.Profil)
            Next
            List_Profils.SelectedIndex = 0
        Else
            MyOptions = New MyOptions
            List_Profils.Items.Add(MyOptions.Profil)
            List_Profils.SelectedIndex = 0
            ListOptions.Add(MyOptions)
        End If
        Me.Text = "Options : " & MyOptions.Profil
    End Sub

#End Region

#Region " Config manager "

    Public Sub LoadConfig()
        ' Général
        Main.Panel1.Height = MyOptions.SizePanel
        TXT_SizePanel.Value = MyOptions.SizePanel
        CB_SaveBDD.Checked = MyOptions.SaveSQL
        CB_ResizePanel.Checked = MyOptions.ResizePanel
        TXT_NewSizePanel.Value = MyOptions.NewSizePanel
        ' MySQL
        '       Général = Général
        TXT_MySQL_Host.Text = MyOptions.MySQL_Host
        TXT_MySQL_User.Text = MyOptions.MySQL_User
        TXT_MySQL_Pass.Text = MyOptions.MySQL_Password
        TXT_MySQL_Database.Text = MyOptions.MySQL_Database
        '       Maps
        TXT_MySQL_TableName_Maps.Text = MyOptions.MySQL_TableName_Maps
        TXT_MySQL_Maps_ID.Text = MyOptions.MySQL_Maps_ID
        TXT_MySQL_Maps_Date.Text = MyOptions.MySQL_Maps_Date
        TXT_MySQL_Maps_Width.Text = MyOptions.MySQL_Maps_Width
        TXT_MySQL_Maps_Height.Text = MyOptions.MySQL_Maps_Height
        TXT_MySQL_Maps_MapData.Text = MyOptions.MySQL_Maps_MapData
        TXT_MySQL_Maps_Key.Text = MyOptions.MySQL_Maps_Key
        TXT_MySQL_Maps_FightPlaces.Text = MyOptions.MySQL_Maps_FightPlaces
        TXT_MySQL_Maps_Monsters.Text = MyOptions.MySQL_Maps_Monsters
        TXT_MySQL_Maps_Capabilities.Text = MyOptions.MySQL_Maps_Capabilities
        TXT_MySQL_Maps_Pos.Text = MyOptions.MySQL_Maps_Pos
        TXT_MySQL_Maps_NbGroups.Text = MyOptions.MySQL_Maps_NbGroups
        TXT_MySQL_Maps_SizeMaxGroup.Text = MyOptions.MySQL_Maps_SizeMaxGroup
        '       Triggers
        TXT_MySQL_TableName_Triggers.Text = MyOptions.MySQL_TableName_Triggers
        TXT_MySQL_Triggers_MapID.Text = MyOptions.MySQL_Triggers_MapID
        TXT_MySQL_Triggers_CellID.Text = MyOptions.MySQL_Triggers_CellID
        TXT_MySQL_Triggers_Action.Text = MyOptions.MySQL_Triggers_Action
        TXT_MySQL_Triggers_Args.Text = MyOptions.MySQL_Triggers_Args
        TXT_MySQL_Triggers_IdTP.Value = MyOptions.MySQL_Triggers_IdTP
        '       End Fight
        TXT_MySQL_TableName_EndFight.Text = MyOptions.MySQL_TableName_EndFight
        TXT_MySQL_EndFight_MapID.Text = MyOptions.MySQL_EndFight_MapID
        TXT_MySQL_EndFight_Action.Text = MyOptions.MySQL_EndFight_Action
        TXT_MySQL_EndFight_Args.Text = MyOptions.MySQL_EndFight_Args
        TXT_MySQL_EndFight_IdTP.Value = MyOptions.MySQL_EndFight_IdTP
        '       MobGroup
        TXT_MySQL_TableName_Mobs.Text = MyOptions.MySQL_TableName_Mobs
        TXT_MySQL_Mobs_MapID.Text = MyOptions.MySQL_Mobs_MapID
        TXT_MySQL_Mobs_CellID.Text = MyOptions.MySQL_Mobs_CellID
        TXT_MySQL_Mobs_GroupData.Text = MyOptions.MySQL_Mobs_GroupData
        '       Pnjs
        TXT_MySQL_TableName_Pnjs.Text = MyOptions.MySQL_TableName_Pnjs
        TXT_MySQL_Pnj_MapID.Text = MyOptions.MySQL_Pnj_MapID
        TXT_MySQL_Pnj_CellID.Text = MyOptions.MySQL_Pnj_CellID
        TXT_MySQL_Pnj_NpcID.Text = MyOptions.MySQL_Pnj_NpcID
        TXT_MySQL_Pnj_Orientation.Text = MyOptions.MySQL_Pnj_Orientation
        '       Houses
        TXT_MySQL_TableName_Houses.Text = MyOptions.MySQL_TableName_Houses
        TXT_MySQL_Houses_ID.Text = MyOptions.MySQL_Houses_ID
        TXT_MySQL_Houses_MapID.Text = MyOptions.MySQL_Houses_MapID
        TXT_MySQL_Houses_CellID.Text = MyOptions.MySQL_Houses_CellID
        TXT_MySQL_Houses_Price.Text = MyOptions.MySQL_Houses_Price
        TXT_MySQL_Houses_ToMapID.Text = MyOptions.MySQL_Houses_ToMapID
        TXT_MySQL_Houses_ToCellID.Text = MyOptions.MySQL_Houses_ToCellID
        '       MountPark
        TXT_MySQL_TableName_MountParks.Text = MyOptions.MySQL_TableName_MountParks
        TXT_MySQL_MountParks_MapID.Text = MyOptions.MySQL_MountParks_MapID
        TXT_MySQL_MountParks_CellID.Text = MyOptions.MySQL_MountParks_CellID
        TXT_MySQL_MountParks_Size.Text = MyOptions.MySQL_MountParks_Size
        TXT_MySQL_MountParks_Price.Text = MyOptions.MySQL_MountParks_Price
        '       Zaaps
        TXT_MySQL_TableName_Zaaps.Text = MyOptions.MySQL_TableName_Zaaps
        TXT_MySQL_Zaaps_MapID.Text = MyOptions.MySQL_Zaaps_MapID
        TXT_MySQL_Zaaps_CellID.Text = MyOptions.MySQL_Zaaps_CellID
        '       Zaapis
        TXT_MySQL_TableName_Zaapis.Text = MyOptions.MySQL_TableName_Zaapis
        TXT_MySQL_Zaapis_MapID.Text = MyOptions.MySQL_Zaapis_MapID
        TXT_MySQL_Zaapis_Align.Text = MyOptions.MySQL_Zaapis_Align
        ' SWF
        TXT_PathToCopySWF.Text = MyOptions.SWF_PathCopy
        ' PHP
        TXT_LinkSWF.Text = MyOptions.LinkSWF
    End Sub

    Public Sub Save()
        ' Général
        MyOptions.SizePanel = TXT_SizePanel.Value
        MyOptions.SaveSQL = CB_SaveBDD.Checked
        MyOptions.ResizePanel = CB_ResizePanel.Checked
        MyOptions.NewSizePanel = TXT_NewSizePanel.Value
        ' MySQL
        '       Général
        MyOptions.MySQL_Host = TXT_MySQL_Host.Text
        MyOptions.MySQL_User = TXT_MySQL_User.Text
        MyOptions.MySQL_Password = TXT_MySQL_Pass.Text
        MyOptions.MySQL_Database = TXT_MySQL_Database.Text
        '        Maps
        MyOptions.MySQL_TableName_Maps = TXT_MySQL_TableName_Maps.Text
        MyOptions.MySQL_Maps_ID = TXT_MySQL_Maps_ID.Text
        MyOptions.MySQL_Maps_Date = TXT_MySQL_Maps_Date.Text
        MyOptions.MySQL_Maps_Width = TXT_MySQL_Maps_Width.Text
        MyOptions.MySQL_Maps_Height = TXT_MySQL_Maps_Height.Text
        MyOptions.MySQL_Maps_MapData = TXT_MySQL_Maps_MapData.Text
        MyOptions.MySQL_Maps_Key = TXT_MySQL_Maps_Key.Text
        MyOptions.MySQL_Maps_FightPlaces = TXT_MySQL_Maps_FightPlaces.Text
        MyOptions.MySQL_Maps_Monsters = TXT_MySQL_Maps_Monsters.Text
        MyOptions.MySQL_Maps_Capabilities = TXT_MySQL_Maps_Capabilities.Text
        MyOptions.MySQL_Maps_Pos = TXT_MySQL_Maps_Pos.Text
        MyOptions.MySQL_Maps_NbGroups = TXT_MySQL_Maps_NbGroups.Text
        MyOptions.MySQL_Maps_SizeMaxGroup = TXT_MySQL_Maps_SizeMaxGroup.Text
        '       Triggers
        MyOptions.MySQL_TableName_Triggers = TXT_MySQL_TableName_Triggers.Text
        MyOptions.MySQL_Triggers_MapID = TXT_MySQL_Triggers_MapID.Text
        MyOptions.MySQL_Triggers_CellID = TXT_MySQL_Triggers_CellID.Text
        MyOptions.MySQL_Triggers_Action = TXT_MySQL_Triggers_Action.Text
        MyOptions.MySQL_Triggers_Args = TXT_MySQL_Triggers_Args.Text
        MyOptions.MySQL_Triggers_IdTP = TXT_MySQL_Triggers_IdTP.Value
        '       End Fight
        MyOptions.MySQL_TableName_EndFight = TXT_MySQL_TableName_EndFight.Text
        MyOptions.MySQL_EndFight_MapID = TXT_MySQL_EndFight_MapID.Text
        MyOptions.MySQL_EndFight_Action = TXT_MySQL_EndFight_Action.Text
        MyOptions.MySQL_EndFight_Args = TXT_MySQL_EndFight_Args.Text
        MyOptions.MySQL_EndFight_IdTP = TXT_MySQL_EndFight_IdTP.Value
        '       MobGroup
        MyOptions.MySQL_TableName_Mobs = TXT_MySQL_TableName_Mobs.Text
        MyOptions.MySQL_Mobs_MapID = TXT_MySQL_Mobs_MapID.Text
        MyOptions.MySQL_Mobs_CellID = TXT_MySQL_Mobs_CellID.Text
        MyOptions.MySQL_Mobs_GroupData = TXT_MySQL_Mobs_GroupData.Text
        '       Pnjs
        MyOptions.MySQL_TableName_Pnjs = TXT_MySQL_TableName_Pnjs.Text
        MyOptions.MySQL_Pnj_MapID = TXT_MySQL_Pnj_MapID.Text
        MyOptions.MySQL_Pnj_CellID = TXT_MySQL_Pnj_CellID.Text
        MyOptions.MySQL_Pnj_NpcID = TXT_MySQL_Pnj_NpcID.Text
        MyOptions.MySQL_Pnj_Orientation = TXT_MySQL_Pnj_Orientation.Text
        '       Houses
        MyOptions.MySQL_TableName_Houses = TXT_MySQL_TableName_Houses.Text
        MyOptions.MySQL_Houses_ID = TXT_MySQL_Houses_ID.Text
        MyOptions.MySQL_Houses_MapID = TXT_MySQL_Houses_MapID.Text
        MyOptions.MySQL_Houses_CellID = TXT_MySQL_Houses_CellID.Text
        MyOptions.MySQL_Houses_Price = TXT_MySQL_Houses_Price.Text
        MyOptions.MySQL_Houses_ToMapID = TXT_MySQL_Houses_ToMapID.Text
        MyOptions.MySQL_Houses_ToCellID = TXT_MySQL_Houses_ToCellID.Text
        '       MountPark
        MyOptions.MySQL_TableName_MountParks = TXT_MySQL_TableName_MountParks.Text
        MyOptions.MySQL_MountParks_MapID = TXT_MySQL_MountParks_MapID.Text
        MyOptions.MySQL_MountParks_CellID = TXT_MySQL_MountParks_CellID.Text
        MyOptions.MySQL_MountParks_Size = TXT_MySQL_MountParks_Size.Text
        MyOptions.MySQL_MountParks_Price = TXT_MySQL_MountParks_Price.Text
        '       Zaaps
        MyOptions.MySQL_TableName_Zaaps = TXT_MySQL_TableName_Zaaps.Text
        MyOptions.MySQL_Zaaps_MapID = TXT_MySQL_Zaaps_MapID.Text
        MyOptions.MySQL_Zaaps_CellID = TXT_MySQL_Zaaps_CellID.Text
        '       Zaapis
        MyOptions.MySQL_TableName_Zaapis = TXT_MySQL_TableName_Zaapis.Text
        MyOptions.MySQL_Zaapis_MapID = TXT_MySQL_Zaapis_MapID.Text
        MyOptions.MySQL_Zaapis_Align = TXT_MySQL_Zaapis_Align.Text
        ' SWF
        MyOptions.SWF_PathCopy = TXT_PathToCopySWF.Text
        ' PHP
        MyOptions.LinkSWF = TXT_LinkSWF.Text
    End Sub

#End Region

#Region " Handles config "

    Private Sub BT_PathToCopySWF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_PathToCopySWF.Click
        Dim Dialog As New FolderBrowserDialog
        If (Dialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            TXT_PathToCopySWF.Text = Dialog.SelectedPath
        End If
    End Sub

    Private Sub TXT_SizePanel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_SizePanel.ValueChanged
        MyOptions.SizePanel = TXT_SizePanel.Value
        Main.Panel1.Height = MyOptions.SizePanel
    End Sub

    Private Sub CB_ResizePanel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CB_ResizePanel.CheckedChanged
        MyOptions.ResizePanel = CB_ResizePanel.Checked
    End Sub

    Private Sub TXT_NewSizePanel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_NewSizePanel.ValueChanged
        MyOptions.NewSizePanel = TXT_NewSizePanel.Value
    End Sub

#End Region

#Region " Profil "

    Private Sub BT_LoadProfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_LoadProfil.Click
        Save()
        MyOptions = ListOptions.Item(List_Profils.SelectedIndex)
        LoadConfig()
    End Sub

    Private Sub BT_CreateProfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_CreateProfil.Click
        If Not TXT_NewProfil.Text = "" Then
            Save()
            MyOptions = New MyOptions
            MyOptions.Profil = TXT_NewProfil.Text
            Me.Text = "Options : " & MyOptions.Profil
            LoadConfig()
            ListOptions.Add(MyOptions)
            List_Profils.Items.Add(MyOptions.Profil)
            List_Profils.SelectedIndex = List_Profils.Items.Count - 1
            TXT_NewProfil.Text = ""
        End If
    End Sub

    Private Sub BT_DeleteProfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_DeleteProfil.Click
        If List_Profils.Items.Count > 1 Then
            ListOptions.RemoveAt(List_Profils.SelectedIndex)
            List_Profils.Items.RemoveAt(List_Profils.SelectedIndex)
            MyOptions = ListOptions.Item(0)
            Me.Text = "Options : " & MyOptions.Profil
            LoadConfig()
        Else
            MsgBox("Impossible de supprimer le dernier profil.")
        End If
    End Sub

#End Region

End Class