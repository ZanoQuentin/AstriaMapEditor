<Serializable()>
Public Class Tile

    Public ID As Integer
    Public PathImage As String
    Public Folder As String
    Public Type As TileType

    <NonSerialized()> Private ImageLoaded As Bitmap = Nothing

    <NonSerialized()> Public Shared Pos_Objects(50000) As Pos
    <NonSerialized()> Public Shared Pos_Grounds(50000) As Pos


    Public Enum TileType
        Background = 0
        Ground = 1
        Objet = 2
    End Enum

    Public Sub New(ByVal _id As Integer, ByVal _image As String, ByVal _folder As String, ByVal _type As TileType)
        ID = _id
        PathImage = _image
        Folder = _folder
        Type = _type
    End Sub

    Public Function Image(Optional ByVal cache As Boolean = False) As Bitmap
        If cache Then
            If IsNothing(ImageLoaded) Then ImageLoaded = DirectCast(Drawing.Image.FromFile(PathImage), Bitmap)
            Return ImageLoaded
        End If
        Return DirectCast(Drawing.Image.FromFile(PathImage), Bitmap)
    End Function

#Region " Shared "

    Public Shared Function Get_Object(ByVal id As Integer) As Tile
        For Each anObject As Tile In Main.List_Objects
            If Not IsNothing(anObject) Then
                If anObject.ID = id Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function Get_Ground(ByVal id As Integer) As Tile
        For Each aGround As Tile In Main.List_Grounds
            If Not IsNothing(aGround) Then
                If aGround.ID = id Then Return aGround
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function Get_Background(ByVal id As Integer) As Tile
        For Each aBackground As Tile In Main.List_Backgrounds
            If Not IsNothing(aBackground) Then
                If aBackground.ID = id Then Return aBackground
            End If
        Next
        Return Nothing
    End Function

    Public Shared Sub UnCacheAll()
        For Each aBackground As Tile In Main.List_Backgrounds
            If Not IsNothing(aBackground) AndAlso Not IsNothing(aBackground.ImageLoaded) Then
                aBackground.ImageLoaded.Dispose()
                aBackground.ImageLoaded = Nothing
            End If
        Next
        For Each aGround As Tile In Main.List_Grounds
            If Not IsNothing(aGround) AndAlso Not IsNothing(aGround.ImageLoaded) Then
                aGround.ImageLoaded.Dispose()
                aGround.ImageLoaded = Nothing
            End If
        Next
        For Each anObject As Tile In Main.List_Objects
            If Not IsNothing(anObject) AndAlso Not IsNothing(anObject.ImageLoaded) Then
                anObject.ImageLoaded.Dispose()
                anObject.ImageLoaded = Nothing
            End If
        Next
    End Sub

    Public Shared Function Get_Object_Pos(ByVal id As Integer) As Pos
        For Each anObject As Pos In Pos_Objects
            If Not IsNothing(anObject) Then
                If anObject.ID = id Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function Get_Ground_Pos(ByVal id As Integer) As Pos
        For Each aGround As Pos In Pos_Grounds
            If Not IsNothing(aGround) Then
                If aGround.ID = id Then Return aGround
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function Count_Grounds() As Integer
        Dim nb As Integer = 0
        For Each aGround As Pos In Pos_Grounds
            If Not aGround.ID = 0 Then
                nb += 1
            End If
        Next
        Return nb
    End Function

    Public Shared Function Count_Objects() As Integer
        Dim nb As Integer = 0
        For Each aObject As Pos In Pos_Objects
            If Not aObject.ID = 0 Then
                nb += 1
            End If
        Next
        Return nb
    End Function

#End Region

    <Serializable()>
    Public Structure Pos
        Public ID As Integer
        Public X As Integer
        Public Y As Integer
    End Structure

End Class
