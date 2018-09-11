import os
import glob
html="<html><body>"
cmd=""
md="#Icons\n"
for fname in glob.glob("./*.svg"):
    html+="<img src='%s' width='100'> "%fname
    cmd+="convert %s %s.ico\n"%(fname.replace(".svg",".png"),fname.replace(".svg",".ico"))
    md+="## %s\n<img src='%s' width='100'>\n\n"%(os.path.basename(fname),os.path.basename(fname))
html+="</body></html>"
with open("index.html",'w') as f:
    f.write(html)
with open("_makeIcons.bat",'w') as f:
    f.write(cmd)
with open("readme.md",'w') as f:
    f.write(md)
print("DONE")