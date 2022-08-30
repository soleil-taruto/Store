/*
	ƒ^ƒCƒ‹ - None
*/

var<int> TileKind_None = @(AUTO);

function <Tile_t> CreateTile_None()
{
	var ret =
	{
		Kind: TileKind_None,
		TileMode: TileMode_e_SPACE,

		// ‚±‚±‚©‚çŒÅ—L
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	// noop
}
