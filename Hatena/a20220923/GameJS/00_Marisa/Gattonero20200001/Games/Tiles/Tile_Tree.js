/*
	タイル - Tree
*/

var<int> TileKind_Tree = @(AUTO);

function <Tile_t> CreateTile_Tree(<int> x, <int> y)
{
	var ret =
	{
		Kind: TileKind_Tree,
		TileMode: TileMode_e_WALL,

		// ここから固有

		<int> X: x,
		<int> Y: y,

		<int> Mode: -1, // { -1 ～ 3 } == { 未設定, 両方無し, 上側のみ, 下側のみ, 両方有り }
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	var<int> x = tile.X;
	var<int> y = tile.Y;

	/*
		true:  左上と右下に木がある。
		false: 右上と左下に木がある。
	*/
	var<boolean> lt_rb = (x + y) % 2 == 0;

	if (tile.Mode == -1)
	{
		var<boolean> mode_t;
		var<boolean> mode_b;

		if (lt_rb) // ? 左上と右下に木がある。
		{
			mode_t = @@_IsFriend2x2(x - 1, y - 1); // 左上に木がある。
			mode_b = @@_IsFriend2x2(x - 0, y - 0); // 右下に木がある。
		}
		else // ? 右上と左下に木がある。
		{
			mode_t = @@_IsFriend2x2(x - 0, y - 1); // 右上に木がある。
			mode_b = @@_IsFriend2x2(x - 1, y - 0); // 左下に木がある。
		}

		tile.Mode = (mode_t ? 1 : 0) | (mode_b ? 2 : 0);
	}

	Draw(P_Tile_Grass, dx, dy, 1.0, 0.0, 1.0);

	if (tile.Mode == 0)
	{
		Draw(P_Tile_Tree_Error, dx, dy, 1.0, 0.0, 1.0);
	}
	if ((tile.Mode & 1) != 0)
	{
		if (lt_rb)
		{
			Draw(P_Tile_Tree_RB, dx, dy, 1.0, 0.0, 1.0);
		}
		else
		{
			Draw(P_Tile_Tree_LB, dx, dy, 1.0, 0.0, 1.0);
		}
	}
	if ((tile.Mode & 2) != 0)
	{
		if (lt_rb)
		{
			Draw(P_Tile_Tree_LT, dx, dy, 1.0, 0.0, 1.0);
		}
		else
		{
			Draw(P_Tile_Tree_RT, dx, dy, 1.0, 0.0, 1.0);
		}
	}
}

function <boolean> @@_IsFriend2x2(<int> x, <int> y)
{
	var ret =
		@@_IsFriend(x + 0, y + 0) &&
		@@_IsFriend(x + 0, y + 1) &&
		@@_IsFriend(x + 1, y + 0) &&
		@@_IsFriend(x + 1, y + 1);

	return ret;
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

	return GetMapCell_XY(x, y).Tile.Kind == TileKind_Tree;
}
