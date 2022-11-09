========
Compress
========

----
コマンド

Compress.exe 入力フォルダ 出力ファイル

	圧縮

Compress.exe /D 入力ファイル 出力フォルダ

	展開


----
圧縮ファイルの推奨拡張子

	.cmp.gz

例：

	CompressedFile.cmp.gz


----
補足

入出力の例：

	以下のフォルダ・ファイル構成があった場合

		C:\Data\Input\.
		|
		+--- SubDate\.
		|    |
		|    +--- SubData.dat
		|    +--- SubData2.dat
		|    +--- SubData3.dat
		|
		+--- Data.dat
		+--- Data2.dat
		+--- Data3.dat

	Compress.exe C:\Data\Input C:\temp\AsshukuZumi.cmp.gz

	で C:\temp\AsshukuZumi.cmp.gz が作成される。

	Compress.exe /D C:\temp\AsshukuZumi.cmp.gz C:\temp2\Soba\Udon\Output

	で、以下のフォルダ・ファイル構成が再現される。

		C:\temp2\Soba\Udon\Output\.
		|
		+--- SubDate\.
		|    |
		|    +--- SubData.dat
		|    +--- SubData2.dat
		|    +--- SubData3.dat
		|
		+--- Data.dat
		+--- Data2.dat
		+--- Data3.dat


上書き：

	出力先フォルダが存在しない場合は作成する。
	出力先フォルダが存在する場合、削除せずそのまま書き出す。

