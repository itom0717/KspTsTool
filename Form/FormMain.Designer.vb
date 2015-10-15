<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
  Inherits System.Windows.Forms.Form

  'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

  'Windows フォーム デザイナーで必要です。
  Private components As System.ComponentModel.IContainer

  'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
  'Windows フォーム デザイナーを使用して変更できます。  
  'コード エディターを使って変更しないでください。
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.CloseButton = New System.Windows.Forms.Button()
    Me.TranslationSettingButton = New System.Windows.Forms.Button()
    Me.SrcPathLabel = New System.Windows.Forms.Label()
    Me.SrcPathTextBox = New System.Windows.Forms.TextBox()
    Me.SrcPathButton = New System.Windows.Forms.Button()
    Me.DstPathLabel = New System.Windows.Forms.Label()
    Me.DstPathTextBox = New System.Windows.Forms.TextBox()
    Me.DstPathButton = New System.Windows.Forms.Button()
    Me.TranslationButton = New System.Windows.Forms.Button()
    Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
    Me.CancelTranslationButton = New System.Windows.Forms.Button()
    Me.ProgressBar = New System.Windows.Forms.ProgressBar()
    Me.ProgressLabel = New System.Windows.Forms.Label()
    Me.ImportTranslationCfgButton = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'CloseButton
    '
    Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CloseButton.Location = New System.Drawing.Point(355, 152)
    Me.CloseButton.Name = "CloseButton"
    Me.CloseButton.Size = New System.Drawing.Size(58, 23)
    Me.CloseButton.TabIndex = 0
    Me.CloseButton.Text = "閉じる"
    Me.CloseButton.UseVisualStyleBackColor = True
    '
    'TranslationSettingButton
    '
    Me.TranslationSettingButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.TranslationSettingButton.Location = New System.Drawing.Point(265, 152)
    Me.TranslationSettingButton.Name = "TranslationSettingButton"
    Me.TranslationSettingButton.Size = New System.Drawing.Size(84, 23)
    Me.TranslationSettingButton.TabIndex = 1
    Me.TranslationSettingButton.Text = "翻訳設定"
    Me.TranslationSettingButton.UseVisualStyleBackColor = True
    '
    'SrcPathLabel
    '
    Me.SrcPathLabel.AutoSize = True
    Me.SrcPathLabel.Location = New System.Drawing.Point(12, 9)
    Me.SrcPathLabel.Name = "SrcPathLabel"
    Me.SrcPathLabel.Size = New System.Drawing.Size(93, 12)
    Me.SrcPathLabel.TabIndex = 2
    Me.SrcPathLabel.Text = "GameDataフォルダ"
    '
    'SrcPathTextBox
    '
    Me.SrcPathTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SrcPathTextBox.BackColor = System.Drawing.SystemColors.Window
    Me.SrcPathTextBox.Location = New System.Drawing.Point(26, 24)
    Me.SrcPathTextBox.Name = "SrcPathTextBox"
    Me.SrcPathTextBox.ReadOnly = True
    Me.SrcPathTextBox.Size = New System.Drawing.Size(355, 19)
    Me.SrcPathTextBox.TabIndex = 3
    '
    'SrcPathButton
    '
    Me.SrcPathButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.SrcPathButton.Location = New System.Drawing.Point(387, 20)
    Me.SrcPathButton.Name = "SrcPathButton"
    Me.SrcPathButton.Size = New System.Drawing.Size(22, 23)
    Me.SrcPathButton.TabIndex = 4
    Me.SrcPathButton.Text = "..."
    Me.SrcPathButton.UseVisualStyleBackColor = True
    '
    'DstPathLabel
    '
    Me.DstPathLabel.AutoSize = True
    Me.DstPathLabel.Location = New System.Drawing.Point(12, 51)
    Me.DstPathLabel.Name = "DstPathLabel"
    Me.DstPathLabel.Size = New System.Drawing.Size(93, 12)
    Me.DstPathLabel.TabIndex = 5
    Me.DstPathLabel.Text = "翻訳データ作成先"
    '
    'DstPathTextBox
    '
    Me.DstPathTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.DstPathTextBox.BackColor = System.Drawing.SystemColors.Window
    Me.DstPathTextBox.Location = New System.Drawing.Point(26, 66)
    Me.DstPathTextBox.Name = "DstPathTextBox"
    Me.DstPathTextBox.ReadOnly = True
    Me.DstPathTextBox.Size = New System.Drawing.Size(355, 19)
    Me.DstPathTextBox.TabIndex = 6
    '
    'DstPathButton
    '
    Me.DstPathButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.DstPathButton.Location = New System.Drawing.Point(387, 62)
    Me.DstPathButton.Name = "DstPathButton"
    Me.DstPathButton.Size = New System.Drawing.Size(22, 23)
    Me.DstPathButton.TabIndex = 7
    Me.DstPathButton.Text = "..."
    Me.DstPathButton.UseVisualStyleBackColor = True
    '
    'TranslationButton
    '
    Me.TranslationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.TranslationButton.Location = New System.Drawing.Point(12, 152)
    Me.TranslationButton.Name = "TranslationButton"
    Me.TranslationButton.Size = New System.Drawing.Size(93, 23)
    Me.TranslationButton.TabIndex = 8
    Me.TranslationButton.Text = "処理実行"
    Me.TranslationButton.UseVisualStyleBackColor = True
    '
    'BackgroundWorker
    '
    '
    'CancelTranslationButton
    '
    Me.CancelTranslationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.CancelTranslationButton.Location = New System.Drawing.Point(44, 164)
    Me.CancelTranslationButton.Name = "CancelTranslationButton"
    Me.CancelTranslationButton.Size = New System.Drawing.Size(93, 23)
    Me.CancelTranslationButton.TabIndex = 9
    Me.CancelTranslationButton.Text = "処理中止"
    Me.CancelTranslationButton.UseVisualStyleBackColor = True
    '
    'ProgressBar
    '
    Me.ProgressBar.Location = New System.Drawing.Point(26, 111)
    Me.ProgressBar.Name = "ProgressBar"
    Me.ProgressBar.Size = New System.Drawing.Size(355, 24)
    Me.ProgressBar.TabIndex = 10
    '
    'ProgressLabel
    '
    Me.ProgressLabel.Location = New System.Drawing.Point(29, 97)
    Me.ProgressLabel.Name = "ProgressLabel"
    Me.ProgressLabel.Size = New System.Drawing.Size(352, 11)
    Me.ProgressLabel.TabIndex = 11
    Me.ProgressLabel.Text = "ProgressLabel"
    Me.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
    '
    'ImportTranslationCfgButton
    '
    Me.ImportTranslationCfgButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.ImportTranslationCfgButton.Location = New System.Drawing.Point(166, 152)
    Me.ImportTranslationCfgButton.Name = "ImportTranslationCfgButton"
    Me.ImportTranslationCfgButton.Size = New System.Drawing.Size(93, 23)
    Me.ImportTranslationCfgButton.TabIndex = 12
    Me.ImportTranslationCfgButton.Text = "翻訳cfg読込"
    Me.ImportTranslationCfgButton.UseVisualStyleBackColor = True
    '
    'FormMain
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(421, 185)
    Me.Controls.Add(Me.ImportTranslationCfgButton)
    Me.Controls.Add(Me.ProgressLabel)
    Me.Controls.Add(Me.ProgressBar)
    Me.Controls.Add(Me.CancelTranslationButton)
    Me.Controls.Add(Me.TranslationButton)
    Me.Controls.Add(Me.DstPathButton)
    Me.Controls.Add(Me.DstPathTextBox)
    Me.Controls.Add(Me.DstPathLabel)
    Me.Controls.Add(Me.SrcPathButton)
    Me.Controls.Add(Me.SrcPathTextBox)
    Me.Controls.Add(Me.SrcPathLabel)
    Me.Controls.Add(Me.TranslationSettingButton)
    Me.Controls.Add(Me.CloseButton)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FormMain"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "FormMain"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

  Friend WithEvents CloseButton As Button
  Friend WithEvents TranslationSettingButton As Button
  Friend WithEvents SrcPathLabel As Label
  Friend WithEvents SrcPathTextBox As TextBox
  Friend WithEvents SrcPathButton As Button
  Friend WithEvents DstPathLabel As Label
  Friend WithEvents DstPathTextBox As TextBox
  Friend WithEvents DstPathButton As Button
  Friend WithEvents TranslationButton As Button
  Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
  Friend WithEvents CancelTranslationButton As Button
  Friend WithEvents ProgressBar As ProgressBar
  Friend WithEvents ProgressLabel As Label
  Friend WithEvents ImportTranslationCfgButton As Button
End Class
