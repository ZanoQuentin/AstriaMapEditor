Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Net

Public Class MapEditor

    Const TIMETOSLEEP As Integer = 800

    ' Configuration
    Public Shared SizeBaseCell As Integer = 26
    Public SizeCell As Integer = 26 ' Taille d'une demi cellule en pixel
    Public SizeTable As New Size(15, 17) ' Taille de la map en nombre de cellules

    Public MyDatas As Map

    ' Dessin
    Public SizeOfImg As Size
    Public MyImage As Bitmap
    Public Grid As Bitmap
    Public G As Graphics
    Public PourceOfTile As Double

    ' Variables temporaires
    Public HoverCell As Integer = 1
    Public SelectedCell As Integer = 1
    Public Edited As Boolean = False
    Private Loaded As Boolean = False
    Private SizeChange As Boolean = False

    Public Sub New(Optional ByRef aMap As Map = Nothing)
        InitializeComponent()
        If Not IsNothing(aMap) Then
            MyDatas = aMap
            MyDatas.Load()
        End If
    End Sub

#Region " Events "
    Private Sub Map_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.KeyPreview = True

        ' Définition des variables
        If IsNothing(MyDatas) Then
            MyDatas = New Map
            ReDim MyDatas.Cells(SizeTable.Height * (SizeTable.Width * 2 - 1) - SizeTable.Width)
            MyDatas.Width = SizeTable.Width
            MyDatas.Height = SizeTable.Height
        End If
        MyDatas.IsEditing = True

        SizeOfImg = New Size(SizeTable.Width * SizeCell * 2, SizeTable.Height * SizeCell)
        Me.Size = New Size(SizeOfImg.Width + 16, SizeOfImg.Height + 38)
        MyImage = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
        Grid = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
        G = Graphics.FromImage(MyImage)
        PourceOfTile = SizeCell / SizeBaseCell
        Main.SelectedMap = Me
        Main.MenuMap_RefreshControls()

        ' Add map to list
        Map.AddMap(MyDatas)

        ' Génère la map
        GenerateGrid()
        MakeUnwalkableBorders()
        DrawAll()

        Loaded = True
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub OnClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Edited Then
            Dim MsgBoxResulte As MsgBoxResult = MsgBox("Voulez-vous enregistrer les modifications apportées à la map ?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Information, "Confirmation")
            If MsgBoxResulte = MsgBoxResult.Yes Then
                Save()
                CloseMe()
            ElseIf MsgBoxResulte = MsgBoxResult.No Then
                CloseMe()
            ElseIf MsgBoxResulte = MsgBoxResult.Cancel Then
                e.Cancel = True
            End If
        Else
            CloseMe()
        End If
    End Sub

    Private Sub CloseMe()
        MyDatas.IsEditing = False
        Dim aCells(Height * (Width * 2 - 1) - Width) As Cell
        MyDatas.Cells = aCells
        MyImage.Dispose()
        Tile.UnCacheAll()
        Grid.Dispose()
        G.Dispose()
        Main.SelectedMap = Nothing
        Main.MenuMap_RefreshControls()
        Main.Main_MenuCell_RefreshInfos()
        Main.ListOfMapEditors.Remove(Me)
        Me.Dispose()
    End Sub

    Private Sub OnRedim(ByVal sender As Object, ByVal e As EventArgs) Handles Me.SizeChanged
        If Loaded Then
            SizeChange = True
        End If
    End Sub
#End Region

#Region " Clavier "

    Public Sub Map_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.F
                Main.ChangeFlip()
                RefreshDraw()
            Case Keys.R
                Main.ChangeRotate()
                RefreshDraw()
            Case Keys.Delete
                Delete_Tile()
                Main.Main_MenuCell_RefreshInfos()
            Case Keys.F1
                Main.Show_Background = False
                DrawAll()
            Case Keys.F2
                Main.Show_Ground = False
                DrawAll()
            Case Keys.F3
                Main.Show_Calque1 = False
                DrawAll()
            Case Keys.F4
                Main.Show_Calque2 = False
                DrawAll()
            Case Keys.P
                If Main.Outil = Main.Tools.CellMode Then
                    For Each MyCell As Cell In MyDatas.Cells
                        AddCellType(True, MyCell.ID)
                    Next
                Else
                    If Not IsNothing(Main.SelectedTile) And Main.SelectedTile.Type = Tile.TileType.Ground Then
                        For Each MyCell As Cell In MyDatas.Cells
                            If IsNothing(MyCell) Then Continue For
                            MyCell.Gfx1 = Main.SelectedTile
                        Next
                        DrawAll()
                    Else
                        MsgBox("La carte ne peut être recouverte que d'un sol.", MsgBoxStyle.Critical)
                    End If
                End If
            Case Keys.N
                For Each MyCell As Cell In MyDatas.Cells
                    If Not IsNothing(MyCell.Gfx3) Then
                        MyCell.UnWalkable = True
                        MyCell.LoS = False
                        MyCell.Path = False
                        MyCell.Paddock = False
                        MyCell.FightCell = 0
                    ElseIf IsNothing(MyDatas.Background) And IsNothing(MyCell.Gfx1) And IsNothing(MyCell.Gfx2) And IsNothing(MyCell.Gfx3) Then
                        MyCell.UnWalkable = True
                        MyCell.Path = False
                        MyCell.Paddock = False
                        MyCell.FightCell = 0
                    Else
                        MyCell.UnWalkable = False
                    End If
                Next
                DrawAll()
                MsgBox("Toutes les cellules vides (sans background) et celles contenant un objet en calque 2 sont désormais non marchables.", MsgBoxStyle.Information)
            Case Keys.Escape
                If Main.Mode_Trigger Then
                    Main.Mode_Trigger = False
                    MsgBox("Mode trigger désactivé.")
                End If
                If Main.Mode_EndFightAction Then
                    Main.Mode_EndFightAction = False
                    MsgBox("Mode action de fin de combat désactivé.")
                End If
                Main.SelectedTile = Nothing
            Case Keys.I
                MyDatas.Cells(SelectedCell).IO = Not MyDatas.Cells(SelectedCell).IO
            Case Keys.F5
                Tile.UnCacheAll()
                DrawAll()
                MsgBox("Cache vidé.")
        End Select
    End Sub

    Public Sub Map_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyData
            Case Keys.F1
                Main.Show_Background = True
                DrawAll()
            Case Keys.F2
                Main.Show_Ground = True
                DrawAll()
            Case Keys.F3
                Main.Show_Calque1 = True
                DrawAll()
            Case Keys.F4
                Main.Show_Calque2 = True
                DrawAll()
        End Select
    End Sub

#End Region

#Region " Souris - Controls "
    Private Sub PictureBox1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.MouseEnter
        PictureBox1.Focus()
        If IsNothing(Main.SelectedMap) OrElse Not Main.SelectedMap.Equals(Me) Then
            Main.SelectedMap = Me
            Main.MenuMap_RefreshControls()
        End If
        ' Resize
        If SizeChange Then
            Dim x As Integer = CInt(Me.Width / (SizeTable.Width * 2 + 1))
            Dim y As Integer = CInt(Me.Height / (SizeTable.Height + 2))
            If x < y Then SizeCell = x Else SizeCell = y
            SizeOfImg = New Size(SizeTable.Width * SizeCell * 2, SizeTable.Height * SizeCell)
            MyImage = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
            G = Graphics.FromImage(MyImage)
            PourceOfTile = SizeCell / SizeBaseCell
            GenerateGrid()
            DrawAll()
            Me.Size = New Size(SizeOfImg.Width + 16, SizeOfImg.Height + 38)
            SizeChange = False
        End If
    End Sub

    'Private Sub PictureBox1_Molette(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseWheel
    '    If SizeCell >= 5 And SizeCell <= 50 Then SizeCell += CInt(e.Delta / 60)
    '    If SizeCell < 5 Then SizeCell = 5
    '    If SizeCell > 50 Then SizeCell = 50
    '    SizeOfImg = New Size(SizeTable.Width * SizeCell * 2, SizeTable.Height * SizeCell + SizeCell)
    '    MyImage = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
    '    G = Graphics.FromImage(MyImage)
    '    PourceOfTile = SizeCell / SizeBaseCell
    '    GenerateGrid()
    '    DrawAll()
    'End Sub

    Private Sub PictureBox1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        ' ID cellule
        Dim ID As Integer = Get_IdCell(PictureBox1.PointToClient(Control.MousePosition))
        ' Draw
        If Not ID = HoverCell And Not ID = -1 Then
            G.Clear(Color.Black)
            G.DrawImage(Grid, New Point(0, 0))
            HoverCell = ID

            PictureBox1_MouseDown(sender, e) ' Si un bouton de souris est enfoncé
            MyDatas.Cells(HoverCell).Border(G, Brushes.Violet)

            If Not Main.Outil = Main.Tools.CellMode Then
                ' Mode construction seulement
                If Main.Outil = Main.Tools.Selector Then
                    If Not IsNothing(MyDatas.Cells(HoverCell).Gfx1) Then MyDatas.Cells(HoverCell).SurRound_Gfx1(G)
                    If Not IsNothing(MyDatas.Cells(HoverCell).Gfx2) Then MyDatas.Cells(HoverCell).SurRound_Gfx2(G)
                    If Not IsNothing(MyDatas.Cells(HoverCell).Gfx3) Then MyDatas.Cells(HoverCell).SurRound_Gfx3(G)
                End If

                If Main.Outil = Main.Tools.Brush Then
                    If Not IsNothing(Main.SelectedTile) Then
                        MyDatas.Cells(HoverCell).Draw_Tile(G, Main.SelectedTile, Main.SelectedFlip, Main.SelectedRotate)
                        If Main.SelectedTile.Type = Tile.TileType.Ground And Not IsNothing(MyDatas.Cells(HoverCell).Gfx1) Then MyDatas.Cells(HoverCell).SurRound_Gfx1(G)
                        If Main.SelectedTile.Type = Tile.TileType.Objet And Main.Calque = 1 And Not IsNothing(MyDatas.Cells(HoverCell).Gfx2) Then MyDatas.Cells(HoverCell).SurRound_Gfx2(G)
                        If Main.SelectedTile.Type = Tile.TileType.Objet And Main.Calque = 2 And Not IsNothing(MyDatas.Cells(HoverCell).Gfx3) Then MyDatas.Cells(HoverCell).SurRound_Gfx3(G)
                    End If
                End If
            End If
            PictureBox1.Image = MyImage
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        If Not e.Button = Windows.Forms.MouseButtons.None Then
            SelectedCell = HoverCell

            If Not IsNothing(Main.SelectedTile) OrElse Not Main.Outil = Main.Tools.Selector Then
                Edited = True
                If e.Button = Windows.Forms.MouseButtons.Middle Then
                    Move_Tile()
                Else
                    ' Mode Trigger
                    If Main.Mode_Trigger Then
                        Main.Trigger_Add(MyDatas.ID, SelectedCell, Me)
                        Exit Sub
                    End If

                    ' Mode EndFightAction
                    If Main.Mode_EndFightAction Then
                        Main.EndFightAction_Add(MyDatas, SelectedCell)
                        Exit Sub
                    End If

                    If Not Main.Outil = Main.Tools.CellMode Then
                        If Main.Outil = Main.Tools.Brush Then
                            ' Mode construction
                            If e.Button = Windows.Forms.MouseButtons.Right Then
                                ' Supprime
                                Delete_Tile()
                            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                                ' Poser une image
                                If Main.Outil = Main.Tools.Brush Then Add_Tile()
                            End If
                        End If
                    Else
                        ' Mode cellule
                        Dim Add As Boolean = True
                        If e.Button = Windows.Forms.MouseButtons.Right Then Add = False
                        AddCellType(Add, SelectedCell)
                    End If
                End If
            End If

            ' Infos
            Main.Main_MenuCell_RefreshInfos()
        End If
    End Sub

#End Region

#Region " Grille "

#Region " No draw "
    Public Function Get_IdCell(ByVal pts As Point) As Integer
        ' Repérage d'une cellule
        Try
            Dim x As Integer = pts.X
            Dim y As Integer = pts.Y

            Dim cell As Cell
            For Each cell In MyDatas.Cells
                If Not IsNothing(cell) Then
                    Dim num6 As Integer = (((y - cell.Location(0).Y) * (cell.Location(1).X - cell.Location(0).X)) - ((x - cell.Location(0).X) * (cell.Location(1).Y - cell.Location(0).Y)))
                    Dim num7 As Integer = (((y - cell.Location(1).Y) * (cell.Location(2).X - cell.Location(1).X)) - ((x - cell.Location(1).X) * (cell.Location(2).Y - cell.Location(1).Y)))
                    Dim num4 As Integer = (((y - cell.Location(2).Y) * (cell.Location(3).X - cell.Location(2).X)) - ((x - cell.Location(2).X) * (cell.Location(3).Y - cell.Location(2).Y)))
                    Dim num5 As Integer = (((y - cell.Location(3).Y) * (cell.Location(0).X - cell.Location(3).X)) - ((x - cell.Location(3).X) * (cell.Location(0).Y - cell.Location(3).Y)))
                    If ((((num6 >= 0) And (num7 >= 0)) And (num4 >= 0)) And (num5 >= 0)) Then
                        Return cell.ID
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox("Cellule non trouvé")
        End Try
        Return -1
    End Function

    Public Sub GenerateGrid()
        ' Génére les cellule
        For n As Integer = 0 To SizeTable.Height - 1
            For i As Integer = 0 To SizeTable.Width
                ' Initialisation des points
                Dim EcartHeight As Integer = n * SizeCell
                Dim EcartWidth As Integer = i * SizeCell * 2
                Dim PointA = New Point(SizeCell + EcartWidth, 0 + EcartHeight)
                Dim PointB = New Point(SizeCell * 2 + EcartWidth, SizeCell / 2 + EcartHeight)
                Dim PointC = New Point(SizeCell + EcartWidth, SizeCell + EcartHeight)
                Dim PointD = New Point(0 + EcartWidth, SizeCell / 2 + EcartHeight)

                Dim ID As Integer = i + (n * SizeTable.Width * 2) - n
                If ID <= MyDatas.Cells.Length - 1 Then
                    If IsNothing(MyDatas.Cells(ID)) Then
                        ' Création de la cellule
                        Dim MyCell As New Cell(Me)
                        MyCell.ID = ID
                        MyCell.Location = {PointA, PointB, PointC, PointD}

                        ' Ajout de la cellule
                        MyDatas.Cells(MyCell.ID) = MyCell
                    Else
                        MyDatas.Cells(ID).JoinMap(Me)
                        MyDatas.Cells(ID).Location = {PointA, PointB, PointC, PointD}
                    End If
                End If
            Next
        Next

        For n As Integer = 0 To SizeTable.Height - 2
            For i As Integer = 0 To SizeTable.Width - 2
                ' Initialisation des points
                Dim EcartHeight As Integer = (n * SizeCell) + (SizeCell / 2)
                Dim EcartWidth As Integer = (i * SizeCell * 2) + SizeCell
                Dim PointA = New Point(SizeCell + EcartWidth, 0 + EcartHeight)
                Dim PointB = New Point(SizeCell * 2 + EcartWidth, SizeCell / 2 + EcartHeight)
                Dim PointC = New Point(SizeCell + EcartWidth, SizeCell + EcartHeight)
                Dim PointD = New Point(0 + EcartWidth, SizeCell / 2 + EcartHeight)

                Dim ID As Integer = i + (n * (SizeTable.Width * 2) + SizeTable.Width) - n
                If ID <= MyDatas.Cells.Length - 1 Then
                    If IsNothing(MyDatas.Cells(ID)) Then
                        ' Création de la cellule
                        Dim MyCell As New Cell(Me)
                        MyCell.ID = ID
                        MyCell.Location = {PointA, PointB, PointC, PointD}

                        ' Ajout de la cellule
                        MyDatas.Cells(MyCell.ID) = MyCell
                    Else
                        MyDatas.Cells(ID).JoinMap(Me)
                        MyDatas.Cells(ID).Location = {PointA, PointB, PointC, PointD}
                    End If
                End If
            Next
        Next
    End Sub
#End Region

#Region " Draw "
    Public Sub DrawAll(Optional ByVal ShowLimit As Boolean = True)
        G.Clear(Color.Black)

        ' Calque 1 : Background
        If Main.Show_Background Then
            If Not IsNothing(MyDatas.Background) Then
                Dim Background_Pos_X As Integer = Tile.Get_Ground_Pos(MyDatas.Background.ID).X * PourceOfTile
                Dim Background_Pos_Y As Integer = Tile.Get_Ground_Pos(MyDatas.Background.ID).Y * PourceOfTile
                G.DrawImage(MyDatas.Background.Image, New Rectangle(New Point(SizeCell - Background_Pos_X, CInt(SizeCell / 2) - Background_Pos_Y), SizeOfImg))
            End If
        End If

        For Each MyCell As Cell In MyDatas.Cells
            If IsNothing(MyCell) Then Continue For
            ' Calque 2 : GfxID1
            If Main.Show_Ground Then If Not IsNothing(MyCell.Gfx1) Then MyCell.Draw_Gfx1(G)
        Next

        For Each MyCell As Cell In MyDatas.Cells
            If IsNothing(MyCell) Then Continue For
            ' Calque 3 : GfxID2
            If Main.Show_Calque1 Then If Not IsNothing(MyCell.Gfx2) Then MyCell.Draw_Gfx2(G)
        Next

        For Each MyCell As Cell In MyDatas.Cells
            If IsNothing(MyCell) Then Continue For
            ' Calque 4 : GfxID3
            If Main.Show_Calque2 Then
                If Not IsNothing(MyCell.Gfx3) Then
                    MyCell.Draw_Gfx3(G)
                    If ShowLimit And MyCell.IO Then MyCell.Draw_IO(G)
                End If
            End If
        Next

        DrawMode()

        'Calque 7 : Limitations de map
        If ShowLimit Then
            Try
                DrawGrid()
                G.DrawRectangle(Pens.Brown, SizeCell, CInt(SizeCell / 2), SizeOfImg.Width - SizeCell * 2, SizeOfImg.Height - SizeCell)
            Catch
            End Try
        End If

        ' Sauvegarde
        Grid = MyImage.Clone()
        PictureBox1.Image = MyImage
    End Sub

    Public Sub DrawMode()
        For Each MyCell As Cell In MyDatas.Cells
            If IsNothing(MyCell) Then Continue For
            ' Calque 5 : Grid
            If Main.Show_Grid Then MyCell.Border(G, Brushes.Gray)
            ' Calque 6 : Couleurs
            If Main.Outil = Main.Tools.CellMode Then MyCell.DrawMode(G)
            ' Calque 7 : ID
            If Main.Show_CellID Then MyCell.Draw_ID(G)
        Next
    End Sub

    Public Sub DrawGrid()
        ' Ligne du haut
        For i As Integer = 0 To SizeTable.Width - 1
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(3), MyDatas.Cells(i).Location(0))
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(0), MyDatas.Cells(i).Location(1))
        Next
        ' Ligne du bas
        For i As Integer = (SizeTable.Height) * ((SizeTable.Width * 2) - 1) - (SizeTable.Width * 2 - 1) To MyDatas.Cells.Length - 1
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(3), MyDatas.Cells(i).Location(2))
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(2), MyDatas.Cells(i).Location(1))
        Next
        ' Ligne de droite
        For i As Integer = SizeTable.Width - 1 To MyDatas.Cells.Length - 1 Step (SizeTable.Width * 2 - 1)
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(0), MyDatas.Cells(i).Location(1))
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(1), MyDatas.Cells(i).Location(2))
        Next
        ' Ligne de gauche
        For i As Integer = 0 To MyDatas.Cells.Length - 1 Step (SizeTable.Width * 2 - 1)
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(0), MyDatas.Cells(i).Location(3))
            G.DrawLine(Pens.Brown, MyDatas.Cells(i).Location(3), MyDatas.Cells(i).Location(2))
        Next
    End Sub

    Public Sub DrawBackground(ByVal image As Tile)
        MyDatas.Background = image
        Main.MenuMap_RefreshControls()
        DrawAll()
    End Sub

    Public Sub RefreshDraw()
        G.Clear(Color.Black)
        G.DrawImage(Grid, New Point(0, 0))
        MyDatas.Cells(HoverCell).Border(G, Brushes.Violet)

        If Not Main.Outil = Main.Tools.CellMode Then
            ' Mode construction seulement
            If Main.Outil = Main.Tools.Selector Then
                If Not IsNothing(MyDatas.Cells(HoverCell).Gfx1) Then MyDatas.Cells(HoverCell).SurRound_Gfx1(G)
                If Not IsNothing(MyDatas.Cells(HoverCell).Gfx2) Then MyDatas.Cells(HoverCell).SurRound_Gfx2(G)
                If Not IsNothing(MyDatas.Cells(HoverCell).Gfx3) Then MyDatas.Cells(HoverCell).SurRound_Gfx3(G)
            End If

            If Main.Outil = Main.Tools.Brush Then
                If Not IsNothing(Main.SelectedTile) Then
                    Select Case Main.SelectedTile.Type
                        Case Tile.TileType.Ground : MyDatas.Cells(HoverCell).SurRound_Gfx1(G)
                        Case Tile.TileType.Objet
                            Select Case Main.Calque
                                Case 1 : MyDatas.Cells(HoverCell).SurRound_Gfx2(G)
                                Case 2 : MyDatas.Cells(HoverCell).SurRound_Gfx3(G)
                            End Select
                    End Select
                    MyDatas.Cells(HoverCell).Draw_Tile(G, Main.SelectedTile, Main.SelectedFlip, Main.SelectedRotate)
                End If
            End If
        End If

        PictureBox1.Image = MyImage
    End Sub

