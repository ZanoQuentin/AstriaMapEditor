Imports System.IO
Imports System.Xml.Serialization

Module XMLLoader

    Friend Sub LoadAllXML()
        ' Chargement des XML de position
        '       Grounds
        Dim DeserialiserOfTiles As New XmlSerializer(GetType(Tile.Pos()))
        Tile.Pos_Grounds = CType(DeserialiserOfTiles.Deserialize(XMLLoader.LoadXML_ByPHP(Main.Link_PHP & "?xml=grounds" & "&hwid=" & Main.Get_HWID())), Tile.Pos())
        Array.Resize(Tile.Pos_Grounds, 50000)
        '       Objects
        Tile.Pos_Objects = CType(DeserialiserOfTiles.Deserialize(XMLLoader.LoadXML_ByPHP(Main.Link_PHP & "?xml=objects" & "&hwid=" & Main.Get_HWID())), Tile.Pos())
        Array.Resize(Tile.Pos_Objects, 50000)
        '       Monsters
        Dim DeserialiserOfMonsters As New XmlSerializer(GetType(Monster()))
        Monster.Monsters = CType(DeserialiserOfMonsters.Deserialize(XMLLoader.LoadXML_ByPHP(Main.Link_PHP & "?xml=monsters" & "&hwid=" & Main.Get_HWID())), Monster())
        '       Areas
        Dim DeserialiserOfAreas As New XmlSerializer(GetType(Area()))
        Area.Areas = CType(DeserialiserOfAreas.Deserialize(XMLLoader.LoadXML_ByPHP(Main.Link_PHP & "?xml=areas" & "&hwid=" & Main.Get_HWID())), Area())
        '       SubAreas
        Dim DeserialiserOfSubAreas As New XmlSerializer(GetType(SubArea()))
        SubArea.SubAreas = CType(DeserialiserOfSubAreas.Deserialize(XMLLoader.LoadXML_ByPHP(Main.Link_PHP & "?xml=subareas" & "&hwid=" & Main.Get_HWID())), SubArea())
    End Sub


    Private Function LoadXML_ByPHP(ByVal link As String) As MemoryStream

        Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(link)
        Dim response As System.Net.HttpWebResponse = request.GetResponse()

        If response.StatusCode = System.Net.HttpStatusCode.OK Then
            Dim stream As System.IO.Stream = response.GetResponseStream()

            Dim reader As New System.IO.StreamReader(stream)
            Dim contents As String = reader.ReadToEnd()
            'Dim IV As Byte() = System.Convert.FromBase64String(contents.Split("|")(0))
            'Dim BytesContent As Byte() = System.Convert.FromBase64String(contents.Split("|")(1))

            'Dim datas As String = LoadXML_Decrypt(BytesContent, IV, "544DDD3F84E2D")
            'IO.File.AppendAllText("text.xml", datas)

            Dim document As New System.Xml.XmlDocument()
            Try
                document.LoadXml(System.Text.Encoding.ASCII.GetString(System.Convert.FromBase64String(contents)))
            Catch
                MsgBox("Erreur dans le contenu de la page : " & vbCrLf & vbCrLf & contents, MsgBoxStyle.Critical, "Erreur Base64")
                End
            End Try

            Dim xmlStream As New MemoryStream()
            document.Save(xmlStream)
            xmlStream.Flush()
            xmlStream.Position = 0

            Return xmlStream
        Else
            Throw New Exception("Could not retrieve document from the URL, response code: " & response.StatusCode)
        End If
    End Function

    'Private Function LoadXML_Decrypt(ByVal content As Byte(), ByVal IV As Byte(), ByVal key As String) As String
    '    Dim algorithm = DirectCast(CryptoConfig.CreateFromName("RIJNDAEL"), SymmetricAlgorithm)
    '    algorithm.Mode = CipherMode.CBC
    '    algorithm.Padding = PaddingMode.Zeros

    '    Dim decryptor As ICryptoTransform = algorithm.CreateDecryptor(System.Text.Encoding.ASCII.GetBytes(key), IV)

    '    Dim fromEncrypt() As Byte = New Byte(content.Length) {}
    '    Dim msDecrypt As New MemoryStream(content)
    '    Dim csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)
    '    csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length)

    '    Return System.Text.Encoding.ASCII.GetString(fromEncrypt)
    'End Function

#Region " [COMMENTAIRES] Ancien chargement des XML "
    'If IO.File.Exists(DirectoryApply & "/Images/grounds.xml") Then
    '    Try
    '        Dim FluxDeFichier As Stream = File.OpenRead(DirectoryApply & "/Images/grounds.xml")
    '        Dim Deserialiseur As New XmlSerializer(GetType(Tile.Pos()))
    '        Tile.Pos_Grounds = CType(Deserialiseur.Deserialize(FluxDeFichier), Tile.Pos())
    '        FluxDeFichier.Close()
    '    Catch ex As Exception
    '        MsgBox("Impossible de lire le fichier grounds.xml : " & ex.Message)
    '    End Try
    'End If
    'If IO.File.Exists(DirectoryApply & "/Images/objects.xml") Then
    '    Try
    '        Dim FluxDeFichier As Stream = File.OpenRead(DirectoryApply & "/Images/objects.xml")
    '        Dim Deserialiseur As New XmlSerializer(GetType(Tile.Pos()))
    '        Tile.Pos_Objects = CType(Deserialiseur.Deserialize(FluxDeFichier), Tile.Pos())
    '        FluxDeFichier.Close()
    '    Catch ex As Exception
    '        MsgBox("Impossible de lire le fichier objects.xml : " & ex.Message)
    '    End Try
    'End If
#End Region

End Module
