''' <summary>
''' 翻訳データベース用DataTable
''' </summary>
Public Class TranslationDataTable
  Inherits DataTable


  ''' <summary>
  ''' フォルダ名
  ''' </summary>
  Public Const ColumnNameDirName As String = "フォルダ名"

  ''' <summary>
  ''' パーツ名
  ''' </summary>
  Public Const ColumnNamePartName As String = "パーツ名"

  ''' <summary>
  ''' パーツタイトル
  ''' </summary>
  Public Const ColumnNamePartTitle As String = "パーツタイトル"

  ''' <summary>
  ''' 元テキスト
  ''' </summary>
  Public Const ColumnNameOriginalText As String = "元テキスト"

  ''' <summary>
  ''' 日本語テキスト
  ''' </summary>
  Public Const ColumnNameJapaneseText As String = "日本語テキスト"

  ''' <summary>
  ''' 備考
  ''' </summary>
  Public Const ColumnNameMemo As String = "備考"


  ''' <summary>
  ''' 追加時の備考
  ''' </summary>
  Public Const MemoAddTextFormat As String = "yyyy/MM/dd HH:mm:ss 追加"

  ''' <summary>
  ''' 変更時の備考
  ''' </summary>
  Public Const MemoChangeTextFormat As String = "yyyy/MM/dd HH:mm:ss 変更"

  ''' <summary>
  ''' 取込時の備考
  ''' </summary>
  Public Const MemoImportTextFormat As String = "yyyy/MM/dd HH:mm:ss 取込"




  ''' <summary>
  ''' New
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub New()

    ' フォルダ名
    With Me.Columns.Add(ColumnNameDirName, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    ' パーツ名
    With Me.Columns.Add(ColumnNamePartName, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    ' パーツタイトル
    With Me.Columns.Add(ColumnNamePartTitle, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    ' 元テキスト
    With Me.Columns.Add(ColumnNameOriginalText, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    ' 日本語テキスト
    With Me.Columns.Add(ColumnNameJapaneseText, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With

    ' 備考
    With Me.Columns.Add(ColumnNameMemo, Type.GetType("System.String"))
      .DefaultValue = ""
      .AllowDBNull = False
    End With
  End Sub


End Class
