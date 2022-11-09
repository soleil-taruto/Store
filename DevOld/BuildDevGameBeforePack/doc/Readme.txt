======================
BuildDevGameBeforePack
======================

----
コマンド

BuildDevGameBeforePack.exe BUILD-DEV-GAME-UNSAFE-MOD 出力DIR リソースDIR

	★第1引数は安全のための固定文字列


----
補足

もっぱら自分用ツール也。

C:\Dev\Game 配下の Debug.bat, Release.bat から呼び出す。


----
_source.txt 書式

ゲームのdatフォルダ配下にリソースの出処を記載して慣習的に配置しているファイル

テキストファイル
文字コード：SJIS

要件
	2行以上
	1行目は作者の名前
	2行目は作者のURL

行数は3行、4行...であっても良い。(3行目以降は無視する)

作者の名前は空文字列であってはならない。

作者のURLは以下のいずれかであること。
	http:// で始まる
	https:// で始まる
	(No URL)

