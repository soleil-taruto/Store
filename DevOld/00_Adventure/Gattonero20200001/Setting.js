/*
	設定
*/

function* <generatorForTask> SettingMain()
{
	var<int> selectIndex = 0;

	FreezeInput();

gameLoop:
	for (; ; )
	{
		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(70, 130, 50);
		SetFSize(40);
		PrintLine("設定");

		selectIndex = DrawSimpleMenu(
			selectIndex,
			70,
			160,
			600,
			30,
			[
				"音楽の音量",
				"効果音の音量",
				"データの消去",
				"戻る",
			]);

		if (DSM_Desided)
		switch (selectIndex)
		{
		case 0:
			yield* @@_VolumeSetting("音楽", MusicVolume, function <void> (<double> volume)
			{
				MusicVolume = volume;
				MusicVolumeChanged();
				SaveLocalStorage();
			});
			break;

		case 1:
			yield* @@_VolumeSetting("効果音", SEVolume, function <void> (<double> volume)
			{
				SEVolume = volume;
				SaveLocalStorage();
			});
			break;

		case 2:
			yield* @@_RemoveSaveData();
			break;

		case 3:
			break gameLoop;
		}
		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_VolumeSetting(<string> name, <double> initVolume, <Action double> volumeChanged)
{
	FreezeInput();

	var<int> volume = ToInt(initVolume * 100.0);

	var<int> METER_L = 100;
	var<int> METER_T = 270;
	var<int> METER_W = Screen_W - 200;
	var<int> METER_H = 60;
	var<int> TSUMAMI_W = 50;

	var<boolean> tsumanderu = false;

	for (var<int> frame = 0; ; frame++)
	{
		if (frame % 60 == 0)
		{
			SE(ChooseOne(S_テスト用));
		}

		if (GetKeyInput(32) == 1)
		{
			break;
		}
		if (GetMouseDown() == -1)
		{
			if (!tsumanderu)
			{
				break;
			}
			tsumanderu = false;
		}
		if (GetMouseDown() == 1)
		{
			if (METER_T < GetMouseY() && GetMouseY() < METER_T + METER_H)
			{
				tsumanderu = true;
			}
		}

		var<boolean> changed = false;

		if (tsumanderu)
		{
			volume = ToInt(ToRange(RateAToB(METER_L + TSUMAMI_W / 2, METER_L + METER_W - TSUMAMI_W / 2, GetMouseX()), 0.0, 1.0) * 100.0);
			changed = true;
		}
		if (IsPound(GetInput_2()))
		{
			volume -= 10;
			changed = true;
		}
		if (IsPound(GetInput_4()))
		{
			volume--;
			changed = true;
		}
		if (IsPound(GetInput_6()))
		{
			volume++;
			changed = true;
		}
		if (IsPound(GetInput_8()))
		{
			volume += 10;
			changed = true;
		}
		if (GetInput_B() == 1)
		{
			volume = DEFAULT_VOLUME * 100.0;
			changed = true;
		}
		if (GetInput_A() == 1)
		{
			break;
		}

		if (changed)
		{
			volume = ToRange(volume, 0, 100);
			volumeChanged(volume / 100.0);
		}

		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#404040");
		PrintRect(METER_L, METER_T, METER_W, METER_H);

		SetColor("#808080");
		PrintRect(AToBRate(METER_L, METER_L + METER_W - TSUMAMI_W, volume / 100.0), METER_T, TSUMAMI_W, METER_H);

		SetColor("#000000");
		SetPrint(100, 400, 24);
		SetFSize(18);
		PrintLine(name + "の音量設定 ( 現在の音量 = " + volume + " )");
		PrintLine("上・右キー　⇒　音量を上げる");
		PrintLine("下・左キー　⇒　音量を上げる");
		PrintLine("Ｘキー　　　⇒　初期値に戻す");
		PrintLine("Ｚキー　　　⇒　戻る");
		PrintLine("メニューに戻るにはスペースキーまたは画面をクリックして下さい。");

		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_RemoveSaveData()
{
	var<int> selectIndex = 0;

	FreezeInput();

gameLoop:
	for (; ; )
	{
		SetColor("#a0b0c0");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetPrint(100, 220, 50);
		SetFSize(20);
		PrintLine("セーブデータを完全に消去します。宜しいですか？");

		selectIndex = DrawSimpleMenu(
			selectIndex,
			70,
			250,
			600,
			30,
			[
				"はい",
				"いいえ",
			]);

		if (DSM_Desided)
		switch (selectIndex)
		{
		case 0:
			ClearLocalStorageValue();
			LoadLocalStorage();
			SE(S_SaveDataRemoved);
			break gameLoop;

		case 1:
			break gameLoop;
		}
		yield 1;
	}
	FreezeInput();
}
