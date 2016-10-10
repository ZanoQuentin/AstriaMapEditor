Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System


Friend Module FightCellManager

    Private hash As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_"

    Public Function GetHashCode(ByRef aMap As Map) As Object
        Dim hashcode As String = ""
        Dim cell As Cell
        For Each cell In aMap.Cells
            If IsNothing(cell) Then Continue For
            If cell.FightCell = 1 Then
                hashcode &= HashCodeCell(cell.ID)
            End If
        Next
        hashcode = (hashcode & "|")
        Dim cell2 As Cell
        For Each cell2 In aMap.Cells
            If IsNothing(cell2) Then Continue For
            If (cell2.FightCell = 2) Then
                hashcode &= HashCodeCell(cell2.ID)
            End If
        Next
        Return hashcode
    End Function

    Private Function HashCodeCell(ByVal cellnum As Integer) As Object
        Dim num2 As Integer = (cellnum Mod hash.Length)
        Dim num As Integer = CInt(Math.Round(CDbl((CDbl((cellnum - num2)) / CDbl(hash.Length)))))
        Return (Conversions.ToString(hash.Chars(num)) & Conversions.ToString(hash.Chars(num2)))
    End Function

    Public Function ParseFightCell(ByVal data As String, ByVal mycells() As Cell)
        Dim str As String = data.Split(New Char() {"|"c})(0)
        Dim str2 As String = data.Split(New Char() {"|"c})(1)
        Dim objArray As Object() = New Object((CInt(Math.Round(CDbl((CDbl(str.Length) / 2)))) + 1) - 1) {}
        Dim objArray2 As Object() = New Object((CInt(Math.Round(CDbl((CDbl(str2.Length) / 2)))) + 1) - 1) {}
        Dim num7 As Double = ((CDbl(str.Length) / 2) - 1)
        Dim i As Double = 0
        Do While (i <= num7)
            Dim str3 As String = str.Substring(CInt(Math.Round(CDbl((0 + (2 * i))))), 1)
            Dim str4 As String = str.Substring(CInt(Math.Round(CDbl((1 + (2 * i))))), 1)
            Dim num As Integer = ((Strings.InStr(hash, str3, CompareMethod.Binary) - 1) * hash.Length)
            Dim num2 As Integer = (Strings.InStr(hash, str4, CompareMethod.Binary) - 1)
            mycells((num + num2)).FightCell = 1
            i += 1
        Loop
        Dim num8 As Double = ((CDbl(str2.Length) / 2) - 1)
        Dim j As Double = 0
        Do While (j <= num8)
            Dim str5 As String = str2.Substring(CInt(Math.Round(CDbl((0 + (2 * j))))), 1)
            Dim str6 As String = str2.Substring(CInt(Math.Round(CDbl((1 + (2 * j))))), 1)
            Dim num4 As Integer = ((Strings.InStr(hash, str5, CompareMethod.Binary) - 1) * hash.Length)
            Dim num5 As Integer = (Strings.InStr(hash, str6, CompareMethod.Binary) - 1)
            mycells((num4 + num5)).FightCell = 2
            j += 1
        Loop
        Return mycells
    End Function

End Module