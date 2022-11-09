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

var<Picture_t[]> @@_WallPictureList =
[
	P_Wall0001,
	P_Wall0001,
	P_Wall0002,
	P_Wall0003,
	P_Wall0001,
	P_Wall0002,
	P_Wall0001,
	P_Wall0002,
	P_Wall0001,
	P_Wall0002,
];

function <Picture_t> GetStageWallPicture(<int> mapIndex)
{
	return @@_WallPictureList[mapIndex];
}
