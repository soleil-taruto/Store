/*
	マップ
*/

/// MapCellType_e
//
var<int> MapCellType_e_Wall = @(AUTO);
var<int> MapCellType_e_None = @(AUTO);
var<int> MapCellType_e_Start = @(AUTO);
var<int> MapCellType_e_Goal = @(AUTO);
var<int> MapCellType_e_Enemy_BDummy = @(AUTO);
var<int> MapCellType_e_Enemy_B1 = @(AUTO); // 青敵, 進行方向：左下
var<int> MapCellType_e_Enemy_B2 = @(AUTO); // 青敵, 進行方向：下
var<int> MapCellType_e_Enemy_B3 = @(AUTO); // 青敵, 進行方向：右下
var<int> MapCellType_e_Enemy_B4 = @(AUTO); // 青敵, 進行方向：左
var<int> MapCellType_e_Enemy_B6 = @(AUTO); // 青敵, 進行方向：右
var<int> MapCellType_e_Enemy_B7 = @(AUTO); // 青敵, 進行方向：左上
var<int> MapCellType_e_Enemy_B8 = @(AUTO); // 青敵, 進行方向：上
var<int> MapCellType_e_Enemy_B9 = @(AUTO); // 青敵, 進行方向：右上
var<int> MapCellType_e_Enemy_R4 = @(AUTO); // 赤敵, 進行方向：左
var<int> MapCellType_e_Enemy_R6 = @(AUTO); // 赤敵, 進行方向：右
var<int> MapCellType_e_Enemy_G1 = @(AUTO); // 緑敵, 時計回り, 初期位置：左下
var<int> MapCellType_e_Enemy_G2 = @(AUTO); // 緑敵, 時計回り, 初期位置：下
var<int> MapCellType_e_Enemy_G3 = @(AUTO); // 緑敵, 時計回り, 初期位置：右下
var<int> MapCellType_e_Enemy_G4 = @(AUTO); // 緑敵, 時計回り, 初期位置：左
var<int> MapCellType_e_Enemy_G6 = @(AUTO); // 緑敵, 時計回り, 初期位置：右
var<int> MapCellType_e_Enemy_G7 = @(AUTO); // 緑敵, 時計回り, 初期位置：左上
var<int> MapCellType_e_Enemy_G8 = @(AUTO); // 緑敵, 時計回り, 初期位置：上
var<int> MapCellType_e_Enemy_G9 = @(AUTO); // 緑敵, 時計回り, 初期位置：右上
var<int> MapCellType_e_Enemy_G1_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：左下
var<int> MapCellType_e_Enemy_G2_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：下
var<int> MapCellType_e_Enemy_G3_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：右下
var<int> MapCellType_e_Enemy_G4_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：左
var<int> MapCellType_e_Enemy_G6_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：右
var<int> MapCellType_e_Enemy_G7_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：左上
var<int> MapCellType_e_Enemy_G8_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：上
var<int> MapCellType_e_Enemy_G9_CCW = @(AUTO); // 緑敵, 反時計回り, 初期位置：右上

/@(ASTR)

/// MapCell_t
{
	<MapCellType_e> Type

	// 壁フラグ
	//
	<boolean> WallFlag

	// 狭い通路フラグ
	//
	<boolean> NarrowFlag
}

