/*
	画像 - 水域タイル
*/

function <Picture_t> @@_Load(<string> url)
{
	return LoadPicture(url);
}

/*
	[x(0～1)][y(0～9)][koma(0～7)]
*/
var<Picture_t[][][]> P_Tile_Water =
[
	// [0][*]
	[
		// [0][0]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0000_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0200_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0400_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0600_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0800_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1000_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1200_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1400_png),
		],

		// [0][1]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0001_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0201_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0401_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0601_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0801_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1001_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1201_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1401_png),
		],

		// [0][2]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0002_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0202_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0402_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0602_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0802_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1002_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1202_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1402_png),
		],

		// [0][3]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0003_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0203_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0403_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0603_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0803_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1003_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1203_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1403_png),
		],

		// [0][4]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0004_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0204_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0404_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0604_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0804_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1004_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1204_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1404_png),
		],

		// [0][5]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0005_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0205_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0405_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0605_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0805_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1005_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1205_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1405_png),
		],

		// [0][6]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0006_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0206_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0406_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0606_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0806_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1006_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1206_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1406_png),
		],

		// [0][7]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0007_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0207_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0407_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0607_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0807_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1007_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1207_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1407_png),
		],

		// [0][8]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0008_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0208_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0408_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0608_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0808_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1008_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1208_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1408_png),
		],

		// [0][9]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0009_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0209_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0409_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0609_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0809_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1009_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1209_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1409_png),
		],
	],

	// [1][*]
	[
		// [1][0]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0100_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0300_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0500_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0700_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0900_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1100_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1300_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1500_png),
		],

		// [1][1]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0101_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0301_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0501_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0701_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0901_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1101_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1301_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1501_png),
		],

		// [1][2]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0102_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0302_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0502_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0702_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0902_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1102_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1302_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1502_png),
		],

		// [1][3]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0103_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0303_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0503_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0703_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0903_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1103_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1303_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1503_png),
		],

		// [1][4]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0104_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0304_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0504_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0704_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0904_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1104_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1304_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1504_png),
		],

		// [1][5]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0105_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0305_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0505_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0705_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0905_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1105_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1305_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1505_png),
		],

		// [1][6]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0106_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0306_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0506_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0706_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0906_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1106_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1306_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1506_png),
		],

		// [1][7]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0107_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0307_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0507_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0707_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0907_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1107_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1307_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1507_png),
		],

		// [1][8]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0108_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0308_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0508_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0708_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0908_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1108_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1308_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1508_png),
		],

		// [1][9]
		[
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0109_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0309_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0509_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0709_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_0909_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1109_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1309_png),
			@@_Load(RESOURCE_ぴぽや倉庫__Water__Tile_1509_png),
		],
	],
];
