# RE4MEAcvTool

**RE4MEAcvTool** is a lightweight command-line utility tailored for extracting and packing Resident Evil 4 Mobile Edition archives (`.bin` format). Developed in C# with a focus on simplicity and clean architecture.

## Features

- **Unpack:** Extracts files from `.bin` archives retaining filenames.
- **Pack:** Creates new `.bin` archives from folders.
- **Auto-Detection:** Automatically determines operation based on input (File -> Unpack, Folder -> Pack).
- **Format Support:** Specifically designed for ACV archive structures.

## Usage

### Method 1: Drag & Drop (Recommended)
1. **To Unpack:** Drag the `.bin` file onto `RE4MEAcvTool.exe`.
2. **To Pack:** Drag the folder containing files onto `RE4MEAcvTool.exe`.

### Method 2: Command Line
```bash
# Unpack a file
RE4MEAcvTool.exe data.bin

# Pack a folder
RE4MEAcvTool.exe data_unpacked