<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InputSizeMap
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InputSizeMap))
        Me.KryptonButton1 = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonNumericUpDown1 = New ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown()
        Me.KryptonNumericUpDown2 = New ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown()
        Me.KryptonLabel1 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonLabel2 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonLabel3 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonLabel4 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonNumericUpDown3 = New ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown()
        Me.KryptonNumericUpDown4 = New ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown()
        Me.SuspendLayout()
        '
        'KryptonButton1
        '
        Me.KryptonButton1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.KryptonButton1.Location = New System.Drawing.Point(0, 81)
        Me.KryptonButton1.Name = "KryptonButton1"
        Me.KryptonButton1.Size = New System.Drawing.Size(253, 33)
        Me.KryptonButton1.TabIndex = 64
        Me.KryptonButton1.Values.Text = "Valider"
        '
        'KryptonNumericUpDown1
        '
        Me.KryptonNumericUpDown1.Location = New System.Drawing.Point(139, 10)
        Me.KryptonNumericUpDown1.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.KryptonNumericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.KryptonNumericUpDown1.Name = "KryptonNumericUpDown1"
        Me.KryptonNumericUpDown1.Size = New System.Drawing.Size(38, 22)
        Me.KryptonNumericUpDown1.TabIndex = 65
        Me.KryptonNumericUpDown1.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'KryptonNumericUpDown2
        '
        Me.KryptonNumericUpDown2.Location = New System.Drawing.Point(205, 10)
        Me.KryptonNumericUpDown2.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.KryptonNumericUpDown2.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.KryptonNumericUpDown2.Name = "KryptonNumericUpDown2"
        Me.KryptonNumericUpDown2.Size = New System.Drawing.Size(38, 22)
        Me.KryptonNumericUpDown2.TabIndex = 66
        Me.KryptonNumericUpDown2.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'KryptonLabel1
        '
        Me.KryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl
        Me.KryptonLabel1.Location = New System.Drawing.Point(178, 4)
        Me.KryptonLabel1.Name = "KryptonLabel1"
        Me.KryptonLabel1.Size = New System.Drawing.Size(26, 29)
        Me.KryptonLabel1.TabIndex = 67
        Me.KryptonLabel1.Values.Text = "×"
        '
        'KryptonLabel2
        '
        Me.KryptonLabel2.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldPanel
        Me.KryptonLabel2.Location = New System.Drawing.Point(8, 12)
        Me.KryptonLabel2.Name = "KryptonLabel2"
        Me.KryptonLabel2.Size = New System.Drawing.Size(112, 20)
        Me.KryptonLabel2.TabIndex = 68
        Me.KryptonLabel2.Values.Text = "Taille de la carte :"
        '
        'KryptonLabel3
        '
        Me.KryptonLabel3.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl
        Me.KryptonLabel3.Location = New System.Drawing.Point(8, 48)
        Me.KryptonLabel3.Name = "KryptonLabel3"
        Me.KryptonLabel3.Size = New System.Drawing.Size(124, 20)
        Me.KryptonLabel3.TabIndex = 72
        Me.KryptonLabel3.Values.Text = "Position de départ : "
        '
        'KryptonLabel4
        '
        Me.KryptonLabel4.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl
        Me.KryptonLabel4.Location = New System.Drawing.Point(178, 42)
        Me.KryptonLabel4.Name = "KryptonLabel4"
        Me.KryptonLabel4.Size = New System.Drawing.Size(18, 29)
        Me.KryptonLabel4.TabIndex = 71
        Me.KryptonLabel4.Values.Text = ","
        '
        'KryptonNumericUpDown3
        '
        Me.KryptonNumericUpDown3.Location = New System.Drawing.Point(205, 48)
        Me.KryptonNumericUpDown3.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.KryptonNumericUpDown3.Minimum = New Decimal(New Integer() {999999, 0, 0, -2147483648})
        Me.KryptonNumericUpDown3.Name = "KryptonNumericUpDown3"
        Me.KryptonNumericUpDown3.Size = New System.Drawing.Size(38, 22)
        Me.KryptonNumericUpDown3.TabIndex = 70
        '
        'KryptonNumericUpDown4
        '
        Me.KryptonNumericUpDown4.Location = New System.Drawing.Point(139, 48)
        Me.KryptonNumericUpDown4.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
        Me.KryptonNumericUpDown4.Minimum = New Decimal(New Integer() {999999, 0, 0, -2147483648})
        Me.KryptonNumericUpDown4.Name = "KryptonNumericUpDown4"
        Me.KryptonNumericUpDown4.Size = New System.Drawing.Size(38, 22)
        Me.KryptonNumericUpDown4.TabIndex = 69
        '
        'InputSizeMap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(253, 114)
        Me.Controls.Add(Me.KryptonLabel3)
        Me.Controls.Add(Me.KryptonLabel4)
        Me.Controls.Add(Me.KryptonNumericUpDown3)
        Me.Controls.Add(Me.KryptonNumericUpDown4)
        Me.Controls.Add(Me.KryptonLabel2)
        Me.Controls.Add(Me.KryptonLabel1)
        Me.Controls.Add(Me.KryptonNumericUpDown2)
        Me.Controls.Add(Me.KryptonNumericUpDown1)
        Me.Controls.Add(Me.KryptonButton1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InputSizeMap"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Taille de la carte"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KryptonButton1 As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonNumericUpDown1 As ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown
    Friend WithEvents KryptonNumericUpDown2 As ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown
    Friend WithEvents KryptonLabel1 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents KryptonLabel2 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents KryptonLabel3 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents KryptonLabel4 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents KryptonNumericUpDown3 As ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown
    Friend WithEvents KryptonNumericUpDown4 As ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown

End Class
