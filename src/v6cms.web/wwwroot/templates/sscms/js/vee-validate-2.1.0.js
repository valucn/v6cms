!function (e, t) {
    "object" == typeof exports && "undefined" != typeof module ? module.exports = t() : "function" == typeof define && define.amd ? define(t) : e.VeeValidate = t()
}(this, function () {
    "use strict";
    var e = 36e5
        , t = 6e4
        , n = 2
        , r = {
            dateTimeDelimeter: /[T ]/,
            plainTime: /:/,
            YY: /^(\d{2})$/,
            YYY: [/^([+-]\d{2})$/, /^([+-]\d{3})$/, /^([+-]\d{4})$/],
            YYYY: /^(\d{4})/,
            YYYYY: [/^([+-]\d{4})/, /^([+-]\d{5})/, /^([+-]\d{6})/],
            MM: /^-(\d{2})$/,
            DDD: /^-?(\d{3})$/,
            MMDD: /^-?(\d{2})-?(\d{2})$/,
            Www: /^-?W(\d{2})$/,
            WwwD: /^-?W(\d{2})-?(\d{1})$/,
            HH: /^(\d{2}([.,]\d*)?)$/,
            HHMM: /^(\d{2}):?(\d{2}([.,]\d*)?)$/,
            HHMMSS: /^(\d{2}):?(\d{2}):?(\d{2}([.,]\d*)?)$/,
            timezone: /([Z+-].*)$/,
            timezoneZ: /^(Z)$/,
            timezoneHH: /^([+-])(\d{2})$/,
            timezoneHHMM: /^([+-])(\d{2}):?(\d{2})$/
        };
    function i(i, o) {
        if (arguments.length < 1)
            throw new TypeError("1 argument required, but only " + arguments.length + " present");
        if (null === i)
            return new Date(NaN);
        var s = o || {}
            , u = void 0 === s.additionalDigits ? n : Number(s.additionalDigits);
        if (2 !== u && 1 !== u && 0 !== u)
            throw new RangeError("additionalDigits must be 0, 1 or 2");
        if (i instanceof Date)
            return new Date(i.getTime());
        if ("string" != typeof i)
            return new Date(i);
        var l = function (e) {
            var t, n = {}, i = e.split(r.dateTimeDelimeter);
            r.plainTime.test(i[0]) ? (n.date = null,
                t = i[0]) : (n.date = i[0],
                    t = i[1]);
            if (t) {
                var a = r.timezone.exec(t);
                a ? (n.time = t.replace(a[1], ""),
                    n.timezone = a[1]) : n.time = t
            }
            return n
        }(i)
            , c = function (e, t) {
                var n, i = r.YYY[t], a = r.YYYYY[t];
                if (n = r.YYYY.exec(e) || a.exec(e)) {
                    var o = n[1];
                    return {
                        year: parseInt(o, 10),
                        restDateString: e.slice(o.length)
                    }
                }
                if (n = r.YY.exec(e) || i.exec(e)) {
                    var s = n[1];
                    return {
                        year: 100 * parseInt(s, 10),
                        restDateString: e.slice(s.length)
                    }
                }
                return {
                    year: null
                }
            }(l.date, u)
            , d = c.year
            , f = function (e, t) {
                if (null === t)
                    return null;
                var n, i, o, s;
                if (0 === e.length)
                    return (i = new Date(0)).setUTCFullYear(t),
                        i;
                if (n = r.MM.exec(e))
                    return i = new Date(0),
                        o = parseInt(n[1], 10) - 1,
                        i.setUTCFullYear(t, o),
                        i;
                if (n = r.DDD.exec(e)) {
                    i = new Date(0);
                    var u = parseInt(n[1], 10);
                    return i.setUTCFullYear(t, 0, u),
                        i
                }
                if (n = r.MMDD.exec(e)) {
                    i = new Date(0),
                        o = parseInt(n[1], 10) - 1;
                    var l = parseInt(n[2], 10);
                    return i.setUTCFullYear(t, o, l),
                        i
                }
                if (n = r.Www.exec(e))
                    return s = parseInt(n[1], 10) - 1,
                        a(t, s);
                if (n = r.WwwD.exec(e)) {
                    s = parseInt(n[1], 10) - 1;
                    var c = parseInt(n[2], 10) - 1;
                    return a(t, s, c)
                }
                return null
            }(c.restDateString, d);
        if (f) {
            var h, m = f.getTime(), p = 0;
            return l.time && (p = function (n) {
                var i, a, o;
                if (i = r.HH.exec(n))
                    return (a = parseFloat(i[1].replace(",", "."))) % 24 * e;
                if (i = r.HHMM.exec(n))
                    return a = parseInt(i[1], 10),
                        o = parseFloat(i[2].replace(",", ".")),
                        a % 24 * e + o * t;
                if (i = r.HHMMSS.exec(n)) {
                    a = parseInt(i[1], 10),
                        o = parseInt(i[2], 10);
                    var s = parseFloat(i[3].replace(",", "."));
                    return a % 24 * e + o * t + 1e3 * s
                }
                return null
            }(l.time)),
                l.timezone ? h = function (e) {
                    var t, n;
                    if (t = r.timezoneZ.exec(e))
                        return 0;
                    if (t = r.timezoneHH.exec(e))
                        return n = 60 * parseInt(t[2], 10),
                            "+" === t[1] ? -n : n;
                    if (t = r.timezoneHHMM.exec(e))
                        return n = 60 * parseInt(t[2], 10) + parseInt(t[3], 10),
                            "+" === t[1] ? -n : n;
                    return 0
                }(l.timezone) : (h = new Date(m + p).getTimezoneOffset(),
                    h = new Date(m + p + h * t).getTimezoneOffset()),
                new Date(m + p + h * t)
        }
        return new Date(i)
    }
    function a(e, t, n) {
        t = t || 0,
            n = n || 0;
        var r = new Date(0);
        r.setUTCFullYear(e, 0, 4);
        var i = 7 * t + n + 1 - (r.getUTCDay() || 7);
        return r.setUTCDate(r.getUTCDate() + i),
            r
    }
    function o(e) {
        e = e || {};
        var t = {};
        for (var n in e)
            e.hasOwnProperty(n) && (t[n] = e[n]);
        return t
    }
    var s = 6e4;
    function u(e, t, n) {
        if (arguments.length < 2)
            throw new TypeError("2 arguments required, but only " + arguments.length + " present");
        return function (e, t, n) {
            if (arguments.length < 2)
                throw new TypeError("2 arguments required, but only " + arguments.length + " present");
            var r = i(e, n).getTime()
                , a = Number(t);
            return new Date(r + a)
        }(e, Number(t) * s, n)
    }
    function l(e, t) {
        if (arguments.length < 1)
            throw new TypeError("1 argument required, but only " + arguments.length + " present");
        var n = i(e, t);
        return !isNaN(n)
    }
    var c = {
        lessThanXSeconds: {
            one: "less than a second",
            other: "less than {{count}} seconds"
        },
        xSeconds: {
            one: "1 second",
            other: "{{count}} seconds"
        },
        halfAMinute: "half a minute",
        lessThanXMinutes: {
            one: "less than a minute",
            other: "less than {{count}} minutes"
        },
        xMinutes: {
            one: "1 minute",
            other: "{{count}} minutes"
        },
        aboutXHours: {
            one: "about 1 hour",
            other: "about {{count}} hours"
        },
        xHours: {
            one: "1 hour",
            other: "{{count}} hours"
        },
        xDays: {
            one: "1 day",
            other: "{{count}} days"
        },
        aboutXMonths: {
            one: "about 1 month",
            other: "about {{count}} months"
        },
        xMonths: {
            one: "1 month",
            other: "{{count}} months"
        },
        aboutXYears: {
            one: "about 1 year",
            other: "about {{count}} years"
        },
        xYears: {
            one: "1 year",
            other: "{{count}} years"
        },
        overXYears: {
            one: "over 1 year",
            other: "over {{count}} years"
        },
        almostXYears: {
            one: "almost 1 year",
            other: "almost {{count}} years"
        }
    };
    var d = /MMMM|MM|DD|dddd/g;
    function f(e) {
        return e.replace(d, function (e) {
            return e.slice(1)
        })
    }
    var h = function (e) {
        var t = {
            LTS: e.LTS,
            LT: e.LT,
            L: e.L,
            LL: e.LL,
            LLL: e.LLL,
            LLLL: e.LLLL,
            l: e.l || f(e.L),
            ll: e.ll || f(e.LL),
            lll: e.lll || f(e.LLL),
            llll: e.llll || f(e.LLLL)
        };
        return function (e) {
            return t[e]
        }
    }({
        LT: "h:mm aa",
        LTS: "h:mm:ss aa",
        L: "MM/DD/YYYY",
        LL: "MMMM D YYYY",
        LLL: "MMMM D YYYY h:mm aa",
        LLLL: "dddd, MMMM D YYYY h:mm aa"
    })
        , m = {
            lastWeek: "[last] dddd [at] LT",
            yesterday: "[yesterday at] LT",
            today: "[today at] LT",
            tomorrow: "[tomorrow at] LT",
            nextWeek: "dddd [at] LT",
            other: "L"
        };
    function p(e, t, n) {
        return function (r, i) {
            var a = i || {}
                , o = a.type ? String(a.type) : t;
            return (e[o] || e[t])[n ? n(Number(r)) : Number(r)]
        }
    }
    function v(e, t) {
        return function (n) {
            var r = n || {}
                , i = r.type ? String(r.type) : t;
            return e[i] || e[t]
        }
    }
    var y = {
        narrow: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
        short: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
        long: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]
    }
        , g = {
            short: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            long: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
        }
        , b = {
            uppercase: ["AM", "PM"],
            lowercase: ["am", "pm"],
            long: ["a.m.", "p.m."]
        };
    function _(e, t) {
        return function (n, r) {
            var i = r || {}
                , a = i.type ? String(i.type) : t
                , o = e[a] || e[t];
            return String(n).match(o)
        }
    }
    function D(e, t) {
        return function (n, r) {
            var i = r || {}
                , a = i.type ? String(i.type) : t
                , o = e[a] || e[t]
                , s = n[1];
            return o.findIndex(function (e) {
                return e.test(s)
            })
        }
    }
    var w, $ = {
        formatDistance: function (e, t, n) {
            var r;
            return n = n || {},
                r = "string" == typeof c[e] ? c[e] : 1 === t ? c[e].one : c[e].other.replace("{{count}}", t),
                n.addSuffix ? n.comparison > 0 ? "in " + r : r + " ago" : r
        },
        formatLong: h,
        formatRelative: function (e, t, n, r) {
            return m[e]
        },
        localize: {
            ordinalNumber: function (e, t) {
                var n = Number(e)
                    , r = n % 100;
                if (r > 20 || r < 10)
                    switch (r % 10) {
                        case 1:
                            return n + "st";
                        case 2:
                            return n + "nd";
                        case 3:
                            return n + "rd"
                    }
                return n + "th"
            },
            weekday: p(y, "long"),
            weekdays: v(y, "long"),
            month: p(g, "long"),
            months: v(g, "long"),
            timeOfDay: p(b, "long", function (e) {
                return e / 12 >= 1 ? 1 : 0
            }),
            timesOfDay: v(b, "long")
        },
        match: {
            ordinalNumbers: (w = /^(\d+)(th|st|nd|rd)?/i,
                function (e) {
                    return String(e).match(w)
                }
            ),
            ordinalNumber: function (e) {
                return parseInt(e[1], 10)
            },
            weekdays: _({
                narrow: /^(su|mo|tu|we|th|fr|sa)/i,
                short: /^(sun|mon|tue|wed|thu|fri|sat)/i,
                long: /^(sunday|monday|tuesday|wednesday|thursday|friday|saturday)/i
            }, "long"),
            weekday: D({
                any: [/^su/i, /^m/i, /^tu/i, /^w/i, /^th/i, /^f/i, /^sa/i]
            }, "any"),
            months: _({
                short: /^(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)/i,
                long: /^(january|february|march|april|may|june|july|august|september|october|november|december)/i
            }, "long"),
            month: D({
                any: [/^ja/i, /^f/i, /^mar/i, /^ap/i, /^may/i, /^jun/i, /^jul/i, /^au/i, /^s/i, /^o/i, /^n/i, /^d/i]
            }, "any"),
            timesOfDay: _({
                short: /^(am|pm)/i,
                long: /^([ap]\.?\s?m\.?)/i
            }, "long"),
            timeOfDay: D({
                any: [/^a/i, /^p/i]
            }, "any")
        },
        options: {
            weekStartsOn: 0,
            firstWeekContainsDate: 1
        }
    }, T = 864e5;
    function M(e, t) {
        var n = i(e, t)
            , r = n.getTime();
        n.setUTCMonth(0, 1),
            n.setUTCHours(0, 0, 0, 0);
        var a = r - n.getTime();
        return Math.floor(a / T) + 1
    }
    function x(e, t) {
        var n = i(e, t)
            , r = n.getUTCDay()
            , a = (r < 1 ? 7 : 0) + r - 1;
        return n.setUTCDate(n.getUTCDate() - a),
            n.setUTCHours(0, 0, 0, 0),
            n
    }
    function A(e, t) {
        var n = i(e, t)
            , r = n.getUTCFullYear()
            , a = new Date(0);
        a.setUTCFullYear(r + 1, 0, 4),
            a.setUTCHours(0, 0, 0, 0);
        var o = x(a, t)
            , s = new Date(0);
        s.setUTCFullYear(r, 0, 4),
            s.setUTCHours(0, 0, 0, 0);
        var u = x(s, t);
        return n.getTime() >= o.getTime() ? r + 1 : n.getTime() >= u.getTime() ? r : r - 1
    }
    function I(e, t) {
        var n = A(e, t)
            , r = new Date(0);
        return r.setUTCFullYear(n, 0, 4),
            r.setUTCHours(0, 0, 0, 0),
            x(r, t)
    }
    var O = 6048e5;
    function F(e, t) {
        var n = i(e, t)
            , r = x(n, t).getTime() - I(n, t).getTime();
        return Math.round(r / O) + 1
    }
    var C = {
        M: function (e) {
            return e.getUTCMonth() + 1
        },
        Mo: function (e, t) {
            var n = e.getUTCMonth() + 1;
            return t.locale.localize.ordinalNumber(n, {
                unit: "month"
            })
        },
        MM: function (e) {
            return L(e.getUTCMonth() + 1, 2)
        },
        MMM: function (e, t) {
            return t.locale.localize.month(e.getUTCMonth(), {
                type: "short"
            })
        },
        MMMM: function (e, t) {
            return t.locale.localize.month(e.getUTCMonth(), {
                type: "long"
            })
        },
        Q: function (e) {
            return Math.ceil((e.getUTCMonth() + 1) / 3)
        },
        Qo: function (e, t) {
            var n = Math.ceil((e.getUTCMonth() + 1) / 3);
            return t.locale.localize.ordinalNumber(n, {
                unit: "quarter"
            })
        },
        D: function (e) {
            return e.getUTCDate()
        },
        Do: function (e, t) {
            return t.locale.localize.ordinalNumber(e.getUTCDate(), {
                unit: "dayOfMonth"
            })
        },
        DD: function (e) {
            return L(e.getUTCDate(), 2)
        },
        DDD: function (e) {
            return M(e)
        },
        DDDo: function (e, t) {
            return t.locale.localize.ordinalNumber(M(e), {
                unit: "dayOfYear"
            })
        },
        DDDD: function (e) {
            return L(M(e), 3)
        },
        dd: function (e, t) {
            return t.locale.localize.weekday(e.getUTCDay(), {
                type: "narrow"
            })
        },
        ddd: function (e, t) {
            return t.locale.localize.weekday(e.getUTCDay(), {
                type: "short"
            })
        },
        dddd: function (e, t) {
            return t.locale.localize.weekday(e.getUTCDay(), {
                type: "long"
            })
        },
        d: function (e) {
            return e.getUTCDay()
        },
        do: function (e, t) {
            return t.locale.localize.ordinalNumber(e.getUTCDay(), {
                unit: "dayOfWeek"
            })
        },
        E: function (e) {
            return e.getUTCDay() || 7
        },
        W: function (e) {
            return F(e)
        },
        Wo: function (e, t) {
            return t.locale.localize.ordinalNumber(F(e), {
                unit: "isoWeek"
            })
        },
        WW: function (e) {
            return L(F(e), 2)
        },
        YY: function (e) {
            return L(e.getUTCFullYear(), 4).substr(2)
        },
        YYYY: function (e) {
            return L(e.getUTCFullYear(), 4)
        },
        GG: function (e) {
            return String(A(e)).substr(2)
        },
        GGGG: function (e) {
            return A(e)
        },
        H: function (e) {
            return e.getUTCHours()
        },
        HH: function (e) {
            return L(e.getUTCHours(), 2)
        },
        h: function (e) {
            var t = e.getUTCHours();
            return 0 === t ? 12 : t > 12 ? t % 12 : t
        },
        hh: function (e) {
            return L(C.h(e), 2)
        },
        m: function (e) {
            return e.getUTCMinutes()
        },
        mm: function (e) {
            return L(e.getUTCMinutes(), 2)
        },
        s: function (e) {
            return e.getUTCSeconds()
        },
        ss: function (e) {
            return L(e.getUTCSeconds(), 2)
        },
        S: function (e) {
            return Math.floor(e.getUTCMilliseconds() / 100)
        },
        SS: function (e) {
            return L(Math.floor(e.getUTCMilliseconds() / 10), 2)
        },
        SSS: function (e) {
            return L(e.getUTCMilliseconds(), 3)
        },
        Z: function (e, t) {
            return N((t._originalDate || e).getTimezoneOffset(), ":")
        },
        ZZ: function (e, t) {
            return N((t._originalDate || e).getTimezoneOffset())
        },
        X: function (e, t) {
            var n = t._originalDate || e;
            return Math.floor(n.getTime() / 1e3)
        },
        x: function (e, t) {
            return (t._originalDate || e).getTime()
        },
        A: function (e, t) {
            return t.locale.localize.timeOfDay(e.getUTCHours(), {
                type: "uppercase"
            })
        },
        a: function (e, t) {
            return t.locale.localize.timeOfDay(e.getUTCHours(), {
                type: "lowercase"
            })
        },
        aa: function (e, t) {
            return t.locale.localize.timeOfDay(e.getUTCHours(), {
                type: "long"
            })
        }
    };
    function N(e, t) {
        t = t || "";
        var n = e > 0 ? "-" : "+"
            , r = Math.abs(e)
            , i = r % 60;
        return n + L(Math.floor(r / 60), 2) + t + L(i, 2)
    }
    function L(e, t) {
        for (var n = Math.abs(e).toString(); n.length < t;)
            n = "0" + n;
        return n
    }
    var Y = /(\[[^[]*])|(\\)?(LTS|LT|LLLL|LLL|LL|L|llll|lll|ll|l)/g
        , S = /(\[[^[]*])|(\\)?(x|ss|s|mm|m|hh|h|do|dddd|ddd|dd|d|aa|a|ZZ|Z|YYYY|YY|X|Wo|WW|W|SSS|SS|S|Qo|Q|Mo|MMMM|MMM|MM|M|HH|H|GGGG|GG|E|Do|DDDo|DDDD|DDD|DD|D|A|.)/g;
    function k(e, t, n) {
        if (arguments.length < 2)
            throw new TypeError("2 arguments required, but only " + arguments.length + " present");
        var r = String(t)
            , a = n || {}
            , s = a.locale || $;
        if (!s.localize)
            throw new RangeError("locale must contain localize property");
        if (!s.formatLong)
            throw new RangeError("locale must contain formatLong property");
        var u = s.formatters || {}
            , c = s.formattingTokensRegExp || S
            , d = s.formatLong
            , f = i(e, a);
        if (!l(f, a))
            return "Invalid Date";
        var h = f.getTimezoneOffset()
            , m = function (e, t, n) {
                var r = i(e, n)
                    , a = Number(t);
                return r.setUTCMinutes(r.getUTCMinutes() + a),
                    r
            }(f, -h, a)
            , p = o(a);
        return p.locale = s,
            p.formatters = C,
            p._originalDate = f,
            r.replace(Y, function (e) {
                return "[" === e[0] ? e : "\\" === e[0] ? E(e) : d(e)
            }).replace(c, function (e) {
                var t = u[e] || C[e];
                return t ? t(m, p) : E(e)
            })
    }
    function E(e) {
        return e.match(/\[[\s\S]/) ? e.replace(/^\[|]$/g, "") : e.replace(/\\/g, "")
    }
    function j(e, t, n) {
        if (arguments.length < 2)
            throw new TypeError("2 arguments required, but only " + arguments.length + " present");
        var r = i(e, n)
            , a = i(t, n);
        return r.getTime() > a.getTime()
    }
    function U(e, t, n) {
        if (arguments.length < 2)
            throw new TypeError("2 arguments required, but only " + arguments.length + " present");
        var r = i(e, n)
            , a = i(t, n);
        return r.getTime() < a.getTime()
    }
    function Z(e, t, n) {
        if (arguments.length < 2)
            throw new TypeError("2 arguments required, but only " + arguments.length + " present");
        var r = i(e, n)
            , a = i(t, n);
        return r.getTime() === a.getTime()
    }
    var z = {
        M: /^(1[0-2]|0?\d)/,
        D: /^(3[0-1]|[0-2]?\d)/,
        DDD: /^(36[0-6]|3[0-5]\d|[0-2]?\d?\d)/,
        W: /^(5[0-3]|[0-4]?\d)/,
        YYYY: /^(\d{1,4})/,
        H: /^(2[0-3]|[0-1]?\d)/,
        m: /^([0-5]?\d)/,
        Z: /^([+-])(\d{2}):(\d{2})/,
        ZZ: /^([+-])(\d{2})(\d{2})/,
        singleDigit: /^(\d)/,
        twoDigits: /^(\d{2})/,
        threeDigits: /^(\d{3})/,
        fourDigits: /^(\d{4})/,
        anyDigits: /^(\d+)/
    };
    function H(e) {
        return parseInt(e[1], 10)
    }
    var q = {
        YY: {
            unit: "twoDigitYear",
            match: z.twoDigits,
            parse: function (e) {
                return H(e)
            }
        },
        YYYY: {
            unit: "year",
            match: z.YYYY,
            parse: H
        },
        GG: {
            unit: "isoYear",
            match: z.twoDigits,
            parse: function (e) {
                return H(e) + 1900
            }
        },
        GGGG: {
            unit: "isoYear",
            match: z.YYYY,
            parse: H
        },
        Q: {
            unit: "quarter",
            match: z.singleDigit,
            parse: H
        },
        Qo: {
            unit: "quarter",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "quarter"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "quarter"
                })
            }
        },
        M: {
            unit: "month",
            match: z.M,
            parse: function (e) {
                return H(e) - 1
            }
        },
        Mo: {
            unit: "month",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "month"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "month"
                }) - 1
            }
        },
        MM: {
            unit: "month",
            match: z.twoDigits,
            parse: function (e) {
                return H(e) - 1
            }
        },
        MMM: {
            unit: "month",
            match: function (e, t) {
                return t.locale.match.months(e, {
                    type: "short"
                })
            },
            parse: function (e, t) {
                return t.locale.match.month(e, {
                    type: "short"
                })
            }
        },
        MMMM: {
            unit: "month",
            match: function (e, t) {
                return t.locale.match.months(e, {
                    type: "long"
                }) || t.locale.match.months(e, {
                    type: "short"
                })
            },
            parse: function (e, t) {
                var n = t.locale.match.month(e, {
                    type: "long"
                });
                return null == n && (n = t.locale.match.month(e, {
                    type: "short"
                })),
                    n
            }
        },
        W: {
            unit: "isoWeek",
            match: z.W,
            parse: H
        },
        Wo: {
            unit: "isoWeek",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "isoWeek"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "isoWeek"
                })
            }
        },
        WW: {
            unit: "isoWeek",
            match: z.twoDigits,
            parse: H
        },
        d: {
            unit: "dayOfWeek",
            match: z.singleDigit,
            parse: H
        },
        do: {
            unit: "dayOfWeek",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "dayOfWeek"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "dayOfWeek"
                })
            }
        },
        dd: {
            unit: "dayOfWeek",
            match: function (e, t) {
                return t.locale.match.weekdays(e, {
                    type: "narrow"
                })
            },
            parse: function (e, t) {
                return t.locale.match.weekday(e, {
                    type: "narrow"
                })
            }
        },
        ddd: {
            unit: "dayOfWeek",
            match: function (e, t) {
                return t.locale.match.weekdays(e, {
                    type: "short"
                }) || t.locale.match.weekdays(e, {
                    type: "narrow"
                })
            },
            parse: function (e, t) {
                var n = t.locale.match.weekday(e, {
                    type: "short"
                });
                return null == n && (n = t.locale.match.weekday(e, {
                    type: "narrow"
                })),
                    n
            }
        },
        dddd: {
            unit: "dayOfWeek",
            match: function (e, t) {
                return t.locale.match.weekdays(e, {
                    type: "long"
                }) || t.locale.match.weekdays(e, {
                    type: "short"
                }) || t.locale.match.weekdays(e, {
                    type: "narrow"
                })
            },
            parse: function (e, t) {
                var n = t.locale.match.weekday(e, {
                    type: "long"
                });
                return null == n && null == (n = t.locale.match.weekday(e, {
                    type: "short"
                })) && (n = t.locale.match.weekday(e, {
                    type: "narrow"
                })),
                    n
            }
        },
        E: {
            unit: "dayOfISOWeek",
            match: z.singleDigit,
            parse: function (e) {
                return H(e)
            }
        },
        D: {
            unit: "dayOfMonth",
            match: z.D,
            parse: H
        },
        Do: {
            unit: "dayOfMonth",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "dayOfMonth"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "dayOfMonth"
                })
            }
        },
        DD: {
            unit: "dayOfMonth",
            match: z.twoDigits,
            parse: H
        },
        DDD: {
            unit: "dayOfYear",
            match: z.DDD,
            parse: H
        },
        DDDo: {
            unit: "dayOfYear",
            match: function (e, t) {
                return t.locale.match.ordinalNumbers(e, {
                    unit: "dayOfYear"
                })
            },
            parse: function (e, t) {
                return t.locale.match.ordinalNumber(e, {
                    unit: "dayOfYear"
                })
            }
        },
        DDDD: {
            unit: "dayOfYear",
            match: z.threeDigits,
            parse: H
        },
        A: {
            unit: "timeOfDay",
            match: function (e, t) {
                return t.locale.match.timesOfDay(e, {
                    type: "short"
                })
            },
            parse: function (e, t) {
                return t.locale.match.timeOfDay(e, {
                    type: "short"
                })
            }
        },
        aa: {
            unit: "timeOfDay",
            match: function (e, t) {
                return t.locale.match.timesOfDay(e, {
                    type: "long"
                }) || t.locale.match.timesOfDay(e, {
                    type: "short"
                })
            },
            parse: function (e, t) {
                var n = t.locale.match.timeOfDay(e, {
                    type: "long"
                });
                return null == n && (n = t.locale.match.timeOfDay(e, {
                    type: "short"
                })),
                    n
            }
        },
        H: {
            unit: "hours",
            match: z.H,
            parse: H
        },
        HH: {
            unit: "hours",
            match: z.twoDigits,
            parse: H
        },
        h: {
            unit: "timeOfDayHours",
            match: z.M,
            parse: H
        },
        hh: {
            unit: "timeOfDayHours",
            match: z.twoDigits,
            parse: H
        },
        m: {
            unit: "minutes",
            match: z.m,
            parse: H
        },
        mm: {
            unit: "minutes",
            match: z.twoDigits,
            parse: H
        },
        s: {
            unit: "seconds",
            match: z.m,
            parse: H
        },
        ss: {
            unit: "seconds",
            match: z.twoDigits,
            parse: H
        },
        S: {
            unit: "milliseconds",
            match: z.singleDigit,
            parse: function (e) {
                return 100 * H(e)
            }
        },
        SS: {
            unit: "milliseconds",
            match: z.twoDigits,
            parse: function (e) {
                return 10 * H(e)
            }
        },
        SSS: {
            unit: "milliseconds",
            match: z.threeDigits,
            parse: H
        },
        Z: {
            unit: "timezone",
            match: z.Z,
            parse: function (e) {
                var t = e[1]
                    , n = 60 * parseInt(e[2], 10) + parseInt(e[3], 10);
                return "+" === t ? n : -n
            }
        },
        ZZ: {
            unit: "timezone",
            match: z.ZZ,
            parse: function (e) {
                var t = e[1]
                    , n = 60 * parseInt(e[2], 10) + parseInt(e[3], 10);
                return "+" === t ? n : -n
            }
        },
        X: {
            unit: "timestamp",
            match: z.anyDigits,
            parse: function (e) {
                return 1e3 * H(e)
            }
        },
        x: {
            unit: "timestamp",
            match: z.anyDigits,
            parse: H
        }
    };
    q.a = q.A;
    var V = 864e5;
    var W = {
        twoDigitYear: {
            priority: 10,
            set: function (e, t) {
                var n = 100 * Math.floor(e.date.getUTCFullYear() / 100) + t;
                return e.date.setUTCFullYear(n, 0, 1),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        year: {
            priority: 10,
            set: function (e, t) {
                return e.date.setUTCFullYear(t, 0, 1),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        isoYear: {
            priority: 10,
            set: function (e, t, n) {
                var r, a, o, s, u, l, c, d;
                return e.date = I((r = e.date,
                    a = t,
                    s = i(r, o = n),
                    u = Number(a),
                    l = I(s, o),
                    c = Math.floor((s.getTime() - l.getTime()) / V),
                    (d = new Date(0)).setUTCFullYear(u, 0, 4),
                    d.setUTCHours(0, 0, 0, 0),
                    (s = I(d, o)).setUTCDate(s.getUTCDate() + c),
                    s), n),
                    e
            }
        },
        quarter: {
            priority: 20,
            set: function (e, t) {
                return e.date.setUTCMonth(3 * (t - 1), 1),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        month: {
            priority: 30,
            set: function (e, t) {
                return e.date.setUTCMonth(t, 1),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        isoWeek: {
            priority: 40,
            set: function (e, t, n) {
                var r, a, o, s, u, l;
                return e.date = x((r = e.date,
                    a = t,
                    s = i(r, o = n),
                    u = Number(a),
                    l = F(s, o) - u,
                    s.setUTCDate(s.getUTCDate() - 7 * l),
                    s), n),
                    e
            }
        },
        dayOfWeek: {
            priority: 50,
            set: function (e, t, n) {
                return e.date = function (e, t, n) {
                    var r = n || {}
                        , a = r.locale
                        , o = a && a.options && a.options.weekStartsOn
                        , s = void 0 === o ? 0 : Number(o)
                        , u = void 0 === r.weekStartsOn ? s : Number(r.weekStartsOn);
                    if (!(u >= 0 && u <= 6))
                        throw new RangeError("weekStartsOn must be between 0 and 6 inclusively");
                    var l = i(e, n)
                        , c = Number(t)
                        , d = ((c % 7 + 7) % 7 < u ? 7 : 0) + c - l.getUTCDay();
                    return l.setUTCDate(l.getUTCDate() + d),
                        l
                }(e.date, t, n),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        dayOfISOWeek: {
            priority: 50,
            set: function (e, t, n) {
                return e.date = function (e, t, n) {
                    var r = Number(t);
                    r % 7 == 0 && (r -= 7);
                    var a = i(e, n)
                        , o = ((r % 7 + 7) % 7 < 1 ? 7 : 0) + r - a.getUTCDay();
                    return a.setUTCDate(a.getUTCDate() + o),
                        a
                }(e.date, t, n),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        dayOfMonth: {
            priority: 50,
            set: function (e, t) {
                return e.date.setUTCDate(t),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        dayOfYear: {
            priority: 50,
            set: function (e, t) {
                return e.date.setUTCMonth(0, t),
                    e.date.setUTCHours(0, 0, 0, 0),
                    e
            }
        },
        timeOfDay: {
            priority: 60,
            set: function (e, t, n) {
                return e.timeOfDay = t,
                    e
            }
        },
        hours: {
            priority: 70,
            set: function (e, t, n) {
                return e.date.setUTCHours(t, 0, 0, 0),
                    e
            }
        },
        timeOfDayHours: {
            priority: 70,
            set: function (e, t, n) {
                var r = e.timeOfDay;
                return null != r && (t = function (e, t) {
                    if (0 === t) {
                        if (12 === e)
                            return 0
                    } else if (12 !== e)
                        return 12 + e;
                    return e
                }(t, r)),
                    e.date.setUTCHours(t, 0, 0, 0),
                    e
            }
        },
        minutes: {
            priority: 80,
            set: function (e, t) {
                return e.date.setUTCMinutes(t, 0, 0),
                    e
            }
        },
        seconds: {
            priority: 90,
            set: function (e, t) {
                return e.date.setUTCSeconds(t, 0),
                    e
            }
        },
        milliseconds: {
            priority: 100,
            set: function (e, t) {
                return e.date.setUTCMilliseconds(t),
                    e
            }
        },
        timezone: {
            priority: 110,
            set: function (e, t) {
                return e.date = new Date(e.date.getTime() - 6e4 * t),
                    e
            }
        },
        timestamp: {
            priority: 120,
            set: function (e, t) {
                return e.date = new Date(t),
                    e
            }
        }
    }
        , P = 110
        , R = 6e4
        , G = /(\[[^[]*])|(\\)?(LTS|LT|LLLL|LLL|LL|L|llll|lll|ll|l)/g
        , B = /(\[[^[]*])|(\\)?(x|ss|s|mm|m|hh|h|do|dddd|ddd|dd|d|aa|a|ZZ|Z|YYYY|YY|X|Wo|WW|W|SSS|SS|S|Qo|Q|Mo|MMMM|MMM|MM|M|HH|H|GGGG|GG|E|Do|DDDo|DDDD|DDD|DD|D|A|.)/g;
    function X(e, t, n, r) {
        if (arguments.length < 3)
            throw new TypeError("3 arguments required, but only " + arguments.length + " present");
        var a = String(e)
            , s = r || {}
            , l = void 0 === s.weekStartsOn ? 0 : Number(s.weekStartsOn);
        if (!(l >= 0 && l <= 6))
            throw new RangeError("weekStartsOn must be between 0 and 6 inclusively");
        var c = s.locale || $
            , d = c.parsers || {}
            , f = c.units || {};
        if (!c.match)
            throw new RangeError("locale must contain match property");
        if (!c.formatLong)
            throw new RangeError("locale must contain formatLong property");
        var h = String(t).replace(G, function (e) {
            return "[" === e[0] ? e : "\\" === e[0] ? function (e) {
                if (e.match(/\[[\s\S]/))
                    return e.replace(/^\[|]$/g, "");
                return e.replace(/\\/g, "")
            }(e) : c.formatLong(e)
        });
        if ("" === h)
            return "" === a ? i(n, s) : new Date(NaN);
        var m = o(s);
        m.locale = c;
        var p, v = h.match(c.parsingTokensRegExp || B), y = v.length, g = [{
            priority: P,
            set: K,
            index: 0
        }];
        for (p = 0; p < y; p++) {
            var b = v[p]
                , _ = d[b] || q[b];
            if (_) {
                var D;
                if (!(D = _.match instanceof RegExp ? _.match.exec(a) : _.match(a, m)))
                    return new Date(NaN);
                var w = _.unit
                    , T = f[w] || W[w];
                g.push({
                    priority: T.priority,
                    set: T.set,
                    value: _.parse(D, m),
                    index: g.length
                });
                var M = D[0];
                a = a.slice(M.length)
            } else {
                var x = v[p].match(/^\[.*]$/) ? v[p].replace(/^\[|]$/g, "") : v[p];
                if (0 !== a.indexOf(x))
                    return new Date(NaN);
                a = a.slice(x.length)
            }
        }
        var A = g.map(function (e) {
            return e.priority
        }).sort(function (e, t) {
            return e - t
        }).filter(function (e, t, n) {
            return n.indexOf(e) === t
        }).map(function (e) {
            return g.filter(function (t) {
                return t.priority === e
            }).reverse()
        }).map(function (e) {
            return e[0]
        })
            , I = i(n, s);
        if (isNaN(I))
            return new Date(NaN);
        var O = {
            date: function (e, t, n) {
                if (arguments.length < 2)
                    throw new TypeError("2 arguments required, but only " + arguments.length + " present");
                return u(e, -Number(t), n)
            }(I, I.getTimezoneOffset())
        }
            , F = A.length;
        for (p = 0; p < F; p++) {
            var C = A[p];
            O = C.set(O, C.value, m)
        }
        return O.date
    }
    function K(e) {
        var t = e.date
            , n = t.getTime()
            , r = t.getTimezoneOffset();
        return r = new Date(n + r * R).getTimezoneOffset(),
            e.date = new Date(n + r * R),
            e
    }
    function Q(e, t) {
        if ("string" != typeof e)
            return l(e) ? e : null;
        var n = X(e, t, new Date);
        return l(n) && k(n, t) === e ? n : null
    }
    var J = {
        validate: function (e, t) {
            void 0 === t && (t = {});
            var n = t.targetValue
                , r = t.inclusion;
            void 0 === r && (r = !1);
            var i = t.format;
            return void 0 === i && (i = r,
                r = !1),
                e = Q(e, i),
                n = Q(n, i),
                !(!e || !n) && (j(e, n) || r && Z(e, n))
        },
        options: {
            hasTarget: !0,
            isDate: !0
        },
        paramNames: ["targetValue", "inclusion", "format"]
    }
        , ee = {
            en: /^[A-Z]*$/i,
            cs: /^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ]*$/i,
            da: /^[A-ZÆØÅ]*$/i,
            de: /^[A-ZÄÖÜß]*$/i,
            es: /^[A-ZÁÉÍÑÓÚÜ]*$/i,
            fr: /^[A-ZÀÂÆÇÉÈÊËÏÎÔŒÙÛÜŸ]*$/i,
            lt: /^[A-ZĄČĘĖĮŠŲŪŽ]*$/i,
            nl: /^[A-ZÉËÏÓÖÜ]*$/i,
            hu: /^[A-ZÁÉÍÓÖŐÚÜŰ]*$/i,
            pl: /^[A-ZĄĆĘŚŁŃÓŻŹ]*$/i,
            pt: /^[A-ZÃÁÀÂÇÉÊÍÕÓÔÚÜ]*$/i,
            ru: /^[А-ЯЁ]*$/i,
            sk: /^[A-ZÁÄČĎÉÍĹĽŇÓŔŠŤÚÝŽ]*$/i,
            sr: /^[A-ZČĆŽŠĐ]*$/i,
            tr: /^[A-ZÇĞİıÖŞÜ]*$/i,
            uk: /^[А-ЩЬЮЯЄІЇҐ]*$/i,
            ar: /^[ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهوىيًٌٍَُِّْٰ]*$/
        }
        , te = {
            en: /^[A-Z\s]*$/i,
            cs: /^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ\s]*$/i,
            da: /^[A-ZÆØÅ\s]*$/i,
            de: /^[A-ZÄÖÜß\s]*$/i,
            es: /^[A-ZÁÉÍÑÓÚÜ\s]*$/i,
            fr: /^[A-ZÀÂÆÇÉÈÊËÏÎÔŒÙÛÜŸ\s]*$/i,
            lt: /^[A-ZĄČĘĖĮŠŲŪŽ\s]*$/i,
            nl: /^[A-ZÉËÏÓÖÜ\s]*$/i,
            hu: /^[A-ZÁÉÍÓÖŐÚÜŰ\s]*$/i,
            pl: /^[A-ZĄĆĘŚŁŃÓŻŹ\s]*$/i,
            pt: /^[A-ZÃÁÀÂÇÉÊÍÕÓÔÚÜ\s]*$/i,
            ru: /^[А-ЯЁ\s]*$/i,
            sk: /^[A-ZÁÄČĎÉÍĹĽŇÓŔŠŤÚÝŽ\s]*$/i,
            sr: /^[A-ZČĆŽŠĐ\s]*$/i,
            tr: /^[A-ZÇĞİıÖŞÜ\s]*$/i,
            uk: /^[А-ЩЬЮЯЄІЇҐ\s]*$/i,
            ar: /^[ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهوىيًٌٍَُِّْٰ\s]*$/
        }
        , ne = {
            en: /^[0-9A-Z]*$/i,
            cs: /^[0-9A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ]*$/i,
            da: /^[0-9A-ZÆØÅ]$/i,
            de: /^[0-9A-ZÄÖÜß]*$/i,
            es: /^[0-9A-ZÁÉÍÑÓÚÜ]*$/i,
            fr: /^[0-9A-ZÀÂÆÇÉÈÊËÏÎÔŒÙÛÜŸ]*$/i,
            lt: /^[0-9A-ZĄČĘĖĮŠŲŪŽ]*$/i,
            hu: /^[0-9A-ZÁÉÍÓÖŐÚÜŰ]*$/i,
            nl: /^[0-9A-ZÉËÏÓÖÜ]*$/i,
            pl: /^[0-9A-ZĄĆĘŚŁŃÓŻŹ]*$/i,
            pt: /^[0-9A-ZÃÁÀÂÇÉÊÍÕÓÔÚÜ]*$/i,
            ru: /^[0-9А-ЯЁ]*$/i,
            sk: /^[0-9A-ZÁÄČĎÉÍĹĽŇÓŔŠŤÚÝŽ]*$/i,
            sr: /^[0-9A-ZČĆŽŠĐ]*$/i,
            tr: /^[0-9A-ZÇĞİıÖŞÜ]*$/i,
            uk: /^[0-9А-ЩЬЮЯЄІЇҐ]*$/i,
            ar: /^[٠١٢٣٤٥٦٧٨٩0-9ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهوىيًٌٍَُِّْٰ]*$/
        }
        , re = {
            en: /^[0-9A-Z_-]*$/i,
            cs: /^[0-9A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ_-]*$/i,
            da: /^[0-9A-ZÆØÅ_-]*$/i,
            de: /^[0-9A-ZÄÖÜß_-]*$/i,
            es: /^[0-9A-ZÁÉÍÑÓÚÜ_-]*$/i,
            fr: /^[0-9A-ZÀÂÆÇÉÈÊËÏÎÔŒÙÛÜŸ_-]*$/i,
            lt: /^[0-9A-ZĄČĘĖĮŠŲŪŽ_-]*$/i,
            nl: /^[0-9A-ZÉËÏÓÖÜ_-]*$/i,
            hu: /^[0-9A-ZÁÉÍÓÖŐÚÜŰ_-]*$/i,
            pl: /^[0-9A-ZĄĆĘŚŁŃÓŻŹ_-]*$/i,
            pt: /^[0-9A-ZÃÁÀÂÇÉÊÍÕÓÔÚÜ_-]*$/i,
            ru: /^[0-9А-ЯЁ_-]*$/i,
            sk: /^[0-9A-ZÁÄČĎÉÍĹĽŇÓŔŠŤÚÝŽ_-]*$/i,
            sr: /^[0-9A-ZČĆŽŠĐ_-]*$/i,
            tr: /^[0-9A-ZÇĞİıÖŞÜ_-]*$/i,
            uk: /^[0-9А-ЩЬЮЯЄІЇҐ_-]*$/i,
            ar: /^[٠١٢٣٤٥٦٧٨٩0-9ءآأؤإئابةتثجحخدذرزسشصضطظعغفقكلمنهوىيًٌٍَُِّْٰ_-]*$/
        }
        , ie = function (e, t) {
            void 0 === t && (t = {});
            var n = t.locale;
            return Array.isArray(e) ? e.every(function (e) {
                return ie(e, [n])
            }) : n ? (ee[n] || ee.en).test(e) : Object.keys(ee).some(function (t) {
                return ee[t].test(e)
            })
        }
        , ae = {
            validate: ie,
            paramNames: ["locale"]
        }
        , oe = function (e, t) {
            void 0 === t && (t = {});
            var n = t.locale;
            return Array.isArray(e) ? e.every(function (e) {
                return oe(e, [n])
            }) : n ? (re[n] || re.en).test(e) : Object.keys(re).some(function (t) {
                return re[t].test(e)
            })
        }
        , se = {
            validate: oe,
            paramNames: ["locale"]
        }
        , ue = function (e, t) {
            void 0 === t && (t = {});
            var n = t.locale;
            return Array.isArray(e) ? e.every(function (e) {
                return ue(e, [n])
            }) : n ? (ne[n] || ne.en).test(e) : Object.keys(ne).some(function (t) {
                return ne[t].test(e)
            })
        }
        , le = {
            validate: ue,
            paramNames: ["locale"]
        }
        , ce = function (e, t) {
            void 0 === t && (t = {});
            var n = t.locale;
            return Array.isArray(e) ? e.every(function (e) {
                return ce(e, [n])
            }) : n ? (te[n] || te.en).test(e) : Object.keys(te).some(function (t) {
                return te[t].test(e)
            })
        }
        , de = {
            validate: ce,
            paramNames: ["locale"]
        }
        , fe = {
            validate: function (e, t) {
                void 0 === t && (t = {});
                var n = t.targetValue
                    , r = t.inclusion;
                void 0 === r && (r = !1);
                var i = t.format;
                return void 0 === i && (i = r,
                    r = !1),
                    e = Q(e, i),
                    n = Q(n, i),
                    !(!e || !n) && (U(e, n) || r && Z(e, n))
            },
            options: {
                hasTarget: !0,
                isDate: !0
            },
            paramNames: ["targetValue", "inclusion", "format"]
        }
        , he = function (e, t) {
            void 0 === t && (t = {});
            var n = t.min
                , r = t.max;
            return Array.isArray(e) ? e.every(function (e) {
                return he(e, {
                    min: n,
                    max: r
                })
            }) : Number(n) <= e && Number(r) >= e
        }
        , me = {
            validate: he,
            paramNames: ["min", "max"]
        }
        , pe = {
            validate: function (e, t) {
                var n = t.targetValue;
                return String(e) === String(n)
            },
            options: {
                hasTarget: !0
            },
            paramNames: ["targetValue"]
        };
    function ve(e) {
        return e && e.__esModule && Object.prototype.hasOwnProperty.call(e, "default") ? e.default : e
    }
    function ye(e, t) {
        return e(t = {
            exports: {}
        }, t.exports),
            t.exports
    }
    var ge = ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function (e) {
                if (!("string" == typeof e || e instanceof String))
                    throw new TypeError("This library (validator.js) validates strings only")
            }
            ,
            e.exports = t.default
    });
    ve(ge);
    var be = ve(ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function (e) {
                (0,
                    r.default)(e);
                var t = e.replace(/[- ]+/g, "");
                if (!i.test(t))
                    return !1;
                for (var n = 0, a = void 0, o = void 0, s = void 0, u = t.length - 1; u >= 0; u--)
                    a = t.substring(u, u + 1),
                        o = parseInt(a, 10),
                        n += s && (o *= 2) >= 10 ? o % 10 + 1 : o,
                        s = !s;
                return !(n % 10 != 0 || !t)
            }
            ;
        var n, r = (n = ge) && n.__esModule ? n : {
            default: n
        };
        var i = /^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|(222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11}|6[27][0-9]{14})$/;
        e.exports = t.default
    }))
        , _e = {
            validate: function (e) {
                return be(String(e))
            }
        }
        , De = {
            validate: function (e, t) {
                void 0 === t && (t = {});
                var n = t.min
                    , r = t.max
                    , i = t.inclusivity;
                void 0 === i && (i = "()");
                var a = t.format;
                void 0 === a && (a = i,
                    i = "()");
                var o = Q(String(n), a)
                    , s = Q(String(r), a)
                    , u = Q(String(e), a);
                return !!(o && s && u) && ("()" === i ? j(u, o) && U(u, s) : "(]" === i ? j(u, o) && (Z(u, s) || U(u, s)) : "[)" === i ? U(u, s) && (Z(u, o) || j(u, o)) : Z(u, s) || Z(u, o) || U(u, s) && j(u, o))
            },
            options: {
                isDate: !0
            },
            paramNames: ["min", "max", "inclusivity", "format"]
        }
        , we = {
            validate: function (e, t) {
                return !!Q(e, t.format)
            },
            options: {
                isDate: !0
            },
            paramNames: ["format"]
        }
        , $e = function (e, t) {
            void 0 === t && (t = {});
            var n = t.decimals;
            void 0 === n && (n = "*");
            var r = t.separator;
            if (void 0 === r && (r = "."),
                Array.isArray(e))
                return e.every(function (e) {
                    return $e(e, {
                        decimals: n,
                        separator: r
                    })
                });
            if (null == e || "" === e)
                return !0;
            if (0 === Number(n))
                return /^-?\d*$/.test(e);
            if (!new RegExp("^-?\\d*(\\" + r + "\\d" + ("*" === n ? "+" : "{1," + n + "}") + ")?$").test(e))
                return !1;
            var i = parseFloat(e);
            return i == i
        }
        , Te = {
            validate: $e,
            paramNames: ["decimals", "separator"]
        }
        , Me = function (e, t) {
            var n = t[0];
            if (Array.isArray(e))
                return e.every(function (e) {
                    return Me(e, [n])
                });
            var r = String(e);
            return /^[0-9]*$/.test(r) && r.length === Number(n)
        }
        , xe = {
            validate: Me
        }
        , Ae = {
            validate: function (e, t) {
                for (var n = t[0], r = t[1], i = [], a = 0; a < e.length; a++) {
                    if (!/\.(jpg|svg|jpeg|png|bmp|gif)$/i.test(e[a].name))
                        return !1;
                    i.push(e[a])
                }
                return Promise.all(i.map(function (e) {
                    return function (e, t, n) {
                        var r = window.URL || window.webkitURL;
                        return new Promise(function (i) {
                            var a = new Image;
                            a.onerror = function () {
                                return i({
                                    valid: !1
                                })
                            }
                                ,
                                a.onload = function () {
                                    return i({
                                        valid: a.width === Number(t) && a.height === Number(n)
                                    })
                                }
                                ,
                                a.src = r.createObjectURL(e)
                        }
                        )
                    }(e, n, r)
                }))
            }
        }
        , Ie = ye(function (e, t) {
            Object.defineProperty(t, "__esModule", {
                value: !0
            }),
                t.default = function () {
                    var e = arguments.length > 0 && void 0 !== arguments[0] ? arguments[0] : {}
                        , t = arguments[1];
                    for (var n in t)
                        void 0 === e[n] && (e[n] = t[n]);
                    return e
                }
                ,
                e.exports = t.default
        });
    ve(Ie);
    var Oe = ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        });
        var n = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (e) {
            return typeof e
        }
            : function (e) {
                return e && "function" == typeof Symbol && e.constructor === Symbol && e !== Symbol.prototype ? "symbol" : typeof e
            }
            ;
        t.default = function (e, t) {
            (0,
                i.default)(e);
            var r = void 0
                , a = void 0;
            "object" === (void 0 === t ? "undefined" : n(t)) ? (r = t.min || 0,
                a = t.max) : (r = arguments[1],
                    a = arguments[2]);
            var o = encodeURI(e).split(/%..|./).length - 1;
            return o >= r && (void 0 === a || o <= a)
        }
            ;
        var r, i = (r = ge) && r.__esModule ? r : {
            default: r
        };
        e.exports = t.default
    });
    ve(Oe);
    var Fe = ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function (e, t) {
                (0,
                    n.default)(e),
                    (t = (0,
                        r.default)(t, a)).allow_trailing_dot && "." === e[e.length - 1] && (e = e.substring(0, e.length - 1));
                for (var i = e.split("."), o = 0; o < i.length; o++)
                    if (i[o].length > 63)
                        return !1;
                if (t.require_tld) {
                    var s = i.pop();
                    if (!i.length || !/^([a-z\u00a1-\uffff]{2,}|xn[a-z0-9-]{2,})$/i.test(s))
                        return !1;
                    if (/[\s\u2002-\u200B\u202F\u205F\u3000\uFEFF\uDB40\uDC20]/.test(s))
                        return !1
                }
                for (var u, l = 0; l < i.length; l++) {
                    if (u = i[l],
                        t.allow_underscores && (u = u.replace(/_/g, "")),
                        !/^[a-z\u00a1-\uffff0-9-]+$/i.test(u))
                        return !1;
                    if (/[\uff01-\uff5e]/.test(u))
                        return !1;
                    if ("-" === u[0] || "-" === u[u.length - 1])
                        return !1
                }
                return !0
            }
            ;
        var n = i(ge)
            , r = i(Ie);
        function i(e) {
            return e && e.__esModule ? e : {
                default: e
            }
        }
        var a = {
            require_tld: !0,
            allow_underscores: !1,
            allow_trailing_dot: !1
        };
        e.exports = t.default
    });
    ve(Fe);
    var Ce, Ne = ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function e(t) {
                var n = arguments.length > 1 && void 0 !== arguments[1] ? arguments[1] : "";
                (0,
                    r.default)(t);
                n = String(n);
                if (!n)
                    return e(t, 4) || e(t, 6);
                if ("4" === n) {
                    if (!i.test(t))
                        return !1;
                    var o = t.split(".").sort(function (e, t) {
                        return e - t
                    });
                    return o[3] <= 255
                }
                if ("6" === n) {
                    var s = t.split(":")
                        , u = !1
                        , l = e(s[s.length - 1], 4)
                        , c = l ? 7 : 8;
                    if (s.length > c)
                        return !1;
                    if ("::" === t)
                        return !0;
                    "::" === t.substr(0, 2) ? (s.shift(),
                        s.shift(),
                        u = !0) : "::" === t.substr(t.length - 2) && (s.pop(),
                            s.pop(),
                            u = !0);
                    for (var d = 0; d < s.length; ++d)
                        if ("" === s[d] && d > 0 && d < s.length - 1) {
                            if (u)
                                return !1;
                            u = !0
                        } else if (l && d === s.length - 1)
                            ;
                        else if (!a.test(s[d]))
                            return !1;
                    return u ? s.length >= 1 : s.length === c
                }
                return !1
            }
            ;
        var n, r = (n = ge) && n.__esModule ? n : {
            default: n
        };
        var i = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/
            , a = /^[0-9A-F]{1,4}$/i;
        e.exports = t.default
    }), Le = ve(Ne), Ye = ve(ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function (e, t) {
                if ((0,
                    n.default)(e),
                    (t = (0,
                        r.default)(t, u)).require_display_name || t.allow_display_name) {
                    var s = e.match(l);
                    if (s)
                        e = s[1];
                    else if (t.require_display_name)
                        return !1
                }
                var p = e.split("@")
                    , v = p.pop()
                    , y = p.join("@")
                    , g = v.toLowerCase();
                if (t.domain_specific_validation && ("gmail.com" === g || "googlemail.com" === g)) {
                    var b = (y = y.toLowerCase()).split("+")[0];
                    if (!(0,
                        i.default)(b.replace(".", ""), {
                            min: 6,
                            max: 30
                        }))
                        return !1;
                    for (var _ = b.split("."), D = 0; D < _.length; D++)
                        if (!d.test(_[D]))
                            return !1
                }
                if (!(0,
                    i.default)(y, {
                        max: 64
                    }) || !(0,
                        i.default)(v, {
                            max: 254
                        }))
                    return !1;
                if (!(0,
                    a.default)(v, {
                        require_tld: t.require_tld
                    })) {
                    if (!t.allow_ip_domain)
                        return !1;
                    if (!(0,
                        o.default)(v)) {
                        if (!v.startsWith("[") || !v.endsWith("]"))
                            return !1;
                        var w = v.substr(1, v.length - 2);
                        if (0 === w.length || !(0,
                            o.default)(w))
                            return !1
                    }
                }
                if ('"' === y[0])
                    return y = y.slice(1, y.length - 1),
                        t.allow_utf8_local_part ? m.test(y) : f.test(y);
                for (var $ = t.allow_utf8_local_part ? h : c, T = y.split("."), M = 0; M < T.length; M++)
                    if (!$.test(T[M]))
                        return !1;
                return !0
            }
            ;
        var n = s(ge)
            , r = s(Ie)
            , i = s(Oe)
            , a = s(Fe)
            , o = s(Ne);
        function s(e) {
            return e && e.__esModule ? e : {
                default: e
            }
        }
        var u = {
            allow_display_name: !1,
            require_display_name: !1,
            allow_utf8_local_part: !0,
            require_tld: !0
        }
            , l = /^[a-z\d!#\$%&'\*\+\-\/=\?\^_`{\|}~\.\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+[a-z\d!#\$%&'\*\+\-\/=\?\^_`{\|}~\,\.\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF\s]*<(.+)>$/i
            , c = /^[a-z\d!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i
            , d = /^[a-z\d]+$/
            , f = /^([\s\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e]|(\\[\x01-\x09\x0b\x0c\x0d-\x7f]))*$/i
            , h = /^[a-z\d!#\$%&'\*\+\-\/=\?\^_`{\|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+$/i
            , m = /^([\s\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|(\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*$/i;
        e.exports = t.default
    })), Se = {
        validate: function (e, t) {
            return void 0 === t && (t = {}),
                t.multiple && (e = e.split(",").map(function (e) {
                    return e.trim()
                })),
                Array.isArray(e) ? e.every(function (e) {
                    return Ye(String(e), t)
                }) : Ye(String(e), t)
        }
    }, ke = !0, Ee = function () {
        try {
            var e = Object.defineProperty({}, "passive", {
                get: function () {
                    ke = !0
                }
            });
            window.addEventListener("testPassive", null, e),
                window.removeEventListener("testPassive", null, e)
        } catch (e) {
            ke = !1
        }
        return ke
    }, je = function (e, t, n) {
        e.addEventListener(t, n, !!ke && {
            passive: !0
        })
    }, Ue = function (e) {
        return ct(["text", "password", "search", "email", "tel", "url", "textarea"], e.type)
    }, Ze = function (e) {
        return ct(["radio", "checkbox"], e.type)
    }, ze = function (e, t) {
        return e.getAttribute("data-vv-" + t)
    }, He = function () {
        for (var e = [], t = arguments.length; t--;)
            e[t] = arguments[t];
        return e.every(function (e) {
            return null == e
        })
    }, qe = function (e, t) {
        if (e instanceof RegExp && t instanceof RegExp)
            return qe(e.source, t.source) && qe(e.flags, t.flags);
        if (Array.isArray(e) && Array.isArray(t)) {
            if (e.length !== t.length)
                return !1;
            for (var n = 0; n < e.length; n++)
                if (!qe(e[n], t[n]))
                    return !1;
            return !0
        }
        return Ke(e) && Ke(t) ? Object.keys(e).every(function (n) {
            return qe(e[n], t[n])
        }) && Object.keys(t).every(function (n) {
            return qe(e[n], t[n])
        }) : e === t
    }, Ve = function (e) {
        return He(e) ? null : "FORM" === e.tagName ? e : He(e.form) ? He(e.parentNode) ? null : Ve(e.parentNode) : e.form
    }, We = function (e, t, n) {
        if (void 0 === n && (n = void 0),
            !e || !t)
            return n;
        var r = t;
        return e.split(".").every(function (e) {
            return e in r ? (r = r[e],
                !0) : (r = n,
                    !1)
        }),
            r
    }, Pe = function (e, t, n, r) {
        return void 0 === t && (t = 0),
            void 0 === n && (n = !1),
            void 0 === r && (r = {
                cancelled: !1
            }),
            0 === t ? e : function () {
                for (var a = [], o = arguments.length; o--;)
                    a[o] = arguments[o];
                var s = n && !i;
                clearTimeout(i),
                    i = setTimeout(function () {
                        i = null,
                            n || r.cancelled || e.apply(void 0, a)
                    }, t),
                    s && e.apply(void 0, a)
            }
            ;
        var i
    }, Re = function (e, t) {
        return t ? e ? ("string" == typeof t && (t = Ge(t)),
            nt({}, t, Ge(e))) : Ge(t) : Ge(e)
    }, Ge = function (e) {
        return e ? Ke(e) ? Object.keys(e).reduce(function (t, n) {
            var r = [];
            return r = !0 === e[n] ? [] : Array.isArray(e[n]) ? e[n] : Ke(e[n]) ? e[n] : [e[n]],
                !1 !== e[n] && (t[n] = r),
                t
        }, {}) : "string" != typeof e ? (Be("rules must be either a string or an object."),
            {}) : e.split("|").reduce(function (e, t) {
                var n = function (e) {
                    var t = []
                        , n = e.split(":")[0];
                    return ct(e, ":") && (t = e.split(":").slice(1).join(":").split(",")),
                    {
                        name: n,
                        params: t
                    }
                }(t);
                return n.name ? (e[n.name] = n.params,
                    e) : e
            }, {}) : {}
    }, Be = function (e) {
        console.warn("[vee-validate] " + e)
    }, Xe = function (e) {
        return new Error("[vee-validate] " + e)
    }, Ke = function (e) {
        return null !== e && e && "object" == typeof e && !Array.isArray(e)
    }, Qe = function (e) {
        return "function" == typeof e
    }, Je = function (e, t) {
        return e.classList ? e.classList.contains(t) : !!e.className.match(new RegExp("(\\s|^)" + t + "(\\s|$)"))
    }, et = function (e, t, n) {
        if (e && t) {
            if (!Array.isArray(t))
                return n ? function (e, t) {
                    e.classList ? e.classList.add(t) : Je(e, t) || (e.className += " " + t)
                }(e, t) : void function (e, t) {
                    if (e.classList)
                        e.classList.remove(t);
                    else if (Je(e, t)) {
                        var n = new RegExp("(\\s|^)" + t + "(\\s|$)");
                        e.className = e.className.replace(n, " ")
                    }
                }(e, t);
            t.forEach(function (t) {
                return et(e, t, n)
            })
        }
    }, tt = function (e) {
        if (Qe(Array.from))
            return Array.from(e);
        for (var t = [], n = e.length, r = 0; r < n; r++)
            t.push(e[r]);
        return t
    }, nt = function (e) {
        for (var t = [], n = arguments.length - 1; n-- > 0;)
            t[n] = arguments[n + 1];
        if (Qe(Object.assign))
            return Object.assign.apply(Object, [e].concat(t));
        if (null == e)
            throw new TypeError("Cannot convert undefined or null to object");
        var r = Object(e);
        return t.forEach(function (e) {
            null != e && Object.keys(e).forEach(function (t) {
                r[t] = e[t]
            })
        }),
            r
    }, rt = 0, it = "{id}", at = function (e, t) {
        for (var n = Array.isArray(e) ? e : tt(e), r = 0; r < n.length; r++)
            if (t(n[r]))
                return n[r]
    }, ot = function (e) {
        if (!e)
            return !1;
        var t = e.componentOptions.tag;
        return /^(keep-alive|transition|transition-group)$/.test(t)
    }, st = function (e) {
        if ("number" == typeof e)
            return e;
        if ("string" == typeof e)
            return parseInt(e);
        var t = {};
        for (var n in e)
            t[n] = parseInt(e[n]);
        return t
    }, ut = function (e, t) {
        return Ke(e) && Ke(t) ? (Object.keys(t).forEach(function (n) {
            var r, i;
            if (Ke(t[n]))
                return e[n] || nt(e, ((r = {})[n] = {},
                    r)),
                    void ut(e[n], t[n]);
            nt(e, ((i = {})[n] = t[n],
                i))
        }),
            e) : e
    }, lt = function (e, t) {
        if (e.required && (t = Re("required", t)),
            Ue(e))
            return "email" === e.type && (t = Re("email" + (e.multiple ? ":multiple" : ""), t)),
                e.pattern && (t = Re({
                    regex: e.pattern
                }, t)),
                e.maxLength >= 0 && e.maxLength < 524288 && (t = Re("max:" + e.maxLength, t)),
                e.minLength > 0 && (t = Re("min:" + e.minLength, t)),
                t;
        if ("number" === e.type)
            return t = Re("decimal", t),
                "" !== e.min && (t = Re("min_value:" + e.min, t)),
                "" !== e.max && (t = Re("max_value:" + e.max, t)),
                t;
        if (function (e) {
            return ct(["date", "week", "month", "datetime-local", "time"], e.type)
        }(e)) {
            var n = e.step && Number(e.step) < 60 ? "HH:mm:ss" : "HH:mm";
            if ("date" === e.type)
                return Re("date_format:YYYY-MM-DD", t);
            if ("datetime-local" === e.type)
                return Re("date_format:YYYY-MM-DDT" + n, t);
            if ("month" === e.type)
                return Re("date_format:YYYY-MM", t);
            if ("week" === e.type)
                return Re("date_format:YYYY-[W]WW", t);
            if ("time" === e.type)
                return Re("date_format:" + n, t)
        }
        return t
    }, ct = function (e, t) {
        return -1 !== e.indexOf(t)
    }, dt = function (e, t) {
        return Array.isArray(e) ? e.every(function (e) {
            return dt(e, t)
        }) : tt(t).some(function (t) {
            return t == e
        })
    }, ft = {
        validate: dt
    }, ht = {
        validate: function () {
            for (var e = [], t = arguments.length; t--;)
                e[t] = arguments[t];
            return !dt.apply(void 0, e)
        }
    }, mt = {
        validate: function (e, t) {
            var n = new RegExp(".(" + t.join("|") + ")$", "i");
            return e.every(function (e) {
                return n.test(e.name)
            })
        }
    }, pt = {
        validate: function (e) {
            return e.every(function (e) {
                return /\.(jpg|svg|jpeg|png|bmp|gif)$/i.test(e.name)
            })
        }
    }, vt = {
        validate: function (e) {
            return Array.isArray(e) ? e.every(function (e) {
                return /^-?[0-9]+$/.test(String(e))
            }) : /^-?[0-9]+$/.test(String(e))
        }
    }, yt = {
        validate: function (e, t) {
            void 0 === t && (t = {});
            var n = t.version;
            return void 0 === n && (n = 4),
                He(e) && (e = ""),
                Array.isArray(e) ? e.every(function (e) {
                    return Le(e, n)
                }) : Le(e, n)
        },
        paramNames: ["version"]
    }, gt = {
        validate: function (e, t) {
            return void 0 === t && (t = []),
                e === t[0]
        }
    }, bt = {
        validate: function (e, t) {
            return void 0 === t && (t = []),
                e !== t[0]
        }
    }, _t = {
        validate: function (e, t) {
            var n = t[0]
                , r = t[1];
            return void 0 === r && (r = void 0),
                n = Number(n),
                null != e && ("number" == typeof e && (e = String(e)),
                    e.length || (e = tt(e)),
                    function (e, t, n) {
                        return void 0 === n ? e.length === t : (n = Number(n),
                            e.length >= t && e.length <= n)
                    }(e, n, r))
        }
    }, Dt = function (e, t) {
        var n = t[0];
        return null == e ? n >= 0 : Array.isArray(e) ? e.every(function (e) {
            return Dt(e, [n])
        }) : String(e).length <= n
    }, wt = {
        validate: Dt
    }, $t = function (e, t) {
        var n = t[0];
        return null != e && "" !== e && (Array.isArray(e) ? e.length > 0 && e.every(function (e) {
            return $t(e, [n])
        }) : Number(e) <= n)
    }, Tt = {
        validate: $t
    }, Mt = {
        validate: function (e, t) {
            var n = new RegExp(t.join("|").replace("*", ".+") + "$", "i");
            return e.every(function (e) {
                return n.test(e.type)
            })
        }
    }, xt = function (e, t) {
        var n = t[0];
        return null != e && (Array.isArray(e) ? e.every(function (e) {
            return xt(e, [n])
        }) : String(e).length >= n)
    }, At = {
        validate: xt
    }, It = function (e, t) {
        var n = t[0];
        return null != e && "" !== e && (Array.isArray(e) ? e.length > 0 && e.every(function (e) {
            return It(e, [n])
        }) : Number(e) >= n)
    }, Ot = {
        validate: It
    }, Ft = {
        validate: function (e) {
            return Array.isArray(e) ? e.every(function (e) {
                return /^[0-9]+$/.test(String(e))
            }) : /^[0-9]+$/.test(String(e))
        }
    }, Ct = {
        validate: function (e, t) {
            var n = t.expression;
            return "string" == typeof n && (n = new RegExp(n)),
                n.test(String(e))
        },
        paramNames: ["expression"]
    }, Nt = {
        validate: function (e, t) {
            void 0 === t && (t = []);
            var n = t[0];
            return void 0 === n && (n = !1),
                Array.isArray(e) ? !!e.length : !(!1 === e && n || null == e || !String(e).trim().length)
        }
    }, Lt = {
        validate: function (e, t) {
            var n = t[0];
            if (isNaN(n))
                return !1;
            for (var r = 1024 * Number(n), i = 0; i < e.length; i++)
                if (e[i].size > r)
                    return !1;
            return !0
        }
    }, Yt = ve(ye(function (e, t) {
        Object.defineProperty(t, "__esModule", {
            value: !0
        }),
            t.default = function (e, t) {
                if ((0,
                    n.default)(e),
                    !e || e.length >= 2083 || /[\s<>]/.test(e))
                    return !1;
                if (0 === e.indexOf("mailto:"))
                    return !1;
                t = (0,
                    a.default)(t, s);
                var o = void 0
                    , c = void 0
                    , d = void 0
                    , f = void 0
                    , h = void 0
                    , m = void 0
                    , p = void 0
                    , v = void 0;
                if (p = e.split("#"),
                    e = p.shift(),
                    p = e.split("?"),
                    e = p.shift(),
                    (p = e.split("://")).length > 1) {
                    if (o = p.shift().toLowerCase(),
                        t.require_valid_protocol && -1 === t.protocols.indexOf(o))
                        return !1
                } else {
                    if (t.require_protocol)
                        return !1;
                    if ("//" === e.substr(0, 2)) {
                        if (!t.allow_protocol_relative_urls)
                            return !1;
                        p[0] = e.substr(2)
                    }
                }
                if ("" === (e = p.join("://")))
                    return !1;
                if (p = e.split("/"),
                    "" === (e = p.shift()) && !t.require_host)
                    return !0;
                if ((p = e.split("@")).length > 1 && (c = p.shift()).indexOf(":") >= 0 && c.split(":").length > 2)
                    return !1;
                f = p.join("@"),
                    m = null,
                    v = null;
                var y = f.match(u);
                y ? (d = "",
                    v = y[1],
                    m = y[2] || null) : (p = f.split(":"),
                        d = p.shift(),
                        p.length && (m = p.join(":")));
                if (null !== m && (h = parseInt(m, 10),
                    !/^[0-9]+$/.test(m) || h <= 0 || h > 65535))
                    return !1;
                if (!((0,
                    i.default)(d) || (0,
                        r.default)(d, t) || v && (0,
                            i.default)(v, 6)))
                    return !1;
                if (d = d || v,
                    t.host_whitelist && !l(d, t.host_whitelist))
                    return !1;
                if (t.host_blacklist && l(d, t.host_blacklist))
                    return !1;
                return !0
            }
            ;
        var n = o(ge)
            , r = o(Fe)
            , i = o(Ne)
            , a = o(Ie);
        function o(e) {
            return e && e.__esModule ? e : {
                default: e
            }
        }
        var s = {
            protocols: ["http", "https", "ftp"],
            require_tld: !0,
            require_protocol: !1,
            require_host: !0,
            require_valid_protocol: !0,
            allow_underscores: !1,
            allow_trailing_dot: !1,
            allow_protocol_relative_urls: !1
        }
            , u = /^\[([^\]]+)\](?::([0-9]+))?$/;
        function l(e, t) {
            for (var n = 0; n < t.length; n++) {
                var r = t[n];
                if (e === r || (i = r,
                    "[object RegExp]" === Object.prototype.toString.call(i) && r.test(e)))
                    return !0
            }
            var i;
            return !1
        }
        e.exports = t.default
    })), St = {
        validate: function (e, t) {
            return void 0 === t && (t = {}),
                He(e) && (e = ""),
                Array.isArray(e) ? e.every(function (e) {
                    return Yt(e, t)
                }) : Yt(e, t)
        }
    }, kt = Object.freeze({
        after: J,
        alpha_dash: se,
        alpha_num: le,
        alpha_spaces: de,
        alpha: ae,
        before: fe,
        between: me,
        confirmed: pe,
        credit_card: _e,
        date_between: De,
        date_format: we,
        decimal: Te,
        digits: xe,
        dimensions: Ae,
        email: Se,
        ext: mt,
        image: pt,
        included: ft,
        integer: vt,
        length: _t,
        ip: yt,
        is_not: bt,
        is: gt,
        max: wt,
        max_value: Tt,
        mimes: Mt,
        min: At,
        min_value: Ot,
        excluded: ht,
        numeric: Ft,
        regex: Ct,
        required: Nt,
        size: Lt,
        url: St
    }), Et = {
        name: "en",
        messages: {
            _default: function (e) {
                return "The " + e + " value is not valid."
            },
            after: function (e, t) {
                var n = t[0];
                return "The " + e + " must be after " + (t[1] ? "or equal to " : "") + n + "."
            },
            alpha_dash: function (e) {
                return "The " + e + " field may contain alpha-numeric characters as well as dashes and underscores."
            },
            alpha_num: function (e) {
                return "The " + e + " field may only contain alpha-numeric characters."
            },
            alpha_spaces: function (e) {
                return "The " + e + " field may only contain alphabetic characters as well as spaces."
            },
            alpha: function (e) {
                return "The " + e + " field may only contain alphabetic characters."
            },
            before: function (e, t) {
                var n = t[0];
                return "The " + e + " must be before " + (t[1] ? "or equal to " : "") + n + "."
            },
            between: function (e, t) {
                return "The " + e + " field must be between " + t[0] + " and " + t[1] + "."
            },
            confirmed: function (e) {
                return "The " + e + " confirmation does not match."
            },
            credit_card: function (e) {
                return "The " + e + " field is invalid."
            },
            date_between: function (e, t) {
                return "The " + e + " must be between " + t[0] + " and " + t[1] + "."
            },
            date_format: function (e, t) {
                return "The " + e + " must be in the format " + t[0] + "."
            },
            decimal: function (e, t) {
                void 0 === t && (t = []);
                var n = t[0];
                return void 0 === n && (n = "*"),
                    "The " + e + " field must be numeric and may contain " + (n && "*" !== n ? n : "") + " decimal points."
            },
            digits: function (e, t) {
                return "The " + e + " field must be numeric and exactly contain " + t[0] + " digits."
            },
            dimensions: function (e, t) {
                return "The " + e + " field must be " + t[0] + " pixels by " + t[1] + " pixels."
            },
            email: function (e) {
                return "The " + e + " field must be a valid email."
            },
            ext: function (e) {
                return "The " + e + " field must be a valid file."
            },
            image: function (e) {
                return "The " + e + " field must be an image."
            },
            included: function (e) {
                return "The " + e + " field must be a valid value."
            },
            integer: function (e) {
                return "The " + e + " field must be an integer."
            },
            ip: function (e) {
                return "The " + e + " field must be a valid ip address."
            },
            length: function (e, t) {
                var n = t[0]
                    , r = t[1];
                return r ? "The " + e + " length must be between " + n + " and " + r + "." : "The " + e + " length must be " + n + "."
            },
            max: function (e, t) {
                return "The " + e + " field may not be greater than " + t[0] + " characters."
            },
            max_value: function (e, t) {
                return "The " + e + " field must be " + t[0] + " or less."
            },
            mimes: function (e) {
                return "The " + e + " field must have a valid file type."
            },
            min: function (e, t) {
                return "The " + e + " field must be at least " + t[0] + " characters."
            },
            min_value: function (e, t) {
                return "The " + e + " field must be " + t[0] + " or more."
            },
            excluded: function (e) {
                return "The " + e + " field must be a valid value."
            },
            numeric: function (e) {
                return "The " + e + " field may only contain numeric characters."
            },
            regex: function (e) {
                return "The " + e + " field format is invalid."
            },
            required: function (e) {
                return "The " + e + " field is required."
            },
            size: function (e, t) {
                return "The " + e + " size must be less than " + function (e) {
                    var t = 0 == (e = 1024 * Number(e)) ? 0 : Math.floor(Math.log(e) / Math.log(1024));
                    return 1 * (e / Math.pow(1024, t)).toFixed(2) + " " + ["Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"][t]
                }(t[0]) + "."
            },
            url: function (e) {
                return "The " + e + " field is not a valid URL."
            }
        },
        attributes: {}
    };
    "undefined" != typeof VeeValidate && VeeValidate.Validator.localize(((Ce = {})[Et.name] = Et,
        Ce));
    var jt = "en"
        , Ut = function (e) {
            void 0 === e && (e = {}),
                this.container = {},
                this.merge(e)
        }
        , Zt = {
            locale: {
                configurable: !0
            }
        };
    Zt.locale.get = function () {
        return jt
    }
        ,
        Zt.locale.set = function (e) {
            jt = e || "en"
        }
        ,
        Ut.prototype.hasLocale = function (e) {
            return !!this.container[e]
        }
        ,
        Ut.prototype.setDateFormat = function (e, t) {
            this.container[e] || (this.container[e] = {}),
                this.container[e].dateFormat = t
        }
        ,
        Ut.prototype.getDateFormat = function (e) {
            return this.container[e] && this.container[e].dateFormat ? this.container[e].dateFormat : null
        }
        ,
        Ut.prototype.getMessage = function (e, t, n) {
            var r = null;
            return r = this.hasMessage(e, t) ? this.container[e].messages[t] : this._getDefaultMessage(e),
                Qe(r) ? r.apply(void 0, n) : r
        }
        ,
        Ut.prototype.getFieldMessage = function (e, t, n, r) {
            if (!this.hasLocale(e))
                return this.getMessage(e, n, r);
            var i = this.container[e].custom && this.container[e].custom[t];
            if (!i || !i[n])
                return this.getMessage(e, n, r);
            var a = i[n];
            return Qe(a) ? a.apply(void 0, r) : a
        }
        ,
        Ut.prototype._getDefaultMessage = function (e) {
            return this.hasMessage(e, "_default") ? this.container[e].messages._default : this.container.en.messages._default
        }
        ,
        Ut.prototype.getAttribute = function (e, t, n) {
            return void 0 === n && (n = ""),
                this.hasAttribute(e, t) ? this.container[e].attributes[t] : n
        }
        ,
        Ut.prototype.hasMessage = function (e, t) {
            return !!(this.hasLocale(e) && this.container[e].messages && this.container[e].messages[t])
        }
        ,
        Ut.prototype.hasAttribute = function (e, t) {
            return !!(this.hasLocale(e) && this.container[e].attributes && this.container[e].attributes[t])
        }
        ,
        Ut.prototype.merge = function (e) {
            ut(this.container, e)
        }
        ,
        Ut.prototype.setMessage = function (e, t, n) {
            this.hasLocale(e) || (this.container[e] = {
                messages: {},
                attributes: {}
            }),
                this.container[e].messages[t] = n
        }
        ,
        Ut.prototype.setAttribute = function (e, t, n) {
            this.hasLocale(e) || (this.container[e] = {
                messages: {},
                attributes: {}
            }),
                this.container[e].attributes[t] = n
        }
        ,
        Object.defineProperties(Ut.prototype, Zt);
    var zt = function (e) {
        return Ke(e) ? Object.keys(e).reduce(function (t, n) {
            return t[n] = zt(e[n]),
                t
        }, {}) : Qe(e) ? e("{0}", ["{1}", "{2}", "{3}"]) : e
    }
        , Ht = function (e, t) {
            this.i18n = e,
                this.rootKey = t
        }
        , qt = {
            locale: {
                configurable: !0
            }
        };
    qt.locale.get = function () {
        return this.i18n.locale
    }
        ,
        qt.locale.set = function (e) {
            Be("Cannot set locale from the validator when using vue-i18n, use i18n.locale setter instead")
        }
        ,
        Ht.prototype.getDateFormat = function (e) {
            return this.i18n.getDateTimeFormat(e || this.locale)
        }
        ,
        Ht.prototype.setDateFormat = function (e, t) {
            this.i18n.setDateTimeFormat(e || this.locale, t)
        }
        ,
        Ht.prototype.getMessage = function (e, t, n) {
            var r = this.rootKey + ".messages." + t;
            return this.i18n.te(r) ? this.i18n.t(r, e, n) : this.i18n.t(this.rootKey + ".messages._default", e, n)
        }
        ,
        Ht.prototype.getAttribute = function (e, t, n) {
            void 0 === n && (n = "");
            var r = this.rootKey + ".attributes." + t;
            return this.i18n.te(r) ? this.i18n.t(r, e) : n
        }
        ,
        Ht.prototype.getFieldMessage = function (e, t, n, r) {
            var i = this.rootKey + ".custom." + t + "." + n;
            return this.i18n.te(i) ? this.i18n.t(i, e, r) : this.getMessage(e, n, r)
        }
        ,
        Ht.prototype.merge = function (e) {
            var t = this;
            Object.keys(e).forEach(function (n) {
                var r, i = ut({}, We(n + "." + t.rootKey, t.i18n.messages, {})), a = ut(i, function (e) {
                    var t = {};
                    return e.messages && (t.messages = zt(e.messages)),
                        e.custom && (t.custom = zt(e.custom)),
                        e.attributes && (t.attributes = e.attributes),
                        He(e.dateFormat) || (t.dateFormat = e.dateFormat),
                        t
                }(e[n]));
                t.i18n.mergeLocaleMessage(n, ((r = {})[t.rootKey] = a,
                    r)),
                    a.dateFormat && t.i18n.setDateTimeFormat(n, a.dateFormat)
            })
        }
        ,
        Ht.prototype.setMessage = function (e, t, n) {
            var r, i;
            this.merge(((i = {})[e] = {
                messages: (r = {},
                    r[t] = n,
                    r)
            },
                i))
        }
        ,
        Ht.prototype.setAttribute = function (e, t, n) {
            var r, i;
            this.merge(((i = {})[e] = {
                attributes: (r = {},
                    r[t] = n,
                    r)
            },
                i))
        }
        ,
        Object.defineProperties(Ht.prototype, qt);
    var Vt = {
        locale: "en",
        delay: 0,
        errorBagName: "errors",
        dictionary: null,
        strict: !0,
        fieldsBagName: "fields",
        classes: !1,
        classNames: null,
        events: "input",
        inject: !0,
        fastExit: !0,
        aria: !0,
        validity: !1,
        i18n: null,
        i18nRootKey: "validation"
    }
        , Wt = nt({}, Vt)
        , Pt = {
            dictionary: new Ut({
                en: {
                    messages: {},
                    attributes: {},
                    custom: {}
                }
            })
        }
        , Rt = function () { }
        , Gt = {
            default: {
                configurable: !0
            },
            current: {
                configurable: !0
            }
        };
    Gt.default.get = function () {
        return Vt
    }
        ,
        Gt.current.get = function () {
            return Wt
        }
        ,
        Rt.dependency = function (e) {
            return Pt[e]
        }
        ,
        Rt.merge = function (e) {
            (Wt = nt({}, Wt, e)).i18n && Rt.register("dictionary", new Ht(Wt.i18n, Wt.i18nRootKey))
        }
        ,
        Rt.register = function (e, t) {
            Pt[e] = t
        }
        ,
        Rt.resolve = function (e) {
            var t = We("$options.$_veeValidate", e, {});
            return nt({}, Rt.current, t)
        }
        ,
        Object.defineProperties(Rt, Gt);
    var Bt = function e(t, n) {
        void 0 === t && (t = null),
            void 0 === n && (n = null),
            this.vmId = n || null,
            this.items = t && t instanceof e ? t.items : []
    };
    Bt.prototype["function" == typeof Symbol ? Symbol.iterator : "@@iterator"] = function () {
        var e = this
            , t = 0;
        return {
            next: function () {
                return {
                    value: e.items[t++],
                    done: t > e.items.length
                }
            }
        }
    }
        ,
        Bt.prototype.add = function (e) {
            var t;
            (t = this.items).push.apply(t, this._normalizeError(e))
        }
        ,
        Bt.prototype._normalizeError = function (e) {
            var t = this;
            return Array.isArray(e) ? e.map(function (e) {
                return e.scope = He(e.scope) ? null : e.scope,
                    e.vmId = He(e.vmId) ? t.vmId || null : e.vmId,
                    e
            }) : (e.scope = He(e.scope) ? null : e.scope,
                e.vmId = He(e.vmId) ? this.vmId || null : e.vmId,
                [e])
        }
        ,
        Bt.prototype.regenerate = function () {
            this.items.forEach(function (e) {
                e.msg = Qe(e.regenerate) ? e.regenerate() : e.msg
            })
        }
        ,
        Bt.prototype.update = function (e, t) {
            var n = at(this.items, function (t) {
                return t.id === e
            });
            if (n) {
                var r = this.items.indexOf(n);
                this.items.splice(r, 1),
                    n.scope = t.scope,
                    this.items.push(n)
            }
        }
        ,
        Bt.prototype.all = function (e) {
            var t = this;
            return this.items.filter(function (n) {
                var r = !0
                    , i = !0;
                return He(e) || (r = n.scope === e),
                    He(t.vmId) || (i = n.vmId === t.vmId),
                    i && r
            }).map(function (e) {
                return e.msg
            })
        }
        ,
        Bt.prototype.any = function (e) {
            var t = this;
            return !!this.items.filter(function (n) {
                var r = !0;
                return He(e) || (r = n.scope === e),
                    He(t.vmId) || (r = n.vmId === t.vmId),
                    r
            }).length
        }
        ,
        Bt.prototype.clear = function (e) {
            var t = this
                , n = He(this.vmId) ? function () {
                    return !0
                }
                    : function (e) {
                        return e.vmId === t.vmId
                    }
                ;
            He(e) && (e = null);
            for (var r = 0; r < this.items.length; ++r)
                n(t.items[r]) && t.items[r].scope === e && (t.items.splice(r, 1),
                    --r)
        }
        ,
        Bt.prototype.collect = function (e, t, n) {
            var r = this;
            void 0 === n && (n = !0);
            var i = !He(e) && !e.includes("*")
                , a = function (e) {
                    var t, a = e.reduce(function (e, t) {
                        return He(r.vmId) || t.vmId === r.vmId ? (e[t.field] || (e[t.field] = []),
                            e[t.field].push(n ? t.msg : t),
                            e) : e
                    }, {});
                    return i ? (t = a,
                        Qe(Object.values) ? Object.values(t) : t[Object.keys(t)[0]])[0] || [] : a
                };
            if (He(e))
                return a(this.items);
            var o = He(t) ? String(e) : t + "." + e
                , s = this._makeCandidateFilters(o)
                , u = s.isPrimary
                , l = s.isAlt
                , c = this.items.reduce(function (e, t) {
                    return u(t) && e.primary.push(t),
                        l(t) && e.alt.push(t),
                        e
                }, {
                    primary: [],
                    alt: []
                });
            return a(c = c.primary.length ? c.primary : c.alt)
        }
        ,
        Bt.prototype.count = function () {
            var e = this;
            return this.vmId ? this.items.filter(function (t) {
                return t.vmId === e.vmId
            }).length : this.items.length
        }
        ,
        Bt.prototype.firstById = function (e) {
            var t = at(this.items, function (t) {
                return t.id === e
            });
            return t ? t.msg : void 0
        }
        ,
        Bt.prototype.first = function (e, t) {
            void 0 === t && (t = null);
            var n = He(t) ? e : t + "." + e
                , r = this._match(n);
            return r && r.msg
        }
        ,
        Bt.prototype.firstRule = function (e, t) {
            var n = this.collect(e, t, !1);
            return n.length && n[0].rule || void 0
        }
        ,
        Bt.prototype.has = function (e, t) {
            return void 0 === t && (t = null),
                !!this.first(e, t)
        }
        ,
        Bt.prototype.firstByRule = function (e, t, n) {
            void 0 === n && (n = null);
            var r = this.collect(e, n, !1).filter(function (e) {
                return e.rule === t
            })[0];
            return r && r.msg || void 0
        }
        ,
        Bt.prototype.firstNot = function (e, t, n) {
            void 0 === t && (t = "required"),
                void 0 === n && (n = null);
            var r = this.collect(e, n, !1).filter(function (e) {
                return e.rule !== t
            })[0];
            return r && r.msg || void 0
        }
        ,
        Bt.prototype.removeById = function (e) {
            var t = function (t) {
                return t.id === e
            };
            Array.isArray(e) && (t = function (t) {
                return -1 !== e.indexOf(t.id)
            }
            );
            for (var n = 0; n < this.items.length; ++n)
                t(this.items[n]) && (this.items.splice(n, 1),
                    --n)
        }
        ,
        Bt.prototype.remove = function (e, t, n) {
            if (!He(e))
                for (var r, i = He(t) ? String(e) : t + "." + e, a = this._makeCandidateFilters(i).isPrimary, o = 0; o < this.items.length; ++o)
                    r = this.items[o],
                        (He(n) ? a(r) : a(r) && r.vmId === n) && (this.items.splice(o, 1),
                            --o)
        }
        ,
        Bt.prototype._makeCandidateFilters = function (e) {
            var t = this
                , n = function () {
                    return !0
                }
                , r = function () {
                    return !0
                }
                , i = function () {
                    return !0
                }
                , a = function () {
                    return !0
                }
                , o = function (e) {
                    var t = null;
                    if (ct(e, ":") && (t = e.split(":").pop(),
                        e = e.replace(":" + t, "")),
                        "#" === e[0])
                        return {
                            id: e.slice(1),
                            rule: t,
                            name: null,
                            scope: null
                        };
                    var n = null
                        , r = e;
                    if (ct(e, ".")) {
                        var i = e.split(".");
                        n = i[0],
                            r = i.slice(1).join(".")
                    }
                    return {
                        id: null,
                        scope: n,
                        name: r,
                        rule: t
                    }
                }(e)
                , s = o.id
                , u = o.rule
                , l = o.scope
                , c = o.name;
            if (u && (n = function (e) {
                return e.rule === u
            }
            ),
                s)
                return {
                    isPrimary: function (e) {
                        return n(e) && function (e) {
                            return s === e.id
                        }
                    },
                    isAlt: function () {
                        return !1
                    }
                };
            r = He(l) ? function (e) {
                return He(e.scope)
            }
                : function (e) {
                    return e.scope === l
                }
                ,
                He(c) || "*" === c || (i = function (e) {
                    return e.field === c
                }
                ),
                He(this.vmId) || (a = function (e) {
                    return e.vmId === t.vmId
                }
                );
            return {
                isPrimary: function (e) {
                    return a(e) && i(e) && n(e) && r(e)
                },
                isAlt: function (e) {
                    return a(e) && n(e) && e.field === l + "." + c
                }
            }
        }
        ,
        Bt.prototype._match = function (e) {
            if (!He(e)) {
                var t = this._makeCandidateFilters(e)
                    , n = t.isPrimary
                    , r = t.isAlt;
                return this.items.reduce(function (e, t, i, a) {
                    var o = i === a.length - 1;
                    return e.primary ? o ? e.primary : e : (n(t) && (e.primary = t),
                        r(t) && (e.alt = t),
                        o ? e.primary || e.alt : e)
                }, {})
            }
        }
        ;
    var Xt = function () { };
    Xt.generate = function (e, t, n) {
        var r = Xt.resolveModel(t, n)
            , i = Rt.resolve(n.context);
        return {
            name: Xt.resolveName(e, n),
            el: e,
            listen: !t.modifiers.disable,
            bails: !!t.modifiers.bails || !0 !== t.modifiers.continues && void 0,
            scope: Xt.resolveScope(e, t, n),
            vm: Xt.makeVM(n.context),
            expression: t.value,
            component: n.componentInstance,
            classes: i.classes,
            classNames: i.classNames,
            getter: Xt.resolveGetter(e, n, r),
            events: Xt.resolveEvents(e, n) || i.events,
            model: r,
            delay: Xt.resolveDelay(e, n, i),
            rules: Xt.resolveRules(e, t, n),
            immediate: !!t.modifiers.initial || !!t.modifiers.immediate,
            validity: i.validity,
            aria: i.aria,
            initialValue: Xt.resolveInitialValue(n)
        }
    }
        ,
        Xt.getCtorConfig = function (e) {
            return e.componentInstance ? We("componentInstance.$options.$_veeValidate", e) : null
        }
        ,
        Xt.resolveRules = function (e, t, n) {
            var r = "";
            return t.value || t && t.expression || (r = ze(e, "rules")),
                t.value && ct(["string", "object"], typeof t.value.rules) ? r = t.value.rules : t.value && (r = t.value),
                n.componentInstance ? r : lt(e, r)
        }
        ,
        Xt.resolveInitialValue = function (e) {
            var t = e.data.model || at(e.data.directives, function (e) {
                return "model" === e.name
            });
            return t && t.value
        }
        ,
        Xt.makeVM = function (e) {
            return {
                get $el() {
                    return e.$el
                },
                get $refs() {
                    return e.$refs
                },
                $watch: e.$watch ? e.$watch.bind(e) : function () { }
                ,
                $validator: e.$validator ? {
                    errors: e.$validator.errors,
                    validate: e.$validator.validate.bind(e.$validator),
                    update: e.$validator.update.bind(e.$validator)
                } : null
            }
        }
        ,
        Xt.resolveDelay = function (e, t, n) {
            var r = ze(e, "delay")
                , i = n && "delay" in n ? n.delay : 0;
            return !r && t.componentInstance && t.componentInstance.$attrs && (r = t.componentInstance.$attrs["data-vv-delay"]),
                Ke(i) ? (He(r) || (i.input = r),
                    st(i)) : st(r || i)
        }
        ,
        Xt.resolveEvents = function (e, t) {
            var n = ze(e, "validate-on");
            if (!n && t.componentInstance && t.componentInstance.$attrs && (n = t.componentInstance.$attrs["data-vv-validate-on"]),
                !n && t.componentInstance) {
                var r = Xt.getCtorConfig(t);
                n = r && r.events
            }
            if (!n && Rt.current.events && (n = Rt.current.events),
                n && t.componentInstance && ct(n, "input")) {
                var i = (t.componentInstance.$options.model || {
                    event: "input"
                }).event;
                if (!i)
                    return n;
                n = n.replace("input", i)
            }
            return n
        }
        ,
        Xt.resolveScope = function (e, t, n) {
            void 0 === n && (n = {});
            var r = null;
            return n.componentInstance && He(r) && (r = n.componentInstance.$attrs && n.componentInstance.$attrs["data-vv-scope"]),
                He(r) ? function (e) {
                    var t = ze(e, "scope");
                    if (He(t)) {
                        var n = Ve(e);
                        n && (t = ze(n, "scope"))
                    }
                    return He(t) ? null : t
                }(e) : r
        }
        ,
        Xt.resolveModel = function (e, t) {
            if (e.arg)
                return {
                    expression: e.arg
                };
            var n = t.data.model || at(t.data.directives, function (e) {
                return "model" === e.name
            });
            if (!n)
                return null;
            var r, i, a, o = !/[^\w.$]/.test(n.expression) && (r = n.expression,
                i = t.context,
                a = i,
                r.split(".").every(function (e) {
                    return e in a && (a = a[e],
                        !0)
                })), s = !(!n.modifiers || !n.modifiers.lazy);
            return o ? {
                expression: n.expression,
                lazy: s
            } : {
                expression: null,
                lazy: s
            }
        }
        ,
        Xt.resolveName = function (e, t) {
            var n = ze(e, "name");
            if (!n && !t.componentInstance)
                return e.name;
            if (!n && t.componentInstance && t.componentInstance.$attrs && (n = t.componentInstance.$attrs["data-vv-name"] || t.componentInstance.$attrs.name),
                !n && t.componentInstance) {
                var r = Xt.getCtorConfig(t);
                return r && Qe(r.name) ? r.name.bind(t.componentInstance)() : t.componentInstance.name
            }
            return n
        }
        ,
        Xt.resolveGetter = function (e, t, n) {
            if (n && n.expression)
                return function () {
                    return We(n.expression, t.context)
                }
                    ;
            if (t.componentInstance) {
                var r = ze(e, "value-path") || t.componentInstance.$attrs && t.componentInstance.$attrs["data-vv-value-path"];
                if (r)
                    return function () {
                        return We(r, t.componentInstance)
                    }
                        ;
                var i = Xt.getCtorConfig(t);
                if (i && Qe(i.value)) {
                    var a = i.value.bind(t.componentInstance);
                    return function () {
                        return a()
                    }
                }
                var o = (t.componentInstance.$options.model || {
                    prop: "value"
                }).prop;
                return function () {
                    return t.componentInstance[o]
                }
            }
            switch (e.type) {
                case "checkbox":
                    return function () {
                        var t = document.querySelectorAll('input[name="' + e.name + '"]');
                        if ((t = tt(t).filter(function (e) {
                            return e.checked
                        })).length)
                            return t.map(function (e) {
                                return e.value
                            })
                    }
                        ;
                case "radio":
                    return function () {
                        var t = document.querySelectorAll('input[name="' + e.name + '"]')
                            , n = at(t, function (e) {
                                return e.checked
                            });
                        return n && n.value
                    }
                        ;
                case "file":
                    return function (t) {
                        return tt(e.files)
                    }
                        ;
                case "select-multiple":
                    return function () {
                        return tt(e.options).filter(function (e) {
                            return e.selected
                        }).map(function (e) {
                            return e.value
                        })
                    }
                        ;
                default:
                    return function () {
                        return e && e.value
                    }
            }
        }
        ;
    var Kt = {}
        , Qt = !0
        , Jt = function (e, t) {
            void 0 === t && (t = {
                fastExit: !0
            }),
                this.strict = Qt,
                this.errors = new Bt,
                this.fields = new on,
                this._createFields(e),
                this.paused = !1,
                this.fastExit = !!He(t && t.fastExit) || t.fastExit
        }
        , en = {
            rules: {
                configurable: !0
            },
            flags: {
                configurable: !0
            },
            dictionary: {
                configurable: !0
            },
            _vm: {
                configurable: !0
            },
            locale: {
                configurable: !0
            }
        }
        , tn = {
            rules: {
                configurable: !0
            },
            dictionary: {
                configurable: !0
            },
            locale: {
                configurable: !0
            }
        };
    tn.rules.get = function () {
        return Kt
    }
        ,
        en.rules.get = function () {
            return Kt
        }
        ,
        en.flags.get = function () {
            return this.fields.items.reduce(function (e, t) {
                var n;
                return t.scope ? (e["$" + t.scope] = ((n = {})[t.name] = t.flags,
                    n),
                    e) : (e[t.name] = t.flags,
                        e)
            }, {})
        }
        ,
        en.dictionary.get = function () {
            return Rt.dependency("dictionary")
        }
        ,
        tn.dictionary.get = function () {
            return Rt.dependency("dictionary")
        }
        ,
        en._vm.get = function () {
            return Rt.dependency("vm")
        }
        ,
        en.locale.get = function () {
            return Jt.locale
        }
        ,
        en.locale.set = function (e) {
            Jt.locale = e
        }
        ,
        tn.locale.get = function () {
            return this.dictionary.locale
        }
        ,
        tn.locale.set = function (e) {
            var t = e !== Jt.dictionary.locale;
            Jt.dictionary.locale = e,
                t && Rt.dependency("vm") && Rt.dependency("vm").$emit("localeChanged")
        }
        ,
        Jt.create = function (e, t) {
            return new Jt(e, t)
        }
        ,
        Jt.extend = function (e, t, n) {
            void 0 === n && (n = {}),
                Jt._guardExtend(e, t),
                Jt._merge(e, {
                    validator: t,
                    paramNames: n && n.paramNames,
                    options: nt({}, {
                        hasTarget: !1,
                        immediate: !0
                    }, n || {})
                })
        }
        ,
        Jt.remove = function (e) {
            delete Kt[e]
        }
        ,
        Jt.isTargetRule = function (e) {
            return !!Kt[e] && Kt[e].options.hasTarget
        }
        ,
        Jt.setStrictMode = function (e) {
            void 0 === e && (e = !0),
                Qt = e
        }
        ,
        Jt.prototype.localize = function (e, t) {
            Jt.localize(e, t)
        }
        ,
        Jt.localize = function (e, t) {
            var n;
            if (Ke(e))
                Jt.dictionary.merge(e);
            else {
                if (t) {
                    var r = e || t.name;
                    t = nt({}, t),
                        Jt.dictionary.merge(((n = {})[r] = t,
                            n))
                }
                e && (Jt.locale = e)
            }
        }
        ,
        Jt.prototype.attach = function (e) {
            var t = e.initialValue
                , n = new rn(e);
            return this.fields.push(n),
                n.immediate ? this.validate("#" + n.id, t || n.value, {
                    vmId: e.vmId
                }) : this._validate(n, t || n.value, {
                    initial: !0
                }).then(function (e) {
                    n.flags.valid = e.valid,
                        n.flags.invalid = !e.valid
                }),
                n
        }
        ,
        Jt.prototype.flag = function (e, t, n) {
            void 0 === n && (n = null);
            var r = this._resolveField(e, void 0, n);
            r && t && r.setFlags(t)
        }
        ,
        Jt.prototype.detach = function (e, t, n) {
            var r = Qe(e.destroy) ? e : this._resolveField(e, t, n);
            r && (r.destroy(),
                this.errors.remove(r.name, r.scope, r.vmId),
                this.fields.remove(r))
        }
        ,
        Jt.prototype.extend = function (e, t, n) {
            void 0 === n && (n = {}),
                Jt.extend(e, t, n)
        }
        ,
        Jt.prototype.reset = function (e) {
            var t = this;
            return this._vm.$nextTick().then(function () {
                return t._vm.$nextTick()
            }).then(function () {
                t.fields.filter(e).forEach(function (e) {
                    e.reset(),
                        t.errors.remove(e.name, e.scope)
                })
            })
        }
        ,
        Jt.prototype.update = function (e, t) {
            var n = t.scope;
            this._resolveField("#" + e) && this.errors.update(e, {
                scope: n
            })
        }
        ,
        Jt.prototype.remove = function (e) {
            Jt.remove(e)
        }
        ,
        Jt.prototype.validate = function (e, t, n) {
            var r = this;
            void 0 === n && (n = {});
            var i = n.silent
                , a = n.vmId;
            if (this.paused)
                return Promise.resolve(!0);
            if (He(e))
                return this.validateScopes({
                    silent: i,
                    vmId: a
                });
            if ("*" === e)
                return this.validateAll(void 0, {
                    silent: i,
                    vmId: a
                });
            if (/^(.+)\.\*$/.test(e)) {
                var o = e.match(/^(.+)\.\*$/)[1];
                return this.validateAll(o)
            }
            var s = this._resolveField(e);
            return s ? (i || (s.flags.pending = !0),
                void 0 === t && (t = s.value),
                this._validate(s, t).then(function (e) {
                    return i || r._handleValidationResults([e], a),
                        e.valid
                })) : this._handleFieldNotFound(name)
        }
        ,
        Jt.prototype.pause = function () {
            return this.paused = !0,
                this
        }
        ,
        Jt.prototype.resume = function () {
            return this.paused = !1,
                this
        }
        ,
        Jt.prototype.validateAll = function (e, t) {
            var n = this;
            void 0 === t && (t = {});
            var r = t.silent
                , i = t.vmId;
            if (this.paused)
                return Promise.resolve(!0);
            var a = null
                , o = !1;
            return "string" == typeof e ? a = {
                scope: e,
                vmId: i
            } : Ke(e) ? (a = Object.keys(e).map(function (e) {
                return {
                    name: e,
                    vmId: i,
                    scope: null
                }
            }),
                o = !0) : a = Array.isArray(e) ? e.map(function (e) {
                    return {
                        name: e,
                        vmId: i
                    }
                }) : {
                    scope: null,
                    vmId: i
                },
                Promise.all(this.fields.filter(a).map(function (t) {
                    return n._validate(t, o ? e[t.name] : t.value)
                })).then(function (e) {
                    return r || n._handleValidationResults(e, i),
                        e.every(function (e) {
                            return e.valid
                        })
                })
        }
        ,
        Jt.prototype.validateScopes = function (e) {
            var t = this;
            void 0 === e && (e = {});
            var n = e.silent
                , r = e.vmId;
            return this.paused ? Promise.resolve(!0) : Promise.all(this.fields.filter({
                vmId: r
            }).map(function (e) {
                return t._validate(e, e.value)
            })).then(function (e) {
                return n || t._handleValidationResults(e, r),
                    e.every(function (e) {
                        return e.valid
                    })
            })
        }
        ,
        Jt.prototype.verify = function (e, t) {
            var n = {
                name: "{field}",
                rules: Ge(t)
            };
            return n.isRequired = n.rules.required,
                this._validate(n, e).then(function (e) {
                    return {
                        valid: e.valid,
                        errors: e.errors.map(function (e) {
                            return e.msg
                        })
                    }
                })
        }
        ,
        Jt.prototype.destroy = function () {
            this._vm.$off("localeChanged")
        }
        ,
        Jt.prototype._createFields = function (e) {
            var t = this;
            e && Object.keys(e).forEach(function (n) {
                var r = nt({}, {
                    name: n,
                    rules: e[n]
                });
                t.attach(r)
            })
        }
        ,
        Jt.prototype._getDateFormat = function (e) {
            var t = null;
            return e.date_format && Array.isArray(e.date_format) && (t = e.date_format[0]),
                t || this.dictionary.getDateFormat(this.locale)
        }
        ,
        Jt.prototype._formatErrorMessage = function (e, t, n, r) {
            void 0 === n && (n = {}),
                void 0 === r && (r = null);
            var i = this._getFieldDisplayName(e)
                , a = this._getLocalizedParams(t, r);
            return this.dictionary.getFieldMessage(this.locale, e.name, t.name, [i, a, n])
        }
        ,
        Jt.prototype._convertParamObjectToArray = function (e, t) {
            if (Array.isArray(e))
                return e;
            var n = Kt[t] && Kt[t].paramNames;
            return n && Ke(e) ? n.reduce(function (t, n) {
                return n in e && t.push(e[n]),
                    t
            }, []) : e
        }
        ,
        Jt.prototype._getLocalizedParams = function (e, t) {
            void 0 === t && (t = null);
            var n = this._convertParamObjectToArray(e.params, e.name);
            return e.options.hasTarget && n && n[0] ? [t || this.dictionary.getAttribute(this.locale, n[0], n[0])].concat(n.slice(1)) : n
        }
        ,
        Jt.prototype._getFieldDisplayName = function (e) {
            return e.alias || this.dictionary.getAttribute(this.locale, e.name, e.name)
        }
        ,
        Jt.prototype._convertParamArrayToObj = function (e, t) {
            var n = Kt[t] && Kt[t].paramNames;
            if (!n)
                return e;
            if (Ke(e)) {
                if (n.some(function (t) {
                    return -1 !== Object.keys(e).indexOf(t)
                }))
                    return e;
                e = [e]
            }
            return e.reduce(function (e, t, r) {
                return e[n[r]] = t,
                    e
            }, {})
        }
        ,
        Jt.prototype._test = function (e, t, n) {
            var r = this
                , i = Kt[n.name] ? Kt[n.name].validate : null
                , a = Array.isArray(n.params) ? tt(n.params) : n.params;
            a || (a = []);
            var o = null;
            if (!i || "function" != typeof i)
                return Promise.reject(Xe("No such validator '" + n.name + "' exists."));
            if (n.options.hasTarget) {
                var s = at(e.dependencies, function (e) {
                    return e.name === n.name
                });
                s && (o = s.field.alias,
                    a = [s.field.value].concat(a.slice(1)))
            } else
                "required" === n.name && e.rejectsFalse && (a = a.length ? a : [!0]);
            if (n.options.isDate) {
                var u = this._getDateFormat(e.rules);
                "date_format" !== n.name && a.push(u)
            }
            var l = i(t, this._convertParamArrayToObj(a, n.name));
            return Qe(l.then) ? l.then(function (t) {
                var i = !0
                    , a = {};
                return Array.isArray(t) ? i = t.every(function (e) {
                    return Ke(e) ? e.valid : e
                }) : (i = Ke(t) ? t.valid : t,
                    a = t.data),
                {
                    valid: i,
                    errors: i ? [] : [r._createFieldError(e, n, a, o)]
                }
            }) : (Ke(l) || (l = {
                valid: l,
                data: {}
            }),
            {
                valid: l.valid,
                errors: l.valid ? [] : [this._createFieldError(e, n, l.data, o)]
            })
        }
        ,
        Jt._merge = function (e, t) {
            var n = t.validator
                , r = t.options
                , i = t.paramNames
                , a = Qe(n) ? n : n.validate;
            n.getMessage && Jt.dictionary.setMessage(Jt.locale, e, n.getMessage),
                Kt[e] = {
                    validate: a,
                    options: r,
                    paramNames: i
                }
        }
        ,
        Jt._guardExtend = function (e, t) {
            if (!Qe(t) && !Qe(t.validate))
                throw Xe("Extension Error: The validator '" + e + "' must be a function or have a 'validate' method.")
        }
        ,
        Jt.prototype._createFieldError = function (e, t, n, r) {
            var i = this;
            return {
                id: e.id,
                vmId: e.vmId,
                field: e.name,
                msg: this._formatErrorMessage(e, t, n, r),
                rule: t.name,
                scope: e.scope,
                regenerate: function () {
                    return i._formatErrorMessage(e, t, n, r)
                }
            }
        }
        ,
        Jt.prototype._resolveField = function (e, t, n) {
            if ("#" === e[0])
                return this.fields.find({
                    id: e.slice(1)
                });
            if (!He(t))
                return this.fields.find({
                    name: e,
                    scope: t,
                    vmId: n
                });
            if (ct(e, ".")) {
                var r = e.split(".")
                    , i = r[0]
                    , a = r.slice(1)
                    , o = this.fields.find({
                        name: a.join("."),
                        scope: i,
                        vmId: n
                    });
                if (o)
                    return o
            }
            return this.fields.find({
                name: e,
                scope: null,
                vmId: n
            })
        }
        ,
        Jt.prototype._handleFieldNotFound = function (e, t) {
            if (!this.strict)
                return Promise.resolve(!0);
            var n = He(t) ? e : (He(t) ? "" : t + ".") + e;
            return Promise.reject(Xe('Validating a non-existent field: "' + n + '". Use "attach()" first.'))
        }
        ,
        Jt.prototype._handleValidationResults = function (e, t) {
            var n = this
                , r = e.map(function (e) {
                    return {
                        id: e.id
                    }
                });
            this.errors.removeById(r.map(function (e) {
                return e.id
            })),
                e.forEach(function (e) {
                    n.errors.remove(e.field, e.scope, t)
                });
            var i = e.reduce(function (e, t) {
                return e.push.apply(e, t.errors),
                    e
            }, []);
            this.errors.add(i),
                this.fields.filter(r).forEach(function (t) {
                    var n = at(e, function (e) {
                        return e.id === t.id
                    });
                    t.setFlags({
                        pending: !1,
                        valid: n.valid,
                        validated: !0
                    })
                })
        }
        ,
        Jt.prototype._shouldSkip = function (e, t) {
            return !1 !== e.bails && (!!e.isDisabled || !e.isRequired && (He(t) || "" === t))
        }
        ,
        Jt.prototype._shouldBail = function (e, t) {
            return void 0 !== e.bails ? e.bails : this.fastExit
        }
        ,
        Jt.prototype._validate = function (e, t, n) {
            var r = this;
            void 0 === n && (n = {});
            var i = n.initial;
            if (this._shouldSkip(e, t))
                return Promise.resolve({
                    valid: !0,
                    id: e.id,
                    field: e.name,
                    scope: e.scope,
                    errors: []
                });
            var a = []
                , o = []
                , s = !1;
            return Object.keys(e.rules).filter(function (e) {
                return !i || !Kt[e] || Kt[e].options.immediate
            }).some(function (n) {
                var i = Kt[n] ? Kt[n].options : {}
                    , u = r._test(e, t, {
                        name: n,
                        params: e.rules[n],
                        options: i
                    });
                return Qe(u.then) ? a.push(u) : !u.valid && r._shouldBail(e, t) ? (o.push.apply(o, u.errors),
                    s = !0) : a.push(new Promise(function (e) {
                        return e(u)
                    }
                    )),
                    s
            }),
                s ? Promise.resolve({
                    valid: !1,
                    errors: o,
                    id: e.id,
                    field: e.name,
                    scope: e.scope
                }) : Promise.all(a).then(function (t) {
                    return t.reduce(function (e, t) {
                        var n;
                        return t.valid || (n = e.errors).push.apply(n, t.errors),
                            e.valid = e.valid && t.valid,
                            e
                    }, {
                        valid: !0,
                        errors: o,
                        id: e.id,
                        field: e.name,
                        scope: e.scope
                    })
                })
        }
        ,
        Object.defineProperties(Jt.prototype, en),
        Object.defineProperties(Jt, tn);
    var nn = {
        targetOf: null,
        immediate: !1,
        scope: null,
        listen: !0,
        name: null,
        rules: {},
        vm: null,
        classes: !1,
        validity: !0,
        aria: !0,
        events: "input|blur",
        delay: 0,
        classNames: {
            touched: "touched",
            untouched: "untouched",
            valid: "valid",
            invalid: "invalid",
            pristine: "pristine",
            dirty: "dirty"
        }
    }
        , rn = function (e) {
            void 0 === e && (e = {}),
                this.id = (rt >= 9999 && (rt = 0,
                    it = it.replace("{id}", "_{id}")),
                    rt++,
                    it.replace("{id}", String(rt))),
                this.el = e.el,
                this.updated = !1,
                this.dependencies = [],
                this.vmId = e.vmId,
                this.watchers = [],
                this.events = [],
                this.delay = 0,
                this.rules = {},
                this._cacheId(e),
                this.classNames = nt({}, nn.classNames),
                e = nt({}, nn, e),
                this._delay = He(e.delay) ? 0 : e.delay,
                this.validity = e.validity,
                this.aria = e.aria,
                this.flags = {
                    untouched: !0,
                    touched: !1,
                    dirty: !1,
                    pristine: !0,
                    valid: null,
                    invalid: null,
                    validated: !1,
                    pending: !1,
                    required: !1,
                    changed: !1
                },
                this.vm = e.vm,
                this.componentInstance = e.component,
                this.ctorConfig = this.componentInstance ? We("$options.$_veeValidate", this.componentInstance) : void 0,
                this.update(e),
                this.initialValue = this.value,
                this.updated = !1
        }
        , an = {
            validator: {
                configurable: !0
            },
            isRequired: {
                configurable: !0
            },
            isDisabled: {
                configurable: !0
            },
            alias: {
                configurable: !0
            },
            value: {
                configurable: !0
            },
            bails: {
                configurable: !0
            },
            rejectsFalse: {
                configurable: !0
            }
        };
    an.validator.get = function () {
        return this.vm && this.vm.$validator ? this.vm.$validator : {
            validate: function () { }
        }
    }
        ,
        an.isRequired.get = function () {
            return !!this.rules.required
        }
        ,
        an.isDisabled.get = function () {
            return !(!this.componentInstance || !this.componentInstance.disabled) || !(!this.el || !this.el.disabled)
        }
        ,
        an.alias.get = function () {
            if (this._alias)
                return this._alias;
            var e = null;
            return this.el && (e = ze(this.el, "as")),
                !e && this.componentInstance ? this.componentInstance.$attrs && this.componentInstance.$attrs["data-vv-as"] : e
        }
        ,
        an.value.get = function () {
            if (Qe(this.getter))
                return this.getter()
        }
        ,
        an.bails.get = function () {
            return this._bails
        }
        ,
        an.rejectsFalse.get = function () {
            return this.componentInstance && this.ctorConfig ? !!this.ctorConfig.rejectsFalse : !!this.el && "checkbox" === this.el.type
        }
        ,
        rn.prototype.matches = function (e) {
            var t = this;
            return !e || (e.id ? this.id === e.id : !!(He(e.vmId) ? function () {
                return !0
            }
                : function (e) {
                    return e === t.vmId
                }
            )(e.vmId) && (void 0 === e.name && void 0 === e.scope || (void 0 === e.scope ? this.name === e.name : void 0 === e.name ? this.scope === e.scope : e.name === this.name && e.scope === this.scope)))
        }
        ,
        rn.prototype._cacheId = function (e) {
            this.el && !e.targetOf && (this.el._veeValidateId = this.id)
        }
        ,
        rn.prototype.update = function (e) {
            var t;
            this.targetOf = e.targetOf || null,
                this.immediate = e.immediate || this.immediate || !1,
                !He(e.scope) && e.scope !== this.scope && Qe(this.validator.update) && this.validator.update(this.id, {
                    scope: e.scope
                }),
                this.scope = He(e.scope) ? He(this.scope) ? null : this.scope : e.scope,
                this.name = (He(e.name) ? e.name : String(e.name)) || this.name || null,
                this.rules = void 0 !== e.rules ? Ge(e.rules) : this.rules,
                this._bails = void 0 !== e.bails ? e.bails : this._bails,
                this.model = e.model || this.model,
                this.listen = void 0 !== e.listen ? e.listen : this.listen,
                this.classes = !(!e.classes && !this.classes) && !this.componentInstance,
                this.classNames = Ke(e.classNames) ? ut(this.classNames, e.classNames) : this.classNames,
                this.getter = Qe(e.getter) ? e.getter : this.getter,
                this._alias = e.alias || this._alias,
                this.events = e.events ? "string" == typeof (t = e.events) && t.length ? t.split("|") : [] : this.events,
                this.delay = function (e, t, n) {
                    return "number" == typeof t ? e.reduce(function (e, n) {
                        return e[n] = t,
                            e
                    }, {}) : e.reduce(function (e, r) {
                        return "object" == typeof t && r in t ? (e[r] = t[r],
                            e) : "number" == typeof n ? (e[r] = n,
                                e) : (e[r] = n && n[r] || 0,
                                    e)
                    }, {})
                }(this.events, e.delay || this.delay, this._delay),
                this.updateDependencies(),
                this.addActionListeners(),
                this.name || this.targetOf || Be('A field is missing a "name" or "data-vv-name" attribute'),
                void 0 !== e.rules && (this.flags.required = this.isRequired),
                this.flags.validated && void 0 !== e.rules && this.updated && this.validator.validate("#" + this.id),
                this.updated = !0,
                this.addValueListeners(),
                this.el && (this.updateClasses(),
                    this.updateAriaAttrs())
        }
        ,
        rn.prototype.reset = function () {
            var e = this;
            this._cancellationToken && (this._cancellationToken.cancelled = !0,
                delete this._cancellationToken);
            var t = {
                untouched: !0,
                touched: !1,
                dirty: !1,
                pristine: !0,
                valid: null,
                invalid: null,
                validated: !1,
                pending: !1,
                required: !1,
                changed: !1
            };
            Object.keys(this.flags).filter(function (e) {
                return "required" !== e
            }).forEach(function (n) {
                e.flags[n] = t[n]
            }),
                this.addActionListeners(),
                this.updateClasses(),
                this.updateAriaAttrs(),
                this.updateCustomValidity()
        }
        ,
        rn.prototype.setFlags = function (e) {
            var t = this
                , n = {
                    pristine: "dirty",
                    dirty: "pristine",
                    valid: "invalid",
                    invalid: "valid",
                    touched: "untouched",
                    untouched: "touched"
                };
            Object.keys(e).forEach(function (r) {
                t.flags[r] = e[r],
                    n[r] && void 0 === e[n[r]] && (t.flags[n[r]] = !e[r])
            }),
                void 0 === e.untouched && void 0 === e.touched && void 0 === e.dirty && void 0 === e.pristine || this.addActionListeners(),
                this.updateClasses(),
                this.updateAriaAttrs(),
                this.updateCustomValidity()
        }
        ,
        rn.prototype.updateDependencies = function () {
            var e = this;
            this.dependencies.forEach(function (e) {
                return e.field.destroy()
            }),
                this.dependencies = [];
            var t = Object.keys(this.rules).reduce(function (t, n) {
                return Jt.isTargetRule(n) && t.push({
                    selector: e.rules[n][0],
                    name: n
                }),
                    t
            }, []);
            t.length && this.vm && this.vm.$el && t.forEach(function (t) {
                var n = t.selector
                    , r = t.name
                    , i = e.vm.$refs[n]
                    , a = Array.isArray(i) ? i[0] : i;
                if (a) {
                    var o = {
                        vm: e.vm,
                        classes: e.classes,
                        classNames: e.classNames,
                        delay: e.delay,
                        scope: e.scope,
                        events: e.events.join("|"),
                        immediate: e.immediate,
                        targetOf: e.id
                    };
                    Qe(a.$watch) ? (o.component = a,
                        o.el = a.$el,
                        o.getter = Xt.resolveGetter(a.$el, a.$vnode)) : (o.el = a,
                            o.getter = Xt.resolveGetter(a, {})),
                        e.dependencies.push({
                            name: r,
                            field: new rn(o)
                        })
                }
            })
        }
        ,
        rn.prototype.unwatch = function (e) {
            if (void 0 === e && (e = null),
                !e)
                return this.watchers.forEach(function (e) {
                    return e.unwatch()
                }),
                    void (this.watchers = []);
            this.watchers.filter(function (t) {
                return e.test(t.tag)
            }).forEach(function (e) {
                return e.unwatch()
            }),
                this.watchers = this.watchers.filter(function (t) {
                    return !e.test(t.tag)
                })
        }
        ,
        rn.prototype.updateClasses = function () {
            var e = this;
            if (this.classes && !this.isDisabled) {
                var t = function (t) {
                    et(t, e.classNames.dirty, e.flags.dirty),
                        et(t, e.classNames.pristine, e.flags.pristine),
                        et(t, e.classNames.touched, e.flags.touched),
                        et(t, e.classNames.untouched, e.flags.untouched),
                        !He(e.flags.valid) && e.flags.validated && et(t, e.classNames.valid, e.flags.valid),
                        !He(e.flags.invalid) && e.flags.validated && et(t, e.classNames.invalid, e.flags.invalid)
                };
                if (Ze(this.el)) {
                    var n = document.querySelectorAll('input[name="' + this.el.name + '"]');
                    tt(n).forEach(t)
                } else
                    t(this.el)
            }
        }
        ,
        rn.prototype.addActionListeners = function () {
            var e = this;
            if (this.unwatch(/class/),
                this.el) {
                var t = function () {
                    e.flags.touched = !0,
                        e.flags.untouched = !1,
                        e.classes && (et(e.el, e.classNames.touched, !0),
                            et(e.el, e.classNames.untouched, !1)),
                        e.unwatch(/^class_blur$/)
                }
                    , n = Ue(this.el) ? "input" : "change"
                    , r = function () {
                        e.flags.dirty = !0,
                            e.flags.pristine = !1,
                            e.classes && (et(e.el, e.classNames.pristine, !1),
                                et(e.el, e.classNames.dirty, !0)),
                            e.unwatch(/^class_input$/)
                    };
                if (this.componentInstance && Qe(this.componentInstance.$once))
                    return this.componentInstance.$once("input", r),
                        this.componentInstance.$once("blur", t),
                        this.watchers.push({
                            tag: "class_input",
                            unwatch: function () {
                                e.componentInstance.$off("input", r)
                            }
                        }),
                        void this.watchers.push({
                            tag: "class_blur",
                            unwatch: function () {
                                e.componentInstance.$off("blur", t)
                            }
                        });
                if (this.el) {
                    je(this.el, n, r);
                    var i = Ze(this.el) ? "change" : "blur";
                    je(this.el, i, t),
                        this.watchers.push({
                            tag: "class_input",
                            unwatch: function () {
                                e.el.removeEventListener(n, r)
                            }
                        }),
                        this.watchers.push({
                            tag: "class_blur",
                            unwatch: function () {
                                e.el.removeEventListener(i, t)
                            }
                        })
                }
            }
        }
        ,
        rn.prototype.checkValueChanged = function () {
            return (null !== this.initialValue || "" !== this.value || !Ue(this.el)) && this.value !== this.initialValue
        }
        ,
        rn.prototype._determineInputEvent = function () {
            return this.componentInstance ? this.componentInstance.$options.model && this.componentInstance.$options.model.event || "input" : this.model && this.model.lazy ? "change" : Ue(this.el) ? "input" : "change"
        }
        ,
        rn.prototype._determineEventList = function (e) {
            return !this.events.length || this.componentInstance || Ue(this.el) ? [].concat(this.events) : this.events.map(function (t) {
                return "input" === t ? e : t
            })
        }
        ,
        rn.prototype.addValueListeners = function () {
            var e = this;
            if (this.unwatch(/^input_.+/),
                this.listen && this.el) {
                var t = {
                    cancelled: !1
                }
                    , n = this.targetOf ? function () {
                        e.flags.changed = e.checkValueChanged(),
                            e.validator.validate("#" + e.targetOf)
                    }
                        : function () {
                            for (var t = [], n = arguments.length; n--;)
                                t[n] = arguments[n];
                            (0 === t.length || Qe(Event) && t[0] instanceof Event || t[0] && t[0].srcElement) && (t[0] = e.value),
                                e.flags.changed = e.checkValueChanged(),
                                e.validator.validate("#" + e.id, t[0])
                        }
                    , r = this._determineInputEvent()
                    , i = this._determineEventList(r);
                if (this.model && ct(i, r)) {
                    var a = null
                        , o = this.model.expression;
                    if (this.model.expression && (a = this.vm,
                        o = this.model.expression),
                        !o && this.componentInstance && this.componentInstance.$options.model && (a = this.componentInstance,
                            o = this.componentInstance.$options.model.prop || "value"),
                        a && o) {
                        var s = Pe(n, this.delay[r], !1, t)
                            , u = a.$watch(o, function () {
                                for (var n = [], r = arguments.length; r--;)
                                    n[r] = arguments[r];
                                e.flags.pending = !0,
                                    e._cancellationToken = t,
                                    s.apply(void 0, n)
                            });
                        this.watchers.push({
                            tag: "input_model",
                            unwatch: u
                        }),
                            i = i.filter(function (e) {
                                return e !== r
                            })
                    }
                }
                i.forEach(function (r) {
                    var i = Pe(n, e.delay[r], !1, t)
                        , a = function () {
                            for (var n = [], r = arguments.length; r--;)
                                n[r] = arguments[r];
                            e.flags.pending = !0,
                                e._cancellationToken = t,
                                i.apply(void 0, n)
                        };
                    e._addComponentEventListener(r, a),
                        e._addHTMLEventListener(r, a)
                })
            }
        }
        ,
        rn.prototype._addComponentEventListener = function (e, t) {
            var n = this;
            this.componentInstance && (this.componentInstance.$on(e, t),
                this.watchers.push({
                    tag: "input_vue",
                    unwatch: function () {
                        n.componentInstance.$off(e, t)
                    }
                }))
        }
        ,
        rn.prototype._addHTMLEventListener = function (e, t) {
            var n = this;
            if (this.el && !this.componentInstance) {
                var r = function (r) {
                    je(r, e, t),
                        n.watchers.push({
                            tag: "input_native",
                            unwatch: function () {
                                r.removeEventListener(e, t)
                            }
                        })
                };
                if (r(this.el),
                    Ze(this.el)) {
                    var i = document.querySelectorAll('input[name="' + this.el.name + '"]');
                    tt(i).forEach(function (e) {
                        e._veeValidateId && e !== n.el || r(e)
                    })
                }
            }
        }
        ,
        rn.prototype.updateAriaAttrs = function () {
            var e = this;
            if (this.aria && this.el && Qe(this.el.setAttribute)) {
                var t = function (t) {
                    t.setAttribute("aria-required", e.isRequired ? "true" : "false"),
                        t.setAttribute("aria-invalid", e.flags.invalid ? "true" : "false")
                };
                if (Ze(this.el)) {
                    var n = document.querySelectorAll('input[name="' + this.el.name + '"]');
                    tt(n).forEach(t)
                } else
                    t(this.el)
            }
        }
        ,
        rn.prototype.updateCustomValidity = function () {
            this.validity && this.el && Qe(this.el.setCustomValidity) && this.validator.errors && this.el.setCustomValidity(this.flags.valid ? "" : this.validator.errors.firstById(this.id) || "")
        }
        ,
        rn.prototype.destroy = function () {
            this._cancellationToken && (this._cancellationToken.cancelled = !0),
                this.unwatch(),
                this.dependencies.forEach(function (e) {
                    return e.field.destroy()
                }),
                this.dependencies = []
        }
        ,
        Object.defineProperties(rn.prototype, an);
    var on = function (e) {
        void 0 === e && (e = []),
            this.items = e || []
    }
        , sn = {
            length: {
                configurable: !0
            }
        };
    on.prototype["function" == typeof Symbol ? Symbol.iterator : "@@iterator"] = function () {
        var e = this
            , t = 0;
        return {
            next: function () {
                return {
                    value: e.items[t++],
                    done: t > e.items.length
                }
            }
        }
    }
        ,
        sn.length.get = function () {
            return this.items.length
        }
        ,
        on.prototype.find = function (e) {
            return at(this.items, function (t) {
                return t.matches(e)
            })
        }
        ,
        on.prototype.filter = function (e) {
            return Array.isArray(e) ? this.items.filter(function (t) {
                return e.some(function (e) {
                    return t.matches(e)
                })
            }) : this.items.filter(function (t) {
                return t.matches(e)
            })
        }
        ,
        on.prototype.map = function (e) {
            return this.items.map(e)
        }
        ,
        on.prototype.remove = function (e) {
            var t = null;
            if (!(t = e instanceof rn ? e : this.find(e)))
                return null;
            var n = this.items.indexOf(t);
            return this.items.splice(n, 1),
                t
        }
        ,
        on.prototype.push = function (e) {
            if (!(e instanceof rn))
                throw Xe("FieldBag only accepts instances of Field that has an id defined.");
            if (!e.id)
                throw Xe("Field id must be defined.");
            if (this.find({
                id: e.id
            }))
                throw Xe("Field with id " + e.id + " is already added.");
            this.items.push(e)
        }
        ,
        Object.defineProperties(on.prototype, sn);
    var un = function (e, t) {
        this.id = t._uid,
            this._base = e,
            this._paused = !1,
            this.errors = new Bt(e.errors, this.id)
    }
        , ln = {
            flags: {
                configurable: !0
            },
            rules: {
                configurable: !0
            },
            fields: {
                configurable: !0
            },
            dictionary: {
                configurable: !0
            },
            locale: {
                configurable: !0
            }
        };
    ln.flags.get = function () {
        var e = this;
        return this._base.fields.items.filter(function (t) {
            return t.vmId === e.id
        }).reduce(function (e, t) {
            return t.scope && (e["$" + t.scope] || (e["$" + t.scope] = {}),
                e["$" + t.scope][t.name] = t.flags),
                e[t.name] = t.flags,
                e
        }, {})
    }
        ,
        ln.rules.get = function () {
            return this._base.rules
        }
        ,
        ln.fields.get = function () {
            return new on(this._base.fields.filter({
                vmId: this.id
            }))
        }
        ,
        ln.dictionary.get = function () {
            return this._base.dictionary
        }
        ,
        ln.locale.get = function () {
            return this._base.locale
        }
        ,
        ln.locale.set = function (e) {
            this._base.locale = e
        }
        ,
        un.prototype.localize = function () {
            for (var e, t = [], n = arguments.length; n--;)
                t[n] = arguments[n];
            return (e = this._base).localize.apply(e, t)
        }
        ,
        un.prototype.update = function () {
            for (var e, t = [], n = arguments.length; n--;)
                t[n] = arguments[n];
            return (e = this._base).update.apply(e, t)
        }
        ,
        un.prototype.attach = function (e) {
            var t = nt({}, e, {
                vmId: this.id
            });
            return this._base.attach(t)
        }
        ,
        un.prototype.pause = function () {
            this._paused = !0
        }
        ,
        un.prototype.resume = function () {
            this._paused = !1
        }
        ,
        un.prototype.remove = function (e) {
            return this._base.remove(e)
        }
        ,
        un.prototype.detach = function () {
            for (var e, t = [], n = arguments.length; n--;)
                t[n] = arguments[n];
            return (e = this._base).detach.apply(e, t.concat([this.id]))
        }
        ,
        un.prototype.extend = function () {
            for (var e, t = [], n = arguments.length; n--;)
                t[n] = arguments[n];
            return (e = this._base).extend.apply(e, t)
        }
        ,
        un.prototype.validate = function (e, t, n) {
            return void 0 === n && (n = {}),
                this._paused ? Promise.resolve(!0) : this._base.validate(e, t, nt({}, {
                    vmId: this.id
                }, n || {}))
        }
        ,
        un.prototype.validateAll = function (e, t) {
            return void 0 === t && (t = {}),
                this._paused ? Promise.resolve(!0) : this._base.validateAll(e, nt({}, {
                    vmId: this.id
                }, t || {}))
        }
        ,
        un.prototype.validateScopes = function (e) {
            return void 0 === e && (e = {}),
                this._paused ? Promise.resolve(!0) : this._base.validateScopes(nt({}, {
                    vmId: this.id
                }, e || {}))
        }
        ,
        un.prototype.destroy = function () {
            delete this.id,
                delete this._base
        }
        ,
        un.prototype.reset = function (e) {
            return this._base.reset(Object.assign({}, e || {}, {
                vmId: this.id
            }))
        }
        ,
        un.prototype.flag = function () {
            for (var e, t = [], n = arguments.length; n--;)
                t[n] = arguments[n];
            return (e = this._base).flag.apply(e, t.concat([this.id]))
        }
        ,
        Object.defineProperties(un.prototype, ln);
    var cn = {
        provide: function () {
            return this.$validator && !ot(this.$vnode) ? {
                $validator: this.$validator
            } : {}
        },
        beforeCreate: function () {
            if (!ot(this.$vnode)) {
                this.$parent || Rt.merge(this.$options.$_veeValidate || {});
                var e = Rt.resolve(this);
                (!this.$parent || this.$options.$_veeValidate && /new/.test(this.$options.$_veeValidate.validator)) && (this.$validator = new un(Rt.dependency("validator"), this));
                var t, n = (t = this.$options.inject,
                    !(!Ke(t) || !t.$validator));
                if (this.$validator || !e.inject || n || (this.$validator = new un(Rt.dependency("validator"), this)),
                    n || this.$validator) {
                    if (!n && this.$validator)
                        this.$options._base.util.defineReactive(this.$validator, "errors", this.$validator.errors);
                    this.$options.computed || (this.$options.computed = {}),
                        this.$options.computed[e.errorBagName || "errors"] = function () {
                            return this.$validator.errors
                        }
                        ,
                        this.$options.computed[e.fieldsBagName || "fields"] = function () {
                            return this.$validator.fields.items.reduce(function (e, t) {
                                return t.scope ? (e["$" + t.scope] || (e["$" + t.scope] = {}),
                                    e["$" + t.scope][t.name] = t.flags,
                                    e) : (e[t.name] = t.flags,
                                        e)
                            }, {})
                        }
                }
            }
        },
        beforeDestroy: function () {
            this.$validator && this._uid === this.$validator.id && this.$validator.errors.clear()
        }
    };
    function dn(e, t) {
        return t && t.$validator ? t.$validator.fields.find({
            id: e._veeValidateId
        }) : null
    }
    var fn, hn = {
        bind: function (e, t, n) {
            var r = n.context.$validator;
            if (r) {
                var i = Xt.generate(e, t, n);
                r.attach(i)
            } else
                Be("No validator instance is present on vm, did you forget to inject '$validator'?")
        },
        inserted: function (e, t, n) {
            var r = dn(e, n.context)
                , i = Xt.resolveScope(e, t, n);
            r && i !== r.scope && (r.update({
                scope: i
            }),
                r.updated = !1)
        },
        update: function (e, t, n) {
            var r = dn(e, n.context);
            if (!(!r || r.updated && qe(t.value, t.oldValue))) {
                var i = Xt.resolveScope(e, t, n)
                    , a = Xt.resolveRules(e, t, n);
                r.update({
                    scope: i,
                    rules: a
                })
            }
        },
        unbind: function (e, t, n) {
            var r = n.context
                , i = dn(e, r);
            i && r.$validator.detach(i)
        }
    };
    var mn = function (e, t) {
        var n = {
            pristine: function (e, t) {
                return e && t
            },
            dirty: function (e, t) {
                return e || t
            },
            touched: function (e, t) {
                return e || t
            },
            untouched: function (e, t) {
                return e && t
            },
            valid: function (e, t) {
                return e && t
            },
            invalid: function (e, t) {
                return e || t
            },
            pending: function (e, t) {
                return e || t
            },
            required: function (e, t) {
                return e || t
            },
            validated: function (e, t) {
                return e && t
            }
        };
        return Object.keys(n).reduce(function (r, i) {
            return r[i] = n[i](e[i], t[i]),
                r
        }, {})
    }
        , pn = function (e, t) {
            return void 0 === t && (t = !0),
                Object.keys(e).reduce(function (n, r) {
                    if (!n)
                        return n = nt({}, e[r]);
                    var i = 0 === r.indexOf("$");
                    return t && i ? mn(pn(e[r]), n) : !t && i ? n : n = mn(n, e[r])
                }, null)
        }
        , vn = {
            name: "vv-error",
            inject: ["$validator"],
            functional: !0,
            props: {
                for: {
                    type: String,
                    required: !0
                },
                tag: {
                    type: String,
                    default: "span"
                }
            },
            render: function (e, t) {
                var n = t.props
                    , r = t.injections;
                return e(n.tag, r.$validator.errors.first(n.for))
            }
        }
        , yn = {
            install: function (e, t) {
                if (void 0 === t && (t = {}),
                    fn && e === fn)
                    Be("already installed, Vue.use(VeeValidate) should only be called once.");
                else {
                    Ee(),
                        fn = e;
                    var n = new Jt(null, t)
                        , r = new fn({
                            data: function () {
                                return {
                                    errors: n.errors,
                                    fields: n.fields
                                }
                            }
                        });
                    Rt.register("vm", r),
                        Rt.register("validator", n),
                        Rt.merge(t);
                    var i = Rt.current
                        , a = i.dictionary
                        , o = i.i18n;
                    a && n.localize(a);
                    var s = function () {
                        n.errors.regenerate()
                    };
                    o ? o._vm.$watch("locale", s) : "undefined" != typeof window && r.$on("localeChanged", s),
                        !o && t.locale && n.localize(t.locale),
                        Jt.setStrictMode(Rt.current.strict),
                        fn.mixin(cn),
                        fn.directive("validate", hn)
                }
            },
            use: function (e, t) {
                if (void 0 === t && (t = {}),
                    !Qe(e))
                    return Be("The plugin must be a callable function");
                e({
                    Validator: Jt,
                    ErrorBag: Bt,
                    Rules: Jt.rules
                }, t)
            },
            directive: hn,
            mixin: cn,
            mapFields: function (e) {
                if (!e)
                    return function () {
                        return pn(this.$validator.flags)
                    }
                        ;
                var t = function (e) {
                    return Array.isArray(e) ? e.reduce(function (e, t) {
                        return ct(t, ".") ? e[t.split(".")[1]] = t : e[t] = t,
                            e
                    }, {}) : e
                }(e);
                return Object.keys(t).reduce(function (e, n) {
                    var r = t[n];
                    return e[n] = function () {
                        if (this.$validator.flags[r])
                            return this.$validator.flags[r];
                        if ("*" === t[n])
                            return pn(this.$validator.flags, !1);
                        if (r.indexOf(".") <= 0)
                            return {};
                        var e = r.split(".")
                            , i = e[0]
                            , a = e.slice(1);
                        return i = this.$validator.flags["$" + i],
                            "*" === (a = a.join(".")) && i ? pn(i) : i && i[a] ? i[a] : {}
                    }
                        ,
                        e
                }, {})
            },
            Validator: Jt,
            ErrorBag: Bt,
            ErrorComponent: vn,
            version: "2.1.0-beta.9"
        };
    return yn.use(function (e) {
        var t = e.Validator;
        Object.keys(kt).forEach(function (e) {
            t.extend(e, kt[e].validate, nt({}, kt[e].options, {
                paramNames: kt[e].paramNames
            }))
        }),
            t.localize("en", Et)
    }),
        yn.Rules = kt,
        yn
});
