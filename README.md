# Balatro Hand Parser

This is a console-based parser for a **domain-specific language (DSL)** that models poker hands in a simplified version of the video game *Balatro*. It uses **Coco/R** to parse custom hand definitions.

## Setup & Build

1. Ensure `coco.exe` and `Balatro.atg` are in the `Coco/` folder.
2. Coco/R is run automatically via a **pre-build event**. This generates `Parser.cs` and `Scanner.cs`.
3. Build the project and run the executable.

## What the Program Does

- Prompts the user for hand definitions in the DSL format
- Parses input using a Coco/R-generated parser
- Reports whether parsing was successful or if syntax errors were found

## DSL Input Format

Currently, the grammar supports the definition of a hand with **cards only**.

### Valid Input Example:
```txt
2HN, 3DG, 5SP, 9CM, KHN // 2 of Hearts [Normal], 3 of Diamonds [Gold], 5 of Spades [Polychrome], 9 of Clubs [Mult], King of Hearts [Normal]
```



### Supported Card Format
- Cards must be written as: [Rank][Suit][Mod]

- Valid ranks: 2–10, J, Q, K, A

- Valid suits: H (Hearts), D (Diamonds), S (Spades), C (Clubs)

- Valid card modifiers: N (Normal), B (Bonus), M (Mult), G (Glass)


### Supported Joker Format
- Jokers must be written as: [Modifier][Type][EffectValue][_OptionalSecondEffectValue][OptionalTarget]

- Valid Joker Modifiers: N (Normal), F (Foil), H (Holographic), P (Polychrome)

- Valid Joker Types: A (Additive), M (Multiplicative), C (Chips&Additive), R (Retrigger)

- EffectValue must be a number, i.e. 2, 4, 9, 50, etc.