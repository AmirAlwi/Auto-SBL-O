
Public Class MainWindow

    Private Sub MainWindow_Shown(sender As Object, e As EventArgs) Handles Me.Shown


        Label4.Text = "V 4.0"

        Dim read As String = vbNullString

        'program location
        Dim exepath As String = AppDomain.CurrentDomain.BaseDirectory
        Dim locLine As String = vbNullString

        'read config file
        Try
            If System.IO.File.Exists(exepath & "exepath.txt") Then
                Dim config As String = My.Computer.FileSystem.ReadAllText(exepath & "exepath.txt")
                Dim sLines() As String = config.Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

                For i As Integer = 0 To sLines.Length - 1
                    sLines(i) = sLines(i).Split(CType(">", Char()), StringSplitOptions.RemoveEmptyEntries)(1) 'split and take index 1 value
                Next

                locLine = sLines(0)

                'populate div selection
                Dim Devision() As String = sLines(1).Split(CType(",", Char()), StringSplitOptions.RemoveEmptyEntries)
                For Each item In Devision
                    ComboBox1.Items.Add(item)
                Next

            End If

            If locLine = Nothing Or (System.IO.File.Exists(exepath & "exepath.txt") = False) Then '

                Out_SummaryPath = "C:\J750\Final Summary"

                WriteReadtxt(1, exepath & "exepath.txt",
                             "Path>" & Out_SummaryPath & vbNewLine &
                             "Div>" & "MMT,RFD")

            Else
                Out_SummaryPath = locLine
            End If

        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try

        CheckCreateFolder(Out_SummaryPath)

        'Auto select toggle file
        If System.IO.File.Exists(exepath & "AutoSelect_Setting") = True Then

            Dim Status As String = WriteReadtxt(2, exepath & "AutoSelect_Setting")
            Select Case Status
                Case "True"
                    CheckBox1.Checked = True
                Case "False"
                    CheckBox1.Checked = False
            End Select
        End If

        'set default sel
        ComboBox1.Text = "MMT"

    End Sub

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ResetDataCFT1()
        ResetData()

        If LotNo.Text <> Nothing And ProgName.Text <> Nothing Then

            Dim tempS As String = LotNo.Text

            If tempS = "%" Then 'search all for eng use
                tempS = ""
            End If

            searchname = "*" & tempS & "_*"

            'LOG 1
            wrtlog(vbNewLine &
                   Date.Today & "," &
                   Now.ToLongTimeString() & "," &
                   LotNo.Text & "," &
                   ProgName.Text, 1)

            If Search_Result.ShowDialog() = DialogResult.OK Then 'pop out search result
                Try
                    ProgFlow()
                Catch ex As Exception
                    wrtlog(ex.ToString, 1)
                End Try

            End If

        End If

    End Sub

    Private Sub ProgFlow()

        Dim LocSortByTime() As String = StoreFileLoc()
        Dim LineMark() As Integer

        For i As Integer = 0 To locindex - 1
            If LocSortByTime(i) IsNot vbNullString Then

                LineMark = Readfile(LocSortByTime(i), i) 'Binline, startline , siteline

                ProcessBinD(LineMark(0), SummaryContent, i)

                ProcessSortD(LineMark(1), LineMark(2), SummaryContent, i)

            End If
        Next

        GenSummary(Maininfo)

        If ProgName.Text <> "!1" Then 'start sbl insert

            SBLBrowse.Show()
            SBLBrowse.WBR.Navigate("http://penengweb/test/mpr/Engineering/SBL/SBL.html")

        End If

    End Sub

    Private Sub LotNo_KeyDown(sender As Object, e As KeyEventArgs) Handles LotNo.KeyDown
        If e.KeyCode = Keys.Enter Then

            Button1_Click(Me, EventArgs.Empty)

        End If

    End Sub

    Private Sub ProgName_KeyDown(sender As Object, e As KeyEventArgs) Handles ProgName.KeyDown
        If e.KeyCode = Keys.Enter Then

            Button1_Click(Me, EventArgs.Empty)

        ElseIf (e.KeyCode And Not Keys.Modifiers) = Keys.F1 AndAlso e.Modifiers = Keys.Control Then

            Process.Start("explorer.exe", Out_SummaryPath)

        ElseIf (e.KeyCode And Not Keys.Modifiers) = Keys.F2 AndAlso e.Modifiers = Keys.Control Then

            ReadTestLot()

        ElseIf (e.KeyCode And Not Keys.Modifiers) = Keys.F3 AndAlso e.Modifiers = Keys.Control Then

            CheckBox1.Visible = True

        End If
    End Sub

    Private Sub Reset_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.LotNo.Text = ""
        Me.ProgName.Text = ""

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim exepath As String = AppDomain.CurrentDomain.BaseDirectory

        If CheckBox1.Checked Then
            WriteReadtxt(1, exepath & "AutoSelect_Setting", "True")
        Else
            WriteReadtxt(1, exepath & "AutoSelect_Setting", "False")
        End If

    End Sub

    Private Sub Label2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Label2.MouseDoubleClick
        MsgBox("Penang 2021 by Mayden @ UTP" & vbNewLine & "mayden@gmail.com")
    End Sub
End Class

