Public Class ClassData 'sum class data

    Public Bin As Integer = 0
    Public BinName As String = String.Empty
    Public Parts As Integer = 0
    Public total As Double = 0.00
    Public Sub New(Tbin As Integer, TbinName As String,
                          Tparts As Integer, Ttotal As Decimal)

        Me.Bin = Tbin 'or sort
        Me.BinName = TbinName
        Me.Parts = Tparts
        Me.total = Ttotal

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
