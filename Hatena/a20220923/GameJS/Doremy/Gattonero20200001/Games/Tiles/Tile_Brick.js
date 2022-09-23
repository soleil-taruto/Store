/*
	É^ÉCÉã - Brick
*/

var<int> TileKind_Brick = @(AUTO);

function <Tile_t> CreateTile_Brick(<int> x, <int> y, <Picture_t> wideLeft, <Picture_t> wideRight, <Picture_t> square)
{
	var ret =
	{
		Kind: TileKind_Brick,
		TileMode: TileMode_e_WALL,

		// Ç±Ç±Ç©ÇÁå≈óL

		<int> X: x,
		<int> Y: y,

		<Picture_t> Wide_L: wideLeft,
		<Picture_t> Wide_R: wideRight,
		<Picture_t> Square: square,

		<int> Mode: -1, // { -1, 1, 2, 3 } == { ñ¢ê›íË, L, R, S }
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	if (tile.Mode == -1)
	{
		var<int> mode;

		if ((tile.X + tile.Y) % 2 == 0)
		{
			mode = 1;
		}
		else
		{
			mode = 2;
		}

		if (mode == 1)
		{
			if (GetMapCell_XY(tile.X + 1, tile.Y).Tile.Kind != TileKind_Brick)
			{
				mode = 3;
			}
		}
		if (mode == 2)
		{
			if (GetMapCell_XY(tile.X - 1, tile.Y).Tile.Kind != TileKind_Brick)
			{
				mode = 3;
			}
		}

		tile.Mode = mode;
	}

	var<Picture_t> picture;

	switch (tile.Mode)
	{
	case 1: picture = tile.Wide_L; break;
	case 2: picture = tile.Wide_R; break;
	case 3: picture = tile.Square; break;

	default:
		error(); // never
	}

	Draw(picture, dx, dy, 1.0, 0.0, 1.0);
}