/// Map_t
{
	// スタート座標
	// 当該セルの中心座標
	// (画面位置・ドット単位)
	//
	<D2Point_t> StartPt

	// マップセルのテーブル
	// 添字：[x][y]
	// サイズ：[MAP_W][MAP_H]
	// (テーブル座標)
	//
	<MapCell_t[][]> Table

	// このマップのステージ・インデックス
	// 0 == テスト用
	// 1 ～ (GetMapCount() - 1) == 本番ステージ
	//
	<int> Index
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
	var<string[]> lines = MAPS[mapIndex];

	if (lines.length != MAP_H)
	{
		error();
	}

	for (var<int> y = 0; y < MAP_H; y++)
	{
		if (@@_LineToChars(lines[y]).length != MAP_W)
		{
			error();
		}
	}

	Map = {};
	Map.Table = [];
	Map.Index = mapIndex;

	for (var<int> x = 0; x < MAP_W; x++)
	{
		Map.Table.push([]);

		for (var<int> y = 0; y < MAP_H; y++)
		{
			Map.Table[x].push(
			{
				Type: @@_CharToType(@@_LineToChars(lines[y])[x]),
			});
		}
	}

	// set StartPt
setStartPt:
	{
		for (var<int> x = 0; x < MAP_W; x++)
		for (var<int> y = 0; y < MAP_H; y++)
		{
			if (Map.Table[x][y].Type == MapCellType_e_Start)
			{
				Map.StartPt = CreateD2Point(
					FIELD_L + x * TILE_W + TILE_W / 2.0,
					FIELD_T + y * TILE_H + TILE_H / 2.0
					);

				break setStartPt;
			}
		}
		error();
	}

	// MapCell_t の Type 以外のフィールドを設定する。
	{
		for (var<int> x = 0; x < MAP_W; x++)
		for (var<int> y = 0; y < MAP_H; y++)
		{
			var<MapCell_t> cell = Map.Table[x][y];

			// reset
			{
				cell.WallFlag = false;
				cell.NarrowFlag = false;
			}
		}

		for (var<int> x = 0; x < MAP_W; x++)
		for (var<int> y = 0; y < MAP_H; y++)
		{
			var<MapCell_t> cell = Map.Table[x][y];

			if (
				cell.Type == MapCellType_e_Wall ||
				IsMapCellType_EnemyGreen(cell.Type)
				)
			{
				cell.WallFlag = true;
			}
		}

		for (var<int> x = 0; x < MAP_W; x++)
		for (var<int> y = 0; y < MAP_H; y++)
		{
			if (
				x + 2 < MAP_W &&
				Map.Table[x + 0][y].WallFlag &&
				Map.Table[x + 1][y].WallFlag == false &&
				Map.Table[x + 2][y].WallFlag
				)
			{
				Map.Table[x + 1][y].NarrowFlag = true;
			}

			if (
				y + 2 < MAP_H &&
				Map.Table[x][y + 0].WallFlag &&
				Map.Table[x][y + 1].WallFlag == false &&
				Map.Table[x][y + 2].WallFlag
				)
			{
				Map.Table[x][y + 1].NarrowFlag = true;
			}
		}
	}

	LoadEnemyOfMap();
}

function <string[]> @@_LineToChars(<stirng> line)
{
	var<string[]> dest = [];

	for (var<int> index = 0; index < line.length; index++)
	{
		if (index + 1 < line.length && "BRGC".indexOf(line[index]) != -1)
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

// 敵のロード
//
function <void> LoadEnemyOfMap()
{
	for (var<int> x = 0; x < MAP_W; x++)
	for (var<int> y = 0; y < MAP_H; y++)
	{
		var<MapCell_t> cell = Map.Table[x][y];

		var<double> dx = FIELD_L + x * TILE_W + TILE_W / 2.0;
		var<double> dy = FIELD_T + y * TILE_H + TILE_H / 2.0;

		switch (cell.Type)
		{
		case MapCellType_e_Enemy_BDummy: // テスト用
			GetEnemies().push(CreateEnemy_BDummy(dx, dy, 1));
			break;

		case MapCellType_e_Goal:
			GetEnemies().push(CreateEnemy_Goal(dx, dy));
			break;

		// 青敵
		//
		case MapCellType_e_Enemy_B1: GetEnemies().push(CreateEnemy_Blue(dx, dy, -1,  1)); break;
		case MapCellType_e_Enemy_B2: GetEnemies().push(CreateEnemy_Blue(dx, dy,  0,  1)); break;
		case MapCellType_e_Enemy_B3: GetEnemies().push(CreateEnemy_Blue(dx, dy,  1,  1)); break;
		case MapCellType_e_Enemy_B4: GetEnemies().push(CreateEnemy_Blue(dx, dy, -1,  0)); break;
		case MapCellType_e_Enemy_B6: GetEnemies().push(CreateEnemy_Blue(dx, dy,  1,  0)); break;
		case MapCellType_e_Enemy_B7: GetEnemies().push(CreateEnemy_Blue(dx, dy, -1, -1)); break;
		case MapCellType_e_Enemy_B8: GetEnemies().push(CreateEnemy_Blue(dx, dy,  0, -1)); break;
		case MapCellType_e_Enemy_B9: GetEnemies().push(CreateEnemy_Blue(dx, dy,  1, -1)); break;

		// 赤敵
		//
		case MapCellType_e_Enemy_R4: GetEnemies().push(CreateEnemy_Red(dx, dy, -1)); break;
		case MapCellType_e_Enemy_R6: GetEnemies().push(CreateEnemy_Red(dx, dy,  1)); break;

		// 緑敵, 時計回り
		//
		case MapCellType_e_Enemy_G1: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 3.0)); break;
		case MapCellType_e_Enemy_G2: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 2.0)); break;
		case MapCellType_e_Enemy_G3: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 1.0)); break;
		case MapCellType_e_Enemy_G4: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 4.0)); break;
		case MapCellType_e_Enemy_G6: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 8.0)); break;
		case MapCellType_e_Enemy_G7: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 5.0)); break;
		case MapCellType_e_Enemy_G8: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 6.0)); break;
		case MapCellType_e_Enemy_G9: GetEnemies().push(CreateEnemy_Green(dx, dy, 1, (Math.PI / 4.0) * 7.0)); break;

		// 緑敵, 反時計回り
		//
		case MapCellType_e_Enemy_G1_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 3.0)); break;
		case MapCellType_e_Enemy_G2_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 2.0)); break;
		case MapCellType_e_Enemy_G3_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 1.0)); break;
		case MapCellType_e_Enemy_G4_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 4.0)); break;
		case MapCellType_e_Enemy_G6_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 8.0)); break;
		case MapCellType_e_Enemy_G7_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 5.0)); break;
		case MapCellType_e_Enemy_G8_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 6.0)); break;
		case MapCellType_e_Enemy_G9_CCW: GetEnemies().push(CreateEnemy_Green(dx, dy, -1, (Math.PI / 4.0) * 7.0)); break;

		default:
			break;
		}
	}
}

