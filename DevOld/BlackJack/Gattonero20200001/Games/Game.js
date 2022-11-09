/*
	ÉQÅ[ÉÄÅEÉÅÉCÉì
*/

var<TaskManager_t> GameTasks = CreateTaskManager();

var<Actor[]> @@_DealerCards = [];
var<Actor[]> @@_PlayerCards = [];

var<int> @@_BackgroundMode = 0;

var<double> @@_BUNNY_X_IN  = Screen_W - 200;
var<double> @@_BUNNY_X_OUT = Screen_W + 300;

var<double> @@_BunnyX     = @@_BUNNY_X_OUT;
var<double> @@_BunnyXDest = @@_BUNNY_X_OUT;

var<int> @@_Credit = 256;
var<int> @@_Bet = 10;

function* <generatorForTask> GameMain()
{
	FreezeInput();
	ClearAllActor();

	var<Func boolean> drawBackground = Supplier(@@_E_DrawBackground());

	for (; ; )
	{
		if (GetMouseDown() == -1)
		{
			var<D2Point_t> musPt = CreateD2Point(GetMouseX(), GetMouseY());

			var<D4Rect_t> upperArea  = CreateD4Rect(0,    0, Screen_W, 200);
			var<D4Rect_t> dealerArea = CreateD4Rect(0,  200, Screen_W, 600);
			var<D4Rect_t> playerArea = CreateD4Rect(0,  800, Screen_W, 600);
			var<D4Rect_t> underLArea = CreateD4Rect(0, 1400, Screen_W / 2, 200);
			var<D4Rect_t> underRArea = CreateD4Rect(Screen_W / 2, 1400, Screen_W / 2, 200);

			if (!IsOut(musPt, upperArea, 0.0))
			{
				@@_Credit *= 2;
				if (65536 < @@_Credit) { @@_Credit = 2; }

				@@_Bet += 10;
				if (200 < @@_Bet) { @@_Bet = 0; }
			}
			if (!IsOut(musPt, dealerArea, 0.0))
			{
				if (1 <= @@_DealerCards.length && IsTrumpReversed(@@_DealerCards[@@_DealerCards.length - 1]))
				{
					SetTrumpReversed(@@_DealerCards[@@_DealerCards.length - 1], false);
				}
				else if (@@_DealerCards.length < 6)
				{
					var<Suit_e> suit = ChooseOne(
					[
						Suit_e_SPADE,
						Suit_e_HEART,
						Suit_e_DIA,
						Suit_e_CLUB,
					]);
					var<int> number = GetRandRange(1, 13);
					var<Actor_t> card = CreateActor_Trump(Screen_W + 300, -300, suit, number, true);

					SetTrumpDest(
						card,
						200 + 60 * @@_DealerCards.length,
						430 + 30 * @@_DealerCards.length
						);

					AddActor(card);
					@@_DealerCards.push(card);
				}
				else
				{
					AddTask(GameTasks, @@_E_ClearCards(@@_DealerCards));
					@@_DealerCards = [];
				}
			}
			if (!IsOut(musPt, playerArea, 0.0))
			{
				if (1 <= @@_PlayerCards.length && IsTrumpReversed(@@_PlayerCards[@@_PlayerCards.length - 1]))
				{
					SetTrumpReversed(@@_PlayerCards[@@_PlayerCards.length - 1], false);
				}
				else if (@@_PlayerCards.length < 6)
				{
					var<Suit_e> suit = ChooseOne(
					[
						Suit_e_SPADE,
						Suit_e_HEART,
						Suit_e_DIA,
						Suit_e_CLUB,
					]);
					var<int> number = GetRandRange(1, 13);
					var<Actor_t> card = CreateActor_Trump(Screen_W + 300, -300, suit, number, true);

					SetTrumpDest(
						card,
						 200 + 60 * @@_PlayerCards.length,
						1030 + 30 * @@_PlayerCards.length
						);

					AddActor(card);
					@@_PlayerCards.push(card);
				}
				else
				{
					AddTask(GameTasks, @@_E_ClearCards(@@_PlayerCards));
					@@_PlayerCards = [];
				}
			}
			if (!IsOut(musPt, underLArea, 0.0))
			{
				@@_BackgroundMode = (@@_BackgroundMode + 1) % 4;
			}
			if (!IsOut(musPt, underRArea, 0.0))
			{
				AddTask(GameTasks, @@_E_ReverseAllCard(@@_DealerCards));
				AddTask(GameTasks, @@_E_ReverseAllCard(@@_PlayerCards));
			}
		}

		// ï`âÊÇ±Ç±Ç©ÇÁ

		SetColor("#404080");
		PrintRect(0, 0, Screen_W, Screen_H);
		SetColor("#505090");
		PrintRect(0,  200, Screen_W, 600);
		PrintRect(0, 1400, Screen_W, 200);

		if (!drawBackground())
		{
			error();
		}

		@@_BunnyX = Approach(@@_BunnyX, @@_BunnyXDest, 0.9);
		Draw(P_Bunny, @@_BunnyX, Screen_H - 350, 1.0, 0.0, 1.0);

		ExecuteAllTask(GameTasks);
		ExecuteAllActor();

		SetPrint(20, 80, 100);
		SetFSize(80);
		SetColor("#ffffff");
		PrintLine("CREDIT: " + @@_Credit);
		PrintLine("BET: " + @@_Bet);

		Draw(P_ButtonBetUp,    200, 1500, 1.0, 0.0, 1.0);
		Draw(P_ButtonBetDown,  600, 1500, 1.0, 0.0, 1.0);
		Draw(P_ButtonStart,   1000, 1500, 1.0, 0.0, 1.0);

		yield 1;
	}
	FreezeInput();
}

