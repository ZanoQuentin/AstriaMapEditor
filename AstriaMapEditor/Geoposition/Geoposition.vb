Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization
Imports System.Net

' Reste : 
'  - Tuiles de création de la géoposition IG

Public Class Geoposition

    ' Données de la map
    Public SizeCell As New Size(166, 100)
    Public SizeMap As New Size(8, 8)
    Public Cells() As CellGeo
    Dim y_PositionTopLeft As Integer = 0
    Dim x_PositionTopLeft As Integer = 0
    Dim BaseX As Integer = 0
    Dim BaseY As Integer = 0
    Dim aColor As Color = Color.Black

    ' Image
    Public MyImage As Bitmap
    Public G As Graphics

    ' Local
    Public SelectedCell As Integer = 0

    ' Configuation de l'île
    Public Island_Name As String = "None"

    Private Sub Geoposition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Taille
        Dim MyInput As New InputSizeMap
        MyInput.ShowDialog()
        SizeMap.Width = MyInput._Width
        SizeMap.Height = MyInput._Height
        x_PositionTopLeft = MyInput._X
        y_PositionTopLeft = MyInput._Y
        MyInput.Dispose()

        Me.KeyPreview = True

        Dim aCells(SizeMap.Width * SizeMap.Height - 1) As CellGeo
        Cells = aCells

        MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1, SizeCell.Height * SizeMap.Height + 1)
        G = Graphics.FromImage(MyImage)

        GenerateGrid()
        DrawAll()

        TXT_X.Text = MyInput._X
        TXT_Y.Text = MyInput._Y

        TXT_A.Items.AddRange(Area.Areas)
        TXT_SubA.Items.AddRange(SubArea.SubAreas)

        Cells(0).Draw_String(G, "Commandes clavier : " & vbCrLf & _
                             "Z : Monter" & vbCrLf & _
                             "S : Descendre" & vbCrLf & _
                             "Q : Aller à gauche" & vbCrLf & _
                             "D : Aller à droite" & vbCrLf & _
                             "N : Retour au point de départ" & vbCrLf _
                             , Brushes.WhiteSmoke)
    End Sub

    Private Sub OnClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim MsgBoxResulte As MsgBoxResult = MsgBox("Voulez-vous enregistrer les modifications apportées à la géoposition ?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Information, "Confirmation")
        If MsgBoxResulte = MsgBoxResult.Yes Then
            Save()
        ElseIf MsgBoxResulte = MsgBoxResult.Cancel Then
            e.Cancel = True
        End If
    End Sub

#Region " Grille "

#Region " No draw "
    Public Function Get_IdCell(ByVal pts As Point) As Integer
        ' Repérage d'une cellule
        Try
            Dim x As Integer = pts.X
            Dim y As Integer = pts.Y

            For Each cell As CellGeo In Cells
                Dim num6 As Integer = (((y - cell.Location(0).Y) * (cell.Location(1).X - cell.Location(0).X)) - ((x - cell.Location(0).X) * (cell.Location(1).Y - cell.Location(0).Y)))
                Dim num7 As Integer = (((y - cell.Location(1).Y) * (cell.Location(2).X - cell.Location(1).X)) - ((x - cell.Location(1).X) * (cell.Location(2).Y - cell.Location(1).Y)))
                Dim num4 As Integer = (((y - cell.Location(2).Y) * (cell.Location(3).X - cell.Location(2).X)) - ((x - cell.Location(2).X) * (cell.Location(3).Y - cell.Location(2).Y)))
                Dim num5 As Integer = (((y - cell.Location(3).Y) * (cell.Location(0).X - cell.Location(3).X)) - ((x - cell.Location(3).X) * (cell.Location(0).Y - cell.Location(3).Y)))
                If ((((num6 >= 0) And (num7 >= 0)) And (num4 >= 0)) And (num5 >= 0)) Then
                    Return cell.ID
                End If
            Next
        Catch ex As Exception
            MsgBox("Cell not found : " & ex.ToString)
        End Try
        Return -1
    End Function

    Public Sub GenerateGrid()
        Dim y As Integer = 0
        Dim x As Integer = 0
        ' Génére les cellule
        For i = 0 To Cells.Length - 1
            If IsNothing(Cells(i)) Then
                Dim aCellGeo As New CellGeo(Me, i)
                With aCellGeo
                    .x_pos = x_PositionTopLeft + x
                    .y_pos = y_PositionTopLeft + y
                    .Location(0) = New Point(x * SizeCell.Width + BaseX, y * SizeCell.Height + BaseY)
                    .Location(1) = New Point(x * SizeCell.Width + SizeCell.Width + BaseX, y * SizeCell.Height + BaseY)
                    .Location(2) = New Point(x * SizeCell.Width + SizeCell.Width + BaseX, y * SizeCell.Height + SizeCell.Height + BaseY)
                    .Location(3) = New Point(x * SizeCell.Width + BaseX, y * SizeCell.Height + SizeCell.Height + BaseY)
                End With
                Cells(i) = aCellGeo
            Else
                Cells(i).x_pos = x_PositionTopLeft + x
                Cells(i).y_pos = y_PositionTopLeft + y
                Cells(i).Location(0) = New Point(x * SizeCell.Width + BaseX, y * SizeCell.Height + BaseY)
                Cells(i).Location(1) = New Point(x * SizeCell.Width + SizeCell.Width + BaseX, y * SizeCell.Height + BaseY)
                Cells(i).Location(2) = New Point(x * SizeCell.Width + SizeCell.Width + BaseX, y * SizeCell.Height + SizeCell.Height + BaseY)
                Cells(i).Location(3) = New Point(x * SizeCell.Width + BaseX, y * SizeCell.Height + SizeCell.Height + BaseY)
            End If

            x += 1
            If x = SizeMap.Width Then
                y += 1
                x = 0
            End If
        Next

    End Sub
#End Region

    Public Sub DrawAll(Optional ByVal drawmode As Boolean = True)
        G.Clear(aColor)

        For Each MyCell As CellGeo In Cells
            If Not IsNothing(MyCell.MyMap) Then
                If Not IsNothing(MyCell.MyMap.Screenshot) Then
                    MyCell.DrawImage(G, MyCell.MyMap.Screenshot)
                Else
                    If drawmode Then MyCell.DrawBorder(G, Brushes.White)
                End If
            Else
                If drawmode Then MyCell.DrawBorder(G, Brushes.White)
            End If
            If drawmode Then MyCell.DrawGeoPos(G, Brushes.White)
        Next

        If drawmode Then
            For Each MyCell As CellGeo In Cells
                MyCell.DrawMode(G)
            Next
        End If

        ' Sauvegarde
        PictureBox1.Image = MyImage
    End Sub
#End Region

#Region " Souris "

    Private Sub PictureBox1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseEnter
        PictureBox1.Focus()
    End Sub

    Private Sub PictureBox1_Molette(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseWheel
        If SizeCell.Height >= 20 And SizeCell.Height <= 200 Then SizeCell.Height += CInt(e.Delta / 20)
        If SizeCell.Height < 20 Then SizeCell.Height = 20
        If SizeCell.Height > 200 Then SizeCell.Height = 200

        SizeCell.Width = CInt(1.66 * SizeCell.Height)

        MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
        G = Graphics.FromImage(MyImage)

        GenerateGrid()
        DrawAll()
    End Sub

    Private Sub PictureBox1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        Dim ID As Integer = Get_IdCell(PictureBox1.PointToClient(Control.MousePosition))

        If Not ID = -1 And Not ID = SelectedCell Then
            Cells(SelectedCell).Selected = False
            Cells(ID).Selected = True
            SelectedCell = ID
            RefreshDatas(ID)
            DrawAll()
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then ContextMenuStrip1.Show(Cursor.Position)

    End Sub


    Dim CanMoveMap As Boolean = False
    Dim MoveMapCoordonnees As Point = Nothing
    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        CanMoveMap = True
        MoveMapCoordonnees = e.Location
    End Sub
    Private Sub PictureBox1_MouseMouve(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove

        If CanMoveMap Then
            Me.Cursor = Windows.Forms.Cursors.NoMove2D

            If (BaseX + (e.X - MoveMapCoordonnees.X)) < -(SizeCell.Width * (SizeMap.Width - 1)) Or (BaseX + (e.X - MoveMapCoordonnees.X)) > SizeCell.Width * (SizeMap.Width - 1) Then Exit Sub
            If (BaseY + (e.Y - MoveMapCoordonnees.Y)) < -(SizeCell.Height * (SizeMap.Height - 1)) Or (BaseY + (e.Y - MoveMapCoordonnees.Y)) > SizeCell.Height * (SizeMap.Height - 1) Then Exit Sub

            BaseX += (e.X - MoveMapCoordonnees.X)
            BaseY += (e.Y - MoveMapCoordonnees.Y)
            MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
            G = Graphics.FromImage(MyImage)
            GenerateGrid()
            DrawAll()

            MoveMapCoordonnees = e.Location
        End If
    End Sub
    Private Sub PictureBox1_MouseUp() Handles PictureBox1.MouseUp
        CanMoveMap = False
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

#End Region

#Region " Clavier "

    Public Sub Map_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.D
                BaseX -= CInt(SizeCell.Width / 10)
                MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
                G = Graphics.FromImage(MyImage)
                GenerateGrid()
                DrawAll()
            Case Keys.Q
                BaseX += CInt(SizeCell.Width / 10)
                MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
                G = Graphics.FromImage(MyImage)
                GenerateGrid()
                DrawAll()
            Case Keys.S
                BaseY -= CInt(SizeCell.Height / 10)
                MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
                G = Graphics.FromImage(MyImage)
                GenerateGrid()
                DrawAll()
            Case Keys.Z
                BaseY += CInt(SizeCell.Height / 10)
                MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
                G = Graphics.FromImage(MyImage)
                GenerateGrid()
                DrawAll()
            Case Keys.N
                BaseY = 0
                BaseX = 0
                GenerateGrid()
                DrawAll()
        End Select
    End Sub

#End Region

#Region " Gestion cellule "
    Private Sub InsérerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsérerToolStripMenuItem.Click, PictureBox1.MouseDoubleClick
        If Not IsNothing(Cells(SelectedCell).MyMap) Then Exit Sub
        ' Select
        If IsNothing(Cells(SelectedCell).MyMap) Then
            Dim SelectMap As New SelectMap()
            SelectMap.ShowDialog()
            Cells(SelectedCell).MyMap = SelectMap.SelectedMap
            RefreshDatas(SelectedCell)
            DrawAll()
        End If
    End Sub

    Private Sub NouvelleMapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NouvelleMapToolStripMenuItem.Click
        If Not IsNothing(Cells(SelectedCell).MyMap) Then Exit Sub
        ' New
        Dim ChildForm As New MapEditor
        ChildForm.SizeTable = New Size(15, 17)
        ChildForm.MdiParent = Main
        Main.m_ChildFormNumber += 1
        ChildForm.Text = "Map " & Main.m_ChildFormNumber
        ChildForm.Show()
        Cells(SelectedCell).MyMap = ChildForm.MyDatas
        Map.AddMap(Cells(SelectedCell).MyMap)
        RefreshDatas(SelectedCell)
        DrawAll()
        Main.ListOfMapEditors.Add(ChildForm)
    End Sub

    Private Sub SupprimerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupprimerToolStripMenuItem.Click
        If IsNothing(Cells(SelectedCell).MyMap) Then Exit Sub
        ' Delete
        Cells(SelectedCell).MyMap = Nothing
        RefreshDatas(SelectedCell)
        GenerateGrid()
        DrawAll()
    End Sub

    Private Sub OuvrirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OuvrirToolStripMenuItem.Click
        If IsNothing(Cells(SelectedCell).MyMap) Then Exit Sub
        ' Open
        Main.OpenProject(Cells(SelectedCell).MyMap)
    End Sub
#End Region

#Region " Fonctions "

    Public Sub RefreshDatas(ByVal ID As Integer)
        If Not IsNothing(Cells(ID).MyMap) Then
            ' Boutons
            OuvrirToolStripMenuItem.Visible = True
            InsérerToolStripMenuItem.Visible = False
            NouvelleMapToolStripMenuItem.Visible = False
            SupprimerToolStripMenuItem.Visible = True
            ' Group
            L_CellInfo.Visible = True
            L_CellInfo.Text = Cells(ID).MyMap.ID & "_" & Cells(ID).MyMap.DateMap & " : " & Cells(ID).MyMap.Width & " × " & Cells(ID).MyMap.Height
            BT_OpenMap.Visible = True
        Else
            ' Boutons
            SupprimerToolStripMenuItem.Visible = False
            InsérerToolStripMenuItem.Visible = True
            NouvelleMapToolStripMenuItem.Visible = True
            SupprimerToolStripMenuItem.Visible = False
            ' Group
            L_CellInfo.Visible = False
            L_CellInfo.Text = ""
            BT_OpenMap.Visible = False
        End If
    End Sub

    Public Sub Save()
        ' Actualisation des mapid
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then aCell.MapID = aCell.MyMap.ID
            aCell.Selected = False
        Next
        Island_Name = TXT_Name.Text

        ' Sauvegarde Binaire
        If Not IO.Directory.Exists(Where_Geo_Directory()) Then IO.Directory.CreateDirectory(Where_Geo_Directory())
        If Not IO.Directory.Exists(Where_Geo_Directory(Island_Name)) Then IO.Directory.CreateDirectory(Where_Geo_Directory(Island_Name))
        Dim FluxDeFichier As FileStream = File.Create(Where_Geo_SaveFile(Island_Name))
        Dim Serialiseur As New BinaryFormatter
        Serialiseur.Serialize(FluxDeFichier, Cells)
        FluxDeFichier.Close()

        ' Sauvegarde png
        Dim before As Integer = SizeCell.Height
        SizeCell.Height = 250
        SizeCell.Width = CInt(1.66 * SizeCell.Height)
        MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
        G = Graphics.FromImage(MyImage)
        GenerateGrid()
        DrawAll(False)
        G.DrawImage(AstriaMapEditor.My.Resources.logo_map, New Point(MyImage.Width - AstriaMapEditor.My.Resources.logo_map.Width - 5, MyImage.Height - AstriaMapEditor.My.Resources.logo_map.Height - 5))
        MyImage.Save(Where_Geo_Image(Island_Name))
        DrawAll()
        G.DrawImage(AstriaMapEditor.My.Resources.logo_map, New Point(MyImage.Width - AstriaMapEditor.My.Resources.logo_map.Width - 5, MyImage.Height - AstriaMapEditor.My.Resources.logo_map.Height - 5))
        MyImage.Save(Where_Geo_ImageMode(Island_Name))
        SizeCell.Height = before
        SizeCell.Width = CInt(1.66 * SizeCell.Height)
        MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
        G = Graphics.FromImage(MyImage)
        GenerateGrid()
        DrawAll()

        Main.ToolStripStatusLabel1.Text = "[" & Date.Now.Hour & "h" & Date.Now.Minute & "] Sauvegarde de la géoposition de " & Island_Name & " effectué"
    End Sub

#End Region

#Region " Menu ""Île"" "
    Private Sub SauvegarderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Save.Click
        Save()
    End Sub

    Private Sub Charger_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Load.Click

        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = Main.DirectoryApply
        OpenFileDialog.Filter = "Fichiers Géoposition (*.geo)|*.geo"

        Me.Enabled = False

        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As New FileInfo(OpenFileDialog.FileName)
            Dim FluxDeFichier As Stream = File.OpenRead(FileName.FullName)
            Dim Deserialiseur As New BinaryFormatter()
            Dim copyCells(SizeMap.Width * SizeMap.Height - 1) As CellGeo
            copyCells = CType(Deserialiseur.Deserialize(FluxDeFichier), CellGeo())
            FluxDeFichier.Close()

            For Each aCell As CellGeo In copyCells
                aCell.Geo = Me
                If Not IsNothing(aCell.MapID) Then
                    For Each aMap As Map In Map.ListOfMaps
                        If aMap.ID = aCell.MapID Then
                            aCell.MyMap = aMap
                            Exit For
                        End If
                    Next
                End If
            Next

            ' Trouve la taille de la map
            Dim minposx = 0
            Dim minposy = 0
            Dim maxposx = 0
            Dim maxposy = 0
            For j = 0 To copyCells.Length - 1
                If copyCells(j).x_pos < minposx Then minposx = copyCells(j).x_pos
                If copyCells(j).y_pos < minposy Then minposy = copyCells(j).y_pos
                If copyCells(j).x_pos > maxposx Then maxposx = copyCells(j).x_pos
                If copyCells(j).y_pos > maxposy Then maxposy = copyCells(j).y_pos
            Next
            SizeMap.Width = (maxposx - minposx) + 1
            SizeMap.Height = (maxposy - minposy) + 1
            x_PositionTopLeft = minposx
            y_PositionTopLeft = minposy

            ' Copie des éléments
            Dim aCells(SizeMap.Width * SizeMap.Height - 1) As CellGeo
            Cells = aCells

            GenerateGrid() ' Génère la grille
            ' Replace les maps
            For i = 0 To Cells.Length - 1
                For j = 0 To copyCells.Length - 1
                    If Cells(i).x_pos = copyCells(j).x_pos And Cells(i).y_pos = copyCells(j).y_pos Then
                        Cells(i).MapID = copyCells(j).MapID
                        Cells(i).MyMap = copyCells(j).MyMap
                    End If
                Next
            Next

            Island_Name = FileName.Name.Substring(0, FileName.Name.Length - FileName.Extension.Length)
            TXT_Name.Text = Island_Name

            MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1, SizeCell.Height * SizeMap.Height + 1)
            G = Graphics.FromImage(MyImage)
            GenerateGrid()
            DrawAll()

        End If

        Me.Enabled = True
    End Sub

    Private Sub OuvrirDansWindows_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_OpenInWindows.Click
        ' Ouvrir dans l'explorateur Windows
        If Not Directory.Exists(FileManager.Where_Geo_Directory(Island_Name)) Then Save()
        Process.Start(FileManager.Where_Geo_Directory(Island_Name))
    End Sub

    Private Sub CouleurArrièrePlan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BT_Color.Click
        Dim acolordialog As New ColorDialog
        acolordialog.ShowDialog()
        aColor = acolordialog.Color
    End Sub

    Private Sub TXT_Pos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_X.TextChanged, TXT_Y.TextChanged
        If IsNothing(Cells) Or Not IsNumeric(TXT_X.Text) Or Not IsNumeric(TXT_Y.Text) Then Exit Sub
        x_PositionTopLeft = CInt(TXT_X.Text)
        y_PositionTopLeft = CInt(TXT_Y.Text)

        GenerateGrid()
        DrawAll()
    End Sub

#Region " Areas "

    Private Sub TXT_SA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_SA.TextChanged
        ' Modification du SuperArea
        If Not IsNumeric(TXT_SA.Text) Then
            TXT_SA.Text = 0
        Else
            If IsNothing(Cells) Then Exit Sub
            For Each aCell As CellGeo In Cells
                If IsNothing(aCell) Then If Not IsNothing(aCell.MyMap) Then aCell.MyMap.SuperArea = TXT_SA.Text
            Next
        End If
    End Sub

    Private Sub TXT_A_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_A.SelectedIndexChanged
        ' Modification du Area
        Dim aArea As Area = Area.GetByName(TXT_A.SelectedText)
        For Each aCell As CellGeo In Cells
            If IsNothing(Cells) Then Exit Sub
            If IsNothing(aCell) Then If Not IsNothing(aCell.MyMap) Then aCell.MyMap.Area = aArea.ID
        Next
    End Sub

    Private Sub TXT_SubA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TXT_SubA.SelectedIndexChanged
        ' Modification du SubArea
        Dim aSubArea As SubArea = SubArea.GetByName(TXT_SubA.SelectedText)
        For Each aCell As CellGeo In Cells
            If IsNothing(Cells) Then Exit Sub
            If IsNothing(aCell) Then If Not IsNothing(aCell.MyMap) Then aCell.MyMap.SubArea = aSubArea.ID
        Next
    End Sub

#End Region

#End Region

#Region " Menu ""Fonctions"" "

#Region " Placement automatique des triggers "
    Private Sub ExécuterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExécuterToolStripMenuItem.Click
        If Not Directory.Exists(FileManager.Where_Geo_Directory(Island_Name)) Then Save()
        If IO.File.Exists(FileManager.Where_Geo_Trigger(Island_Name)) Then IO.File.Delete(FileManager.Where_Geo_Trigger(Island_Name))

        For Each aCell As CellGeo In Cells
            If IsNothing(aCell.MyMap) Then Continue For
            If IsNothing(aCell.MyMap.Cells) Then aCell.MyMap.Load()

            Dim TriggersToConfig As List(Of Cell) = Get_Triggers(aCell)
            Dim Triggable As List(Of Integer)() = Get_Cells_Triggables(aCell)

            For Each Trigger As Cell In TriggersToConfig
                If IsNothing(Cells) Then Continue For

                Try
                    ' Map dessus
                    If Triggable(0).Contains(Trigger.ID) Then
                        Dim ID_Top As Integer = aCell.ID - SizeMap.Width
                        If Not ID_Top < 0 AndAlso Not IsNothing(Cells(ID_Top).MyMap) Then ' Existe
                            ResolveTriggers(aCell, Cells(ID_Top), Trigger.ID, 1)
                        End If
                    End If

                    ' Map dessous
                    If Triggable(1).Contains(Trigger.ID) Then
                        Dim ID_Bottom As Integer = aCell.ID + SizeMap.Width
                        If Not ID_Bottom > Cells.Length _
                            AndAlso Not IsNothing(Cells(ID_Bottom).MyMap) Then ' Existe
                            ResolveTriggers(aCell, Cells(ID_Bottom), Trigger.ID, 2)
                        End If
                    End If

                    ' Map droite
                    If Triggable(2).Contains(Trigger.ID) Then
                        Dim ID_Right As Integer = aCell.ID + 1
                        If Not aCell.ID Mod SizeMap.Width = 0 AndAlso Not IsNothing(Cells(ID_Right).MyMap) Then ' Existe
                            ResolveTriggers(aCell, Cells(ID_Right), Trigger.ID, 3)
                        End If
                    End If

                    ' Map gauche
                    If Triggable(3).Contains(Trigger.ID) Then
                        Dim ID_Left As Integer = aCell.ID - 1
                        If Not aCell.ID Mod (SizeMap.Width + 1) = 0 AndAlso Not IsNothing(Cells(ID_Left).MyMap) Then ' Existe
                            ResolveTriggers(aCell, Cells(ID_Left), Trigger.ID, 4)
                        End If
                    End If
                Catch ex As Exception
                    System.IO.File.AppendAllText("Error.log", ex.ToString & vbCrLf)
                End Try
            Next
        Next
        Main.Outil = Main.Tools.CellMode
        Main.RefreshAllMaps()
        If MsgBox("Tous les triggers extérieurs ont été automatiquement ajoutés dans le fichier sql situé dans le dossier de la géoposition de " & Island_Name & "." & vbCrLf & vbCrLf & "Voulez-vous l'ouvrir dans l'explorateur Windows ?", MsgBoxStyle.YesNo, "Triggers placés") = MsgBoxResult.Yes Then
            Process.Start(FileManager.Where_Geo_Directory(Island_Name))
        End If
    End Sub

    Private Sub ResolveTriggers(ByRef aCell As CellGeo, ByRef ToCell As CellGeo, ByVal Trigger As Integer, ByVal where As Integer)
        If IsNothing(ToCell.MyMap.Cells) Then ToCell.MyMap.Load()
        Dim ToTriggables As List(Of Integer)() = Get_Cells_Triggables(ToCell)
        Dim ToTriggers As List(Of Cell) = Get_Triggers(ToCell)

        Select Case where
            Case 1 ' Top
                For Each aToTrigger As Cell In ToTriggers
                    If ToTriggables(1).Contains(aToTrigger.ID) Then ' S'il y a un trigger en bas de la map de destination
                        ' Trouve la cellule de destination (in bottom)
                        If Not ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width).UnWalkable Then
                            AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width).ID)
                        Else
                            If Not ToCell.MyMap.Cells(aToTrigger.ID - (ToCell.MyMap.Width - 1)).UnWalkable Then
                                AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID - (ToCell.MyMap.Width - 1)).ID)
                            End If
                        End If
                        ' Trouve la cellule de retour (in top)
                        If Not aCell.MyMap.Cells(Trigger + aCell.MyMap.Width).UnWalkable Then
                            AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger + aCell.MyMap.Width).ID)
                        Else
                            If Not aCell.MyMap.Cells(Trigger + aCell.MyMap.Width - 1).UnWalkable Then
                                AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger + aCell.MyMap.Width - 1).ID)
                            End If
                        End If
                    End If
                Next
            Case 2 ' Bottom
                For Each aToTrigger As Cell In ToTriggers
                    If ToTriggables(0).Contains(aToTrigger.ID) Then ' S'il y a un trigger en haut de la map de destination
                        ' Trouve la cellule de destination (in top)
                        If Not ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width).UnWalkable Then
                            AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width).ID)
                        Else
                            If Not ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width - 1).UnWalkable Then
                                AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width - 1).ID)
                            End If
                        End If
                        ' Trouve la cellule de retour (in bottom)
                        If Not aCell.MyMap.Cells(Trigger - aCell.MyMap.Width).UnWalkable Then
                            AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger - aCell.MyMap.Width).ID)
                        Else
                            If Not aCell.MyMap.Cells(Trigger - aCell.MyMap.Width - 1).UnWalkable Then
                                AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger - aCell.MyMap.Width - 1).ID)
                            End If
                        End If
                    End If
                Next
            Case 3 ' Right
                For Each aToTrigger As Cell In ToTriggers
                    If ToTriggables(3).Contains(aToTrigger.ID) Then ' S'il y a un trigger à gauche de la map de destination
                        ' Trouve la cellule de destination (in left)
                        If Not ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width - 1).UnWalkable Then
                            AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width - 1).ID)
                        Else
                            If Not ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width).UnWalkable Then
                                AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width).ID)
                            End If
                        End If
                        ' Trouve la cellule de retour (in right)
                        If Not aCell.MyMap.Cells(Trigger - aCell.MyMap.Width).UnWalkable Then
                            AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger - aCell.MyMap.Width).ID)
                        Else
                            If Not aCell.MyMap.Cells(Trigger + aCell.MyMap.Width - 1).UnWalkable Then
                                AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger + aCell.MyMap.Width - 1).ID)
                            End If
                        End If
                    End If
                Next
            Case 4 ' Left
                For Each aToTrigger As Cell In ToTriggers
                    If ToTriggables(2).Contains(aToTrigger.ID) Then ' S'il y a un trigger à droite de la map de destination
                        ' Trouve la cellule de destination (in right)
                        If Not ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width).UnWalkable Then
                            AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID - ToCell.MyMap.Width).ID)
                        Else
                            If Not ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width - 1).UnWalkable Then
                                AddTrigger(aCell.MyMap, Trigger, ToCell.MyMap, ToCell.MyMap.Cells(aToTrigger.ID + ToCell.MyMap.Width - 1).ID)
                            End If
                        End If
                        ' Trouve la cellule de retour (in left)
                        If Not aCell.MyMap.Cells(Trigger - aCell.MyMap.Width + 1).UnWalkable Then
                            AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger - aCell.MyMap.Width + 1).ID)
                        Else
                            If Not aCell.MyMap.Cells(Trigger + aCell.MyMap.Width).UnWalkable Then
                                AddTrigger(ToCell.MyMap, aToTrigger.ID, aCell.MyMap, aCell.MyMap.Cells(Trigger + aCell.MyMap.Width).ID)
                            End If
                        End If
                    End If
                Next
        End Select
    End Sub

    Private Sub AddTrigger(ByRef Map As Map, ByVal Cellid As Integer, ByRef ToMap As Map, ByVal toCellid As Integer)
        IO.File.AppendAllText(FileManager.Where_Geo_Trigger(Island_Name), MySQL.Get_SqlTrigger(Map.ID, Cellid, ToMap.ID, toCellid))

        Main.NbTriggers += 1
        Map.Cells(Cellid).Trigger = True
        Map.Cells(Cellid).TriggerName = Main.NbTriggers & "D"
        ToMap.Cells(toCellid).Trigger = True
        ToMap.Cells(toCellid).TriggerName = Main.NbTriggers & "A"
    End Sub

    Private Function Get_Triggers(ByRef aCell As CellGeo) As List(Of Cell)
        ' Récupération des ID des triggers de la map sélectionné
        Dim triggers As New List(Of Cell)
        For Each MyCell As Cell In aCell.MyMap.Cells
            If IsNothing(MyCell) Then Continue For
            If Not IsNothing(MyCell.Gfx2) AndAlso MyCell.Gfx2.ID = ToolStripComboBox3.Text Then triggers.Add(MyCell)
            If Not IsNothing(MyCell.Gfx3) AndAlso MyCell.Gfx3.ID = ToolStripComboBox3.Text Then triggers.Add(MyCell)
        Next

        Return triggers
    End Function

    Private Function Get_Cells_Triggables(ByRef aCell As CellGeo) As List(Of Integer)()
        ' Ligne du haut
        Dim ToTop As New List(Of Integer)
        '       Première ligne
        For i As Integer = aCell.MyMap.Width + 1 To aCell.MyMap.Width * 2 - 3
            ToTop.Add(i)
        Next
        '       Deuxième ligne
        For i As Integer = (aCell.MyMap.Width * 2) + 1 To aCell.MyMap.Width * 3 - 4
            ToTop.Add(i)
        Next

        ' Ligne du bas
        '       Première ligne
        Dim ToBottom As New List(Of Integer)
        For i As Integer = (aCell.MyMap.Height) * ((aCell.MyMap.Width * 2) - 1) - (aCell.MyMap.Width * 3 - 3) To (aCell.MyMap.Height) * ((aCell.MyMap.Width * 2) - 1) - (aCell.MyMap.Width * 2) - 1
            ToBottom.Add(i)
        Next
        '       Deuxième ligne
        For i As Integer = (aCell.MyMap.Height) * ((aCell.MyMap.Width * 2) - 1) - (aCell.MyMap.Width * 4 - 4) To (aCell.MyMap.Height) * ((aCell.MyMap.Width * 2) - 1) - (aCell.MyMap.Width * 3) - 1
            ToBottom.Add(i)
        Next

        ' Ligne de droite
        '       Première ligne
        Dim ToRight As New List(Of Integer)
        For i As Integer = (aCell.MyMap.Width - 1) * 4 + 1 To ((aCell.MyMap.Width - 1) * 2) * (aCell.MyMap.Height - 1) Step (aCell.MyMap.Width * 2 - 1)
            ToRight.Add(i)
        Next
        '       Deuxième ligne
        For i As Integer = (aCell.MyMap.Width - 1) * 5 + 1 To ((aCell.MyMap.Width - 1) * 2) * (aCell.MyMap.Height - 2) Step (aCell.MyMap.Width * 2 - 1)
            ToRight.Add(i)
        Next

        ' Ligne de gauche
        '       Première ligne
        Dim ToLeft As New List(Of Integer)
        For i As Integer = aCell.MyMap.Width * 3 - 1 To ((aCell.MyMap.Width * 2) - 1) * (aCell.MyMap.Height - 2) Step (aCell.MyMap.Width * 2 - 1)
            ToLeft.Add(i)
        Next
        '       Deuxième ligne
        For i As Integer = aCell.MyMap.Width * 4 - 1 To ((aCell.MyMap.Width * 2) - 1) * (aCell.MyMap.Height - 2) Step (aCell.MyMap.Width * 2 - 1)
            ToLeft.Add(i)
        Next

        Return {ToTop, ToBottom, ToRight, ToLeft}
    End Function
