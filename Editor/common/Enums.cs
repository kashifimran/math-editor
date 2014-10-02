
namespace Editor
{
    public enum CommandType
    {
        None,
        Text,
        ShowBox, HideBox,
        SquareRoot, nRoot, Division,
        LeftBracket, RightBracket, LeftRightBracket,
        Sub, Super, SubAndSuper, SignComposite, Composite, CompositeBig,
        TopBracket, BottomBracket, DoubleArrowBarBracket,
        Decorated, Arrow, Box, Matrix, CustomMatrix, DecoratedCharacter
    }

    public enum HAlignment { Left, Center, Right }
    public enum VAlignment { Center, Top, Bottom }

    public enum Position 
    { 
        None, Middle, Top, Bottom, Left, Right, Sub, Super, SubAndSuper, BottomAndTop,
        TopLeft, BottomLeft, TopRight, BottomRight, Over
    }

    public enum CharacterDecorationType
    {
        None,
        StrikeThrough, DoubleStrikeThrough, VStrikeThrough, 
        VDoubleStrikeThrough, Cross, LeftCross, RightCross,     
        LeftUprightCross, RightUprightCross,        
        Unicode,
    }


    //public enum CharacterDecorationType
    //{
    //    StrikeThrough, DoubleStrikeThrough, HStrikeThrough, HDoubleStrikeThrough, Cross, LeftCross, RightCross,

    //    SuperRing, TopRing, TopBar,
    //    RightArrowTop, LeftArrowTop, DoubleArrowTop,
    //    LeftHarpoonTop, RightHarpoonTop,
    //    Prime, DoublePrime, TriplePrime,
    //    TopDot, TopDoubleDot, TopTripleDot, TopFourDots,
    //    TopVel, TopHat, TopBreve,
    //    Accute, Grave, GraveLeft, TopCircumflex,

    //    BottomDot, BottomDoubleDot, BottomTripleDot, BottomFourDots,
    //    BottomBar, BottomTilde, BottomBreve, BottomCircumflex,
    //    BottomRightArrow, BottomLeftArrow, BottomDoubleArrow,
    //    BottomRightHarpoon, BottomLeftHarpoon,
    //}

    public enum DivisionType 
    { 
        DivRegular, DivDoubleBar, DivTripleBar, 
        DivMath, DivMathWithTop, 
        DivHoriz, DivSlanted, 
        DivRegularSmall, DivHorizSmall, DivSlantedSmall,
        DivMathInverted, DivInvertedWithBottom,
        DivTriangleFixed, DivTriangleExpanding
    }

    public enum BoxType { All, LeftTop, RightTop, LeftBottom, RightBottom }

    public enum ArrowType
    {
        RightArrow, LeftArrow, DoubleArrow, RightLeftArrow, RightSmallLeftArrow, SmallRightLeftArrow,
        RightLeftHarpoon, RightSmallLeftHarpoon, SmallRightLeftHarpoon
    }

    public enum DecorationType
    {
        Tilde, Hat, Parenthesis, Tortoise, RightArrow, LeftArrow, DoubleArrow, RightHarpoonUpBarb, LeftHarpoonUpBarb,
        RightHarpoonDownBarb, LeftHarpoonDownBarb, Bar, DoubleBar, StrikeThrough, Cross, RightCross, LeftCross,
    }    

    public enum SignCompositeSymbol
    {
        Sum, Product, CoProduct, Intersection, Union, Integral, DoubleIntegral, TripleIntegral,
        ContourIntegral, SurfaceIntegral, VolumeIntegral, ClockContourIntegral, AntiClockContourIntegral
    }

    public enum IntegralType
    {
        Integral, DoubleIntegral, TripleIntegral,
        ContourIntegral,
        SurfaceIntegral,
        VolumeIntegral,
        ClockContourIntegral,
        AntiClockContourIntegral,
    }

    public enum BracketSignType
    {
        LeftRound, RightRound, LeftCurly, RightCurly, LeftSquare, RightSquare, LeftAngle, RightAngle,
        LeftBar, RightBar, LeftSquareBar, RightSquareBar, LeftDoubleBar, RightDoubleBar, LeftCeiling, RightCeiling,
        LeftFloor, RightFloor,
    }

    public enum HorizontalBracketSignType
    {
        TopCurly, BottomCurly, ToSquare, BottomSquare,
    }

    //public enum SubSuperType { Sub = 0, Super = 1 }   

    public enum FontType
    {
        SystemDefault, STIXGeneral, STIXIntegralsD, STIXIntegralsSm, STIXNonUnicode, STIXSizeThreeSym, STIXSizeTwoSym, STIXVariants,
        STIXSizeFourSym, STIXIntegralsUpSm, STIXSizeOneSym, STIXIntegralsUpD, STIXIntegralsUp, STIXSizeFiveSym,
        Segoe, Arial, TimesNewRoman, CourierNew, Courier, Georgia, Impact, LucidaSansUnicode, Tahoma, Verdana,
        Webdings, Wingdings, MSSerif, MSSansSerif, ComicSansMS, ArialBlack, LucidaConsole, PalatinoLinotype,
        TrebuchetMS, Symbol

        //STIXGeneral,        STIXGeneralBol,     STIXGeneralBolIta,  STIXGeneralItalic,
        //STIXIntDBol,        STIXIntDReg,        STIXIntSmBol,       STIXIntSmReg,       STIXIntUpBol, STIXIntUpDBol,
        //STIXIntUpDReg,      STIXIntUpReg,       STIXIntUpSmBol,     STIXIntUpSmReg,     STIXNonUni,
        //STIXNonUniBol,      STIXNonUniBolIta,   STIXNonUniIta,      STIXSizFiveSymReg,  STIXSizFourSymBol,
        //STIXSizFourSymReg,  STIXSizOneSymBol,   STIXSizOneSymReg,   STIXSizThreeSymBol, STIXSizThreeSymReg,
        //STIXSizTwoSymBol,   STIXSizTwoSymReg,   STIXVar,            STIXVarBol,
    }

    public enum EditorMode
    {
        Math, Text, 
    }
}