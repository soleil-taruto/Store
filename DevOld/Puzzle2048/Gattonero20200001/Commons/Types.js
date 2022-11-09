/*
	型
*/

/@(ASTR)

/// D2Size_t
{
	<double> W // 幅
	<double> H // 高さ
}

/// D3Color_t
{
	// 各値は 0.0 ～ 1.0

	<double> R // 赤
	<double> G // 緑
	<double> B // 青
}

/// D4Color_t
{
	// 各値は 0.0 ～ 1.0

	<double> R // 赤
	<double> G // 緑
	<double> B // 青
	<double> A // アルファ値
}

/// D4Rect_t
{
	<double> L // 左位置-X
	<double> T // 上位置-Y
	<double> W // 幅
	<double> H // 高さ
}

/// I2Size_t
{
	<int> W // 幅
	<int> H // 高さ
}

/// I3Color_t
{
	// 各値は 0 ～ 255

	<int> R // 赤
	<int> G // 緑
	<int> B // 青
}

/// I4Color_t
{
	// 各値は 0 ～ 255

	<int> R // 赤
	<int> G // 緑
	<int> B // 青
	<int> A // アルファ値
}

/// I4Rect_t
{
	<int> L // 左位置-X
	<int> T // 上位置-Y
	<int> W // 幅
	<int> H // 高さ
}

/// P4Poly_t
{
	<D2Point_t> LT // 左上
	<D2Point_t> RT // 右上
	<D2Point_t> RB // 右下
	<D2Point_t> LB // 左下
}

@(ASTR)/
