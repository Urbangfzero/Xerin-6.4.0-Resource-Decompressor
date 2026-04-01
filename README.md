
<h1 align="center">Xerin 6.4.0 Resource Decompressor</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-4.8-512BD4?style=flat-square" />
  <img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=flat-square" />
  <img src="https://img.shields.io/badge/Target-Xerin%206.4.0-black?style=flat-square" />
</p>

<p align="center">
  Lightweight .NET tool for extracting and decompressing resources protected by Xerin 6.4.0
</p>

---

## Important Notice

This tool targets **Xerin 6.4.0 only**.

It is not expected to work with:

- Other Xerin versions  
- Modified builds  
- Different obfuscators  

Xerin uses version-specific internals (resource formats, encryption, compression).  
Any deviation may result in failure or invalid output.

---

## Acknowledgment

Full credit goes to the creator of the Xerin obfuscator:

- https://github.com/ItIsInx

This project exists for:

- Reverse engineering practice  
- Malware analysis  
- Educational research  
- Studying protection mechanisms  

---

## Overview

The decompressor performs the following:

- Loads a protected .NET assembly  
- Locates encrypted resource streams  
- Identifies the decryption routine in `<Module>`  
- Extracts encryption keys  
- Decrypts resource data  
- Decompresses using DeflateStream  
- Rebuilds and writes the recovered output  

---

## Features

- Detection of Xerin 6.4.0 resource protection  
- Automatic decryptor discovery  
- Key extraction from runtime logic  
- Resource decryption and decompression  
- Resource replacement support  
- Clean and minimal console output  

## Usage

### Command Line
Run the decompressor on a protected .NET assembly:

```bash
XerinDecompressor.exe <ProtectedAssembly.exe>
```
## Project Structure
### The Xerin 6.4.0 Resource Decompressor project is organized as follows:
```bash
/XerinDecompressor
 ├── /Source
 │    ├── Program.cs                   # Main entry point
 │    │
 │    ├── /Core
 │    │    ├── Decompressor.cs         # Handles resource decompression
 │    │    ├── Decryptor.cs            # Handles decryption logic
 │    │    ├── KeyDetector.cs          # Finds encryption keys in assembly
 │    │    ├── ResourceHelper.cs       # Helper functions for resources
 │    │    └── ResourceReplacer.cs     # Replaces decrypted resources back into assembly
 │    │
 │    └── /Logger
 │         └── Logger.cs               # Handles console output/logging
 │
 └── README.md                          # This documentation
 ```

## Requirements

- .NET Framework 4.8  
- Windows 10 / 11  

### Dependency

```bash
dotnet add package dnlib