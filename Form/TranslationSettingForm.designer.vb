<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TranslationSettingForm
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
    Me.SaveButton = New System.Windows.Forms.Button()
    Me.IDLabel = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.ClientSecretTextBox = New System.Windows.Forms.TextBox()
    Me.AutoTranslationCheckBox = New System.Windows.Forms.CheckBox()
    Me.ClientIdTextBox = New System.Windows.Forms.TextBox()
    Me.SuspendLayout()
    '
    'CloseButton
    '
    Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.CloseButton.Location = New System.Drawing.Point(390, 106)
    Me.CloseButton.Name = "CloseButton"
    Me.CloseButton.Size = New System.Drawing.Size(75, 23)
    Me.CloseButton.TabIndex = 0
    Me.CloseButton.Text = "キャンセル"
    Me.CloseButton.UseVisualStyleBackColor = True
    '
    'SaveButton
    '
    Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.SaveButton.Location = New System.Drawing.Point(309, 106)
    Me.SaveButton.Name = "SaveButton"
    Me.SaveButton.Size = New System.Drawing.Size(75, 23)
    Me.SaveButton.TabIndex = 1
    Me.SaveButton.Text = "保存"
    Me.SaveButton.UseVisualStyleBackColor = True
    '
    'IDLabel
    '
    Me.IDLabel.AutoSize = True
    Me.IDLabel.Location = New System.Drawing.Point(22, 72)
    Me.IDLabel.Name = "IDLabel"
    Me.IDLabel.Size = New System.Drawing.Size(63, 12)
    Me.IDLabel.TabIndex = 3
    Me.IDLabel.Text = "顧客の秘密"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(22, 47)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(67, 12)
    Me.Label3.TabIndex = 4
    Me.Label3.Text = "クライアントID"
    '
    'ClientSecretTextBox
    '
    Me.ClientSecretTextBox.Location = New System.Drawing.Point(123, 69)
    Me.ClientSecretTextBox.Name = "ClientSecretTextBox"
    Me.ClientSecretTextBox.Size = New System.Drawing.Size(298, 19)
    Me.ClientSecretTextBox.TabIndex = 5
    '
    'AutoTranslationCheckBox
    '
    Me.AutoTranslationCheckBox.AutoSize = True
    Me.AutoTranslationCheckBox.Location = New System.Drawing.Point(12, 12)
    Me.AutoTranslationCheckBox.Name = "AutoTranslationCheckBox"
    Me.AutoTranslationCheckBox.Size = New System.Drawing.Size(253, 16)
    Me.AutoTranslationCheckBox.TabIndex = 7
    Me.AutoTranslationCheckBox.Text = "Microsoft Translator APIを使用して翻訳を行う"
    Me.AutoTranslationCheckBox.UseVisualStyleBackColor = True
    '
    'ClientIdTextBox
    '
    Me.ClientIdTextBox.Location = New System.Drawing.Point(123, 44)
    Me.ClientIdTextBox.Name = "ClientIdTextBox"
    Me.ClientIdTextBox.Size = New System.Drawing.Size(298, 19)
    Me.ClientIdTextBox.TabIndex = 8
    '
    'TranslationSettingForm
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(477, 141)
    Me.Controls.Add(Me.ClientIdTextBox)
    Me.Controls.Add(Me.AutoTranslationCheckBox)
    Me.Controls.Add(Me.ClientSecretTextBox)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.IDLabel)
    Me.Controls.Add(Me.SaveButton)
    Me.Controls.Add(Me.CloseButton)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "TranslationSettingForm"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "翻訳設定設定"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

  Friend WithEvents CloseButton As Button
  Friend WithEvents SaveButton As Button
  Friend WithEvents IDLabel As Label
  Friend WithEvents Label3 As Label
  Friend WithEvents ClientSecretTextBox As TextBox
  Friend WithEvents AutoTranslationCheckBox As CheckBox
  Friend WithEvents ClientIdTextBox As TextBox
End Class
