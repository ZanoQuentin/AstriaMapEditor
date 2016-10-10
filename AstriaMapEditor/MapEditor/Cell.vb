Imports System.Drawing.Drawing2D
Public Class Cell

    Public Map As MapEditor

    Public ID As Integer
    Public Location() As Point
    Public Gfx1 As Tile = Nothing
    Public Gfx2 As Tile = Nothing
    Public Gfx3 As Tile = Nothing

    Public UnWalkable As Boolean = False
    Public Path As Boolean = False
    Public LoS As Boolean = True
    Public Paddock As Boolean = False
    Public TriggerCell As Boolean = False
    Public Door As Boolean = False
    Public IO As Boolean = False
    Public FightCell As Integer = 0

    Public FlipGfx1 As Boolean = False
    Public FlipGfx2 As Boolean = False
    Public FlipGfx3 As Boolean = False
    Public RotaGfx1 As Integer = 0
    Public RotaGfx2 As Integer = 0
    Public InclineSol As Integer = 1
    Public NivSol As Integer = 7

    Public Trigger As Boolean = False
    Public TriggerName As String = ""


    Public Sub New(Optional ByRef aMap As MapEditor = Nothing)
        If Not IsNothing(aMap) Then Map = aMap
    End Sub

    Public Sub JoinMap(ByRef aMap As MapEditor)
        Map = aMap
    End Sub

    Public Function GetDatas()
        Return Builder.GetCellData(Me)
    End Function

    Public Function Type(Optional ByVal _id As Integer = -1)
        If _id = -1 Then
            If UnWalkable Then Return MovementEnum.UNWALKABLE
            If Paddock Then Return MovementEnum.PADDOCK
            If Path Then Return MovementEnum.PATH
            If Door Then Return MovementEnum.DOOR
            If Trigger Then Return MovementEnum.TRIGGER
            Return MovementEnum.WALKABLE
        Else
            If _id = MovementEnum.UNWALKABLE Then
                UnWalkable = True
            Else
                UnWalkable = False
                If _id = MovementEnum.PADDOCK Then Paddock = True
                If _id = MovementEnum.PATH Then Path = True
                If _id = MovementEnum.DOOR Then Door = True
                If _id = MovementEnum.TRIGGER Then TriggerCell = True
            End If
            Return True
        End If
    End Function

#Region " Draw "

#Region " Cell "

#Region " Bases "
    Public Function Border(ByRef G As Graphics, ByVal color As Brush) As Graphics
        G.DrawPolygon(New Pen(color), {Location(0), Location(1), Location(2), Location(3)})
        Return G
    End Function

    Public Function Fill(ByRef G As Graphics, ByVal color As Brush) As Graphics
        G.FillPolygon(color, {Location(0), Location(1), Location(2), Location(3)}, FillMode.Winding)
        Return G
    End Function

    Public Function Draw_String(ByRef G As Graphics, ByVal str As String, Optional ByVal color As Brush = Nothing) As Graphics
        If IsNothing(color) Then color = Brushes.White
        G.DrawString(str, Map.Font, color, New Point(Location(3).X + Map.SizeCell - 5, Location(0).Y + (Map.SizeCell / 4)))
        Return G
    End Function
#End Region

#Region " Total "
    Public Function DrawMode(ByRef G As Graphics) As Graphics
        ' Colorie la case selon son type
        If UnWalkable Then Draw_Walkable(G)
        If Path Then Draw_Path(G)
        If Not LoS Then Draw_LoS(G)
        If Paddock Then Draw_Paddock(G)
        If FightCell = 1 Then Draw_FightCell(G, 1, Color.Red, Brushes.Red)
        If FightCell = 2 Then Draw_FightCell(G, 2, Color.Blue, Brushes.Blue)
        If Trigger Then Draw_Trigger(G)
        'If Selected Then Draw_Selected(G)
        Return G
    End Function
#End Region

