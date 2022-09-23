/*
	音楽
*/

function <Sound_t> @@_Load(<string> url)
{
	return LoadSound(url);
}

// ここから各種音楽

// プリフィクス
// M_ ... 音楽

//var<Sound_t> M_無音 = @@_Load(RESOURCE_General__muon_mp3); // デカいのでロードしない。

// ★ここまで固定 -- 持ち回り_共通 -- サンプルとしてキープ

var<Sound_t> M_Title = @@_Load(RESOURCE_DovaSyndrome__Hanabi_mp3);
var<Sound_t> M_Ending      = @@_Load(RESOURCE_HMIX__n118_mp3);
var<Sound_t> M_Stage01     = @@_Load(RESOURCE_HMIX__n138_mp3);
var<Sound_t> M_Stage02     = @@_Load(RESOURCE_HMIX__n70_mp3);
var<Sound_t> M_Stage03     = @@_Load(RESOURCE_HMIX__n13_mp3);
var<Sound_t> M_Stage01Boss = @@_Load(RESOURCE_ユーフルカ__Battle_Vampire_loop_m4a);
var<Sound_t> M_Stage02Boss = @@_Load(RESOURCE_ユーフルカ__Battle_Conflict_loop_m4a);
var<Sound_t> M_Stage03Boss = @@_Load(RESOURCE_ユーフルカ__Battle_rapier_loop_m4a);
