''' <summary>
''' cfgからデータを取得するして、ScienceDefs.cfgの情報取得
''' </summary>
Public Class ScienceDefsInfo

  ''' <summary>
  ''' ScienceDefs.cfgか判定
  ''' </summary>
  ''' <returns>
  ''' ScienceDefs.cfgの場合 Falseを返す
  ''' </returns>
  Public ReadOnly Property NoExpDefData As Boolean
    Get
      Return (Me.ExpDefDataList.Count = 0)
    End Get
  End Property


  ''' <summary>
  ''' Result用クラス
  ''' </summary>
  Public Class ResultData

    ''' <summary>
    ''' KeyText
    ''' </summary>
    Public KeyText As String = ""

    ''' <summary>
    ''' 同じキーの場合のインデックス
    ''' </summary>
    ''' <remarks>
    ''' 0から開始
    ''' </remarks>
    Public KeyIndex As Integer = 0


    ''' <summary>
    ''' Description Original
    ''' </summary>
    ''' <returns></returns>
    Public Property MessageOriginal As String = Nothing

    ''' <summary>
    ''' Description Japanese
    ''' </summary>
    ''' <returns></returns>
    Public Property MessageJapanese As String = Nothing

  End Class

  ''' <summary>
  ''' EXPERIMENT_DEFINITION用クラス
  ''' </summary>
  Public Class ExpDefData
    ''' <summary>
    ''' ID
    ''' </summary>
    ''' <returns></returns>
    Public Property ID As String = Nothing

    ''' <summary>
    ''' Title
    ''' </summary>
    ''' <returns></returns>
    Public Property Title As String = Nothing

    ''' <summary>
    ''' Description
    ''' </summary>
    ''' <returns></returns>
    Public Property Result As New List(Of ResultData)

  End Class

  ''' <summary>
  ''' EXPERIMENT_DEFINITIONデータ
  ''' </summary>
  Public ExpDefDataList As New List(Of ExpDefData)


  ''' <summary>
  ''' インスタンスを生成
  ''' </summary>
  ''' <param name="filename"></param>
  Public Sub New(filename As String)
    Me.GetPartInfo(filename)
  End Sub


  ''' <summary>
  ''' ScienceDefs.cfgファイルから各値を取得する
  ''' </summary>
  ''' <param name="scienceDefsFilename">読み込むScienceDefs.cfgファイル</param>
  Private Sub GetPartInfo(scienceDefsFilename As String)
    Try

      Dim rExpDef As New System.Text.RegularExpressions.Regex(
          "^EXPERIMENT_DEFINITION($|{|\s)",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rResult As New System.Text.RegularExpressions.Regex(
          "^RESULTS($|{|\s)",
          System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rName As New System.Text.RegularExpressions.Regex(
            "^id\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rTitle As New System.Text.RegularExpressions.Regex(
            "^title\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)

      Dim rKey As New System.Text.RegularExpressions.Regex(
            "^([^=]+)\s*=\s*(.+)$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)


      Dim mc As System.Text.RegularExpressions.MatchCollection


      Me.ExpDefDataList.Clear()

      '各cfgファイルの解析
      Using sr As New System.IO.StreamReader(scienceDefsFilename, System.Text.Encoding.UTF8)

        Dim nestLevel As Integer = 0
        Dim isExpDef As Boolean = False 'EXPERIMENT_DEFINITIONの中
        Dim isExpDefEnter As Boolean = False 'EXPERIMENT_DEFINITIONの中に入った
        Dim isResult As Boolean = False 'Resultの中
        Dim isResultEnter As Boolean = False 'Resultの中に入った

        Dim expDefData As ExpDefData = Nothing

        While sr.Peek() > -1
          Dim line As String = sr.ReadLine()

          '空白を取り除く
          line = line.Trim()

          'コメント削除
          Dim ptr1 = line.IndexOf("//", 0)
          If ptr1 >= 0 Then
            line = line.Substring(0, ptr1).Trim()
          End If


          'EXPERIMENT_DEFINITION判定
          If Not isExpDef AndAlso nestLevel = 0 AndAlso rExpDef.IsMatch(line) Then
            isExpDef = True
            isExpDefEnter = False
            isResult = False
            isResultEnter = False

            expDefData = New ExpDefData
            expDefData.Result.Clear()
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


          'Name/Titleの取得
          If isExpDef AndAlso nestLevel = 1 Then
            isExpDefEnter = True 'EXPERIMENT_DEFINITIONの中に入った

            If IsNothing(expDefData.ID) Then '最初の１回のみ
              mc = rName.Matches(line)
              If mc.Count >= 1 Then
                expDefData.ID = mc(0).Groups(1).Value
              End If
            End If

            If IsNothing(expDefData.Title) Then '最初の１回のみ
              mc = rTitle.Matches(line)
              If mc.Count >= 1 Then
                expDefData.Title = mc(0).Groups(1).Value
              End If
            End If

          End If


          If isResult AndAlso nestLevel = 2 Then
            isResultEnter = True 'sResultの中に入った

            mc = rKey.Matches(line)
            If mc.Count >= 1 Then
              Dim resultData As New ResultData
              resultData.KeyText = mc(0).Groups(1).Value.Trim()
              resultData.MessageOriginal = mc(0).Groups(2).Value.Trim()

              If Not IsNothing(expDefData) Then
                expDefData.Result.Add(resultData)
              End If
            End If


          End If


          '括弧のネスト判定
          For i As Integer = 0 To line.Length - 1
            If line.Substring(i, 1).Equals("}") Then
              nestLevel -= 1
            End If
          Next

          If isResultEnter AndAlso nestLevel = 1 Then
            'Resultの外にでた
            isResult = False
            isResultEnter = False
          End If

          If isExpDefEnter AndAlso nestLevel = 0 Then
            'EXPERIMENT_DEFINITIONの外にでた
            isExpDef = False
            isExpDefEnter = False

            If Not IsNothing(expDefData) AndAlso expDefData.Result.Count >= 1 Then
              Me.CheckIndex(expDefData)
              Me.ExpDefDataList.Add(expDefData)
            End If
            expDefData = Nothing
          End If


        End While

        '閉じる
        sr.Close()
      End Using

    Catch ex As Exception
      Throw
    End Try


  End Sub

  ''' <summary>
  ''' KeyIndexを取得しておく
  ''' </summary>
  ''' <param name="expDefData"></param>
  Private Sub CheckIndex(expDefData As ExpDefData)
    For i As Integer = 0 To expDefData.Result.Count - 1
      If expDefData.Result(i).KeyIndex = 0 Then
        Dim newIndex As Integer = 1
        For j As Integer = i + 1 To expDefData.Result.Count - 1
          If expDefData.Result(i).KeyText.Equals(expDefData.Result(j).KeyText, StringComparison.CurrentCultureIgnoreCase) Then
            expDefData.Result(j).KeyIndex = newIndex
            newIndex += 1
          End If
        Next
      End If
    Next
  End Sub

End Class
