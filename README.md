# XNB2CP
An automatic XNB mod to [Content Patcher](https://github.com/Pathoschild/StardewMods/tree/stable/ContentPatcher) mod converter for Stadew Valley mods.
XNB2CP reads in XNB mods and generates the equivalent mod in Content Patcher's format.

## Features:
- Automatic mapping detection between the mod's files and SDV's files that supports:
	- A single Content folder that matches SDV's Content folder
	- A collection of individual files, as long as they are unique within SDV's Content folder
- Conversion of dictionary and image XNB files to Content Patcher's format (fonts and maps coming soon)
- Automatic creation of manifest.json and assets folder

## Known Limitations:
- Currently images cause large content.json files due to adding only the exact pixels that changed.

## Requirements:
- [xnbcli](https://github.com/LeonBlade/xnbcli)
	- Install as xnbcli/[windows/mac/linux]/[executable, dxt.node] inside the XNB2CP folder, the build process will handle copying it to where it needs to be.

## License:
GNU GPL v3.0