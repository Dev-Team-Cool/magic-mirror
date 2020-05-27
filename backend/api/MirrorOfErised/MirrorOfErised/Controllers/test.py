import sys
import pathlib
import os

filename = sys.argv[1]
path = os.path.join( "F:", "School", "magic-mirror","backend","api","MirrorOfErised","MirrorOfErised","wwwroot","images" )
fullpath = pathlib.Path(path, filename)


print(fullpath)