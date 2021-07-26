Module Summary

    Public Sub GenSummary(ByRef infos As String(,))
        ''''''''' Log 7 generate summary ''''''''''''
        Dim BinName As String() = {"Pass", "DC", "Func", "Cont", " "}
        Dim check As Boolean = False

        'check different lot? and summary code
        If locindex > 1 Then

            For i As Integer = locindex To 2 Step -1

                If Maininfo(i, 2).ToString <> Maininfo(i - 1, 2).ToString Then

                    wrtlog("mixxed Lot , exit flow " & Maininfo(2, 2).ToString & "  " & Maininfo(1, 2).ToString, 1)
                    ResetData()
                    Exit Sub
                End If
            Next
        End If

        Dim temp As String() = Maininfo(1, CodeLoc(1)).ToString.Split(CType(",", Char()), StringSplitOptions.RemoveEmptyEntries)

        For Each item As String In temp

            If item = "1A" Or item = "2A" Then
                check = True
            End If
        Next

        If check = False Then
            wrtlog("does not contain any 1A file summary", 1)
            Exit Sub
        End If

        Dim FinalSumFileName As String = $" {Out_SummaryPath}\Custom_{Maininfo(1, LotInfoLoc(2))}_{Maininfo(1, 0).Remove(Maininfo(1, 0).Length - 4)}_{Maininfo(1, 2)}_Final.txt"

        Dim wrt As IO.StreamWriter
        wrt = New IO.StreamWriter(FinalSumFileName)

        wrt.WriteLine("Summary Report")

        For i As Integer = 0 To LastInfoLine - 3
            wrt.WriteLine(infos(0, i).PadLeft(16) & "    " & Maininfo(1, i))
        Next

        calcqty()

        wrt.WriteLine("Lot Total:".PadLeft(16) & "    " & LotTotal)
        wrt.WriteLine(vbCrLf & "Bin Total" & vbCrLf &
                      "       Site      Bin      BinName      Parts   %/Total" & vbCrLf &
                      "------------------------------------------------------------------------")

        For i As Integer = 3 To 0 Step -1
            'write bin 1 to 4
            If HrdwBin(i).total <> 0 Then
                wrt.WriteLine((i + 1).ToString.PadLeft(20) & BinName(i).PadLeft(14) &
                          HrdwBin(i).Parts.ToString.PadLeft(10) & Math.Round(HrdwBin(i).total, 2).ToString.PadLeft(10))

                wrtlog("bin " & i + 1 & "-" & HrdwBin(i).Parts, 2)
            End If
        Next

        If HrdwBin(4).total <> 0 Then
            ' Bin N/A
            wrt.WriteLine(("N/A").ToString.PadLeft(20) & BinName(4).PadLeft(14) &
                          HrdwBin(4).Parts.ToString.PadLeft(10) & Math.Round(HrdwBin(4).total, 2).ToString.PadLeft(10))

            wrtlog("bin N/A" & "-" & HrdwBin(4).Parts, 3)
        End If

        wrtlogcomma()
        wrt.WriteLine()

        '''''sort data
        wrt.WriteLine(vbCrLf & "Sort Total" & vbCrLf &
                      "      Site      Sort     SortName     Parts    %/Total" & vbCrLf &
                      "------------------------------------------------------------------------")

        For Each Location As Integer In SortLoc 'for each registered bin location

            wrtlog("sort " & Location + 1 & " - " & SftwBin(Location).Parts, 2)
            wrt.WriteLine((Location + 1).ToString.PadLeft(20) & SftwBin(Location).BinName.PadLeft(14) & SftwBin(Location).Parts.ToString.PadLeft(10) &
                              Math.Round(SftwBin(Location).total, 2).ToString.PadLeft(10))

        Next
        wrtlogcomma()

        wrt.WriteLine()
        wrt.WriteLine("Total Device : {0} Total Device Pass : {1}", QtyTested, SftwBin(0).Parts) 'based on registered tested unit
        wrtlog(LotTotal.ToString, 1)
        wrtlog(QtyTested.ToString, 1)
        wrt.Close()

        wrtlog("Summary Complete", 1)

    End Sub ' end of generate summary

    Private Sub calcqty()

        For i As Integer = 0 To 3
            QtyTested += HrdwBin(i).Parts
        Next

        Try

            For i As Integer = 0 To 4
                HrdwBin(i).total = HrdwBin(i).Parts / QtyTested * 100
            Next

        Catch ex As Exception
            wrtlog(ex.ToString, 1)
            ResetData()
            Exit Sub
        End Try

        Dim count As Integer = 0
        For i As Integer = 0 To 299

            If SftwBin(i).Parts > 0 Then
                SftwBin(i).total = SftwBin(i).Parts / QtyTested * 100
                SortLoc(count) = i
                count += 1
            End If

        Next
        Array.Resize(SortLoc, count)
    End Sub

End Module
