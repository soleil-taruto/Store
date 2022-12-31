===========
CatalogCopy
===========

----
コマンド

CatalogCopy.exe /A コピー先ディレクトリ 出力カタログファイル

	カタログファイル生成


CatalogCopy.exe /B コピー元ディレクトリ 入力カタログファイル 出力差分ディレクトリ

	差分ディレクトリ生成


CatalogCopy.exe /C 出力差分ディレクトリ コピー先ディレクトリ

	差分適用


----
推奨拡張子

カタログファイル

	.cata

差分ディレクトリ

	.diff


----
実行例

CatalogCopy.exe /A C:\Output C:\Data\Catalog.cata

CatalogCopy.exe /B C:\Input C:\Data\Catalog.cata C:\Data\Difference.diff

CatalogCopy.exe /C C:\Data\Difference.diff C:\Output

