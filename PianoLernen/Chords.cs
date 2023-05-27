using System;

[Flags]
public enum EMinorChords
{
	None = 0,
	Em = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	Gm = Note.G | Note.BFlat | Note.D,
	Am = Note.A | Note.C | Note.E,
	Bm = Note.B | Note.D | Note.FSharp
}

[Flags]
public enum DSharpMinorChords
{
	None = 0,
	DSharpMinor = Note.DSharp | Note.FSharp | Note.A,
	GSharpMinor = Note.GSharp | Note.B | Note.DSharp,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp
}

[Flags]
public enum FSharpMinorChords
{
	None = 0,
    FMajor = Note.F | Note.A | Note.C,
    GSharpMinor = Note.GSharp | Note.B | Note.DSharp,
    BFlatMajor = Note.BFlat | Note.D | Note.F,
    CSharpMinor = Note.CSharp | Note.E | Note.GSharp,
    EFlatMajor = Note.EFlat | Note.G | Note.BFlat
}

[Flags]
public enum FSharpMajorChords
{
	None = 0,
	FSharpMajor = Note.FSharp | Note.ASharp | Note.CSharp,
	BMajor = Note.B | Note.DSharp | Note.FSharp,
	EMajor = Note.E | Note.GSharp | Note.B,
	AMajor = Note.A | Note.CSharp | Note.E,
	DMajor = Note.D | Note.FSharp | Note.A
}

[Flags]
public enum GMajorChords
{
	None = 0,
	GMajor = Note.G | Note.B | Note.D,
	AMinor = Note.A | Note.C | Note.E,
	BMajor = Note.B | Note.DSharp | Note.FSharp,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EMinor = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp
}

[Flags]
public enum FSharpDiminishedChords
{
	None = 0,
	FSharpDiminished = Note.FSharp | Note.A | Note.CSharp,
	GSharpDiminished = Note.GSharp | Note.B | Note.DSharp,
	ASharpDiminished = Note.ASharp | Note.CSharp | Note.FSharp
}

[Flags]
public enum GMinorChords
{
	None = 0,
	GMajor = Note.G | Note.B | Note.D,
	AMinor = Note.A | Note.C | Note.E,
	BFlatMinor = Note.BFlat | Note.DFlat | Note.F,
	CMinor = Note.C | Note.EFlat | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EFlatMinor = Note.EFlat | Note.GFlat | Note.BFlat
}

[Flags]
public enum BMajorChords
{
	None = 0,
	BMajor = Note.B | Note.DSharp | Note.FSharp,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EMinor = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	GSharpDiminished = Note.GSharp | Note.B | Note.DSharp
}

[Flags]
public enum GSharpMinorChords
{
	None = 0,
	GMajor = Note.G | Note.B | Note.D,
	AMinor = Note.A | Note.C | Note.E,
	BFlatMajor = Note.BFlat | Note.DFlat | Note.GFlat,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp,
	DMinor = Note.D | Note.FSharp | Note.A,
	EFlatMajor = Note.EFlat | Note.GFlat | Note.BFlat
}

[Flags]
public enum AMinorChords
{
	None = 0,
	AMinor = Note.A | Note.C | Note.E,
	BFlatMinor = Note.BFlat | Note.DFlat | Note.F,
	CMinor = Note.C | Note.EFlat | Note.G,
	DMinor = Note.D | Note.F | Note.AFlat,
	EFlatMinor = Note.EFlat | Note.GFlat | Note.BFlat,
	FMinor = Note.F | Note.AFlat | Note.CFlat,
	GMinor = Note.G | Note.BFlat | Note.D
}

[Flags]
public enum ADiminishedChords
{
	None = 0,
	ADim = Note.A | Note.C | Note.EFlat,
	BDim = Note.B | Note.D | Note.F,
	CDim = Note.C | Note.EFlat | Note.G,
	DDim = Note.D | Note.F | Note.AFlat,
	EDim = Note.E | Note.G | Note.BFlat,
	FDim = Note.F | Note.A | Note.CFlat,
	GDim = Note.G | Note.BFlat | Note.DFlat
}

[Flags]
public enum DMinorChords
{
	None = 0,
	DMinor = Note.D | Note.F | Note.A,
	DMinor7 = Note.D | Note.F | Note.A | Note.C,
	DMinor9 = Note.D | Note.F | Note.A | Note.C | Note.E,
	DMinor11 = Note.D | Note.F | Note.A | Note.C | Note.E | Note.G,
	DMinor13 = Note.D | Note.F | Note.A | Note.C | Note.E | Note.G | Note.B
}

