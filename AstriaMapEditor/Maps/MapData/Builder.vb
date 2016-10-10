Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.CompilerServices
Imports System

Public Module Builder

    Public ZKARRAY As String() = New String() {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "-", "_"}

    Public Function GetMapData(ByRef aMap As Map) As String
        Try
            Dim mapdata As String = Nothing
            Dim cell As Cell
            For Each cell In aMap.Cells
                mapdata &= GetCellData(cell)
            Next
            Return mapdata
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return ""
    End Function

    Public Function GetCellData(ByVal Cell As Cell) As String
        If IsNothing(Cell) Then Return ""

        ' Initialisation valeurs
        Dim Gfx1 As Integer = 0
        If Not IsNothing(Cell.Gfx1) Then Gfx1 = Cell.Gfx1.ID
        Dim Gfx2 As Integer = 0
        If Not IsNothing(Cell.Gfx2) Then Gfx2 = Cell.Gfx2.ID
        Dim Gfx3 As Integer = 0
        If Not IsNothing(Cell.Gfx3) Then Gfx3 = Cell.Gfx3.ID

        Dim str As String = Nothing
        Dim numArray As Integer() = New Integer() {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        ' Conversion
        If 1 Then
            numArray(0) = &H20
        Else
            numArray(0) = 0
        End If

        If Cell.LoS Then
            numArray(0) = (numArray(0) Or 1)
        Else
            numArray(0) = (numArray(0) Or 0)
        End If

        numArray(0) = (numArray(0) Or ((Gfx1 And &H600) >> 6))
        numArray(0) = (numArray(0) Or ((Gfx2 And &H2000) >> 11))
        numArray(0) = (numArray(0) Or ((Gfx3 And &H2000) >> 12))
        numArray(1) = ((Cell.RotaGfx1 And 3) << 4)
        numArray(1) = (numArray(1) Or (Cell.NivSol And 15))
        numArray(2) = ((Cell.Type() And 7) << 3)
        numArray(2) = (numArray(2) Or ((Gfx1 >> 6) And 7))
        numArray(3) = (Gfx1 And &H3F)
        numArray(4) = ((Cell.InclineSol And 15) << 2)

        If Cell.FlipGfx1 Then
            numArray(4) = (numArray(4) Or 2)
        Else
            numArray(4) = (numArray(4) Or 0)
        End If

        numArray(4) = (numArray(4) Or ((Gfx2 >> 12) And 1))
        numArray(5) = ((Gfx2 >> 6) And &H3F)
        numArray(6) = (Gfx2 And &H3F)
        numArray(7) = ((Cell.RotaGfx2 And 3) << 4)

        If Cell.FlipGfx2 Then
            numArray(7) = (numArray(7) Or 8)
        Else
            numArray(7) = (numArray(7) Or 0)
        End If

        If Cell.FlipGfx3 Then
            numArray(7) = (numArray(7) Or 4)
        Else
            numArray(7) = (numArray(7) Or 0)
        End If

        If Cell.IO Then
            numArray(7) = (numArray(7) Or 2)
        Else
            numArray(7) = (numArray(7) Or 0)
        End If

        numArray(7) = (numArray(7) Or ((Gfx3 >> 12) And 1))
        numArray(8) = ((Gfx3 >> 6) And &H3F)
        numArray(9) = (Gfx3 And &H3F)

        Dim num As Integer
        For Each num In numArray
            str &= ZKARRAY(num)
        Next
        Return str
    End Function

End Module