#Region " Draw fix "
    Public Function Draw_Walkable(ByRef G As Graphics)
        ' Replie la cellule
        Fill(G, New SolidBrush(Color.FromArgb(50, Color.Red)))
        ' Trace une croix
        Dim A As New Point(Location(0).X + (Map.SizeCell / 5), Location(3).Y - (Map.SizeCell / 10))
        Dim D As New Point(Location(0).X - (Map.SizeCell / 5), A.Y)
        Dim C As New Point(D.X, Location(3).Y + (Map.SizeCell / 10))
        Dim B As New Point(A.X, C.Y)
        G.DrawLine(Pens.DarkRed, A, C)
        G.DrawLine(Pens.DarkRed, B, D)
        ' Change la couleur des bordures
        Border(G, Brushes.DarkRed)
        Return G
    End Function

    Public Function Draw_Path(ByRef G As Graphics)
        ' Remplie la cellule
        Fill(G, New SolidBrush(Color.FromArgb(50, Color.Yellow)))
        ' Change la couleur des bordures
        Border(G, Brushes.Yellow)
        ' Dessine une ellipse
        Dim aRect As New Rectangle(New Point(Location(0).X - (Map.SizeCell / 5), Location(3).Y - (Map.SizeCell / 10)), New Size(Map.SizeCell / 2.5, Map.SizeCell / 5))
        G.DrawEllipse(Pens.Yellow, aRect)
        Return G
    End Function

    Public Function Draw_LoS(ByRef G As Graphics)
        ' Remplie la cellule
        Dim brush As New SolidBrush(Color.FromArgb(50, Color.Blue))
        Fill(G, brush)
        ' Trace une mini cellule
        Dim A As New Point(Location(0).X, Location(0).Y + (Map.SizeCell / 4))
        Dim B As New Point(Location(1).X - (Map.SizeCell / 2), Location(1).Y)
        Dim C As New Point(Location(2).X, Location(2).Y - (Map.SizeCell / 4))
        Dim D As New Point(Location(3).X + (Map.SizeCell / 2), Location(3).Y)
        G.DrawPolygon(Pens.DarkBlue, {A, B, C, D})
        ' Remplie la mini cellule
        G.FillPolygon(brush, {A, B, C, D}, FillMode.Winding)
        Return G
    End Function

    Public Function Draw_Paddock(ByRef G As Graphics)
        ' Remplie la cellule
        Fill(G, New SolidBrush(Color.FromArgb(50, Color.SaddleBrown)))
        ' Change la couleur des bordures
        Border(G, Brushes.SaddleBrown)
        ' Dessine un rectangle
        Dim aRect As New Rectangle(New Point(Location(0).X - (Map.SizeCell / 5), Location(3).Y - (Map.SizeCell / 10)), New Size(Map.SizeCell / 2.5, Map.SizeCell / 5))
        G.DrawRectangle(Pens.SaddleBrown, aRect)
        Return G
    End Function

    Public Function Draw_FightCell(ByRef G As Graphics, ByVal number As Integer, ByVal backgroundcolor As Color, ByVal bordercolor As Brush)
        Fill(G, New SolidBrush(Color.FromArgb(90, backgroundcolor)))
        G.DrawString(number, Map.Font, Brushes.White, New Point(Location(3).X + Map.SizeCell - 5, Location(0).Y + (Map.SizeCell / 4)))
        Border(G, bordercolor)
        Return G
    End Function

    Public Function Draw_Trigger(ByRef G As Graphics)
        Fill(G, New SolidBrush(Color.FromArgb(90, Color.Yellow)))
        Draw_String(G, TriggerName)
        Border(G, Brushes.Blue)
        Return G
    End Function

    Public Function Draw_IO(ByRef G As Graphics) As Graphics
        Draw_String(G, "IO", Brushes.Yellow)
        Return G
    End Function

    Public Function Draw_ID(ByRef G As Graphics) As Graphics
        Draw_String(G, ID.ToString)
        Return G
    End Function

    'Public Function Draw_Selected(ByRef G As Graphics)
    '    Fill(G, New SolidBrush(Color.FromArgb(90, Color.Azure)))
    '    Border(G, Brushes.Azure)
    '    Return G
    'End Function
#End Region

#End Region

