Imports System.Web
Imports System.Runtime.Serialization.Json

''' <summary>
''' Microsoft Translator APIを使用した自動翻訳でAccessTokenを取得
''' </summary>
''' <remarks>
''' 参考
''' https://msdn.microsoft.com/en-us/library/ff512421
''' C#からVB.NETへ移植してます。
''' </remarks>
Public Class AdmAuthentication

  ''' <summary>
  ''' AccessUri
  ''' </summary>
  Public Const DatamarketAccessUri As String = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13"

  ''' <summary>
  ''' クライアントID
  ''' </summary>
  Private ClientId As String

  ''' <summary>
  ''' 顧客の秘密
  ''' </summary>
  Private CientSecret As String

  ''' <summary>
  ''' RequestURI
  ''' </summary>
  Private Request As String

  ''' <summary>
  ''' インスタンスを生成
  ''' </summary>
  ''' <param name="clientId"></param>
  ''' <param name="clientSecret"></param>
  Public Sub New(clientId As String, clientSecret As String)
    Me.ClientId = clientId
    Me.CientSecret = clientSecret
    Me.Request = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                                HttpUtility.UrlEncode(clientId), HttpUtility.UrlEncode(clientSecret))
  End Sub

  ''' <summary>
  ''' GetAccessTokenを取得
  ''' </summary>
  ''' <returns></returns>
  Public Function GetAccessToken() As AdmAccessToken
    Return Me.HttpPost(DatamarketAccessUri, Me.Request)
  End Function

  ''' <summary>
  ''' HttpPostでAccessTokenを取得する
  ''' </summary>
  ''' <param name="datamarketAccessUri"></param>
  ''' <param name="requestDetails"></param>
  ''' <returns></returns>
  Private Function HttpPost(datamarketAccessUri As String, requestDetails As String) As AdmAccessToken
    Dim webRequest As System.Net.WebRequest = System.Net.WebRequest.Create(datamarketAccessUri)
    webRequest.ContentType = "application/x-www-form-urlencoded"
    webRequest.Method = "POST"

    Dim bytes() As Byte = System.Text.Encoding.ASCII.GetBytes(requestDetails)
    webRequest.ContentLength = bytes.Length

    Using outputStream As IO.Stream = webRequest.GetRequestStream()
      outputStream.Write(bytes, 0, bytes.Length)
    End Using

    Using webResponse As Net.WebResponse = webRequest.GetResponse()

      Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(AdmAccessToken))

      'Get deserialized object from JSON stream
      Dim token As AdmAccessToken = CType(serializer.ReadObject(webResponse.GetResponseStream()), AdmAccessToken)

      Return token
    End Using

  End Function

End Class

