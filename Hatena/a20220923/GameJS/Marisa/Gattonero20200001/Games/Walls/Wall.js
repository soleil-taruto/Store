/*
	•ÇŽ†
*/

/@(ASTR)

/// Wall_t
{
	// •ÇŽ†‚ð•`‰æ‚·‚éB
	//
	<generatorForTask> Draw
}

@(ASTR)/

/*
	•`‰æ
*/
function <void> DrawWall(<Wall_t> wall)
{
	if (!NextVal(wall.Draw))
	{
		error();
	}
}
