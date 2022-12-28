===============
RemoteCmdServer
===============

----
コマンド

	RemoteCmdServer.exe [/P] ポート番号 バッチファイル-DIR ファイル送受信-DIR

		/P ... POST-リクエストのみ受け付ける。


----
終了方法

	RemoteCmdServer.exe /S

	または、キー押下


----
アクセス方法

	http://localhost/B/aaa/bbb/ccc.xxx

		(バッチファイル-DIR)\aaa\bbb\ccc.xxx をバッチファイルとして実行する。


	http://localhost/D/aaa/bbb/ccc.xxx

		(ファイル送受信-DIR)\aaa\bbb\ccc.xxx をダウンロードする。


	http://localhost/U/aaa/bbb/ccc.xxx

		POST-リクエスト

			リクエスト-BODYの内容を (ファイル送受信-DIR)\aaa\bbb\ccc.xxx に出力する。

		GET-リクエスト

			空のファイル (ファイル送受信-DIR)\aaa\bbb\ccc.xxx を作成する。

		★フォルダが無ければ作成する。


	http://localhost/A/aaa/bbb/ccc.xxx

		POST-リクエスト

			リクエスト-BODYの内容を (ファイル送受信-DIR)\aaa\bbb\ccc.xxx に追記する。

		GET-リクエスト

			空のバイト列を (ファイル送受信-DIR)\aaa\bbb\ccc.xxx に追記する。

		★フォルダが無ければ作成する。


	http://localhost/K/aaa/bbb/ccc.xxx

		(ファイル送受信-DIR)\aaa\bbb\ccc.xxx を削除する。


----
補足

送信ファイル最大サイズ = 100 TB

