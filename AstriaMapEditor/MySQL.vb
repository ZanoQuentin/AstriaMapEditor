Imports MySql.Data.MySqlClient
Module MySQL

    Public Bdd As New MySqlConnection

#Region " Open "
    Public Sub Open()
        Try
            Dim ConnexionString As String = String.Concat("server=", Options.MyOptions.MySQL_Host, ";" _
                                    , "uid=", Options.MyOptions.MySQL_User, ";" _
                                    , "pwd='", Options.MyOptions.MySQL_Password, "';" _
                                    , "database=", Options.MyOptions.MySQL_Database, ";")

            Bdd.ConnectionString = ConnexionString
            Bdd.Open()
        Catch ex As MySqlException
            MsgBox("Connexion impossible avec la base de données." & vbCrLf & vbCrLf & "Veuillez vérifier que les informations enregistrées dans la configuration sont corectes.", MsgBoxStyle.Critical, "Impossible de se connecter à la base de données")
        End Try
    End Sub
#End Region

#Region " Commandes "
    Public Sub Execute(ByVal Commande As String)
        Open()
        Dim SQLCommandDelete As New MySqlCommand(Commande, Bdd)
        SQLCommandDelete.ExecuteNonQuery()
        Bdd.Close()
    End Sub
#End Region

#Region " Get "
    Public Function Get_SqlMap(ByVal MyDatas As Map) As String
        Dim CommandeSQL As String = _
        "DELETE FROM `" & Options.MyOptions.MySQL_TableName_Maps & "` WHERE (`" & Options.MyOptions.MySQL_Maps_ID & "`='" & MyDatas.ID & "');" & vbCrLf & _
        "INSERT INTO `" & Options.MyOptions.MySQL_TableName_Maps & "` " & _
                                "(" & _
                                    "`" & Options.MyOptions.MySQL_Maps_ID & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Date & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Width & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Height & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_MapData & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Key & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_FightPlaces & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Monsters & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Capabilities & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_Pos & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_NbGroups & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Maps_SizeMaxGroup & "`" & _
                                ") VALUES (" & _
                                    "'" & MyDatas.ID & "', " & _
                                    "'" & MyDatas.DateMap & "', " & _
                                    "'" & MyDatas.Width & "', " & _
                                    "'" & MyDatas.Height & "', " & _
                                    "'" & Builder.GetMapData(MyDatas) & "', " & _
                                    "'" & MyDatas.Key & "', " & _
                                    "'" & FightCellManager.GetHashCode(MyDatas) & "', " & _
                                    "'" & MyDatas.Mobs & "', " & _
                                    "'" & MyDatas.Capabilities & "', " & _
                                    "'" & MyDatas.X & "," & MyDatas.Y & "," & MyDatas.SubArea & "', " & _
                                    "'" & MyDatas.NbGroups & "', " & _
                                    "'" & MyDatas.GroupMaxSize & "'" & _
                                ");"
        If MyDatas.NextCell <> 0 And MyDatas.NextRoom <> 0 Then
            CommandeSQL &= vbCrLf & Get_SqlDungeon(MyDatas.ID, MyDatas.NextRoom, MyDatas.NextCell)
        End If

        If MyDatas.GroupFixe_Mobs <> "" Then
            CommandeSQL &= vbCrLf & Get_SqlGroupMobsFix(MyDatas.ID, MyDatas.GroupFixe_Cell, MyDatas.GroupFixe_Mobs)
        End If

        Return CommandeSQL
    End Function

    Public Function Get_SqlDungeon(ByVal mapid As Integer, ByVal tomapid As Integer, ByVal tocellid As Integer) As String
        Return _
                "DELETE FROM `" & Options.MyOptions.MySQL_TableName_EndFight & "` WHERE (`" & Options.MyOptions.MySQL_EndFight_MapID & "`='" & mapid & "');" & vbCrLf & _
                "INSERT INTO `" & Options.MyOptions.MySQL_TableName_Mobs & "` VALUES ('" & mapid & "', '4', '0', '" & tomapid & "," & tocellid & "', '');"
    End Function

    Public Function Get_SqlGroupMobsFix(ByVal mapid As Integer, ByVal cellid As Integer, ByVal group As String) As String
        Return _
                "DELETE FROM `" & Options.MyOptions.MySQL_TableName_Mobs & "` WHERE (`" & Options.MyOptions.MySQL_Mobs_MapID & "`='" & mapid & "');" & vbCrLf & _
                "INSERT INTO `" & Options.MyOptions.MySQL_TableName_EndFight & "` " & _
                                "(" & _
                                    "`" & Options.MyOptions.MySQL_Mobs_MapID & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Mobs_CellID & "`, " & _
                                    "`" & Options.MyOptions.MySQL_Mobs_GroupData & "`" & _
                                ") VALUES (" & _
                                    "'" & mapid & "', " & _
                                    "'" & cellid & "', " & _
                                    "'" & group & "'" & _
                                ");"
    End Function

    Public Function Get_SqlTrigger(ByVal mapid As Integer, ByVal cellid As Integer, ByVal tomapid As Integer, ByVal tocellid As Integer) As String
        Return _
                "DELETE FROM `" & Options.MyOptions.MySQL_TableName_Triggers & "` WHERE `" & Options.MyOptions.MySQL_Triggers_MapID & "`=" & mapid & " AND `" & Options.MyOptions.MySQL_Triggers_CellID & "`=" & cellid & " AND `" & Options.MyOptions.MySQL_Triggers_Args & "`='" & tomapid & "," & tocellid & "';" & vbCrLf & _
                "INSERT INTO `" & Options.MyOptions.MySQL_TableName_Triggers & "` VALUES(" & mapid & ", " & cellid & ", 0, 1, '" & tomapid & "," & tocellid & "', -1);" & vbCrLf
    End Function

    Public Function Get_SqlHouse(ByVal ID As Integer, ByVal ByMapID As Integer, ByVal ByCellID As Integer, ByVal Price As Integer, ByVal ToMapID As Integer, ByVal ToCellID As Integer) As String
        Return _
                "DELETE FROM `" & Options.MyOptions.MySQL_TableName_Houses & "` WHERE `" & Options.MyOptions.MySQL_Houses_ID & "`=" & ID & ";" & vbCrLf & _
                "INSERT INTO `" & Options.MyOptions.MySQL_TableName_Houses & "`(" & _
                                                                                    Options.MyOptions.MySQL_Houses_ID & ", " & _
                                                                                    Options.MyOptions.MySQL_Houses_MapID & ", " & _
                                                                                    Options.MyOptions.MySQL_Houses_CellID & ", " & _
                                                                                    Options.MyOptions.MySQL_Houses_Price & ", " & _
                                                                                    Options.MyOptions.MySQL_Houses_ToMapID & ", " & _
                                                                                    Options.MyOptions.MySQL_Houses_ToCellID & ") VALUES(" _
                                                                                    & ID & ", " & ByMapID & ", " & ByCellID & ", " & Price & ", " & ToMapID & ", " & ToCellID & _
                                                                                    ");" & vbCrLf
    End Function

    Public Function Get_SqlMountpark(ByVal MapID As Integer, ByVal CellID As Integer, ByVal Size As Integer, ByVal Price As Integer) As String
        Return _
                "DELETE FROM `" & Options.MyOptions.MySQL_TableName_MountParks & "` WHERE `" & Options.MyOptions.MySQL_MountParks_MapID & "`=" & MapID & " AND  `" & Options.MyOptions.MySQL_MountParks_CellID & "`=" & CellID & ";" & vbCrLf & _
                "INSERT INTO `" & Options.MyOptions.MySQL_TableName_MountParks & "`(" & _
                                                                                    Options.MyOptions.MySQL_MountParks_MapID & ", " & _
                                                                                    Options.MyOptions.MySQL_MountParks_CellID & ", " & _
                                                                                    Options.MyOptions.MySQL_MountParks_Price & ", " & _
                                                                                    Options.MyOptions.MySQL_MountParks_Size & ") VALUES(" & _
                                                                                    MapID & ", " & CellID & ", " & Price & ", " & Size & _
                                                                                    ");" & vbCrLf
    End Function

    Public Function Get_UpdateMapPos(ByVal x As Integer, ByVal y As Integer, ByVal subarea As Integer, ByVal mapid As Integer) As String
        Return "UPDATE `" & Options.MyOptions.MySQL_TableName_Maps & "` SET " & Options.MyOptions.MySQL_Maps_Pos & "='" & x & "," & y & "," & subarea & "' WHERE " & Options.MyOptions.MySQL_Maps_ID & "=" & mapid & ";" & vbCrLf
    End Function
#End Region

End Module