function* <generatorForTask> @@_E_ClearCards(<Actor[]> cards)
{
	cards = ToArray(cards); // íxâÑÇ∑ÇÈÇÃÇ≈îOÇÃÇΩÇﬂï°êªÇµÇƒÇ®Ç≠ÅB

	for (var<Actor> card of cards)
	{
		SetTrumpDest(card, -300, card.Y);

		yield* Repeat(1, 5);
	}
	yield* Repeat(1, 100); // ÉJÅ[ÉhÇ™âÊñ äOÇ…èoÇÈÇ‹Ç≈ë“Ç¬ÅB

	for (var<Actor> card of cards)
	{
		KillActor(card);
	}
}

function* <generatorForTask> @@_E_ReverseAllCard(<Actor[]> cards)
{
	cards = ToArray(cards); // íxâÑÇ∑ÇÈÇÃÇ≈îOÇÃÇΩÇﬂï°êªÇµÇƒÇ®Ç≠ÅB

	for (var<Actor> card of cards)
	{
		SetTrumpReversed(card, !IsTrumpReversed(card));

		yield* Repeat(1, 10);
	}
}

function* <generatorForTask> @@_E_DrawBackground()
{
	for (; ; )
	{
		yield 1; // 2bs

		while (@@_BackgroundMode == 0)
		{
			yield 1;
		}

		@@_BunnyXDest = @@_BUNNY_X_IN;

		while (@@_BackgroundMode == 1)
		{
			yield 1;
		}
		@@_BunnyXDest = @@_BUNNY_X_OUT;

		for (var<Scene_t> scene of CreateScene(30))
		{
			Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5, scene.RemRate * -0.2, 1.2);
			Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5, scene.RemRate *  0.2, 1.2);

			yield 1;
		}
		while (@@_BackgroundMode == 2)
		{
			Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.2);

			yield 1;
		}
		for (var<Scene_t> scene of CreateScene(30))
		{
			Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.RemRate, scene.Rate * -0.3, 1.2);
			Draw(P_Bunny_H_01, Screen_W / 2, Screen_H / 2, 0.5 * scene.RemRate, scene.Rate *  0.3, 1.2);

			yield 1;
		}

		for (var<Scene_t> scene of CreateScene(30))
		{
			Draw(P_Bunny_H_02, Screen_W / 2, Screen_H / 2, scene.Rate, 0.0, 1.2);

			yield 1;
		}
		while (@@_BackgroundMode == 3)
		{
			Draw(P_Bunny_H_02, Screen_W / 2, Screen_H / 2, 1.0, 0.0, 1.2);

			yield 1;
		}
		for (var<Scene_t> scene of CreateScene(30))
		{
			Draw(P_Bunny_H_02, Screen_W / 2, Screen_H / 2, scene.RemRate, 0.0, 1.2);

			yield 1;
		}
	}
}
