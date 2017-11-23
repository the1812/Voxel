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

Here is default settings: (You can also find it in `Voxel\Model\Settings.cs`)
```json
{
	"ShowSplashScreen" : true,
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

## Help
- Main Window:
	- `Non-scalable Tile`:
		- `Select Target`: Select the target file (`.exe` only), or right-click to select folder.
		>In the future versions the `.exe` limit will be removed.
		- `Select Image`: Select the image that shows on your tile, or right-click to use no image (show the icon of target).
		- `Set Backcolor`: Set backcolor of tile, or right-click to use default color.
		- `Use Dark Theme`: If checked, the tile name will be black (useful when background image is bright).
		- `Show Name`: If checked, your tile will show target name.
		- `Medium/Small`: Toggle this to see what your tile looks like on different sizes.
		- `Reset`: Reset all tile settings above except the target.
		- `Generate`: Generate all necessary files for your tile, and clear tile cache if it's enabled in settings.
		- `Add to Start`: Create a shortcut file on Start Menu folder so that you can pin it later.
		- `Import`: Import tile data from `.voxel` file.
		- `Export`: Export tile data to `.voxel` file.
	- `Scalable Tile`: Coming soon.
	- `Image Tile`: Coming soon.
	- `Clear Tile Cache`: Clear cache for all tiles on start menu.
	>You can qucikly make it by using the command-line argument `--clear`.