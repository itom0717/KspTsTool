Kerbal Space Program Translation Support Tool
====

このプログラムは Kerbal Space Program のPart(MODにも対応)の説明文を抽出し、Microsoft Translator APIを使用して自動翻訳します。  
翻訳したデータは ModuleManager.dll で読み込ませて使用します。


## 開発環境
 Microsoft Visual Studio Community 2015

## 必要ランタイム
 .NET Framework 4.5  

## 使い方

事前準備  
  自動翻訳させる場合はMicrosoft Translator APIの「クライアントID」と「顧客の秘密」を取得する必要があります。  
  ここでは特に方法は記載しません。  


�@ GameDataフォルダは Kerbal Space Programをインストールした先にある、GameDataを指定してください。  
�A 翻訳データ先は、空のフォルダを指定します。  
�B「翻訳」設定で、Microsoft Translator APIの「クライアントID」と「顧客の秘密」を指定します。  
�C 処理実行で処理を開始します。  自動翻訳する場合は少し時間がかかりますので、気長に待ってください。
�D 終了すると�Aで指定したフォルダにcfgファイルが生成されていますので、ModuleManager.dll で読み込ませてください。
   ModuleManager.dll は説明を省きます。

�E 手動で翻訳する場合は、本実行ファイルと同じ場所に「TranslationDataBase.tsv」がありますので、直接編集してください。  
  タブ区切りのテキストファイルです。（文字コードはUTF-16です）


## Licence
* MIT  
    * see LICENSE.txt

## Author

[itom0717](https://github.com/itom0717)
