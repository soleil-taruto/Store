/*
	パネル
*/

/@(ASTR)

/// Panel_t
{
	<int> Exponent // 0 ～ (P_数字パネル.length - 1)

	<D2Point_t> DrawPt     // 描画位置_現在位置
	<D2Point_t> DrawPtDest // 描画位置_終点 , null == 接地済み
}

@(ASTR)/

function <Panel_t> CreatePanel(<int> exponent)
{
	var ret =
	{
		Exponent: exponent,

		DrawPtBgn: null,
		DrawPtEnd: null,
		DrawPt: null,
	};

	return ret;
}

/*
	1フレームに於ける行動と描画を行う。
*/
function <void> DrawPanel(<Panel_t> panel, <double> draw_x, <double> draw_y)
{
	Draw(P_数字パネル[panel.Exponent], draw_x, draw_y, 1.0, 0.0, 1.0);
}
