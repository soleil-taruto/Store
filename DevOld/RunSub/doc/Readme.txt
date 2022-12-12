======
RunSub
======

----
説明

指定ディレクトリ配下の <指定ノード>.bat と <指定ノード>.exe を全て実行します。


----
コマンド

RunAll.exe TARGET-DIR TARGET-NODE

	TARGET-DIR  ... 指定ディレクトリ
	TARGET-NODE ... 指定ノード


実行例

	RunSub.exe C:\Dev\Program Release
	RunSub.exe \temp ABC
	RunSub.exe . Clean


----
補足

★指定ディレクトリ直下にある <指定ノード>.bat と <指定ノード>.exe は実行しません。

実行順序は { 1. 絶対パスの辞書順 , 2. (*.bat) -> (*.exe) } です。

指定ディレクトリの直下に "RunSub_{639ec312-0240-4736-8f30-3745d1ed29fc}.tmp" というファイルを一時的に書き出します。
このファイルは正常終了時に削除されます。

ディレクトリ一覧は DIR /AD /B /ON /S コマンドのリダイレクトから取得します。
変な名前のディレクトリがあると正常に動作しないかもしれません。

