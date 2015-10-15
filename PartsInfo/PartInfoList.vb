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
  ''' 保存処理
  ''' </summary>
  Public Sub Save()

    'ファイルを上書きし、書き込む 
    Using sw As New System.IO.StreamWriter(Me.TranslationFilename, False, System.Text.Encoding.GetEncoding("UTF-16"))
      For Each partInfo As PartInfo In Me

        If Not IsNothing(partInfo.Name) AndAlso Not partInfo.Name.Equals("") Then
          Dim commentOutFlag As Boolean = False
          If IsNothing(partInfo.DescriptionJapanese) _
            OrElse partInfo.DescriptionJapanese.Equals("") _
            OrElse partInfo.DescriptionJapanese.Equals((partInfo.Description)) Then
            commentOutFlag = True
          End If

          sw.WriteLine(CStr(IIf(commentOutFlag, "//", "")) & "@PART[" & partInfo.Name & "]")
          sw.WriteLine("//Title = " & CStr(IIf(Not IsNothing(partInfo.Title), partInfo.Title, "")))

          If commentOutFlag Then
            sw.WriteLine("//{@description = " & CStr(IIf(Not IsNothing(partInfo.Description), partInfo.Description, "")) & "}")
          Else
            sw.WriteLine("{@description = " & CStr(IIf(Not IsNothing(partInfo.DescriptionJapanese), partInfo.DescriptionJapanese, "")) & "}")
          End If

          sw.WriteLine("")
        End If
      Next
      sw.Close()
    End Using

  End Sub


End Class
