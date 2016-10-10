Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text

Public Class Decryptage

    Public Shared Function Checksum(ByVal Data As String) As String
        Dim num As Integer
        Dim num3 As Integer = (Data.Length - 1)
        Dim i As Integer = 0
        Do While (i <= num3)
            num = (num + (Strings.Asc(Data.Substring(i, 1)) Mod &H10))
            i += 1
        Loop
        Dim strArray As String() = New String() {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"}
        Return strArray((num Mod &H10))
    End Function

    Public Shared Function DecypherData(ByVal Data As String, ByVal Key As String, ByVal Checksum As Integer) As String
        Dim dataToDecrypt As String = Nothing
        Dim num4 As Integer = (Data.Length - 2)
        Dim i As Integer = 0
        Do While (i <= num4)
            Dim num As Integer = CInt(Convert.ToInt64(Data.Substring(i, 2), &H10))
            Dim num2 As Integer = Strings.Asc(Key.Substring(CInt(Math.Round(CDbl((((CDbl(i) / 2) + Checksum) Mod CDbl(Key.Length))))), 1))
            dataToDecrypt = (dataToDecrypt & Conversions.ToString(Strings.Chr((num Xor num2))))
            i = (i + 2)
        Loop
        Return Decryptage.Unescape(dataToDecrypt)
    End Function

    Public Shared Function HashCodes(ByVal a As String) As Object
        Dim str As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_"
        Return str.IndexOf(a)
    End Function

    Public Shared Function PrepareKey(ByVal Data As String) As String
        Dim dataToDecrypt As String = Nothing
        Dim num2 As Integer = (Data.Length - 2)
        Dim i As Integer = 0
        Do While (i <= num2)
            dataToDecrypt = (dataToDecrypt & Conversions.ToString(Strings.Chr(CInt(Convert.ToInt64(Data.Substring(i, 2), &H10)))))
            i = (i + 2)
        Loop
        Return Decryptage.Unescape(dataToDecrypt)
    End Function

    Private Shared Function Unescape(ByVal DataToDecrypt As String) As String
        Return Uri.UnescapeDataString(DataToDecrypt)
    End Function

End Class