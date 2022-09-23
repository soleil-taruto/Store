/*
	ƒ^ƒCƒ‹ - •Ç
*/

var<int> TileKind_Wall = @(AUTO);

function <Tile_t> CreateTile_Wall(<Picture_t> picture)
{
	var ret =
	{
		Kind: TileKind_Wall,
		TileMode: TileMode_e_WALL,

		// ‚±‚±‚©‚çŒÅ—L

		Picture: picture,
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	Draw(tile.Picture, dx, dy, 1.0, 0.0, 1.0);
}