function <MapCellType_e> @@_CharToType(<string> chr)
{
	if (chr == "■") return MapCellType_e_Wall;
	if (chr == "　") return MapCellType_e_None;
	if (chr == "始") return MapCellType_e_Start;
	if (chr == "終") return MapCellType_e_Goal;
	if (chr == "敵") return MapCellType_e_Enemy_BDummy;

	// 青敵
	//
	if (chr == "B1") return MapCellType_e_Enemy_B1;
	if (chr == "B2") return MapCellType_e_Enemy_B2;
	if (chr == "B3") return MapCellType_e_Enemy_B3;
	if (chr == "B4") return MapCellType_e_Enemy_B4;
	if (chr == "B6") return MapCellType_e_Enemy_B6;
	if (chr == "B7") return MapCellType_e_Enemy_B7;
	if (chr == "B8") return MapCellType_e_Enemy_B8;
	if (chr == "B9") return MapCellType_e_Enemy_B9;

	// 赤敵
	//
	if (chr == "R4") return MapCellType_e_Enemy_R4;
	if (chr == "R6") return MapCellType_e_Enemy_R6;

	// 緑敵, 時計回り
	//
	if (chr == "G1") return MapCellType_e_Enemy_G1;
	if (chr == "G2") return MapCellType_e_Enemy_G2;
	if (chr == "G3") return MapCellType_e_Enemy_G3;
	if (chr == "G4") return MapCellType_e_Enemy_G4;
	if (chr == "G6") return MapCellType_e_Enemy_G6;
	if (chr == "G7") return MapCellType_e_Enemy_G7;
	if (chr == "G8") return MapCellType_e_Enemy_G8;
	if (chr == "G9") return MapCellType_e_Enemy_G9;

	// 緑敵, 反時計回り
	//
	if (chr == "C1") return MapCellType_e_Enemy_G1_CCW;
	if (chr == "C2") return MapCellType_e_Enemy_G2_CCW;
	if (chr == "C3") return MapCellType_e_Enemy_G3_CCW;
	if (chr == "C4") return MapCellType_e_Enemy_G4_CCW;
	if (chr == "C6") return MapCellType_e_Enemy_G6_CCW;
	if (chr == "C7") return MapCellType_e_Enemy_G7_CCW;
	if (chr == "C8") return MapCellType_e_Enemy_G8_CCW;
	if (chr == "C9") return MapCellType_e_Enemy_G9_CCW;

	error();
}

var<MapCell_t> DEFAULT_MAP_CELL =
{
	Type: MapCellType_e_Wall,
	WallFlag: true,
};

function <MapCell_t> GetMapCell(<I2Point_t> pt)
{
	if (
		pt.X < 0 || MAP_W <= pt.X ||
		pt.Y < 0 || MAP_H <= pt.Y
		)
	{
		return DEFAULT_MAP_CELL;
	}

	return Map.Table[pt.X][pt.Y];
}

function <boolean> IsMapCellType_EnemyGreen(<MapCellType_e> type)
{
	var ret =
		type == MapCellType_e_Enemy_G1 ||
		type == MapCellType_e_Enemy_G2 ||
		type == MapCellType_e_Enemy_G3 ||
		type == MapCellType_e_Enemy_G4 ||
		type == MapCellType_e_Enemy_G6 ||
		type == MapCellType_e_Enemy_G7 ||
		type == MapCellType_e_Enemy_G8 ||
		type == MapCellType_e_Enemy_G9 ||
		type == MapCellType_e_Enemy_G1_CCW ||
		type == MapCellType_e_Enemy_G2_CCW ||
		type == MapCellType_e_Enemy_G3_CCW ||
		type == MapCellType_e_Enemy_G4_CCW ||
		type == MapCellType_e_Enemy_G6_CCW ||
		type == MapCellType_e_Enemy_G7_CCW ||
		type == MapCellType_e_Enemy_G8_CCW ||
		type == MapCellType_e_Enemy_G9_CCW;

	return ret;
}
