''' <summary>
''' 翻訳データベース用DataTable
''' </summary>
Public Class TranslationDataTable
  Inherits DataTable

  ''' <summary>
  ''' New
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub New()

    With Me.Columns.Add("フォルダ名", Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    With Me.Columns.Add("パーツ名", Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    With Me.Columns.Add("元テキスト", Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    With Me.Columns.Add("日本語テキスト", Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    With Me.Columns.Add("備考", Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With
  End Sub


End Class