#Region " Images "
    Public Function Draw_Gfx1(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx1) Then Return Draw_Tile(G, Gfx1, FlipGfx1, RotaGfx1)
        Return G
    End Function

    Public Function Draw_Gfx2(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx2) Then Return Draw_Tile(G, Gfx2, FlipGfx2, RotaGfx2)
        Return G
    End Function

    Public Function Draw_Gfx3(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx3) Then Return Draw_Tile(G, Gfx3, FlipGfx3, 0)
        Return G
    End Function

    Public Function Draw_Tile(ByRef G As Graphics, ByVal aTile As Tile, ByVal Flip As Boolean, ByVal Rotate As Integer) As Graphics
        Dim aImage As Image = aTile.Image(True).Clone
        Dim SizeImage As New Size(aImage.Size.Width * Map.PourceOfTile, aImage.Size.Height * Map.PourceOfTile)

        ' Positions
        Dim Base_X As Integer
        Dim Base_Y As Integer
        Dim aPos As Tile.Pos
        If aTile.Type = Tile.TileType.Ground Then
            If Tile.Get_Ground_Pos(aTile.ID).ID = 0 Then
                Dim pos As Tile.Pos
                pos.ID = aTile.ID
                pos.X = CInt(aImage.Width / 2)
                pos.Y = CInt(aImage.Height / 2)
                Tile.Pos_Grounds(Tile.Count_Grounds) = pos
            End If
            aPos = Tile.Get_Ground_Pos(aTile.ID)
        Else
            If Tile.Get_Object_Pos(aTile.ID).ID = 0 Then
                Dim pos As Tile.Pos
                pos.ID = aTile.ID
                pos.X = CInt(aImage.Width / 2)
                pos.Y = CInt(aImage.Height / 2)
                Tile.Pos_Objects(Tile.Count_Objects) = pos
            End If
            aPos = Tile.Get_Object_Pos(aTile.ID)
        End If
        Base_X = aPos.X
        Base_Y = aPos.Y
        Dim Pos_X As Integer = Base_X * Map.PourceOfTile
        Dim Pos_Y As Integer = Base_Y * Map.PourceOfTile

        ' Flip
        If Flip Then
            aImage.RotateFlip(RotateFlipType.RotateNoneFlipX)
            If aTile.Type = Tile.TileType.Objet Then Pos_X = aImage.Width - (Base_X * Map.PourceOfTile)
        End If

        ' Rotation
        If Not Rotate = 0 Then
            Select Case Rotate
                Case 1
                    aImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
                    ' Modifications width/height
                    SizeImage.Height = Math.Ceiling(aImage.Height / 100 * 51.85 * Map.PourceOfTile)
                    SizeImage.Width = Math.Ceiling(aImage.Width / 100 * 192.86 * Map.PourceOfTile)
                    ResizeImg(aImage, SizeImage.Height, SizeImage.Width) ' Inversion dû à la rotation
                    Pos_Y = (Base_X * Map.PourceOfTile) / 100 * 51.85
                    Pos_X = SizeImage.Width - (Base_Y * Map.PourceOfTile) / 100 * 192.86
                Case 2
                    aImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
                    If aTile.Type = Tile.TileType.Objet Then Pos_X = SizeImage.Width - (Base_X * Map.PourceOfTile)
                    Pos_Y = SizeImage.Height - (Base_Y * Map.PourceOfTile)
                Case 3
                    aImage.RotateFlip(RotateFlipType.Rotate270FlipNone)
                    ' Modifications width/height
                    SizeImage.Height = Math.Ceiling(aImage.Height / 100 * 51.85 * Map.PourceOfTile)
                    SizeImage.Width = Math.Ceiling(aImage.Width / 100 * 192.86 * Map.PourceOfTile)
                    ResizeImg(aImage, SizeImage.Height, SizeImage.Width) ' Inversion dû à la rotation
                    Pos_Y = (Base_X * Map.PourceOfTile) / 100 * 51.85
                    Pos_X = (Base_Y * Map.PourceOfTile) / 100 * 192.86
            End Select
        End If

        ' Image
        G.DrawImage(aImage, New Rectangle(New Point(Location(3).X + Map.SizeCell - Pos_X, Location(2).Y - (Map.SizeCell / 2) - Pos_Y), SizeImage))
        aImage.Dispose()

        Return G
    End Function

