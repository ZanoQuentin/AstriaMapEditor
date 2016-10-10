<Serializable()>
Public Class Monster

    Public ID As Integer
    Public Name As String
    Public GfxID As Integer
    Public Grades As String
    Public Pdvs As String
    Public Points As String
    Public Initiative As String
    Public MinKamas As Integer
    Public MaxKamas As Integer
    Public Experience As String

    Public Structure Caracteristiques
        Dim Level As Integer
        Dim Pdvs As Integer
        Dim PA As Integer
        Dim PM As Integer
        Dim Initiative As Integer
        Dim Experience As Integer
    End Structure

    <NonSerialized()> Public Shared Monsters(50000) As Monster

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public Shared Function GetByID(ByVal id As Integer) As Monster
        For Each anObject As Monster In Monsters
            If Not IsNothing(anObject) Then
                If anObject.ID = id Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Function Explode() As Caracteristiques()
        Dim MyCaracts(100) As Caracteristiques
        Dim nb As Integer = 0
        ' Level
        For Each stat As String In Grades.Split("|")
            MyCaracts(nb).Level = stat.Split("@")(0)
            nb += 1
        Next
        nb = 0
        ' Pdvs
        For Each pdv As String In Pdvs.Split("|")
            If pdv = "" Then pdv = 0
            MyCaracts(nb).Pdvs = pdv
            nb += 1
        Next
        nb = 0
        ' PA/PM
        For Each papm As String In Points.Split("|")
            MyCaracts(nb).PA = papm.Split(";")(0)
            MyCaracts(nb).PM = papm.Split(";")(1)
            nb += 1
        Next
        nb = 0
        ' Initiative
        For Each init As String In Initiative.Split("|")
            If init = "" Then init = 0
            MyCaracts(nb).Initiative = init
            nb += 1
        Next
        nb = 0
        ' Experience
        For Each xp As String In Experience.Split("|")
            If xp = "" Then xp = 0
            MyCaracts(nb).Experience = xp
            nb += 1
        Next
        nb = 0

        Return MyCaracts
    End Function

End Class
