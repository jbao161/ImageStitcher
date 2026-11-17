# Keyboard Shortcuts Configuration Guide

## Overview

ImageStitcher now supports **configurable keyboard shortcuts** for moving images to folders. Instead of being hard-coded, shortcuts are defined in a JSON configuration file that automatically loads at startup.

## Configuration File Location

The configuration file is stored at:
```
C:\Users\<YourUsername>\AppData\Local\ImageStitcher\KeyboardShortcuts.json
```

The file is automatically created with default settings when the application first runs.

## How to Configure Shortcuts

### Step 1: Locate the Configuration File
- Run ImageStitcher at least once (if you haven't already)
- Navigate to: `C:\Users\<YourUsername>\AppData\Local\ImageStitcher\`
- Open `KeyboardShortcuts.json` in a text editor (Notepad, VS Code, etc.)

### Step 2: Edit the Shortcuts
Each shortcut is defined as a JSON object with three properties:
- **key**: The keyboard combination (e.g., "Q", "Alt+Q", "Shift+W", "Ctrl+Alt+E")
- **destinationFolder**: The absolute path to the folder where images will be moved
- **description**: A description of what this shortcut does (optional, for your reference)

### Step 3: Example Configuration

```json
[
  {
	"key": "Q",
	"destinationFolder": "C:\\Pictures\\Sorted\\Category1",
	"description": "Move to Category 1"
  },
  {
	"key": "W",
	"destinationFolder": "C:\\Pictures\\Sorted\\Category2",
	"description": "Move to Category 2"
  },
  {
	"key": "E",
	"destinationFolder": "C:\\Pictures\\Sorted\\Category3",
	"description": "Move to Category 3"
  },
  {
	"key": "Alt+Q",
	"destinationFolder": "D:\\Archive\\Old Files",
	"description": "Archive old files"
  },
  {
	"key": "Shift+W",
	"destinationFolder": "C:\\Pictures\\Favorites",
	"description": "Move to Favorites folder"
  }
]
```

## Supported Key Combinations

### Single Keys
- `A`, `B`, `C`, ... `Z` (letters)
- `0`, `1`, ... `9` (numbers)
- `F1`, `F2`, ... `F12` (function keys)
- `Enter`, `Space`, `Tab`, `Escape`, `Delete`, `Insert`
- `Left`, `Right`, `Up`, `Down` (arrow keys)
- `Home`, `End`, `PageUp`, `PageDown`

### Modifier + Key Combinations
- `Alt+Q` - Alt plus Q
- `Ctrl+E` - Ctrl (or Control) plus E
- `Shift+W` - Shift plus W
- `Alt+Ctrl+Z` - Alt plus Ctrl plus Z
- `Shift+Alt+E` - Shift plus Alt plus E
- `Ctrl+Shift+Alt+Q` - All three modifiers plus Q

### Valid Modifier Names
- `Alt`
- `Ctrl` or `Control`
- `Shift`

## Important Notes

1. **Folder Paths Must Exist**: The destination folder must exist before using the shortcut. The application will not create folders automatically.

2. **Absolute Paths Required**: Always use complete paths (e.g., `C:\Pictures\Sorted\`, not just `\Sorted\`)

3. **Windows Paths**: Use backslashes `\` or forward slashes `/` in paths. Both work fine.

4. **Duplicate Keys**: If you define the same key twice, only the first one will be used.

5. **Empty Destinations**: Shortcuts with empty `destinationFolder` values will be ignored.

6. **Case Sensitivity**: Key names are case-insensitive (e.g., "q" and "Q" are the same).

7. **Reload Required**: After editing the configuration file, **restart ImageStitcher** for changes to take effect.

## Migration from Old System

If you were previously using the hard-coded Q, W, E, R, Z shortcuts, you can set them up in the new configuration file:

```json
[
  {
	"key": "Q",
	"destinationFolder": "path/to/folder1"
  },
  {
	"key": "W",
	"destinationFolder": "path/to/folder2"
  },
  {
	"key": "E",
	"destinationFolder": "path/to/folder3"
  },
  {
	"key": "R",
	"destinationFolder": "path/to/folder4"
  },
  {
	"key": "Z",
	"destinationFolder": "path/to/folder5"
  }
]
```

## Troubleshooting

### Shortcuts Not Working
1. Check that the JSON file is valid (use a JSON validator: jsonlint.com)
2. Verify the folder paths exist
3. Make sure you restarted ImageStitcher after editing the config file
4. Check that the key names are spelled correctly

### JSON Syntax Errors
- Each JSON object ends with a comma (`,`) except the last one
- Paths should be in quotes: `"C:\\My Folder\\"`
- Use `\\` for backslashes in paths (or use forward slashes `/`)

### File Creation Issues
- If the file doesn't auto-create, manually create the `%AppData%\Local\ImageStitcher` directory
- Paste the example from `KEYBOARD_SHORTCUTS_SETUP.md` into a new `KeyboardShortcuts.json` file

## Example: Full Working Configuration

Save this to `KeyboardShortcuts.json`:

```json
[
  {
	"key": "Q",
	"destinationFolder": "C:\\Users\\YourName\\Pictures\\Sorted\\Keep",
	"description": "Move to Keep folder"
  },
  {
	"key": "W",
	"destinationFolder": "C:\\Users\\YourName\\Pictures\\Sorted\\Edit Later",
	"description": "Move to Edit Later folder"
  },
  {
	"key": "E",
	"destinationFolder": "C:\\Users\\YourName\\Pictures\\Sorted\\Delete",
	"description": "Move to Delete folder"
  },
  {
	"key": "Z",
	"destinationFolder": "C:\\Users\\YourName\\Pictures\\Archive",
	"description": "Move to Archive folder"
  }
]
```

Then restart ImageStitcher and press Q, W, E, or Z to move images to the corresponding folders!
