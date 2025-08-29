#!/bin/bash

# WARNING: This script replaces all pngs in Assets with webps.

cd Assets
shopt -s nullglob          # makes *.png expand to nothing if there are no matches
for f in *.png; do
    webp="${f%.png}.webp"
    magick "$f" -strip -quality 80 "$webp" && rm "$f"
done
