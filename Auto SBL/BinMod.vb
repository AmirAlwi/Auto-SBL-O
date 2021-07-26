Module BinMod

    Dim retestqty As Integer = 0
    Dim RawHrdwBin(10, 20) As ClassData
    Dim RawSftwBin(10, 300) As ClassData

    Public Sub ProcessBinD(BinLine As Integer, lines() As String, Index As Integer)
        ''''''''' log 5 bin data process ''''''''''''''")
        retestqty = 0
        'Splitting Text to var

        For i As Integer = CodeLoc(2) + 11 To BinLine - 1

            Dim temp As String() = lines(i).Split(CType(" ", Char()), StringSplitOptions.RemoveEmptyEntries)
            Try
                If temp(1) = "N/A" Then

                    RawHrdwBin(Index, i - (CodeLoc(2) + 11)) = New ClassData(5, "N/A - error",
                                                      Convert.ToInt32(temp(2)), Convert.ToDecimal(temp(3)))
                ElseIf temp.Length = 5 Then

                    RawHrdwBin(Index, i - (CodeLoc(2) + 11)) = New ClassData(Convert.ToInt32(temp(1)), temp(2).ToString,
                                                       Convert.ToInt32(temp(3)), Convert.ToDecimal(temp(4)))
                ElseIf temp.Length = 4 Then


                    RawHrdwBin(Index, i - (CodeLoc(2) + 11)) = New ClassData(Convert.ToInt32(temp(1)), "N/A",
                                                       Convert.ToInt32(temp(2)), Convert.ToDecimal(temp(3)))
                End If

            Catch ex As Exception
                wrtlog("seperation fail, : " & lines(i), 1)
            End Try
        Next

        Select Case Maininfo(Index + 1, CodeLoc(1)).ToString

            Case "1A", "2A"
                For n As Integer = CodeLoc(2) + 11 To BinLine - 1
                    HrdwBin(RawHrdwBin(Index, n - (CodeLoc(2) + 11)).Bin - 1).Parts += RawHrdwBin(Index, n - (CodeLoc(2) + 11)).Parts
                Next
                wrtlog(",", 1)
                For i As Integer = 0 To 4
                    wrtlog(HrdwBin(i).Parts & ",", 1)
                Next

            Case "1B", "2B", "1E", "2E"
                For i As Integer = CodeLoc(2) + 11 To BinLine - 1
                    retestqty += RawHrdwBin(Index, i - (CodeLoc(2) + 11)).Parts
                Next

                Calcretest(Index)

                CompNextSplitB(Maininfo(Index + 1, CodeLoc(1)).ToString, BinLine, Index)

                For i As Integer = 0 To 4
                    wrtlog(HrdwBin(i).Parts & ",", 1)
                Next

        End Select
    End Sub

    Public Sub ProcessSortD(startline As Integer, Sortline As Integer, lines As String(), index As Integer)

        'processing sort data
        Dim num As Integer ' use for parsing

        '''''' log 6 sort data process '''''''''")
        For i As Integer = startline To Sortline - 1
            Dim temp As String() = lines(i).Split(CType(" ", Char()), StringSplitOptions.RemoveEmptyEntries)

            If temp(1) = "N/A" Then

                RawSftwBin(index, i - startline) = New ClassData(299, "N/A - error",
                                          Convert.ToInt32(temp(2)), Convert.ToDecimal(temp(3)))
            ElseIf temp.Length = 5 Then

                RawSftwBin(index, i - startline) = New ClassData(Convert.ToInt32(temp(1)), temp(2).ToString,
                                                   Convert.ToInt32(temp(3)), Convert.ToDecimal(temp(4)))

            ElseIf Integer.TryParse(temp(2), num) And Integer.TryParse(temp(1), num) Then

                RawSftwBin(index, i - startline) = New ClassData(Convert.ToInt32(temp(1)), "N/A",
                                           Convert.ToInt32(temp(2)), Convert.ToDecimal(temp(3)))

            ElseIf Not Integer.TryParse(temp(1), num) And Integer.TryParse(temp(2), num) Then

                ExtractNum(index, i - startline, temp)

            End If

        Next

        Dim SelCase As String = Maininfo(index + 1, CodeLoc(1)).ToString

        Select Case SelCase
            Case "1A", "2A"
                For i As Integer = startline To Sortline - 1

                    For j As Integer = 0 To 299
                        ''continue here
                        Try
                            If RawSftwBin(index, i - startline).Bin = SftwBin(j).Bin Then
                                SftwBin(j).Parts += RawSftwBin(index, i - startline).Parts
                                SftwBin(j).BinName = RawSftwBin(index, i - startline).BinName
                            End If
                        Catch ex As Exception
                            wrtlog("Code A error " & "bin : " & SftwBin(j).Bin & "bin name : " & RawSftwBin(index, i - startline).BinName, 1)
                        End Try
                    Next

                Next
            Case "1B", "2B", "1E", "2E"
                CompNextSplitS(Maininfo(index + 1, CodeLoc(1)).ToString, startline, Sortline, index)

        End Select

    End Sub

    Private Sub CompNextSplitB(SumC As String, Binline As Integer, index As Integer)

        Dim LSplit As Integer = Convert.ToInt32(SumC.Remove(1, 1))

        If LSplit = 1 Then ' reset bin 2,3,4 counter
            For i As Integer = 1 To 3
                HrdwBin(i).Parts = 0
            Next
        End If

        For j As Integer = (CodeLoc(2) + 11) To Binline - 1

            If RawHrdwBin(index, j - (CodeLoc(2) + 11)).Bin = 1 Then 'continue bin 1 part count
                HrdwBin(0).Parts += RawHrdwBin(index, j - (CodeLoc(2) + 11)).Parts
            End If

            If RawHrdwBin(index, j - (CodeLoc(2) + 11)).Bin <> 1 And LSplit = 1 Then
                HrdwBin(RawHrdwBin(index, j - (CodeLoc(2) + 11)).Bin - 1).Parts += RawHrdwBin(index, j - (CodeLoc(2) + 11)).Parts ' bin 2,3,4
            End If

        Next
    End Sub

    Private Sub CompNextSplitS(SumC As String, startline As Integer, Sortline As Integer, index As Integer)

        Dim LSplit As Integer = Convert.ToInt32(SumC.Remove(1, 1)) 'give sequence of code

        If LSplit = 1 Then 'reset bin 2,3,4
            For i As Integer = 1 To 299 ' sortdatactf1(1) is for bin 1 
                SftwBin(i) = New ClassData(i + 1, String.Empty, 0, 0)
            Next
        End If

        For i As Integer = startline To Sortline - 1

            If RawSftwBin(index, i - startline).Bin = 1 Then 'continue add bin 1 parts
                SftwBin(0).Parts += RawSftwBin(index, i - startline).Parts
            End If

            If RawSftwBin(index, i - startline).Bin <> 1 Then '
                SftwBin(RawSftwBin(index, i - startline).Bin - 1).Parts += RawSftwBin(index, i - startline).Parts
                SftwBin(RawSftwBin(index, i - startline).Bin - 1).BinName = RawSftwBin(index, i - startline).BinName
            End If

        Next

    End Sub

    Private Sub ExtractNum(index As Integer, line As Integer, SplitD As String())
        Dim TempSplit As String

        For i As Integer = 3 To 1 Step -1

            TempSplit = SplitD(1).Remove(i)

            If Integer.TryParse(TempSplit, 0) Then

                RawSftwBin(index, line) = New ClassData(Convert.ToInt32(TempSplit), SplitD(1).Remove(0, i).ToString,
                                                       Convert.ToInt32(SplitD(2)), Convert.ToDecimal(SplitD(3)))
                Exit For
            End If
        Next
    End Sub

    Private Sub Calcretest(Index As Integer)
        Dim checkbalance As Integer
        Dim Sumbalance As Integer
        Dim CodeA_Qty As Integer = 0

        For i As Integer = 0 To 3
            CodeA_Qty += HrdwBin(i).Parts
        Next

        checkbalance = LotTotal - CodeA_Qty
        wrtlog(checkbalance.ToString, 1)

        If checkbalance < 0 Then
            HrdwBin(0).Parts = HrdwBin(0).Parts + checkbalance
            SftwBin(0).Parts = HrdwBin(0).Parts

        End If

        Sumbalance = LotTotal - (HrdwBin(0).Parts + retestqty)
        wrtlog(Sumbalance.ToString, 1)
        If Sumbalance < 0 Then
            HrdwBin(0).Parts = HrdwBin(0).Parts + Sumbalance
            SftwBin(0).Parts = HrdwBin(0).Parts
        End If

    End Sub

End Module
