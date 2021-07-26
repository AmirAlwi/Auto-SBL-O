Module ProgTesting
    Dim TestLot As String()
    Public Sub LotInput(LotIndex As Integer)

        MainWindow.LotNo.Text = TestLot(LotIndex)
        MainWindow.ProgName.Text = "!1" '"5P49V5901B802_FT.TXT"

        MainWindow.Button1_Click(MainWindow, EventArgs.Empty)

        'If Form2.WebBrowser1.Url = New Uri("http://penengweb/test/mpr/Engineering/SBL/SBL2.asp") Then

        'End If

    End Sub

    Public Sub ReadTestLot()
        TestLot = IO.File.ReadAllLines("D:\WFH\REV\TestLot.txt")

        For i As Integer = 0 To TestLot.Length - 1
            LotInput(i)
        Next

    End Sub
End Module
