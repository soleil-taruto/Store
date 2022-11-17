===========
EditPicture
===========


----
コマンド

EditPicture.exe (画像ファイル | 画像ファイル入りフォルダ) [/E 倍率] [/T X Y] [/TC R G B] [/BC R G B] [/ACM X Y M] [/S 幅 高さ]
                                                          [/S4 幅 高さ X-SAMPLING Y-SAMPLING]

	/E   ... 拡大する。倍率は２以上の整数であること。
	/T   ... 指定座標の色を透明にする。
	/TC  ... 指定色を透明にする。
	/BC  ... 指定色を背景色にして透過無しにする。
	/ACM ... 自動カット・マージン
	         第1-2パラメータ＝マージン色を取得する位置(X,Y)
	         第3パラメータ＝新しいマージンの幅
	/S   ... 指定サイズに拡大・縮小する。
	/S4  ... 同上


実行例

EditPicture.exe C:\Data\Images /E 2
EditPicture.exe C:\Data\Images /T 0 0
EditPicture.exe C:\Data\Images /TC 255 255 255
EditPicture.exe C:\Data\Images /BC 255 255 255
EditPicture.exe C:\Data\Images /ACM 0 0 0
EditPicture.exe C:\Data\Images /ACM 0 0 10
EditPicture.exe C:\Data\Images /S 800 600


以下のようにコマンドを連結しても良い。

EditPicture.exe C:\Data\Images /E 2 /T 0 0

