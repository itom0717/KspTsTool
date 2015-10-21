Imports System.ComponentModel


''' <summary>
''' メインフォーム
''' </summary>
Public Class FormMain

  ''' <summary>
  ''' 翻訳ファイル取り込みフォルダの前回のフォルダ記憶用
  ''' </summary>
  Private ImportTranslationFilrPath As String = ""


  ''' <summary>
  ''' FormMain_Load
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load

    'フォームタイトル設定
    Me.Text = My.Application.Info.ProductName

    '初期値（前回の値)を設定
    Me.SrcPathTextBox.Text = Common.File.AddDirectorySeparator(My.Settings.SrcPath)
    Me.DstPathTextBox.Text = Common.File.AddDirectorySeparator(My.Settings.DstPath)

    'キャンセルボタン設定
    Me.CancelTranslationButton.Top = Me.TranslationButton.Top
    Me.CancelTranslationButton.Left = Me.TranslationButton.Left
    Me.CancelTranslationButton.Width = Me.TranslationButton.Width
    Me.CancelTranslationButton.Height = Me.TranslationButton.Height
    Me.CancelTranslationButton.Visible = False

    '処理中テキストクリア
    Me.ProgressLabel.Text = ""

    'ボタンの状態変更
    Me.EnableButtons(True)

  End Sub

  ''' <summary>
  ''' FormMain_FormClosed  
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub FormMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
    '設定値記憶
    My.Settings.SrcPath = Me.SrcPathTextBox.Text
    My.Settings.DstPath = Me.DstPathTextBox.Text
    My.Settings.Save()
  End Sub

  ''' <summary>
  ''' FormMain_FormClosing
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    'バックグラウンド処理中であれば中止確認する
    If Me.BackgroundWorker.IsBusy Then
      e.Cancel = Not Me.ConfirmCancel()
    End If

  End Sub

  ''' <summary>
  ''' 閉じるボタン
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
    Me.Close()
  End Sub

  ''' <summary>
  ''' フォルダ選択ボタン
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub PathButton_Click(sender As Object, e As EventArgs) Handles SrcPathButton.Click, DstPathButton.Click
    Dim button As Button = DirectCast(sender, Button)

    Dim path As String
    '押されたボタンによって取得するテキストボックスを分ける
    If button.Name.Equals(Me.SrcPathButton.Name) Then
      'SrcPath
      path = Me.SrcPathTextBox.Text
    Else
      'DstPath
      path = Me.DstPathTextBox.Text
    End If


    'フォルダ選択
    'FolderBrowserDialogクラスのインスタンスを作成
    Dim folderBrowserDialog As New FolderBrowserDialog
    With folderBrowserDialog

      '上部に表示する説明テキストを指定する
      .Description = "フォルダを指定してください。"

      'ルートフォルダを指定する
      .RootFolder = Environment.SpecialFolder.Desktop

      '最初に選択するフォルダを指定する
      'RootFolder以下にあるフォルダである必要がある
      .SelectedPath = path

      'ユーザーが新しいフォルダを作成できるようにする
      .ShowNewFolderButton = True

      'ダイアログを表示する
      If .ShowDialog(Me) = DialogResult.OK Then
        '選択された
        path = Common.File.AddDirectorySeparator(.SelectedPath)
      Else
        path = "" 'キャンセル
      End If
    End With

    'フォルダが選択された場合値をセットする
    If Not path.Equals("") Then
      'テキストボックスにセット
      If button.Name.Equals(Me.SrcPathButton.Name) Then
        'SrcPath
        Me.SrcPathTextBox.Text = path
      Else
        'DstPath
        Me.DstPathTextBox.Text = path
      End If
    End If
  End Sub


  ''' <summary>
  ''' フォルダを開くボタン
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub OpenPathButton_Click(sender As Object, e As EventArgs) Handles OpenSrcPathButton.Click, OpenDstPathButton.Click

    Dim button As Button = DirectCast(sender, Button)

    Dim path As String
    '押されたボタンによって取得するテキストボックスを分ける
    If button.Name.Equals(Me.OpenSrcPathButton.Name) Then
      'SrcPath
      path = Me.SrcPathTextBox.Text
    Else
      'DstPath
      path = Me.DstPathTextBox.Text
    End If

    If path.Equals("") Then
      MessageBox.Show("フォルダを選択してください。",
                      My.Application.Info.ProductName,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information)
      Return
    End If

    path = Common.File.AddDirectorySeparator(path)
    If Not Common.File.ExistsDirectory(path) Then
      MessageBox.Show("存在しないフォルダです。",
                My.Application.Info.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
      Return
    End If

    Try
      'フォルダを開く
      System.Diagnostics.Process.Start(path)
    Catch ex As Exception
      MessageBox.Show("フォルダを開くことができません。",
          My.Application.Info.ProductName,
          MessageBoxButtons.OK,
          MessageBoxIcon.Error)
      Return
    End Try
  End Sub


  ''' <summary>
  ''' 翻訳ファイルの読込
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub ImportTranslationCfgButton_Click(sender As Object, e As EventArgs) Handles ImportTranslationCfgButton.Click
    Dim filename As String = ""

    '記憶されているフォルダのチェック
    If Me.ImportTranslationFilrPath.Equals("") _
      OrElse Not Common.File.ExistsDirectory(Me.ImportTranslationFilrPath) Then
      '未設定or存在しないので、デスクトップを初期値として設定
      Me.ImportTranslationFilrPath = Common.File.GetDesktopDirectory
    End If

    'OpenFileDialogクラスのインスタンスを作成
    Dim openFileDialog As New OpenFileDialog
    With openFileDialog

      'はじめに「ファイル名」で表示される文字列を指定する
      .FileName = ""

      'はじめに表示されるフォルダを指定する
      .InitialDirectory = Me.ImportTranslationFilrPath

      '[ファイルの種類]に表示される選択肢を指定
      .Filter = "ModuleManager用cfgファイル|*.cfg|翻訳データベース|*." & Common.File.GetExtension(TranslationDataBase.DatabaseFileName) & ""

      '[ファイルの種類]
      .FilterIndex = 1

      'タイトルを設定する
      .Title = "翻訳用ModuleManagerのcfgファイルまたは翻訳データベースを選択してください"

      'ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
      .RestoreDirectory = True

      '存在しないファイルの名前が指定されたとき警告を表示する
      .CheckFileExists = True

      '存在しないパスが指定されたとき警告を表示する
      .CheckPathExists = True

      'ダイアログを表示する
      If .ShowDialog() = DialogResult.OK Then
        'OKボタンがクリックされた
        filename = .FileName

        'フォルダを記憶
        Me.ImportTranslationFilrPath = Common.File.GetDirectoryName(filename)
      End If
    End With
    If filename.Equals("") Then
      Return
    End If


    Try
      '翻訳データベースへ取り込み
      Dim translationDataBase As New TranslationDataBase()
      Dim importCount As Integer = translationDataBase.ImportTranslationFile(filename)
      translationDataBase = Nothing

      Dim msg As String = ""
      If importCount >= 1 Then
        msg = "読込処理が終了しました。(" & importCount & "件読込)"
      Else
        msg = "取り込むデータはありませんでした。"
      End If
      MessageBox.Show(msg,
                      My.Application.Info.ProductName,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information)

    Catch ex As Exception
      MessageBox.Show(ex.Message,
                My.Application.Info.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
    End Try

  End Sub


  ''' <summary>
  ''' 翻訳設定画面を出す
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub TranslationSettingButton_Click(sender As Object, e As EventArgs) Handles TranslationSettingButton.Click
    Dim form As New TranslationSettingForm
    form.ShowDialog()
  End Sub


  ''' <summary>
  ''' 処理実行ボタン
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub TranslationButton_Click(sender As Object, e As EventArgs) Handles TranslationButton.Click

    '実行確認
    If MessageBox.Show("処理を実行しますか？",
                       My.Application.Info.ProductName,
                       MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question) <> DialogResult.OK Then
      Return
    End If

    Try

      '処理設定値の取得＆値チェック
      Dim translationSetting As New TranslationSetting
      With Nothing
        Dim errMsg As String = ""
        translationSetting.SrcPath = Me.SrcPathTextBox.Text.Trim()
        translationSetting.DsgtPath = Me.DstPathTextBox.Text.Trim()
        translationSetting.IsTranslation = My.Settings.IsTranslation
        translationSetting.MicrosoftTranslatorAPIClientId = My.Settings.MicrosoftTranslatorAPIClientId
        translationSetting.MicrosoftTranslatorAPIClientSecret = My.Settings.MicrosoftTranslatorAPIClientSecret
        If translationSetting.MicrosoftTranslatorAPIClientId.Trim.Equals("") _
          OrElse translationSetting.MicrosoftTranslatorAPIClientSecret.Trim.Equals("") Then
          'ClientIdまたはClientSecretが指定していない場合は、自動翻訳無効
          translationSetting.IsTranslation = False
        End If

        'パスの確認
        If translationSetting.SrcPath.Equals("") Then
          errMsg = Me.SrcPathLabel.Text & "を指定してください。"
        ElseIf translationSetting.SrcPath.Equals("") Then
          errMsg = Me.DstPathLabel.Text & "を指定してください。"
        ElseIf Not Common.File.ExistsDirectory(translationSetting.SrcPath) Then
          errMsg = Me.SrcPathLabel.Text & "で指定されたフォルダが見つかりません。"
        ElseIf Not Common.File.ExistsDirectory(translationSetting.DsgtPath) Then
          errMsg = Me.DstPathLabel.Text & "で指定されたフォルダが見つかりません。"
          'ElseIf Not Common.File.IsEmptyDirectory(translationSetting.DsgtPath) Then
          '  '保存先は空でないとダメ
          '  errMsg = Me.DstPathLabel.Text & "で指定されたフォルダが空ではありません。"
        End If
        If Not errMsg.Equals("") Then
          '設定値にエラーが有る場合エラーを表示してここで戻る
          MessageBox.Show(errMsg,
                          My.Application.Info.ProductName,
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information)
          Return
        End If
      End With

      If Not Common.File.IsEmptyDirectory(translationSetting.DsgtPath) Then

        If MessageBox.Show("保存先のフォルダにファイルが存在しますが続行しますか？",
                   My.Application.Info.ProductName,
                   MessageBoxButtons.OKCancel,
                   MessageBoxIcon.Question) <> DialogResult.OK Then
          Return
        End If

      End If




      'プログレスバー初期設定
      Me.ProgressLabel.Text = "処理開始"
      Me.ProgressBar.Value = 0
      Me.ProgressBar.Minimum = 0
      Me.ProgressBar.Maximum = 100

      'ボタン類を無効にする
      Me.EnableButtons(False)

      'バックグラウンド処理を開始する
      Me.BackgroundWorker.WorkerReportsProgress = True 'プログレスバー表示する
      Me.BackgroundWorker.WorkerSupportsCancellation = True 'キャンセル可能
      Me.BackgroundWorker.RunWorkerAsync(translationSetting)

    Catch ex As Exception
      'エラーが出たら終了
      MessageBox.Show(ex.Message,
                      My.Application.Info.ProductName,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error)
      Me.Close()
    End Try

  End Sub

  ''' <summary>
  ''' ボタン類のDisable/Enable切り替え
  ''' </summary>
  Private Sub EnableButtons(isEnable As Boolean)
    Me.SrcPathButton.Enabled = isEnable
    Me.DstPathButton.Enabled = isEnable
    Me.ImportTranslationCfgButton.Enabled = isEnable
    Me.TranslationSettingButton.Enabled = isEnable
    Me.TranslationButton.Visible = isEnable
    Me.CancelTranslationButton.Visible = Not isEnable
  End Sub

  ''' <summary>
  ''' 中止ボタン
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelTranslationButton.Click
    If Me.BackgroundWorker.IsBusy Then
      Me.ConfirmCancel()
    End If
  End Sub

  ''' <summary>
  ''' 中止処理確認＆中止設定
  ''' </summary>
  ''' <return>中止したらTrueを返す</return>
  Private Function ConfirmCancel() As Boolean

    If MessageBox.Show("中止しますか？",
                       My.Application.Info.ProductName,
                       MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question) <> DialogResult.OK Then

      Return False
    End If

    '中止設定
    If Me.BackgroundWorker.IsBusy Then
      Me.BackgroundWorker.CancelAsync()
      Me.CancelTranslationButton.Enabled = False
    End If

    Return True
  End Function

  ''' <summary>
  ''' 処理開始
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker.DoWork

    'BackgroundWorker
    Dim bw As BackgroundWorker = DirectCast(sender, BackgroundWorker)

    '処理設定値
    Dim translationSetting As TranslationSetting = CType(e.Argument, TranslationSetting)

    '翻訳DB
    Dim translationDataBase As TranslationDataBase
    If translationSetting.IsTranslation Then
      translationDataBase = New TranslationDataBase(translationSetting.MicrosoftTranslatorAPIClientId,
                                                    translationSetting.MicrosoftTranslatorAPIClientSecret)
    Else
      translationDataBase = New TranslationDataBase()
    End If


    Try

      '翻訳DB読込
      translationDataBase.LoadDatabse()

      '処理対象フォルダ一覧格納用List
      Dim execDirList As New List(Of String)


      With Nothing
        '第一階層のフォルダを列挙して、対象のフォルダを調査
        Dim dirList As List(Of String) = Common.File.GetFolderList(translationSetting.SrcPath, "*.*", False)
        '除くフォルダ判定
        For i As Integer = 0 To dirList.Count - 1
          'ディレクトリ名
          Dim dirName As String = Common.File.GetFileName(Common.File.DeleteDirectorySeparator(dirList(i)))
          'Partsフォルダ
          Dim partDir As String = Common.File.AddDirectorySeparator(Common.File.AddDirectorySeparator(dirList(i)) & "Parts")

          If dirName.Equals("ToJPparts", StringComparison.CurrentCultureIgnoreCase) Then
            '登録しない
          ElseIf dirName.Equals("Flags", StringComparison.CurrentCultureIgnoreCase) Then
            '登録しない
          ElseIf dirName.Equals("") Then
            '登録しない
          ElseIf Not Common.File.ExistsDirectory(partDir) Then
            'Partsフォルダがない場合は登録しない
          Else
            execDirList.Add(dirList(i))
          End If
        Next
      End With



      With Nothing
        Dim progressText As String = ""
        Dim progressNow As Double
        Dim progressRatio As Double
        Dim progressRatioSub As Double


        '各フォルダの処理
        progressRatio = CDbl(1.0 / execDirList.Count)
        For i As Integer = 0 To execDirList.Count - 1
          'ディレクトリ名
          Dim dirName As String = Common.File.GetFileName(Common.File.DeleteDirectorySeparator(execDirList(i)))
          'Partsフォルダ
          Dim partDir As String = Common.File.AddDirectorySeparator(Common.File.AddDirectorySeparator(execDirList(i)) & "Parts")

          '処理状況の表示を変更する
          progressText = "[ " & dirName & " ]"
          progressNow = CDbl(progressRatio * i)
          bw.ReportProgress(CInt(progressNow * 100.0), progressText & "を処理中...")

          'キャンセルされたか調べる
          If bw.CancellationPending Then
            'キャンセルされたとき
            e.Cancel = True
            Return
          End If

          With Nothing
            'パーツ情報クラスを定義
            Dim partInfoList As New PartInfoList(Common.File.AddDirectorySeparator(translationSetting.DsgtPath) & dirName & ".cfg")

            'フォルダ内の*.cfgを列挙
            Dim cfgList As List(Of String) = Common.File.GetFileList(partDir, "*.cfg", True)
            If cfgList.Count = 0 Then
              'ファイル無しは次へ
              Continue For
            End If

            'cgfを順番に処理
            progressRatioSub = CDbl((progressRatio / 2.0) / cfgList.Count)
            For j As Integer = 0 To cfgList.Count - 1
              Dim cfgFile As String = cfgList(j)

              '処理状況の表示を変更する
              progressNow += progressRatioSub
              bw.ReportProgress(CInt(progressNow * 100.0), progressText & " --- cfgファイル調査中(" & (j + 1) & "/" & cfgList.Count & ")...")

              'キャンセルされたか調べる
              If bw.CancellationPending Then
                'キャンセルされたとき
                e.Cancel = True
                Return
              End If

              'cfgファイルから情報取得
              Dim partInfo As New PartInfo(cfgFile)
              If Not partInfo.NoPartData Then
                '情報が取得できたらpartInfoListに追加する
                partInfoList.Add(partInfo)
              End If
            Next


            '翻訳処理
            progressRatioSub = CDbl((progressRatio / 2.0) / partInfoList.Count)
            For j As Integer = 0 To partInfoList.Count - 1
              Dim partInfo As PartInfo = partInfoList(j)

              '処理状況の表示を変更する
              progressNow += progressRatioSub
              bw.ReportProgress(CInt(progressNow * 100.0), progressText & " --- 翻訳中(" & (j + 1) & "/" & partInfoList.Count & ")...")

              'キャンセルされたか調べる
              If bw.CancellationPending Then
                'キャンセルされたとき
                e.Cancel = True
                Return
              End If

              '翻訳処理
              partInfo.DescriptionJapanese = translationDataBase.Translate(dirName,
                                                                           partInfo.Name,
                                                                           partInfo.Title,
                                                                           partInfo.Description)
            Next


            If partInfoList.Count >= 1 Then
              'データがあれば翻訳パッチファイルに保存する
              partInfoList.Save()
            End If
          End With

        Next
      End With


    Finally

      '翻訳DB保存
      translationDataBase.SaveDatabase()

    End Try

  End Sub


  ''' <summary>
  ''' 処理中表示
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker.ProgressChanged

    'プログレスバーの値を変更する
    If e.ProgressPercentage < Me.ProgressBar.Minimum Then
      Me.ProgressBar.Value = Me.ProgressBar.Minimum
    ElseIf Me.ProgressBar.Maximum < e.ProgressPercentage Then
      Me.ProgressBar.Value = Me.ProgressBar.Maximum
    Else
      Me.ProgressBar.Value = e.ProgressPercentage
    End If
    'メッセージのテキストを変更する
    Me.ProgressLabel.Text = DirectCast(e.UserState, String)

  End Sub


  ''' <summary>
  ''' バックグラウンド処理完了時の処理
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker.RunWorkerCompleted

    If e.Error IsNot Nothing Then
      'エラー
      Me.ProgressLabel.Text = "エラーが発生しました。"

      MessageBox.Show(e.Error.Message,
                      My.Application.Info.ProductName,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error)

    ElseIf e.Cancelled Then

      'キャンセル
      Me.ProgressLabel.Text = "処理を中止しました。"

    Else
      '成功
      Me.ProgressBar.Value = Me.ProgressBar.Maximum
      Me.ProgressLabel.Text = "処理が終了しました。"

      'フォルダを開く
      System.Diagnostics.Process.Start(Me.DstPathTextBox.Text.Trim())
    End If

    'ボタン類を有効にする
    Me.EnableButtons(True)
  End Sub


End Class
