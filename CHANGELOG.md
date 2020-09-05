# CHANGELOG

## v0.2

XCDESaveData.cs, PartyMember.cs:

Added support for the following:

* Party Member: Skill Tree and Expert Mode related data
    - Skill Tree Selected Index
    - Skill Points (SP) for each Skill Tree
    - Skill Tree No. of unlocked skill slots
    - Skill Link Skills
    - Expert Mode Level
    - Expert Mode EXP
    - Expert Mode Reserve EXP

XCDESaveThumbnail.cs:

* BUGFIX: ReverseBytes() was altering original byte array instead of writing changes to new array

Item.cs:

* UPDATE: Added default constructor for Item

## v0.1
Added support for the following:

* Party: List of characters in Party + total count
* Party Member: about 1/3 of Party Member data, including:
    - Level
    - EXP
    - AP
    - Affinity Coins
    - Equipped Items
    - Set Cosmetics
    - Set Arts
* Arts Levels + Arts Max Unlock Levels

## v0.0
initial commit, WIP prerelease. Supports the following:

bfsgame0xx.sav: (Single Save File)

* Money
* Noponstones
* Inventory structure
* Items (almost 100%, a few sections of item data are still unidentified)

bfsgame0xx.tmb: (Save File Thumbnail Image)

* Thumbnail image
