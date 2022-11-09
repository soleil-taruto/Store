/*
	ステージ情報
*/

var<Sound_t[]> @@_MusicList =
[
	M_Field,
	M_Field,
	M_Field,
	M_Boss,
	M_Field,
	M_Field,
	M_Field,
	M_Field,
	M_Field,
	M_Field,
];

function <void> PlayStageMusic(<int> mapIndex)
{
	Play(@@_MusicList[mapIndex]);
}

var<Func Wall_t> @@_WallCreatorList =
[
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
	() => CreateWall_None(),
];

function <Wall_t> GetStageWall(<int> mapIndex)
{
	return @@_WallCreatorList[mapIndex]();
}

var<Func Tile_t> @@_DefaultTileCreatorList =
[
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
	() => CreateTile_Grass(),
];

function <Tile_t> GetDefaultTile(<int> mapIndex)
{
	return @@_DefaultTileCreatorList[mapIndex]();
}
