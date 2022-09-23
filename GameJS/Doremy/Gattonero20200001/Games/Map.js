/*
	マップ
*/

/@(ASTR)

/// MapCell_t
{
	// タイル
	//
	<Tile_t> Tile

	// 敵生成
	// 敵が居ない場合は null または null を返すこと。
	//
	<Func Enemy_t> F_CreateEnemy
}

/// Map_t
{
	// テーブルの幅 (横のセル数)
	// MAP_W_MIN～
	//
	var<int> W

	// テーブルの高さ (縦のセル数)
	// MAP_H_MIN～
	//
	var<int> H

	// マップセルのテーブル
	// 添字：[x][y]
	// サイズ：[this.W][this.H]
	//
	<MapCell_t[][]> Table
}

@(ASTR)/

/*
	マップの数を取得する。
*/
function <int> GetMapCount()
{
	return MAPS.length;
}

/*
	読み込まれたマップ
*/
var<Map_t> Map = null;

/*
	マップを読み込む

	mapIndex: 0 ～ (GetMapCount() - 1)
*/
function <void> LoadMap(<int> mapIndex)
{
	Map = {};

	var<string[]> lines = MAPS[mapIndex];

	{
		var<int> h = lines.length;

		if (h < MAP_H_MIN || IMAX < h)
		{
			error();
		}

		var<int> w = lines[0].length;

		if (w < MAP_W_MIN || IMAX < w)
		{
			error()
		}

		for (var<int> y = 0; y < h; y++)
		{
			if (@@_LineToChars(lines[y]).length != w)
			{
				error();
			}
		}

		Map.W = w;
		Map.H = h;
	}

	Map.Table = [];

	for (var<int> x = 0; x < Map.W; x++)
	{
		Map.Table.push([]);

		for (var<int> y = 0; y < Map.H; y++)
		{
			@@_X = x;
			@@_Y = y;

			Map.Table[x].push(@@_CharToMapCell(@@_LineToChars(lines[y])[x]));
		}
	}
}

// 敵のロード
//
function <void> LoadEnemyOfMap()
{
	ClearArray(GetEnemies());

	for (var<int> x = 0; x < Map.W; x++)
	for (var<int> y = 0; y < Map.H; y++)
	{
		var<MapCell_t> cell = Map.Table[x][y];
		var<Func Enemy_t> f_createEnemy = cell.F_CreateEnemy;

		if (f_createEnemy != null)
		{
			var<Enemy_t> enemy = f_createEnemy();

			if (enemy != null)
			{
				GetEnemies().push(enemy);
			}
		}
	}
}

// プレイヤーをスタート地点へ移動する。
//
function MoveToStartPtOfMap()
{
	var<D2Point_t> pt = @@_GetStartPt();

	PlayerX = pt.X;
	PlayerY = pt.Y;
}

function <D2Point_t> @@_GetStartPt()
{
	for (var<Enemy_t> enemy of GetEnemies())
	{
		if (enemy.Kind == EnemyKind_Start)
		{
			return CreateD2Point(enemy.X, enemy.Y);
		}
	}
	error(); // スタート地点見つからじ。
}

function <string[]> @@_LineToChars(<stirng> line)
{
	var<string[]> dest = [];

	for (var<int> index = 0; index < line.length; index++)
	{
		if (index + 1 < line.length && (DECIMAL + ALPHA_UPPER + ALPHA_LOWER).indexOf(line[index]) != -1)
		{
			dest.push(line.substring(index, index + 2));
			index++;
		}
		else
		{
			dest.push(line.substring(index, index + 1));
		}
	}
	return dest;
}

var<int> @@_X = -1;
var<int> @@_Y = -1;

function <MapCell_t> @@_CharToMapCell(<string> chr)
{
	// 敵の座標(テーブル・インデックス)
	//
	var<int> ix = @@_X;
	var<int> iy = @@_Y;

	// 敵の位置(ドット単位)
	//
	var<D2Point> pt = ToFieldPoint_XY(ix, iy);
	var<double> x = pt.X;
	var<double> y = pt.Y;

	var<Picture_t[]> fencePictures =
	[
		P_Dummy,
		P_Tile_Fence1,
		P_Tile_Fence2,
		P_Tile_Fence3,
		P_Tile_Fence4,
		P_Tile_Fence5,
		P_Tile_Fence6,
		P_Tile_Fence7,
		P_Tile_Fence8,
		P_Tile_Fence9,
	];

	// タイル系
	//
	if (chr == "壁") return @@_CreateMapCell_T(CreateTile_BDummy()); // ★サンプル
	if (chr == "UC") return @@_CreateMapCell_T(CreateTile_BUnderConstruction());
	if (chr == "　") return @@_CreateMapCell_T(CreateTile_None());
	if (chr == "W1") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_B1));
	if (chr == "W2") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_B2));
	if (chr == "W3") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_B3));
	if (chr == "W4") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_B4));
	if (chr == "梯") return @@_CreateMapCell_T(CreateTile_Ladder());
	if (chr == "■") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_Brick_S));
	if (chr == "芝") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_Ground1));
	if (chr == "地") return @@_CreateMapCell_T(CreateTile_Wall(P_Tile_Ground2));
	if (chr == "煉") return @@_CreateMapCell_T(CreateTile_Brick(ix, iy, P_Tile_Brick_L1, P_Tile_Brick_L2, P_Tile_Brick_L3));
	if (chr == "柵") return @@_CreateMapCell_T(CreateTile_Fence(ix, iy, fencePictures));

	// 敵系
	//
	if (chr == "敵") return @@_CreateMapCell_E(() => CreateEnemy_BDummy(x, y)); // ★サンプル
	if (chr == "始") return @@_CreateMapCell_E(() => CreateEnemy_Start(x, y));
	if (chr == "終") return @@_CreateMapCell_E(() => CreateEnemy_Goal(x, y));
	if (chr == "蛙") return @@_CreateMapCell_E(() => CreateEnemy_Frog(x, y));
	if (chr == "砲") return @@_CreateMapCell_E(() => CreateEnemy_Houdai(x, y));
	if (chr == "B1") return @@_CreateMapCell_E(() => CreateEnemy_Boss01(x, y));
