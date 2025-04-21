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
hand {
  cards: [2H, 3D, 5S, 9C, KH]
}

### Invalid Input Example:
```txt
hand {
  cards: [2H, 3D, ZZ]    // 'ZZ' is not a valid card
}

hand cards: [2H, 3D, 5S]  // Missing opening/closing braces
```

### Supported Card Format
- Cards must be written as: [Rank][Suit]

- Valid ranks: 2–10, J, Q, K, A

- Valid suits: H (Hearts), D (Diamonds), S (Spades), C (Clubs)