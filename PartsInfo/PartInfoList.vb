''' <summary>
''' パーツ情報List
''' </summary>
Public Class PartInfoList
  Inherits List(Of PartInfo)

  ''' <summary>
  ''' 保存ファイル名
  ''' </summary>
  Private Property TranslationFilename As String = ""

  ''' <summary>
  ''' New
  ''' </summary>
  ''' <param name="translationFilename"></param>
  Public Sub New(translationFilename As String)
    Me.TranslationFilename = translationFilename
  End Sub

  ''' <summary>
  ''' Module Manager用cfgファイルの作成
  ''' </summary>
  ''' <remarks>
  ''' 参考
  ''' https://github.com/sarbian/ModuleManager/wiki/Module-Manager-Syntax
  ''' </remarks>
  Public Sub Save()

    Using sw As New System.IO.StreamWriter(Me.TranslationFilename, False, System.Text.Encoding.GetEncoding("UTF-16"))
      For Each partInfo As PartInfo In Me

        If Not IsNothing(partInfo.Name) AndAlso Not partInfo.Name.Equals("") Then

          Dim outputFlag As Boolean = True
          If IsNothing(partInfo.DescriptionJapanese) _
            OrElse partInfo.DescriptionJapanese.Equals("") _
            OrElse partInfo.DescriptionJapanese.Equals((partInfo.Description)) Then
            '日本語が未入力、元テキストと同じ場合は出力しない
            outputFlag = False
          End If

          sw.WriteLine("//" & CStr(IIf(Not IsNothing(partInfo.Title), partInfo.Title, partInfo.Name)))
          If outputFlag Then
            sw.WriteLine(String.Format("@PART[{0}]", partInfo.Name))
            sw.WriteLine("{")
            sw.WriteLine("  //japasese text")
            sw.WriteLine(String.Format("  @description = {0}", CStr(IIf(Not IsNothing(partInfo.DescriptionJapanese), partInfo.DescriptionJapanese, ""))))
            sw.WriteLine("}")
          Else
            sw.WriteLine(String.Format("//@PART[{0}]", partInfo.Name))
            sw.WriteLine("//{")
            sw.WriteLine("//  original text")
            sw.WriteLine(String.Format("//  @description = {0}", CStr(IIf(Not IsNothing(partInfo.Description), partInfo.Description, ""))))
            sw.WriteLine("//}")
          End If
          sw.WriteLine("")
        End If
      Next
      sw.Close()
    End Using

  End Sub


End Class
