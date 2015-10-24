Kerbal Space Program Translation Support Tool
====

このプログラムは Kerbal Space Program のPart(MODにも対応)および ScienceDefs.cfg(MODも対応)の説明文を抽出し、Microsoft Translator APIを使用して自動で機械翻訳します。  
ModuleManager.dll で使用するcfgファイルを作成します。  

###※Kerbal Space Program本体のファイルは変更しません。ModuleManager.dll用のConfigファイルを作成するだけです。


## 開発環境
 Microsoft Visual Studio Community 2015

## 必要ランタイム
 .NET Framework 4.5.2  

## 使い方

###事前準備  
自動翻訳させる場合はMicrosoft Translator APIの「クライアントID」と「顧客の秘密」を取得する必要があります。  
※Windows Azure Marketplaceへ登録等が必要です。(有料版もありますが、無料版でOK） 


###実行方法  
�@ GameDataフォルダは Kerbal Space Programをインストールした先にある、GameDataフォルダを指定します。  
  
�A 翻訳データ保存先は、任意のフォルダを指定します。  
  
�B「翻訳」設定で、Microsoft Translator APIの「クライアントID」と「顧客の秘密」を入力します。  
  
�C 処理実行で処理を開始します。  自動で機械翻訳する場合は少し時間がかかりますので、気長に待ってください。
  
�D 終了すると�Aで指定したフォルダに ModuleManager用のcfgファイル が生成されますので、ModuleManager.dll で読み込ませてください。  
  ※確認は ModuleManager.dll Ver 2.6.8で行っています。

　　Kerbal Space Program  
　　　　　+--- GameData  
   　　　　　+--- Squad  
   　　　　　+--- (各MODのフォルダ)   
   　　　　　+---  ・  
   　　　　　+---  ・  
   　　　　　+---  ・  
   　　　　　+--- ModuleManager.dll <--- を設置  
   　　　　　+--- ToJapanese  <--- 任意のフォルダを作成し、この中に �Aのフォルダ内に作成されたファイルを入れる  
  


###自分で翻訳または日本語を修正する場合  
機械翻訳では（ほとんど）意味が通じないので、自分で修正してください。  

�@翻訳データ保存先にある、*.cfgファイルの日本語部分を編集します。  
　※コメントアウト // になっている場合は　//を消してください。

�AModuleManager.dllで読み込ませます。  終わり。

�B次回処理時に修正した翻訳データを使用するため、翻訳データベースに取り込んでおきます。

�C「翻訳済ファイル取込」で 修正した*.cfgファイルを選択し、取り込みます。

※有志の方々が翻訳された *.cfgファイルも取り込めるかも。  


###翻訳データベースファイル

本実行ファイルと同じ場所に「TranslationDataBase.tsv」がそのファイルです。  
タブ区切りのテキストファイルです。（文字コードはUTF-8です）  

 

## Licence
* MIT  
    * see LICENSE.txt

## Author

[itom0717](https://github.com/itom0717)