#End Region

#Region " Ouvrir toutes les maps "
    Private Sub OuvrirToutesLesMapsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OuvrirToutesLesMapsToolStripMenuItem.Click
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) AndAlso Not aCell.MyMap.IsEditing Then
                Main.OpenProject(aCell.MyMap)
            End If
        Next
    End Sub
#End Region

#Region " Générer les positions des maps "
    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GéopositionsDesMapsToolStripMenuItem.Click

        If Not Directory.Exists(FileManager.Where_Geo_Directory(Island_Name)) Then Save()
        If IO.File.Exists(Where_Geo_UpdateMapPos(Island_Name)) Then IO.File.Delete(Where_Geo_UpdateMapPos(Island_Name))

        ' SQL
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then
                IO.File.AppendAllText(Where_Geo_UpdateMapPos(Island_Name), Get_UpdateMapPos(aCell.x_pos, aCell.y_pos, 0, aCell.MyMap.ID))
                IO.File.AppendAllText(Where_Geo_SWFMapPos(Island_Name), "MA.m[" & aCell.MyMap.ID & "] = {ep: 1, sa: " & aCell.MyMap.SubArea & ", y: " & aCell.y_pos & ", x: " & aCell.x_pos & "};" & vbCrLf)
            End If
        Next
        If MsgBox("Les fichiers ont été ajoutés au dossier de la géoposition de " & Island_Name & "." & vbCrLf & vbCrLf & "Voulez-vous l'ouvrir dans l'explorateur Windows ?", MsgBoxStyle.YesNo, "Ajout effectué") = MsgBoxResult.Yes Then
            Process.Start(FileManager.Where_Geo_Directory(Island_Name))
        End If
    End Sub
