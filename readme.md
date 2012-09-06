# Cyotek.Data.Nbt

NBT (Named Binary Tag) is a tag based binary format designed to carry large amounts of binary data with smaller amounts of additional data. This is currently the format that Minecraft uses for player and region data.

Cyotek.Data.Nbt is a library for reading and writing NBT format files used by Minecraft. However, the format is versatile enough to use for many other applications.

It was originally based on [LibNBT](http://libnbt.codeplex.com/) found on CodePlex, but I've made a lot of changes to it. The API has substantially changed, although it should be easier to use than the original.

In addition to support for the native binary NBT format, this library also offers the ability to read and write NBT tags to and from XML... not quite as "binary", but certainly more readable!

### Features
* Support for all tags in the specification
* Reads and writes binary files compatible with existing NBT libraries and tools
* Supports reading and writing to XML based files for human readable output
* Ability to provide custom readers/writers if required
* Extended API for working with NBT documents
* Query support

### License

As per the original library, this source is licensed under a [Creative Commons Share Alike](http://creativecommons.org/licenses/by-sa/2.0/).