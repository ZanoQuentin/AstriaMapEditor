Module FileManager

#Region " Géoposition "

    Const Geo_Directory As String = "Géopositions"

    Public Function Where_Geo_Directory(Optional ByVal Island_Name As String = "") As String
        If Island_Name = "" Then
            Return Main.DirectoryApply & "\" & Geo_Directory
        Else
            Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name
        End If
    End Function

#Region " SQL "
    Public Function Where_Geo_Trigger(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\scripted_cells.sql"
    End Function

    Public Function Where_Geo_UpdateMapPos(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\update_maps_pos.sql"
    End Function

    Public Function Where_Geo_SQLHouses(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\install_houses.sql"
    End Function

    Public Function Where_Geo_SQLMountParks(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\install_mountparks.sql"
    End Function
    Public Function Where_Geo_SQLMountParksLogs(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\mountparks.logs.txt"
    End Function
#End Region

#Region " SWF"

    Public Function Where_Geo_SWFMapPos(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\maps.swf.txt"
    End Function

    Public Function Where_Geo_SWFHouses(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\houses.swf.txt"
    End Function
    Public Function Where_Geo_SWFHousesLogs(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\houses.logs.txt"
    End Function

#End Region

#Region " Sauvegarde "
    Public Function Where_Geo_SaveFile(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\" & Island_Name & ".geo"
    End Function

    Public Function Where_Geo_Image(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\" & Island_Name & ".png"
    End Function

    Public Function Where_Geo_ImageMode(ByVal Island_Name As String) As String
        Return Main.DirectoryApply & "\" & Geo_Directory & "\" & Island_Name & "\" & Island_Name & "_Mode.png"
    End Function
#End Region

#End Region

End Module
