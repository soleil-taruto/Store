/*
	ƒ^ƒCƒ‹ - Grass
*/

var<int> TileKind_Grass = @(AUTO);

function <Tile_t> CreateTile_Grass()
{
	var ret =
	{
		Kind: TileKind_Grass,
		TileMode: TileMode_e_SPACE,

		// ‚±‚±‚©‚çŒÅ—L
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	Draw(P_Tile_Grass, dx, dy, 1.0, 0.0, 1.0);
}