[Flags]
public enum CMajorChords
{
	None = 0,
	CMajor = Note.C | Note.E | Note.G,
	DMajor = Note.D | Note.FSharp | Note.A,
	EMajor = Note.E | Note.GSharp | Note.B,
	FMajor = Note.F | Note.A | Note.C,
	GMajor = Note.G | Note.B | Note.D,
	AMajor = Note.A | Note.CSharp | Note.E,
	BMajor = Note.B | Note.DSharp | Note.FSharp
}

[Flags]
public enum BMinorChords
{
	None = 0,
	BMajor = Note.B | Note.DSharp | Note.FSharp,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp, 
	DMinor = Note.D | Note.F | Note.A,
	EFlatMinor = Note.EFlat | Note.GFlat | Note.BFlat, 
	EMinor = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp, 
	GMinor = Note.G | Note.BFlat | Note.D, 
	AFlatMinor = Note.AFlat | Note.CFlat | Note.EFlat
}

[Flags]
public enum CSharpMajorChords
{
	None = 0,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EMinor = Note.E | Note.G | Note.B,
	FMajor = Note.F | Note.A | Note.C,
	GMajor = Note.G | Note.B | Note.D,
	AMinor = Note.A | Note.C | Note.E,
	BbMajor = Note.BFlat | Note.D | Note.F
}

[Flags]
public enum ASharpMajorChords
{
	None = 0,
	ASharpMajor = Note.ASharp | Note.CSharp | Note.FSharp,
	BFlatMajor = Note.BFlat | Note.DFlat | Note.GFlat,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp,
	DSharpMinor = Note.DSharp | Note.FSharp | Note.ASharp,
	EFlatMajor = Note.EFlat | Note.GFlat | Note.BFlat,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	GSharpMinor = Note.GSharp | Note.B | Note.DSharp
}

[Flags]
public enum CSharpMinorChords
{
	None = 0, 
	CMinor = Note.C | Note.EFlat | Note.G, 
	DMinor = Note.D | Note.F | Note.A, 
	EMinor = Note.E | Note.G | Note.B, 
	FMinor = Note.F | Note.GSharp | Note.C, 
	GSharpM = Note.GSharp | Note.BFlat | Note.DSharp, 
	AMinor = Note.A | Note.C | Note.E
}

[Flags]
public enum FDiminishedChords
{
	None = 0,
	FirstChord = Note.C | Note.E | Note.GSharp,
	SecondChord = Note.D | Note.F | Note.A,
	ThirdChord = Note.EFlat | Note.G | Note.BFlat,
	FourthChord = Note.F | Note.AFlat | Note.C
}

[Flags]
public enum CSharpDiminishedChords
{
	None = 0,
	CDim = Note.C | Note.EFlat | Note.G,
	CDim7 = Note.C | Note.EFlat | Note.G | Note.BFlat,
	DDim = Note.D | Note.F | Note.AFlat,
	DDim7 = Note.D | Note.F | Note.AFlat | Note.CFlat,
	EDim = Note.E | Note.G | Note.BFlat,
	EDim7 = Note.E | Note.G | Note.BFlat | Note.DFlat,
	FDim = Note.F | Note.A | Note.CFlat,
	FDim7 = Note.F | Note.A | Note.CFlat | Note.EFlat,
	GDim = Note.G | Note.BFlat | Note.DFlat
}

[Flags]
public enum ASharpDiminishedChords
{
	None = 0,
	ASharpDiminished = Note.ASharp | Note.CSharp | Note.F,
	BFlatDiminished = Note.BFlat | Note.DFlat | Note.FSharp,
	CDiminished = Note.C | Note.EFlat | Note.GSharp,
	DSharpDiminished = Note.DSharp | Note.FSharp | Note.A,
	EDiminished = Note.E | Note.GFlat | Note.ASharp,
	FSharpDiminished = Note.FSharp | Note.A | Note.CSharp
}

[Flags]
public enum FMajorChords
{
	None = 0,
	FMajor = Note.F | Note.A | Note.C,
	GMajor = Note.G | Note.B | Note.D,
	AMajor = Note.A | Note.CSharp | Note.E,
	BFlatMajor = Note.BFlat | Note.DFlat | Note.F,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EMinor = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp
}

