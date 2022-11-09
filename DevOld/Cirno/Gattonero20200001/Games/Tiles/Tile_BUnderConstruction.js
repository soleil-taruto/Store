/*
	タイル - BUnderConstruction ★サンプル
*/

var<int> TileKind_BUnderConstruction = @(AUTO);

function <Tile_t> CreateTile_BUnderConstruction()
{
	var ret =
	{
		Kind: TileKind_BUnderConstruction,
		WallFlag: true,

		// ここから固有
	};

	ret.Draw = @@_Draw;

	return ret;
}

function <void> @@_Draw(<Tile_t> tile, <double> dx, <double> dy)
{
	var<int> ix = ToInt(dx);
	var<int> iy = ToInt(dy);

	AddTask(FrontTasks, function* <generatorForTask> ()
	{
		SetColor(I4ColorToString(CreateI4Color(255, 128, 0, 128)));
		PrintRect(ix, iy, 380, 50);

		SetPrint(ix + 10 , iy + 36);
		SetFSize(30);
		SetColor(I3ColorToString(ToFix(ProcFrame / 30) % 2 == 0 ? CreateI3Color(255, 255, 0) : CreateI3Color(64, 64, 64)));
		PrintLine("UNDER CONSTRUCTION");
	}());
}
