CALL C:\Factory\SetEnv.bat
CALL Clean.bat
cx **

ROBOCOPY dat out\Data /MIR

COPY /B Elsa20200001\Elsa20200001\bin\Release\Elsa20200001.exe out\Game.exe
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLib.dll out
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLibDotNet.dll out

xcp doc out
C:\Factory\Petra\EditConfig.exe out\Config.conf ;[LOG_ENABLED_MARK] 1

C:\apps\BuildDevGameBeforePack\BuildDevGameBeforePack.exe BUILD-DEV-GAME-UNSAFE-MOD out dat

C:\Factory\SubTools\zip.exe /PE- /RVE- /B /G out *P
