===============
ConfuserForElsa
===============

----
コマンド

ConfuserForElsa.exe 難読化対象ソリューションファイル 作業フォルダ

	作業フォルダ ... 存在するフォルダであること。


----
実行例

ConfuserForElsa.exe C:\Dev\Game\00_MSSAGame\Elsa20200001\Elsa20200001.sln C:\temp


----
補足

難読化した実行ファイルは、難読化対象ソリューション配下の bin\Release の直下に以下のとおり置かれます。

	<Project>.exe-confused                       ... 難読化した実行ファイル
	<Project>.exe-confused-rename-table.txt.gz   ... 置き換えた名前の対応テーブルのテキストファイルを圧縮(gz)したファイル


難読化したくない行の記述例：

	Year // KeepComment:@^_ConfuserForElsa // NoRename:@^_ConfuserForElsa

