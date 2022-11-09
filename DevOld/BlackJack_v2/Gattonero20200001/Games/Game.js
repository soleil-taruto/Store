/*
	ゲーム・メイン
*/

// カメラ位置(整数)
var<D2Point_t> Camera = CreateD2Point(0.0, 0.0);

// ゲーム用タスク
var<TaskManager_t> GameTasks = CreateTaskManager();

var<int> @@_Credit = 1000;
var<int> @@_Bet = 0;

function* <generatorForTask> GameMain()
{
	FreezeInput();
	ClearAllActor();
	ClearAllTask(GameTasks);

	for (; ; )
	{
		@@_Bet = 0;

		yield* @@_BetMain();

		// ----

		@@_DrawBattleBackground = Supplier(@@_E_DrawBattleBackground());

		for (var<Scene_t> scene of CreateScene(20))
		{
			if (!@@_DrawBattleBackground())
			{
				error();
			}
			yield 1;
		}

		// ----

		@@_DealerDamage = 0;

		for (; ; )
		{
			yield* @@_PlayerTurnMain();
			yield* @@_DealerTurnMain();
			yield* @@_BattleResultMain();

			if (@@_DealerDamage < 0)
			{
				break;
			}
			else if (@@_DEALER_DAMAGE_MAX <= @@_DealerDamage)
			{
				yield* @@_GohoubiMain();

				break;
			}
		}
	}
	FreezeInput();
	ClearAllActor();
	ClearAllTask(GameTasks);
}

function* <generatorForTask> @@_BetMain()
{
	FreezeInput();

	var<double> betUpZure = 0.0;
	var<double> betDownZure = 0.0;
	var<double> startBuru = 0.0;

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			var<int> INC_SPAN = 10;

			if (HoveredPicture == P_ButtonBetUp)
			{
				var<int> inc = Math.min(INC_SPAN, @@_Credit);

				@@_Credit -= INC_SPAN;
				@@_Bet += INC_SPAN;

				betUpZure = 10.0;
			}
			if (HoveredPicture == P_ButtonBetDown)
			{
				var<int> inc = Math.min(INC_SPAN, @@_Bet);

				@@_Credit += inc;
				@@_Bet -= inc;

				betDownZure = 10.0;
			}
			if (HoveredPicture == P_ButtonStart)
			{
				if (1 <= @@_Bet)
				{
					break;
				}
				else
				{
					startBuru = 50.0;
				}
			}
		}

		betUpZure = Approach(betUpZure, 0.0, 0.9);
		betDownZure = Approach(betDownZure, 0.0, 0.9);
		startBuru = Approach(startBuru, 0.0, 0.9);

		// 描画ここから

		SetColor("#404080");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#ffffff");
		SetFSize(120);
		SetPrint(100, 500, 300);
		PrintLine("CREDIT: " + @@_Credit);
		PrintLine("BET: " + @@_Bet);

		Draw(P_ButtonBetUp, 200, 1500 + betUpZure, 1.0, 0.0, 1.0);
		Draw(P_ButtonBetDown, 600, 1500 + betDownZure, 1.0, 0.0, 1.0);
		Draw(P_ButtonStart, 1000 + GetRand2() * startBuru, 1500 + GetRand2() * startBuru, 1.0, 0.0, 1.0);

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	for (var<Scene_t> scene of CreateScene(20))
	{
		SetColor("#404080");
		PrintRect(0, 0, Screen_W, Screen_H);

		DrawCurtain(-scene.Rate);

		yield 1;
	}
	FreezeInput();
}

var<int> @@_CARD_X = 200;
var<int> @@_CARD_X_STEP = 130;

var<int> @@_DEALER_CARD_Y = 500;
var<int> @@_PLAYER_CARD_Y = 1000;
var<int> @@_CARD_Y_STEP = 20;

var<int> @@_DEALER_SCORE_Y = 300;
var<int> @@_PLAYER_SCORE_Y = 1400;
var<int> @@_SCORE_FONT_SIZE = 100;

var<Func boolean> @@_DrawBattleBackground = () => true;

