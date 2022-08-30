/*
	タイル
*/

/// TileMode_e
//
var<int> TileMode_e_SPACE = @(AUTO); // 空間
var<int> TileMode_e_WATER = @(AUTO); // 水域
var<int> TileMode_e_WALL  = @(AUTO); // 壁

/@(ASTR)

/// Tile_t
{
	<int> Kind // タイルの種類
	<TileMode_e> TileMode // タイルの振る舞い
	<Action Tile_t double double> Draw // 描画
}

@(ASTR)/

/*
	描画

	(dx, dy): 描画位置(カメラ位置適用済み)
*/
function <void> DrawTile(<Tile_t> tile, <double> dx, <double> dy)
{
	return tile.Draw(tile, dx, dy);
}
