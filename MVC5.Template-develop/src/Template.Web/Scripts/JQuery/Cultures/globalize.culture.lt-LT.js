(function( window, undefined ) {

var Globalize;

if ( typeof require !== "undefined" &&
    typeof exports !== "undefined" &&
    typeof module !== "undefined" ) {
    // Assume CommonJS
    Globalize = require( "globalize" );
} else {
    // Global variable
    Globalize = window.Globalize;
}

Globalize.addCultureInfo( "lt-LT", "default", {
    name: "lt-LT",
    englishName: "Lithuanian (Lithuania)",
    nativeName: "lietuvių (Lietuva)",
    language: "lt",
    numberFormat: {
        ",": ".",
        ".": ",",
        negativeInfinity: "-begalybė",
        positiveInfinity: "begalybė",
        percent: {
            pattern: ["-n%","n%"],
            ",": ".",
            ".": ","
        },
        currency: {
            pattern: ["-n $","n $"],
            ",": ".",
            ".": ",",
            symbol: "€"
        }
    },
    calendars: {
        standard: {
            "/": ".",
            firstDay: 1,
            days: {
                names: ["sekmadienis","pirmadienis","antradienis","trečiadienis","ketvirtadienis","penktadienis","šeštadienis"],
                namesAbbr: ["Sk","Pr","An","Tr","Kt","Pn","Št"],
                namesShort: ["S","P","A","T","K","Pn","Š"]
            },
            months: {
                names: ["sausis","vasaris","kovas","balandis","gegužė","birželis","liepa","rugpjūtis","rugsėjis","spalis","lapkritis","gruodis",""],
                namesAbbr: ["Sau","Vas","Kov","Bal","Geg","Bir","Lie","Rgp","Rgs","Spl","Lap","Grd",""]
            },
            monthsGenitive: {
                names: ["sausio","vasario","kovo","balandžio","gegužės","birželio","liepos","rugpjūčio","rugsėjo","spalio","lapkričio","gruodžio",""],
                namesAbbr: ["Sau","Vas","Kov","Bal","Geg","Bir","Lie","Rgp","Rgs","Spl","Lap","Grd",""]
            },
            AM: null,
            PM: null,
            patterns: {
                d: "yyyy.MM.dd",
                dt: "yyyy.MM.dd HH:mm",
                D: "yyyy 'm.' MMMM d 'd.'",
                t: "HH:mm",
                T: "HH:mm:ss",
                f: "yyyy 'm.' MMMM d 'd.' HH:mm",
                F: "yyyy 'm.' MMMM d 'd.' HH:mm:ss",
                M: "MMMM d 'd.'",
                Y: "yyyy 'm.' MMMM"
            }
        }
    }
});

}( this ));

Globalize.culture("lt-LT");