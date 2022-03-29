/*
	画像
*/

function @@_Load(url)
{
	LOGPOS;
	Loading++;

	var image = new Image();

	image.src = url;
	image.onload = function()
	{
		LOGPOS;
		Loading--;
	};

	image.onerror = function()
	{
		error;
	};

	return image;
}

// ここから各種画像

// プリフィクス
// P_ ... 画像

var P_Dummy = @@_Load(Resources.General__Dummy_png); // ★サンプルとしてキープ

// 背景は [ 日中, 夕方, 夜(点灯), 夜(消灯) ] の順である。

var P_背景_自宅 =
[
	@@_Load(Resources.消失点__house_enterance_interior_a_jpg),
	@@_Load(Resources.消失点__house_enterance_interior_b_jpg),
	@@_Load(Resources.消失点__house_enterance_interior_c_jpg),
	@@_Load(Resources.消失点__house_enterance_interior_d_jpg),
];
var P_背景_自室 =
[
	@@_Load(Resources.消失点__house_room_weekly_apartment_a_jpg),
	@@_Load(Resources.消失点__house_room_weekly_apartment_b_jpg),
	@@_Load(Resources.消失点__house_room_weekly_apartment_c_jpg),
	@@_Load(Resources.消失点__house_room_weekly_apartment_d_jpg),
];
var P_背景_自宅トイレ =
[
	@@_Load(Resources.消失点__house_toilet_a_jpg),
	@@_Load(Resources.消失点__house_toilet_b_jpg),
	@@_Load(Resources.消失点__house_toilet_c_jpg),
	@@_Load(Resources.消失点__house_toilet_d_jpg),
];
var P_背景_学校廊下 =
[
	@@_Load(Resources.消失点__school_corridor_a_jpg),
	@@_Load(Resources.消失点__school_corridor_b_jpg),
	@@_Load(Resources.消失点__school_corridor_c_jpg),
	@@_Load(Resources.消失点__school_corridor_d_jpg),
];

var @@_更衣室A_点灯 = @@_Load(Resources.消失点__school_dressing_room_a_jpg); // ベンチ有り_点灯
var @@_更衣室A_消灯 = @@_Load(Resources.消失点__school_dressing_room_b_jpg); // ベンチ有り_消灯
var @@_更衣室B_点灯 = @@_Load(Resources.消失点__school_dressing_room_c_jpg); // ベンチ無し_点灯
var @@_更衣室B_消灯 = @@_Load(Resources.消失点__school_dressing_room_d_jpg); // ベンチ無し_消灯

var P_背景_更衣室A = // ベンチ有り
[
	@@_更衣室A_点灯,
	@@_更衣室A_点灯,
	@@_更衣室A_点灯,
	@@_更衣室A_消灯,
];
var P_背景_更衣室B = // ベンチ無し
[
	@@_更衣室B_点灯,
	@@_更衣室B_点灯,
	@@_更衣室B_点灯,
	@@_更衣室B_消灯,
];
var P_背景_校門 =
[
	@@_Load(Resources.消失点__school_gate_a_jpg),
	@@_Load(Resources.消失点__school_gate_b_jpg),
	@@_Load(Resources.消失点__school_gate_c_jpg),
	@@_Load(Resources.消失点__school_gate_d_jpg),
];
var P_背景_体育館 =
[
	@@_Load(Resources.消失点__school_gym_a_jpg),
	@@_Load(Resources.消失点__school_gym_b_jpg),
	@@_Load(Resources.消失点__school_gym_c_jpg),
	@@_Load(Resources.消失点__school_gym_d_jpg),
];
var P_背景_公園 =
[
	@@_Load(Resources.消失点__town_park_a_jpg),
	@@_Load(Resources.消失点__town_park_b_jpg),
	@@_Load(Resources.消失点__town_park_c_jpg),
	@@_Load(Resources.消失点__town_park_d_jpg),
];
