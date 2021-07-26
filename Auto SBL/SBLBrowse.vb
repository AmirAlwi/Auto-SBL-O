Public Class SBLBrowse
    Private LoadB As Boolean = False
    Private Sub WBR_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WBR.DocumentCompleted

        ''''''''''' Log 8 SBL insert ''''''''''''''")
        If WBR.Url = New Uri("http://penengweb/test/mpr/Engineering/SBL/SBL.html") Then

            LoadB = True
            Sblinfo()
            LoadB = True 'place here allowed back

        ElseIf WBR.Url = New Uri("http://penengweb/test/mpr/Engineering/SBL/SBL1.asp") Then

            Dim reply As String = WBR.DocumentText.Remove(51)

            If reply = "There are no such PLOAD in the SBL Lookup Table !!!" Then
                MsgBox("Load files is not in the SBL", vbInformation)
                wrtlog("Load files is not in the SBL", 1)
            Else

                SbldataSB()
                SbldataHB()
            End If

        ElseIf WBR.Url = New Uri("http://penengweb/test/mpr/Engineering/SBL/SBL2.asp") Then

            Dim webreply As String = WBR.DocumentText
            Dim cutreply As String = webreply.Remove(31)
            Dim webfile As IO.StreamWriter

            If cutreply = "Sum of IB1 + IB2 + IB3 + IB4 ca" Then

                MsgBox("Ecxceed lot quantity", vbInformation)
                wrtlog("Exceed lot quantity", 1)

            Else

                wrtlog("SBL Completed", 3)

                webfile = New IO.StreamWriter(Out_SummaryPath & "\" & Maininfo(1, 2) & "_Final.html")
                webfile.Write(webreply)
                webfile.Close()

            End If

        End If

    End Sub

    Private Sub Sblinfo()
        Dim name As String() = {"lot_id", "device_id", "tester_id", "handler_id",
            "loadboard_id", "temp", "operator_id"}

        For i As Integer = 0 To name.Length - 1
            WBR.Document.GetElementById(name(i)).SetAttribute("value", Maininfo(1, LotInfoLoc(i)))
        Next

        WBR.Document.GetElementById("lot_qty").SetAttribute("value", LotTotal.ToString) 'lottotalQtyTested
        WBR.Document.GetElementById("test_program").SetAttribute("value", MainWindow.ProgName.Text)

        submit(LoadB)

        LoadB = False

    End Sub

    Private Sub SbldataHB()

        For i As Integer = 0 To 3

            WBR.Document.GetElementById("IB" & i + 1).SetAttribute("value", HrdwBin(i).Parts.ToString)

        Next

        submit(LoadB)
        LoadB = False

    End Sub

    Private Sub SbldataSB()

        Dim rread As String = WBR.DocumentText
        Dim find As Integer

        For i As Integer = 1 To 298
            find = rread.IndexOf(">S" & i + 1 & "<") 'sortdatactf1(0):= bin 1 ...
            If find <> -1 Then 'index return -1 - not found
                wrtlog(">S" & i + 1, 2)
                'sortdatactf(i) bin 1 to 298 (299 for NA bin)
                WBR.Document.GetElementById("S" & i + 1).SetAttribute("value", SftwBin(i).Parts.ToString)
            End If
        Next

        wrtlogcomma()
    End Sub

    Private Sub submit(cond As Boolean)

        If cond = True Then

            Dim l_forms = WBR.Document.GetElementsByTagName("form") 'login

            If l_forms.Count > 0 Then

                l_forms.Item(0).InvokeMember("submit")

            End If

        End If

    End Sub

    Private Sub Print_Click(sender As Object, e As EventArgs) Handles Print.Click
        WBR.ShowPrintDialog()
    End Sub

End Class