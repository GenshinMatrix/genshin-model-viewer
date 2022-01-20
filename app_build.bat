@cd /d %~sdp0

set "binpath_com=Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\"
set "binpath_pro=Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\"
set "binpath_ent=Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\"
set "path=%path%;C:\%binpath_com%"
set "path=%path%;D:\%binpath_com%"
set "path=%path%;E:\%binpath_com%"
set "path=%path%;F:\%binpath_com%"
set "path=%path%;C:\%binpath_pro%"
set "path=%path%;D:\%binpath_pro%"
set "path=%path%;E:\%binpath_pro%"
set "path=%path%;F:\%binpath_pro%"
set "path=%path%;C:\%binpath_ent%"
set "path=%path%;D:\%binpath_ent%"
set "path=%path%;E:\%binpath_ent%"
set "path=%path%;F:\%binpath_ent%"
msbuild src/GenshinModelViewer.sln /t:Rebuild /p:Configuration=Release

set "binpath_7z=:\Program Files\7-Zip\"
set "path=%path%;C:\%binpath_7z%"
set "path=%path%;D:\%binpath_7z%"
set "path=%path%;E:\%binpath_7z%"
set "path=%path%;F:\%binpath_7z%"
del .\src\GenshinModelViewer\bin\x64\Release\net48\*.config
del .\src\GenshinModelViewer\bin\x64\Release\net48\*.pdb
del .\src\GenshinModelViewer\bin\x64\Release\net48\*.xml
copy /y .\src\packages\SevenZip\SevenZip.dll .\src\GenshinModelViewer\bin\x64\Release\net48
rd /s /q .\src\GenshinModelViewer\bin\x64\Release\net48\genshin-model-viewer_Secure
cd .\src\GenshinModelViewer\bin\x64\Release
ren net48 genshin-model-viewer
7z a ..\..\..\..\..\genshin-model-viewer_build%date:~,4%%date:~5,2%%date:~8,2%.7z .\genshin-model-viewer\ -t7z -mx=5 -r -y
ren genshin-model-viewer net48

set "binpath_rea=:\Program Files (x86)\Eziriz\.NET Reactor\"
set "path=%path%;C:\%binpath_rea%"
set "path=%path%;D:\%binpath_rea%"
set "path=%path%;E:\%binpath_rea%"
set "path=%path%;F:\%binpath_rea%"
cd net48
dotNET_Reactor.Console -file "genshin-model-viewer.exe" -satellite_assemblies "HelixToolkit.dll -embed;HelixToolkit.Wpf.dll -embed;Pfim.dll -embed;QuickLook.Common.dll -embed;QuickLook.Plugin.HelixViewer.dll -embed;SharpVectors.Converters.Wpf.dll -embed;SharpVectors.Core.dll -embed;SharpVectors.Css.dll -embed;SharpVectors.Dom.dll -embed;SharpVectors.Model.dll -embed;SharpVectors.Rendering.Gdi.dll -embed;SharpVectors.Rendering.Wpf.dll -embed;SharpVectors.Runtime.Wpf.dll -embed" -embed 1 -stringencryption 0 -suppressildasm 0 -obfuscation 0
cd genshin-model-viewer_Secure
copy /y ..\SevenZipSharp.dll .
copy /y ..\SevenZip.dll .
7z a ..\..\..\..\..\..\..\genshin-model-viewer_sbuild%date:~,4%%date:~5,2%%date:~8,2%.7z .\* -t7z -mx=5 -r -y
@pause
