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

var<Sound_t> M_Title  = @@_Load(RESOURCE_MusMus__MusMus_BGM_093_mp3);
var<Sound_t> M_Field  = @@_Load(RESOURCE_MusMus__tw050_mp3);
var<Sound_t> M_Ending = @@_Load(RESOURCE_MusMus__tw006_mp3);
