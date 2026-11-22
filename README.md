[简体中文](doc/README.zh-CN.md)

# SP2 Cover From File

`SP2 Cover From File` is a local UI plugin for the Windows x64 edition of `SimplePlanes 2`.
The current version is `1.0.0`. It is loaded with the BepInEx 5 Mono x64 runtime and is built
against game version `0.7.10.102`.

The plugin adds a **From File** button to the *Share / Upload craft* dialog so you can pick the cover
image straight from your computer, instead of importing the picture as a blueprint and taking an
in-game screenshot. It does not modify crafts, physics or network messages, and it does not depend on
other custom plugins.

## Direct installation (BepInEx included)

The `release` directory contains a ready-to-install archive:

[`release/SP2CoverFromFile-v1.0.0-win-x64.zip`](release/SP2CoverFromFile-v1.0.0-win-x64.zip)

The archive includes the complete BepInEx `5.4.23.5` Windows x64 runtime together with the plugin,
so **you do not need to install BepInEx separately**.

1. Close `SimplePlanes 2` completely.
2. Open the archive and extract **everything** into the game directory — the folder that contains
   `SimplePlanes 2.exe`.
3. If Windows asks whether to merge the `BepInEx` folder, allow it.
4. Start the game, open the *Share* (upload craft) dialog.

The installed files should look like this:

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

The archive does not contain `BepInEx/config`, logs, caches or other plugins, so extracting it over
an existing setup will not reset your settings. The SHA-256 of the archive is recorded in
[`release/SHA256SUMS.txt`](release/SHA256SUMS.txt).
> If you already use BepInEx 5 for other SimplePlanes 2 mods, prefer copying only
> `BepInEx/plugins/SP2CoverFromFile.dll` from the archive instead of extracting the bundled runtime, so you do not
> overwrite a newer BepInEx installation.

## First-time BepInEx 5 installation (manual)

BepInEx is the mod loader this plugin runs inside. If you already use BepInEx 5 for other
SimplePlanes 2 mods, skip to step 5.

1. **Close the game completely.** BepInEx cannot be installed while `SimplePlanes 2.exe` is running.
2. Download the official BepInEx 5 Windows x64 archive:
   [`BepInEx_win_x64_5.4.23.5.zip`](https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip)
   (SHA-256 `82f9878551030f54657792c0740d9d51a09500eeae1fba21106b0c441e6732c4`).
3. Extract the archive **into the game directory** — the folder containing `SimplePlanes 2.exe`.
   After extracting you must see `winhttp.dll`, `doorstop_config.ini`, `.doorstop_version` and the
   `BepInEx` folder **next to the game executable**, not inside another folder.
4. Start the game once, then close it. A new file `BepInEx\LogOutput.log` should appear and contain
   the line `Chainloader startup complete`. This proves BepInEx is working.
5. Copy `dist\SP2CoverFromFile.dll` from this repository (or from the release archive) into:

   ```text
   <game folder>\BepInEx\plugins\SP2CoverFromFile.dll
   ```

6. Start the game again. `BepInEx\LogOutput.log` should now contain:

   ```text
   [Info   :SP2 Cover From File] SP2 Cover From File 1.0.0 已加载。
   [Info   :SP2 Cover From File] 已挂接上传对话框，可用“From File”从电脑选择封面图。
   ```

### Common installation mistakes

| Symptom | Cause / fix |
| --- | --- |
| Game starts, nothing happens, no `BepInEx\LogOutput.log` | Files were extracted one level too deep. `winhttp.dll` must sit directly next to `SimplePlanes 2.exe`. |
| `LogOutput.log` exists but has no plugin lines | The DLL is in the wrong folder. It must be inside `BepInEx\plugins\`. |
| The game closes instantly after installation | Another copy of BepInEx / an incompatible loader is present. Remove the old `BepInEx` folder and redo step 3. |
| Plugin DLL cannot be replaced | The game is still running. Close it and try again. |

## Upgrading

Close `SimplePlanes 2` and download the newest archive from the
[releases page](releases/latest).

- **Plugin only:** replace `BepInEx/plugins/SP2CoverFromFile.dll` with the new one. Your
  configuration and other plugins are kept.
- **Bundled BepInEx too:** extract the whole archive into the game directory and allow files to be
  overwritten. The package contains no personal configuration, so your settings remain.
- Do **not** delete the whole `BepInEx` folder if other plugins are installed.

## Usage

1. Enter the designer, save the craft, then open **Share** (upload craft).
2. In the screenshot list you will see a **From File** button directly above **Add Screenshot**.
3. Click it and pick a `.png`, `.jpg` or `.jpeg`.
4. The image becomes the **first screenshot** — that is the cover used by the website.
5. Fill in the title/description and press **Share** as usual.

| Detail | Behaviour |
| --- | --- |
| Accepted formats | PNG, JPEG |
| Position in list | Inserted as the first (cover) screenshot |
| Reordering | Use the existing move up/down buttons on each screenshot |
| Limit | The game allows at most 3 screenshots; delete one first if the list is full |
| Upload | Handled entirely by the game; the image is encoded to PNG like a normal screenshot |

## Configuration

Created after the first launch:

```text
BepInEx/config/sp2.cover-from-file.cfg
```

| Key | Default | Description |
| --- | --- | --- |
| `General / Enabled` | `true` | Master switch |
| `General / AllowCoverFromFile` | `true` | Show the **From File** button in the upload dialog |

Editing the file requires a game restart.

## Uninstall

Delete only the plugin file:

```text
BepInEx/plugins/SP2CoverFromFile.dll
```

Keep `BepInEx`, `winhttp.dll` and `doorstop_config.ini` if other mods still use BepInEx.
The configuration file `BepInEx/config/sp2.cover-from-file.cfg` may be deleted as well.

## Troubleshooting

- **No From File button:** make sure you opened the *Upload craft* dialog (not the bug report dialog),
  and that `AllowCoverFromFile` is `true`. Check `BepInEx\LogOutput.log` for the two lines above.
- **Button does nothing:** the game allows at most 3 screenshots; delete one first.
- **Image rejected:** make sure the file is a real PNG/JPEG, not another format with a renamed extension.
- **Demo build:** the demo version cannot upload crafts.

## Privacy and release scope

- No telemetry, no online updates, no data upload.
- The repository and release archive contain no personal configuration, logs, caches, account
  details or developer-machine paths.
- BepInEx in the release archive comes from the official `5.4.23.5` Windows x64 release and is
  verified against the pinned SHA-256 above.

See [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).

## Building from source

Requirements: Windows, the .NET Framework C# compiler (bundled with Windows) and a local
SimplePlanes 2 installation.

```powershell
.\build.ps1
.\build.ps1 -GamePath "D:\SteamLibrary\steamapps\common\SimplePlanes 2"
```

Build a ready-to-install archive (downloads and verifies the official BepInEx 5.4.23.5 zip):

```powershell
.\package.ps1 -Version 1.0.0
```

Run the offline tests (no game required):

```powershell
.\tests\run-tests.ps1
```

## How it works

The upload dialog's screenshot list is a `VerticalLayout` (`screenshots-parent`) containing an
`Add Screenshot` button and one `screenshot` item per captured image. On dialog initialization the
plugin creates a `Button` widget with the game's own widget API, inserts it above *Add Screenshot*,
and on click opens the game's bundled `StandaloneFileBrowser` picker. The chosen file is loaded with
`Texture2D.LoadImage`, turned into a `screenshot` list item from the game's template, moved to index
0 (the cover), and the dialog's own refresh methods are called. The upload request itself is never
patched.

## License

[MIT](LICENSE)
