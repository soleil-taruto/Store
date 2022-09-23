/*
	ƒ^ƒCƒ‹ - •Ç
*/

var<int> TileKind_Ladder = @(AUTO);

function <Tile_t> CreateTile_Ladder()
{
	var ret =
	{
		Kind: TileKind_Ladder,
		TileMode: TileMode_e_LADDER,

		// ‚±‚±‚©‚çŒÅ—L
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	Draw(P_Tile_Ladder, dx, dy, 1.0, 0.0, 1.0);
}
