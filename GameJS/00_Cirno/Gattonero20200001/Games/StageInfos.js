/*
	ステージ情報

	各リスト長は (GetMapCount() + 1) であること。

	[0] == テスト用
	[1] == ステージ_01
	[2] == ステージ_02
	[3] == ステージ_03
	...
	[n] == ステージ_n
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
	() => CreateWall_Simple(P_Wall0001),
	() => CreateWall_Simple(P_Wall0001),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0003),
	() => CreateWall_Simple(P_Wall0001),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0001),
	() => CreateWall_Simple(P_Wall0002),
	() => CreateWall_Simple(P_Wall0001),
	() => CreateWall_Simple(P_Wall0002),
];

function <Wall_t> GetStageWall(<int> mapIndex)
{
	return @@_WallCreatorList[mapIndex]();
}
