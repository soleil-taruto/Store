======
RunAll
======

----
説明

指定ディレクトリ直下の *.bat と *.exe を全て実行します。


----
コマンド

RunAll.exe TARGET-DIR

	TARGET-DIR ... 指定ディレクトリ


実行例

	RunAll.exe C:\home\bat
	RunAll.exe \temp
	RunAll.exe .


----
補足

実行順序は { 1. (*.bat) -> (*.exe) , 2. 名前の辞書順 } です。

指定ディレクトリの直下に "RunAll_{91f9b5c9-261f-4534-bb18-202b24c223e1}.tmp" というファイルを一時的に書き出します。
このファイルは正常終了時に削除されます。

ファイル一覧は DIR *.xxx /A-D /B /ON コマンドのリダイレクトから取得します。
変な名前のファイルがあると正常に動作しないかもしれません。

