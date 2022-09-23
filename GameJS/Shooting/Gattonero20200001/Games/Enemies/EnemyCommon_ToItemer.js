/*
	敵共通 - 雑魚敵のアイテム所持化
*/

/*
	Enemy_t 追加フィールド
	{
		<EnemyItemType_e> @@_ItemType // アイテムの種類
	}
*/

/*
	雑魚敵のアイテム所持化

	使用例：
		var<Enemy_t> enemy = EnemyCommon_ToItemer(CreateEnemy_XXX());
		----
		var<Enemy_t> enemy = CreateEnemy_XXX();
		EnemyCommon_ToItemer(enemy);
*/
function <Enemy_t> EnemyCommon_ToItemer(<Enemy_t> enemy, <EnemyItemType_e> itemType)
{
	enemy.@@_ItemType = itemType;

	AddTask(GameTasks, @@_Each(enemy));

	return enemy;
}

/*
	タスク
*/
function* <generatorForTask> @@_Each(<Enemy_t> enemy)
{
	for (var<int> frame = 0; ; frame++)
	{
		if (enemy.HP == -1) // ? 既に死亡 -> 終了
		{
			break;
		}

		yield 1;
	}

	@@_DropItem(enemy);
}

/*
	アイテム投下_実行
*/
function <void> @@_DropItem(<Enemy_t> enemy)
{
	var<double> T_MGN = 100.0; // 画面外・上部マージン -- 画面上部に敵出現直後に撃破された場合を考慮

	// ? 画面外 -> アイテムを落とさない。
	if (IsOutOfScreen(CreateD2Point(enemy.X, enemy.Y), 0.0))
	{
		return;
	}

	GetEnemies().push(CreateEnemy_Item(enemy.X, enemy.Y, enemy.@@_ItemType));
}
