<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrieurGfx
    Inherits ComponentFactory.Krypton.Toolkit.KryptonForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrieurGfx))
        Me.KryptonPanel = New ComponentFactory.Krypton.Toolkit.KryptonPanel()
        Me.KryptonRichTextBox2 = New ComponentFactory.Krypton.Toolkit.KryptonRichTextBox()
        Me.Conv = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonRichTextBox1 = New ComponentFactory.Krypton.Toolkit.KryptonRichTextBox()
        Me.KryptonManager = New ComponentFactory.Krypton.Toolkit.KryptonManager(Me.components)
        Me.KryptonLabel1 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        CType(Me.KryptonPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.KryptonPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'KryptonPanel
        '
        Me.KryptonPanel.Controls.Add(Me.KryptonLabel1)
        Me.KryptonPanel.Controls.Add(Me.KryptonRichTextBox2)
        Me.KryptonPanel.Controls.Add(Me.Conv)
        Me.KryptonPanel.Controls.Add(Me.KryptonRichTextBox1)
        Me.KryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KryptonPanel.Location = New System.Drawing.Point(0, 0)
        Me.KryptonPanel.Name = "KryptonPanel"
        Me.KryptonPanel.Size = New System.Drawing.Size(375, 420)
        Me.KryptonPanel.TabIndex = 0
        '
        'KryptonRichTextBox2
        '
        Me.KryptonRichTextBox2.Location = New System.Drawing.Point(231, 99)
        Me.KryptonRichTextBox2.Name = "KryptonRichTextBox2"
        Me.KryptonRichTextBox2.ReadOnly = True
        Me.KryptonRichTextBox2.Size = New System.Drawing.Size(130, 309)
        Me.KryptonRichTextBox2.TabIndex = 2
        Me.KryptonRichTextBox2.Text = ""
        '
        'Conv
        '
        Me.Conv.Location = New System.Drawing.Point(146, 99)
        Me.Conv.Name = "Conv"
        Me.Conv.Size = New System.Drawing.Size(79, 309)
        Me.Conv.TabIndex = 1
        Me.Conv.Values.Text = "Trier"
        '
        'KryptonRichTextBox1
        '
        Me.KryptonRichTextBox1.Location = New System.Drawing.Point(10, 99)
        Me.KryptonRichTextBox1.Name = "KryptonRichTextBox1"
        Me.KryptonRichTextBox1.Size = New System.Drawing.Size(130, 309)
        Me.KryptonRichTextBox1.TabIndex = 0
        Me.KryptonRichTextBox1.Text = ""
        '
        'KryptonManager
        '
        Me.KryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver
        '
        'KryptonLabel1
        '
        Me.KryptonLabel1.Location = New System.Drawing.Point(4, 9)
        Me.KryptonLabel1.Name = "KryptonLabel1"
        Me.KryptonLabel1.Size = New System.Drawing.Size(374, 84)
        Me.KryptonLabel1.TabIndex = 3
        Me.KryptonLabel1.Values.Text = resources.GetString("KryptonLabel1.Values.Text")
        '
        'TrieurGfx
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(375, 420)
        Me.Controls.Add(Me.KryptonPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TrieurGfx"
        Me.Text = "Trieur de GfxID"
        CType(Me.KryptonPanel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KryptonPanel.ResumeLayout(False)
        Me.KryptonPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KryptonPanel As ComponentFactory.Krypton.Toolkit.KryptonPanel
    Friend WithEvents KryptonManager As ComponentFactory.Krypton.Toolkit.KryptonManager

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Friend WithEvents KryptonRichTextBox2 As ComponentFactory.Krypton.Toolkit.KryptonRichTextBox
    Friend WithEvents Conv As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents KryptonRichTextBox1 As ComponentFactory.Krypton.Toolkit.KryptonRichTextBox
    Friend WithEvents KryptonLabel1 As ComponentFactory.Krypton.Toolkit.KryptonLabel
End Class
