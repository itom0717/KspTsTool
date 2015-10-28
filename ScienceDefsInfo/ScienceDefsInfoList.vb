''' <summary>
'''  ScienceDefsList
''' </summary>
Public Class ScienceDefsInfoList
  Inherits List(Of ScienceDefsInfo)

  ''' <summary>
  '''  Module Manager用cfgファイル名
  ''' </summary>
  Private Property ModuleManagerConfigFilename As String = ""

  ''' <summary>
  ''' インスタンスを生成
  ''' </summary>
  ''' <param name="moduleManagerConfigFilename"></param>
  Public Sub New(moduleManagerConfigFilename As String)
    Me.ModuleManagerConfigFilename = moduleManagerConfigFilename
  End Sub

  ''' <summary>
  ''' Module Manager用cfgファイルの作成
  ''' </summary>
  ''' <remarks>
  ''' 参考
  ''' https://github.com/sarbian/ModuleManager/wiki/Module-Manager-Syntax
  ''' </remarks>
  Public Sub Save()

    Using sw As New System.IO.StreamWriter(Me.ModuleManagerConfigFilename, False, System.Text.Encoding.UTF8)

      'ファイル数分ループ
      For Each scienceDefsInfo As ScienceDefsInfo In Me

        For Each expDefData As ScienceDefsInfo.ExpDefData In scienceDefsInfo.ExpDefDataList
          'IDが無いものは処理しない
          If Not IsNothing(expDefData.ID) AndAlso Not expDefData.ID.Equals("") Then

            sw.WriteLine(String.Format("@EXPERIMENT_DEFINITION:HAS[#id[{0}]]", expDefData.ID))
            sw.WriteLine("{")
            sw.WriteLine("  //Title")
            sw.WriteLine("  //  " & CStr(IIf(Not IsNothing(expDefData.Title), expDefData.Title, expDefData.ID)))
            sw.WriteLine("  @RESULTS")
            sw.WriteLine("  {")
            For Each resultData As ScienceDefsInfo.ResultData In expDefData.Result

              Dim outputFlag As Boolean = True
              If IsNothing(resultData.MessageJapanese) _
                    OrElse resultData.MessageJapanese.Equals("") _
                    OrElse resultData.MessageJapanese.Equals((resultData.MessageOriginal)) Then
                '日本語が未入力、元テキストと同じ場合は出力しない
                outputFlag = False
              End If

              sw.WriteLine("    //Original Text")
              sw.WriteLine("    //  " & String.Format("@{0},{1} = {2}", resultData.KeyText, resultData.KeyIndex, CStr(IIf(Not IsNothing(resultData.MessageOriginal), resultData.MessageOriginal, ""))))
              If resultData.Memo.Equals("") Then
                sw.WriteLine("    //Japanese Text")
              Else
                sw.WriteLine("    //Japanese Text ---- " & resultData.Memo)
              End If
              If Not outputFlag Then
                sw.Write("    //  ")
              Else
                sw.Write("        ")
              End If
              sw.WriteLine(String.Format("@{0},{1} = {2}", resultData.KeyText, resultData.KeyIndex, CStr(IIf(Not IsNothing(resultData.MessageJapanese), resultData.MessageJapanese, ""))))
              sw.WriteLine("")

            Next
            sw.WriteLine("  }")

            sw.WriteLine("}")
            sw.WriteLine("")

          End If
        Next


      Next
      sw.Close()
    End Using



  End Sub


End Class
