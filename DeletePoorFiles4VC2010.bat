rem
rem ****************
rem by Gemstone
rem ****************
rem
del /s *.pdb
del /s *.idb
del /s *.obj
del /s *.log
del /s *.aps
del /s *.embed.manifest.res
del /s *.embed.manifest
del /s *.intermediate.manifest
del /s *.ilk
del /s *.exp
del /s *.pch
del /s *_manifest.rc
del /s *.lastbuildstate
del /s *.tlog
del /s *.asp
del /s *.user
del /s *.opensdf
del /s *.sdf
del /s /ah *.suo
del /s Output-Build.txt
rd /s /q _Obj
del /s Log.dat
rd /s /q _Log
cd Src
rd /s /q ipch
cd..

