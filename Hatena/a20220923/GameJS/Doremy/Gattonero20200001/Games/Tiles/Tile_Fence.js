/*
	タイル - Fence
*/

var<int> TileKind_Fence = @(AUTO);

function <Tile_t> CreateTile_Fence(<int> x, <int> y, <Picture_t[]> pictures)
{
	var ret =
	{
		Kind: TileKind_Fence,
		TileMode: TileMode_e_CLOUD,

		// ここから固有

		<int> X: x,
		<int> Y: y,

		<Picture_t[]> Pictures: pictures, // [1]～[9] の画像を使用する。

		<int> Mode: -1, // -1 == 未設定, 1～9 == タイルの位置(テンキー方式)
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	if (tile.Mode == -1)
	{
		var<int> mode = 0;

		if (GetMapCell_XY(tile.X - 1, tile.Y).Tile.Kind != TileKind_Fence) mode += 1;
		if (GetMapCell_XY(tile.X + 1, tile.Y).Tile.Kind != TileKind_Fence) mode += 2;
		if (GetMapCell_XY(tile.X, tile.Y - 1).Tile.Kind != TileKind_Fence) mode += 4;
		if (GetMapCell_XY(tile.X, tile.Y + 1).Tile.Kind != TileKind_Fence) mode += 8;

		switch (mode)
		{
		case 0: mode = 5; break;

		case 1: mode = 4; break;
		case 2: mode = 6; break;
		case 4: mode = 8; break;
		case 8: mode = 2; break;

		case 1 + 4: mode = 7; break;
		case 2 + 4: mode = 9; break;
		case 1 + 8: mode = 1; break;
		case 2 + 8: mode = 3; break;

		default:
			error(); // 周囲のフェンスの配置が想定外
		}

		tile.Mode = mode;
	}

	var<Picture_t> picture = tile.Pictures[tile.Mode];

	// プレイヤー・敵より手前に表示する。

	AddTask(FrontTasks, function* <generatorForTask> ()
	{
		Draw(picture, dx, dy, 1.0, 0.0, 1.0);
	}());
}
