[English](../README.md)

# SP2 Cover From File（简体中文）

`SP2 Cover From File` 是适用于 Windows x64 版 `SimplePlanes 2` 的本地界面插件。
当前版本为 `1.0.0`，使用 `BepInEx 5 Mono x64` 加载，已针对游戏版本 `0.7.10.102` 构建。

插件在“上传作品 / Share”对话框里加一个 **From File** 按钮：直接从电脑里选一张图作为封面，
不用再先把图当蓝图导入、再在游戏里截图。它不修改载具、不改变物理、不发送网络消息，
也不依赖其他自定义插件。

## 直接安装（已包含 BepInEx）

仓库的 `release` 目录提供可部署压缩包：

[`release/SP2CoverFromFile-v1.0.0-win-x64.zip`](../release/SP2CoverFromFile-v1.0.0-win-x64.zip)

压缩包已经包含插件 DLL 和完整的 BepInEx `5.4.23.5` Windows x64 运行时，
**不需要另行安装 BepInEx**。

1. 完全关闭 `SimplePlanes 2`。
2. 打开压缩包，将其中全部内容直接解压到游戏根目录，也就是 `SimplePlanes 2.exe` 所在目录。
3. 如果系统询问是否合并 `BepInEx` 文件夹，选择合并。
4. 启动游戏，打开“上传作品”对话框。

安装后的关键文件结构如下：

```text
SimplePlanes 2/
├─ .doorstop_version
├─ doorstop_config.ini
├─ winhttp.dll
├─ BepInEx/
│  ├─ core/
│  └─ plugins/
│     └─ SP2CoverFromFile.dll
└─ SimplePlanes 2.exe
```

发布包不包含 `BepInEx/config`、日志、缓存或其他插件，所以覆盖安装不会重置现有配置。
压缩包的 SHA-256 位于 [`release/SHA256SUMS.txt`](../release/SHA256SUMS.txt)。
> 如果你已经为其他模组装过 BepInEx 5，建议只从压缩包里复制 `BepInEx/plugins/SP2CoverFromFile.dll`，
> 不要覆盖随包附带的 BepInEx 运行时，以免把你现有的新版本降级。

## 第一次安装 BepInEx 5（手动）

BepInEx 是让这类插件能够加载的“模组加载器”。如果你已经为其他 SimplePlanes 2 模组装过
BepInEx 5，可以直接跳到第 5 步。

1. **完全关闭游戏。** 游戏（`SimplePlanes 2.exe`）运行时无法安装 BepInEx。
2. 下载官方 BepInEx 5 Windows x64 压缩包：
   [`BepInEx_win_x64_5.4.23.5.zip`](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip)
   （SHA-256 `82f9878551030f54657792c0740d9d51a09500eeae1fba21106b0c441e6732c4`）。
3. 把压缩包**解压到游戏根目录**，也就是 `SimplePlanes 2.exe` 所在目录。解压后应当能直接看到
   `winhttp.dll`、`doorstop_config.ini`、`.doorstop_version` 和 `BepInEx` 文件夹，
   它们必须和游戏可执行文件**同级**，不能多套一层文件夹。
4. 启动一次游戏再退出。此时应生成 `BepInEx\LogOutput.log`，里面包含
   `Chainloader startup complete` 一行，说明 BepInEx 已正常工作。
5. 把本仓库的 `dist\SP2CoverFromFile.dll`（或发布压缩包里的同名文件）复制到：

   ```text
   <游戏目录>\BepInEx\plugins\SP2CoverFromFile.dll
   ```

6. 再次启动游戏。`BepInEx\LogOutput.log` 中应出现：

   ```text
   [Info   :SP2 Cover From File] SP2 Cover From File 1.0.0 已加载。
   [Info   :SP2 Cover From File] 已挂接上传对话框，可用“From File”从电脑选择封面图。
   ```

### 常见安装错误

