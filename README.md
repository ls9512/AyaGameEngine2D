![license](https://img.shields.io/github/license/ls9512/AyaGameEngine2D)
![topLanguage](https://img.shields.io/github/languages/top/ls9512/AyaGameEngine2D)
![size](https://img.shields.io/github/languages/code-size/ls9512/AyaGameEngine2D)
[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)

# Aya Game Engine 2D
## 简介
这个项目最初是学生时代初学C#，想尝试做游戏进而去尝试之后的产物，在不使用游戏引擎的情况下，基于OpenGL和原生C#实现的简易2D游戏引擎，以及两个测试用小游戏项目。

AGE2D 基于 **CSGL** 实现游戏绘图，基于 **Bass.Net** 实现音乐和音效播放，对 **OpenGL** 原生接口进行封装实现了一套简易的绘图库以实现复杂2D游戏画面和游戏UI，并且实现了基于CPU时钟计数的游戏主循环以驱动游戏执行，提供了简单的键盘、鼠标输入管理。同时附带一些常用的工具类，因此与其说是一个游戏引擎，倒不如说是一个类似 **EasyX** 的 2D绘图工具库。

项目代码开源，但样例工程中的素材仅供学习交流使用，不可用于商业用途。
最后一次提交仅对文件目录进行整理，项目后续不会再有更新支持，如使用中遇到问题请自行尝试解决。

## 文件说明
|目录|说明|
|-|-|
|Source|AGE2D 引擎原工程|
|Library|项目所涉及的第三方运行库|
|Doc|项目开发过程中形成的日志、文档|
|Samples|样例游戏工程|
|Samples\AcgParkour|样例工程：横版卷轴跑酷游戏|
|Samples\JewelHunter|样例工程：宝石消除游戏|


## 调试注意事项
* 编译运行样例工程时，依赖库需要被拷贝至编译出的 exe 所在目录
* 项目内附带的 Bass.Net 仅能运行在 x86 环境 
* 游戏资源加载目录根据具体运行调试环境不同可能需要修改