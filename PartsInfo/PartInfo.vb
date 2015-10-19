''' <summary>
''' cfgからデータを取得するして、パーツ情報取得
''' </summary>
Public Class PartInfo

  ''' <summary>
  ''' パーツデータか判定
  ''' </summary>
  ''' <returns>
  ''' パーツデータの場合 Falseを返す
  ''' </returns>
  Public ReadOnly Property NoPartData As Boolean
    Get
      Return (IsNothing(Me.Name) OrElse Me.Name.Equals(""))
    End Get
  End Property

  ''' <summary>
  ''' Name
  ''' </summary>
  ''' <returns></returns>
  Public Property Name As String = Nothing

  ''' <summary>
  ''' Title
  ''' </summary>
  ''' <returns></returns>
  Public Property Title As String = Nothing

  ''' <summary>
  ''' Description
  ''' </summary>
  ''' <returns></returns>
  Public Property Description As String = Nothing

  ''' <summary>
  ''' Description Japanese
  ''' </summary>
  ''' <returns></returns>
  Public Property DescriptionJapanese As String = Nothing


  ''' <summary>
  ''' インスタンスを生成
  ''' </summary>
  ''' <param name="filename"></param>
  Public Sub New(filename As String)
    Me.GetPartInfo(filename)
  End Sub


  ''' <summary>
  ''' part cfgファイルから各値を取得する
  ''' </summary>
  ''' <param name="partCfgFilename">読み込むパーツのcfgファイル</param>
  Private Sub GetPartInfo(partCfgFilename As String)
    Try

      Dim rPart As New System.Text.RegularExpressions.Regex(
          "^PART($|{|\s)",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rName As New System.Text.RegularExpressions.Regex(
            "^Name\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rTitle As New System.Text.RegularExpressions.Regex(
            "^title\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rDescription As New System.Text.RegularExpressions.Regex(
            "^description\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)


      Dim mc As System.Text.RegularExpressions.MatchCollection


      Me.Name = Nothing
      Me.Title = Nothing
      Me.Description = Nothing
      Me.DescriptionJapanese = Nothing

      '各cfgファイルの解析
      Using sr As New System.IO.StreamReader(partCfgFilename, System.Text.Encoding.UTF8)

        Dim nestLevel As Integer = 0
        Dim isPart As Boolean = False 'Partの中
        Dim isPartEnter As Boolean = False 'partの中に入った

        While sr.Peek() > -1
          Dim line As String = sr.ReadLine()

          '空白を取り除く
          line = line.Trim()

          'コメント削除
          Dim ptr1 = line.IndexOf("//", 0)
          If ptr1 >= 0 Then
            line = line.Substring(0, ptr1).Trim()
          End If


          'Parts判定
          If Not isPart AndAlso nestLevel = 0 AndAlso rPart.IsMatch(line) Then
            isPart = True
            isPartEnter = False
          End If


          '括弧のネスト判定
          For i As Integer = 0 To line.Length - 1
            If line.Substring(i, 1).Equals("{") Then
              nestLevel += 1
            End If
          Next


          'Name/Title/descriptionの取得
          If isPart AndAlso nestLevel = 1 Then
            isPartEnter = True 'partの中に入った

            If IsNothing(Me.Name) Then '最初の１回のみ
              mc = rName.Matches(line)
              If mc.Count >= 1 Then
                Me.Name = mc(0).Groups(1).Value
              End If
            End If

            If IsNothing(Me.Title) Then '最初の１回のみ
              mc = rTitle.Matches(line)
              If mc.Count >= 1 Then
                Me.Title = mc(0).Groups(1).Value
              End If
            End If

            If IsNothing(Me.Description) Then '最初の１回のみ
              mc = rDescription.Matches(line)
              If mc.Count >= 1 Then
                Me.Description = mc(0).Groups(1).Value
              End If
            End If

            If Not IsNothing(Me.Name) _
              AndAlso Not IsNothing(Me.Title) _
              AndAlso Not IsNothing(Me.Description) Then
              '全部取得したら抜ける
              Exit While
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
            Exit While
          End If


        End While

        '閉じる
        sr.Close()
      End Using

    Catch ex As Exception
      Throw
    End Try


  End Sub



End Class
