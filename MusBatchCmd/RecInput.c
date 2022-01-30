/*
	RecInput.exe /R REC-FILE R-CTRL-AND-END

		REC-FILE       ... 保存ファイル
		R-CTRL-AND-END ... 右コントロール押下で停止するか, 0 or not 0

	RecInput.exe /S

		停止する。
*/

#pragma comment(lib, "user32.lib")

#include <windows.h>
#include <stdio.h>

typedef unsigned char uchar;
typedef signed int sint;
typedef unsigned int uint;

static void error(void)
{
	printf("ERROR\n");
	exit(1);
}
static void errorCase(int status)
{
	if (status)
		error();
}

#define REC_MAX 360000
#define REC_MARGIN 10

static uint MtxProc;
static uint EvStop;
static char *RecFile;
static int RCtrlAndEnd;

typedef struct Record_st
{
	sint X;
	sint Y;
	uchar States[0x100];
}
Record_t;

static Record_t *Records;

static void DoRecInput(void)
{
	uint vk;
	uint recIndex;
	uint recCount;
	Record_t *record;
	Record_t *prevRecord;
	FILE *fp;

	for (vk = 0; vk <= 0xff; vk++)
		GetAsyncKeyState(vk);

	Records = calloc(REC_MAX + REC_MARGIN, sizeof(Record_t));
	errorCase(!Records);

	for (recIndex = 0; ; recIndex++)
	{
		errorCase(REC_MAX <= recIndex); // 記憶領域の上限を超えました。

		record = Records + recIndex;

		{
			POINT pos;

			GetCursorPos(&pos);

			record->X = pos.x;
			record->Y = pos.y;
		}

		for (vk = 0; vk <= 0xff; vk++)
			record->States[vk] = GetAsyncKeyState(vk) ? 1 : 0;

		if (RCtrlAndEnd && record->States[VK_RCONTROL])
			break;

		if (WaitForSingleObject((HANDLE)EvStop, 10) == WAIT_OBJECT_0)
			break;
	}

	recCount = recIndex + 1;
	fp = fopen(RecFile, "wb");
	errorCase(!fp);

	if (recCount)
	{
		fprintf(fp, "M %d %d\n", record->X, record->Y);

		for (recIndex = 1; recIndex < recCount; recIndex++)
		{
			record = Records + recIndex;
			prevRecord = Records + recIndex - 1;

			if (prevRecord->X != record->X || prevRecord->Y != record->Y)
				fprintf(fp, "M %d %d", record->X, record->Y);

			for (vk = 0; vk <= 0xff; vk++)
				if (prevRecord->States[vk] ? !record->States[vk] : record->States[vk])
					fprintf(fp, " %c %u", record->States[vk] ? 'D' : 'U', vk);

			fprintf(fp, "\n");
		}
	}
	fclose(fp);
	free(Records);
}
int main(int argc, char **argv)
{
	MtxProc = (uint)CreateMutexA(NULL, FALSE, "{2ccc9820-c8dd-4c43-b8ec-02c97a0fb31f}");
	errorCase(!MtxProc);
	EvStop = (uint)CreateEventA(NULL, FALSE, FALSE, "{e2be109b-c282-40fc-86de-d86f383127a0}");
	errorCase(!EvStop);

	printf("%d\n", argc);

	if (argc == 4 && !_stricmp(argv[1], "/R"))
	{
		RecFile = argv[2];
		RCtrlAndEnd = atoi(argv[3]);

		if (WaitForSingleObject((HANDLE)MtxProc, 0) == WAIT_OBJECT_0)
		{
			DoRecInput();
			ReleaseMutex((HANDLE)MtxProc);
		}
	}
	else if (argc == 2 && !_stricmp(argv[1], "/S"))
	{
		SetEvent((HANDLE)EvStop);
	}
	CloseHandle((HANDLE)MtxProc);
	CloseHandle((HANDLE)EvStop);
}
