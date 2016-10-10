<Serializable()>
Public Class CellGeo

    Public ID As Integer
    Public Location(4) As Point

    <NonSerialized()> Public MyMap As Map
    Public MapID As Integer
    Public x_pos As Integer
    Public y_pos As Integer
    <NonSerialized()> Public Geo As Geoposition
    <NonSerialized()> Public Selected As Boolean = False

    Public Sub New(ByRef aGeo As Geoposition, ByVal aID As Integer)
        Geo = aGeo
        ID = aID
    End Sub

    Public Function DrawImage(ByVal G As Graphics, ByVal aImage As Bitmap)
        G.DrawImage(aImage, New Rectangle(Location(0), Geo.SizeCell))
        Return G
    End Function

    Public Function DrawBorder(ByRef G As Graphics, ByVal color As Brush) As Graphics
        G.DrawRectangle(New Pen(color), New Rectangle(Location(0), Geo.SizeCell))
        Return G
    End Function

    Public Function DrawGeoPos(ByRef G As Graphics, ByVal color As Brush) As Graphics
        G.DrawString(x_pos & ", " & y_pos & " (" & MapID & ")", Geo.Font, Brushes.White, New Point(Location(3).X + 10, Location(0).Y + 10))
        Return G
    End Function

    Public Function DrawFill(ByRef G As Graphics, ByVal color As Brush) As Graphics
        G.FillRectangle(color, New Rectangle(Location(0), Geo.SizeCell))
        Return G
    End Function

    Public Function DrawMode(ByRef G As Graphics) As Graphics
        If Selected Then
            DrawFill(G, New SolidBrush(Color.FromArgb(50, Color.Purple)))
            DrawBorder(G, Brushes.Purple)
        End If
        Return G
    End Function

    Public Function Draw_String(ByRef G As Graphics, ByVal str As String, Optional ByVal color As Brush = Nothing) As Graphics
        If IsNothing(color) Then color = Brushes.White
        G.DrawString(str, Geo.Font, color, New Point(Location(3).X + 10, Location(0).Y + 10))
        Return G
    End Function

End Class