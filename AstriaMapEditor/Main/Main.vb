Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text

Public Class Main

    Public m_ChildFormNumber As Integer
    Private Options As Options
    Public DirectoryApply As String

    Public SelectedTile As Tile = Nothing
    Public Outil As Tools = Tools.Brush
    Public Calque As Integer = 1

    Public Show_Background As Boolean = True
    Public Show_Ground As Boolean = True
    Public Show_Calque1 As Boolean = True
    Public Show_Calque2 As Boolean = True
    Public Show_Grid As Boolean = False
    Public Show_CellID As Boolean = False

    Public aCellMode As CellMode

    Public SelectedMap As MapEditor
    Public SelectedFlip As Boolean = False
    Public SelectedRotate As Integer = 0

    Public Mode_Trigger As Boolean = False
    Public Trigger_Cell1 As Integer = 0
    Public Trigger_Map1 As Integer = 0
    Public NbTriggers As Integer = 0

    Public Mode_EndFightAction As Boolean = False
    Public EndFightAction_Map1 As Map

    Private SelectedCalque_In_CellMenu As Integer = 1
    Private ChangingCheckBoxState As Boolean = False
    Private ChangingCheckBoxState2 As Boolean = False

    Public ListOfMapEditors As New List(Of MapEditor)

    Public Const Link_PHP As String = "http://quenttiin.alwaysdata.net/ame_manager.php"
    ' Supprimé pour le login et l'obtention des XML, par contre sert pour : ImportMapByBDD (importation de maps directement depuis sa DB)

    Enum Tools
        Brush
        Selector
        CellMode
    End Enum

    Enum CellMode
        Null
        Unwalkable
        LoS
        Path
        Paddock
        Fight1
        Fight2
    End Enum

#Region " Chargements "

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.KeyPreview = True
        Me.AllowDrop = True
        Dim MyFileInfos As New FileInfo(Environment.GetCommandLineArgs(0))
        DirectoryApply = MyFileInfos.DirectoryName
        ChDir(DirectoryApply)
        Options = New Options

        ' Version
        ToolStripStatusLabel4.Text = "v." & My.Application.Info.Version.ToString & " By Zano"

        ' Nettoyage de l'autosave
        ClearAutoSave()

        ' Suppression du dossier temporaire
        If IO.Directory.Exists("tmp") Then IO.Directory.Delete("tmp", True)

        ' XML
        XMLLoader.LoadAllXML()

        ' Chargement des Areas/SubAreas
        ComboBox_Areas.Items.AddRange(Area.Areas)
        ComboBox_SubAreas.Items.AddRange(SubArea.SubAreas)

        ' Background Worker du chargement des images
        LoadImages.WorkerReportsProgress = True
        AddHandler LoadImages.DoWork, AddressOf LoadImages_DoWork
        AddHandler LoadImages.RunWorkerCompleted, AddressOf LoadImages_RunWorkerCompleted
        AddHandler LoadImages.ProgressChanged, AddressOf LoadImages_ProgressChanged
        If Not LoadImages.IsBusy Then
            RunWorkerCompleted = True
            DoWork = True
            LoadImages.RunWorkerAsync()
        End If

        ' Background Worker du chargement des tiles
        AddHandler LoadTiles.DoWork, AddressOf LoadTiles_DoWork
        AddHandler LoadTiles.RunWorkerCompleted, AddressOf LoadTiles_RunWorkerCompleted

        ' Chargement des maps
        LoadMaps()
        Main_MenuCell_RefreshInfos()
        MenuMap_RefreshControls()

        ' Nettoyage
        GC.Collect()
    End Sub

    Private Sub Launch()
        ' En cas d'ouverture du logiciel avec un fichier AME ou SWF
        If Environment.GetCommandLineArgs.Count >= 2 Then
            'Ouvrir un projet avec le fichier en parametre
            Dim MyAMEInfos As New FileInfo(Environment.GetCommandLineArgs(1))
            If MyAMEInfos.Extension = ".ame" Then
                OpenAMEProject(MyAMEInfos.FullName)
            ElseIf MyAMEInfos.Extension = ".swf" Then
                OpenSWFProject(MyAMEInfos.FullName)
            Else
                MsgBox("Le format " & MyAMEInfos.Extension & " n'est pas pris en compte par l'éditeur.")
                End
            End If
        End If
    End Sub

#Region " Drag&Drop "

    Private Sub Me_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Me_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop
        For Each PassedFile In e.Data.GetData(DataFormats.FileDrop)
            Dim myfile As New FileInfo(PassedFile)
            If myfile.Extension = ".ame" Then
                OpenAMEProject(myfile.FullName)
            ElseIf myfile.Extension = ".swf" Then
                OpenSWFProject(myfile.FullName)
            Else
                MsgBox("Le format " & myfile.Extension & " n'est pas pris en compte par l'éditeur.")
            End If
        Next
    End Sub

