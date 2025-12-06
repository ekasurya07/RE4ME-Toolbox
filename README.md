# RE4ME-Toolbox

**RE4METoolbox** is a collection of lightweight command-line utilities tailored for modding, translating, and exploring **Resident Evil 4 Mobile Edition**.

## Included Tools

Currently, the toolbox contains the following utilities:

### 1. RE4MEAcvTool
A tool specifically designed for ACV archive structures (`.bin` files).
* **Unpack:** Extracts files from `.bin` archives retaining filenames.
* **Pack:** Creates new `.bin` archives from folders.

### 2. RE4MEMisTextTool
A tool for extracting and repacking mission text files.
* **Format:** Converts game files to **.tsv** (Tab-Separated Values) for easy editing in Excel or Notepad++.
* **Auto-Detection:** Simply drag a file or folder to process it automatically.
* **Modes:** Supports explicit `unpack` and `pack` commands via CLI.

*(More tools will be added in future updates)*

## Usage

### Method 1: Drag & Drop (Recommended)
All tools support **Drag & Drop** for maximum convenience.

* **RE4MEAcvTool:**
    * Drag `.bin` file -> **Unpack**
    * Drag Folder -> **Pack**
* **RE4MEMisTextTool:**
    * Drag file/folder -> **Auto-detect** (Unpacks `.bin` to `.tsv` / Packs `.tsv` to `.bin`)

### Method 2: Command Line (CLI)

You can run tools via the command line for precise control.

**For Archives (RE4MEAcvTool):**
```bash
RE4MEAcvTool.exe data.bin
RE4MEAcvTool.exe data_unpacked_folder
```

**For Text (RE4MEMisTextTool):**
```bash
RE4MEMisTextTool.exe unpack file.bin
RE4MEMisTextTool.exe pack file.tsv