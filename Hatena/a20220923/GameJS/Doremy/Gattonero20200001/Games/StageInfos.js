/*
	ステージ情報
*/

var<Sound_t[]> @@_MusicList =
[
	M_Field,
	M_Field,
	M_Field,
	M_Field,
	M_Boss,
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
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_SimpleDouble(P_Wall0001B, P_Wall0001F),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0003),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0003),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0003),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0003),
];

function <Wall_t> GetStageWall(<int> mapIndex)
{
	return @@_WallCreatorList[mapIndex]();
}
