/*
	タイル
*/

/@(ASTR)

/// Tile_t
{
	<int> Kind // タイルの種類
	<boolean> WallFlag // 壁か
	<Action double double> Draw // 描画
}

@(ASTR)/

/*
	描画
*/
function <void> DrawTile(<Tile_t> tile, <double> dx, <double> dy)
{
	return tile.Draw(tile, dx, dy);
}