#End Region

#Region "Configuration des maisons & enclos"

    Private Sub GénérerUnTemplateDesMaisonsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ConfigurationDesMaisonsEnclosToolStripMenuItem.Click

        If Not Directory.Exists(FileManager.Where_Geo_Directory(Island_Name)) Then Save()
        If IO.File.Exists(FileManager.Where_Geo_Trigger(Island_Name)) Then IO.File.Delete(FileManager.Where_Geo_Trigger(Island_Name))

        Dim StartID As Integer = CInt(InputBox("Veuillez entrer l'ID de la première maison", "Maison", "1165"))
        Dim Name As String = InputBox("Veuillez entrer un nom générique de maison", "Nom", "Maison de " & Island_Name)
        Dim Description As String = InputBox("Veuillez entrer une description générique de maison", "Description", "Noble habitat, placée au coeur de la ville et haut confort intérieur. Cette maison saura vous séduire par la richesse de son ameublement.")
        Dim PriceFor1Place As Integer = CInt(InputBox("Veuillez entrer le prix d'une place d'elevage, les prix des enclos seront calculés à partir de celui ci", "Enclos", "500000"))
        Dim Houses As Integer = 0
        Dim MountParks As Integer = 0

        For Each aCell As CellGeo In Cells
            If IsNothing(aCell.MyMap) Then Continue For
            If IsNothing(aCell.MyMap.Cells) Then aCell.MyMap.Load()

            ' Search houses & mountparks
            Dim HousesCells As New List(Of Integer)
            Dim MountParksCells As New List(Of Integer)
            For Each MyCell As Cell In aCell.MyMap.Cells
                If IsNothing(MyCell) Then Continue For
                If Not IsNothing(MyCell.Gfx3) AndAlso (MyCell.Gfx3.ID >= 6700 And MyCell.Gfx3.ID <= 6776) Then ' Ids Doors
                    If MyCell.Gfx3.ID = 6763 Or MyCell.Gfx3.ID = 6766 Or MyCell.Gfx3.ID = 6767 Or MyCell.Gfx3.ID = 6772 Then ' Is MountPark
                        MountParksCells.Add(MyCell.ID)
                    Else ' Is House
                        HousesCells.Add(MyCell.ID)
                    End If
                End If
            Next

            ' Houses
            If HousesCells.Count > 0 Then ' Map contains houses
                ' Write houses
                ' SWF
                For i = 0 To HousesCells.Count - 1
                    ' SQL
                    IO.File.AppendAllText(FileManager.Where_Geo_SQLHouses(Island_Name), MySQL.Get_SqlHouse(StartID + Houses + i, aCell.MyMap.ID, HousesCells(i), 1000000, 0, 0))
                    ' Logs
                    IO.File.AppendAllText(FileManager.Where_Geo_SWFHousesLogs(Island_Name), "Maison n°" & StartID + Houses + i & " en map n°" & aCell.MapID & " sur la cellule n°" & HousesCells(i) & "." & vbCrLf)
                    ' SWF
                    IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), "H.h[" & (StartID + Houses + i) & "] = {n: """ & Name & """, d: """ & Description & """};" & vbCrLf)
                    IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), "H.m[<IN MAPID>] = " & (StartID + Houses + i) & ";" & vbCrLf)
                Next
                IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), "H.d[" & aCell.MapID & "] = {")
                For i = 0 To HousesCells.Count - 1
                    IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), "c" & HousesCells(i) & ": " & (StartID + Houses + i))
                    If Not i = HousesCells.Count - 1 Then IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), ", ")
                Next
                IO.File.AppendAllText(FileManager.Where_Geo_SWFHouses(Island_Name), "};" & vbCrLf)
            End If

            ' Mountparks
            If MountParksCells.Count > 0 Then ' Map contains mountparks

                For i = 0 To MountParksCells.Count - 1
                    ' Check Cellid
                    Dim CellIDBeforeDoor As Integer = 0
                    If aCell.MyMap.Cells(MountParksCells(i)).FlipGfx3 Then ' Check (top left) and (bottom right)
                        If aCell.MyMap.Cells(MountParksCells(i) - aCell.MyMap.Width).Type = MovementEnum.PADDOCK Then ' top left
                            CellIDBeforeDoor = MountParksCells(i) + aCell.MyMap.Width
                        Else ' bottom right
                            CellIDBeforeDoor = MountParksCells(i) - aCell.MyMap.Width
                        End If
                    Else
                        If aCell.MyMap.Cells(MountParksCells(i) - (aCell.MyMap.Width - 1)).Type = MovementEnum.PADDOCK Then ' top right
                            CellIDBeforeDoor = MountParksCells(i) + aCell.MyMap.Width - 1
                        Else ' bottom left
                            CellIDBeforeDoor = MountParksCells(i) - (aCell.MyMap.Width - 1)
                        End If
                    End If

                    ' Size
                    Dim Size As Integer = 5
                    If MountParksCells.Count = 1 Then ' To found a size...
                        ' Calcul size
                        Dim CountPaddockCells = 0
                        For Each MyCell As Cell In aCell.MyMap.Cells
                            If IsNothing(MyCell) Then Continue For
                            If MyCell.Type = MovementEnum.PADDOCK Then CountPaddockCells = CountPaddockCells + 1
                        Next
                        Size = Math.Ceiling(CountPaddockCells / 10)
                    End If

                    ' Price
                    Dim Price As Integer = PriceFor1Place * Size

                    ' SQL
                    IO.File.AppendAllText(FileManager.Where_Geo_SQLMountParks(Island_Name), MySQL.Get_SqlMountpark(aCell.MyMap.ID, CellIDBeforeDoor, Size, Price))
                    ' Logs
                    IO.File.AppendAllText(FileManager.Where_Geo_SQLMountParksLogs(Island_Name), "Enclo en map n°" & aCell.MapID & " sur la cellule n°" & CellIDBeforeDoor & "." & vbCrLf)

                    ' Count
                    MountParks = MountParks + 1
                Next
            End If

                Houses = Houses + HousesCells.Count
        Next

        If MsgBox(MountParks & " enclos et " & Houses & " maisons ajoutées !" & vbCrLf & "Les fichiers ont été ajoutés au dossier de la géoposition de " & Island_Name & "." & vbCrLf & vbCrLf & "Voulez-vous l'ouvrir dans l'explorateur Windows ?", MsgBoxStyle.YesNo, "Ajout effectué") = MsgBoxResult.Yes Then
            Process.Start(FileManager.Where_Geo_Directory(Island_Name))
        End If
    End Sub

#End Region

#Region " Définir les monstres pour toutes les maps "

    Private Sub DefinirLesMonstresPourToutesLesMapsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem.Click
        Dim aMobGroup As New MobGroup()
        aMobGroup.ShowDialog()
        If aMobGroup.Mobs = "" Then
            aMobGroup.Dispose()
            Exit Sub
        End If
        Dim Mobs As String = aMobGroup.Mobs
        aMobGroup.Dispose()

        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then aCell.MyMap.Mobs = Mobs
        Next

        MsgBox("Le groupe de monstre a été redéfini sur toutes les maps de l'île.")
    End Sub

#End Region

#Region " Préparation pack "

    Private Sub PréparationPatchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PréparationPatchToolStripMenuItem.Click
        Save()
        '   Création d'un dossier spécifique
        If IO.Directory.Exists(Where_Geo_Directory(Island_Name) & "\Pack") Then IO.Directory.Delete(Where_Geo_Directory(Island_Name) & "\Pack", True)
        IO.Directory.CreateDirectory(Where_Geo_Directory(Island_Name) & "\Pack")
        '   Copie des fichiers
        '       Géoposition
        IO.File.Copy(Where_Geo_Image(Island_Name), Where_Geo_Directory(Island_Name) & "\Pack\Géoposition.png")
        IO.File.Copy(Where_Geo_ImageMode(Island_Name), Where_Geo_Directory(Island_Name) & "\Pack\Géoposition ids.png")
        '       SWF, PNG
        IO.Directory.CreateDirectory(Where_Geo_Directory(Island_Name) & "\Pack\SWF")
        IO.Directory.CreateDirectory(Where_Geo_Directory(Island_Name) & "\Pack\PNG")
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then
                If IO.File.Exists(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & "_" & aCell.MyMap.DateMap & ".swf") Then _
                    IO.File.Copy(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & "_" & aCell.MyMap.DateMap & ".swf", Where_Geo_Directory(Island_Name) & "\Pack\SWF\" & aCell.MyMap.ID & "_" & aCell.MyMap.DateMap & ".swf")
                If IO.File.Exists(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & ".png") Then _
                    IO.File.Copy(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & ".png", Where_Geo_Directory(Island_Name) & "\Pack\PNG\" & aCell.MyMap.ID & ".png")
                If IO.File.Exists(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & "_ModeCell.png") Then _
                    IO.File.Copy(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & "_ModeCell.png", Where_Geo_Directory(Island_Name) & "\Pack\PNG\" & aCell.MyMap.ID & "_ModeCell.png")
            End If
        Next

        '   SQLs
        Dim sql As String = _
            "-----------------------------------------------" & vbCrLf & _
            "---------------- Maps installer ---------------" & vbCrLf & _
            "-- " & Island_Name & " --" & vbCrLf & _
            "-- Generated by Astria Map Editor v." & My.Application.Info.Version.ToString & " --" & vbCrLf & _
            "-----------------------------------------------" & vbCrLf & _
            vbCrLf & _
            "-- Maps --"
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then
                sql &= vbCrLf & IO.File.ReadAllText(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\" & aCell.MyMap.ID & ".sql")
            End If
        Next
        If IO.File.Exists(Where_Geo_Trigger(Island_Name)) Then
            sql &= vbCrLf & vbCrLf & "-- Triggers --"
            sql &= vbCrLf & IO.File.ReadAllText(Where_Geo_Trigger(Island_Name))
        End If
        IO.File.WriteAllText(Where_Geo_Directory(Island_Name) & "\Pack\Install.sql", sql)

        ' Gfx utilisés
        Dim list As String = ""
        For Each aCell As CellGeo In Cells
            If Not IsNothing(aCell.MyMap) Then
                list &= IO.File.ReadAllText(Main.DirectoryApply & "\Maps\" & aCell.MyMap.ID & "\GfxID utilisés.txt")
            End If
        Next
        Dim txts As String() = list.Split(vbLf)
        Dim nb As New List(Of Integer)
        For Each atxt As String In txts
            If Not atxt = "" AndAlso Not nb.Contains(atxt) AndAlso IsNumeric(atxt) Then
                nb.Add(CInt(atxt))
            End If
        Next
        nb.Sort()

        Dim txt As String = _
            "-----------------------------------------------" & vbCrLf & _
            "---------------- GfxID utilisés ---------------" & vbCrLf & _
            "-- " & Island_Name & " --" & vbCrLf & _
            "-- Generated by Astria Map Editor v." & My.Application.Info.Version.ToString & " --" & vbCrLf & _
            "-----------------------------------------------" & vbCrLf & _
            vbCrLf
        For Each anb As Integer In nb
            txt &= anb & vbCrLf
        Next
        IO.File.WriteAllText(Where_Geo_Directory(Island_Name) & "\Pack\GfxID utilisés.txt", txt)
    End Sub

#End Region

#End Region

#Region " Modification de la taille de la grille "

#Region " Function "

    Private Sub EditGridSize(ByVal newWidth As Integer, ByVal newHeight As Integer, ByVal newposx As Integer, ByVal newposy As Integer)
        Me.Enabled = False

        ' Copie pour sauvegarder
        Dim copyCells(SizeMap.Width * SizeMap.Height - 1) As CellGeo
        Cells.CopyTo(copyCells, 0)

        ' Modification de la grille
        If x_PositionTopLeft <> newposx Then x_PositionTopLeft = newposx
        If y_PositionTopLeft <> newposy Then y_PositionTopLeft = newposy
        If newWidth > 0 And SizeMap.Width <> newWidth Then SizeMap.Width = newWidth
        If newHeight > 0 And SizeMap.Height <> newHeight Then SizeMap.Height = newHeight

        ' Copie des éléments
        Dim aCells(SizeMap.Width * SizeMap.Height - 1) As CellGeo
        Cells = aCells
        GenerateGrid() ' Génère la grille
        ' Replace les maps
        For i = 0 To Cells.Length - 1
            For j = 0 To copyCells.Length - 1
                If Cells(i).x_pos = copyCells(j).x_pos And Cells(i).y_pos = copyCells(j).y_pos Then
                    Cells(i).MapID = copyCells(j).MapID
                    Cells(i).MyMap = copyCells(j).MyMap
                End If
            Next
        Next

        MyImage = New Bitmap(SizeCell.Width * SizeMap.Width + 1 + BaseX, SizeCell.Height * SizeMap.Height + 1 + BaseY)
        G = Graphics.FromImage(MyImage)
        DrawAll()

        Me.Enabled = True
    End Sub

#End Region

#Region " Controllers "

    Private Sub ColonneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColonneToolStripMenuItem.Click
        ' Ajout colonne droite
        EditGridSize(SizeMap.Width + 1, 0, x_PositionTopLeft, y_PositionTopLeft)
    End Sub

    Private Sub ColonneGaucheToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColonneGaucheToolStripMenuItem1.Click
        ' Ajout colonne gauche
        EditGridSize(SizeMap.Width + 1, 0, x_PositionTopLeft - 1, y_PositionTopLeft)
    End Sub

    Private Sub LigneHautToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigneHautToolStripMenuItem.Click
        ' Ajout ligne haut
        EditGridSize(0, SizeMap.Height + 1, x_PositionTopLeft, y_PositionTopLeft - 1)
    End Sub

    Private Sub LigneBasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigneBasToolStripMenuItem.Click
        ' Ajout ligne bas
        EditGridSize(0, SizeMap.Height + 1, x_PositionTopLeft, y_PositionTopLeft)
    End Sub

    Private Sub ColonneDroiteToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColonneDroiteToolStripMenuItem1.Click
        ' Suppression colonne droite
        EditGridSize(SizeMap.Width - 1, 0, x_PositionTopLeft, y_PositionTopLeft)
    End Sub

    Private Sub ColonneGaucheToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColonneGaucheToolStripMenuItem2.Click
        ' Suppression colonne gauche
        EditGridSize(SizeMap.Width - 1, 0, x_PositionTopLeft + 1, y_PositionTopLeft)
    End Sub

    Private Sub LigneHautToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigneHautToolStripMenuItem1.Click
        ' Suppression ligne haut
        EditGridSize(0, SizeMap.Height - 1, x_PositionTopLeft, y_PositionTopLeft + 1)
    End Sub

    Private Sub LigneBasToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigneBasToolStripMenuItem1.Click
        ' Suppression ligne bas
        EditGridSize(0, SizeMap.Height - 1, x_PositionTopLeft, y_PositionTopLeft)
    End Sub

#End Region

#End Region

End Class