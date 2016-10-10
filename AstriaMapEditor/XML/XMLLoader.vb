Imports System.IO
Imports System.Xml.Serialization

Module XMLLoader

    Friend Sub LoadAllXML()
        ' Chargement des XML de position
        '       Grounds
        Dim DeserialiserOfTiles As New XmlSerializer(GetType(Tile.Pos()))
        Tile.Pos_Grounds = CType(DeserialiserOfTiles.Deserialize(XMLLoader.LoadXML("XML/grounds.xml")), Tile.Pos())
        Array.Resize(Tile.Pos_Grounds, 50000)
        '       Objects
        Tile.Pos_Objects = CType(DeserialiserOfTiles.Deserialize(XMLLoader.LoadXML("XML/objects.xml")), Tile.Pos())
        Array.Resize(Tile.Pos_Objects, 50000)
        '       Monsters
        Dim DeserialiserOfMonsters As New XmlSerializer(GetType(Monster()))
        Monster.Monsters = CType(DeserialiserOfMonsters.Deserialize(XMLLoader.LoadXML("XML/monsters.xml")), Monster())
        '       Areas
        Dim DeserialiserOfAreas As New XmlSerializer(GetType(Area()))
        Area.Areas = CType(DeserialiserOfAreas.Deserialize(XMLLoader.LoadXML("XML/areas.xml")), Area())
        '       SubAreas
        Dim DeserialiserOfSubAreas As New XmlSerializer(GetType(SubArea()))
        SubArea.SubAreas = CType(DeserialiserOfSubAreas.Deserialize(XMLLoader.LoadXML("XML/subareas.xml")), SubArea())
    End Sub


    Private Function LoadXML(ByVal path As String) As MemoryStream

        Dim document As New System.Xml.XmlDocument()
        document.LoadXml(IO.File.ReadAllText(path))

        Dim xmlStream As New MemoryStream()
        document.Save(xmlStream)
        xmlStream.Flush()
        xmlStream.Position = 0

        Return xmlStream

    End Function

End Module
