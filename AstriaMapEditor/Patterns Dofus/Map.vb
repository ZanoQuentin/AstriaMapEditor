Imports Microsoft.VisualBasic.CompilerServices

<Serializable()>
Public Class Map

    <NonSerialized()> Public IDClient As Integer = 0
    Public Screenshot As Bitmap = Nothing
    <NonSerialized()> Public IsEditing As Boolean = False

    Public ID As Integer = 1
    Public DateMap As String = "AME"
    <NonSerialized()> Public Background As Tile = Nothing
    Public BackgroundID As Integer = 0
    Public Musique As Integer = 0
    Public MusiqueName As String = ""
    Public Ambiance As Integer = 0
    Public BOutdoor As Boolean = False
    Public Capabilities As Integer = 0
    Public Width As Integer = 15
    Public Height As Integer = 17
    Public Key As String = Nothing
    Public MapData As String = ""
    Public FightPlaces As String = ""
    Public NbGroups As Integer = 5
    Public GroupMaxSize As Integer = 6
    <NonSerialized()> Public Cells(Height * (Width * 2 - 1) - Width) As Cell

    Public X As Integer = 0
    Public Y As Integer = 0
    Public Area As Integer = 0
    Public SubArea As Integer = 0
    Public SuperArea As Integer = 0

    Public NextRoom As Integer = 0
    Public NextCell As Integer = 0

    Public Mobs As String = ""

    Public GroupFixe_Mobs As String = ""
    Public GroupFixe_Cell As Integer = 0


    Public Sub New()
        AddMap(Me)
    End Sub

    Public Sub Load()
        If Not MapData = "" Then
            If Key = "" AndAlso Crypted() Then Key = InputBox("La MapData de cette map est crypté, veuillez entrer la clé de décryptage de celle ci.", "Map crypté")

            Dim aCells(Height * (Width * 2 - 1) - Width) As Cell
            For i = 0 To (Height * (Width * 2 - 1) - Width)
                aCells(i) = New Cell()
                aCells(i).ID = i
            Next
            Cells = aCells
            UncompressMap()
        End If
        If Not FightPlaces = "" Then LoadFightPlaces()
        If Not BackgroundID = 0 Then Background = Tile.Get_Background(BackgroundID)
    End Sub

    Private Function Crypted()
        ' Décryptage de la map data si crypt
        '       On compte le nombre de chiffres
        Dim NbNumerics As Integer = 0
        For Each a As Char In MapData.ToCharArray
            If IsNumeric(a) Then NbNumerics += 1
        Next
        '       Si le nombre de chiffres dépasse 1000, la mapdata paraît crypté
        If NbNumerics > 1000 Then
            Return True
        End If
        Return False
    End Function

    Public Sub SaveFightPlaces()
        FightPlaces = FightCellManager.GetHashCode(Me)
    End Sub

    Public Sub LoadFightPlaces()
        If Not FightPlaces = "" Then Cells = FightCellManager.ParseFightCell(FightPlaces, Cells)
    End Sub

    Private Sub UncompressCell(ByVal CellData As String, ByVal CellID As Integer)
        Dim numArray As Integer() = New Integer(10 - 1) {}

        For i = 0 To (CellData.Length - 1)
            numArray(i) = CInt(Decryptage.HashCodes(CellData.Chars(i)))
        Next

        Cells(CellID).LoS = ((numArray(0) And 1) > 0)
        Cells(CellID).RotaGfx1 = ((numArray(1) And &H30) >> 4)
        Cells(CellID).NivSol = (numArray(1) And 15)
        Cells(CellID).Type(((numArray(2) And &H38) >> 3) And -1025)
        Cells(CellID).Gfx1 = Tile.Get_Ground((((numArray(0) And &H18) << 6) + ((numArray(2) And 7) << 6)) + numArray(3))
        Cells(CellID).InclineSol = ((numArray(4) And 60) >> 2)
        Cells(CellID).FlipGfx1 = (((numArray(4) And 2) >> 1) > 0)
        Cells(CellID).Gfx2 = Tile.Get_Object((((((numArray(0) And 4) << 11) + ((numArray(4) And 1) << 12)) + (numArray(5) << 6)) + numArray(6)))
        Cells(CellID).RotaGfx2 = ((numArray(7) And &H30) >> 4)
        Cells(CellID).FlipGfx2 = (((numArray(7) And 8) >> 3) > 0)
        Cells(CellID).FlipGfx3 = (((numArray(7) And 4) >> 2) > 0)
        Cells(CellID).IO = (((numArray(7) And 2) >> 1) > 0)
        Cells(CellID).Gfx3 = Tile.Get_Object(((((numArray(0) And 2) << 12) + ((numArray(7) And 1) << 12)) + (numArray(8) << 6)) + numArray(9))
    End Sub

    Public Sub UncompressMap()
        ' Decrypte la key
        Try
            If Not Key = Nothing Then
                Key = Trim(Key)
                Key = Key.Replace(vbCr, "").Replace(vbLf, "").Replace(vbCrLf, "")
                Key = Decryptage.PrepareKey(Key)
                Dim checksum As Integer = CInt((Convert.ToInt64(Decryptage.Checksum(Key), &H10) * 2))
                MapData = Decryptage.DecypherData(MapData, Key, checksum)
                Key = ""
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        ' Décompile la mapdata
        Dim num3 As Integer = ((Cells.Length * 10) - 10)
        Dim i As Integer = 0
        Do While (i <= num3)
            UncompressCell(MapData.Substring(i, 10), CInt(Math.Round(CDbl((CDbl(i) / 10)))))
            i = (i + 10)
        Loop
    End Sub

#Region " Shared "
    <NonSerialized()> Public Shared ListOfMaps As New List(Of Map)

    Public Shared Sub AddMap(ByVal aMap As Map)
        Dim ok As Boolean = True
        For Each myMap As Map In ListOfMaps
            If myMap.ID = aMap.ID Then
                ok = False
                Exit For
            End If
        Next
        If ok Then
            aMap.IDClient = ListOfMaps.Count
            ListOfMaps.Add(aMap)
        Else
            aMap.Finalize()
        End If
    End Sub

    Public Shared Function IdWasLoaded(ByVal aMap As Map) As Boolean
        For Each myMap As Map In ListOfMaps
            If myMap.ID = aMap.ID And myMap.MapData = aMap.MapData Then
                Return True
                Exit For
            End If
        Next
        Return False
    End Function

    Public Shared Function GetByID(ByVal ID As Integer) As Map
        For Each myMap As Map In ListOfMaps
            If myMap.ID = ID Then
                Return myMap
                Exit For
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function Get_Capabilities(ByVal canTP As Boolean, ByVal canSave As Boolean, ByVal canAttackCB As Boolean, ByVal canChallCB As Boolean) As String
        Dim stre As String = (If(Not canTP, "1", "0")) & (If(Not canSave, "1", "0")) & (If(Not canAttackCB, "1", "0")) & (If(Not canChallCB, "1", "0"))
        Return Convert.ToInt32(stre, 2)
    End Function

#End Region

End Class
