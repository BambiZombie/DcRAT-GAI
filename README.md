# DcRAT-GAI
前段时间个人学习使用，二开DcRAT，主要是增加了功能性插件，没有做ByPass

增加功能如下：

| 更新时间 | 更新内容                   | 更新目录                        |
| -------- | -------------------------- | ------------------------------- |
| 20221222 | Dump Lsass                 | Plugin/Minidump                 |
| 20221224 | 添加后门账号               | Plugin/UserAdd                  |
| 20221229 | 不使用Powershell执行命令   | Plugin/NoPowershell             |
| 20230102 | 修复NoPowershell退出bug    | Plugin/NoPowershell             |
| 20230102 | 添加shellcode注入          | Plugin/ShellcodeInject          |
| 20230103 | 添加Mimikatz的提取密码功能 | Plugin/Mimikatz                 |
| 20230103 | 修改了log输出方向          | Server/Handle_Packet/HandleLogs |
| 20230103 | 收集各种密码               | Plugin/PwdCollection            |
| 20230104 | 修改注册表自启动           | Plugin/WinLogonHelper           |
| 20230104 | 劫持txt默认扩展名权限维持  | Plugin/HijackDefaultExt         |
| 20230104 | 劫持屏幕保护程序权限维持   | Plugin/HijackScrnSaver          |
| 20230104 | WMI后门                    | Plugin/WmiBackdoor              |
| 20230105 | 窃取远程rdp凭据            | Plugin/RdpThief                 |
| 20230105 | 收集密码管理器密码         | Plugin/WcmDump                  |
| 20230106 | 优化界面                   | Server/Form                     |

