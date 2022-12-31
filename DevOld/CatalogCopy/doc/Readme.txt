===========
CatalogCopy
===========


差分コピーを３段階に分けて行う。

1. コピー先ディレクトリの情報(カタログ)を生成する。
2. カタログを元にコピー元ディレクトリから差分を生成する。
3. コピー先ディレクトリに差分を適用する。

別々の環境にある複数のディレクトリを同期するために使用することを想定している。
差分が少なければ、主従両環境間でやり取りする情報量は少なくなるはず。


----
コマンド

CatalogCopy.exe /A コピー先ディレクトリ 出力カタログファイル

	カタログファイル生成


CatalogCopy.exe /B コピー元ディレクトリ 入力カタログファイル 出力差分ディレクトリ

	差分ディレクトリ生成


CatalogCopy.exe /C 入力差分ディレクトリ コピー先ディレクトリ

	差分適用


CatalogCopy.exe /D 入力差分ディレクトリ コピー先ディレクトリ

	差分適用・移動モード
	差分ディレクトリとコピー先ディレクトリが同じドライブにあるとき高速に動作するが、
	差分ディレクトリの中身は破壊される。


----
推奨拡張子

カタログファイル

	.cata

差分ディレクトリ

	.diff


----
実行例

CatalogCopy.exe /A C:\Output C:\temp\Catalog.cata

CatalogCopy.exe /B C:\Input C:\temp\Catalog.cata C:\temp\Difference.diff

CatalogCopy.exe /C C:\temp\Difference.diff C:\Output

