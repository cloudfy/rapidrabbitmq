@echo off
setlocal

REM Define the base directory to start the search
set "base_dir=X:\...\dep\erl"

REM Search for folders named "doc" and delete them
for /d /r "%base_dir%" %%d in (doc) do (
    if exist "%%~fd" (
        echo Deleting folder: %%~fd
        rmdir /s /q "%%~fd"
    )
)

echo All "doc" folders have been removed!
pause