#Region " Functions images "

    Private Function ResizeImg(ByRef aImage As Image, ByVal newWidth As Integer, ByVal NewHeight As Integer) As Image
        Dim thumb As New Bitmap(newWidth, NewHeight)
        Dim gra As Graphics = Graphics.FromImage(thumb)
        gra.DrawImage(aImage, New Rectangle(0, 0, newWidth, NewHeight), New Rectangle(0, 0, aImage.Width, aImage.Height), GraphicsUnit.Pixel)
        gra.Dispose()
        aImage = thumb.Clone
        thumb.Dispose()
        Return aImage
    End Function

#End Region

#End Region

#Region " SurRound "
    Public Function SurRound_Gfx1(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx1) Then Return SurRound(G, Gfx1, FlipGfx1, RotaGfx1)
        Return G
    End Function

    Public Function SurRound_Gfx2(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx2) Then Return SurRound(G, Gfx2, FlipGfx2, RotaGfx2)
        Return G
    End Function

    Public Function SurRound_Gfx3(ByRef G As Graphics) As Graphics
        If Not IsNothing(Gfx3) Then Return SurRound(G, Gfx3, FlipGfx3, 0)
        Return G
    End Function

    Public Function SurRound(ByRef G As Graphics, ByVal aTile As Tile, ByVal Flip As Boolean, ByVal Rotate As Integer) As Graphics
        If Not IsNothing(aTile) Then
            Dim aImage As Image = aTile.Image.Clone
            Dim SizeImage As New Size(aImage.Size.Width * Map.PourceOfTile, aImage.Size.Height * Map.PourceOfTile)

            ' Positions
            Dim Base_X As Integer
            Dim Base_Y As Integer
            Dim aPos As Tile.Pos
            If aTile.Type = Tile.TileType.Ground Then
                aPos = Tile.Get_Ground_Pos(aTile.ID)
            Else
                aPos = Tile.Get_Object_Pos(aTile.ID)
            End If
            Base_X = aPos.X
            Base_Y = aPos.Y
            Dim Pos_X As Integer = Base_X * Map.PourceOfTile
            Dim Pos_Y As Integer = Base_Y * Map.PourceOfTile

            ' Flip
            If Flip Then
                aImage.RotateFlip(RotateFlipType.RotateNoneFlipX)
                If aTile.Type = Tile.TileType.Objet Then Pos_X = aImage.Width - (Base_X * Map.PourceOfTile)
            End If

            ' Rotation
            If Not Rotate = 0 Then
                Select Case Rotate
                    Case 1
                        aImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
                        ' Modifications width/height
                        SizeImage.Height = Math.Ceiling(aImage.Height / 100 * 51.85 * Map.PourceOfTile)
                        SizeImage.Width = Math.Ceiling(aImage.Width / 100 * 192.86 * Map.PourceOfTile)
                        ResizeImg(aImage, SizeImage.Height, SizeImage.Width) ' Inversion dû à la rotation
                        Pos_Y = (Base_X * Map.PourceOfTile) / 100 * 51.85
                        Pos_X = SizeImage.Width - (Base_Y * Map.PourceOfTile) / 100 * 192.86
                    Case 2
                        aImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
                        If aTile.Type = Tile.TileType.Objet Then Pos_X = SizeImage.Width - (Base_X * Map.PourceOfTile)
                        Pos_Y = SizeImage.Height - (Base_Y * Map.PourceOfTile)
                    Case 3
                        aImage.RotateFlip(RotateFlipType.Rotate270FlipNone)
                        ' Modifications width/height
                        SizeImage.Height = Math.Ceiling(aImage.Height / 100 * 51.85 * Map.PourceOfTile)
                        SizeImage.Width = Math.Ceiling(aImage.Width / 100 * 192.86 * Map.PourceOfTile)
                        ResizeImg(aImage, SizeImage.Height, SizeImage.Width) ' Inversion dû à la rotation
                        Pos_Y = (Base_X * Map.PourceOfTile) / 100 * 51.85
                        Pos_X = (Base_Y * Map.PourceOfTile) / 100 * 192.86
                End Select
            End If

            Dim MyRectangle As New Rectangle(New Point(Location(3).X + Map.SizeCell - Pos_X, Location(2).Y - (Map.SizeCell / 2) - Pos_Y), SizeImage)
            G.DrawRectangle(Pens.White, MyRectangle)
        End If
        Return G
    End Function

#End Region

#End Region

End Class