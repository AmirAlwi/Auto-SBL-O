Module FunMod

    Public Out_SummaryPath As String = ""
    Public searchname As String = ""
    Public locindex As Integer = 0
    Public LotTotal As Integer = 0
    Public QtyTested As Integer = 0
    Public HrdwBin(5) As ClassData
    Public SftwBin(300) As ClassData
    Public SummaryContent() As String
    Public Maininfo(10, 30) As String
    Public LotInfoLoc(8) As Integer
    Public CodeLoc(3) As Integer
    Public SortLoc(300) As Integer

    Public Function WriteReadtxt(sel As Integer,
                                 path As String,
                                 Optional input As String = "") As String

        Dim result As String = ""

        Select Case sel
            Case 1
                My.Computer.FileSystem.WriteAllText(path, input, False)
            Case 2
                result = My.Computer.FileSystem.ReadAllText(path)
        End Select

        Return result

    End Function

    'Public Function SelecOutFolder() As String

    '    Dim location As String = ""
    '    Try
    '        If (MainWindow.FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then

    '            location = MainWindow.FolderBrowserDialog1.SelectedPath & "\Final Summary"

    '            CheckCreateFolder(location)
    '            MsgBox("Output Summary location created")

    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex)
    '    End Try

    '    Return location
    'End Function

    Public Sub CheckCreateFolder(location As String)

        If (Not System.IO.Directory.Exists(location)) Then

            System.IO.Directory.CreateDirectory(location)

        End If
    End Sub

    Public Sub wrtlog(textinput As String, command As Integer) 'CVT logfile

        Dim Symbol As String = ""
        If command = 1 Then
            Symbol = ","
        ElseIf command = 2 Then
            Symbol = ";"
        End If
        My.Computer.FileSystem.WriteAllText(AppDomain.CurrentDomain.BaseDirectory & "LogfileCSV.txt", textinput & Symbol, True)

    End Sub

    Public Sub wrtlogcomma()

        My.Computer.FileSystem.WriteAllText(AppDomain.CurrentDomain.BaseDirectory & "LogfileCSV.txt", ",", True)

    End Sub

    Public Sub Cls()
        Search_Result.Invoke(Sub() Search_Result.ListBox.Items.Clear())
    End Sub

    Public Sub Prompt(text As String)
        Search_Result.Invoke(Sub() Search_Result.ListBox.Items.Add(text))
    End Sub

    Public Sub AutoSelect(name As String, storeloca As String(), lengthofloc As Integer, y As Integer)

        Dim z As Integer = 1
        Dim Gate As Boolean = True
        Dim CGate As Boolean
        Dim CLatest As Boolean = False

        If y = 0 Then
            Search_Result.ListBox.SetSelected(y, True)
        Else

            For n As Integer = 0 To y - 1

                For m As Integer = z To y

                    If storeloca(n).Remove(lengthofloc) = storeloca(m).Remove(lengthofloc) Then
                        CLatest = True 'check new file

                        If storeloca(n).Remove(0, lengthofloc) > storeloca(m).Remove(0, lengthofloc) Then

                            CGate = True
                        Else

                            CGate = False
                        End If

                        Gate *= CGate

                        If Gate = True And m = y Then

                            Search_Result.ListBox.SetSelected(n, True)
                            Exit For
                            Exit For
                        ElseIf Gate = False And n = y - 1 Then

                            Search_Result.ListBox.SetSelected(m, True)
                        End If
                    Else

                        If CLatest = False And y = m Then
                            Search_Result.ListBox.SetSelected(n, True)
                            Search_Result.ListBox.SetSelected(m, True)
                        ElseIf CLatest = False Then
                            Search_Result.ListBox.SetSelected(n, True)
                        End If
                    End If

                Next
                CLatest = False
                z += 1
            Next
        End If



    End Sub

    Public Function ArrangeFile(Locdata As String()) As String()
        Dim LocTimeOrder(10) As String
        Array.Resize(LocTimeOrder, locindex)

        For i As Integer = 0 To locindex - 1
            LocTimeOrder(i) = Locdata(i).Remove(0, Locdata(i).Length - 20)
        Next

        Dim ArrangeLoc() As String = CType(LocTimeOrder.Clone, String())

        Array.Sort(ArrangeLoc)

        For i As Integer = 0 To locindex - 1
            For n As Integer = 0 To locindex - 1
                If ArrangeLoc(i) = LocTimeOrder(n) Then
                    ArrangeLoc(i) = Locdata(n)
                    wrtlog(ArrangeLoc(i), 2)
                End If
            Next
        Next

        Return ArrangeLoc

    End Function

    Private Function SearchStartEndLine(Start As Integer, LineLength As Integer, Lines() As String, Optional condition As String = "") As Integer

        For i As Integer = Start To LineLength - 1 'check line number for bin data
            If Lines(i) = condition Then
                Return i
                Exit For
            End If
        Next

    End Function

    Public Function Readfile(filelocation As String, index As Integer) As Integer() ' read lot sum info        

        Dim BinLine As Integer = 0
        Dim infoline As Integer = 0
        Dim Siteline As Integer = 0
        Dim startL As Integer = 0
        Dim InfoName As String
        Dim CodeInfo() As String = {"Test Code:", "Summary Code:", "Lot QTY:"}
        Dim LotInfo() As String = {"Lot:", "Part Type:", "Node Name:", "Handler ID:", "Loadboard ID:",
                                    "Temperature:", "Operator:"}

        'log 4 read file
        '''''''''''''log 4 read info file '''''''''''")

        'read all sum info

        Dim lines As String() = IO.File.ReadAllLines(filelocation)
        SummaryContent = CType(lines.Clone, String())

        If lines.Length > 25 Then 'check valid files

            For n As Integer = 0 To lines.Length 'check line number for bin data

                If lines(n) = vbNullString Then

                    infoline = n

                    Exit For

                End If

            Next

            For i As Integer = 2 To infoline - 1

                Maininfo(index + 1, i - 2) = lines(i).Remove(0, 20) ' maininfo(0) is for name
                InfoName = lines(i).Remove(16).ToString.TrimStart

                If index = 0 Then

                    Maininfo(0, i - 2) = InfoName

                    For n As Integer = 0 To 6

                        If InfoName = LotInfo(n) Then
                            LotInfoLoc(n) = i - 2
                        End If

                    Next

                    For m As Integer = 0 To 2

                        If InfoName = CodeInfo(m) Then
                            CodeLoc(m) = i - 2

                        End If

                    Next

                End If

            Next
            If index = 0 Then
                For i As Integer = 1 To 5
                    wrtlog(Maininfo(1, LotInfoLoc(i)), 2)
                Next
                wrtlogcomma()
            End If


            '''''''''''''''updates on test code eliminate requirement
            If Maininfo(index + 1, CodeLoc(1)).ToString = "1A" Then 'Lot A total
                LotTotal = Convert.ToInt32(Maininfo(index + 1, CodeLoc(2)))
            End If 'end of read sum info
            ''''''''''''''''''

            If index <> 0 Then
                My.Computer.FileSystem.WriteAllText(AppDomain.CurrentDomain.BaseDirectory & "LogfileCSV.txt", vbNewLine, True)
                wrtlog(",,,,,,,", 1)
            End If

            wrtlog(index.ToString, 1)
            wrtlog(Maininfo(index + 1, CodeLoc(1)), 1)
            wrtlog(Maininfo(index + 1, CodeLoc(0)), 1)

            If index > 0 Then
                For i As Integer = 0 To 2
                    Maininfo(1, CodeLoc(2) - i) = Maininfo(1, CodeLoc(2) - i) & "," & Maininfo(index + 1, CodeLoc(2) - i)
                Next
            End If 'add sum info

            BinLine = SearchStartEndLine(CodeLoc(2) + 9, lines.Length, lines)

            wrtlog(BinLine.ToString, 1)

            startL = SearchStartEndLine(BinLine, lines.Length, lines, "Sort Total") + 3

            wrtlog(startL.ToString, 1)

            Siteline = SearchStartEndLine(startL, lines.Length, lines)

            wrtlog(Siteline.ToString, 1)

        End If
        Dim linemarks() As Integer = {BinLine, startL, Siteline}
        Array.Resize(linemarks, 3)

        Return linemarks

    End Function

    Public Function StoreFileLoc() As String()

        ''''''''''''' log 3 select file ''''''''''''''
        ResetDataCFT1()
        Dim div As String = MainWindow.ComboBox1.Text
        Dim Loc As String = "C:\J750\Test Summary\" & div & "\Final_Test"

        ''check folder existance
        If (Not System.IO.Directory.Exists(Out_SummaryPath)) Then
            System.IO.Directory.CreateDirectory(Out_SummaryPath)
        End If

        'select file
        If Search_Result.ListBox.Items.Count > 0 Then

            If Search_Result.ListBox.SelectedItems.Count < 7 Then
                For i As Integer = 0 To Search_Result.ListBox.SelectedItems.Count - 1
                    Dim SelItem As String = Search_Result.ListBox.SelectedItems(i).ToString
                    Search_Result.Locdata(i) = (Loc & SelItem)
                    wrtlog(SelItem, 2)
                Next
                locindex = Search_Result.ListBox.SelectedItems.Count
            End If

        End If

        wrtlogcomma()

        'rearrange file
        Dim LocSortByTime() As String = ArrangeFile(Search_Result.Locdata)

        wrtlogcomma()

        Return LocSortByTime

    End Function

    Public Sub ResetDataCFT1() 'for bindata storage

        For n As Integer = 1 To 4

            HrdwBin(n - 1) = New ClassData(n, String.Empty, 0, 0)  'start site 0 bin1 until site 3 bin 4

        Next

        HrdwBin(4) = New ClassData(5, String.Empty, 0, 0)

        For i As Integer = 0 To 299

            SftwBin(i) = New ClassData(i + 1, String.Empty, 0, 0)

        Next

    End Sub

    Public Sub ResetData()

        locindex = 0
        LotTotal = 0
        QtyTested = 0

        Array.Resize(SortLoc, 300)
        For i As Integer = 0 To 299
            SortLoc(i) = 0
        Next

    End Sub

End Module
