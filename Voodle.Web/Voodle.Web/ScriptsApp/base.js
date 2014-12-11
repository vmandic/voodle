// core app object
var _app = _app || {};

/**
 * Toastr options preset
 */
toastr.options = {
    "closeButton": true,
    "debug": false,
    "positionClass": "toast-top-right",
    "onclick": null,
    "showDuration": "800",
    "hideDuration": "200",
    "timeOut": "1500",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

//By default the on before unload the user gets block by a loader for a smoother UX and loading info to the user
//window.onbeforeunload = _app.showLoader;

function getVW() {
    return document.documentElement.clientWidth;
}

function getVH() {
    return document.documentElement.clientHeight;
}

/*
 * Retrieves a value from the current query string for a given string key.
 */
function getQueryStringParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function disableLetters(e) {
    var chrs = [],
        k = e.which;
    for (var i = 48; i < 58; i++) { chrs.push(i); }
    for (var i = 96; i < 107; i++) { chrs.push(i); } //numpad keys
    chrs.push(0, 8, 188, 190, 38, 40, 44, 46); //backspace, delete, comma, period, down and up arrows
    if (($.inArray(k, chrs) >= 0) === false) {
        e.preventDefault();
    }
}

function parseBool(str) {
    if (typeof str === 'string' && str.toLowerCase() == 'true') {
        return true;
    }

    return (parseInt(str) > 0);
}

//string format implementation
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

Array.prototype.move = function (from, to) {
    //moves an element from one index to another in the array
    this.splice(to, 0, this.splice(from, 1)[0]);
};

Array.prototype.first = function () {
    return this[0];
};

Array.prototype.last = function () {
    return this[this.length - 1];
};

// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
    // if the other array is a falsy value, return
    if (!array) {
        return false;
    }

    // compare lengths - can save a lot of time 
    if (this.length != array.length) {
        return false;
    }

    for (var i = 0, l = this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i])) {
                return false;
            }
        } else if (this[i] !== array[i]) {
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;
        }
    }
    return true;
};

function viewIs(viewMatch) {
    // a helper for securing javascript code execution
    return viewId === viewMatch;
}

//flattens a flat array for JSON.stringify(array) to avoid converting circular structures
function flatArrayForJSONStringify(data) {
    var tmp = [];
    $.each(data, function () {
        var obj = {};
        for (var prop in this) {
            obj[prop] = this[prop];
            //we could do recursion here for n-depth object array
        }
        tmp.push(obj);
    });
    return tmp;
}

function cloneObject(obj) {
    return JSON.parse(JSON.stringify(obj));
}

function cloneKnockoutObservableToObject(obsObj, ignores) {
    return JSON.parse(ko.mapping.toJSON(obsObj, ignores));
}

function cloneKnockoutObservable(obsObj) {
    return ko.mapping.fromJS(ko.mapping.toJS(obsObj));
}

if (!String.prototype.startsWith) {
    (function () {
        'use strict'; // needed to support `apply`/`call` with `undefined`/`null`
        var defineProperty = (function () {
            // IE 8 only supports `Object.defineProperty` on DOM elements
            try {
                var object = {};
                var $defineProperty = Object.defineProperty;
                var result = $defineProperty(object, object, object) && $defineProperty;
            } catch (error) { }
            return result;
        }());
        var toString = {}.toString;
        var startsWith = function (search) {
            if (this == null) {
                throw TypeError();
            }
            var string = String(this);
            if (search && toString.call(search) == '[object RegExp]') {
                throw TypeError();
            }
            var stringLength = string.length;
            var searchString = String(search);
            var searchLength = searchString.length;
            var position = arguments.length > 1 ? arguments[1] : undefined;
            // `ToInteger`
            var pos = position ? Number(position) : 0;
            if (pos != pos) { // better `isNaN`
                pos = 0;
            }
            var start = Math.min(Math.max(pos, 0), stringLength);
            // Avoid the `indexOf` call if no match is possible
            if (searchLength + start > stringLength) {
                return false;
            }
            var index = -1;
            while (++index < searchLength) {
                if (string.charCodeAt(start + index) != searchString.charCodeAt(index)) {
                    return false;
                }
            }
            return true;
        };
        if (defineProperty) {
            defineProperty(String.prototype, 'startsWith', {
                'value': startsWith,
                'configurable': true,
                'writable': true
            });
        } else {
            String.prototype.startsWith = startsWith;
        }
    }());
}

if (!String.prototype.endsWith) {
    (function () {
        'use strict'; // needed to support `apply`/`call` with `undefined`/`null`
        var defineProperty = (function () {
            // IE 8 only supports `Object.defineProperty` on DOM elements
            try {
                var object = {};
                var $defineProperty = Object.defineProperty;
                var result = $defineProperty(object, object, object) && $defineProperty;
            } catch (error) { }
            return result;
        }());
        var toString = {}.toString;
        var endsWith = function (search) {
            if (this == null) {
                throw TypeError();
            }
            var string = String(this);
            if (search && toString.call(search) == '[object RegExp]') {
                throw TypeError();
            }
            var stringLength = string.length;
            var searchString = String(search);
            var searchLength = searchString.length;
            var pos = stringLength;
            if (arguments.length > 1) {
                var position = arguments[1];
                if (position !== undefined) {
                    // `ToInteger`
                    pos = position ? Number(position) : 0;
                    if (pos !== pos) { // better `isNaN`
                        pos = 0;
                    }
                }
            }
            var end = Math.min(Math.max(pos, 0), stringLength);
            var start = end - searchLength;
            if (start < 0) {
                return false;
            }
            var index = -1;
            while (++index < searchLength) {
                if (string.charCodeAt(start + index) != searchString.charCodeAt(index)) {
                    return false;
                }
            }
            return true;
        };
        if (defineProperty) {
            defineProperty(String.prototype, 'endsWith', {
                'value': endsWith,
                'configurable': true,
                'writable': true
            });
        } else {
            String.prototype.endsWith = endsWith;
        }
    }());
}

if (!String.prototype.contains) {
    String.prototype.contains = function () {
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}


/*
 * The universal on loaded logic, this will trigger every time on every view which has base.js related to it.
 */
$(function () {
    var msg = getQueryStringParameterByName("msg");

    if (msg) {
        var parts = msg.split("|");
        //a generic message method that can be sent from the server side with TempData["toastr-msg"]
        parts && parts[0] !== "" && parts[1] !== "" && eval("toastr.{0}('{1}');".format(parts[1], parts[0]));
    }
});