/*
	タイル - BDummy ★サンプル
*/

var<int> TileKind_BDummy = @(AUTO);

function <Tile_t> CreateTile_BDummy()
{
	var ret =
	{
		Kind: TileKind_BDummy,
		WallFlag: true,

		// ここから固有

		<double> Dummy_01: 1.0,
		<double> Dummy_02: 2.0,
		<double> Dummy_03: 3.0,
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	Draw(P_Dummy, dx, dy, 1.0, 0.0, 1.0);
}