| 现象 | 原因与处理 |
| --- | --- |
| 游戏能启动，但完全没有 `BepInEx\LogOutput.log` | 解压多套了一层文件夹。`winhttp.dll` 必须直接放在 `SimplePlanes 2.exe` 旁边。 |
| 有 `LogOutput.log`，但没有插件加载信息 | DLL 放错位置。必须放进 `BepInEx\plugins\` 目录。 |
| 装完后游戏一启动就闪退 | 存在旧版/不兼容的 BepInEx。删除旧 `BepInEx` 文件夹后按第 3 步重装。 |
| 覆盖 DLL 时提示文件被占用 | 游戏还在运行，完全退出后再复制。 |

## 升级插件

先完全关闭 `SimplePlanes 2`，从 [最新发布页](../releases/latest)
下载新的 Windows x64 压缩包，然后任选一种方式：

- **只替换插件 DLL：** 用新包里的 `BepInEx/plugins/SP2CoverFromFile.dll` 覆盖游戏目录中的同名文件，
  现有配置和其他插件都会保留。
- **同时更新随包附带的 BepInEx：** 把新压缩包全部内容解压到游戏根目录，允许合并文件夹并覆盖同名文件。
  发布包不含个人配置，因此现有设置会保留。
- 如果 `BepInEx/plugins` 里还有别的插件，**不要**删除整个 `BepInEx` 文件夹。

## 使用方法

1. 进入设计器，保存作品，然后打开 **Share（上传作品）** 对话框；
2. 截图列表里会多出一个 **From File** 按钮，位置在 **Add Screenshot** 上方；
3. 点它，选择一张 `.png` / `.jpg` / `.jpeg`；
4. 图片会作为**第一张截图**加入，也就是网站显示的封面；
5. 照常填写标题、描述，点 **Share** 上传。

| 细节 | 行为 |
| --- | --- |
| 支持格式 | PNG、JPEG |
| 插入位置 | 第一张（封面） |
| 调整顺序 | 用每张截图自带的上下箭头 |
| 数量限制 | 游戏最多允许 3 张截图，满了需要先删一张 |
| 上传 | 完全走游戏原有流程，图片会和普通截图一样被编码成 PNG 提交 |

## 配置

首次启动后生成：

```text
BepInEx/config/sp2.cover-from-file.cfg
```

| 配置项 | 默认值 | 说明 |
| --- | --- | --- |
| `General / Enabled` | `true` | 总开关 |
| `General / AllowCoverFromFile` | `true` | 是否显示 **From File** 按钮 |

修改配置文件后需要重启游戏。

## 卸载

只删除插件文件即可：

```text
BepInEx/plugins/SP2CoverFromFile.dll
```

如果还有其他模组在使用 BepInEx，请不要删除 `BepInEx` 文件夹、`winhttp.dll` 或
`doorstop_config.ini`。配置文件 `BepInEx/config/sp2.cover-from-file.cfg` 可以一并删除。

## 常见问题

- **看不到 From File 按钮：** 确认打开的是“上传作品”对话框（不是问题反馈对话框），
  并确认配置里 `AllowCoverFromFile = true`；同时检查 `BepInEx\LogOutput.log` 里有没有上面两行日志。
- **点按钮没反应：** 游戏最多允许 3 张截图，先删掉一张再试。
- **提示图片无法读取：** 确认文件是真正的 PNG/JPEG，而不是改了扩展名的其他格式。
- **试用版无效：** 试用版游戏无法上传作品。

## 隐私与发布范围

- 插件没有遥测、联网更新或数据上传功能。
- 仓库和发布包不包含个人配置、运行日志、缓存、账户信息或开发机器路径。
- 发布包中的 BepInEx 来自官方 `5.4.23.5` Windows x64 发行包，并使用上面固定的 SHA-256 校验。

第三方组件及许可证见 [THIRD_PARTY_NOTICES.md](../THIRD_PARTY_NOTICES.md)。

## 从源码构建

需要 Windows、.NET Framework 自带的 C# 编译器，以及本机安装的 `SimplePlanes 2`。

```powershell
.\build.ps1
.\build.ps1 -GamePath "D:\SteamLibrary\steamapps\common\SimplePlanes 2"
```

生成可直接分发的压缩包（会自动下载并校验官方 BepInEx 5.4.23.5）：

```powershell
.\package.ps1 -Version 1.0.0
```

运行离线测试（不需要游戏）：

```powershell
.\tests\run-tests.ps1
```

## 工作原理

上传对话框的截图列表是一个 `VerticalLayout`（`screenshots-parent`），里面有一个
`Add Screenshot` 按钮和若干 `screenshot` 列表项。插件在对话框初始化时，用游戏自己的控件 API
创建一个 `Button` 插到 `Add Screenshot` 上方；点击时打开游戏自带的 `StandaloneFileBrowser`
文件选择框，用 `Texture2D.LoadImage` 读取图片，按游戏模板创建 `screenshot` 列表项并移到第 0 位
（封面），最后调用对话框自己的刷新方法。上传请求本身完全没有被修改。

## 许可证

[MIT](../LICENSE)