#End Region

#End Region

#Region " Fonctions "
    Public Sub Delete_Tile(Optional ByVal calque As Integer = 0)
        If calque = 0 Then
            If Not IsNothing(MyDatas.Cells(SelectedCell).Gfx2) And Not IsNothing(MyDatas.Cells(SelectedCell).Gfx3) Then
                If Main.Calque = 1 Then MyDatas.Cells(SelectedCell).Gfx2 = Nothing
                If Main.Calque = 2 Then MyDatas.Cells(SelectedCell).Gfx3 = Nothing
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx3) Then
                MyDatas.Cells(SelectedCell).Gfx3 = Nothing
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx2) Then
                MyDatas.Cells(SelectedCell).Gfx2 = Nothing
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx1) Then
                MyDatas.Cells(SelectedCell).Gfx1 = Nothing
                DrawAll()
            End If
        Else
            If calque = 3 Then
                MyDatas.Cells(SelectedCell).Gfx3 = Nothing
            ElseIf calque = 2 Then
                MyDatas.Cells(SelectedCell).Gfx2 = Nothing
            ElseIf calque = 1 Then
                MyDatas.Cells(SelectedCell).Gfx1 = Nothing
            End If
            DrawAll()
        End If
    End Sub

    Public Sub Move_Tile(Optional ByVal calque As Integer = 0)
        If calque = 0 Then
            If Not IsNothing(MyDatas.Cells(SelectedCell).Gfx2) And Not IsNothing(MyDatas.Cells(SelectedCell).Gfx3) Then
                If Main.Calque = 1 Then
                    Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx2
                    MyDatas.Cells(SelectedCell).Gfx2 = Nothing
                End If
                If Main.Calque = 2 Then
                    Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx3
                    MyDatas.Cells(SelectedCell).Gfx3 = Nothing
                End If
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx3) Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx3
                MyDatas.Cells(SelectedCell).Gfx3 = Nothing
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx2) Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx2
                MyDatas.Cells(SelectedCell).Gfx2 = Nothing
                DrawAll()
            ElseIf Not IsNothing(MyDatas.Cells(SelectedCell).Gfx1) Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx1
                MyDatas.Cells(SelectedCell).Gfx1 = Nothing
                DrawAll()
            End If
        Else
            If calque = 3 Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx3
                MyDatas.Cells(SelectedCell).Gfx3 = Nothing
                DrawAll()
            ElseIf calque = 2 Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx2
                MyDatas.Cells(SelectedCell).Gfx2 = Nothing
                DrawAll()
            ElseIf calque = 1 Then
                Main.SelectedTile = MyDatas.Cells(SelectedCell).Gfx1
                MyDatas.Cells(SelectedCell).Gfx1 = Nothing
                DrawAll()
            End If
        End If
    End Sub


    Public Sub Add_Tile()
        If Not IsNothing(Main.SelectedTile) Then
            Select Case Main.SelectedTile.Type
                Case Tile.TileType.Ground
                    MyDatas.Cells(SelectedCell).Gfx1 = Main.SelectedTile
                    MyDatas.Cells(SelectedCell).FlipGfx1 = Main.SelectedFlip
                    MyDatas.Cells(SelectedCell).RotaGfx1 = Main.SelectedRotate

                Case Tile.TileType.Objet
                    Select Case Main.Calque
                        Case 1
                            MyDatas.Cells(SelectedCell).Gfx2 = Main.SelectedTile
                            MyDatas.Cells(SelectedCell).FlipGfx2 = Main.SelectedFlip
                            MyDatas.Cells(SelectedCell).RotaGfx2 = Main.SelectedRotate

                        Case 2
                            MyDatas.Cells(SelectedCell).Gfx3 = Main.SelectedTile
                            MyDatas.Cells(SelectedCell).FlipGfx3 = Main.SelectedFlip

                    End Select
            End Select
            DrawAll()
        End If
    End Sub

    Public Sub AddCellType(ByVal Add As Boolean, ByVal cellid As Integer)
        Dim DebugMode As Boolean = False
        If DebugMode Then MsgBox("AddCellType :" & Add.ToString & " | " & cellid & vbCrLf & "Mode_Cell_ID : " & Main.aCellMode.ToString)
        Select Case Main.aCellMode
            Case Main.CellMode.Unwalkable
                If Not MyDatas.Cells(cellid).UnWalkable = Add Then
                    MyDatas.Cells(cellid).UnWalkable = Add
                    MyDatas.Cells(cellid).Path = False
                    MyDatas.Cells(cellid).Paddock = False
                    MyDatas.Cells(cellid).FightCell = 0
                    RefreshCellType(Not Add, cellid)
                    If DebugMode Then MsgBox("UnWalkable")
                End If
                Exit Sub
            Case Main.CellMode.LoS
                If Not MyDatas.Cells(cellid).LoS = Not Add Then
                    MyDatas.Cells(cellid).LoS = Not Add
                    RefreshCellType(Not Add, cellid)
                    If DebugMode Then MsgBox("LoS")
                End If
                Exit Sub
            Case Main.CellMode.Path
                If Not MyDatas.Cells(cellid).UnWalkable AndAlso Not MyDatas.Cells(cellid).Path = Add Then
                    MyDatas.Cells(cellid).Path = Add
                    RefreshCellType(Not Add, cellid)
                    If DebugMode Then MsgBox("Path")
                End If
                Exit Sub
            Case Main.CellMode.Paddock
                If Not MyDatas.Cells(cellid).UnWalkable AndAlso Not MyDatas.Cells(cellid).Paddock = Add Then
                    MyDatas.Cells(cellid).Paddock = Add
                    RefreshCellType(Not Add, cellid)
                    If DebugMode Then MsgBox("Paddock")
                End If
                Exit Sub
            Case Main.CellMode.Fight1
                If Not MyDatas.Cells(cellid).UnWalkable Then
                    If Add Then
                        If Not MyDatas.Cells(cellid).FightCell = 1 Then
                            MyDatas.Cells(cellid).FightCell = 1
                            RefreshCellType(False, cellid)
                        End If
                    Else
                        MyDatas.Cells(cellid).FightCell = 0
                        DrawAll()
                    End If
                    If DebugMode Then MsgBox("Fight1")
                End If
                Exit Sub
            Case Main.CellMode.Fight2
                If Not MyDatas.Cells(cellid).UnWalkable Then
                    If Add Then
                        If Not MyDatas.Cells(cellid).FightCell = 2 Then
                            MyDatas.Cells(cellid).FightCell = 2
                            RefreshCellType(False, cellid)
                        End If
                    Else
                        MyDatas.Cells(cellid).FightCell = 0
                        DrawAll()
                    End If
                    If DebugMode Then MsgBox("Fight2")
                End If
                Exit Sub
        End Select
    End Sub

    Private Sub RefreshCellType(ByVal draw_all As Boolean, ByVal cellid As Integer)
        If draw_all Then
            DrawAll()
        Else
            G.Clear(Color.Black)
            G.DrawImage(Grid, New Point(0, 0))
            MyDatas.Cells(cellid).DrawMode(G)
            Grid = MyImage.Clone()
            PictureBox1.Image = MyImage
        End If
    End Sub

    Public Sub DeleteAll_ThisTile(ByVal calque As Integer)
        For Each aCell In MyDatas.Cells
            If aCell.Equals(MyDatas.Cells(SelectedCell)) Then
                Continue For
            End If
            If calque = 3 Then
                If Not IsNothing(aCell.Gfx3) AndAlso aCell.Gfx3.ID = MyDatas.Cells(SelectedCell).Gfx3.ID Then aCell.Gfx3 = Nothing
            ElseIf calque = 2 Then
                If Not IsNothing(aCell.Gfx2) AndAlso aCell.Gfx2.ID = MyDatas.Cells(SelectedCell).Gfx2.ID Then aCell.Gfx2 = Nothing
            ElseIf calque = 3 Then
                If Not IsNothing(aCell.Gfx1) AndAlso aCell.Gfx1.ID = MyDatas.Cells(SelectedCell).Gfx1.ID Then aCell.Gfx1 = Nothing
            End If
        Next
        Delete_Tile(calque)
        DrawAll()
    End Sub

    Public Sub ReplaceAll_ThisTile_By(ByVal calque As Integer, ByVal id As Integer)
        For Each aCell In MyDatas.Cells
            If aCell.Equals(MyDatas.Cells(SelectedCell)) Then
                Continue For
            End If
            If calque = 3 Then
                If Not IsNothing(aCell.Gfx3) AndAlso aCell.Gfx3.ID = MyDatas.Cells(SelectedCell).Gfx3.ID Then aCell.Gfx3 = Tile.Get_Object(CInt(id))
            ElseIf calque = 2 Then
                If Not IsNothing(aCell.Gfx2) AndAlso aCell.Gfx2.ID = MyDatas.Cells(SelectedCell).Gfx2.ID Then aCell.Gfx2 = Tile.Get_Object(CInt(id))
            ElseIf calque = 1 Then
                If Not IsNothing(aCell.Gfx1) AndAlso aCell.Gfx1.ID = MyDatas.Cells(SelectedCell).Gfx1.ID Then aCell.Gfx1 = Tile.Get_Ground(CInt(id))
            End If
        Next
        If calque = 3 Then
            MyDatas.Cells(SelectedCell).Gfx3 = Tile.Get_Object(CInt(id))
        ElseIf calque = 2 Then
            MyDatas.Cells(SelectedCell).Gfx2 = Tile.Get_Object(CInt(id))
        ElseIf calque = 1 Then
            MyDatas.Cells(SelectedCell).Gfx1 = Tile.Get_Ground(CInt(id))
        End If
        DrawAll()
    End Sub

    Private Sub MakeUnwalkableBorders()
        ' Ligne du haut
        For i As Integer = 0 To SizeTable.Width - 1 : MyDatas.Cells(i).UnWalkable = True : Next
        ' Ligne du bas
        For i As Integer = (SizeTable.Height) * ((SizeTable.Width * 2) - 1) - (SizeTable.Width * 2 - 1) To MyDatas.Cells.Length - 1 : MyDatas.Cells(i).UnWalkable = True : Next
        ' Ligne de droite
        For i As Integer = SizeTable.Width - 1 To MyDatas.Cells.Length - 1 Step (SizeTable.Width * 2 - 1) : MyDatas.Cells(i).UnWalkable = True : Next
        ' Ligne de gauche
        For i As Integer = 0 To MyDatas.Cells.Length - 1 Step (SizeTable.Width * 2 - 1) : MyDatas.Cells(i).UnWalkable = True : Next
    End Sub