#End Region

    Private Sub LoadMaps()
        ' Chargement des maps du dossier Maps
        Dim InfosDirectory1 As New DirectoryInfo(DirectoryApply & "\Maps")
        For Each DirectoryName As DirectoryInfo In InfosDirectory1.GetDirectories
            For Each FileName As FileInfo In DirectoryName.GetFiles
                If FileName.Extension = ".ame" Then
                    If DirectoryName.Name = "AutoSave" Then Continue For
                    ' Chargement du fichier ame
                    Try
                        Dim FluxDeFichier As Stream = File.OpenRead(FileName.FullName)
                        Dim Deserialiseur As New BinaryFormatter()
                        Dim aMap As Map = CType(Deserialiseur.Deserialize(FluxDeFichier), Map)
                        FluxDeFichier.Close()

                        If Not IsNumeric(DirectoryName.Name) Or Not DirectoryName.Name = aMap.ID.ToString Then
                            Try
                                My.Computer.FileSystem.RenameDirectory(DirectoryName.FullName, DirectoryName.Parent.FullName & "\" & aMap.ID.ToString)
                            Catch
                                MsgBox("Le dossier """ & DirectoryName.Name & """ n'a pas le nom de l'id de la map présent à l'intérieur de celui ci." & vbCrLf & "De plus, il n'est pas possible de le renommer, un dossier contenant une map avec ce même id est déjà existant." & vbCrLf & "Il sera donc sera ignoré.")
                                Continue For
                            End Try
                        End If

                        If IsNothing(aMap.Screenshot) Then
                            If IO.File.Exists(DirectoryApply & "\Maps\" & aMap.ID & "\" & aMap.ID & ".png") Then
                                Dim fileStream As New System.IO.FileStream(DirectoryApply & "\Maps\" & aMap.ID & "\" & aMap.ID & ".png", IO.FileMode.Open, IO.FileAccess.Read)
                                aMap.Screenshot = New Bitmap(Image.FromStream(fileStream))
                                fileStream.Close()
                            Else
                                Dim SizeOfImg As New Size(15 * MapEditor.SizeBaseCell * 2, 17 * MapEditor.SizeBaseCell + MapEditor.SizeBaseCell)
                                aMap.Screenshot = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
                            End If
                        End If

                        If Not IsNothing(aMap) Then
                            Map.AddMap(aMap)
                        End If
                    Catch ex As Exception
                        MsgBox("Erreur lors du chargement de la map : " & FileName.FullName & vbCrLf & vbCrLf & ex.Message)
                    End Try
                End If
            Next
        Next
    End Sub

    Friend Sub ClearAutoSave()
        If Not IO.Directory.Exists(DirectoryApply & "/Maps/AutoSave") Then Exit Sub

        Dim files As FileInfo() = New DirectoryInfo(DirectoryApply & "/Maps/AutoSave").GetFiles
        For Each info4 As FileInfo In files
            If Not info4.Extension = ".ame" Then
                IO.File.Delete(info4.FullName)
            Else
                Dim uNow As Integer = (DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
                Dim uFile As Integer = (info4.LastWriteTimeUtc - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
                If uNow - uFile > 60 * 60 * 24 Then
                    IO.File.Delete(info4.FullName)
                End If
            End If
        Next
    End Sub

#End Region

#Region " Menus "

#Region " Speedbar top "

#Region " Fichiers "
    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles BT_GestionnaireDeMaps.Click
        Dim SelectMap As New SelectMap()
        SelectMap.ShowDialog()
        If Not IsNothing(SelectMap.SelectedMap) Then
            If Not SelectMap.SelectedMap.IsEditing Then
                Dim ChildForm As New MapEditor(SelectMap.SelectedMap)
                ChildForm.MdiParent = Me
                m_ChildFormNumber += 1
                ChildForm.SizeTable = New Size(ChildForm.MyDatas.Width, ChildForm.MyDatas.Height)
                ChildForm.Text = "Map " & ChildForm.MyDatas.ID
                ChildForm.Show()
                ListOfMapEditors.Add(ChildForm)
            Else
                MsgBox("Map déjà en cours d'édition.")
            End If
        End If
        SelectMap.Dispose()
    End Sub

    Friend Sub ImporterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Importation.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = DirectoryApply
        OpenFileDialog.Filter = "Fichiers Map (*.ame, *.swf)|*.ame;*_*.swf"
        OpenFileDialog.Multiselect = True
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            For Each file In OpenFileDialog.FileNames
                Dim FileName As New FileInfo(file)
                If FileName.Extension = ".ame" Then
                    OpenAMEProject(FileName.FullName)
                ElseIf FileName.Extension = ".swf" Then
                    OpenSWFProject(FileName.FullName)
                Else
                    MsgBox("Le format " & FileName.Extension & " n'est pas pris en compte par l'éditeur.")
                End If
            Next
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        ' Importation PHP
        ImportMapByBDD.ShowDialog()
    End Sub

    Private Sub MoyenneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_NewMapMedium.Click
        OpenNewProject(15, 17)
    End Sub

    Private Sub GrandeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_NewMapBig.Click
        OpenNewProject(19, 22)
    End Sub

    Private Sub BT_NewMapOther_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_NewMapOther.Click
        Dim MyInput As New InputSizeMap
        With MyInput
            .KryptonLabel3.Visible = False
            .KryptonNumericUpDown4.Visible = False
            .KryptonNumericUpDown3.Visible = False
            .KryptonLabel4.Visible = False
            .ShowDialog()
        End With
        OpenNewProject(MyInput._Width, MyInput._Height)
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Save.Click
        If Not IsNothing(SelectedMap) Then
            SelectedMap.Save()
            SelectedMap.Edited = False
        End If
    End Sub

    Private Sub ToolStripButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        For Each aMap As MapEditor In Me.ListOfMapEditors
            aMap.Save()
            aMap.Edited = False
        Next
        ToolStripStatusLabel1.Text = "[" & Date.Now.Hour & "h" & Date.Now.Minute & "] Toutes les maps ont été sauvegardé."
    End Sub

#End Region

#Region " Outils "
    Private Sub GéopositionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Geoposition.Click
        Dim Geo As New Geoposition
        Geo.MdiParent = Me
        Geo.Show()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Options.Click
        Options.Show()
    End Sub
#End Region

#Region " Edition "
    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Utilisez My.Computer.Clipboard pour insérer les images ou le texte sélectionné dans le Presse-papiers
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Utilisez My.Computer.Clipboard pour insérer les images ou le texte sélectionné dans le Presse-papiers
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Utilisez My.Computer.Clipboard.GetText() ou My.Computer.Clipboard.GetData pour extraire les informations du Presse-papiers.
    End Sub
#End Region

#Region " ? "
    Private Sub HelpMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Help.Click
        Manuel.Show()
    End Sub
#End Region

#Region " Liens "
    Private Sub ToolStripButton14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton14.Click
        System.Diagnostics.Process.Start("http://ame.astria-serv.com")
    End Sub

    Private Sub ToolStripButton15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton15.Click
        System.Diagnostics.Process.Start("http://www.astria-serv.com")
    End Sub
#End Region

#Region " Mode trigger "
    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Trigger.Click
        MsgBox("Veuillez cliquer sur la cellule de départ." & vbCrLf & vbCrLf & "Appuyez sur Echap pour annuler.")
        Outil = Tools.CellMode
        Mode_Trigger = True
        RefreshAllMaps()
    End Sub

    Public Sub Trigger_Add(ByVal mapid As Integer, ByVal cellid As Integer, ByRef map As MapEditor)
        If Trigger_Cell1 = 0 Then
            NbTriggers += 1
            Trigger_Cell1 = cellid
            Trigger_Map1 = mapid
            map.MyDatas.Cells(cellid).Trigger = True
            map.MyDatas.Cells(cellid).TriggerName = NbTriggers & "D"
            map.DrawAll()
        Else
            Dim sql As String = _
                    "DELETE FROM `scripted_cells` WHERE `MapID`=" & Trigger_Map1 & " AND `CellID`=" & Trigger_Cell1 & " AND `ActionsArgs`='" & mapid & "," & cellid & "';" & vbCrLf & _
                    "INSERT INTO `scripted_cells` VALUES(" & Trigger_Map1 & ", " & Trigger_Cell1 & ", 0, 1, '" & mapid & "," & cellid & "', -1);" & vbCrLf
            IO.File.AppendAllText(DirectoryApply & "\Maps\scripted_cells.sql", sql)
            Trigger_Cell1 = 0
            Trigger_Map1 = 0
            map.MyDatas.Cells(cellid).Trigger = True
            map.MyDatas.Cells(cellid).TriggerName = NbTriggers & "A"
            map.DrawAll()
            If MsgBox("La ligne SQL a été ajouté au fichier Maps\scripted_cells.sql." & vbCrLf & "Continuer ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                MsgBox("Veuillez cliquer sur la cellule de départ." & vbCrLf & vbCrLf & "Appuyez sur Echap pour annuler.")
            Else
                Mode_Trigger = False
                Outil = Tools.Brush
                RefreshAllMaps()
            End If
        End If
    End Sub
#End Region

#Region " Mode endfightaction "
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Dungeon.Click
        MsgBox("Veuillez cliquer sur la map de départ." & vbCrLf & vbCrLf & "Appuyez sur Echap pour annuler.")
        Outil = Tools.CellMode
        Mode_EndFightAction = True
        RefreshAllMaps()
    End Sub

    Public Sub EndFightAction_Add(ByRef map As Map, ByVal cellid As Integer)
        If IsNothing(EndFightAction_Map1) Then
            EndFightAction_Map1 = map
            MsgBox("Veuillez cliquer sur la cellule d'arrivé.")
        Else
            If Not map.ID = EndFightAction_Map1.ID Then
                MsgBox("La map n°" & EndFightAction_Map1.ID & " téléportera maintenant en fin de combat vers la map n°" & map.ID & " sur la cellule n°" & cellid & "." & vbCrLf & vbCrLf & "La ligne SQL sera ajouté au fichier d'installation de la map lors de la prochaine sauvegarde de celle ci." & vbCrLf & "Pour annuler cette action supprimer la dans gestionnaire de map.")
                EndFightAction_Map1.NextRoom = map.ID
                EndFightAction_Map1.NextCell = cellid
                IO.File.AppendAllText(Me.DirectoryApply & "/Maps/endfight_action.sql", MySQL.Get_SqlDungeon(EndFightAction_Map1.ID, map.ID, cellid))
            Else
                MsgBox("La map de départ et celle d'arrivée doivent avoir un identifiant différent.")
            End If
            EndFightAction_Map1 = map
            MsgBox("Veuillez cliquer sur la cellule d'arrivé suivante.")
        End If
    End Sub
#End Region

    Private Sub BT_Puzzle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Puzzle.Click
        Dim nb As Integer = 0
        Dim nb2 As Integer = 0
        For Each aMap As MapEditor In Me.ListOfMapEditors
            aMap.SizeCell = 15
            aMap.SizeOfImg = New Size(aMap.SizeTable.Width * aMap.SizeCell * 2, aMap.SizeTable.Height * aMap.SizeCell)
            aMap.Size = New Size(aMap.SizeOfImg.Width + 16, aMap.SizeOfImg.Height + 38)
            aMap.MyImage = New Bitmap(aMap.SizeOfImg.Width, aMap.SizeOfImg.Height)
            aMap.Grid = New Bitmap(aMap.SizeOfImg.Width, aMap.SizeOfImg.Height)
            aMap.G = Graphics.FromImage(aMap.MyImage)
            aMap.PourceOfTile = aMap.SizeCell / MapEditor.SizeBaseCell
            aMap.GenerateGrid()
            aMap.DrawAll()

            aMap.Location = New Point(aMap.Size.Width * nb, aMap.Size.Height * nb2)

            If nb = 3 Then
                nb = -1
                nb2 += 1
            End If
            nb += 1
        Next
    End Sub

    Private Sub ToolStripButton19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_SortGfxID.Click
        TrieurGfx.Show()
    End Sub

#End Region

#Region " Speedbar left "
    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Flip.Click
        ChangeFlip()
    End Sub

    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Rotate.Click
        ChangeRotate()
    End Sub

#Region " Calques "
    ' Actif
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Calque1.Click
        Calque = 1
        BT_Calque1.Checked = True
        BT_Calque2.Checked = False
    End Sub

    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Calque2.Click
        Calque = 2
        BT_Calque1.Checked = False
        BT_Calque2.Checked = True
    End Sub

    ' Affichage
    Private Sub FondToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FondToolStripMenuItem.Click
        If Show_Background = False Then Show_Background = True Else Show_Background = False
        RefreshAllMaps()
    End Sub

    Private Sub SolToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SolToolStripMenuItem.Click
        If Show_Ground = False Then Show_Ground = True Else Show_Ground = False
        RefreshAllMaps()
    End Sub

    Private Sub Calque1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calque1ToolStripMenuItem.Click
        If Show_Calque1 = False Then Show_Calque1 = True Else Show_Calque1 = False
        RefreshAllMaps()
    End Sub

    Private Sub Calque2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calque2ToolStripMenuItem.Click
        If Show_Calque2 = False Then Show_Calque2 = True Else Show_Calque2 = False
        RefreshAllMaps()
    End Sub

    Private Sub GrilleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrilleToolStripMenuItem.Click
        If Show_Grid = False Then Show_Grid = True Else Show_Grid = False
        RefreshAllMaps()
    End Sub

    Private Sub IDDesCellulesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IDDesCellulesToolStripMenuItem.Click
        If Show_CellID = False Then Show_CellID = True Else Show_CellID = False
        RefreshAllMaps()
    End Sub

#End Region

#Region " Types de cellule "

    Private Sub AucunToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_ModeConstruction.Click
        Outil = Tools.Brush
        CelluleModeMenuItem_NoChecked()
        BT_ModeConstruction.Checked = True
        BT_ModeSelect.Checked = False
        aCellMode = CellMode.Null
        RefreshAllMaps()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_ModeSelect.Click
        Outil = Tools.Selector
        CelluleModeMenuItem_NoChecked()
        BT_ModeConstruction.Checked = False
        BT_ModeSelect.Checked = True
        aCellMode = CellMode.Null
        RefreshAllMaps()
    End Sub

    Private Sub NonMarchableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Unwalkable.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_Unwalkable.Checked = True
        aCellMode = CellMode.Unwalkable
        RefreshAllMaps()
    End Sub

    Private Sub LigneDeVueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_LoS.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_LoS.Checked = True
        aCellMode = CellMode.LoS
        RefreshAllMaps()
    End Sub

    Private Sub CheminToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Chemin.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_Chemin.Checked = True
        aCellMode = CellMode.Path
        RefreshAllMaps()
    End Sub

    Private Sub CelluleEnclosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Paddock.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_Paddock.Checked = True
        aCellMode = CellMode.Paddock
        RefreshAllMaps()
    End Sub

    Private Sub CelluleDeCombat1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_FightCell1.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_FightCell1.Checked = True
        aCellMode = CellMode.Fight1
        RefreshAllMaps()
    End Sub

    Private Sub CelluleDeCombat2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_FightCell2.Click
        Outil = Tools.CellMode
        CelluleModeMenuItem_NoChecked()
        BT_FightCell2.Checked = True
        aCellMode = CellMode.Fight2
        RefreshAllMaps()
    End Sub

    Private Sub CelluleModeMenuItem_NoChecked()
        BT_ModeConstruction.Checked = False
        BT_Unwalkable.Checked = False
        BT_LoS.Checked = False
        BT_Chemin.Checked = False
        BT_Paddock.Checked = False
        BT_FightCell1.Checked = False
        BT_FightCell2.Checked = False
        BT_ModeSelect.Checked = False
    End Sub

#End Region

#End Region

#End Region

#Region " Tiles "
    Private WithEvents LoadTiles As New BackgroundWorker()
    Private varLoadTiles_RunWorkerCompleted As Boolean = False
    Private varLoadTiles_DoWork As Boolean = False

    Private Sub LoadTiles_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles LoadTiles.DoWork

        If varLoadTiles_DoWork = True Then
            varLoadTiles_DoWork = False
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
            e.Result = LoadTiles_Treatment(worker, e)
        End If
    End Sub

    Private Function LoadTiles_Treatment(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs) As ImageList

        Dim imageListLarge As New ImageList()
        imageListLarge.ImageSize = New Size(30, 30)
        imageListLarge.ColorDepth = ColorDepth.Depth32Bit

        Dim ListTiles() As Tile = Nothing
        If TreeView1.SelectedNode.FullPath.Contains("Sols") Then
            ListTiles = List_Grounds
        ElseIf TreeView1.SelectedNode.FullPath.Contains("Objets") Then
            ListTiles = List_Objects
        End If

        For Each ATile As Tile In ListTiles
            If IsNothing(ATile) Then Continue For
            If ATile.Folder = TreeView1.SelectedNode.Text Then
                imageListLarge.Images.Add(ATile.Image())
            End If
        Next

        Return imageListLarge
    End Function

    Private Sub LoadTiles_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles LoadTiles.RunWorkerCompleted
        If varLoadTiles_RunWorkerCompleted = True Then
            varLoadTiles_RunWorkerCompleted = False
            If Not (e.Error Is Nothing) Then
                MsgBox("Une erreur est survenue ! Détails : " & vbCrLf & e.Error.ToString)
            Else
                ListView1.LargeImageList = e.Result
                Panel1.Refresh()
            End If
            ToolStripStatusLabel1.Text = "Prêt"
            TreeView1.Enabled = True
        End If
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs)
        If TreeView1.SelectedNode.Level = 0 Then Exit Sub
        If Not LoadTiles.IsBusy Then
            ToolStripStatusLabel1.Text = "Chargement des images """ & TreeView1.SelectedNode.Text & """ en cours..."

            If Not IsNothing(ListView1.LargeImageList) Then ListView1.LargeImageList.Dispose()
            ListView1.Items.Clear()

            TreeView1.Enabled = False

            Dim ListTiles() As Tile = Nothing
            If TreeView1.SelectedNode.FullPath.Contains("Sols") Then
                ListTiles = List_Grounds
            ElseIf TreeView1.SelectedNode.FullPath.Contains("Objets") Then
                ListTiles = List_Objects
            End If

            Dim i As Integer = 0
            For Each ATile As Tile In ListTiles
                If IsNothing(ATile) Then Continue For
                If ATile.Folder = TreeView1.SelectedNode.Text Then
                    ListView1.Items.Add(ATile.ID.ToString, i)
                    i += 1
                End If
            Next

            varLoadTiles_RunWorkerCompleted = True
            varLoadTiles_DoWork = True
            LoadTiles.RunWorkerAsync()
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 0 Then Exit Sub
        If TreeView1.SelectedNode.FullPath.Contains("Sols") Then
            SelectedTile = List_Grounds(ListView1.SelectedItems(0).Text)
        ElseIf TreeView1.SelectedNode.FullPath.Contains("Objets") Then
            SelectedTile = List_Objects(ListView1.SelectedItems(0).Text)
        End If
        If Outil = Tools.CellMode Then
            Outil = Tools.Brush
            CelluleModeMenuItem_NoChecked()
            BT_ModeConstruction.Checked = True
            aCellMode = CellMode.Null
            RefreshAllMaps()
        End If
    End Sub

    Private Sub ListView1_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.MouseEnter
        ListView1.Focus()
        If Options.MyOptions.ResizePanel Then Panel1.Size = New Size(Panel1.Size.Width, Options.MyOptions.NewSizePanel)
    End Sub

    Private Sub TreeView1_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TreeView1.MouseEnter
        TreeView1.Focus()
        If Options.MyOptions.ResizePanel Then Panel1.Size = New Size(Panel1.Size.Width, Options.MyOptions.NewSizePanel)
    End Sub

    Private Sub ListView1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.Leave, TreeView1.Leave
        If Not TreeView1.Focused AndAlso Not ListView1.Focused Then If Options.MyOptions.ResizePanel Then Panel1.Size = New Size(Panel1.Size.Width, Options.MyOptions.SizePanel)
    End Sub

#End Region

#Region " Chargement des images "
    Private WithEvents LoadImages As New BackgroundWorker()
    Private RunWorkerCompleted As Boolean = False
    Private DoWork As Boolean = False
    Public List_Backgrounds(100000) As Tile
    Public List_Grounds(100000) As Tile
    Public List_Objects(100000) As Tile
    Private WithEvents TreeView1 As TreeView

    Private Sub LoadImages_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles LoadImages.DoWork
        If DoWork = True Then
            DoWork = False
            Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
            e.Result = Treatment(worker, e)
        End If
    End Sub

    Private Function Treatment(ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs) As Object()
        Dim MyTreeView As New TreeView
        With MyTreeView
            .Dock = DockStyle.Left
            .Size = New Size(160, 181)
            .Nodes.Add("Grounds")
            .Nodes(0).Text = "Sols"
            .Nodes.Add("Objects")
            .Nodes(1).Text = "Objets"
        End With
        Dim MyList_Backgrounds(100000) As Tile
        Dim MyList_Grounds(100000) As Tile
        Dim MyList_Objects(100000) As Tile

        ' Vérification de l'existence des dossiers
        If Not Directory.Exists(DirectoryApply & "/Images") Then
            MsgBox("Impossible de trouver le dossier des images.", MsgBoxStyle.OkOnly, Nothing)
            End
        End If
        If Not Directory.Exists(DirectoryApply & "/Images/backgrounds") Then
            MsgBox("Impossible de trouver le dossier des fonds.", MsgBoxStyle.OkOnly, Nothing)
            End
        End If
        If Not Directory.Exists(DirectoryApply & "/Images/grounds") Then
            MsgBox("Impossible de trouver le dossier des sols.", MsgBoxStyle.OkOnly, Nothing)
            End
        End If
        If Not Directory.Exists(DirectoryApply & "/Images/objects") Then
            MsgBox("Impossible de trouver le dossier des objets.", MsgBoxStyle.OkOnly, Nothing)
            End
        End If

        worker.ReportProgress(0, "Chargement des backgrounds...")
        Dim files As FileInfo() = New DirectoryInfo(DirectoryApply & "/Images/backgrounds").GetFiles
        For Each info4 As FileInfo In files
            If info4.Extension = ".jpg" Or info4.Extension = ".png" Or info4.Extension = ".jpeg" Or info4.Extension = ".bmp" Then
                Dim id_str As String = info4.Name.Split(New Char() {"."c})(0)
                If Not IsNumeric(id_str) Then
                    If MsgBox("Nom de fichier invalide : " & info4.FullName & vbCrLf & vbCrLf & "Voulez-vous le supprimer ?", MsgBoxStyle.YesNo, "Nom de fichier invalide") = MsgBoxResult.Yes Then
                        IO.File.Delete(info4.FullName)
                    End If
                Else
                    Dim id As Integer = CInt(id_str)
                    Try
                        MyList_Backgrounds(id) = New Tile(id, info4.FullName, "", Tile.TileType.Background)
                    Catch ex As Exception
                        MsgBox("Le fond " & id & " est présent deux fois.", MsgBoxStyle.OkOnly, "Background double")
                    End Try
                End If
            End If
        Next

        worker.ReportProgress(0, "Chargement des sols...")
        SearchGrounds(DirectoryApply & "/Images/grounds", MyTreeView.Nodes(0), MyList_Grounds)

        worker.ReportProgress(0, "Chargement des objets...")
        SearchObjects(DirectoryApply & "/Images/objects", MyTreeView.Nodes(1), MyList_Objects)

        Return {MyTreeView, MyList_Backgrounds, MyList_Grounds, MyList_Objects}
    End Function

    Private Sub SearchGrounds(ByVal directory As String, ByRef Node As TreeNode, ByRef MyList_Grounds() As Tile)
        Dim InfosDirectory1 As New DirectoryInfo(directory)

        Dim NewNode As New TreeNode()
        If Not directory = DirectoryApply & "/Images/grounds" Then
            NewNode.Text = InfosDirectory1.Name
            Node.Nodes.Add(NewNode)
        Else
            NewNode = Node
        End If

        For Each FileName As FileInfo In InfosDirectory1.GetFiles
            If FileName.Extension = ".jpg" Or FileName.Extension = ".png" Or FileName.Extension = ".jpeg" Or FileName.Extension = ".bmp" Then
                Dim id_str As String = FileName.Name.Split(New Char() {"."c})(0)
                If IsNumeric(id_str) Then
                    Dim id As Integer = CInt(id_str)
                    Try
                        MyList_Grounds(id) = New Tile(id, FileName.FullName, InfosDirectory1.Name, Tile.TileType.Ground)
                    Catch ex As Exception
                        MsgBox("Le sol " & id & " est présent deux fois.", MsgBoxStyle.OkOnly, "Sol double")
                    End Try
                Else
                    If MsgBox("Nom de fichier invalide : " & FileName.FullName & vbCrLf & vbCrLf & "Voulez-vous le supprimer ?", MsgBoxStyle.YesNo, "Nom de fichier invalide") = MsgBoxResult.Yes Then
                        IO.File.Delete(FileName.FullName)
                    End If
                End If
            End If
        Next

        For Each DirectoryName As DirectoryInfo In InfosDirectory1.GetDirectories
            SearchGrounds(directory & "/" & DirectoryName.Name, NewNode, MyList_Grounds)
        Next
    End Sub

    Private Sub SearchObjects(ByVal directory As String, ByRef Node As TreeNode, ByRef MyList_Objects() As Tile)
        Dim InfosDirectory1 As New DirectoryInfo(directory)

        Dim NewNode As New TreeNode()
        If Not directory = DirectoryApply & "/Images/objects" Then
            NewNode.Text = InfosDirectory1.Name
            Node.Nodes.Add(NewNode)
        Else
            NewNode = Node
        End If

        For Each FileName As FileInfo In InfosDirectory1.GetFiles
            If FileName.Extension = ".jpg" Or FileName.Extension = ".png" Or FileName.Extension = ".jpeg" Or FileName.Extension = ".bmp" Then
                Dim id_str As String = FileName.Name.Split(New Char() {"."c})(0)
                If Not IsNumeric(id_str) Then
                    If MsgBox("Nom de fichier invalide : " & FileName.FullName & vbCrLf & vbCrLf & "Voulez-vous le supprimer ?", MsgBoxStyle.YesNo, "Nom de fichier invalide") = MsgBoxResult.Yes Then
                        IO.File.Delete(FileName.FullName)
                    End If
                Else
                    Dim id As Integer = CInt(id_str)
                    Try
                        MyList_Objects(id) = New Tile(id, FileName.FullName, InfosDirectory1.Name, Tile.TileType.Objet)
                    Catch ex As Exception
                        MsgBox("L'objet " & id & " est présent deux fois.", MsgBoxStyle.OkOnly, "Objet double")
                    End Try
                End If
            End If
        Next

        For Each DirectoryName As DirectoryInfo In InfosDirectory1.GetDirectories
            SearchObjects(directory & "/" & DirectoryName.Name, NewNode, MyList_Objects)
        Next
    End Sub

    Private Sub LoadImages_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles LoadImages.ProgressChanged
        ToolStripStatusLabel1.Text = e.UserState.ToString
    End Sub

    Private Sub LoadImages_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles LoadImages.RunWorkerCompleted
        If RunWorkerCompleted = True Then
            RunWorkerCompleted = False
            If Not (e.Error Is Nothing) Then
                MsgBox("Une erreur est survenue ! Détails : " & vbCrLf & e.Error.ToString)
            Else
                Panel1.Controls.Add(e.Result(0))
                TreeView1 = Panel1.Controls(1)
                AddHandler TreeView1.AfterSelect, AddressOf TreeView1_AfterSelect
                ListView1.Visible = True
                List_Backgrounds = e.Result(1)
                List_Grounds = e.Result(2)
                List_Objects = e.Result(3)
            End If
            ToolStripStatusLabel1.Text = "Prêt"
            Launch()
        End If
    End Sub

#End Region

#Region " Fonctions "
    Public Sub RefreshAllMaps()
        For Each aMap As MapEditor In Me.ListOfMapEditors
            aMap.DrawAll()
        Next
    End Sub

    Public Sub ChangeRotate()
        SelectedRotate = (SelectedRotate + 1) Mod 4
    End Sub

    Public Sub ChangeFlip()
        SelectedFlip = Not SelectedFlip
        If SelectedFlip Then
            BT_Flip.Image = My.Resources.gauche
        Else
            BT_Flip.Image = My.Resources.droite
        End If
    End Sub

    Public Sub OpenAMEProject(ByVal FileName As String)
        Dim FluxDeFichier As Stream = File.OpenRead(FileName)
        Dim Deserialiseur As New BinaryFormatter()
        Dim aMap As Map = CType(Deserialiseur.Deserialize(FluxDeFichier), Map)
        FluxDeFichier.Close()

        OpenProject(aMap)
    End Sub

    Public Sub OpenSWFProject(ByVal FileName As String, Optional ByVal key As String = "")
        Dim aMap As Map = Unpacker.UnPackerSwf(FileName)
        If IsNothing(aMap) Then
            MsgBox("Le fichier SWF sélectionné est illisible.")
            Exit Sub
        End If

        aMap.Key = key
        OpenProject(aMap)
    End Sub

    Public Sub OpenProject(ByVal aMap As Map)
        If Not Map.IdWasLoaded(aMap) Then
            Map.AddMap(aMap)
        Else
            aMap = Map.GetByID(aMap.ID)
        End If

        If aMap.IsEditing Then
            MsgBox("Cette map est déjà en cours d'édition.")
        Else
            Dim ChildForm As New MapEditor(aMap)
            ChildForm.SizeTable = New Size(aMap.Width, aMap.Height)
            ChildForm.MdiParent = Me
            m_ChildFormNumber += 1
            ChildForm.Text = "Map " & ChildForm.MyDatas.ID
            ChildForm.Show()

            ListOfMapEditors.Add(ChildForm)

            SelectedMap = ChildForm
            MenuMap_RefreshControls()
        End If
    End Sub

    Public Sub OpenNewProject(ByVal width As Integer, ByVal height As Integer)
        Dim ChildForm As New MapEditor
        ChildForm.SizeTable = New Size(width, height)
        ChildForm.MdiParent = Me
        m_ChildFormNumber += 1
        ChildForm.Text = "Map " & m_ChildFormNumber
        ChildForm.Show()
        ListOfMapEditors.Add(ChildForm)
    End Sub

#End Region

#Region " Menu droite "

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.MouseEnter
        TabControl1.Focus()
    End Sub

#Region " Map "
    Public Sub MenuMap_RefreshControls()
        ChangingCheckBoxState2 = True
        If IsNothing(SelectedMap) Then
            KryptonNumericUpDown2.Value = 1
            KryptonTextBox2.Text = "AME"
            KryptonNumericUpDown1.Value = 0
            PictureBox4.BackgroundImage = Nothing
            KryptonLabel11.Text = "Aucune"
            KryptonNumericUpDown3.Value = 17
            KryptonNumericUpDown4.Value = 15
            KryptonNumericUpDown5.Value = 5
            KryptonNumericUpDown6.Value = 6
            KryptonNumericUpDown9.Value = 0
            ComboBox_Areas.Text = ""
            ComboBox_SubAreas.Text = ""
            KryptonNumericUpDown8.Value = 0
            KryptonNumericUpDown7.Value = 0
            KryptonCheckedListBox2.SetItemChecked(0, False)
            KryptonCheckedListBox2.SetItemChecked(1, False)
            KryptonCheckedListBox2.SetItemChecked(2, False)
            KryptonCheckedListBox2.SetItemChecked(3, False)
            KryptonCheckedListBox2.SetItemChecked(4, False)
        Else
            KryptonNumericUpDown2.Value = SelectedMap.MyDatas.ID
            KryptonTextBox2.Text = SelectedMap.MyDatas.DateMap
            KryptonNumericUpDown1.Value = SelectedMap.MyDatas.Ambiance
            If Not IsNothing(SelectedMap.MyDatas.Background) Then
                PictureBox4.BackgroundImage = SelectedMap.MyDatas.Background.Image
            Else
                PictureBox4.BackgroundImage = Nothing
            End If
            If Not SelectedMap.MyDatas.MusiqueName = "" Then
                KryptonLabel11.Text = SelectedMap.MyDatas.MusiqueName
            Else
                KryptonLabel11.Text = "Aucune"
            End If
            KryptonNumericUpDown3.Value = SelectedMap.MyDatas.Height
            KryptonNumericUpDown4.Value = SelectedMap.MyDatas.Width
            KryptonNumericUpDown5.Value = SelectedMap.MyDatas.NbGroups
            KryptonNumericUpDown6.Value = SelectedMap.MyDatas.GroupMaxSize
            KryptonNumericUpDown9.Value = SelectedMap.MyDatas.SuperArea
            If Not IsNothing(Area.GetByID(SelectedMap.MyDatas.Area)) Then ComboBox_Areas.Text = Area.GetByID(SelectedMap.MyDatas.Area).Name
            If Not IsNothing(SubArea.GetByID(SelectedMap.MyDatas.SubArea)) Then ComboBox_SubAreas.Text = SubArea.GetByID(SelectedMap.MyDatas.SubArea).Name
            KryptonNumericUpDown8.Value = SelectedMap.MyDatas.X
            KryptonNumericUpDown7.Value = SelectedMap.MyDatas.Y

            KryptonCheckedListBox2.SetItemChecked(0, SelectedMap.MyDatas.BOutdoor)
            KryptonCheckedListBox2.SetItemChecked(1, (SelectedMap.MyDatas.Capabilities & 1))
            KryptonCheckedListBox2.SetItemChecked(2, ((SelectedMap.MyDatas.Capabilities >> 1) & 1))
            KryptonCheckedListBox2.SetItemChecked(3, ((SelectedMap.MyDatas.Capabilities >> 2) & 1))
            KryptonCheckedListBox2.SetItemChecked(4, ((SelectedMap.MyDatas.Capabilities >> 3) & 1))
        End If
        ChangingCheckBoxState2 = False
    End Sub

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton1.Click
        '  Background
        If IsNothing(SelectedMap) Then Exit Sub
        Dim PanelBackground As New BackgroundManager(SelectedMap)
        PanelBackground.Show()
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' Delete background
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.DrawBackground(Nothing)
    End Sub

    Private Sub KryptonNumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown2.ValueChanged
        ' ID
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.ID = KryptonNumericUpDown2.Value
    End Sub

    Private Sub KryptonTextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonTextBox2.TextChanged
        ' Map's Name
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.DateMap = KryptonTextBox2.Text
    End Sub

    Private Sub KryptonNumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown1.ValueChanged
        ' Ambiance
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.Ambiance = KryptonNumericUpDown1.Value
    End Sub

    Private Sub KryptonButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton2.Click
        ' Music
        If IsNothing(SelectedMap) Then Exit Sub
        Dim PanelMusic As New MusicManager(SelectedMap)
        PanelMusic.Show()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ' Delete music
        SelectedMap.MyDatas.Musique = 0
        SelectedMap.MyDatas.MusiqueName = "Aucune"
        MenuMap_RefreshControls()
    End Sub

    Private Sub KryptonNumericUpDown5_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown5.ValueChanged
        ' Nb Groups
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.NbGroups = KryptonNumericUpDown5.Value
    End Sub

    Private Sub KryptonNumericUpDown6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown6.ValueChanged
        ' GroupMaxSize
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.GroupMaxSize = KryptonNumericUpDown6.Value
    End Sub

    Private Sub KryptonButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton3.Click
        ' Ouvrir dans l'explorateur Windows
        If IsNothing(SelectedMap) Then Exit Sub
        If Not Directory.Exists(DirectoryApply & "\Maps\" & SelectedMap.MyDatas.ID) Then SelectedMap.Save()
        Process.Start(DirectoryApply & "\Maps\" & SelectedMap.MyDatas.ID)
    End Sub

    Private Sub KryptonButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton4.Click
        ' Exportation BDD
        If IsNothing(SelectedMap) Then Exit Sub
        MySQL.Execute(MySQL.Get_SqlMap(SelectedMap.MyDatas))
        MsgBox("La map a été inséré dans la base de données.")
    End Sub

    Private Sub KryptonNumericUpDown9_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown9.ValueChanged
        ' SuperArea
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.SuperArea = KryptonNumericUpDown9.Value
    End Sub

    Private Sub ComboBox_Areas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Areas.SelectedIndexChanged
        ' Area
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.Area = Area.GetByName(ComboBox_Areas.Text).ID
    End Sub

    Private Sub ComboBox_SubAreas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_SubAreas.SelectedIndexChanged
        ' SubArea
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.SubArea = SubArea.GetByName(ComboBox_SubAreas.Text).ID
    End Sub

    Private Sub KryptonNumericUpDown8_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown8.ValueChanged
        ' X
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.X = KryptonNumericUpDown8.Value
    End Sub

    Private Sub KryptonNumericUpDown7_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonNumericUpDown7.ValueChanged
        ' Y
        If IsNothing(SelectedMap) Then Exit Sub
        SelectedMap.MyDatas.Y = KryptonNumericUpDown7.Value
    End Sub

    Private Sub Rights_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonCheckedListBox2.ItemCheck
        If IsNothing(SelectedMap) Or ChangingCheckBoxState2 Then Exit Sub
        If KryptonCheckedListBox2.SelectedIndex = 0 Then
            SelectedMap.MyDatas.BOutdoor = KryptonCheckedListBox2.GetItemChecked(0)
        Else
            SelectedMap.MyDatas.Capabilities = Map.Get_Capabilities(Not KryptonCheckedListBox2.GetItemChecked(1), Not KryptonCheckedListBox2.GetItemChecked(2), Not KryptonCheckedListBox2.GetItemChecked(3), Not KryptonCheckedListBox2.GetItemChecked(4))
        End If
    End Sub

#End Region

#Region " Cellule "

#Region " Monster "

    Private Sub BT_PlaceMonsters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_PlaceMonsters.Click
        If IsNothing(SelectedMap) Then Exit Sub
        Dim aMobGroup As New MobGroup(SelectedMap.MyDatas.Mobs)
        aMobGroup.ShowDialog()
        If aMobGroup.Mobs = "" Then
            SelectedMap.MyDatas.Mobs = ""
            aMobGroup.Dispose()
            Exit Sub
        End If
        Dim Mobs As String = aMobGroup.Mobs
        aMobGroup.Dispose()
        SelectedMap.MyDatas.Mobs = Mobs
    End Sub

    Private Sub BT_Place_MonstersGroupFix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Place_MonstersGroupFix.Click
        If IsNothing(SelectedMap) Then Exit Sub
        Dim aMobGroup As New MobGroup(SelectedMap.MyDatas.GroupFixe_Mobs)
        aMobGroup.ShowDialog()
        If aMobGroup.Mobs = "" Then
            aMobGroup.Dispose()
            SelectedMap.MyDatas.GroupFixe_Mobs = ""
            SelectedMap.MyDatas.GroupFixe_Cell = 0
            Exit Sub
        End If
        Dim Mobs As String = aMobGroup.Mobs
        aMobGroup.Dispose()
        SelectedMap.MyDatas.Mobs = ""
        SelectedMap.MyDatas.GroupFixe_Mobs = Mobs
        SelectedMap.MyDatas.GroupFixe_Cell = SelectedMap.SelectedCell
        MsgBox("Le groupe de monstre a été défini sur la cellule n°" & SelectedMap.SelectedCell & ".")
    End Sub

#End Region

    Public Sub Main_MenuCell_RefreshInfos()
        ChangingCheckBoxState = True
        If IsNothing(SelectedMap) Then
            KryptonLabel1.Text = "Cellule"
            PictureBox1.BackgroundImage = Nothing
            PictureBox2.BackgroundImage = Nothing
            PictureBox3.BackgroundImage = Nothing
            KryptonCheckedListBox1.SetItemCheckState(0, False)
            KryptonCheckedListBox1.SetItemCheckState(1, False)
            KryptonCheckedListBox1.SetItemCheckState(2, False)
            KryptonCheckedListBox1.SetItemCheckState(3, False)
            KryptonCheckedListBox1.SetItemCheckState(4, False)
            KryptonCheckedListBox1.SetItemCheckState(5, False)
            KryptonCheckedListBox1.SetItemCheckState(6, False)
            KryptonCheckedListBox1.Enabled = False
            KryptonTextBox1.Text = ""
            TXT_ID_Sol.Text = ""
            TXT_ID_GFX2.Text = ""
            TXT_ID_GFX3.Text = ""
            BT_Del_Sol.Visible = False
            BT_Del_Gfx2.Visible = False
            BT_Del_Gfx3.Visible = False
            NB_GroundLevel.Value = 0
            NB_GroundSlope.Value = 0
        Else
            KryptonLabel1.Text = "Cellule n°" & SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).ID
            '       Reset
            PictureBox1.BackgroundImage = Nothing
            PictureBox2.BackgroundImage = Nothing
            PictureBox3.BackgroundImage = Nothing
            TXT_ID_Sol.Text = ""
            TXT_ID_GFX2.Text = ""
            TXT_ID_GFX3.Text = ""
            BT_Del_Sol.Visible = False
            BT_Del_Gfx2.Visible = False
            BT_Del_Gfx3.Visible = False
            '       Modification
            If Not IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx1) Then
                PictureBox1.BackgroundImage = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx1.Image
                TXT_ID_Sol.Text = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx1.ID
                BT_Del_Sol.Visible = True
            End If
            If Not IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx2) Then
                PictureBox2.BackgroundImage = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx2.Image
                TXT_ID_GFX2.Text = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx2.ID
                BT_Del_Gfx2.Visible = True
            End If
            If Not IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx3) Then
                PictureBox3.BackgroundImage = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx3.Image
                TXT_ID_GFX3.Text = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx3.ID
                BT_Del_Gfx3.Visible = True
            End If

            KryptonCheckedListBox1.Enabled = True
            KryptonCheckedListBox1.SetItemChecked(0, Not SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).UnWalkable)
            KryptonCheckedListBox1.SetItemChecked(1, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).LoS)
            KryptonCheckedListBox1.SetItemChecked(2, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Path)
            KryptonCheckedListBox1.SetItemChecked(3, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Paddock)
            KryptonCheckedListBox1.SetItemChecked(4, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Door)
            KryptonCheckedListBox1.SetItemChecked(5, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).TriggerCell)
            KryptonCheckedListBox1.SetItemChecked(6, SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).IO)

            NB_GroundLevel.Value = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).NivSol
            NB_GroundSlope.Value = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).InclineSol

            KryptonTextBox1.Text = SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).GetDatas
        End If
        ChangingCheckBoxState = False
    End Sub

#Region " Suppression calques "
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Del_Sol.Click
        If IsNothing(SelectedMap) Then Exit Sub
        ' Suppression calque 1
        SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx1 = Nothing
        PictureBox1.BackgroundImage = Nothing
        TXT_ID_Sol.Text = ""
        BT_Del_Sol.Visible = False
        SelectedMap.DrawAll()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Del_Gfx2.Click
        If IsNothing(SelectedMap) Then Exit Sub
        ' Suppression calque 2
        SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx2 = Nothing
        PictureBox2.BackgroundImage = Nothing
        TXT_ID_GFX2.Text = ""
        BT_Del_Gfx2.Visible = False
        SelectedMap.DrawAll()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Del_Gfx3.Click
        If IsNothing(SelectedMap) Then Exit Sub
        ' Suppression calque 3
        SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx3 = Nothing
        PictureBox3.BackgroundImage = Nothing
        TXT_ID_GFX3.Text = ""
        BT_Del_Gfx3.Visible = False
        SelectedMap.DrawAll()
    End Sub
#End Region

#Region " CheckBox types cases "
    Private Sub IO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonCheckedListBox1.ItemCheck
        If IsNothing(SelectedMap) Then Exit Sub
        If Not ChangingCheckBoxState Then
            Select Case KryptonCheckedListBox1.SelectedIndex
                Case 0
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).UnWalkable = KryptonCheckedListBox1.GetItemChecked(0)
                Case 1
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).LoS = Not KryptonCheckedListBox1.GetItemChecked(1)
                Case 2
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Path = Not KryptonCheckedListBox1.GetItemChecked(2)
                Case 3
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Paddock = Not KryptonCheckedListBox1.GetItemChecked(3)
                Case 4
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Door = Not KryptonCheckedListBox1.GetItemChecked(4)
                Case 5
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).TriggerCell = Not KryptonCheckedListBox1.GetItemChecked(5)
                Case 6
                    SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).IO = Not KryptonCheckedListBox1.GetItemChecked(6)
            End Select
            SelectedMap.DrawAll()
        End If
    End Sub
#End Region

#Region " ContextMenu "
    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click
        If IsNothing(SelectedMap) Then Exit Sub
        If IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx1) Then Exit Sub
        SelectedCalque_In_CellMenu = 1
        ContextMenuStrip1.Show(Cursor.Position)
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click
        If IsNothing(SelectedMap) Then Exit Sub
        If IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx2) Then Exit Sub
        SelectedCalque_In_CellMenu = 2
        ContextMenuStrip1.Show(Cursor.Position)
    End Sub

    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click
        If IsNothing(SelectedMap) Then Exit Sub
        If IsNothing(SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).Gfx3) Then Exit Sub
        SelectedCalque_In_CellMenu = 3
        ContextMenuStrip1.Show(Cursor.Position)
    End Sub

    Private Sub SupprimerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupprimerToolStripMenuItem.Click
        SelectedMap.Delete_Tile(SelectedCalque_In_CellMenu)
        Main_MenuCell_RefreshInfos()
    End Sub

    Private Sub SupprimerTousCesTilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupprimerTousCesTilesToolStripMenuItem.Click
        SelectedMap.DeleteAll_ThisTile(SelectedCalque_In_CellMenu)
        Main_MenuCell_RefreshInfos()
    End Sub

    Private Sub OkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkToolStripMenuItem.Click
        If Not IsNumeric(ToolStripTextBox1.Text) Then
            MsgBox("""" & ToolStripTextBox1.Text & """ n'est pas un identifiant valide.")
            Exit Sub
        End If
        SelectedMap.ReplaceAll_ThisTile_By(SelectedCalque_In_CellMenu, CInt(ToolStripTextBox1.Text))
        Main_MenuCell_RefreshInfos()
    End Sub

    Private Sub DeplacerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DéplacerToolStripMenuItem.Click
        SelectedMap.Move_Tile(SelectedCalque_In_CellMenu)
        Main_MenuCell_RefreshInfos()
    End Sub
#End Region

#Region " Numeric Sol "
    Private Sub NB_GroundLevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NB_GroundLevel.ValueChanged
        If IsNothing(SelectedMap) Then Exit Sub
        If Not ChangingCheckBoxState Then SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).NivSol = NB_GroundLevel.Value
    End Sub

    Private Sub NB_GroundSlope_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NB_GroundSlope.ValueChanged
        If IsNothing(SelectedMap) Then Exit Sub
        If Not ChangingCheckBoxState Then SelectedMap.MyDatas.Cells(SelectedMap.SelectedCell).InclineSol = NB_GroundSlope.Value
    End Sub
#End Region

#End Region

#End Region

#Region " Préférence design "
    Private Sub ResetToolStripDesign()
        PofessionalSystemToolStripMenuItem.Checked = False
        Office2007BlueToolStripMenuItem.Checked = False
        Office2007SilverToolStripMenuItem.Checked = False
        Office2007BlackToolStripMenuItem.Checked = False
        Office2010BlueToolStripMenuItem1.Checked = False
        Office2010BlueToolStripMenuItem.Checked = False
        Office2010SiToolStripMenuItem.Checked = False
        SparkleBlueToolStripMenuItem1.Checked = False
        SparkleOrangeToolStripMenuItem.Checked = False
        SparkleBlueToolStripMenuItem.Checked = False
    End Sub

    Private Sub PofessionalSystemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PofessionalSystemToolStripMenuItem.Click
        ResetToolStripDesign()
        PofessionalSystemToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.ProfessionalSystem
    End Sub

    Private Sub Office2007BlueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2007BlueToolStripMenuItem.Click
        ResetToolStripDesign()
        Office2007BlueToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2007Blue
    End Sub

    Private Sub Office2007SilverToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2007SilverToolStripMenuItem.Click
        ResetToolStripDesign()
        Office2007SilverToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2007Silver
    End Sub

    Private Sub Office2007BlackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2007BlackToolStripMenuItem.Click
        ResetToolStripDesign()
        Office2007BlackToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2007Black
    End Sub

    Private Sub Office2010BlueToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2010BlueToolStripMenuItem1.Click
        ResetToolStripDesign()
        Office2010BlueToolStripMenuItem1.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Blue
    End Sub

    Private Sub Office2010BlueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2010BlueToolStripMenuItem.Click
        ResetToolStripDesign()
        Office2010BlueToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver
    End Sub

    Private Sub Office2010SiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2010SiToolStripMenuItem.Click
        ResetToolStripDesign()
        Office2010SiToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Black
    End Sub

    Private Sub SparkleBlueToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SparkleBlueToolStripMenuItem1.Click
        ResetToolStripDesign()
        SparkleBlueToolStripMenuItem1.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.SparkleBlue
    End Sub

    Private Sub SparkleOrangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SparkleOrangeToolStripMenuItem.Click
        ResetToolStripDesign()
        SparkleOrangeToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.SparkleOrange
    End Sub

    Private Sub SparkleBlueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SparkleBlueToolStripMenuItem.Click
        ResetToolStripDesign()
        SparkleBlueToolStripMenuItem.Checked = True
        KryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.SparklePurple
    End Sub
#End Region

    Friend Shared Function Get_HWID() As String
        For Each nic As NetworkInformation.NetworkInterface In NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
            If nic.NetworkInterfaceType = NetworkInformation.NetworkInterfaceType.Ethernet AndAlso nic.OperationalStatus = NetworkInformation.OperationalStatus.Up Then
                Dim macAdd = nic.GetPhysicalAddress().ToString()
                Dim macAddLen = nic.GetPhysicalAddress().ToString().Length

                Dim str As String = ""

                For i As Integer = 0 To macAddLen - 1 Step 2
                    str = String.Concat(str, "-", macAdd.Substring(i, 2))
                Next

                Return str.Substring(1)
            ElseIf nic.NetworkInterfaceType = NetworkInformation.NetworkInterfaceType.Wireless80211 AndAlso nic.OperationalStatus = NetworkInformation.OperationalStatus.Up Then
                Dim macAdd = nic.GetPhysicalAddress().ToString()
                Dim macAddLen = nic.GetPhysicalAddress().ToString().Length

                Dim str As String = ""

                For i As Integer = 0 To macAddLen - 1 Step 2
                    str = String.Concat(str, "-", macAdd.Substring(i, 2))
                Next

                Return str.Substring(1)
            End If
        Next
        Return Nothing
    End Function

End Class