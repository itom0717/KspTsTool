Kerbal Space Program Translation Support Tool
====

このプログラムは Kerbal Space Program のPart(MODにも対応)の説明文を抽出し、Microsoft Translator APIを使用して自動翻訳します。  
 ModuleManager.dll で使用するcfgファイルを作成します。


## 開発環境
 Microsoft Visual Studio Community 2015

## 必要ランタイム
 .NET Framework 4.5  

## 使い方

事前準備  
  自動翻訳させる場合はMicrosoft Translator APIの「クライアントID」と「顧客の秘密」を取得する必要があります。  
  
  
① GameDataフォルダは Kerbal Space Programをインストールした先にある、GameDataフォルダを指定してください。  
  
② 翻訳データ先は、任意のフォルダを指定します。  
  
③「翻訳」設定で、Microsoft Translator APIの「クライアントID」と「顧客の秘密」を指定します。  
  
④ 処理実行で処理を開始します。  自動翻訳する場合は少し時間がかかりますので、気長に待ってください。
  
⑤ 終了すると②で指定したフォルダに ModuleManager用のcfgファイル が生成されますので、ModuleManager.dll で読み込ませてください。
  
⑥ 手動で翻訳する場合は、本実行ファイルと同じ場所に「TranslationDataBase.tsv」がありますので、直接編集してください。  
  タブ区切りのテキストファイルです。（文字コードはUTF-16です）
  
⑦「翻訳cfg読込」ボタンで別の翻訳済みの ModuleManager用のcfgファイル から、翻訳テキストを「TranslationDataBase.tsv」に取り込むことができます。  


## Licence
* MIT  
    * see LICENSE.txt

## Author

[itom0717](https://github.com/itom0717)
