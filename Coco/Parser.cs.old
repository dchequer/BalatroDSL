using BalatroDSL.Models;



using System;



public class Parser {
	public const int _EOF = 0;
	public const int _card = 1;
	public const int _joker = 2;
	public const int maxT = 4;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

public Hand currentHand;
int cardCounter = 0;

void AddCardToHand(string cardText) 
{
    string rankStr = cardText.Substring(0, cardText.Length - 2);
    char suitChar = cardText[cardText.Length - 2];
    char modChar = cardText[cardText.Length - 1];

    Rank rank = rankStr switch
    {
        "2" => Rank.Two,
        "3" => Rank.Three,
        "4" => Rank.Four,
        "5" => Rank.Five,
        "6" => Rank.Six,
        "7" => Rank.Seven,
        "8" => Rank.Eight,
        "9" => Rank.Nine,
        "10" => Rank.Ten,
        "J" => Rank.Jack,
        "Q" => Rank.Queen,
        "K" => Rank.King,
        "A" => Rank.Ace,
        _ => throw new Exception("Invalid rank")
    };

    Suit suit = suitChar switch {
        'H' => Suit.Hearts,
        'D' => Suit.Diamonds,
        'S' => Suit.Spades,
        'C' => Suit.Clubs,
        _ => throw new Exception("Invalid suit")
    };

    CardModifier modifier = modChar switch {
        'N' => CardModifier.None,
        'B' => CardModifier.Bonus,
        'M' => CardModifier.Mult,
        'G' => CardModifier.Glass,
        _ => throw new Exception("Invalid card modifier")
    };

    var card = new Card {
        Rank = rank,
        Suit = suit,
        Modifier = modifier,
        OriginalIndex = cardCounter++,
    };

    currentHand.Cards.Add(card);
}

void AddJokerToHand(string text)
{
    if (string.IsNullOrWhiteSpace(text) || text.Length < 2)
        throw new Exception("Invalid Joker input.");

    text = text.ToUpperInvariant();

    var joker = new Joker();

    // Step 1: Modifier
    joker.Modifier = text[0] switch
    {
        'N' => JokerModifier.None,
        'F' => JokerModifier.Foil,
        'H' => JokerModifier.Holographic,
        'P' => JokerModifier.Polychrome,
        _ => throw new Exception("Invalid Joker Modifier.")
    };

    // Step 2: Type
    char typeLetter = text[1];
    var rest = text.Substring(2); // all after modifier and type

    switch (typeLetter)
    {
        case 'A':
            joker.Type = JokerType.AdditiveMult;
            joker.EffectValue1 = ParseSingleValue(rest);
            break;

        case 'M':
            joker.Type = JokerType.Multiplicative;
            joker.EffectValue1 = ParseSingleValue(rest);
            break;

        case 'C':
            joker.Type = JokerType.ChipsAndAdditive;
            ParseTwoValues(rest, out int chips, out int mult);
            joker.EffectValue1 = chips;
            joker.EffectValue2 = mult;
            break;

        case 'T':
            joker.Type = JokerType.Trigger;
            ParseTriggerJoker(rest, joker);
            break;

        default:
            throw new Exception("Invalid Joker Type Letter.");
    }

    currentHand.Jokers.Add(joker);
}

int ParseSingleValue(string text)
{
    if (int.TryParse(text, out int value))
        return value;

    throw new Exception("Expected numeric value for Joker.");
}

void ParseTwoValues(string text, out int first, out int second)
{
    var parts = text.Split('_');
    if (parts.Length != 2)
        throw new Exception("Expected two values separated by '_'.");

    if (!int.TryParse(parts[0], out first) || !int.TryParse(parts[1], out second))
        throw new Exception("Invalid numeric values for Chips&Mult Joker.");
}

void ParseTriggerJoker(string text, Joker joker)
{
    if (string.IsNullOrEmpty(text))
        throw new Exception("Trigger Joker must specify target ranks.");

    if (text.Length >= 2 && (text[0] == 'A' || text[0] == 'M' || text[0] == 'C'))
    {
        // Explicit effect specified
        char effectType = text[0];
        var parts = text.Substring(1).Split('_');

        if (parts.Length != 2)
            throw new Exception("Expected '_' separator between value and target.");

        if (!int.TryParse(parts[0], out int value))
            throw new Exception("Invalid numeric effect value.");

        joker.EffectValue1 = value;
        joker.TriggerMode = JokerTriggerMode.AddEffectOnTrigger;
        joker.TriggerEffect = effectType switch
        {
            'A' => JokerTriggerEffect.AddMult,
            'M' => JokerTriggerEffect.MultiplyMult,
            'C' => JokerTriggerEffect.AddChips,
            _ => throw new Exception("Unknown trigger effect type.")
        };

        joker.TriggerTarget = parts[1];
    }
    else
    {
        // No explicit effect => simple replay trigger
        joker.TriggerMode = JokerTriggerMode.ReplayCard;
        joker.TriggerEffect = JokerTriggerEffect.None;
        joker.TriggerTarget = text;
    }
}




	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Balatro() {
		currentHand = new Hand(); 
		ParseHand();
	}

	void ParseHand() {
		Entry();
		while (la.kind == 3) {
			Get();
			Entry();
		}
	}

	void Entry() {
		if (la.kind == 1) {
			Get();
			AddCardToHand(t.val); 
		} else if (la.kind == 2) {
			Get();
			AddJokerToHand(t.val); 
		} else SynErr(5);
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Balatro();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "card expected"; break;
			case 2: s = "joker expected"; break;
			case 3: s = "\",\" expected"; break;
			case 4: s = "??? expected"; break;
			case 5: s = "invalid Entry"; break;

			default: s = "error " + n; break;
		}
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
		count++;
	}
	
	public virtual void SemErr (string s) {
		errorStream.WriteLine(s);
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
		errorStream.WriteLine(errMsgFormat, line, col, s);
	}
	
	public virtual void Warning(string s) {
		errorStream.WriteLine(s);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
