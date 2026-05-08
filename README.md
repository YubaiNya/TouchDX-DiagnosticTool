# TouchDX-DiagnosticTool

TouchDX 项目的 PC 端网络与输入诊断工具，专门为调试 **maimai DX**（舞萌 DX）的远程控制器而设计。

此工具主要用于帮助玩家测试：
- 安卓端 `MaiNative` 与 PC 之间的网络连通性。
- 触控数据的接收和解析状态，直观展示 64 位输入掩码（包括屏幕 A-E 区、外围按键以及 Select/Test/Service/Coin 等控制键）。

## 如何使用

1. 前往本项目的 [Releases](https://github.com/YubaiNya/TouchDX-DiagnosticTool/releases) 下载最新版本并打开工具。
2. 确保手机和电脑在同一局域网下，或已配置好 ADB 端口转发（推荐执行：`adb reverse tcp:4321 tcp:4321`）。
3. 诊断工具将显示本机的所有 IP 地址，在手机端的 TouchDX 客户端输入对应的 IP（若是 ADB 转发则输入 `127.0.0.1`）。
4. 在手机端进行触摸，工具界面会实时反馈当前的连接状态和接收到的 maimai DX 具体操作掩码数据（如 TouchE1、Btn3 等）。

## 如何构建

1. 使用 Visual Studio 2022 或是 .NET 命令行工具打开本项目的 `.csproj`。
2. 本项目基于 .NET 构建，无需复杂的外部依赖。
3. 编译运行即可。

## 许可

本项目采用 [AGPL-3.0 License](LICENSE) 开源许可。
