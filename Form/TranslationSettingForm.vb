Public Class TranslationSettingForm
  Private Sub TranslationSettingForm_Load(sender As Object, e As EventArgs) Handles Me.Load
    Me.ClientIdTextBox.Text = My.Settings.MicrosoftTranslatorAPIClientId
    Me.ClientSecretTextBox.Text = My.Settings.MicrosoftTranslatorAPIClientSecret

    If Me.AutoTranslationCheckBox.Checked = My.Settings.IsTranslation Then
      '同じ場合は一旦逆の値を指定して、イベントが発生するようにする
      Me.AutoTranslationCheckBox.Checked = Not My.Settings.IsTranslation
    End If
    Me.AutoTranslationCheckBox.Checked = My.Settings.IsTranslation
  End Sub

  ''' <summary>
  ''' 保存して閉じる
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
    My.Settings.MicrosoftTranslatorAPIClientId = Me.ClientIdTextBox.Text
    My.Settings.MicrosoftTranslatorAPIClientSecret = Me.ClientSecretTextBox.Text
    My.Settings.IsTranslation = Me.AutoTranslationCheckBox.Checked
    Me.Close()
  End Sub

  ''' <summary>
  ''' 閉じる
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Close()
  End Sub

  ''' <summary>
  ''' AutoTranslationCheckBox
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub AutoTranslationCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles AutoTranslationCheckBox.CheckedChanged
    Dim checkBox As CheckBox = CType(sender, CheckBox)
    If checkBox.Checked Then
      Me.ClientIdTextBox.Enabled = True
      Me.ClientSecretTextBox.Enabled = True
    Else
      Me.ClientIdTextBox.Enabled = False
      Me.ClientSecretTextBox.Enabled = False
    End If
  End Sub
End Class