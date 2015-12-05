''' <summary>
''' パーツ情報List
''' </summary>
Public Class PartInfoList
  Inherits List(Of PartInfo)

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

      '部品数分ループ
      For Each partInfo As PartInfo In Me
        For Each partInfoData As PartInfo.PartInfoData In partInfo.PartInfoDataList

          'Nameが無いものは処理しない
          If Not IsNothing(partInfoData.Name) AndAlso Not partInfoData.Name.Equals("") Then

            Dim outputFlag As Boolean = True
            If IsNothing(partInfoData.DescriptionJapanese) _
                OrElse partInfoData.DescriptionJapanese.Equals("") _
                OrElse partInfoData.DescriptionJapanese.Equals((partInfoData.Description)) Then
              '日本語が未入力、元テキストと同じ場合は出力しない
              outputFlag = False
            End If

            'スペースが含まれている場合は、？に変換
            Dim partName As String = partInfoData.Name
            partName = partName.Replace(" ", "?")

            sw.WriteLine(String.Format("@PART[{0}]", partName))
            sw.WriteLine("{")
            sw.WriteLine("//Title")
            sw.WriteLine("//  " & CStr(IIf(Not IsNothing(partInfoData.Title), partInfoData.Title, partInfoData.Name)))
            sw.WriteLine("//Original Text")
            sw.WriteLine("//  " & String.Format("@description = {0}", CStr(IIf(Not IsNothing(partInfoData.Description), partInfoData.Description, ""))))
            If partInfoData.Memo.Equals("") Then
              sw.WriteLine("//Japanese Text")
            Else
              sw.WriteLine("//Japanese Text ---- " & partInfoData.Memo)
            End If
            If Not outputFlag Then
              sw.Write("//  ")
            Else
              sw.Write("    ")
            End If
            sw.WriteLine(String.Format("@description = {0}", CStr(IIf(Not IsNothing(partInfoData.DescriptionJapanese), partInfoData.DescriptionJapanese, ""))))
            sw.WriteLine("}")
            sw.WriteLine("")
          End If

        Next
      Next
      sw.Close()
    End Using

  End Sub


End Class
