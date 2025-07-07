import os
import re
import json
import argparse


def createkeymap(il2cppcs, bytesfilefloder, outputjson):
    classpattern = re.compile(r'\b(class|struct)\s+([a-zA-Z0-9_]+)')

    classmap = {}
    try:
        with open(il2cppcs, 'r', encoding='utf-8') as f:
            for line in f:
                match = classpattern.search(line)
                if match:
                    key = match.group(2)
                    name = key.lower()
                    classmap[name] = key
    except FileNotFoundError:
        print(f"Not Find {il2cppcs}")
        return
    
    keymap = {}
    try:
        allfiles = [f for f in os.listdir(bytesfilefloder) if f.endswith('.bytes')]
        for filename in allfiles:
            basename = os.path.splitext(filename)[0]
            hashkey = classmap.get(basename)
            if hashkey:
                keymap[filename] = hashkey
            else:
                print(f"{filename} cannot find key")
    except FileNotFoundError:
        print(f"Not Find {bytesfilefloder}")
        return
        
    try:
        with open(outputjson, 'w', encoding='utf-8') as f:
            json.dump(keymap, f, indent=4, sort_keys=True)
            print(f"The key Has been dumped into {outputjson}")
    except Exception as e:
        print(f"dump key error: {e}")

if __name__ == '__main__':
    parser = argparse.ArgumentParser(
        epilog="Usage: GenXORNamelist.py <inputBytesFileFolder>"
    )

    parser.add_argument("bytefilepath")

    args = parser.parse_args()

    createkeymap("BlueArchive.cs", args.bytefilepath, "BytesKey.json")