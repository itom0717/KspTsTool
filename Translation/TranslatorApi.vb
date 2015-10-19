''' <summary>
''' Microsoft Translator APIを使用した自動翻訳
''' </summary>
''' <remarks>
''' 参考
''' https://msdn.microsoft.com/en-us/library/ff512421
''' C#からVB.NETへ移植してます。
''' </remarks>
Public Class TranslatorApi

  ''' <summary>
  ''' AdmAccessToken
  ''' </summary>
  Private AdmToken As AdmAccessToken = Nothing

  ''' <summary>
  ''' AdmAuthentication
  ''' </summary>
  Private AdmAuth As AdmAuthentication = Nothing

  ''' <summary>
  ''' アクセストークン取得からの時間計測用
  ''' </summary>
  Private Stopwatch As New System.Diagnostics.Stopwatch()

  ''' <summary>
  ''' アクセストークンの有効期限(秒）
  ''' </summary>
  Private AccessTokenExpiresIn As Integer = 0


  ''' <summary>
  ''' New
  ''' </summary>
  Public Sub New(clientId As String, clientSecret As String)

    'Get Client Id And Client Secret from https//datamarket.azure.com/developer/applications/
    'Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 
    ' clientIDには、事前にマイクロソフトへ登録した「クライアントID」を設定。
    ' client secretには、事前にマイクロソフトへ登録した「顧客の秘密」を設定。
    Me.AdmAuth = New AdmAuthentication(clientId, clientSecret)

  End Sub

  ''' <summary>
  ''' Finalize
  ''' </summary>
  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  ''' <summary>
  ''' 英語から日本語へ翻訳
  ''' </summary>
  ''' <param name="englishText"></param>
  ''' <returns></returns>
  Public Function TranslateEnglishToJapanese(englishText As String) As String

    Try

      'アクセストークン取得
      'アクセストークンは10分間有効であるため、余裕を見て9分以上経過している場合に再度取得する
      If IsNothing(AdmToken) OrElse Me.Stopwatch.ElapsedMilliseconds > (Me.AccessTokenExpiresIn * 1000) Then
        Me.Stopwatch.Reset()
        Me.Stopwatch.Start()
        Me.AdmToken = Me.AdmAuth.GetAccessToken()

        ' アクセストークンの有効期限(秒）を記憶
        ' 余裕を見て90%にする
        Me.AccessTokenExpiresIn = CInt(Me.AdmToken.expires_in * 0.9)
      End If

      ' Create a header with the access_token property of the returned token
      Dim headerValue As String = "Bearer " + Me.AdmToken.access_token

      '翻訳実施
      Dim japaneseText As String = Me.TranslateMethod(headerValue, englishText)
      System.Threading.Thread.Sleep(100) 'wait

      Return japaneseText
    Catch ex As Exception
      Throw New ApplicationException(Me.GetErrorMessage(ex), ex)
    End Try

  End Function


  ''' <summary>
  ''' TranslateMethod
  ''' </summary>
  ''' <param name="authToken"></param>
  ''' <param name="text"></param>
  ''' <returns></returns>
  Private Function TranslateMethod(authToken As String, text As String) As String
    Dim translation As String = ""
    Dim [from] As String = "en"
    Dim [to] As String = "ja"

    Dim format As String = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text={0}&from={1}&to={2}"
    Dim uri As String = String.Format(format, System.Web.HttpUtility.UrlEncode(text), [from], [to])


    Dim httpWebRequest As Net.HttpWebRequest = CType(Net.WebRequest.Create(uri), Net.HttpWebRequest)
    httpWebRequest.Headers.Add("Authorization", authToken)
    Dim response As Net.WebResponse = Nothing
    Try
      response = httpWebRequest.GetResponse()
      Using stream As IO.Stream = response.GetResponseStream()

        Dim dcs As System.Runtime.Serialization.DataContractSerializer =
                        New System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"))
        translation = dcs.ReadObject(stream).ToString()

      End Using

    Finally

      If Not IsNothing(response) Then
        response.Close()
        response = Nothing
      End If

    End Try

    Return translation

  End Function

  ''' <summary>
  ''' GetErrorMessage
  ''' </summary>
  ''' <param name="e"></param>
  ''' <returns></returns>
  Private Function GetErrorMessage(e As System.Net.WebException) As String
    Dim sb As New System.Text.StringBuilder
    sb.AppendLine(e.ToString())

    ' Obtain detailed error information
    Dim strResponse As String = ""
    Using response As Net.HttpWebResponse = CType(e.Response, Net.HttpWebResponse)
      Using responseStream As IO.Stream = response.GetResponseStream()
        Using sr As IO.StreamReader = New IO.StreamReader(responseStream, System.Text.Encoding.ASCII)
          strResponse = sr.ReadToEnd()
        End Using
      End Using
      sb.AppendLine("Http status code=" & e.Status & ", error message=" & strResponse)
      Return sb.ToString()
    End Using
  End Function

End Class
