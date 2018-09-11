import os
import glob
html="<html><body>"
cmd=""
for fname in glob.glob("./*.svg"):
    html+="<img src='%s' width='100'> "%fname
    cmd+="convert %s %s.ico\n"%(fname.replace(".svg",".png"),fname.replace(".svg",".ico"))
html+="</body></html>"
with open("index.html",'w') as f:
    f.write(html)
with open("_makeIcons.bat",'w') as f:
    f.write(cmd)
print("DONE")