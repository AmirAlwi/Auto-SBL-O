<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SBLBrowse
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
        Me.WBR = New System.Windows.Forms.WebBrowser()
        Me.Print = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'WBR
        '
        Me.WBR.Location = New System.Drawing.Point(12, 12)
        Me.WBR.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WBR.Name = "WBR"
        Me.WBR.Size = New System.Drawing.Size(586, 645)
        Me.WBR.TabIndex = 0
        '
        'Print
        '
        Me.Print.Location = New System.Drawing.Point(512, 666)
        Me.Print.Name = "Print"
        Me.Print.Size = New System.Drawing.Size(85, 32)
        Me.Print.TabIndex = 1
        Me.Print.Text = "Print"
        Me.Print.UseVisualStyleBackColor = True
        '
        'SBLBrowse
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(609, 707)
        Me.Controls.Add(Me.Print)
        Me.Controls.Add(Me.WBR)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "SBLBrowse"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SBL Result"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents WBR As WebBrowser
    Friend WithEvents Print As Button
End Class
