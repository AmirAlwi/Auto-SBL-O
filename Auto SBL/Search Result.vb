Public Class Search_Result

    Public Locdata(10) As String

    Private Sub Form3_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim Div As String = MainWindow.ComboBox1.Text
        ResetData()
        Dim Loc As String = "C:\J750\Test Summary\" & Div & "\Final_Test"
        Cls()
        getfile(Loc, searchname)
    End Sub

    Private Sub getfile(filelocation As String, name As String)             ' search lot sum file location

        Dim storeloca(10) As String
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim lengthofloc As Integer = 0

        '"'''''''''''log 2 search file''''''''''''
        Try
            For Each item As String In My.Computer.FileSystem.GetFiles(filelocation, FileIO.SearchOption.SearchTopLevelOnly, name)

                If Search("Custom", item) And Not Search("FT2_", item) And Not Search("F2_", item) And Not Search("SU_", item) And Not Search("CAL_", item) And Not Search("QA_", item) Then

                    Dim SearchLocation As String = item.Remove(0, filelocation.Length)
                    Prompt(SearchLocation)

                    If name <> "**" Then
                        storeloca(x) = SearchLocation
                        lengthofloc = item.Length - 20 - filelocation.Length
                        y = x
                        x += 1
                        wrtlog(item, 2)
                    End If

                End If
            Next
        Catch ex As Exception
            wrtlog(ex.ToString, 1)
        End Try

        wrtlogcomma()

        Array.Resize(Locdata, ListBox.Items.Count) ' set locdata size

        ' auto selection
        If name <> "**" And ListBox.Items.Count > 0 Then
            AutoSelect(name, storeloca, lengthofloc, y)

            If MainWindow.CheckBox1.Checked Then
                MsgBox(ListBox.SelectedItems.Count & " file selected")
                Me.Sel_Click(Me, EventArgs.Empty) 'call after auto selec function
            End If
        End If
    End Sub

    Private Sub Sel_Click(sender As Object, e As EventArgs) Handles Sel.Click

        Try
            DialogResult = DialogResult.OK
        Catch ex As Exception
            wrtlog(ex.ToString, 1)
        End Try

    End Sub

    Private Function Search(filter As String, item As String) As Boolean
        Return item.ToString.Contains(filter)
    End Function

End Class