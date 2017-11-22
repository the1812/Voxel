# Voxel
Tile generator for Windows 10

## About Tiles
>Windows tile can have different size depending on the DPI.
* Non-scalable Tile: The size of tile image is fixed.
* Scalable Tile: Images with different sizes, Windows can choose the currect one automatically.
* Image Tile: Dozens of non-scalable tiles that show an image like a jigsaw.

## App Settings
The settings file is saved in the same directory and named `Voxel.settings.json`.

If the app couldn't start, delete the settings file may solve the problem.
>App will crash if the content of settings file is not complete, in the future versions it will overwrite that bad file using default settings.

Here is default settings:
```json
{
	"ShowSplashScreen" : false,
	"StartMenu" : {
		"Fullscreen" : false,
		"ShowMoreTiles" : false
	},
	"NonscalableTile" : {
		"AutoLoadXml" : true,
		"AutoLoadVoxelFile" : false,
		"ClearTileCacheOnGenerate" : true,
		"ColorPickerView" : {
			"PreviewOnTile" : true,
			"ShowSampleText" : true
		}
	}
}
```
- `ShowSplashScreen`: Show a splash screen when app is starting.
- `StartMenu`: The editor use these settings to determine the size of tile.
    - `Fullscreen`: Set it to true if you use fullscreen start menu.
    - `ShowMoreTiles`: Set it to true if you enabled Show More Tiles in system settings.
- `NonscalableTile`: Settings for non-scalable tile editor.
    - `AutoLoadXml`: If the `$TargetName$.VisualElementsManifest.xml` exists in the same directory, load tile information from it.
    - `AutoLoadVoxelFile`: If the `$TargetName$.voxel` exists in the same directory, load tile information from it.
    - `ClearTileCacheOnGenerate`: When generating, scan all shortcut files on start menu that match the current target, and refresh them.
    - `ColorPickerView`: Settings for color picker.
        - `PreviewOnTile`: Preview new color on editor.
        - `ShowSampleText`: Show sample text to check the readablity.

## Contribution
### Leave your name
- Add your name to `names` in `Voxel/Contributors.cs`.
>Contributor names will be sorted alphabetically and shown on the `About` window (in future versions).
### Branch
- Commit to the `beta` branch.
### Coding Style
- See `Coding Style.md`.