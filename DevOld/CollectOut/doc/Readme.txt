==========
CollectOut
==========


リリース・ビルドの出力を回収する。

検索ディレクトリの配下にある out ディレクトリの中身を
out の親ディレクトリ名のフォルダに入れて出力ディレクトリにコピーする。

out の中身が１つの .zip ファイルである場合：
・その .zip ファイルを出力ディレクトリにコピーする。
・バックアップ先ディレクトリが指定されていればバックアップを行う。

出力ディレクトリは C:\1 ～ 999


----
コマンド

CollectOut.exe 検索ディレクトリ [/B バックアップ先ディレクトリ] [/D 配布先ディレクトリ...]

★ /B は /D よりも先に指定すること。


実行例

CollectOut.exe C:\Dev
CollectOut.exe C:\Dev\Game /D C:\home\HPStore\Game C:\home\HPGame\Sword\Storehouse
CollectOut.exe C:\Dev\GameJS /D C:\home\HPStore\GameJS C:\home\HPGame\Shield\Contents C:\home\HPGame\Soleil\Contents
CollectOut.exe C:\Dev /D C:\home\HPStore\Game C:\home\HPGame\Sword\Storehouse C:\home\HPStore\GameJS C:\home\HPGame\Shield\Contents C:\home\HPGame\Soleil\Contents
CollectOut.exe C:\Dev\Game /B C:\home\HPStore\GameOldVersion /D C:\home\HPStore\Game C:\home\HPGame\Sword\Storehouse

