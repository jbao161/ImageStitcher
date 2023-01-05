#!/usr/bin/env python
from sys import argv
from pathlib import Path
from PIL import Image, ImageSequence


def webp2gif(path: Path):
    img = Image.open(path)

    frames: list[Image.Image] = []
    for frame in ImageSequence.Iterator(img):        
        im2 = Image.new("RGB", frame.size, (255, 255, 255))        
        bands = frame.split()
        mask = bands[3] if len(bands)>3 else None
        im2.paste(frame, mask=mask)
        frames.append(im2.convert('RGB'))

    frames[0].save(path.with_suffix('.gif'),
                   format='gif',
                   save_all=True,
                   append_images=frames[1:],
                   optimize=True,
                   duration=img.info.get("duration", 10),
                   loop=img.info.get("loop", 0),
                   quality=100)


webp2gif(Path(argv[1]))