var<Actor[]> @@_StockCards = [];
var<Actor[]> @@_DealerCards = [];
var<Actor[]> @@_PlayerCards = [];

var<int> @@_DEALER_DAMAGE_MAX = 14;

/*
	ディーラーのダメージ
	初期値：0
	値域：-1 ～ @@_DEALER_DAMAGE_MAX
*/
var<int> @@_DealerDamage = 0;

function* <generatorForTask> @@_PlayerTurnMain()
{
	FreezeInput();

	@@_StockCards = @@_GetAllCard();

	Shuffle(@@_StockCards);

	@@_DealerCards = [];
	@@_PlayerCards = [];

	@@_DealerCards.push(@@_StockCards.pop());
	@@_DealerCards.push(@@_StockCards.pop());

	@@_PlayerCards.push(@@_StockCards.pop());
	@@_PlayerCards.push(@@_StockCards.pop());

	for (var<Actor_t> card of @@_DealerCards.concat(@@_PlayerCards))
	{
		SetTrumpPos(card, Screen_W + 300, -300);
		SetTrumpAutoStRot(card);
		AddActor(card);
	}
	for (var<int> index = 0; index < @@_DealerCards.length; index++)
	{
		AddDelay(GameTasks, 0 + 10 * index, () => SetTrumpDest(@@_DealerCards[index], @@_CARD_X + @@_CARD_X_STEP * index, @@_DEALER_CARD_Y + @@_CARD_Y_STEP * index));
	}
	for (var<int> index = 0; index < @@_PlayerCards.length; index++)
	{
		AddDelay(GameTasks, 5 + 10 * index, () => SetTrumpDest(@@_PlayerCards[index], @@_CARD_X + @@_CARD_X_STEP * index, @@_PLAYER_CARD_Y + @@_CARD_Y_STEP * index));
	}

	AddDelay(GameTasks, 30, () => SetTrumpReversed(@@_DealerCards[0], false));
	AddDelay(GameTasks, 50, () => SetTrumpReversed(@@_PlayerCards[0], false));
	AddDelay(GameTasks, 70, () => SetTrumpReversed(@@_PlayerCards[1], false));

	for (; ; )
	{
		var<int[]> cardsScores = @@_GetCardsScores(@@_PlayerCards);

		if (cardsScores.length == 0) // ? Burst
		{
			break;
		}

		if (GetMouseDown() == -1)
		{
			if (HoveredPicture == P_ButtonStand)
			{
				break;
			}
			if (HoveredPicture == P_ButtonHit)
			{
				var<int> count  = @@_PlayerCards.length;
				var<Actor_t> card = @@_StockCards.pop();

				@@_PlayerCards.push(card);

				SetTrumpPos(card, Screen_W + 300, -300);
				SetTrumpDest(card, @@_CARD_X + @@_CARD_X_STEP * count, @@_PLAYER_CARD_Y + @@_CARD_Y_STEP * count);
				SetTrumpAutoStRot(card);
				AddActor(card);
				AddDelay(GameTasks, 50, () => SetTrumpReversed(card, false));
			}

			if (GetMouseY() < 200) // ★★★ デバッグ用 ★★★
			{
				@@_DealerDamage = @@_DEALER_DAMAGE_MAX - 1;
			}
		}

		// 描画ここから

		if (!@@_DrawBattleBackground())
		{
			error();
		}
		@@_DrawHeader();

		var<string> strCardsScores = cardsScores.join("/");

		SetColor("#ffffff");
		SetFSize(@@_SCORE_FONT_SIZE);
		SetPrint(ToFix((Screen_W - GetPrintLineWidth(strCardsScores)) / 2), @@_PLAYER_SCORE_Y);
		PrintLine(strCardsScores);

		Draw(P_ButtonStand, 200, 1500, 1.0, 0.0, 1.0);
		Draw(P_ButtonHit, 1000, 1500, 1.0, 0.0, 1.0);

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_DealerTurnMain()
{
	FreezeInput();

	SetTrumpReversed(@@_DealerCards[1], false);

	for (var<int> frame = 0; ; frame++)
	{
		var<int[]> dealerCardsScores = @@_GetCardsScores(@@_DealerCards);
		var<int[]> playerCardsScores = @@_GetCardsScores(@@_PlayerCards);

		if (dealerCardsScores.length == 0) // ? Burst
		{
			break;
		}
		if (17 <= dealerCardsScores[0]) // HACK: ディーラーのヒット継続条件が不明？？？
		{
			break;
		}

		if (1 <= frame && frame % 60 == 0) // ディーラーのヒット
		{
			var<int> count  = @@_DealerCards.length;
			var<Actor_t> card = @@_StockCards.pop();

			@@_DealerCards.push(card);

			SetTrumpPos(card, Screen_W + 300, -300);
			SetTrumpDest(card, @@_CARD_X + @@_CARD_X_STEP * count, @@_DEALER_CARD_Y + @@_CARD_Y_STEP * count);
			SetTrumpAutoStRot(card);
			AddActor(card);
			AddDelay(GameTasks, 45, () => SetTrumpReversed(card, false));
		}

		// 描画ここから

		if (!@@_DrawBattleBackground())
		{
			error();
		}
		@@_DrawHeader();

		// ディーラーのスコア
		{
			var<int[]> cardsScores = dealerCardsScores;

			var<string> strCardsScores;

			if (1 <= cardsScores.length)
			{
				strCardsScores = cardsScores.join("/");
			}
			else
			{
				strCardsScores = "BURST";
			}

			SetColor("#ffffff");
			SetFSize(@@_SCORE_FONT_SIZE);
			SetPrint(ToFix((Screen_W - GetPrintLineWidth(strCardsScores)) / 2), @@_DEALER_SCORE_Y);
			PrintLine(strCardsScores);
		}

		// プレイヤーのスコア
		{
			var<int[]> cardsScores = playerCardsScores;

			var<string> strCardsScores;
			var<string> strCardsScoresColor;

			if (1 <= cardsScores.length)
			{
				strCardsScores = cardsScores.join("/");
				strCardsScoresColor = "#00ffff";
			}
			else
			{
				strCardsScores = "BURST";
				strCardsScoresColor = "#ff0000";
			}

			SetColor(strCardsScoresColor);
			SetFSize(@@_SCORE_FONT_SIZE);
			SetPrint(ToFix((Screen_W - GetPrintLineWidth(strCardsScores)) / 2), @@_PLAYER_SCORE_Y);
			PrintLine(strCardsScores);
		}

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_BattleResultMain()
{
	FreezeInput();

	var<int[]> dealerCardsScores = @@_GetCardsScores(@@_DealerCards);
	var<int[]> playerCardsScores = @@_GetCardsScores(@@_PlayerCards);

	/*
		勝者
		0 == ドロー
		1 == ディーラーの勝利
		2 == プレイヤーの勝利
	*/
	var<int> winner;

	if (dealerCardsScores.length == 0 && playerCardsScores.length == 0)
	{
		winner = 0;
	}
	else if (dealerCardsScores.length == 0)
	{
		winner = 2;
	}
	else if (playerCardsScores.length == 0)
	{
		winner = 1;
	}
	else
	{
		var<int> dealerScore = dealerCardsScores[dealerCardsScores.length - 1];
		var<int> playerScore = playerCardsScores[playerCardsScores.length - 1];

		if (dealerScore == playerScore)
		{
			winner = 0;
		}
		else if (dealerScore < playerScore)
		{
			winner = 2;
		}
		else
		{
			winner = 1;
		}
	}

	var<int> beforeDealerDamage = @@_DealerDamage;

	switch (winner)
	{
	case 0: break;
	case 1: @@_DealerDamage--; break;
	case 2: @@_DealerDamage++; break;

	default:
		error(); // never
	}

	{
		var<int> i = -1;

		if (winner == 1 && 0 <= @@_DealerDamage)
		{
			i = @@_DealerDamage;
		}
		else if (winner == 2)
		{
			i = beforeDealerDamage;
		}

		if (i != -1)
		{
			var<int> x = i % 7;
			var<int> y = ToFix(i / 7);

			AddEffect_Explode(870 + x * 40, 70 + y * 40);
		}
	}

	{
		var<string> strResult;
		var<I3Color_t> backColor;

		switch (winner)
		{
		case 0:
			strResult = "DRAW";
			backColor = CreateI3Color(80, 80, 80);
			break;

		case 1:
			strResult = "YOU LOSE";
			backColor = CreateI3Color(0, 160, 160);
			break;

		case 2:
			strResult = "YOU WIN";
			backColor = CreateI3Color(160, 160, 0);
			break;

		default:
			error(); // never
		}

		Effect_BattleResult_DeadFlag = false;
		AddEffect(Effect_BattleResult(strResult, backColor));
	}

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			if (HoveredPicture == P_ButtonNext)
			{
				break;
			}
		}

		// 描画ここから

		if (!@@_DrawBattleBackground())
		{
			error();
		}
		@@_DrawHeader();

		// ディーラーのスコア
		{
			var<int[]> cardsScores = dealerCardsScores;

			var<string> strCardsScores;
			var<string> strCardsScoresColor;

			if (1 <= cardsScores.length)
			{
//				strCardsScores = cardsScores.join("/");
				strCardsScores = cardsScores[cardsScores.length - 1] + "";
			}
			else
			{
				strCardsScores = "BURST";
			}

			switch (winner)
			{
			case 0: strCardsScoresColor = "#a0a0a0"; break;
			case 1: strCardsScoresColor = "#ffa000"; break;
			case 2: strCardsScoresColor = "#00a0ff"; break;

			default:
				error(); // never
			}

			SetColor(strCardsScoresColor);
			SetFSize(@@_SCORE_FONT_SIZE);
			SetPrint(ToFix((Screen_W - GetPrintLineWidth(strCardsScores)) / 2), @@_DEALER_SCORE_Y);
			PrintLine(strCardsScores);
		}

		// プレイヤーのスコア
		{
			var<int[]> cardsScores = playerCardsScores;

			var<string> strCardsScores;
			var<string> strCardsScoresColor;

			if (1 <= cardsScores.length)
			{
//				strCardsScores = cardsScores.join("/");
				strCardsScores = cardsScores[cardsScores.length - 1] + "";
			}
			else
			{
				strCardsScores = "BURST";
			}

			switch (winner)
			{
			case 0: strCardsScoresColor = "#a0a0a0"; break;
			case 1: strCardsScoresColor = "#00a0ff"; break;
			case 2: strCardsScoresColor = "#ffa000"; break;

			default:
				error(); // never
			}

			SetColor(strCardsScoresColor);
			SetFSize(@@_SCORE_FONT_SIZE);
			SetPrint(ToFix((Screen_W - GetPrintLineWidth(strCardsScores)) / 2), @@_PLAYER_SCORE_Y);
			PrintLine(strCardsScores);
		}

		Draw(P_ButtonNext, 600, 1500, 1.0, 0.0, 1.0);

		ExecuteAllActor();
		ExecuteAllTask(GameTasks);
		yield 1;
	}
	FreezeInput();

	@@_ExeuntCards(@@_DealerCards);
	@@_ExeuntCards(@@_PlayerCards);

	@@_DealerCards = [];
	@@_PlayerCards = [];

	Effect_BattleResult_DeadFlag = true;
}

function* <generatorForTask> @@_E_DrawBattleBackground()
{
	var<double> x = Screen_W;
	var<double> a = 0.0;
	var<double> z = 3.0;

	var<double> xDest = 0.0;
	var<double> aDest = 0.25;
	var<double> zDest = 1.6;

	var<double> zureRate = 1.0;

	for (; ; )
	{
		x = Approach(x, xDest, 0.9999);
		a = Approach(a, aDest, 0.93);
		z = Approach(z, zDest, 0.95);

		zureRate = Approach(zureRate, 0.0, 0.94);

		SetColor("#000000");
		PrintRect(0, 0, Screen_W, Screen_H);

		Draw(P_BattleBackground, x, Screen_H / 2.0, a, 0.0, z);

		if (MICRO < zureRate)
		{
			var<double> zure = zureRate * 300.0;

			Draw(P_BattleBackground, x - zure, Screen_H / 2.0 - zure, 0.75 * zureRate, 0.0, z);
			Draw(P_BattleBackground, x + zure, Screen_H / 2.0 + zure, 0.75 * zureRate, 0.0, z);
		}
		yield 1;
	}
}

function <Actor_t[]> @@_GetAllCard()
{
	var<Actor_t[]> dest = [];

	for (var<Suit_e> suit of [ Suit_e_SPADE, Suit_e_HEART, Suit_e_DIA, Suit_e_CLUB ])
	for (var<int> number = 1; number <= 13; number++)
	{
		dest.push(CreateActor_Trump(-300, -300, suit, number, true));
	}
	return dest;
}

function <int[]> @@_GetCardsScores(<Actor_t[]> cards)
{
	var<int> score = 0;
	var<int> ace = 0;

	for (var<Actor_t> card of cards)
	{
		var<int> n = card.Number;

		if (n == 1)
		{
			ace++;
		}
		n = Math.min(n, 10);

		score += n;
	}
	var<int[]> dest = [];

	dest.push(score);

	for (var<int> i = 0; i < ace; i++)
	{
		dest.push(score + 10 * (i + 1));
	}
	while (1 <= dest.length && 21 < dest[dest.length - 1])
	{
		dest.pop();
	}
	return dest;
}

function <void> @@_ExeuntCards(<Actor_t[]> cards)
{
	var<int> delayFrame = 0;

	for (var<Actor> card of cards)
	{
		delayFrame += 5;

		AddDelay(GameTasks, delayFrame, () => SetTrumpDest(card, -300, card.Y));
		AddDelay(GameTasks, delayFrame + 100, () => KillActor(card));
	}
}

function <void> @@_DrawHeader()
{
	var<string> meter = "";

	for (var<int> i = 0; i < @@_DEALER_DAMAGE_MAX; i++)
	{
		meter += i < @@_DealerDamage ? "■" : "□";
	}

	SetColor("#40408080");
	PrintRect(50, 50, Screen_W - 100, 120);

	SetColor("#ffffff");
	SetFSize(40);
	SetPrint(70, 100, 50);
	PrintLine("CREDIT: " + @@_Credit);
	PrintLine("BET: " + @@_Bet);

	SetColor("#ffffff");
	SetFSize(40);
	SetPrint(620, 100, 50);
	PrintLine("ディーラー");

	SetColor("#ffffff");
	SetFSize(40);
	SetPrint(850, 100, 50);
	PrintLine(meter.substring(0, 7));
	PrintLine(meter.substring(7));
}

function* <generatorForTask> @@_GohoubiMain()
{
	for (var<Scene_t> scene of CreateScene(30))
	{
		if (!@@_DrawBattleBackground())
		{
			error();
		}

		Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.Rate, scene.RemRate * -0.2, 1.2);
		Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.Rate, scene.RemRate *  0.2, 1.2);

		yield 1;
	}

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			if (HoveredPicture == P_ButtonContinue)
			{
				break;
			}
			if (HoveredPicture == P_ButtonX)
			{
				FreezeInput();

				for (; ; )
				{
					if (GetMouseDown() == -1)
					{
						break;
					}
					Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.2);
					yield 1;
				}
				FreezeInput();
			}
		}

		Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.2);

		Draw(P_ButtonContinue, 200, 1500, 1.0, 0.0, 1.0);
		Draw(P_ButtonX, 1000, 1500, 1.0, 0.0, 1.0);

		yield 1;
	}

	for (var<Scene_t> scene of CreateScene(30))
	{
		if (!@@_DrawBattleBackground())
		{
			error();
		}

		Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.RemRate, scene.Rate * -0.3, 1.2);
		Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.RemRate, scene.Rate *  0.3, 1.2);

		yield 1;
	}
}
