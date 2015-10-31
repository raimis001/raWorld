RMDIR E:\GDrive\src\RaWorld3D\Assets /S /Q
RMDIR E:\GDrive\src\RaWorld3D\ProjectSettings /S /Q
pause
xcopy Assets\* E:\GDrive\src\RaWorld3D\Assets /E /I
xcopy ProjectSettings\* E:\GDrive\src\RaWorld3D\ProjectSettings /E /I
pause