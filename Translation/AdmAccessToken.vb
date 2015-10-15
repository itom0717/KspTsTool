''' <summary>
''' AdmAccessToken
''' </summary>
''' <remarks>
''' 参考
''' https://msdn.microsoft.com/en-us/library/hh454950.aspx
''' </remarks>
Public Class AdmAccessToken

  ''' <summary>
  ''' 、Microsoftの翻訳APIへのアクセスの認証に使用できるアクセストークン
  ''' </summary>
  ''' <returns></returns>
  Public Property access_token As String

  ''' <summary>
  ''' アクセストークンの形式
  ''' </summary>
  ''' <returns></returns>
  Public Property token_type As String

  ''' <summary>
  ''' アクセス・トークンの有効期限(秒数)
  ''' </summary>
  ''' <returns></returns>
  Public Property expires_in As String

  ''' <summary>
  ''' このトークンが有効であるドメイン。マイクロソフトの翻訳APIの場合、ドメインである
  ''' </summary>
  ''' <returns></returns>
  Public Property scope As String

End Class
