# XNB2CP
An automated XNB mod to [Content Patcher](https://github.com/Pathoschild/StardewMods/tree/stable/ContentPatcher) mod converter for Stadew Valley mods.
XNB2CP reads in XNB mods and generates the equivalent mod in Content Patcher's format.

# Discontinued
Work on this project is discontinued. The original goal was to create an automatic conversion tool, but my first efforts resulted in extremely large, unintuive content.json files. The community ended up doing conversions for most files manually, so there isn't need for such a tool anymore.

## Features:
- Automatic mapping detection between the mod's files and SDV's files that supports multiple formats.
	- Supports a single Content folder as well as individual files, as long as they are unique within SDV's Content folder.
- Conversion of dictionary and image XNB files to Content Patcher's format.
- Automatic creation of manifest.json and assets folder (along with content.json)

## Known Limitations:
- Currently images cause large content.json files due to adding only the exact pixels that changed.

## Requirements:
- [xnbcli](https://github.com/LeonBlade/xnbcli)
	- Install as xnbcli/[windows/mac/linux]/[executable, dxt.node] inside the XNB2CP folder, the build process will handle copying it to where it needs to be.

## License:
GNU GPL v3.0