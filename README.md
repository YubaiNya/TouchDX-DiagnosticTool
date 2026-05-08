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

## 💖 赞助与支持

如果您觉得这个诊断工具在排查网络问题时对您有所帮助，欢迎通过**爱发电**支持作者：

<a href="https://ifdian.net/a/YubaiNya" target="_blank">
  <img src="https://img.shields.io/badge/%E7%88%B1%E5%8F%91%E7%94%B5-%E8%B5%9E%E5%8A%A9%E4%BD%9C%E8%80%85-946ce6?style=for-the-badge&logo=buy-me-a-coffee&logoColor=white" alt="赞助作者">
</a>

## 🚨 问题反馈 (Issues)

**注意**：为了方便统一管理，**请将所有 Bug 反馈和功能建议统一提交至主仓库 [YubaiNya/TouchDX 的 Issues 页面](https://github.com/YubaiNya/TouchDX/issues)**。请不要在本仓库单独提交 Issue，否则可能会因为未及时看到而导致修复延误！

## 许可

本项目采用 [AGPL-3.0 License](LICENSE) 开源许可。
