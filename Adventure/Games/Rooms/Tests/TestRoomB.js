/*
	ƒeƒXƒgEƒ‹[ƒ€ (šƒTƒ“ƒvƒ‹‚Æ‚µ‚ÄƒL[ƒv)
*/

var @@_”wŒiTable =
[
	P_”wŒi_©‘î,
	P_”wŒi_©º,
	P_”wŒi_©‘îƒgƒCƒŒ,
	P_”wŒi_ŠwZ˜L‰º,
	P_”wŒi_XˆßºA,
	P_”wŒi_XˆßºB,
	P_”wŒi_Z–å,
	P_”wŒi_‘ÌˆçŠÙ,
	P_”wŒi_Œö‰€,
];

var @@_”wŒiList = P_”wŒi_©‘î;

var @@_ŠÔ‘Ñ = 0; // 0`3: { “ú’†, —[•û, –é(“_“”), –é(Á“”) }

function* TestRoomB()
{
	EnterRoom();

	for (; ; )
	{
		// •”‰®ŒÅ—L‚Ìˆ—‚±‚±‚©‚ç

		if (GetMouseDown() == -1)
		{
			if (GetMouseX() < Screen_W / 2)
			{
				var i = @@_”wŒiTable.indexOf(@@_”wŒiList);
				i = (i + 1) % @@_”wŒiTable.length;
				@@_”wŒiList = @@_”wŒiTable[i];
			}
			else
			{
				@@_ŠÔ‘Ñ = (@@_ŠÔ‘Ñ + 1) % 4;
			}
		}

		// •”‰®ŒÅ—L‚Ìˆ—‚±‚±‚Ü‚Å

		StartDrawRoom();

		// •”‰®ŒÅ—L‚Ì•`‰æ‚±‚±‚©‚ç

		Draw(@@_”wŒiList[@@_ŠÔ‘Ñ], Screen_W / 2, Screen_H / 2, 1, 0, 1);

		SetColor("#ffffff");
		SetPrint(50, 60, 50);
		SetSize(16);
		PrintLine("TestRoom2 -- Click Left -> Move, Right -> Time");

		// •”‰®ŒÅ—L‚Ì•`‰æ‚±‚±‚Ü‚Å

		EndDrawRoom();

		yield 1;
	}
	LeaveRoom();
}
