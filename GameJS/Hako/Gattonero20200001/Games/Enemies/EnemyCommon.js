/*
	“G‹¤’Ê
*/

function <void> EnemyCommon_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// none
}

function <void> EnemyCommon_Dead(<Enemy_t> enemy)
{
	AddEffect_Explode(enemy.X, enemy.Y);
}
