/*
	�A�v���P�[�V�����p���C�����W���[��
*/

var APP_IDENT = "{ccd34185-1876-4fd0-a703-aa3b57d4b360}"; // �A�v�����ɕύX����B

window.onload = function() { Main(); }; // �G���g���[�|�C���g�Ăяo��

// �G���g���[�|�C���g
function Main()
{
	ProcMain(@@_Main());
}

// ���C��
function* @@_Main()
{
	// ���\�[�X�ǂݍ��ݒ��͑ҋ@
	while (1 <= Loading)
	{
		SetColor("#ffffff");
		PrintRect(0, 0, Screen_W, Screen_H);

		SetColor("#000000");
		SetSize(16);
		SetPrint(10, 25, 50);
		PrintLine("���\�[�X��ǂݍ���ł��܂� ...�@�c�� " + Loading + " ��");

		yield 1;
	}

	// -- choose one --

	yield* @@_Main2();
//	yield* Test01();
//	yield* Test02();
//	yield* Test03();

	// --
}

// �{�ԗp���C��
function* @@_Main2()
{
	yield* TitleMain();
}