[Flags]
public enum DSharpDiminishedChords
{
	None = 0,
	DSharpDiminished = Note.DSharp | Note.FSharp | Note.A,
	EFlatDiminished = Note.EFlat | Note.G | Note.BFlat,
	ESharpDiminished = Note.ESharp | Note.GSharp | Note.B,
	FSharpDiminished = Note.FSharp | Note.A | Note.C,
	GSharpDiminished = Note.GSharp | Note.B | Note.DSharp,
	AFlatDiminished = Note.AFlat | Note.C | Note.EFlat,
	ASharpDiminished = Note.ASharp | Note.CSharp | Note.E
}

[Flags]
public enum EDiminishedChords
{
	None = 0,
	EDiminished = Note.E | Note.G | Note.BFlat,
	FSharpDiminished = Note.FSharp | Note.A | Note.C,
	GSharpDiminished = Note.GSharp | Note.B | Note.D,
	ADiminished = Note.A | Note.CSharp | Note.EFlat,
	BFlatDiminished = Note.BFlat | Note.DFlat | Note.F
}

[Flags]
public enum BDiminishedChords
{
	None = 0,
	BDiminished = Note.B | Note.D | Note.F,
	CDiminished = Note.C | Note.EFlat | Note.G,
	DDiminished = Note.D | Note.F | Note.AFlat,
	EDiminished = Note.E | Note.G | Note.BFlat,
	FDiminished = Note.F | Note.A | Note.CFlat,
	GDiminished = Note.G | Note.BFlat | Note.DFlat,
	ADiminished = Note.A | Note.C | Note.EFlat
}

[Flags]
public enum CMinorChords
{
	None = 0,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EFlatMajor = Note.EFlat | Note.G | Note.BFlat,
	FMinor = Note.F | Note.AFlat | Note.C,
	GMinor = Note.G | Note.BFlat | Note.D,
	AFlatMajor = Note.AFlat | Note.CFlat | Note.EFlat
}

[Flags]
public enum FMinorChords
{
	None = 0,
	FMajor = Note.F | Note.A | Note.C,
	GMinor = Note.G | Note.BFlat | Note.D,
	BbMajor = Note.BFlat | Note.D | Note.F,
	CMinor = Note.C | Note.EFlat | Note.G,
	DMinor = Note.D | Note.F | Note.A
}

[Flags]
public enum ASharpMinorChords
{
	None = 0,
	AMinor = Note.A | Note.C | Note.E,
	BFlatDiminished = Note.BFlat | Note.DFlat | Note.F,
	CSharpDiminished = Note.CSharp | Note.E | Note.G,
	DMinor = Note.D | Note.FSharp | Note.A,
	EFlatMajor = Note.EFlat | Note.G | Note.BFlat,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	GSharpDiminished = Note.GSharp | Note.B | Note.DSharp
}

[Flags]
public enum DMajorChords
{
	None = 0,
	DMajor = Note.D | Note.FSharp | Note.A,
	DMajor7 = Note.D | Note.FSharp | Note.A | Note.CSharp,
	DMinor = Note.D | Note.F | Note.A,
	DMinor7 = Note.D | Note.F | Note.A | Note.C,
	D7 = Note.D | Note.FSharp | Note.A | Note.C,
	DMinorMajor7 = Note.D | Note.F | Note.A | Note.CSharp
}

[Flags]
public enum DMinorSharpChords
{
	None = 0,
	DMinor = Note.D | Note.F | Note.A,
	EMinor = Note.E | Note.G | Note.B,
	FSharpMinor = Note.FSharp | Note.ASharp | Note.CSharp,
	GMinor = Note.G | Note.BFlat | Note.D,
	AFlatMinor = Note.AFlat | Note.CFlat | Note.EFlat,
	BMinor = Note.B | Note.D | Note.FSharp
}

[Flags]
public enum DSharpMajorChords
{
	None = 0,
	DSharpMajor = Note.DSharp | Note.FSharp | Note.ASharp,
	ASharpMinor = Note.ASharp | Note.CSharp | Note.FSharp,
	ESharpMinor = Note.ESharp | Note.GSharp | Note.CSharp,
	BSharpMinor = Note.BSharp | Note.DSharp | Note.GSharp,
	FSharpDiminished = Note.FSharp | Note.ASharp | Note.DSharp
}

