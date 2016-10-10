<Serializable()>
Public Class Area

    Public ID As Integer
    Public Name As String
    Public SuperArea As String

    <NonSerialized()> Public Shared Areas(50000) As Area

    Public Shared Function GetByID(ByVal id As Integer) As Area
        For Each anObject As Area In Areas
            If Not IsNothing(anObject) Then
                If anObject.ID = id Then Return anObject
            End If
        Next
        Return Nothing
    End Function

    Public Shared Function GetByName(ByVal name As String) As Area
        For Each anObject As Area In Areas
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
