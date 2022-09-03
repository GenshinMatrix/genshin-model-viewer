cd /d %~dp0
rename GenshinModelViewer.Packaging_*_x64.cer App.cer
rename GenshinModelViewer.Packaging_*_x64.msixbundle App.msixbundle
del App_Setup.exe
nsis\tools\makensis .\nsis\setup.nsi
del GenshinModelViewer_Setup.exe
rename App_Setup.exe GenshinModelViewer_Setup.exe
@pause
