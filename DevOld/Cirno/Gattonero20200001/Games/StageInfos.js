/*
	ステージ情報
*/

var<Audio[]> @@_MusicList =
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

var<Image[]> @@_WallPictureList =
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

function <Image> GetStageWallPicture(<int> mapIndex)
{
	return @@_WallPictureList[mapIndex];
}
