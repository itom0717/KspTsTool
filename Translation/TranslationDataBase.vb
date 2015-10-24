''' <summary>
''' 翻訳データベース処理
''' </summary>
Public Class TranslationDataBase

  ''' <summary>
  ''' 翻訳データベースの名前
  ''' </summary>
  Public Const DatabaseFileName As String = "TranslationDataBase.tsv"


  ''' <summary>
  ''' 翻訳DataTable
  ''' </summary>
  Public TranslationDataTable As TranslationDataTable = Nothing

  ''' <summary>
  ''' ファイル名
  ''' </summary>
  Private DatabaseFilePath As String = ""

  ''' <summary>
  ''' 自動翻訳処理用Class
  ''' </summary>
  Private TranslatorApi As TranslatorApi = Nothing

  ''' <summary>
  ''' インスタンスを生成(TranslatorApi使用しない場合)
  ''' </summary>
  Public Sub New()
    Me.New("", "", "")
  End Sub

  ''' <summary>
  ''' インスタンスを生成(TranslatorApi使用しない場合+インポートファイル読込用)
  ''' </summary>
  ''' <param name="databaseFilePath"></param>
  Public Sub New(databaseFilePath As String)
    Me.New("", "", databaseFilePath)
  End Sub

  ''' <summary>
  ''' インスタンスを生成(TranslatorApi使用する場合)
  ''' </summary>
  ''' <param name="clientId">マイクロソフトへ登録した「クライアントID」</param>
  ''' <param name="clientSecret">マイクロソフトへ登録した「顧客の秘密」</param>
  ''' <remarks>
  ''' clientId または clientSecret が空欄の場合は、TranslatorApiを使用しない。
  ''' </remarks>
  Public Sub New(clientId As String, clientSecret As String, Optional databaseFilePath As String = "")

    ' ファイル名
    If databaseFilePath.Equals("") Then
      Me.DatabaseFilePath = Common.File.AddDirectorySeparator(My.Application.Info.DirectoryPath) & DatabaseFileName
    Else
      'インポート用ファイル
      Me.DatabaseFilePath = databaseFilePath
    End If

    '翻訳DataTable
    Me.TranslationDataTable = New TranslationDataTable

    'TranslatorApi（自動翻訳）の設定
    If Not clientId.Equals("") AndAlso Not clientSecret.Equals("") Then
      'TranslatorApi使用する
      Me.TranslatorApi = New TranslatorApi(clientId, clientSecret)
    Else
      'TranslatorApi使用しない
      Me.TranslatorApi = Nothing
    End If
  End Sub

  ''' <summary>
  ''' 終了処理
  ''' </summary>
  Protected Overrides Sub Finalize()
    Me.TranslationDataTable = Nothing
    Me.TranslatorApi = Nothing
    MyBase.Finalize()
  End Sub

  ''' <summary>
  ''' 翻訳データベースの読込
  ''' </summary>
  Public Sub LoadDatabse()

    If Not Common.File.ExistsFile(Me.DatabaseFilePath) Then
      'ファイルが存在しない場合は、何もしない
      Return
    End If

    Dim dirNameIndex As Integer = -1
    Dim partNameIndex As Integer = -1
    Dim partTitleIndex As Integer = -1
    Dim orgTextIndex As Integer = -1
    Dim japaneseTextIndex As Integer = -1
    Dim memoIndex As Integer = -1
    For i As Integer = 0 To Me.TranslationDataTable.Columns.Count - 1
      Select Case Me.TranslationDataTable.Columns(i).ColumnName
        Case TranslationDataTable.ColumnNameDirName
          dirNameIndex = i
        Case TranslationDataTable.ColumnNamePartName
          partNameIndex = i
        Case TranslationDataTable.ColumnNamePartTitle
          partTitleIndex = i
        Case TranslationDataTable.ColumnNameOriginalText
          orgTextIndex = i
        Case TranslationDataTable.ColumnNameJapaneseText
          japaneseTextIndex = i
        Case TranslationDataTable.ColumnNameMemo
          memoIndex = i
      End Select
    Next

    '旧フォーマット用
    Dim oldDirNameIndex As Integer = CInt(IIf(dirNameIndex < partTitleIndex, dirNameIndex, dirNameIndex - 1))
    Dim oldPartNameIndex As Integer = CInt(IIf(partNameIndex < partTitleIndex, partNameIndex, partNameIndex - 1))
    Dim oldOrgTextIndex As Integer = CInt(IIf(orgTextIndex < partTitleIndex, orgTextIndex, orgTextIndex - 1))
    Dim oldJapaneseTextIndex As Integer = CInt(IIf(japaneseTextIndex < partTitleIndex, japaneseTextIndex, japaneseTextIndex - 1))
    Dim oldMemoIndex As Integer = CInt(IIf(memoIndex < partTitleIndex, memoIndex, memoIndex - 1))



    'TextFieldParserでファイルを開く
    Using tfp As New FileIO.TextFieldParser(Me.DatabaseFilePath, System.Text.Encoding.UTF8)

      'フィールドが文字で区切られているとする
      tfp.TextFieldType = FileIO.FieldType.Delimited

      '区切り文字を指定 タブコードとする
      tfp.Delimiters = New String() {vbTab}

      'フィールドを"で囲み、改行文字、区切り文字を含めることができるか
      tfp.HasFieldsEnclosedInQuotes = False

      'フィールドの前後からスペースを削除する
      tfp.TrimWhiteSpace = True

      Dim oldFormat As Boolean = False
      If Not tfp.EndOfData Then
        '先頭行（ヘッダー行）は捨てる

        '先頭行（ヘッダー行）で新旧データ判定
        'フィールドを読み込む
        Dim fields As String() = tfp.ReadFields()

        If Not fields(partTitleIndex).Equals(TranslationDataTable.ColumnNamePartTitle) Then
          'Titleがない場合は、旧フォーマット
          oldFormat = True
        End If

      End If
      While Not tfp.EndOfData
        'フィールドを読み込む
        Dim fields As String() = tfp.ReadFields()

        Dim newRow As DataRow = Me.TranslationDataTable.NewRow()

        '各カラムにデータをセット
        If Not oldFormat Then
          newRow(dirNameIndex) = fields(dirNameIndex)
          newRow(partNameIndex) = fields(partNameIndex)
          newRow(partTitleIndex) = fields(partTitleIndex)
          newRow(orgTextIndex) = fields(orgTextIndex)
          newRow(japaneseTextIndex) = fields(japaneseTextIndex)
          newRow(memoIndex) = fields(memoIndex)
        Else
          '旧フォーマット
          newRow(dirNameIndex) = fields(oldDirNameIndex)
          newRow(partNameIndex) = fields(oldPartNameIndex)
          newRow(partTitleIndex) = ""
          newRow(orgTextIndex) = fields(oldOrgTextIndex)
          newRow(japaneseTextIndex) = fields(oldJapaneseTextIndex)
          newRow(memoIndex) = fields(oldMemoIndex)
        End If

        'DataTableに追加
        Me.TranslationDataTable.Rows.Add(newRow)
      End While

      'ファイルを閉じる
      tfp.Close()
    End Using

    '変更無しに設定
    Me.TranslationDataTable.AcceptChanges()
  End Sub

  ''' <summary>
  ''' 翻訳データベースの更新
  ''' </summary>
  Public Sub SaveDatabase()

    If IsNothing(Me.TranslationDataTable.GetChanges()) Then
      '変更されたデータがなければ何もしない
      Return
    End If

    Try
      Dim fileBase As String = Common.File.GetWithoutExtension(Me.DatabaseFilePath)


      '一旦*.tmpファイルで保存
      Dim tmpFilename As String = fileBase & ".tmp"
      If Common.File.ExistsFile(tmpFilename) Then
        Common.File.DeleteFile(tmpFilename)
      End If
      Using sw As New System.IO.StreamWriter(tmpFilename, False, System.Text.Encoding.UTF8)

        Dim columnLine As String = ""
        For Each column As DataColumn In Me.TranslationDataTable.Columns
          If Not columnLine.Equals("") Then
            columnLine &= vbTab
          End If
          columnLine &= column.ColumnName
        Next
        sw.WriteLine(columnLine)

        For Each row As DataRow In Me.TranslationDataTable.Select(Nothing,
                                                                  TranslationDataTable.ColumnNameDirName &
                                                                  "," &
                                                                  TranslationDataTable.ColumnNamePartName)
          Dim line As String = ""

          For i As Integer = 0 To Me.TranslationDataTable.Columns.Count - 1
            If i <> 0 Then
              line &= vbTab
            End If
            Dim value As String = row(i)
            If IsNothing(value) Then
              value = ""
            End If
            line &= value
          Next

          sw.WriteLine(line)
        Next
        sw.Close()
      End Using

      'バックアップファイルの処理
      For bkCount As Integer = 9 To 0 Step -1
        Dim filename1 As String = fileBase & CStr(IIf(bkCount = 0, ".bak", ".bk" & bkCount))
        If Common.File.ExistsFile(filename1) Then
          Common.File.DeleteFile(filename1)
        End If

        Dim filename2 As String = fileBase
        Select Case bkCount - 1
          Case -1
            filename2 &= "." & Common.File.GetExtension(Me.DatabaseFilePath)
          Case 0
            filename2 &= ".bak"
          Case Else
            filename2 &= ".bk" & (bkCount - 1)
        End Select
        If Common.File.ExistsFile(filename2) Then
          Common.File.MoveFile(filename2, filename1)
        End If

      Next

      '*.tmpを*.tsvへ変更
      Common.File.MoveFile(tmpFilename, Me.DatabaseFilePath)

      '正常に保存できたら、変更無しに設定
      Me.TranslationDataTable.AcceptChanges()

    Catch ex As Exception
      Throw
    End Try

  End Sub


  ''' <summary>
  ''' 翻訳ファイルの読込
  ''' </summary>
  ''' <param name="filename"></param>
  Public Function ImportTranslationFile(filename As String) As Integer


    If Common.File.GetExtension(filename).Equals(Common.File.GetExtension(DatabaseFileName), StringComparison.CurrentCultureIgnoreCase) Then
      'tsvファイル
      Return Me.ImportDatabaseFile(filename)
    Else
      ' ModuleManager用のcfgファイル読込
      Return Me.ImportModuleManagerCfgFile(filename)
    End If


  End Function

  ''' <summary>
  ''' 翻訳用データベースファイルの取り込み
  ''' </summary>
  ''' <param name="tsvFilename">tsvファイル</param>
  ''' <returns></returns>
  Private Function ImportDatabaseFile(tsvFilename As String) As Integer

    Try
      '翻訳DB読込
      Me.LoadDatabse()

      'インポートする翻訳DB
      Dim importTranslationDataBase As New TranslationDataBase(tsvFilename)
      importTranslationDataBase.LoadDatabse()

      '読み込んだデータでループ
      For Each importRow As DataRow In importTranslationDataBase.TranslationDataTable.Rows

        If Not CStr(importRow(TranslationDataTable.ColumnNameJapaneseText)).Equals("") Then

          'データが存在するかチェック
          Dim selectRow() As DataRow _
                      = Me.TranslationDataTable.Select(
                         TranslationDataTable.ColumnNameDirName & "='" & Me.DoubleSiglQrt(CStr(importRow(TranslationDataTable.ColumnNameDirName))) & "'" &
                         " AND " &
                         TranslationDataTable.ColumnNamePartName & "='" & Me.DoubleSiglQrt(CStr(importRow(TranslationDataTable.ColumnNamePartName))) & "'")

          If selectRow.Count > 0 Then
            '存在するため、データ変更
            For i As Integer = 0 To selectRow.Count - 1

              If Not CStr(selectRow(i)(TranslationDataTable.ColumnNameOriginalText)).Equals(CStr(importRow(TranslationDataTable.ColumnNameOriginalText))) _
                  OrElse Not CStr(selectRow(i)(TranslationDataTable.ColumnNameJapaneseText)).Equals(CStr(importRow(TranslationDataTable.ColumnNameJapaneseText))) Then
                selectRow(i)(TranslationDataTable.ColumnNameOriginalText) = CStr(importRow(TranslationDataTable.ColumnNameOriginalText))
                selectRow(i)(TranslationDataTable.ColumnNameJapaneseText) = CStr(importRow(TranslationDataTable.ColumnNameJapaneseText))
                selectRow(i)(TranslationDataTable.ColumnNameMemo) = Now.ToString(TranslationDataTable.MemoAddTextFormat)
              End If

            Next
          Else
            '存在しないので新規追加
            Dim newRow As DataRow = Me.TranslationDataTable.NewRow()

            '各カラムにデータをセット
            For i As Integer = 0 To Me.TranslationDataTable.Columns.Count - 1
              newRow(i) = importRow(i)
            Next

            'DataTableに追加
            Me.TranslationDataTable.Rows.Add(newRow)
          End If


        End If

      Next
      importTranslationDataBase = Nothing


      '変更件数を返す
      Dim importCount As Integer = 0
      Dim dt As DataTable = Me.TranslationDataTable.GetChanges()
      If Not IsNothing(dt) Then
        importCount = dt.Rows.Count
      End If

      '翻訳DB保存
      Me.SaveDatabase()

      Return importCount
    Catch ex As Exception
      Throw
    End Try

  End Function

  ''' <summary>
  ''' ModuleManager用のcfgファイル読込
  ''' </summary>
  ''' <param name="moduleManagerCfgFilename"></param>
  Private Function ImportModuleManagerCfgFile(moduleManagerCfgFilename As String) As Integer

    Try
      Dim rPart As New System.Text.RegularExpressions.Regex(
            "^@PART\s*\[([^\}]+)\]",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rDescription As New System.Text.RegularExpressions.Regex(
              "@description\s*=\s*([^}]+)($|})",
              System.Text.RegularExpressions.RegexOptions.IgnoreCase)


      Dim rExpDef As New System.Text.RegularExpressions.Regex(
          "^@EXPERIMENT_DEFINITION\s*:\s*HAS\[\s*#id\[\s*([^\}]+)\s*\]\s*\]($|{|\s)",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rResult As New System.Text.RegularExpressions.Regex(
          "^@RESULTS($|{|\s)",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rKeyText As New System.Text.RegularExpressions.Regex(
            "^@([^=,]+),(\d+)\s*=\s*(.+)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)





      Dim mc As System.Text.RegularExpressions.MatchCollection
      Dim part As String = ""
      Dim expDef As String = ""

      Dim description As String = ""
      Dim check As New Hashtable

      '翻訳DB読込
      Me.LoadDatabse()

      '各cfgファイルの解析
      Using sr As New System.IO.StreamReader(moduleManagerCfgFilename, System.Text.Encoding.UTF8)

        Dim nestLevel As Integer = 0
        Dim isPart As Boolean = False 'Partの中
        Dim isPartEnter As Boolean = False 'partの中に入った
        Dim isExpDef As Boolean = False 'EXPERIMENT_DEFINITIONの中
        Dim isExpDefEnter As Boolean = False 'EXPERIMENT_DEFINITIONの中に入った
        Dim isResult As Boolean = False 'Resultの中
        Dim isResultEnter As Boolean = False 'Resultの中に入った



        While sr.Peek() > -1
          Dim line As String = sr.ReadLine()

          '空白を取り除く
          line = line.Trim()

          'コメント削除
          Dim ptr1 = line.IndexOf("//", 0)
          If ptr1 >= 0 Then
            line = line.Substring(0, ptr1).Trim()
          End If

          If line.Equals("") Then
            Continue While
          End If

          '@PART[***]
          mc = rPart.Matches(line)
          If mc.Count >= 1 Then
            part = mc(0).Groups(1).Value
            If Not isPart AndAlso nestLevel = 0 AndAlso Not part.Equals("") Then
              'PARTの中に入った
              isPart = True
              isPartEnter = False
            End If
          End If


          '@EXPERIMENT_DEFINITION:HAS[#id[***]]
          mc = rExpDef.Matches(line)
          If mc.Count >= 1 Then
            expDef = mc(0).Groups(1).Value
            If Not isExpDef AndAlso nestLevel = 0 AndAlso Not expDef.Equals("") Then
              'EXPERIMENT_DEFINITIONの中に入った
              isExpDef = True
              isExpDefEnter = False
              isResult = False
              isResultEnter = False
            End If
          End If

          'Result判定
          If Not isResult AndAlso nestLevel = 1 AndAlso rResult.IsMatch(line) Then
            isResult = True
            isResultEnter = False
          End If


          '括弧のネスト判定
          For i As Integer = 0 To line.Length - 1
            If line.Substring(i, 1).Equals("{") Then
              nestLevel += 1
            End If
          Next


          'descriptionの取得
          If isPart AndAlso nestLevel = 1 Then
            isPartEnter = True 'partの中に入った

            mc = rDescription.Matches(line)
            If mc.Count >= 1 Then
              description = mc(0).Groups(1).Value.Trim()


              If Not part.Equals("") AndAlso Not check.ContainsKey(part) Then
                check.Add(part, 1) '同じ名前のものは取り込まない
                Me.UpdateTranslateDatabaseForImportModuleManagerCfg(part, description)
              End If

            End If

          End If


          If isExpDef AndAlso nestLevel = 1 Then
            isExpDefEnter = True 'EXPERIMENT_DEFINITIONの中に入った
          End If
          If isResult AndAlso nestLevel = 2 Then
            isResultEnter = True 'sResultの中に入った



            '@default,0 = *******
            mc = rKeyText.Matches(line)
            If mc.Count >= 1 Then

              Dim key As String = expDef & "\" & mc(0).Groups(1).Value & "\" & mc(0).Groups(2).Value
              description = mc(0).Groups(3).Value.Trim()

              If Not key.Equals("") AndAlso Not check.ContainsKey(key) Then
                check.Add(key, 1) '同じ名前のものは取り込まない
                Me.UpdateTranslateDatabaseForImportModuleManagerCfg(key, description)
              End If


            End If
          End If


          '括弧のネスト判定
          For i As Integer = 0 To line.Length - 1
            If line.Substring(i, 1).Equals("}") Then
              nestLevel -= 1
            End If
          Next

          If isPartEnter AndAlso nestLevel = 0 Then
            'PARTの外にでた
            isPart = False
            isPartEnter = False
          End If

          If isResultEnter AndAlso nestLevel = 1 Then
            'Resultの外にでた
            isResult = False
            isResultEnter = False
          End If

          If isExpDefEnter AndAlso nestLevel = 0 Then
            'EXPERIMENT_DEFINITIONの外にでた
            isExpDef = False
            isExpDefEnter = False
          End If

        End While

        '閉じる
        sr.Close()
      End Using

      '変更件数を返す
      Dim importCount As Integer = 0
      Dim dt As DataTable = Me.TranslationDataTable.GetChanges()
      If Not IsNothing(dt) Then
        importCount = dt.Rows.Count
      End If

      '翻訳DB保存
      Me.SaveDatabase()

      Return importCount
    Catch ex As Exception
      Throw
    End Try

  End Function


  ''' <summary>
  ''' インポート用DB更新
  ''' </summary>
  Private Sub UpdateTranslateDatabaseForImportModuleManagerCfg(idKey As String, description As String)


    Dim selectRow() As DataRow = Me.TranslationDataTable.Select(TranslationDataTable.ColumnNamePartName & "='" & Me.DoubleSiglQrt(idKey) & "'")
    If selectRow.Count > 0 Then
      For i As Integer = 0 To selectRow.Count - 1
        '加工
        If description.Substring(0, 1).Equals("""") _
                        AndAlso description.Substring(description.Length - 1, 1).Equals("""") Then
          description = description.Substring(1, description.Length - 2)
        End If
        If description.Substring(description.Length - 2, 2).Equals("。。") Then
          description = description.Substring(0, description.Length - 1)
        End If

        If Not description.Equals(CStr(selectRow(i)(TranslationDataTable.ColumnNameJapaneseText))) Then
          selectRow(i)(TranslationDataTable.ColumnNameJapaneseText) = description
          selectRow(i)(TranslationDataTable.ColumnNameMemo) = Now.ToString(TranslationDataTable.MemoImportTextFormat)
        End If
      Next
    End If


  End Sub




  ''' <summary>
  ''' 翻訳処理
  ''' </summary>
  Public Function Translate(dirName As String,
                            partName As String,
                            partTitle As String,
                            srcText As String) As String

    If dirName.Equals("") OrElse partName.Equals("") Then
      Return ""
    End If
    If IsNothing(srcText) OrElse srcText.Equals("") Then
      Return ""
    End If



    Dim jpnText As String = ""

    Try

      'フォルダ名
      'パーツ名
      '元テキスト
      '日本語テキスト

      'DBに存在するかチェック
      Dim selectRow() As DataRow = Me.TranslationDataTable.Select(
                                    TranslationDataTable.ColumnNameDirName & "='" & Me.DoubleSiglQrt(dirName) & "'" &
                                    " AND " &
                                    TranslationDataTable.ColumnNamePartName & "='" & Me.DoubleSiglQrt(partName) & "'")
      If selectRow.Count > 0 Then
        'データあり
        jpnText = selectRow(0)(TranslationDataTable.ColumnNameJapaneseText).trim.ToString()

        'パーツタイトル変更されていたら変更する
        If Not partTitle.Equals(CStr(selectRow(0)(TranslationDataTable.ColumnNamePartTitle))) Then
          selectRow(0)(TranslationDataTable.ColumnNamePartTitle) = partTitle
        End If

        '元テキスト変更の場合
        If Not srcText.Equals(CStr(selectRow(0)(TranslationDataTable.ColumnNameOriginalText))) Then
          '翻訳もののテキストと同じデータがあればそれを使用する
          jpnText = Me.SearchSameText(srcText)
          selectRow(0)(TranslationDataTable.ColumnNameOriginalText) = srcText
        ElseIf jpnText.Equals("") Then
          '元テキストが変更されていない場合+翻訳テキストが空欄
          '翻訳もののテキストと同じデータがあればそれを使用する
          jpnText = Me.SearchSameText(srcText)
        End If

        If Not IsNothing(Me.TranslatorApi) AndAlso jpnText.Equals("") Then
          '空欄の場合や元テキストが変更された場合は再翻訳
          jpnText = Me.TranslatorApi.TranslateEnglishToJapanese(srcText)
        End If

        If Not jpnText.Equals(CStr(selectRow(0)(TranslationDataTable.ColumnNameJapaneseText))) Then
          selectRow(0)(TranslationDataTable.ColumnNameJapaneseText) = jpnText
          selectRow(0)(TranslationDataTable.ColumnNameMemo) = Now.ToString(TranslationDataTable.MemoChangeTextFormat)
        End If

      Else
        'データ無し
        jpnText = ""

        '翻訳もののテキストと同じデータがあればそれを使用する
        jpnText = Me.SearchSameText(srcText)

        '自動翻訳
        If jpnText.Equals("") AndAlso Not IsNothing(Me.TranslatorApi) Then
          jpnText = Me.TranslatorApi.TranslateEnglishToJapanese(srcText)
        End If

        Dim newRow As DataRow = Me.TranslationDataTable.NewRow()

        newRow(TranslationDataTable.ColumnNameDirName) = dirName
        newRow(TranslationDataTable.ColumnNamePartTitle) = partTitle
        newRow(TranslationDataTable.ColumnNamePartName) = partName
        newRow(TranslationDataTable.ColumnNameOriginalText) = srcText
        newRow(TranslationDataTable.ColumnNameJapaneseText) = jpnText
        newRow(TranslationDataTable.ColumnNameMemo) = Now.ToString(TranslationDataTable.MemoAddTextFormat)

        Me.TranslationDataTable.Rows.Add(newRow)

      End If

      Return jpnText

    Catch ex As Exception
      Throw
    End Try

  End Function


  ''' <summary>
  ''' 翻訳済みのテキストと同じデータがあるか検索し、あればそのテキストを返す
  ''' </summary>
  ''' <param name="srcText"></param>
  ''' <returns></returns>
  Private Function SearchSameText(srcText As String) As String
    Dim jpnText As String = ""

    Dim selectRow() As DataRow = Me.TranslationDataTable.Select(TranslationDataTable.ColumnNameOriginalText & "='" & Me.DoubleSiglQrt(srcText) & "'")
    If selectRow.Count > 0 Then
      For i As Integer = 0 To selectRow.Count - 1
        jpnText = selectRow(0)(TranslationDataTable.ColumnNameJapaneseText).trim.ToString()
        If Not jpnText.Equals("") Then
          Exit For
        End If
      Next
    End If

    Return jpnText
  End Function

  ''' <summary>
  ''' シングルコーテーションを２つにする。
  ''' </summary>
  ''' <param name="text"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function DoubleSiglQrt(ByVal text As String) As String
    Dim returnValue As String = text
    returnValue = returnValue.Replace("'", "''")
    Return returnValue
  End Function



End Class
