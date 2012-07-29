# Cyotek.Data.Nbt

NBT (Named Binary Tag) is a tag based binary format designed to carry large amounts of binary data with smaller amounts of additional data. This is currently the format that Minecraft uses for player and region data.

Cyotek.Data.Nbt is a library for reading and writing NBT format files used by Minecraft. However, the format is versatile enough to use for many other applications.

It was originally based on [LibNBT](http://libnbt.codeplex.com/) found on CodePlex, but I've made a lot of changes to it to make it easier to use in real world applications. Or at least easier for me in mine!

> Note: I haven't fully tested this (yet). It works so far from what I'm using it for, but no doubt bugs exist. I need to adapt (and expand) the original tests that were written for the library to ensure I haven't broken anything.

### License

As per the original library, this source is licensed under a [Creative Commons Share Alike](http://creativecommons.org/licenses/by-sa/2.0/).