<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Geoposition
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Geoposition))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoPlacementTriggersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoPlacementDesTriggersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripComboBox3 = New System.Windows.Forms.ToolStripComboBox()
        Me.ExécuterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OuvrirToutesLesMapsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.PréparationPatchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GrilleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneDroiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneGaucheToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LigneHautToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LigneBasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneGaucheToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneDroiteToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ColonneGaucheToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LigneHautToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LigneBasToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.NouvelleMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OuvrirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsérerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SupprimerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip = New System.Windows.Forms.ToolStrip()
        Me.BT_Save = New System.Windows.Forms.ToolStripButton()
        Me.BT_Load = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.TXT_Name = New System.Windows.Forms.ToolStripTextBox()
        Me.TXT_SA = New System.Windows.Forms.ToolStripTextBox()
        Me.TXT_A = New System.Windows.Forms.ToolStripComboBox()
        Me.TXT_SubA = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.TXT_X = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.TXT_Y = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.BT_Color = New System.Windows.Forms.ToolStripButton()
        Me.BT_OpenMap = New System.Windows.Forms.ToolStripButton()
        Me.L_CellInfo = New System.Windows.Forms.ToolStripLabel()
        Me.BT_OpenInWindows = New System.Windows.Forms.ToolStripButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GénérerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GéopositionsDesMapsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurationDesMaisonsEnclosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.ToolStrip.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoPlacementTriggersToolStripMenuItem, Me.GrilleToolStripMenuItem, Me.ToolStripSeparator1, Me.NouvelleMapToolStripMenuItem, Me.OuvrirToolStripMenuItem, Me.InsérerToolStripMenuItem, Me.SupprimerToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(149, 142)
        '
        'AutoPlacementTriggersToolStripMenuItem
        '
        Me.AutoPlacementTriggersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoPlacementDesTriggersToolStripMenuItem, Me.OuvrirToutesLesMapsToolStripMenuItem, Me.GénérerToolStripMenuItem, Me.DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem, Me.ToolStripSeparator6, Me.PréparationPatchToolStripMenuItem})
        Me.AutoPlacementTriggersToolStripMenuItem.Image = CType(resources.GetObject("AutoPlacementTriggersToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AutoPlacementTriggersToolStripMenuItem.Name = "AutoPlacementTriggersToolStripMenuItem"
        Me.AutoPlacementTriggersToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.AutoPlacementTriggersToolStripMenuItem.Text = "Fonctions"
        '
        'AutoPlacementDesTriggersToolStripMenuItem
        '
        Me.AutoPlacementDesTriggersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox3, Me.ExécuterToolStripMenuItem})
        Me.AutoPlacementDesTriggersToolStripMenuItem.Image = CType(resources.GetObject("AutoPlacementDesTriggersToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AutoPlacementDesTriggersToolStripMenuItem.Name = "AutoPlacementDesTriggersToolStripMenuItem"
        Me.AutoPlacementDesTriggersToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.AutoPlacementDesTriggersToolStripMenuItem.Text = "Auto placement des triggers"
        '
        'ToolStripComboBox3
        '
        Me.ToolStripComboBox3.Items.AddRange(New Object() {"1029", "1030", "4088", "4829"})
        Me.ToolStripComboBox3.Name = "ToolStripComboBox3"
        Me.ToolStripComboBox3.Size = New System.Drawing.Size(121, 23)
        Me.ToolStripComboBox3.Text = "1030"
        '
        'ExécuterToolStripMenuItem
        '
        Me.ExécuterToolStripMenuItem.Name = "ExécuterToolStripMenuItem"
        Me.ExécuterToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ExécuterToolStripMenuItem.Text = "Exécuter"
        '
        'OuvrirToutesLesMapsToolStripMenuItem
        '
        Me.OuvrirToutesLesMapsToolStripMenuItem.Image = CType(resources.GetObject("OuvrirToutesLesMapsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OuvrirToutesLesMapsToolStripMenuItem.Name = "OuvrirToutesLesMapsToolStripMenuItem"
        Me.OuvrirToutesLesMapsToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.OuvrirToutesLesMapsToolStripMenuItem.Text = "Ouvrir toutes les maps"
        '
        'DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem
        '
        Me.DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem.Name = "DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem"
        Me.DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem.Text = "Définir les monstres pour toutes les maps"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(288, 6)
        '
        'PréparationPatchToolStripMenuItem
        '
        Me.PréparationPatchToolStripMenuItem.Name = "PréparationPatchToolStripMenuItem"
        Me.PréparationPatchToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.PréparationPatchToolStripMenuItem.Text = "Préparation patch"
        '
        'GrilleToolStripMenuItem
        '
        Me.GrilleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColonneDroiteToolStripMenuItem, Me.ColonneGaucheToolStripMenuItem})
        Me.GrilleToolStripMenuItem.Name = "GrilleToolStripMenuItem"
        Me.GrilleToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.GrilleToolStripMenuItem.Text = "Grille"
        '
        'ColonneDroiteToolStripMenuItem
        '
        Me.ColonneDroiteToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColonneToolStripMenuItem, Me.ColonneGaucheToolStripMenuItem1, Me.LigneHautToolStripMenuItem, Me.LigneBasToolStripMenuItem})
        Me.ColonneDroiteToolStripMenuItem.Name = "ColonneDroiteToolStripMenuItem"
        Me.ColonneDroiteToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.ColonneDroiteToolStripMenuItem.Text = "Ajout"
        '
        'ColonneToolStripMenuItem
        '
        Me.ColonneToolStripMenuItem.Name = "ColonneToolStripMenuItem"
        Me.ColonneToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ColonneToolStripMenuItem.Text = "Colonne droite"
        '
        'ColonneGaucheToolStripMenuItem1
        '
        Me.ColonneGaucheToolStripMenuItem1.Name = "ColonneGaucheToolStripMenuItem1"
        Me.ColonneGaucheToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
        Me.ColonneGaucheToolStripMenuItem1.Text = "Colonne gauche"
        '
        'LigneHautToolStripMenuItem
        '
        Me.LigneHautToolStripMenuItem.Name = "LigneHautToolStripMenuItem"
        Me.LigneHautToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.LigneHautToolStripMenuItem.Text = "Ligne haut"
        '
        'LigneBasToolStripMenuItem
        '
        Me.LigneBasToolStripMenuItem.Name = "LigneBasToolStripMenuItem"
        Me.LigneBasToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.LigneBasToolStripMenuItem.Text = "Ligne bas"
        '
        'ColonneGaucheToolStripMenuItem
        '
        Me.ColonneGaucheToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColonneDroiteToolStripMenuItem1, Me.ColonneGaucheToolStripMenuItem2, Me.LigneHautToolStripMenuItem1, Me.LigneBasToolStripMenuItem1})
        Me.ColonneGaucheToolStripMenuItem.Name = "ColonneGaucheToolStripMenuItem"
        Me.ColonneGaucheToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.ColonneGaucheToolStripMenuItem.Text = "Suppression"
        '
        'ColonneDroiteToolStripMenuItem1
        '
        Me.ColonneDroiteToolStripMenuItem1.Name = "ColonneDroiteToolStripMenuItem1"
        Me.ColonneDroiteToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
        Me.ColonneDroiteToolStripMenuItem1.Text = "Colonne droite"
        '
        'ColonneGaucheToolStripMenuItem2
        '
        Me.ColonneGaucheToolStripMenuItem2.Name = "ColonneGaucheToolStripMenuItem2"
        Me.ColonneGaucheToolStripMenuItem2.Size = New System.Drawing.Size(161, 22)
        Me.ColonneGaucheToolStripMenuItem2.Text = "Colonne gauche"
        '
        'LigneHautToolStripMenuItem1
        '
        Me.LigneHautToolStripMenuItem1.Name = "LigneHautToolStripMenuItem1"
        Me.LigneHautToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
        Me.LigneHautToolStripMenuItem1.Text = "Ligne haut"
        '
        'LigneBasToolStripMenuItem1
        '
        Me.LigneBasToolStripMenuItem1.Name = "LigneBasToolStripMenuItem1"
        Me.LigneBasToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
        Me.LigneBasToolStripMenuItem1.Text = "Ligne bas"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(145, 6)
        '
        'NouvelleMapToolStripMenuItem
        '
        Me.NouvelleMapToolStripMenuItem.Image = CType(resources.GetObject("NouvelleMapToolStripMenuItem.Image"), System.Drawing.Image)
        Me.NouvelleMapToolStripMenuItem.Name = "NouvelleMapToolStripMenuItem"
        Me.NouvelleMapToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.NouvelleMapToolStripMenuItem.Text = "Nouvelle map"
        '
        'OuvrirToolStripMenuItem
        '
        Me.OuvrirToolStripMenuItem.Image = CType(resources.GetObject("OuvrirToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OuvrirToolStripMenuItem.Name = "OuvrirToolStripMenuItem"
        Me.OuvrirToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.OuvrirToolStripMenuItem.Text = "Ouvrir"
        '
        'InsérerToolStripMenuItem
        '
        Me.InsérerToolStripMenuItem.Image = CType(resources.GetObject("InsérerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.InsérerToolStripMenuItem.Name = "InsérerToolStripMenuItem"
        Me.InsérerToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.InsérerToolStripMenuItem.Text = "Insérer"
        '
        'SupprimerToolStripMenuItem
        '
        Me.SupprimerToolStripMenuItem.Image = CType(resources.GetObject("SupprimerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SupprimerToolStripMenuItem.Name = "SupprimerToolStripMenuItem"
        Me.SupprimerToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.SupprimerToolStripMenuItem.Text = "Supprimer"
        '
        'ToolStrip
        '
        Me.ToolStrip.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BT_Save, Me.BT_Load, Me.ToolStripSeparator8, Me.TXT_Name, Me.TXT_SA, Me.TXT_A, Me.TXT_SubA, Me.ToolStripSeparator7, Me.TXT_X, Me.ToolStripLabel2, Me.TXT_Y, Me.ToolStripSeparator9, Me.BT_Color, Me.BT_OpenMap, Me.L_CellInfo, Me.BT_OpenInWindows})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.Size = New System.Drawing.Size(902, 25)
        Me.ToolStrip.TabIndex = 2
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'BT_Save
        '
        Me.BT_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BT_Save.Image = CType(resources.GetObject("BT_Save.Image"), System.Drawing.Image)
        Me.BT_Save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BT_Save.Name = "BT_Save"
        Me.BT_Save.Size = New System.Drawing.Size(23, 22)
        Me.BT_Save.Text = "Sauvegarder"
        '
        'BT_Load
        '
        Me.BT_Load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BT_Load.Image = CType(resources.GetObject("BT_Load.Image"), System.Drawing.Image)
        Me.BT_Load.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BT_Load.Name = "BT_Load"
        Me.BT_Load.Size = New System.Drawing.Size(23, 22)
        Me.BT_Load.Text = "Charger"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 25)
        '
        'TXT_Name
        '
        Me.TXT_Name.Name = "TXT_Name"
        Me.TXT_Name.Size = New System.Drawing.Size(100, 25)
        Me.TXT_Name.Text = "None"
        '
        'TXT_SA
        '
        Me.TXT_SA.Name = "TXT_SA"
        Me.TXT_SA.Size = New System.Drawing.Size(25, 25)
        Me.TXT_SA.Text = "0"
        Me.TXT_SA.ToolTipText = "SuperArea"
        '
        'TXT_A
        '
        Me.TXT_A.Name = "TXT_A"
        Me.TXT_A.Size = New System.Drawing.Size(121, 25)
        Me.TXT_A.ToolTipText = "Area"
        '
        'TXT_SubA
        '
        Me.TXT_SubA.Name = "TXT_SubA"
        Me.TXT_SubA.Size = New System.Drawing.Size(121, 25)
        Me.TXT_SubA.ToolTipText = "SubArea"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'TXT_X
        '
        Me.TXT_X.Name = "TXT_X"
        Me.TXT_X.Size = New System.Drawing.Size(25, 25)
        Me.TXT_X.Text = "0"
        Me.TXT_X.ToolTipText = "X"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(10, 22)
        Me.ToolStripLabel2.Text = ","
        '
        'TXT_Y
        '
        Me.TXT_Y.Name = "TXT_Y"
        Me.TXT_Y.Size = New System.Drawing.Size(25, 25)
        Me.TXT_Y.Text = "0"
        Me.TXT_Y.ToolTipText = "Y"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 25)
        '
        'BT_Color
        '
        Me.BT_Color.BackColor = System.Drawing.Color.Transparent
        Me.BT_Color.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BT_Color.ForeColor = System.Drawing.Color.AliceBlue
        Me.BT_Color.Image = CType(resources.GetObject("BT_Color.Image"), System.Drawing.Image)
        Me.BT_Color.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BT_Color.Name = "BT_Color"
        Me.BT_Color.Size = New System.Drawing.Size(23, 22)
        Me.BT_Color.Text = "Couleur d'arrière plan"
        '
        'BT_OpenMap
        '
        Me.BT_OpenMap.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.BT_OpenMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BT_OpenMap.Image = CType(resources.GetObject("BT_OpenMap.Image"), System.Drawing.Image)
        Me.BT_OpenMap.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BT_OpenMap.Name = "BT_OpenMap"
        Me.BT_OpenMap.Size = New System.Drawing.Size(23, 22)
        Me.BT_OpenMap.Text = "Ouvrir"
        Me.BT_OpenMap.Visible = False
        '
        'L_CellInfo
        '
        Me.L_CellInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.L_CellInfo.Name = "L_CellInfo"
        Me.L_CellInfo.Size = New System.Drawing.Size(48, 22)
        Me.L_CellInfo.Text = "CellInfo"
        Me.L_CellInfo.Visible = False
        '
        'BT_OpenInWindows
        '
        Me.BT_OpenInWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BT_OpenInWindows.Image = CType(resources.GetObject("BT_OpenInWindows.Image"), System.Drawing.Image)
        Me.BT_OpenInWindows.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BT_OpenInWindows.Name = "BT_OpenInWindows"
        Me.BT_OpenInWindows.Size = New System.Drawing.Size(23, 22)
        Me.BT_OpenInWindows.Text = "Ouvrir dans Windows"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Black
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(0, 25)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(902, 509)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'GénérerToolStripMenuItem
        '
        Me.GénérerToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GéopositionsDesMapsToolStripMenuItem, Me.ConfigurationDesMaisonsEnclosToolStripMenuItem})
        Me.GénérerToolStripMenuItem.Name = "GénérerToolStripMenuItem"
        Me.GénérerToolStripMenuItem.Size = New System.Drawing.Size(291, 22)
        Me.GénérerToolStripMenuItem.Text = "Générer"
        '
        'GéopositionsDesMapsToolStripMenuItem
        '
        Me.GéopositionsDesMapsToolStripMenuItem.Name = "GéopositionsDesMapsToolStripMenuItem"
        Me.GéopositionsDesMapsToolStripMenuItem.Size = New System.Drawing.Size(266, 22)
        Me.GéopositionsDesMapsToolStripMenuItem.Text = "Géopositions des maps"
        '
        'ConfigurationDesMaisonsEnclosToolStripMenuItem
        '
        Me.ConfigurationDesMaisonsEnclosToolStripMenuItem.Name = "ConfigurationDesMaisonsEnclosToolStripMenuItem"
        Me.ConfigurationDesMaisonsEnclosToolStripMenuItem.Size = New System.Drawing.Size(266, 22)
        Me.ConfigurationDesMaisonsEnclosToolStripMenuItem.Text = "Configuration des maisons et enclos"
        '
        'Geoposition
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(902, 534)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ToolStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Geoposition"
        Me.Text = "Gestionnaire d'île"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NouvelleMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OuvrirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InsérerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SupprimerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoPlacementTriggersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoPlacementDesTriggersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExécuterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OuvrirToutesLesMapsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBox3 As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents DéfinirLesMonstresPourToutesLesMapsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PréparationPatchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents BT_Save As System.Windows.Forms.ToolStripButton
    Friend WithEvents BT_Load As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TXT_Name As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TXT_SA As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TXT_A As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents TXT_SubA As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TXT_X As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents TXT_Y As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BT_Color As System.Windows.Forms.ToolStripButton
    Friend WithEvents L_CellInfo As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BT_OpenMap As System.Windows.Forms.ToolStripButton
    Friend WithEvents BT_OpenInWindows As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents GrilleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneDroiteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneGaucheToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigneHautToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigneBasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneGaucheToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneDroiteToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColonneGaucheToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigneHautToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LigneBasToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents GénérerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GéopositionsDesMapsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigurationDesMaisonsEnclosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
