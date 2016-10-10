Imports System.Net
Imports System.Security.Cryptography
Imports System.Text

Public Class Connexion

    Friend connected As Boolean = False

    Private Sub KryptonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KryptonButton1.Click
        If KryptonTextBox2.Text = "" Or KryptonTextBox1.Text = "" Then
            MsgBox("Connexion refusée.")
        End If
        Try
            Dim client As New WebClient
            If client.DownloadString(Main.Link_PHP & "?pseudo=" & KryptonTextBox2.Text & "&pass=" & MD5StringHash(KryptonTextBox1.Text) & "&hwid=" & Main.Get_HWID()) <> "0" Then
                connected = True
                Me.Close()
            Else
                MsgBox("Connexion refusée." & MD5StringHash(KryptonTextBox1.Text))
            End If
        Catch
            MsgBox("Serveur en maintenance.")
            End
        End Try
    End Sub

    Private Function MD5StringHash(ByVal strString As String) As String
        Dim MD5 As New MD5CryptoServiceProvider
        Dim Data As Byte()
        Dim Result As Byte()
        Dim R As String = ""
        Dim Temp As String = ""

        Data = Encoding.ASCII.GetBytes(strString)
        Result = MD5.ComputeHash(Data)
        For i As Integer = 0 To Result.Length - 1
            Temp = Hex(3.668 * Result(i) + 58.398)
            If Len(Temp) = 1 Then Temp = "0" & Temp
            R += Temp
        Next
        Return R
    End Function

End Class
