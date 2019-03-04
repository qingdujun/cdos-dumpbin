# cdos-dumpbin.exe

本软件提供两个功能：

- 分析某款软件的DLL依赖情况；

- 比较某款软件与其他多款软件DLL依赖情况的差异。


# 如何开始使用？

- 举个例子：>cdos-dumpbin.exe -dll "C:\Program Files\Microsoft Office"
（分析完毕后，直接在屏幕显示）

- 举个例子：>cdos-dumpbin.exe -cmp "./conf_cmp_1vsN.ini" 

（分析完毕之后，会在当前目录下生成“res_cmp_1vsN.csv”文件）

更详细的内容请查看使用说明《软件cdos-dumpbin.exe使用说明.docx》。