/*
	ƒ^ƒCƒ‹ - Water
*/

var<int> TileKind_Water = @(AUTO);

function <Tile_t> CreateTile_Water(<int> x, <int> y)
{
	var ret =
	{
		Kind: TileKind_Water,
		TileMode: TileMode_e_WATER,

		// ‚±‚±‚©‚çŒÅ—L

		<int> X: x,
		<int> Y: y,

		<int> Mode_LT: -1,
		<int> Mode_RT: -1,
		<int> Mode_LB: -1,
		<int> Mode_RB: -1,
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	var<int> x = tile.X;
	var<int> y = tile.Y;

	if (tile.Mode_LT == -1)
	{
		var<boolean> stranger_4 = !@@_IsFriend(x - 1, y);
		var<boolean> stranger_6 = !@@_IsFriend(x + 1, y);
		var<boolean> stranger_8 = !@@_IsFriend(x, y - 1);
		var<boolean> stranger_2 = !@@_IsFriend(x, y + 1);

		var<boolean> stranger_1 = !@@_IsFriend(x - 1, y + 1);
		var<boolean> stranger_3 = !@@_IsFriend(x + 1, y + 1);
		var<boolean> stranger_7 = !@@_IsFriend(x - 1, y - 1);
		var<boolean> stranger_9 = !@@_IsFriend(x + 1, y - 1);

		var<int> mode;

		if (stranger_4 && stranger_8)
		{
			mode = 0;
		}
		else if (stranger_4)
		{
			mode = 1;
		}
		else if (stranger_8)
		{
			mode = 2;
		}
		else if (stranger_7)
		{
			mode = 3;
		}
		else
		{
			mode = 4;
		}
		tile.Mode_LT = mode;

		if (stranger_6 && stranger_8)
		{
			mode = 0;
		}
		else if (stranger_6)
		{
			mode = 1;
		}
		else if (stranger_8)
		{
			mode = 2;
		}
		else if (stranger_9)
		{
			mode = 3;
		}
		else
		{
			mode = 4;
		}
		tile.Mode_RT = mode;

		if (stranger_4 && stranger_2)
		{
			mode = 0;
		}
		else if (stranger_4)
		{
			mode = 1;
		}
		else if (stranger_2)
		{
			mode = 2;
		}
		else if (stranger_1)
		{
			mode = 3;
		}
		else
		{
			mode = 4;
		}
		tile.Mode_LB = mode;

		if (stranger_6 && stranger_2)
		{
			mode = 0;
		}
		else if (stranger_6)
		{
			mode = 1;
		}
		else if (stranger_2)
		{
			mode = 2;
		}
		else if (stranger_3)
		{
			mode = 3;
		}
		else
		{
			mode = 4;
		}
		tile.Mode_RB = mode;
	}

	var<int> koma = ToFix(ProcFrame / 4) % 8;

	Draw(P_Tile_Grass, dx, dy, 1.0, 0.0, 1.0);

	Draw(P_Tile_Water[0][tile.Mode_LT * 2 + 0][koma], dx - TILE_W / 4.0, dy - TILE_H / 4.0, 1.0, 0.0, 1.0);
	Draw(P_Tile_Water[1][tile.Mode_RT * 2 + 0][koma], dx + TILE_W / 4.0, dy - TILE_H / 4.0, 1.0, 0.0, 1.0);
	Draw(P_Tile_Water[0][tile.Mode_LB * 2 + 1][koma], dx - TILE_W / 4.0, dy + TILE_H / 4.0, 1.0, 0.0, 1.0);
	Draw(P_Tile_Water[1][tile.Mode_RB * 2 + 1][koma], dx + TILE_W / 4.0, dy + TILE_H / 4.0, 1.0, 0.0, 1.0);
}

function <boolean> @@_IsFriend(<int> x, <int> y)
{
	if (
		x < 0 || Map.W <= x ||
		y < 0 || Map.H <= y
		)
	{
		return true;
	}

	return GetMapCell_XY(x, y).Tile.Kind == TileKind_Water;
}
