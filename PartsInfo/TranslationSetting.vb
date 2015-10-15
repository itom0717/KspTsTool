''' <summary>
''' 設定情報クラス
''' </summary>
Public Class TranslationSetting

  ''' <summary>
  ''' 元フォルダ
  ''' </summary>
  ''' <returns></returns>
  Public Property SrcPath As String = ""

  ''' <summary>
  ''' 保存先フォルダ
  ''' </summary>
  ''' <returns></returns>
  Public Property DsgtPath As String = ""


  ''' <summary>
  '''自動翻訳を行うか
  ''' </summary>
  Public IsTranslation As Boolean = False


  ''' <summary>
  ''' クライアントID
  ''' </summary>
  ''' <returns></returns>
  Public Property MicrosoftTranslatorAPIClientId As String = ""

  ''' <summary>
  ''' 顧客の秘密
  ''' </summary>
  ''' <returns></returns>
  Public Property MicrosoftTranslatorAPIClientSecret As String = ""



End Class
