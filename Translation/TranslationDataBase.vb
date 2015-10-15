''' <summary>
''' 翻訳データベース処理
''' </summary>
Public Class TranslationDataBase


  ''' <summary>
  ''' 翻訳DataTable
  ''' </summary>
  Private TranslationDataTable As TranslationDataTable = Nothing

  ''' <summary>
  ''' ファイル名
  ''' </summary>
  Private Filename As String = ""

  ''' <summary>
  ''' 自動翻訳処理
  ''' </summary>
  Private TranslatorApi As TranslatorApi = Nothing

  ''' <summary>
  '''　データが変更されたか？
  ''' </summary>
  Private IsChange As Boolean = False

  ''' <summary>
  ''' NEW
  ''' </summary>
  Public Sub New()
    Me.New("", "")
  End Sub

  ''' <summary>
  ''' NEW
  ''' </summary>
  Public Sub New(clientId As String, clientSecret As String)

    ' ファイル名
    Me.Filename = Common.File.AddDirectorySeparator(My.Application.Info.DirectoryPath) & "TranslationDataBase.tsv"

    '翻訳DB
    Me.TranslationDataTable = New TranslationDataTable

    '自動翻訳
    If Not clientId.Equals("") AndAlso Not clientSecret.Equals("") Then
      Me.TranslatorApi = New TranslatorApi(clientId, clientSecret)
    Else
      Me.TranslatorApi = Nothing
    End If
  End Sub

  ''' <summary>
  ''' Finalize
  ''' </summary>
  Protected Overrides Sub Finalize()
    Me.TranslationDataTable = Nothing
    Me.TranslatorApi = Nothing
    MyBase.Finalize()
  End Sub

  ''' <summary>
  ''' DB読込
  ''' </summary>
  Public Sub LoadDatabse()

    If Not Common.File.ExistsFile(Me.Filename) Then
      Return
    End If


    Using tfp As New FileIO.TextFieldParser(Me.Filename, System.Text.Encoding.GetEncoding("UTF-16"))

      'フィールドが文字で区切られているとする
      'デフォルトでDelimitedなので、必要なし
      tfp.TextFieldType = FileIO.FieldType.Delimited

      '区切り文字を指定
      tfp.Delimiters = New String() {vbTab}

      'フィールドを"で囲み、改行文字、区切り文字を含めることができるか
      tfp.HasFieldsEnclosedInQuotes = False

      'フィールドの前後からスペースを削除する
      tfp.TrimWhiteSpace = True


      If Not tfp.EndOfData Then
        '先頭行は捨てる
        tfp.ReadFields()
      End If
      While Not tfp.EndOfData
        'フィールドを読み込む
        Dim fields As String() = tfp.ReadFields()
        Dim newRow As DataRow = Me.TranslationDataTable.NewRow()

        For i As Integer = 0 To Me.TranslationDataTable.Columns.Count - 1
          newRow(i) = fields(i)
        Next

        Me.TranslationDataTable.Rows.Add(newRow)
      End While

      tfp.Close()
    End Using

    Me.IsChange = False

  End Sub

  ''' <summary>
  ''' 翻訳ファイル保存
  ''' </summary>
  Public Sub SaveDatabase()

    If Not Me.IsChange Then
      Return
    End If

    Try

      Dim tmpFilename As String = Me.Filename & ".Tmp"
      Dim bakFilename As String = Me.Filename & ".Bak"
      If Common.File.ExistsFile(tmpFilename) Then
        Common.File.DeleteFile(tmpFilename)
      End If
      Using sw As New System.IO.StreamWriter(tmpFilename, False, System.Text.Encoding.GetEncoding("UTF-16"))

        Dim columnLine As String = ""
        For Each column As DataColumn In Me.TranslationDataTable.Columns
          If Not columnLine.Equals("") Then
            columnLine &= vbTab
          End If
          columnLine &= column.ColumnName
        Next
        sw.WriteLine(columnLine)

        For Each row As DataRow In Me.TranslationDataTable.Select(Nothing, "フォルダ名, パーツ名")
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

      If Common.File.ExistsFile(Me.Filename) Then
        If Common.File.ExistsFile(bakFilename) Then
          Common.File.DeleteFile(bakFilename)
        End If
        Common.File.MoveFile(Me.Filename, bakFilename)
      End If

      Common.File.MoveFile(tmpFilename, Me.Filename)

    Catch ex As Exception
      Throw
    End Try

  End Sub

  ''' <summary>
  ''' 翻訳ファイル読込
  ''' </summary>
  ''' <param name="filename"></param>
  Public Function ImportTranslationFile(filename As String) As Integer

    Dim rPart As New System.Text.RegularExpressions.Regex(
          "^@PART\[(.+)\]$",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

    Dim rDescription As New System.Text.RegularExpressions.Regex(
            "^{@description\s*=\s*([^}]*)}$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

    Dim mc As System.Text.RegularExpressions.MatchCollection


    Dim importCount As Integer = 0

    Try
      Me.IsChange = False

      Dim check As New Hashtable

      '翻訳DB読込
      Me.LoadDatabse()

      Using sr As New System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-16"))

        Dim part As String = ""
        Dim description As String = ""

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
            '空行は次へ
            Continue While
          End If

          '@PART[***]
          mc = rPart.Matches(line)
          If mc.Count >= 1 Then
            part = mc(0).Groups(1).Value
            Continue While '次へ
          End If

          '@description
          If Not part.Equals("") AndAlso Not check.ContainsKey(part) Then
            check.Add(part, 1) '同じ名前のものは取り込まない
            mc = rDescription.Matches(line)
            If mc.Count >= 1 Then
              description = mc(0).Groups(1).Value.Trim()
              If Not description.Equals("") Then
                Dim selectRow() As DataRow = Me.TranslationDataTable.Select("パーツ名='" & Me.DoubleSiglQrt(part) & "'")
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

                    If Not description.Equals(CStr(selectRow(i)("日本語テキスト"))) Then

                      selectRow(i)("日本語テキスト") = description
                      selectRow(i)("備考") = Now.ToString("yyyy/MM/dd HH:mm:ss 取込")

                      Me.IsChange = True
                      importCount += 1
                    End If
                  Next
                End If
              End If

              part = ""
            End If
          End If

        End While

        '閉じる
        sr.Close()
      End Using

      If Me.IsChange Then
        '翻訳DB保存
        Me.SaveDatabase()
      End If

    Catch ex As Exception
      Throw
    End Try

    Return importCount

  End Function


  ''' <summary>
  ''' 翻訳処理
  ''' </summary>
  Public Function Translate(dirName As String,
                            partName As String,
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
      Dim selectRow() As DataRow = Me.TranslationDataTable.Select("フォルダ名 = '" & Me.DoubleSiglQrt(dirName) & "' AND パーツ名='" & Me.DoubleSiglQrt(partName) & "'")
      If selectRow.Count > 0 Then
        'データあり
        jpnText = selectRow(0)("日本語テキスト").trim.ToString()

        '元テキスト変更の場合
        If Not srcText.Equals(CStr(selectRow(0)("元テキスト"))) Then
          '翻訳もののテキストと同じデータがあればそれを使用する
          jpnText = Me.SearchSameText(srcText)
          selectRow(0)("元テキスト") = srcText
          Me.IsChange = True
        ElseIf jpnText.Equals("") Then
          '元テキストが変更されていない場合+翻訳テキストが空欄
          '翻訳もののテキストと同じデータがあればそれを使用する
          jpnText = Me.SearchSameText(srcText)
        End If

        If Not IsNothing(Me.TranslatorApi) AndAlso jpnText.Equals("") Then
          '空欄の場合や元テキストが変更された場合は再翻訳
          jpnText = Me.TranslatorApi.TranslateEnglishToJapanese(srcText)
        End If

        If Not jpnText.Equals(CStr(selectRow(0)("日本語テキスト"))) Then
          selectRow(0)("日本語テキスト") = jpnText
          selectRow(0)("備考") = Now.ToString("yyyy/MM/dd HH:mm:ss 変更")
          Me.IsChange = True
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

        newRow("フォルダ名") = dirName
        newRow("パーツ名") = partName
        newRow("元テキスト") = srcText
        newRow("日本語テキスト") = jpnText
        newRow("備考") = Now.ToString("yyyy/MM/dd HH:mm:ss 追加")

        Me.TranslationDataTable.Rows.Add(newRow)
        Me.IsChange = True

      End If

      Return jpnText

    Catch ex As Exception
      Throw
    End Try

  End Function


  ''' <summary>
  ''' 翻訳もののテキストと同じデータがあるか検索
  ''' </summary>
  ''' <param name="srcText"></param>
  ''' <returns></returns>
  Private Function SearchSameText(srcText As String) As String
    Dim jpnText As String = ""

    Dim selectRow() As DataRow = Me.TranslationDataTable.Select("元テキスト='" & Me.DoubleSiglQrt(srcText) & "'")
    If selectRow.Count > 0 Then
      For i As Integer = 0 To selectRow.Count - 1
        jpnText = selectRow(0)("日本語テキスト").trim.ToString()
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
  Public Function DoubleSiglQrt(ByVal text As String) As String
    Return text.Replace("'", "''")
  End Function

End Class
