<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Search_Result
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Sel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListBox
        '
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.HorizontalScrollbar = True
        Me.ListBox.Location = New System.Drawing.Point(13, 35)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBox.Size = New System.Drawing.Size(461, 134)
        Me.ListBox.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Lot No."
        '
        'Sel
        '
        Me.Sel.Location = New System.Drawing.Point(371, 175)
        Me.Sel.Name = "Sel"
        Me.Sel.Size = New System.Drawing.Size(103, 35)
        Me.Sel.TabIndex = 2
        Me.Sel.Text = "Select"
        Me.Sel.UseVisualStyleBackColor = True
        '
        'Search_Result
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(486, 217)
        Me.Controls.Add(Me.Sel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "Search_Result"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Search_Result"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListBox As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Sel As Button
End Class