//	if (chr == "B2") return @@_CreateMapCell_E(() => CreateEnemy_Boss02(x, y));
//	if (chr == "B3") return @@_CreateMapCell_E(() => CreateEnemy_Boss03(x, y));
	if (chr == "AC") return @@_CreateMapCell_E(() => CreateEnemy_AutoStageClear(x, y));

	error();
}

function <MapCell_t> @@_CreateMapCell_T(<Tile_t> tile)
{
	return @@_CreateMapCell(tile, () => null);
}

function <MapCell_t> @@_CreateMapCell_E(<Func Enemy_t> f_createEnemy)
{
	return @@_CreateMapCell(CreateTile_None(), f_createEnemy);
}

function <MapCell_t> @@_CreateMapCell(<Tile_t> tile, <Func Enemy_t> f_createEnemy)
{
	var ret =
	{
		Tile: tile,
		F_CreateEnemy: f_createEnemy,
	};

	return ret;
}

var<MapCell_t> DEFAULT_MAP_CELL;

function <void> @(UNQN)_INIT()
{
	DEFAULT_MAP_CELL =
	{
		Tile: CreateTile_Wall(P_TileNone),
		F_CreateEnemy: () => null,
	};
}

function <MapCell_t> GetMapCell(<I2Point_t> pt)
{
	return GetMapCell_XY(pt.X, pt.Y);
}

function <MapCell_t> GetMapCell_XY(<int> x, <int> y)
{
	if (
		x < 0 || Map.W <= x ||
		y < 0 || Map.H <= y
		)
	{
		return DEFAULT_MAP_CELL;
	}

	return Map.Table[x][y];
}

// ================================
// マップの壁・足場等の判定ここから
// ================================

/*
	指定位置(テーブル・インデックス)が壁であるか判定する。
*/
function <boolean> IsWall(<I2Point_t> pt)
{
	return GetMapCell(pt).Tile.TileMode == TileMode_e_WALL;
}

function <boolean> IsWall_XY(<int> x, <int> y)
{
	return IsWall(CreateI2Point(x, y));
}

/*
	指定位置(ドット単位・マップ上の座標)が壁であるか判定する。
*/
function <boolean> IsPtWall(<D2Point_t> pt)
{
	return GetMapCell(ToTablePoint(pt)).Tile.TileMode == TileMode_e_WALL;
}

function <boolean> IsPtWall_XY(<double> x, <double> y)
{
	return IsPtWall(CreateD2Point(x, y));
}

/*
	指定位置(ドット単位・マップ上の座標)が地面であるか判定する。
*/
function <boolean> IsPtGround(<D2Point_t> pt)
{
	var<MapCell_t> cell = GetMapCell(ToTablePoint(pt));

	if (
		cell.Tile.TileMode == TileMode_e_LADDER ||
		cell.Tile.TileMode == TileMode_e_CLOUD
		)
	{
		var<I2Point_t> tablePt = ToTablePoint(pt);
		tablePt.Y--;
		var<MapCell_t> upperCell = GetMapCell(tablePt);

		if (cell.Tile.TileMode == upperCell.Tile.TileMode) // ? 真上も同種のタイル
		{
			return false;
		}

		var<double> t = ToTileCenterY(pt.Y) - TILE_H / 2.0;

		if (pt.Y < t + LADDER_TOP_GROUND_Y_SIZE)
		{
			return true;
		}

		return false;
	}

	if (cell.Tile.TileMode == TileMode_e_WALL)
	{
		return true;
	}

	return false;
}

function <boolean> IsPtGround_XY(<double> x, <double> y)
{
	return IsPtGround(CreateD2Point(x, y));
}

/*
	指定位置(ドット単位・マップ上の座標)が梯子であるか判定する。
*/
function <boolean> IsPtLadder(<D2Point_t> pt)
{
	return GetMapCell(ToTablePoint(pt)).Tile.TileMode == TileMode_e_LADDER;
}

function <boolean> IsPtLadder_XY(<double> x, <double> y)
{
	return IsPtLadder(CreateD2Point(x, y));
}

// ================================
// マップの壁・足場等の判定ここまで
// ================================
