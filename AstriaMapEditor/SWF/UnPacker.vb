Imports SwfDotNet.IO
Imports SwfDotNet.IO.ByteCode
Imports SwfDotNet.IO.ByteCode.Actions
Imports SwfDotNet.IO.Tags

Module Unpacker

    Public Function UnPackerSwf(ByVal Path As String) As Map
        Dim list As New List(Of String)
        Dim swf As Swf = New SwfReader(Path).ReadSwf
        Dim enumerator As IEnumerator = swf.Tags.GetEnumerator
        Do While enumerator.MoveNext
            Dim current As BaseTag = DirectCast(enumerator.Current, BaseTag)
            If (current.ActionRecCount <> 0) Then
                Dim str As String = Nothing
                Dim enumerator2 As IEnumerator = current.GetEnumerator
                Do While enumerator2.MoveNext
                    Dim list2 As ArrayList = New Decompiler(swf.Version).Decompile(DirectCast(enumerator2.Current, Byte()))
                    Dim enumerator3 As IEnumerator = list2.GetEnumerator
                    Try
                        Do While enumerator3.MoveNext
                            str = (str & DirectCast(enumerator3.Current, BaseAction).ToString & ChrW(13) & ChrW(10))
                        Loop
                    Finally
                        If TypeOf enumerator3 Is IDisposable Then
                            TryCast(enumerator3, IDisposable).Dispose()
                        End If
                    End Try
                Loop

                ' Ajout des données
                Dim aMap As New Map
                aMap.MapData = Split(str.ToString, "'")(&H1D)
                aMap.ID = CInt(Split(Split(str.ToString, "push")(8), " ")(1))
                aMap.Background = Tile.Get_Background(CInt(Split(Split(str.ToString, "push")(14), " ")(1)))
                aMap.Height = CInt(Split(Split(str.ToString, "push")(12), " ")(1))
                aMap.Width = CInt(Split(Split(str.ToString, "push")(10), " ")(1))
                aMap.Ambiance = CInt(Split(Split(str.ToString, "push")(&H10), " ")(1))
                aMap.Musique = CInt(Split(Split(str.ToString, "push")(&H12), " ")(1))
                aMap.Capabilities = CInt(Split(Split(str.ToString, "push")(&H16), " ")(1))
                aMap.BOutdoor = CBool(Split(Split(str.ToString, "push")(20), " ")(1))
                Dim fileinfo As New IO.FileInfo(Path)
                If fileinfo.Name.Contains("_") Then aMap.DateMap = fileinfo.Name.Split("_")(1).Split(".swf")(0)

                Return aMap
            End If
        Loop
        Return Nothing
    End Function

End Module