[Flags]
public enum DDiminishedChords
{
	None = 0,
	Ddim = Note.D | Note.F | Note.AFlat,
	Edim = Note.E | Note.G | Note.BFlat,
	Fdim = Note.F | Note.A | Note.CFlat,
	Gdim = Note.G | Note.B | Note.DFlat,
	Adim = Note.A | Note.C | Note.EFlat,
	Bdim = Note.B | Note.D | Note.FSharp
}

[Flags]
public enum GSharpDiminishedChords
{
	None = 0,
	GSharpDiminishedChord = Note.GSharp | Note.BFlat | Note.D,
	ASharpDiminishedChord = Note.ASharp | Note.CSharp | Note.F,
	BDiminishedChord = Note.B | Note.DSharp | Note.FSharp,
	CDiminishedChord = Note.C | Note.EFlat | Note.G,
	DSharpDiminishedChord = Note.DSharp | Note.F | Note.A,
	EDiminishedChord = Note.E | Note.GFlat | Note.AFlat,
	FDiminishedChord = Note.F | Note.A | Note.C,
	GDiminishedChord = Note.G | Note.BFlat | Note.DFlat
}

[Flags]
public enum GSharpMajorChords
{
	None = 0,
	GSharpMajor = Note.GSharp | Note.B | Note.DSharp,
	AFlatMajor = Note.AFlat | Note.C | Note.EFlat,
	BFlatMajor = Note.BFlat | Note.DFlat | Note.F,
	CSharpMajor = Note.CSharp | Note.E | Note.GSharp,
	DSharpMajor = Note.DSharp | Note.FSharp | Note.ASharp,
	EFlatMajor = Note.EFlat | Note.GFlat | Note.BFlat,
	FSharpMajor = Note.FSharp | Note.A | Note.CSharp
}

[Flags]
public enum CMSharpMinorChords
{
	None = 0,
	CM = Note.C | Note.E | Note.G,
	CSharpM = Note.CSharp | Note.F | Note.GSharp,
	DM = Note.D | Note.FSharp | Note.A,
	DSharpM = Note.DSharp | Note.G | Note.ASharp,
	EM = Note.E | Note.GSharp | Note.B,
	FM = Note.F | Note.A | Note.C,
	FSharpM = Note.FSharp | Note.ASharp | Note.CSharp,
	GM = Note.G | Note.B | Note.D,
	GSharpM = Note.GSharp | Note.C | Note.DSharp,
	AM = Note.A | Note.CSharp | Note.E,
	// todo, this thing got jacked
	//ASharpM = Note.ASharp | Note.D |
}

[Flags]
public enum EMajorChords
{
	None = 0,
	EMajor = Note.E | Note.GSharp | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	GSharpMinor = Note.GSharp | Note.B | Note.DSharp,
	AFlatMajor = Note.AFlat | Note.C | Note.EFlat,
	BFlatMajor = Note.BFlat | Note.D | Note.F,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp,
	DSharpDiminished = Note.DSharp | Note.FSharp | Note.AFlat
}

[Flags]
public enum GDiminishedChords
{
	None = 0,
	GDiminished = Note.G | Note.BFlat | Note.DFlat,
	ADiminished = Note.A | Note.C | Note.EFlat,
	BDiminished = Note.B | Note.D | Note.F,
	CDiminished = Note.C | Note.EFlat | Note.GFlat,
	DDiminished = Note.D | Note.F | Note.AFlat,
	EDiminished = Note.E | Note.GFlat | Note.BFlat,
	FDiminished = Note.F | Note.AFlat | Note.CFlat
}

[Flags]
public enum FSharpMajorMinorChords
{
	None = 0,
	FSharpMajor = Note.FSharp | Note.ASharp | Note.CSharp,
	GSharpMinor = Note.GSharp | Note.B | Note.D,
	AFlatMajor = Note.AFlat | Note.CFlat | Note.F,
	BFlatMinor = Note.BFlat | Note.DFlat | Note.FSharp,
	CSharpMinor = Note.CSharp | Note.E | Note.GSharp
}

[Flags]
public enum AMajorChords
{
	None = 0,
	AMajor = Note.A | Note.CSharp | Note.E,
	BMinor = Note.B | Note.D | Note.FSharp,
	CMajor = Note.C | Note.E | Note.G,
	DMinor = Note.D | Note.F | Note.A,
	EMajor = Note.E | Note.GSharp | Note.B,
	FSharpMinor = Note.FSharp | Note.A | Note.CSharp,
	GMajor = Note.G | Note.B | Note.D,
	AMinor = Note.A | Note.C | Note.E
}
