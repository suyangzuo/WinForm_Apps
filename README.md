# 📦 密码生成器-WinForm

一个基于 .NET 6 的简单 Windows Forms 密码生成器，使用 SunnyUI 进行界面美化，支持在 Windows x64 上构建与发布。

## 🔍 项目概览

- 名称：密码生成器-WinForm
- 类型：Windows Forms (.NET 6)
- 依赖：SunnyUI.dll / SunnyUI.Common.dll（已包含于 `bin/Debug/net6.0-windows`）
- 平台：Windows（推荐 win-x64）

## 🚀 快速开始

1. 克隆仓库：

   git clone <仓库地址>

2. 进入仓库目录：

   cd WinForm_Apps

3. 构建项目：

   dotnet build

### 🛠️ 构建与运行

- 构建（Debug）：

  dotnet build

- 运行：

  dotnet run --project "密码生成器-WinForm/密码生成器-WinForm.csproj"

- 以 Release 配置构建：

  dotnet build -c Release

### ✅ 发布

将应用发布为 Windows 自包含可执行文件（示例：win-x64）：

  dotnet publish "密码生成器-WinForm/密码生成器-WinForm.csproj" -c Release -r win-x64 -o ./publish --self-contained true

发布为框架依赖方式：

  dotnet publish "密码生成器-WinForm/密码生成器-WinForm.csproj" -c Release -o ./publish

发布输出将写入 `./publish` 目录，包含可执行文件与依赖项。

## 🧪 测试

本仓库当前不包含单元测试项目。若添加测试，请使用：

  dotnet test

## 🛠 常见问题 / 已知问题

- 如果运行时找不到 SunnyUI 依赖，请确认 `SunnyUI.dll` 与 `SunnyUI.Common.dll` 已在 `bin/Debug/net6.0-windows` 或发布目录中。
- 若出现权限或防病毒阻止执行的问题，请在受信任的位置运行或将可执行文件加入白名单。

## 🤝 如何贡献

欢迎贡献：请先打开 Issue 讨论，再提交 Pull Request。PR 请包含改动说明与必要截图/日志。分支命名建议使用：`feature/xxx` 或 `fix/xxx`。

## 📜 许可

本项目默认使用 MIT 许可。若你有其他许可偏好，请在仓库中添加 `LICENSE` 文件并说明。

## 📫 联系方式

项目维护者：仓库所有者（请在 README 中替换为你的联系方式或链接）。

