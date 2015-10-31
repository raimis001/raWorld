RMDIR Assets /S /Q
RMDIR ProjectSettings /S /Q
pause
xcopy E:\GDrive\src\RaWorld3D\Assets\* Assets /E /I
xcopy  E:\GDrive\src\RaWorld3D\ProjectSettings\* ProjectSettings /E /I
xcopy  E:\GDrive\src\RaWorld3D\flaimages\* flaimages /E /I
pause