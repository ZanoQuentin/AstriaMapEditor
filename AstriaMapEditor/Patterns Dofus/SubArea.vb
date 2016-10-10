<Serializable()>
Public Class SubArea

    Public ID As Integer
    Public Name As String
    Public Area As String

    <NonSerialized()> Public Shared SubAreas(50000) As SubArea

    Public Sub New()

    End Sub

    Public Shared Function GetByID(ByVal id As Integer) As SubArea
        For Each anObject As SubArea In SubAreas
            If Not IsNothing(anObject) Then
                If anObject.ID = id Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function GetByName(ByVal name As String) As SubArea
        For Each anObject As SubArea In SubAreas
            If Not IsNothing(anObject) Then
                If anObject.Name = name Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Overrides Function ToString() As String
        Return Name
    End Function

End Class
