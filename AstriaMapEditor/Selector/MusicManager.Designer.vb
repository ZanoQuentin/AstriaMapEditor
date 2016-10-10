<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MusicManager
    Inherits ComponentFactory.Krypton.Toolkit.KryptonForm

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MusicManager))
        Me.BT_Valide = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonListBox1 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.SuspendLayout()
        '
        'BT_Valide
        '
        Me.BT_Valide.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BT_Valide.Location = New System.Drawing.Point(0, 328)
        Me.BT_Valide.Name = "BT_Valide"
        Me.BT_Valide.Size = New System.Drawing.Size(165, 41)
        Me.BT_Valide.TabIndex = 3
        Me.BT_Valide.Values.Text = "Valider"
        '
        'KryptonListBox1
        '
        Me.KryptonListBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.KryptonListBox1.Location = New System.Drawing.Point(0, 0)
        Me.KryptonListBox1.Name = "KryptonListBox1"
        Me.KryptonListBox1.Size = New System.Drawing.Size(165, 328)
        Me.KryptonListBox1.TabIndex = 4
        '
        'MusicManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(165, 369)
        Me.Controls.Add(Me.KryptonListBox1)
        Me.Controls.Add(Me.BT_Valide)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MusicManager"
        Me.Text = "Musique Manager"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BT_Valide As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonListBox1 As ComponentFactory.Krypton.Toolkit.KryptonListBox
End Class
