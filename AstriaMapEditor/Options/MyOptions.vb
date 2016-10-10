<Serializable()>
Public Class MyOptions

    Public Profil As String = "User"

    ' Général
    Public SizePanel As Integer = 130
    Public SaveSQL As Boolean = False
    Public ResizePanel As Boolean = False
    Public NewSizePanel As Integer = 260

    ' MySQL
    '       Général
    Public MySQL_Host As String = "localhost"
    Public MySQL_User As String = "root"
    Public MySQL_Password As String = ""
    Public MySQL_Database As String = "ancestra_static"
    '       Maps
    Public MySQL_TableName_Maps As String = "maps"
    Public MySQL_Maps_ID As String = "id"
    Public MySQL_Maps_Date As String = "date"
    Public MySQL_Maps_Width As String = "width"
    Public MySQL_Maps_Height As String = "heigth"
    Public MySQL_Maps_MapData As String = "mapData"
    Public MySQL_Maps_Key As String = "key"
    Public MySQL_Maps_FightPlaces As String = "places"
    Public MySQL_Maps_Monsters As String = "monsters"
    Public MySQL_Maps_Capabilities As String = "capabilities"
    Public MySQL_Maps_Pos As String = "mappos"
    Public MySQL_Maps_NbGroups As String = "numgroup"
    Public MySQL_Maps_SizeMaxGroup As String = "groupmaxsize"
    '       Triggers
    Public MySQL_TableName_Triggers As String = "scripted_cells"
    Public MySQL_Triggers_MapID As String = "MapID"
    Public MySQL_Triggers_CellID As String = "CellID"
    Public MySQL_Triggers_Action As String = "ActionID"
    Public MySQL_Triggers_Args As String = "ActionsArgs"
    Public MySQL_Triggers_IdTP As Integer = 4
    '       End Fight
    Public MySQL_TableName_EndFight As String = "endfight_action"
    Public MySQL_EndFight_MapID As String = "map"
    Public MySQL_EndFight_Action As String = "action"
    Public MySQL_EndFight_Args As String = "args"
    Public MySQL_EndFight_IdTP As Integer = 4
    '       MobGroup
    Public MySQL_TableName_Mobs As String = "mobgroups_fix"
    Public MySQL_Mobs_MapID As String = "mapid"
    Public MySQL_Mobs_CellID As String = "cellid"
    Public MySQL_Mobs_GroupData As String = "groupData"
    '       Pnjs
    Public MySQL_TableName_Pnjs As String = "npcs"
    Public MySQL_Pnj_MapID As String = "mapid"
    Public MySQL_Pnj_CellID As String = "cellid"
    Public MySQL_Pnj_NpcID As String = "npcid"
    Public MySQL_Pnj_Orientation As String = "orientation"
    '       Houses
    Public MySQL_TableName_Houses As String = "houses"
    Public MySQL_Houses_ID As String = "id"
    Public MySQL_Houses_MapID As String = "map_id"
    Public MySQL_Houses_CellID As String = "cell_id"
    Public MySQL_Houses_Price As String = "sale"
    Public MySQL_Houses_ToMapID As String = "mapid"
    Public MySQL_Houses_ToCellID As String = "caseid"
    '       MountPark
    Public MySQL_TableName_MountParks As String = "mountpark_data"
    Public MySQL_MountParks_MapID As String = "mapid"
    Public MySQL_MountParks_CellID As String = "cellid"
    Public MySQL_MountParks_Size As String = "size"
    Public MySQL_MountParks_Price As String = "price"
    '       Zaaps
    Public MySQL_TableName_Zaaps As String = "zaaps"
    Public MySQL_Zaaps_MapID As String = "mapID"
    Public MySQL_Zaaps_CellID As String = "cellID"
    '       Zaapis
    Public MySQL_TableName_Zaapis As String = "zaapi"
    Public MySQL_Zaapis_MapID As String = "mapid"
    Public MySQL_Zaapis_Align As String = "align"

    ' SWF
    Public SWF_PathCopy As String = ""

    ' PHP
    Public LinkSWF As String = "http://astria-serv.com/SWF/maps/"

    ' Design


    Public Function MoveFile() As Boolean
        If SWF_PathCopy = "" Then Return False
        Return True
    End Function

End Class