#End Region

#Region " Sauvegarde "

    Public Sub Save()
        Me.Enabled = False

        ' Refraichie les variables
        Main.MenuMap_RefreshControls()

        ' Création du dossier
        IO.Directory.CreateDirectory(Main.DirectoryApply & "\Maps\" & MyDatas.ID)

        ' Sauvegarde de l'image
        Save_Img(Main.DirectoryApply & "\Maps\" & MyDatas.ID)

        ' Sauvegarde du sql
        IO.File.WriteAllText(Main.DirectoryApply & "\Maps\" & MyDatas.ID & "\" & MyDatas.ID & ".sql", MySQL.Get_SqlMap(MyDatas))

        ' Sauvegade du swf
        Save_SWF(Main.DirectoryApply & "\Maps\" & MyDatas.ID)

        ' Sauvegarde binaire
        Save_Bin(Main.DirectoryApply & "\Maps\" & MyDatas.ID)

        ' Liste des tiles utilisés
        Dim textids As String = ""
        For Each aCell As Cell In MyDatas.Cells
            If IsNothing(aCell) Then Continue For
            If Not IsNothing(aCell.Gfx1) AndAlso Not textids.Contains(aCell.Gfx1.ID) Then textids &= aCell.Gfx1.ID & vbCrLf
            If Not IsNothing(aCell.Gfx2) AndAlso Not textids.Contains(aCell.Gfx2.ID) Then textids &= aCell.Gfx2.ID & vbCrLf
            If Not IsNothing(aCell.Gfx3) AndAlso Not textids.Contains(aCell.Gfx3.ID) Then textids &= aCell.Gfx3.ID & vbCrLf
        Next
        IO.File.WriteAllText(Main.DirectoryApply & "\Maps\" & MyDatas.ID & "\GfxID utilisés.txt", textids)

        ' Insertion dans la BDD
        If Options.MyOptions.SaveSQL Then
            MySQL.Execute(MySQL.Get_SqlMap(MyDatas))
            MsgBox("La map a été inséré dans la base de données.")
        End If

        ' Copie le fichier SWF
        If Options.MyOptions.MoveFile Then
            File.Copy(Main.DirectoryApply & "\Maps\" & MyDatas.ID & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf", Options.MyOptions.SWF_PathCopy & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf", True)
        End If

        ' Supprime l'AutoSave
        If IO.File.Exists(Main.DirectoryApply & "\Maps\AutoSave\" & MyDatas.ID & "_" & MyDatas.DateMap & ".ame") Then IO.File.Delete(Main.DirectoryApply & "\Maps\AutoSave\" & MyDatas.ID)

        Me.Enabled = True

        ' Notification
        Main.ToolStripStatusLabel1.Text = "[" & Date.Now.Hour & "h" & Date.Now.Minute & "] Sauvegarde de la map n°" & MyDatas.ID & " effectué"
    End Sub

    Private Sub Save_Img(ByVal path As String)
        ' Sauvegarde de l'image
        ' Resize
        Dim oldsize As Integer = SizeCell
        SizeCell = SizeBaseCell
        SizeOfImg = New Size(SizeTable.Width * SizeCell * 2, SizeTable.Height * SizeCell)
        MyImage = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
        G = Graphics.FromImage(MyImage)
        PourceOfTile = SizeCell / SizeBaseCell
        GenerateGrid()
        ' Image modecell
        Main.Outil = Main.Tools.CellMode
        Main.Show_Grid = True
        DrawAll(True)
        MyImage.Save(path & "\" & MyDatas.ID & "_ModeCell.png")
        ' Image normale
        Main.Outil = Main.Tools.Brush
        Main.Show_Grid = False
        DrawAll(False)
        MyImage = RogneImage(MyImage, SizeCell, CInt(SizeCell / 2), SizeOfImg.Width - SizeCell * 2, SizeOfImg.Height - SizeCell)
        MyDatas.Screenshot = MyImage.Clone
        G = Graphics.FromImage(MyImage)
        G.DrawImage(AstriaMapEditor.My.Resources.logo_map, New Point(MyImage.Width - AstriaMapEditor.My.Resources.logo_map.Width - 5, MyImage.Height - AstriaMapEditor.My.Resources.logo_map.Height - 5))
        MyImage.Save(path & "\" & MyDatas.ID & ".png")
        ' Resize
        SizeCell = oldsize
        SizeOfImg = New Size(SizeTable.Width * SizeCell * 2, SizeTable.Height * SizeCell)
        MyImage = New Bitmap(SizeOfImg.Width, SizeOfImg.Height)
        G = Graphics.FromImage(MyImage)
        PourceOfTile = SizeCell / SizeBaseCell
        GenerateGrid()
        DrawAll()
    End Sub

    Private Sub Save_SWF(ByVal path As String)
        ' Sauvegade du swf
        If File.Exists(path & "\temp.flm") Then File.Delete(path & "\temp.flm")
        If File.Exists(path & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf") Then File.Delete(path & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf")
        IO.File.WriteAllText(path & "\temp.flm", Flasm.Get_FlasmCode(MyDatas))

        Flasm.Compile(path & "\temp.flm")
        System.Threading.Thread.Sleep(TIMETOSLEEP)
        If Not File.Exists(Main.DirectoryApply & "\Flasm\blank.$wf") Then System.Threading.Thread.Sleep(3000)

        If Not File.Exists(Main.DirectoryApply & "\Flasm\blank.$wf") Then
            MsgBox("Erreur de compilation du flm (" & MyDatas.ID & "_" & MyDatas.DateMap & ") : vérifiez que blank.swf ne soit pas corrompu (>100 octets)")
        Else
            IO.File.Delete(path & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf")
            IO.File.Move(Main.DirectoryApply & "\Flasm\blank.swf", path & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".swf")
            FileSystem.Rename(Main.DirectoryApply & "\Flasm\blank.$wf", Main.DirectoryApply & "\Flasm\blank.swf")
            IO.File.Delete(path & "\temp.flm")
        End If
    End Sub

    Private Sub Save_Bin(ByVal path As String)
        ' Sauvegarde binaire
        MyDatas.MapData = Builder.GetMapData(MyDatas)
        MyDatas.SaveFightPlaces()
        If Not IsNothing(MyDatas.Background) Then MyDatas.BackgroundID = MyDatas.Background.ID
        Dim FluxDeFichier As FileStream = File.Create(path & "\" & MyDatas.ID & "_" & MyDatas.DateMap & ".ame")
        Dim Serialiseur As New BinaryFormatter
        Serialiseur.Serialize(FluxDeFichier, MyDatas)
        FluxDeFichier.Close()
    End Sub

    Private Function RogneImage(ByVal ImaSource As Bitmap, ByVal xPixelDep As Int32, ByVal yPixelDep As Int32, ByVal xPixelTotal As Int32, ByVal yPixelTotal As Int32) As Bitmap
        Dim nouvImage As New Bitmap(xPixelTotal, yPixelTotal)
        Dim graph As Graphics = Graphics.FromImage(nouvImage)
        Dim rect As New Rectangle(0, 0, xPixelTotal, yPixelTotal)
        graph.DrawImage(ImaSource, rect, xPixelDep, yPixelDep, xPixelTotal, yPixelTotal, GraphicsUnit.Pixel)
        Return nouvImage
    End Function

    Private Sub TIC_AutoSave_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TIC_AutoSave.Tick
        ' Sauvegarde auto
        If Not IO.Directory.Exists(Main.DirectoryApply & "\Maps\AutoSave") Then IO.Directory.CreateDirectory(Main.DirectoryApply & "\Maps\AutoSave")
        Save_Bin(Main.DirectoryApply & "\Maps\AutoSave")
        Main.ToolStripStatusLabel1.Text = "[" & Date.Now.Hour & "h" & Date.Now.Minute & "] AutoSauvegarde de la map n°" & MyDatas.ID & " effectué (Maps\AutoSave)"
    End Sub
#End Region

End Class