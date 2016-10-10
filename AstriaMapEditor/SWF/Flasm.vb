Imports System.IO
Public Module Flasm

    ''' <summary>
    ''' Compile un FLM
    ''' </summary>
    ''' <param name="FileName">Nom du fichier sans extension</param>
    ''' <remarks></remarks>
    Public Sub Compile(ByVal FileName As String)
        Try ' Compilation du fichier swf
            Dim MyProcess As New Process
            MyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            MyProcess.StartInfo.CreateNoWindow = True
            MyProcess.StartInfo.FileName = "Flasm\flasm.exe"
            MyProcess.StartInfo.Arguments = "-a """ & FileName & """"
            MyProcess.Start()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Public Function Get_FlasmCode(ByRef MyDatas As Map)
        Dim BackgroundID As Integer = 0
        If Not IsNothing(MyDatas.Background) Then BackgroundID = MyDatas.Background.ID

        Dim str As String = ""
        str = "movie 'Flasm\blank.swf' compressed" & ChrW(13) & ChrW(10)
        str = ((((((((((((((((((((((((((str & "  frame 0" & ChrW(13) & ChrW(10) & ChrW(13) & ChrW(10) & "    constants '_parent', '_url', 'System', 'security', 'allowDomain', 'id', 'width', 'height', 'backgroundNum', 'ambianceId', 'musicId', 'bOutdoor', 'capabilities', 'mapData', '") & _
                Builder.GetMapData(MyDatas) & "'" & ChrW(13) & ChrW(10)) & "    push c:0" & ChrW(13) & ChrW(10) & "    getVariable" & ChrW(13) & ChrW(10)) & "    push c:1" & ChrW(13) & ChrW(10) & "    getMember" & ChrW(13) & ChrW(10)) & "    push 1, c:2" & ChrW(13) & ChrW(10) & "    getVariable" & ChrW(13) & ChrW(10)) & "    push c:3" & ChrW(13) & ChrW(10) & "    getMember" & ChrW(13) & ChrW(10)) & "    push c:4" & ChrW(13) & ChrW(10) & "    callMethod" & ChrW(13) & ChrW(10)) & "    pop" & ChrW(13) & ChrW(10) & "    push c:5" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.ID & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:6" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.Width & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:7" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.Height & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:8" & ChrW(13) & ChrW(10)) & "    push " & _
                BackgroundID & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:9" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.Ambiance & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:10" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.Musique & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:11" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.BOutdoor & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:12" & ChrW(13) & ChrW(10)) & "    push " & _
                MyDatas.Capabilities & ChrW(13) & ChrW(10)) & "    setVariable" & ChrW(13) & ChrW(10) & "    push c:13" & ChrW(13) & ChrW(10)) & "    push c:14" & ChrW(13) & ChrW(10) & "    setVariable" & ChrW(13) & ChrW(10)) & "  end" & ChrW(13) & ChrW(10) & "end" & ChrW(13) & ChrW(10))
        Return str
    End Function

End Module