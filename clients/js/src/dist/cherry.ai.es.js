var axios$2 = {exports: {}};

var bind$2 = function bind(fn, thisArg) {
  return function wrap() {
    var args = new Array(arguments.length);
    for (var i = 0; i < args.length; i++) {
      args[i] = arguments[i];
    }
    return fn.apply(thisArg, args);
  };
};

var bind$1 = bind$2;

// utils is a library of generic helper functions non-specific to axios

var toString = Object.prototype.toString;

/**
 * Determine if a value is an Array
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an Array, otherwise false
 */
function isArray(val) {
  return Array.isArray(val);
}

/**
 * Determine if a value is undefined
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if the value is undefined, otherwise false
 */
function isUndefined(val) {
  return typeof val === 'undefined';
}

/**
 * Determine if a value is a Buffer
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Buffer, otherwise false
 */
function isBuffer(val) {
  return val !== null && !isUndefined(val) && val.constructor !== null && !isUndefined(val.constructor)
    && typeof val.constructor.isBuffer === 'function' && val.constructor.isBuffer(val);
}

/**
 * Determine if a value is an ArrayBuffer
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an ArrayBuffer, otherwise false
 */
function isArrayBuffer(val) {
  return toString.call(val) === '[object ArrayBuffer]';
}

/**
 * Determine if a value is a FormData
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an FormData, otherwise false
 */
function isFormData(val) {
  return toString.call(val) === '[object FormData]';
}

/**
 * Determine if a value is a view on an ArrayBuffer
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a view on an ArrayBuffer, otherwise false
 */
function isArrayBufferView(val) {
  var result;
  if ((typeof ArrayBuffer !== 'undefined') && (ArrayBuffer.isView)) {
    result = ArrayBuffer.isView(val);
  } else {
    result = (val) && (val.buffer) && (isArrayBuffer(val.buffer));
  }
  return result;
}

/**
 * Determine if a value is a String
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a String, otherwise false
 */
function isString(val) {
  return typeof val === 'string';
}

/**
 * Determine if a value is a Number
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Number, otherwise false
 */
function isNumber(val) {
  return typeof val === 'number';
}

/**
 * Determine if a value is an Object
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is an Object, otherwise false
 */
function isObject(val) {
  return val !== null && typeof val === 'object';
}

/**
 * Determine if a value is a plain Object
 *
 * @param {Object} val The value to test
 * @return {boolean} True if value is a plain Object, otherwise false
 */
function isPlainObject(val) {
  if (toString.call(val) !== '[object Object]') {
    return false;
  }

  var prototype = Object.getPrototypeOf(val);
  return prototype === null || prototype === Object.prototype;
}

/**
 * Determine if a value is a Date
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Date, otherwise false
 */
function isDate(val) {
  return toString.call(val) === '[object Date]';
}

/**
 * Determine if a value is a File
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a File, otherwise false
 */
function isFile(val) {
  return toString.call(val) === '[object File]';
}

/**
 * Determine if a value is a Blob
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Blob, otherwise false
 */
function isBlob(val) {
  return toString.call(val) === '[object Blob]';
}

/**
 * Determine if a value is a Function
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Function, otherwise false
 */
function isFunction(val) {
  return toString.call(val) === '[object Function]';
}

/**
 * Determine if a value is a Stream
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a Stream, otherwise false
 */
function isStream(val) {
  return isObject(val) && isFunction(val.pipe);
}

/**
 * Determine if a value is a URLSearchParams object
 *
 * @param {Object} val The value to test
 * @returns {boolean} True if value is a URLSearchParams object, otherwise false
 */
function isURLSearchParams(val) {
  return toString.call(val) === '[object URLSearchParams]';
}

/**
 * Trim excess whitespace off the beginning and end of a string
 *
 * @param {String} str The String to trim
 * @returns {String} The String freed of excess whitespace
 */
function trim(str) {
  return str.trim ? str.trim() : str.replace(/^\s+|\s+$/g, '');
}

/**
 * Determine if we're running in a standard browser environment
 *
 * This allows axios to run in a web worker, and react-native.
 * Both environments support XMLHttpRequest, but not fully standard globals.
 *
 * web workers:
 *  typeof window -> undefined
 *  typeof document -> undefined
 *
 * react-native:
 *  navigator.product -> 'ReactNative'
 * nativescript
 *  navigator.product -> 'NativeScript' or 'NS'
 */
function isStandardBrowserEnv() {
  if (typeof navigator !== 'undefined' && (navigator.product === 'ReactNative' ||
                                           navigator.product === 'NativeScript' ||
                                           navigator.product === 'NS')) {
    return false;
  }
  return (
    typeof window !== 'undefined' &&
    typeof document !== 'undefined'
  );
}

/**
 * Iterate over an Array or an Object invoking a function for each item.
 *
 * If `obj` is an Array callback will be called passing
 * the value, index, and complete array for each item.
 *
 * If 'obj' is an Object callback will be called passing
 * the value, key, and complete object for each property.
 *
 * @param {Object|Array} obj The object to iterate
 * @param {Function} fn The callback to invoke for each item
 */
function forEach(obj, fn) {
  // Don't bother if no value provided
  if (obj === null || typeof obj === 'undefined') {
    return;
  }

  // Force an array if not already something iterable
  if (typeof obj !== 'object') {
    /*eslint no-param-reassign:0*/
    obj = [obj];
  }

  if (isArray(obj)) {
    // Iterate over array values
    for (var i = 0, l = obj.length; i < l; i++) {
      fn.call(null, obj[i], i, obj);
    }
  } else {
    // Iterate over object keys
    for (var key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        fn.call(null, obj[key], key, obj);
      }
    }
  }
}

/**
 * Accepts varargs expecting each argument to be an object, then
 * immutably merges the properties of each object and returns result.
 *
 * When multiple objects contain the same key the later object in
 * the arguments list will take precedence.
 *
 * Example:
 *
 * ```js
 * var result = merge({foo: 123}, {foo: 456});
 * console.log(result.foo); // outputs 456
 * ```
 *
 * @param {Object} obj1 Object to merge
 * @returns {Object} Result of all merge properties
 */
function merge(/* obj1, obj2, obj3, ... */) {
  var result = {};
  function assignValue(val, key) {
    if (isPlainObject(result[key]) && isPlainObject(val)) {
      result[key] = merge(result[key], val);
    } else if (isPlainObject(val)) {
      result[key] = merge({}, val);
    } else if (isArray(val)) {
      result[key] = val.slice();
    } else {
      result[key] = val;
    }
  }

  for (var i = 0, l = arguments.length; i < l; i++) {
    forEach(arguments[i], assignValue);
  }
  return result;
}

/**
 * Extends object a by mutably adding to it the properties of object b.
 *
 * @param {Object} a The object to be extended
 * @param {Object} b The object to copy properties from
 * @param {Object} thisArg The object to bind function to
 * @return {Object} The resulting value of object a
 */
function extend(a, b, thisArg) {
  forEach(b, function assignValue(val, key) {
    if (thisArg && typeof val === 'function') {
      a[key] = bind$1(val, thisArg);
    } else {
      a[key] = val;
    }
  });
  return a;
}

/**
 * Remove byte order marker. This catches EF BB BF (the UTF-8 BOM)
 *
 * @param {string} content with BOM
 * @return {string} content value without BOM
 */
function stripBOM(content) {
  if (content.charCodeAt(0) === 0xFEFF) {
    content = content.slice(1);
  }
  return content;
}

var utils$e = {
  isArray: isArray,
  isArrayBuffer: isArrayBuffer,
  isBuffer: isBuffer,
  isFormData: isFormData,
  isArrayBufferView: isArrayBufferView,
  isString: isString,
  isNumber: isNumber,
  isObject: isObject,
  isPlainObject: isPlainObject,
  isUndefined: isUndefined,
  isDate: isDate,
  isFile: isFile,
  isBlob: isBlob,
  isFunction: isFunction,
  isStream: isStream,
  isURLSearchParams: isURLSearchParams,
  isStandardBrowserEnv: isStandardBrowserEnv,
  forEach: forEach,
  merge: merge,
  extend: extend,
  trim: trim,
  stripBOM: stripBOM
};

var utils$d = utils$e;

function encode(val) {
  return encodeURIComponent(val).
    replace(/%3A/gi, ':').
    replace(/%24/g, '$').
    replace(/%2C/gi, ',').
    replace(/%20/g, '+').
    replace(/%5B/gi, '[').
    replace(/%5D/gi, ']');
}

/**
 * Build a URL by appending params to the end
 *
 * @param {string} url The base of the url (e.g., http://www.google.com)
 * @param {object} [params] The params to be appended
 * @returns {string} The formatted url
 */
var buildURL$2 = function buildURL(url, params, paramsSerializer) {
  /*eslint no-param-reassign:0*/
  if (!params) {
    return url;
  }

  var serializedParams;
  if (paramsSerializer) {
    serializedParams = paramsSerializer(params);
  } else if (utils$d.isURLSearchParams(params)) {
    serializedParams = params.toString();
  } else {
    var parts = [];

    utils$d.forEach(params, function serialize(val, key) {
      if (val === null || typeof val === 'undefined') {
        return;
      }

      if (utils$d.isArray(val)) {
        key = key + '[]';
      } else {
        val = [val];
      }

      utils$d.forEach(val, function parseValue(v) {
        if (utils$d.isDate(v)) {
          v = v.toISOString();
        } else if (utils$d.isObject(v)) {
          v = JSON.stringify(v);
        }
        parts.push(encode(key) + '=' + encode(v));
      });
    });

    serializedParams = parts.join('&');
  }

  if (serializedParams) {
    var hashmarkIndex = url.indexOf('#');
    if (hashmarkIndex !== -1) {
      url = url.slice(0, hashmarkIndex);
    }

    url += (url.indexOf('?') === -1 ? '?' : '&') + serializedParams;
  }

  return url;
};

var utils$c = utils$e;

function InterceptorManager$1() {
  this.handlers = [];
}

/**
 * Add a new interceptor to the stack
 *
 * @param {Function} fulfilled The function to handle `then` for a `Promise`
 * @param {Function} rejected The function to handle `reject` for a `Promise`
 *
 * @return {Number} An ID used to remove interceptor later
 */
InterceptorManager$1.prototype.use = function use(fulfilled, rejected, options) {
  this.handlers.push({
    fulfilled: fulfilled,
    rejected: rejected,
    synchronous: options ? options.synchronous : false,
    runWhen: options ? options.runWhen : null
  });
  return this.handlers.length - 1;
};

/**
 * Remove an interceptor from the stack
 *
 * @param {Number} id The ID that was returned by `use`
 */
InterceptorManager$1.prototype.eject = function eject(id) {
  if (this.handlers[id]) {
    this.handlers[id] = null;
  }
};

/**
 * Iterate over all the registered interceptors
 *
 * This method is particularly useful for skipping over any
 * interceptors that may have become `null` calling `eject`.
 *
 * @param {Function} fn The function to call for each interceptor
 */
InterceptorManager$1.prototype.forEach = function forEach(fn) {
  utils$c.forEach(this.handlers, function forEachHandler(h) {
    if (h !== null) {
      fn(h);
    }
  });
};

var InterceptorManager_1 = InterceptorManager$1;

var utils$b = utils$e;

var normalizeHeaderName$1 = function normalizeHeaderName(headers, normalizedName) {
  utils$b.forEach(headers, function processHeader(value, name) {
    if (name !== normalizedName && name.toUpperCase() === normalizedName.toUpperCase()) {
      headers[normalizedName] = value;
      delete headers[name];
    }
  });
};

/**
 * Update an Error with the specified config, error code, and response.
 *
 * @param {Error} error The error to update.
 * @param {Object} config The config.
 * @param {string} [code] The error code (for example, 'ECONNABORTED').
 * @param {Object} [request] The request.
 * @param {Object} [response] The response.
 * @returns {Error} The error.
 */
var enhanceError$2 = function enhanceError(error, config, code, request, response) {
  error.config = config;
  if (code) {
    error.code = code;
  }

  error.request = request;
  error.response = response;
  error.isAxiosError = true;

  error.toJSON = function toJSON() {
    return {
      // Standard
      message: this.message,
      name: this.name,
      // Microsoft
      description: this.description,
      number: this.number,
      // Mozilla
      fileName: this.fileName,
      lineNumber: this.lineNumber,
      columnNumber: this.columnNumber,
      stack: this.stack,
      // Axios
      config: this.config,
      code: this.code,
      status: this.response && this.response.status ? this.response.status : null
    };
  };
  return error;
};

var transitional = {
  silentJSONParsing: true,
  forcedJSONParsing: true,
  clarifyTimeoutError: false
};

var enhanceError$1 = enhanceError$2;

/**
 * Create an Error with the specified message, config, error code, request and response.
 *
 * @param {string} message The error message.
 * @param {Object} config The config.
 * @param {string} [code] The error code (for example, 'ECONNABORTED').
 * @param {Object} [request] The request.
 * @param {Object} [response] The response.
 * @returns {Error} The created error.
 */
var createError$2 = function createError(message, config, code, request, response) {
  var error = new Error(message);
  return enhanceError$1(error, config, code, request, response);
};

var createError$1 = createError$2;

/**
 * Resolve or reject a Promise based on response status.
 *
 * @param {Function} resolve A function that resolves the promise.
 * @param {Function} reject A function that rejects the promise.
 * @param {object} response The response.
 */
var settle$1 = function settle(resolve, reject, response) {
  var validateStatus = response.config.validateStatus;
  if (!response.status || !validateStatus || validateStatus(response.status)) {
    resolve(response);
  } else {
    reject(createError$1(
      'Request failed with status code ' + response.status,
      response.config,
      null,
      response.request,
      response
    ));
  }
};

var utils$a = utils$e;

var cookies$1 = (
  utils$a.isStandardBrowserEnv() ?

  // Standard browser envs support document.cookie
    (function standardBrowserEnv() {
      return {
        write: function write(name, value, expires, path, domain, secure) {
          var cookie = [];
          cookie.push(name + '=' + encodeURIComponent(value));

          if (utils$a.isNumber(expires)) {
            cookie.push('expires=' + new Date(expires).toGMTString());
          }

          if (utils$a.isString(path)) {
            cookie.push('path=' + path);
          }

          if (utils$a.isString(domain)) {
            cookie.push('domain=' + domain);
          }

          if (secure === true) {
            cookie.push('secure');
          }

          document.cookie = cookie.join('; ');
        },

        read: function read(name) {
          var match = document.cookie.match(new RegExp('(^|;\\s*)(' + name + ')=([^;]*)'));
          return (match ? decodeURIComponent(match[3]) : null);
        },

        remove: function remove(name) {
          this.write(name, '', Date.now() - 86400000);
        }
      };
    })() :

  // Non standard browser env (web workers, react-native) lack needed support.
    (function nonStandardBrowserEnv() {
      return {
        write: function write() {},
        read: function read() { return null; },
        remove: function remove() {}
      };
    })()
);

/**
 * Determines whether the specified URL is absolute
 *
 * @param {string} url The URL to test
 * @returns {boolean} True if the specified URL is absolute, otherwise false
 */
var isAbsoluteURL$1 = function isAbsoluteURL(url) {
  // A URL is considered absolute if it begins with "<scheme>://" or "//" (protocol-relative URL).
  // RFC 3986 defines scheme name as a sequence of characters beginning with a letter and followed
  // by any combination of letters, digits, plus, period, or hyphen.
  return /^([a-z][a-z\d+\-.]*:)?\/\//i.test(url);
};

/**
 * Creates a new URL by combining the specified URLs
 *
 * @param {string} baseURL The base URL
 * @param {string} relativeURL The relative URL
 * @returns {string} The combined URL
 */
var combineURLs$1 = function combineURLs(baseURL, relativeURL) {
  return relativeURL
    ? baseURL.replace(/\/+$/, '') + '/' + relativeURL.replace(/^\/+/, '')
    : baseURL;
};

var isAbsoluteURL = isAbsoluteURL$1;
var combineURLs = combineURLs$1;

/**
 * Creates a new URL by combining the baseURL with the requestedURL,
 * only when the requestedURL is not already an absolute URL.
 * If the requestURL is absolute, this function returns the requestedURL untouched.
 *
 * @param {string} baseURL The base URL
 * @param {string} requestedURL Absolute or relative URL to combine
 * @returns {string} The combined full path
 */
var buildFullPath$1 = function buildFullPath(baseURL, requestedURL) {
  if (baseURL && !isAbsoluteURL(requestedURL)) {
    return combineURLs(baseURL, requestedURL);
  }
  return requestedURL;
};

var utils$9 = utils$e;

// Headers whose duplicates are ignored by node
// c.f. https://nodejs.org/api/http.html#http_message_headers
var ignoreDuplicateOf = [
  'age', 'authorization', 'content-length', 'content-type', 'etag',
  'expires', 'from', 'host', 'if-modified-since', 'if-unmodified-since',
  'last-modified', 'location', 'max-forwards', 'proxy-authorization',
  'referer', 'retry-after', 'user-agent'
];

/**
 * Parse headers into an object
 *
 * ```
 * Date: Wed, 27 Aug 2014 08:58:49 GMT
 * Content-Type: application/json
 * Connection: keep-alive
 * Transfer-Encoding: chunked
 * ```
 *
 * @param {String} headers Headers needing to be parsed
 * @returns {Object} Headers parsed into an object
 */
var parseHeaders$1 = function parseHeaders(headers) {
  var parsed = {};
  var key;
  var val;
  var i;

  if (!headers) { return parsed; }

  utils$9.forEach(headers.split('\n'), function parser(line) {
    i = line.indexOf(':');
    key = utils$9.trim(line.substr(0, i)).toLowerCase();
    val = utils$9.trim(line.substr(i + 1));

    if (key) {
      if (parsed[key] && ignoreDuplicateOf.indexOf(key) >= 0) {
        return;
      }
      if (key === 'set-cookie') {
        parsed[key] = (parsed[key] ? parsed[key] : []).concat([val]);
      } else {
        parsed[key] = parsed[key] ? parsed[key] + ', ' + val : val;
      }
    }
  });

  return parsed;
};

var utils$8 = utils$e;

var isURLSameOrigin$1 = (
  utils$8.isStandardBrowserEnv() ?

  // Standard browser envs have full support of the APIs needed to test
  // whether the request URL is of the same origin as current location.
    (function standardBrowserEnv() {
      var msie = /(msie|trident)/i.test(navigator.userAgent);
      var urlParsingNode = document.createElement('a');
      var originURL;

      /**
    * Parse a URL to discover it's components
    *
    * @param {String} url The URL to be parsed
    * @returns {Object}
    */
      function resolveURL(url) {
        var href = url;

        if (msie) {
        // IE needs attribute set twice to normalize properties
          urlParsingNode.setAttribute('href', href);
          href = urlParsingNode.href;
        }

        urlParsingNode.setAttribute('href', href);

        // urlParsingNode provides the UrlUtils interface - http://url.spec.whatwg.org/#urlutils
        return {
          href: urlParsingNode.href,
          protocol: urlParsingNode.protocol ? urlParsingNode.protocol.replace(/:$/, '') : '',
          host: urlParsingNode.host,
          search: urlParsingNode.search ? urlParsingNode.search.replace(/^\?/, '') : '',
          hash: urlParsingNode.hash ? urlParsingNode.hash.replace(/^#/, '') : '',
          hostname: urlParsingNode.hostname,
          port: urlParsingNode.port,
          pathname: (urlParsingNode.pathname.charAt(0) === '/') ?
            urlParsingNode.pathname :
            '/' + urlParsingNode.pathname
        };
      }

      originURL = resolveURL(window.location.href);

      /**
    * Determine if a URL shares the same origin as the current location
    *
    * @param {String} requestURL The URL to test
    * @returns {boolean} True if URL shares the same origin, otherwise false
    */
      return function isURLSameOrigin(requestURL) {
        var parsed = (utils$8.isString(requestURL)) ? resolveURL(requestURL) : requestURL;
        return (parsed.protocol === originURL.protocol &&
            parsed.host === originURL.host);
      };
    })() :

  // Non standard browser envs (web workers, react-native) lack needed support.
    (function nonStandardBrowserEnv() {
      return function isURLSameOrigin() {
        return true;
      };
    })()
);

/**
 * A `Cancel` is an object that is thrown when an operation is canceled.
 *
 * @class
 * @param {string=} message The message.
 */
function Cancel$3(message) {
  this.message = message;
}

Cancel$3.prototype.toString = function toString() {
  return 'Cancel' + (this.message ? ': ' + this.message : '');
};

Cancel$3.prototype.__CANCEL__ = true;

var Cancel_1 = Cancel$3;

var utils$7 = utils$e;
var settle = settle$1;
var cookies = cookies$1;
var buildURL$1 = buildURL$2;
var buildFullPath = buildFullPath$1;
var parseHeaders = parseHeaders$1;
var isURLSameOrigin = isURLSameOrigin$1;
var createError = createError$2;
var transitionalDefaults$1 = transitional;
var Cancel$2 = Cancel_1;

var xhr = function xhrAdapter(config) {
  return new Promise(function dispatchXhrRequest(resolve, reject) {
    var requestData = config.data;
    var requestHeaders = config.headers;
    var responseType = config.responseType;
    var onCanceled;
    function done() {
      if (config.cancelToken) {
        config.cancelToken.unsubscribe(onCanceled);
      }

      if (config.signal) {
        config.signal.removeEventListener('abort', onCanceled);
      }
    }

    if (utils$7.isFormData(requestData)) {
      delete requestHeaders['Content-Type']; // Let the browser set it
    }

    var request = new XMLHttpRequest();

    // HTTP basic authentication
    if (config.auth) {
      var username = config.auth.username || '';
      var password = config.auth.password ? unescape(encodeURIComponent(config.auth.password)) : '';
      requestHeaders.Authorization = 'Basic ' + btoa(username + ':' + password);
    }

    var fullPath = buildFullPath(config.baseURL, config.url);
    request.open(config.method.toUpperCase(), buildURL$1(fullPath, config.params, config.paramsSerializer), true);

    // Set the request timeout in MS
    request.timeout = config.timeout;

    function onloadend() {
      if (!request) {
        return;
      }
      // Prepare the response
      var responseHeaders = 'getAllResponseHeaders' in request ? parseHeaders(request.getAllResponseHeaders()) : null;
      var responseData = !responseType || responseType === 'text' ||  responseType === 'json' ?
        request.responseText : request.response;
      var response = {
        data: responseData,
        status: request.status,
        statusText: request.statusText,
        headers: responseHeaders,
        config: config,
        request: request
      };

      settle(function _resolve(value) {
        resolve(value);
        done();
      }, function _reject(err) {
        reject(err);
        done();
      }, response);

      // Clean up request
      request = null;
    }

    if ('onloadend' in request) {
      // Use onloadend if available
      request.onloadend = onloadend;
    } else {
      // Listen for ready state to emulate onloadend
      request.onreadystatechange = function handleLoad() {
        if (!request || request.readyState !== 4) {
          return;
        }

        // The request errored out and we didn't get a response, this will be
        // handled by onerror instead
        // With one exception: request that using file: protocol, most browsers
        // will return status as 0 even though it's a successful request
        if (request.status === 0 && !(request.responseURL && request.responseURL.indexOf('file:') === 0)) {
          return;
        }
        // readystate handler is calling before onerror or ontimeout handlers,
        // so we should call onloadend on the next 'tick'
        setTimeout(onloadend);
      };
    }

    // Handle browser request cancellation (as opposed to a manual cancellation)
    request.onabort = function handleAbort() {
      if (!request) {
        return;
      }

      reject(createError('Request aborted', config, 'ECONNABORTED', request));

      // Clean up request
      request = null;
    };

    // Handle low level network errors
    request.onerror = function handleError() {
      // Real errors are hidden from us by the browser
      // onerror should only fire if it's a network error
      reject(createError('Network Error', config, null, request));

      // Clean up request
      request = null;
    };

    // Handle timeout
    request.ontimeout = function handleTimeout() {
      var timeoutErrorMessage = config.timeout ? 'timeout of ' + config.timeout + 'ms exceeded' : 'timeout exceeded';
      var transitional = config.transitional || transitionalDefaults$1;
      if (config.timeoutErrorMessage) {
        timeoutErrorMessage = config.timeoutErrorMessage;
      }
      reject(createError(
        timeoutErrorMessage,
        config,
        transitional.clarifyTimeoutError ? 'ETIMEDOUT' : 'ECONNABORTED',
        request));

      // Clean up request
      request = null;
    };

    // Add xsrf header
    // This is only done if running in a standard browser environment.
    // Specifically not if we're in a web worker, or react-native.
    if (utils$7.isStandardBrowserEnv()) {
      // Add xsrf header
      var xsrfValue = (config.withCredentials || isURLSameOrigin(fullPath)) && config.xsrfCookieName ?
        cookies.read(config.xsrfCookieName) :
        undefined;

      if (xsrfValue) {
        requestHeaders[config.xsrfHeaderName] = xsrfValue;
      }
    }

    // Add headers to the request
    if ('setRequestHeader' in request) {
      utils$7.forEach(requestHeaders, function setRequestHeader(val, key) {
        if (typeof requestData === 'undefined' && key.toLowerCase() === 'content-type') {
          // Remove Content-Type if data is undefined
          delete requestHeaders[key];
        } else {
          // Otherwise add header to the request
          request.setRequestHeader(key, val);
        }
      });
    }

    // Add withCredentials to request if needed
    if (!utils$7.isUndefined(config.withCredentials)) {
      request.withCredentials = !!config.withCredentials;
    }

    // Add responseType to request if needed
    if (responseType && responseType !== 'json') {
      request.responseType = config.responseType;
    }

    // Handle progress if needed
    if (typeof config.onDownloadProgress === 'function') {
      request.addEventListener('progress', config.onDownloadProgress);
    }

    // Not all browsers support upload events
    if (typeof config.onUploadProgress === 'function' && request.upload) {
      request.upload.addEventListener('progress', config.onUploadProgress);
    }

    if (config.cancelToken || config.signal) {
      // Handle cancellation
      // eslint-disable-next-line func-names
      onCanceled = function(cancel) {
        if (!request) {
          return;
        }
        reject(!cancel || (cancel && cancel.type) ? new Cancel$2('canceled') : cancel);
        request.abort();
        request = null;
      };

      config.cancelToken && config.cancelToken.subscribe(onCanceled);
      if (config.signal) {
        config.signal.aborted ? onCanceled() : config.signal.addEventListener('abort', onCanceled);
      }
    }

    if (!requestData) {
      requestData = null;
    }

    // Send the request
    request.send(requestData);
  });
};

var utils$6 = utils$e;
var normalizeHeaderName = normalizeHeaderName$1;
var enhanceError = enhanceError$2;
var transitionalDefaults = transitional;

var DEFAULT_CONTENT_TYPE = {
  'Content-Type': 'application/x-www-form-urlencoded'
};

function setContentTypeIfUnset(headers, value) {
  if (!utils$6.isUndefined(headers) && utils$6.isUndefined(headers['Content-Type'])) {
    headers['Content-Type'] = value;
  }
}

function getDefaultAdapter() {
  var adapter;
  if (typeof XMLHttpRequest !== 'undefined') {
    // For browsers use XHR adapter
    adapter = xhr;
  } else if (typeof process !== 'undefined' && Object.prototype.toString.call(process) === '[object process]') {
    // For node use HTTP adapter
    adapter = xhr;
  }
  return adapter;
}

function stringifySafely(rawValue, parser, encoder) {
  if (utils$6.isString(rawValue)) {
    try {
      (parser || JSON.parse)(rawValue);
      return utils$6.trim(rawValue);
    } catch (e) {
      if (e.name !== 'SyntaxError') {
        throw e;
      }
    }
  }

  return (encoder || JSON.stringify)(rawValue);
}

var defaults$3 = {

  transitional: transitionalDefaults,

  adapter: getDefaultAdapter(),

  transformRequest: [function transformRequest(data, headers) {
    normalizeHeaderName(headers, 'Accept');
    normalizeHeaderName(headers, 'Content-Type');

    if (utils$6.isFormData(data) ||
      utils$6.isArrayBuffer(data) ||
      utils$6.isBuffer(data) ||
      utils$6.isStream(data) ||
      utils$6.isFile(data) ||
      utils$6.isBlob(data)
    ) {
      return data;
    }
    if (utils$6.isArrayBufferView(data)) {
      return data.buffer;
    }
    if (utils$6.isURLSearchParams(data)) {
      setContentTypeIfUnset(headers, 'application/x-www-form-urlencoded;charset=utf-8');
      return data.toString();
    }
    if (utils$6.isObject(data) || (headers && headers['Content-Type'] === 'application/json')) {
      setContentTypeIfUnset(headers, 'application/json');
      return stringifySafely(data);
    }
    return data;
  }],

  transformResponse: [function transformResponse(data) {
    var transitional = this.transitional || defaults$3.transitional;
    var silentJSONParsing = transitional && transitional.silentJSONParsing;
    var forcedJSONParsing = transitional && transitional.forcedJSONParsing;
    var strictJSONParsing = !silentJSONParsing && this.responseType === 'json';

    if (strictJSONParsing || (forcedJSONParsing && utils$6.isString(data) && data.length)) {
      try {
        return JSON.parse(data);
      } catch (e) {
        if (strictJSONParsing) {
          if (e.name === 'SyntaxError') {
            throw enhanceError(e, this, 'E_JSON_PARSE');
          }
          throw e;
        }
      }
    }

    return data;
  }],

  /**
   * A timeout in milliseconds to abort a request. If set to 0 (default) a
   * timeout is not created.
   */
  timeout: 0,

  xsrfCookieName: 'XSRF-TOKEN',
  xsrfHeaderName: 'X-XSRF-TOKEN',

  maxContentLength: -1,
  maxBodyLength: -1,

  validateStatus: function validateStatus(status) {
    return status >= 200 && status < 300;
  },

  headers: {
    common: {
      'Accept': 'application/json, text/plain, */*'
    }
  }
};

utils$6.forEach(['delete', 'get', 'head'], function forEachMethodNoData(method) {
  defaults$3.headers[method] = {};
});

utils$6.forEach(['post', 'put', 'patch'], function forEachMethodWithData(method) {
  defaults$3.headers[method] = utils$6.merge(DEFAULT_CONTENT_TYPE);
});

var defaults_1 = defaults$3;

var utils$5 = utils$e;
var defaults$2 = defaults_1;

/**
 * Transform the data for a request or a response
 *
 * @param {Object|String} data The data to be transformed
 * @param {Array} headers The headers for the request or response
 * @param {Array|Function} fns A single function or Array of functions
 * @returns {*} The resulting transformed data
 */
var transformData$1 = function transformData(data, headers, fns) {
  var context = this || defaults$2;
  /*eslint no-param-reassign:0*/
  utils$5.forEach(fns, function transform(fn) {
    data = fn.call(context, data, headers);
  });

  return data;
};

var isCancel$1 = function isCancel(value) {
  return !!(value && value.__CANCEL__);
};

var utils$4 = utils$e;
var transformData = transformData$1;
var isCancel = isCancel$1;
var defaults$1 = defaults_1;
var Cancel$1 = Cancel_1;

/**
 * Throws a `Cancel` if cancellation has been requested.
 */
function throwIfCancellationRequested(config) {
  if (config.cancelToken) {
    config.cancelToken.throwIfRequested();
  }

  if (config.signal && config.signal.aborted) {
    throw new Cancel$1('canceled');
  }
}

/**
 * Dispatch a request to the server using the configured adapter.
 *
 * @param {object} config The config that is to be used for the request
 * @returns {Promise} The Promise to be fulfilled
 */
var dispatchRequest$1 = function dispatchRequest(config) {
  throwIfCancellationRequested(config);

  // Ensure headers exist
  config.headers = config.headers || {};

  // Transform request data
  config.data = transformData.call(
    config,
    config.data,
    config.headers,
    config.transformRequest
  );

  // Flatten headers
  config.headers = utils$4.merge(
    config.headers.common || {},
    config.headers[config.method] || {},
    config.headers
  );

  utils$4.forEach(
    ['delete', 'get', 'head', 'post', 'put', 'patch', 'common'],
    function cleanHeaderConfig(method) {
      delete config.headers[method];
    }
  );

  var adapter = config.adapter || defaults$1.adapter;

  return adapter(config).then(function onAdapterResolution(response) {
    throwIfCancellationRequested(config);

    // Transform response data
    response.data = transformData.call(
      config,
      response.data,
      response.headers,
      config.transformResponse
    );

    return response;
  }, function onAdapterRejection(reason) {
    if (!isCancel(reason)) {
      throwIfCancellationRequested(config);

      // Transform response data
      if (reason && reason.response) {
        reason.response.data = transformData.call(
          config,
          reason.response.data,
          reason.response.headers,
          config.transformResponse
        );
      }
    }

    return Promise.reject(reason);
  });
};

var utils$3 = utils$e;

/**
 * Config-specific merge-function which creates a new config-object
 * by merging two configuration objects together.
 *
 * @param {Object} config1
 * @param {Object} config2
 * @returns {Object} New object resulting from merging config2 to config1
 */
var mergeConfig$2 = function mergeConfig(config1, config2) {
  // eslint-disable-next-line no-param-reassign
  config2 = config2 || {};
  var config = {};

  function getMergedValue(target, source) {
    if (utils$3.isPlainObject(target) && utils$3.isPlainObject(source)) {
      return utils$3.merge(target, source);
    } else if (utils$3.isPlainObject(source)) {
      return utils$3.merge({}, source);
    } else if (utils$3.isArray(source)) {
      return source.slice();
    }
    return source;
  }

  // eslint-disable-next-line consistent-return
  function mergeDeepProperties(prop) {
    if (!utils$3.isUndefined(config2[prop])) {
      return getMergedValue(config1[prop], config2[prop]);
    } else if (!utils$3.isUndefined(config1[prop])) {
      return getMergedValue(undefined, config1[prop]);
    }
  }

  // eslint-disable-next-line consistent-return
  function valueFromConfig2(prop) {
    if (!utils$3.isUndefined(config2[prop])) {
      return getMergedValue(undefined, config2[prop]);
    }
  }

  // eslint-disable-next-line consistent-return
  function defaultToConfig2(prop) {
    if (!utils$3.isUndefined(config2[prop])) {
      return getMergedValue(undefined, config2[prop]);
    } else if (!utils$3.isUndefined(config1[prop])) {
      return getMergedValue(undefined, config1[prop]);
    }
  }

  // eslint-disable-next-line consistent-return
  function mergeDirectKeys(prop) {
    if (prop in config2) {
      return getMergedValue(config1[prop], config2[prop]);
    } else if (prop in config1) {
      return getMergedValue(undefined, config1[prop]);
    }
  }

  var mergeMap = {
    'url': valueFromConfig2,
    'method': valueFromConfig2,
    'data': valueFromConfig2,
    'baseURL': defaultToConfig2,
    'transformRequest': defaultToConfig2,
    'transformResponse': defaultToConfig2,
    'paramsSerializer': defaultToConfig2,
    'timeout': defaultToConfig2,
    'timeoutMessage': defaultToConfig2,
    'withCredentials': defaultToConfig2,
    'adapter': defaultToConfig2,
    'responseType': defaultToConfig2,
    'xsrfCookieName': defaultToConfig2,
    'xsrfHeaderName': defaultToConfig2,
    'onUploadProgress': defaultToConfig2,
    'onDownloadProgress': defaultToConfig2,
    'decompress': defaultToConfig2,
    'maxContentLength': defaultToConfig2,
    'maxBodyLength': defaultToConfig2,
    'transport': defaultToConfig2,
    'httpAgent': defaultToConfig2,
    'httpsAgent': defaultToConfig2,
    'cancelToken': defaultToConfig2,
    'socketPath': defaultToConfig2,
    'responseEncoding': defaultToConfig2,
    'validateStatus': mergeDirectKeys
  };

  utils$3.forEach(Object.keys(config1).concat(Object.keys(config2)), function computeConfigValue(prop) {
    var merge = mergeMap[prop] || mergeDeepProperties;
    var configValue = merge(prop);
    (utils$3.isUndefined(configValue) && merge !== mergeDirectKeys) || (config[prop] = configValue);
  });

  return config;
};

var data = {
  "version": "0.26.1"
};

var VERSION = data.version;

var validators$1 = {};

// eslint-disable-next-line func-names
['object', 'boolean', 'number', 'function', 'string', 'symbol'].forEach(function(type, i) {
  validators$1[type] = function validator(thing) {
    return typeof thing === type || 'a' + (i < 1 ? 'n ' : ' ') + type;
  };
});

var deprecatedWarnings = {};

/**
 * Transitional option validator
 * @param {function|boolean?} validator - set to false if the transitional option has been removed
 * @param {string?} version - deprecated version / removed since version
 * @param {string?} message - some message with additional info
 * @returns {function}
 */
validators$1.transitional = function transitional(validator, version, message) {
  function formatMessage(opt, desc) {
    return '[Axios v' + VERSION + '] Transitional option \'' + opt + '\'' + desc + (message ? '. ' + message : '');
  }

  // eslint-disable-next-line func-names
  return function(value, opt, opts) {
    if (validator === false) {
      throw new Error(formatMessage(opt, ' has been removed' + (version ? ' in ' + version : '')));
    }

    if (version && !deprecatedWarnings[opt]) {
      deprecatedWarnings[opt] = true;
      // eslint-disable-next-line no-console
      console.warn(
        formatMessage(
          opt,
          ' has been deprecated since v' + version + ' and will be removed in the near future'
        )
      );
    }

    return validator ? validator(value, opt, opts) : true;
  };
};

/**
 * Assert object's properties type
 * @param {object} options
 * @param {object} schema
 * @param {boolean?} allowUnknown
 */

function assertOptions(options, schema, allowUnknown) {
  if (typeof options !== 'object') {
    throw new TypeError('options must be an object');
  }
  var keys = Object.keys(options);
  var i = keys.length;
  while (i-- > 0) {
    var opt = keys[i];
    var validator = schema[opt];
    if (validator) {
      var value = options[opt];
      var result = value === undefined || validator(value, opt, options);
      if (result !== true) {
        throw new TypeError('option ' + opt + ' must be ' + result);
      }
      continue;
    }
    if (allowUnknown !== true) {
      throw Error('Unknown option ' + opt);
    }
  }
}

var validator$1 = {
  assertOptions: assertOptions,
  validators: validators$1
};

var utils$2 = utils$e;
var buildURL = buildURL$2;
var InterceptorManager = InterceptorManager_1;
var dispatchRequest = dispatchRequest$1;
var mergeConfig$1 = mergeConfig$2;
var validator = validator$1;

var validators = validator.validators;
/**
 * Create a new instance of Axios
 *
 * @param {Object} instanceConfig The default config for the instance
 */
function Axios$1(instanceConfig) {
  this.defaults = instanceConfig;
  this.interceptors = {
    request: new InterceptorManager(),
    response: new InterceptorManager()
  };
}

/**
 * Dispatch a request
 *
 * @param {Object} config The config specific for this request (merged with this.defaults)
 */
Axios$1.prototype.request = function request(configOrUrl, config) {
  /*eslint no-param-reassign:0*/
  // Allow for axios('example/url'[, config]) a la fetch API
  if (typeof configOrUrl === 'string') {
    config = config || {};
    config.url = configOrUrl;
  } else {
    config = configOrUrl || {};
  }

  config = mergeConfig$1(this.defaults, config);

  // Set config.method
  if (config.method) {
    config.method = config.method.toLowerCase();
  } else if (this.defaults.method) {
    config.method = this.defaults.method.toLowerCase();
  } else {
    config.method = 'get';
  }

  var transitional = config.transitional;

  if (transitional !== undefined) {
    validator.assertOptions(transitional, {
      silentJSONParsing: validators.transitional(validators.boolean),
      forcedJSONParsing: validators.transitional(validators.boolean),
      clarifyTimeoutError: validators.transitional(validators.boolean)
    }, false);
  }

  // filter out skipped interceptors
  var requestInterceptorChain = [];
  var synchronousRequestInterceptors = true;
  this.interceptors.request.forEach(function unshiftRequestInterceptors(interceptor) {
    if (typeof interceptor.runWhen === 'function' && interceptor.runWhen(config) === false) {
      return;
    }

    synchronousRequestInterceptors = synchronousRequestInterceptors && interceptor.synchronous;

    requestInterceptorChain.unshift(interceptor.fulfilled, interceptor.rejected);
  });

  var responseInterceptorChain = [];
  this.interceptors.response.forEach(function pushResponseInterceptors(interceptor) {
    responseInterceptorChain.push(interceptor.fulfilled, interceptor.rejected);
  });

  var promise;

  if (!synchronousRequestInterceptors) {
    var chain = [dispatchRequest, undefined];

    Array.prototype.unshift.apply(chain, requestInterceptorChain);
    chain = chain.concat(responseInterceptorChain);

    promise = Promise.resolve(config);
    while (chain.length) {
      promise = promise.then(chain.shift(), chain.shift());
    }

    return promise;
  }


  var newConfig = config;
  while (requestInterceptorChain.length) {
    var onFulfilled = requestInterceptorChain.shift();
    var onRejected = requestInterceptorChain.shift();
    try {
      newConfig = onFulfilled(newConfig);
    } catch (error) {
      onRejected(error);
      break;
    }
  }

  try {
    promise = dispatchRequest(newConfig);
  } catch (error) {
    return Promise.reject(error);
  }

  while (responseInterceptorChain.length) {
    promise = promise.then(responseInterceptorChain.shift(), responseInterceptorChain.shift());
  }

  return promise;
};

Axios$1.prototype.getUri = function getUri(config) {
  config = mergeConfig$1(this.defaults, config);
  return buildURL(config.url, config.params, config.paramsSerializer).replace(/^\?/, '');
};

// Provide aliases for supported request methods
utils$2.forEach(['delete', 'get', 'head', 'options'], function forEachMethodNoData(method) {
  /*eslint func-names:0*/
  Axios$1.prototype[method] = function(url, config) {
    return this.request(mergeConfig$1(config || {}, {
      method: method,
      url: url,
      data: (config || {}).data
    }));
  };
});

utils$2.forEach(['post', 'put', 'patch'], function forEachMethodWithData(method) {
  /*eslint func-names:0*/
  Axios$1.prototype[method] = function(url, data, config) {
    return this.request(mergeConfig$1(config || {}, {
      method: method,
      url: url,
      data: data
    }));
  };
});

var Axios_1 = Axios$1;

var Cancel = Cancel_1;

/**
 * A `CancelToken` is an object that can be used to request cancellation of an operation.
 *
 * @class
 * @param {Function} executor The executor function.
 */
function CancelToken(executor) {
  if (typeof executor !== 'function') {
    throw new TypeError('executor must be a function.');
  }

  var resolvePromise;

  this.promise = new Promise(function promiseExecutor(resolve) {
    resolvePromise = resolve;
  });

  var token = this;

  // eslint-disable-next-line func-names
  this.promise.then(function(cancel) {
    if (!token._listeners) return;

    var i;
    var l = token._listeners.length;

    for (i = 0; i < l; i++) {
      token._listeners[i](cancel);
    }
    token._listeners = null;
  });

  // eslint-disable-next-line func-names
  this.promise.then = function(onfulfilled) {
    var _resolve;
    // eslint-disable-next-line func-names
    var promise = new Promise(function(resolve) {
      token.subscribe(resolve);
      _resolve = resolve;
    }).then(onfulfilled);

    promise.cancel = function reject() {
      token.unsubscribe(_resolve);
    };

    return promise;
  };

  executor(function cancel(message) {
    if (token.reason) {
      // Cancellation has already been requested
      return;
    }

    token.reason = new Cancel(message);
    resolvePromise(token.reason);
  });
}

/**
 * Throws a `Cancel` if cancellation has been requested.
 */
CancelToken.prototype.throwIfRequested = function throwIfRequested() {
  if (this.reason) {
    throw this.reason;
  }
};

/**
 * Subscribe to the cancel signal
 */

CancelToken.prototype.subscribe = function subscribe(listener) {
  if (this.reason) {
    listener(this.reason);
    return;
  }

  if (this._listeners) {
    this._listeners.push(listener);
  } else {
    this._listeners = [listener];
  }
};

/**
 * Unsubscribe from the cancel signal
 */

CancelToken.prototype.unsubscribe = function unsubscribe(listener) {
  if (!this._listeners) {
    return;
  }
  var index = this._listeners.indexOf(listener);
  if (index !== -1) {
    this._listeners.splice(index, 1);
  }
};

/**
 * Returns an object that contains a new `CancelToken` and a function that, when called,
 * cancels the `CancelToken`.
 */
CancelToken.source = function source() {
  var cancel;
  var token = new CancelToken(function executor(c) {
    cancel = c;
  });
  return {
    token: token,
    cancel: cancel
  };
};

var CancelToken_1 = CancelToken;

/**
 * Syntactic sugar for invoking a function and expanding an array for arguments.
 *
 * Common use case would be to use `Function.prototype.apply`.
 *
 *  ```js
 *  function f(x, y, z) {}
 *  var args = [1, 2, 3];
 *  f.apply(null, args);
 *  ```
 *
 * With `spread` this example can be re-written.
 *
 *  ```js
 *  spread(function(x, y, z) {})([1, 2, 3]);
 *  ```
 *
 * @param {Function} callback
 * @returns {Function}
 */
var spread = function spread(callback) {
  return function wrap(arr) {
    return callback.apply(null, arr);
  };
};

var utils$1 = utils$e;

/**
 * Determines whether the payload is an error thrown by Axios
 *
 * @param {*} payload The value to test
 * @returns {boolean} True if the payload is an error thrown by Axios, otherwise false
 */
var isAxiosError = function isAxiosError(payload) {
  return utils$1.isObject(payload) && (payload.isAxiosError === true);
};

var utils = utils$e;
var bind = bind$2;
var Axios = Axios_1;
var mergeConfig = mergeConfig$2;
var defaults = defaults_1;

/**
 * Create an instance of Axios
 *
 * @param {Object} defaultConfig The default config for the instance
 * @return {Axios} A new instance of Axios
 */
function createInstance(defaultConfig) {
  var context = new Axios(defaultConfig);
  var instance = bind(Axios.prototype.request, context);

  // Copy axios.prototype to instance
  utils.extend(instance, Axios.prototype, context);

  // Copy context to instance
  utils.extend(instance, context);

  // Factory for creating new instances
  instance.create = function create(instanceConfig) {
    return createInstance(mergeConfig(defaultConfig, instanceConfig));
  };

  return instance;
}

// Create the default instance to be exported
var axios$1 = createInstance(defaults);

// Expose Axios class to allow class inheritance
axios$1.Axios = Axios;

// Expose Cancel & CancelToken
axios$1.Cancel = Cancel_1;
axios$1.CancelToken = CancelToken_1;
axios$1.isCancel = isCancel$1;
axios$1.VERSION = data.version;

// Expose all/spread
axios$1.all = function all(promises) {
  return Promise.all(promises);
};
axios$1.spread = spread;

// Expose isAxiosError
axios$1.isAxiosError = isAxiosError;

axios$2.exports = axios$1;

// Allow use of default import syntax in TypeScript
axios$2.exports.default = axios$1;

var axios = axios$2.exports;

let storedBaseUrl = "";
const setBaseUrl = (baseUrl) => {
    storedBaseUrl = baseUrl.substr(-1) === "/" ? baseUrl.slice(0, -1) : baseUrl;
};
const getBaseUrl = () => storedBaseUrl;

let currentInstance = null;
let currentConfig = null;
const defaultConfig = {
    baseUrl: getBaseUrl(),
};
// sets the current config
const initialise = (config = defaultConfig) => {
    currentConfig = config;
    return axios.create({
        baseURL: config.baseUrl,
        timeout: config.timeout,
        headers: config.tenant ? { "x-tenant": config.tenant } : {},
        validateStatus: function (status) {
            // return status < 500; // Resolve only if the status code is less than 500
            return true; // always resolve the promise
        },
    });
};
const current = (config) => {
    // if current instance is null or undefined
    if (!currentInstance) {
        currentInstance = initialise(config);
    }
    else if (!config) {
        currentInstance = initialise();
    }
    // if config isn't exactly the same object
    else if (config !== currentConfig) {
        if (config.baseUrl !== (currentConfig === null || currentConfig === void 0 ? void 0 : currentConfig.baseUrl) ||
            config.tenant !== currentConfig.tenant ||
            config.timeout !== currentConfig.timeout) {
            // then create a new instance and config
            currentInstance = initialise(config);
        }
    }
    return currentInstance;
};

var axiosInstance = /*#__PURE__*/Object.freeze({
  __proto__: null,
  current: current
});

// tenant
let storedTenant = null;
const setTenant = (tenant) => {
    console.debug(`Setting tenant: ${tenant}`);
    storedTenant = tenant;
};
// environment
let defaltEnvironmentId = null;
const setDefaultEnvironmentId$1 = (e) => {
    defaltEnvironmentId = e;
};
let defaultApiKey = null;
const setDefaultApiKey = (k) => {
    defaultApiKey = k;
};
const defaultHeaders = {
    "Content-Type": "application/json",
};
const headers = (token, apiKey) => {
    let headers = { ...defaultHeaders };
    if (storedTenant) {
        headers["x-tenant"] = storedTenant;
    }
    if (token) {
        headers.Authorization = `Bearer ${token}`;
    }
    if (apiKey) {
        headers["x-api-key"] = `${apiKey}`; // ensure its a string
    }
    else if (defaultApiKey) {
        headers["x-api-key"] = `${defaultApiKey}`; // ensure its a string
    }
    if (defaltEnvironmentId) {
        headers["x-environment"] = defaltEnvironmentId;
    }
    return headers;
};

const defaultErrorResponseHandler = async (response) => {
    console.debug(response);
    // is not a promise
    if (response.status >= 500) {
        console.error(`Server responded: ${response.statusText}`);
        console.error(response.data);
        return { error: response.data };
    }
    else if (response.status >= 400) {
        console.warn(`Server responded: ${response.statusText}`);
        console.warn(response.data);
        throw response.data;
    }
};
let errorResponseHandler = defaultErrorResponseHandler;
const setErrorResponseHandler = (errorHandler) => {
    errorResponseHandler = errorHandler;
};
// this function is called in api.js functions.
const handleErrorResponse = async (response) => {
    console.debug("SDK is handling an error response");
    return await errorResponseHandler(response);
};

var errorHandling = /*#__PURE__*/Object.freeze({
  __proto__: null,
  setErrorResponseHandler: setErrorResponseHandler,
  handleErrorResponse: handleErrorResponse
});

const executeFetch = async ({ token, apiKey, path, page, pageSize, body, method, query, responseType, }) => {
    const baseUrl = getBaseUrl();
    const client = current({ baseUrl: baseUrl });
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query || {})) {
        if (key && value) {
            params.append(key, value);
        }
    }
    if (page) {
        params.append("p.page", `${page}`);
    }
    if (pageSize) {
        params.append("p.pageSize", `${pageSize}`);
    }
    if (apiKey) {
        params.append("apiKey", `${apiKey}`);
    }
    let response;
    try {
        response = await client({
            method,
            url: path,
            params,
            headers: headers(token, null),
            data: body,
            responseType: responseType,
        });
    }
    catch (ex) {
        // something failed. ther server responded outside of 2xx
        // if we always resolve the promise, then this is always true
        console.error(ex);
        throw ex;
    }
    // happy path
    if (response.status <= 299) {
        return response.data;
    }
    else {
        console.debug(response);
        return await handleErrorResponse(response);
    }
};

const fetchActivityFeedEntitiesAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/ActivityFeed",
        token,
        page,
    });
};

var activityFeedApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchActivityFeedEntitiesAsync: fetchActivityFeedEntitiesAsync
});

const fetchApiKeysAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        page,
    });
};
var ApiKeyType;
(function (ApiKeyType) {
    ApiKeyType["Server"] = "Server";
    ApiKeyType["Web"] = "Web";
})(ApiKeyType || (ApiKeyType = {}));
const createApiKeyAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/apiKeys",
        token,
        method: "post",
        body: payload,
    });
};
const exchangeApiKeyAsync = async ({ apiKey, }) => {
    return await executeFetch({
        path: "api/apiKeys/exchange",
        method: "post",
        body: { apiKey },
    });
};
const deleteApiKeyAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/apiKeys/${id}`,
        token,
        method: "delete",
    });
};

var apiKeyApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchApiKeysAsync: fetchApiKeysAsync,
  createApiKeyAsync: createApiKeyAsync,
  exchangeApiKeyAsync: exchangeApiKeyAsync,
  deleteApiKeyAsync: deleteApiKeyAsync
});

const fetchBusinessesAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchBusinessAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
    });
};
const deleteBusinessAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Businesses/${id}`,
        token,
        method: "delete",
    });
};
const createBusinessAsync = async ({ token, business, }) => {
    return await executeFetch({
        path: "api/Businesses",
        token,
        method: "post",
        body: business,
    });
};
const updateBusinessPropertiesAsync = async ({ token, id, properties, }) => {
    return await executeFetch({
        token,
        path: `api/Businesses/${id}/Properties`,
        method: "post",
        body: properties,
    });
};
const fetchBusinessMembersAsync = async ({ token, id, page, searchTerm, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members`,
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const deleteBusinessMemberAsync = async ({ token, id, customerId, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members/${customerId}`,
        token,
        method: "delete",
    });
};
const addBusinessMemberAsync = async ({ token, id, customer, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Members`,
        token,
        method: "post",
        body: customer,
    });
};
const fetchRecommendationsAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/businesses/${id}/recommendations`,
    });
};

var businessesApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchBusinessesAsync: fetchBusinessesAsync,
  fetchBusinessAsync: fetchBusinessAsync,
  deleteBusinessAsync: deleteBusinessAsync,
  createBusinessAsync: createBusinessAsync,
  updateBusinessPropertiesAsync: updateBusinessPropertiesAsync,
  fetchBusinessMembersAsync: fetchBusinessMembersAsync,
  deleteBusinessMemberAsync: deleteBusinessMemberAsync,
  addBusinessMemberAsync: addBusinessMemberAsync,
  fetchRecommendationsAsync: fetchRecommendationsAsync
});

const fetchChannelsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        page,
    });
};
const createChannelAsync = async ({ token, channel, }) => {
    return await executeFetch({
        path: "api/Channels",
        token,
        method: "post",
        body: channel,
    });
};
const fetchChannelAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
    });
};
const deleteChannelAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Channels/${id}`,
        token,
        method: "delete",
    });
};
const updateChannelEndpointAsync = async ({ token, id, endpoint, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/Endpoint`,
        method: "post",
        body: endpoint,
    });
};
const updateChannelPropertiesAsync = async ({ token, id, properties, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/WebProperties`,
        method: "post",
        body: properties,
    });
};
const updateEmailChannelTriggerAsync = async ({ token, id, listTrigger, }) => {
    return await executeFetch({
        token,
        path: `api/Channels/${id}/EmailTrigger`,
        method: "post",
        body: listTrigger,
    });
};

var channelsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchChannelsAsync: fetchChannelsAsync,
  createChannelAsync: createChannelAsync,
  fetchChannelAsync: fetchChannelAsync,
  deleteChannelAsync: deleteChannelAsync,
  updateChannelEndpointAsync: updateChannelEndpointAsync,
  updateChannelPropertiesAsync: updateChannelPropertiesAsync,
  updateEmailChannelTriggerAsync: updateEmailChannelTriggerAsync
});

/**
 * Returns an array with arrays of the given size.
 *
 * @param myArray {Array} array to split
 * @param chunk_size {Integer} Size of every group
 */
function chunkArray(myArray, chunk_size) {
    let index = 0;
    const arrayLength = myArray.length;
    const tempArray = [];
    for (index = 0; index < arrayLength; index += chunk_size) {
        const myChunk = myArray.slice(index, index + chunk_size);
        // Do something if you want with the group
        tempArray.push(myChunk);
    }
    return tempArray;
}

const MAX_ARRAY = 5000;
const basePath = "api/Customers";
const fetchCustomersAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
        path: basePath,
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const updateMergePropertiesAsync$1 = async ({ token, id, properties }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/properties`,
        method: "post",
        body: properties,
    });
};
const fetchCustomerAsync = async ({ token, id, useInternalId }) => {
    return await executeFetch({
        path: `${basePath}/${id}`,
        token,
        query: {
            useInternalId,
        },
    });
};
const fetchUniqueCustomerActionGroupsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/action-groups`,
    });
};
const fetchLatestRecommendationsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/latest-recommendations`,
    });
};
const fetchCustomerActionAsync = async ({ token, id, category, actionName, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/actions/${category}`,
        token,
        query: {
            actionName,
        },
    });
};
const uploadUserDataAsync$1 = async ({ token, payload }) => {
    const payloads = chunkArray(payload.users, MAX_ARRAY).map((users) => ({
        users,
    }));
    const responses = [];
    for (const p of payloads) {
        const response = await executeFetch({
            token,
            path: basePath,
            method: "put",
            body: p,
        });
        if (response.ok) {
            responses.push(await response.json());
        }
        else {
            return await handleErrorResponse(response);
        }
    }
    return responses;
};
const createOrUpdateCustomerAsync = async ({ token, customer, user, }) => {
    if (user) {
        console.warn("user is a deprecated property in createOrUpdateCustomerAsync(). use 'customer'.");
    }
    return await executeFetch({
        path: basePath,
        method: "post",
        body: customer || user,
        token,
    });
};
const fetchCustomersActionsAsync = async ({ token, page, id, revenueOnly, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/Actions`,
        token,
        page,
        query: {
            revenueOnly: !!revenueOnly,
        },
    });
};
const setCustomerMetricAsync = async ({ token, id, metricId, useInternalId, value, }) => {
    return await executeFetch({
        path: `${basePath}/${id}/Metrics/${metricId}`,
        method: "post",
        token,
        query: {
            useInternalId,
        },
        body: { value },
    });
};
const deleteCustomerAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `${basePath}/${id}`,
        token,
        method: "delete",
    });
};
const fetchCustomerSegmentsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `${basePath}/${id}/segments`,
    });
};

var customersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchCustomersAsync: fetchCustomersAsync,
  updateMergePropertiesAsync: updateMergePropertiesAsync$1,
  fetchCustomerAsync: fetchCustomerAsync,
  fetchUniqueCustomerActionGroupsAsync: fetchUniqueCustomerActionGroupsAsync,
  fetchLatestRecommendationsAsync: fetchLatestRecommendationsAsync$1,
  fetchCustomerActionAsync: fetchCustomerActionAsync,
  uploadUserDataAsync: uploadUserDataAsync$1,
  createOrUpdateCustomerAsync: createOrUpdateCustomerAsync,
  fetchCustomersActionsAsync: fetchCustomersActionsAsync,
  setCustomerMetricAsync: setCustomerMetricAsync,
  deleteCustomerAsync: deleteCustomerAsync$1,
  fetchCustomerSegmentsAsync: fetchCustomerSegmentsAsync
});

const fetchEventSummaryAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/datasummary/events",
        token,
    });
};
const fetchEventKindNamesAsync = async ({ token }) => {
    return await executeFetch({
        path: `api/datasummary/event-kind-names`,
        token,
    });
};
const fetchEventKindSummaryAsync = async ({ token, kind }) => {
    return await executeFetch({
        path: `api/datasummary/event-kind/${kind}`,
        token,
    });
};
const fetchEventTimelineAsync = async ({ token, kind, eventType }) => {
    return await executeFetch({
        path: `api/datasummary/events/timeline/${kind}/${eventType}`,
        token,
    });
};
const fetchGeneralSummaryAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/DataSummary/GeneralSummary",
        token,
    });
};
const fetchLatestActionsAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/datasummary/actions",
        token,
    });
};

var dataSummaryApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchEventSummaryAsync: fetchEventSummaryAsync,
  fetchEventKindNamesAsync: fetchEventKindNamesAsync,
  fetchEventKindSummaryAsync: fetchEventKindSummaryAsync,
  fetchEventTimelineAsync: fetchEventTimelineAsync,
  fetchGeneralSummaryAsync: fetchGeneralSummaryAsync,
  fetchLatestActionsAsync: fetchLatestActionsAsync
});

const fetchDeploymentConfigurationAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/deployment/configuration",
        token,
    });
};

var deploymentApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchDeploymentConfigurationAsync: fetchDeploymentConfigurationAsync
});

const fetchEventAsync = async ({ id, token }) => {
    return await executeFetch({
        token,
        path: `api/events/${id}`,
    });
};
const eventKinds = {
    custom: "custom",
    propertyUpdate: "propertyUpdate",
    behaviour: "behaviour",
    pageView: "pageView",
    identify: "identify",
    consumeRecommendation: "consumeRecommendation",
    addToBusiness: "addToBusiness",
    purchase: "purchase",
    usePromotion: "usePromotion",
    promotionPresented: "promotionPresented",
};
const createEventsAsync = async ({ apiKey, token, events, }) => {
    return await executeFetch({
        path: "api/events",
        method: "post",
        token,
        apiKey,
        body: events,
    });
};
const fetchCustomersEventsAsync = async ({ token, id, page, pageSize, useInternalId, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/events`,
        token,
        page,
        pageSize,
        query: {
            useInternalId,
        },
    });
};
const fetchTrackedUsersEventsAsync = fetchCustomersEventsAsync;
// useful extension methods to create certain event kinds
const createRecommendationConsumedEventAsync = async ({ token, commonUserId, customerId, correlatorId, properties, }) => {
    const payload = {
        commonUserId,
        customerId,
        eventId: `recommendation-${correlatorId}-${new Date().getTime()}`,
        recommendationCorrelatorId: correlatorId,
        kind: "consumeRecommendation",
        eventType: "generated",
        properties,
    };
    return await createEventsAsync({ token, events: [payload] });
};
const fetchBusinessEventsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/events`,
        token,
    });
};

var eventsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchEventAsync: fetchEventAsync,
  eventKinds: eventKinds,
  createEventsAsync: createEventsAsync,
  fetchCustomersEventsAsync: fetchCustomersEventsAsync,
  fetchTrackedUsersEventsAsync: fetchTrackedUsersEventsAsync,
  createRecommendationConsumedEventAsync: createRecommendationConsumedEventAsync,
  fetchBusinessEventsAsync: fetchBusinessEventsAsync
});

const fetchEnvironmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        page,
    });
};
const createEnvironmentAsync = async ({ token, environment }) => {
    return await executeFetch({
        path: "api/Environments",
        token,
        method: "post",
        body: environment,
    });
};
const deleteEnvironmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Environments/${id}`,
        token,
        method: "delete",
    });
};
const setDefaultEnvironmentId = setDefaultEnvironmentId$1;

var environmentsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchEnvironmentsAsync: fetchEnvironmentsAsync,
  createEnvironmentAsync: createEnvironmentAsync,
  deleteEnvironmentAsync: deleteEnvironmentAsync,
  setDefaultEnvironmentId: setDefaultEnvironmentId
});

console.warn("Deprecation Notice: Features are replaced by Metrics.");
const fetchFeatureGeneratorsAsync = async ({ page, token }) => {
    return await executeFetch({
        path: "api/FeatureGenerators",
        token,
        page,
    });
};
const createFeatureGeneratorAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/FeatureGenerators",
        token,
        method: "post",
        body: payload,
    });
};
const deleteFeatureGeneratorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/FeatureGenerators/${id}`,
        token,
        method: "delete",
    });
};
const manualTriggerFeatureGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        token,
        path: `api/FeatureGenerators/${id}/Trigger`,
        method: "post",
        body: {},
    });
};

var featureGeneratorsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchFeatureGeneratorsAsync: fetchFeatureGeneratorsAsync,
  createFeatureGeneratorAsync: createFeatureGeneratorAsync,
  deleteFeatureGeneratorAsync: deleteFeatureGeneratorAsync,
  manualTriggerFeatureGeneratorsAsync: manualTriggerFeatureGeneratorsAsync
});

console.warn("Deprecation Notice: Feature Generators are replaced by Metric Generators.");
const fetchFeaturesAsync = async ({ token, page, searchTerm }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
    });
};
const fetchFeatureTrackedUsersAsync = async ({ token, page, id }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUsers`,
        token,
        page,
    });
};
const fetchFeatureTrackedUserFeaturesAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Features/${id}/TrackedUserFeatures`,
        token,
        page,
    });
};
const createFeatureAsync = async ({ token, feature }) => {
    return await executeFetch({
        path: "api/Features",
        token,
        method: "post",
        body: feature,
    });
};
const deleteFeatureAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}`,
        token,
        method: "delete",
    });
};
const fetchTrackedUserFeaturesAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features`,
        token,
    });
};
const fetchTrackedUserFeatureValuesAsync = async ({ token, id, feature, version, }) => {
    return await executeFetch({
        path: `api/TrackedUsers/${id}/features/${feature}`,
        token,
        query: {
            version,
        },
    });
};
const fetchDestinationsAsync$8 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
    });
};
const createDestinationAsync$8 = async ({ token, id, destination }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
const deleteDestinationAsync$1 = async ({ token, id, destinationId }) => {
    return await executeFetch({
        path: `api/features/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
const fetchGeneratorsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/features/${id}/Generators`,
        token,
    });
};

var featuresApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchFeaturesAsync: fetchFeaturesAsync,
  fetchFeatureAsync: fetchFeatureAsync,
  fetchFeatureTrackedUsersAsync: fetchFeatureTrackedUsersAsync,
  fetchFeatureTrackedUserFeaturesAsync: fetchFeatureTrackedUserFeaturesAsync,
  createFeatureAsync: createFeatureAsync,
  deleteFeatureAsync: deleteFeatureAsync,
  fetchTrackedUserFeaturesAsync: fetchTrackedUserFeaturesAsync,
  fetchTrackedUserFeatureValuesAsync: fetchTrackedUserFeatureValuesAsync,
  fetchDestinationsAsync: fetchDestinationsAsync$8,
  createDestinationAsync: createDestinationAsync$8,
  deleteDestinationAsync: deleteDestinationAsync$1,
  fetchGeneratorsAsync: fetchGeneratorsAsync$1
});

const fetchMetricsAsync = async ({ token, page, scope, searchTerm, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.scope": scope,
        },
    });
};
const fetchMetricAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
    });
};
const fetchMetricCustomersAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Customers`,
        token,
        page,
    });
};
const fetchMetricCustomerMetricsAsync = async ({ token, page, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/CustomerMetrics`,
        token,
        page,
    });
};
const createMetricAsync = async ({ token, metric, }) => {
    return await executeFetch({
        path: "api/Metrics",
        token,
        method: "post",
        body: metric,
    });
};
const deleteMetricAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}`,
        token,
        method: "delete",
    });
};
const fetchCustomersMetricsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics`,
        token,
    });
};
const fetchCustomersMetricAsync = async ({ token, id, metricId, version, }) => {
    return await executeFetch({
        path: `api/Customers/${id}/Metrics/${metricId}`,
        token,
        query: {
            version,
        },
    });
};
const fetchAggregateMetricValuesNumericAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesNumeric`,
        token,
    });
};
const fetchAggregateMetricValuesStringAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/AggregateMetricValuesString`,
        token,
    });
};
const fetchDestinationsAsync$7 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
    });
};
const fetchExportCustomers = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/ExportCustomers`,
        token,
        responseType: "blob",
    });
};
const fetchMetricBinValuesNumericAsync = async ({ token, id, binCount, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/NumericMetricBinValues`,
        token,
        query: {
            binCount,
        },
    });
};
const fetchMetricBinValuesStringAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/CategoricalMetricBinValues`,
        token,
    });
};
const createDestinationAsync$7 = async ({ token, id, destination, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations`,
        token,
        method: "post",
        body: destination,
    });
};
const deleteDestinationAsync = async ({ token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};
const fetchGeneratorsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Metrics/${id}/Generators`,
        token,
    });
};
const fetchBusinessMetricsAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Metrics`,
        token,
    });
};
const fetchBusinessMetricAsync = async ({ token, id, metricId, version, }) => {
    return await executeFetch({
        path: `api/Businesses/${id}/Metrics/${metricId}`,
        token,
        query: {
            version,
        },
    });
};

var metricsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchMetricsAsync: fetchMetricsAsync,
  fetchMetricAsync: fetchMetricAsync,
  fetchMetricCustomersAsync: fetchMetricCustomersAsync,
  fetchMetricCustomerMetricsAsync: fetchMetricCustomerMetricsAsync,
  createMetricAsync: createMetricAsync,
  deleteMetricAsync: deleteMetricAsync,
  fetchCustomersMetricsAsync: fetchCustomersMetricsAsync,
  fetchCustomersMetricAsync: fetchCustomersMetricAsync,
  fetchAggregateMetricValuesNumericAsync: fetchAggregateMetricValuesNumericAsync,
  fetchAggregateMetricValuesStringAsync: fetchAggregateMetricValuesStringAsync,
  fetchDestinationsAsync: fetchDestinationsAsync$7,
  fetchExportCustomers: fetchExportCustomers,
  fetchMetricBinValuesNumericAsync: fetchMetricBinValuesNumericAsync,
  fetchMetricBinValuesStringAsync: fetchMetricBinValuesStringAsync,
  createDestinationAsync: createDestinationAsync$7,
  deleteDestinationAsync: deleteDestinationAsync,
  fetchGeneratorsAsync: fetchGeneratorsAsync,
  fetchBusinessMetricsAsync: fetchBusinessMetricsAsync,
  fetchBusinessMetricAsync: fetchBusinessMetricAsync
});

const fetchMetricGeneratorsAsync = async ({ page, token, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        page,
    });
};
const createMetricGeneratorAsync = async ({ token, generator, }) => {
    return await executeFetch({
        path: "api/MetricGenerators",
        token,
        method: "post",
        body: generator,
    });
};
const deleteMetricGeneratorAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/MetricGenerators/${id}`,
        token,
        method: "delete",
    });
};
const manualTriggerMetricGeneratorsAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/MetricGenerators/${id}/Trigger`,
        method: "post",
        body: {},
    });
};

var metricGeneratorsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchMetricGeneratorsAsync: fetchMetricGeneratorsAsync,
  createMetricGeneratorAsync: createMetricGeneratorAsync,
  deleteMetricGeneratorAsync: deleteMetricGeneratorAsync,
  manualTriggerMetricGeneratorsAsync: manualTriggerMetricGeneratorsAsync
});

const fetchIntegratedSystemsAsync = async ({ token, page, systemType, }) => {
    return await executeFetch({
        path: "api/IntegratedSystems",
        token,
        page,
        query: {
            "q.scope": systemType,
        },
    });
};
const fetchIntegratedSystemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/IntegratedSystems/${id}`,
        token,
    });
};
const renameAsync = async ({ token, id, name }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/name`,
        token,
        method: "post",
        query: {
            name,
        },
    });
};
const createIntegratedSystemAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/IntegratedSystems",
        token,
        method: "post",
        body: payload,
    });
};
const deleteIntegratedSystemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}`,
        token,
        method: "delete",
    });
};
const fetchWebhookReceiversAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/webhookreceivers`,
        token,
    });
};
const createWebhookReceiverAsync = async ({ token, id, useSharedSecret, }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/webhookreceivers`,
        token,
        method: "post",
        body: {},
        query: {
            useSharedSecret,
        },
    });
};
const setIsDCGeneratorAsync = async ({ token, id, value }) => {
    return await executeFetch({
        path: `api/integratedSystems/${id}/dcgenerator`,
        token,
        method: "post",
        body: value,
    });
};

var integratedSystemsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchIntegratedSystemsAsync: fetchIntegratedSystemsAsync,
  fetchIntegratedSystemAsync: fetchIntegratedSystemAsync,
  renameAsync: renameAsync,
  createIntegratedSystemAsync: createIntegratedSystemAsync,
  deleteIntegratedSystemAsync: deleteIntegratedSystemAsync,
  fetchWebhookReceiversAsync: fetchWebhookReceiversAsync,
  createWebhookReceiverAsync: createWebhookReceiverAsync,
  setIsDCGeneratorAsync: setIsDCGeneratorAsync
});

const fetchModelRegistrationsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/ModelRegistrations",
        token,
        page,
    });
};
const fetchModelRegistrationAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${id}`,
        token,
    });
};
const deleteModelRegistrationAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${id}`,
        token,
        method: "delete",
    });
};
const createModelRegistrationAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/ModelRegistrations",
        token,
        method: "post",
        body: payload,
    });
};
const invokeModelAsync = async ({ token, modelId, metrics }) => {
    return await executeFetch({
        path: `api/ModelRegistrations/${modelId}/invoke`,
        token,
        method: "post",
        body: metrics,
    });
};

var modelRegistrationsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchModelRegistrationsAsync: fetchModelRegistrationsAsync,
  fetchModelRegistrationAsync: fetchModelRegistrationAsync,
  deleteModelRegistrationAsync: deleteModelRegistrationAsync,
  createModelRegistrationAsync: createModelRegistrationAsync,
  invokeModelAsync: invokeModelAsync
});

const invokeGenericModelAsync = async ({ token, id, input }) => {
    return await executeFetch({
        path: `api/models/generic/${id}/invoke`,
        body: input,
        method: "post",
        token,
    });
};

var index = /*#__PURE__*/Object.freeze({
  __proto__: null,
  invokeGenericModelAsync: invokeGenericModelAsync
});

const fetchParametersAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        page,
    });
};
const fetchParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
    });
};
const deleteParameterAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/parameters/${id}`,
        token,
        method: "delete",
    });
};
const createParameterAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Parameters",
        token,
        method: "post",
        body: payload,
    });
};

var parametersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchParametersAsync: fetchParametersAsync,
  fetchParameterAsync: fetchParameterAsync,
  deleteParameterAsync: deleteParameterAsync,
  createParameterAsync: createParameterAsync
});

const fetchLinkedRegisteredModelAsync$6 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
        token,
    });
};
const createLinkedRegisteredModelAsync$1 = async ({ recommenderApiName, token, id, modelId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ModelRegistration`,
        token,
        method: "post",
        body: { modelId },
    });
};

const fetchRecommenderTargetVariableValuesAsync = async ({ recommenderApiName, name, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
        token,
        query: {
            name,
        },
    });
};
const createRecommenderTargetVariableValueAsync = async ({ recommenderApiName, targetVariableValue, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TargetVariableValues`,
        token,
        method: "post",
        body: targetVariableValue,
    });
};

const fetchRecommenderInvokationLogsAsync = async ({ recommenderApiName, token, id, page, pageSize, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/InvokationLogs`,
        page,
        pageSize,
        token,
    });
};

const setArgumentsAsync$6 = async ({ recommenderApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
const fetchArgumentsAsync$5 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
const fetchChoosePromotionArgumentRulesAsync$3 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChoosePromotionArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChoosePromotionArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const fetchChooseSegmentArgumentRulesAsync$3 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChooseSegmentArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChooseSegmentArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ChooseSegmentArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const deleteArgumentRuleAsync$3 = async ({ recommenderApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};

const setSettingsAsync$6 = async ({ recommenderApiName, token, id, settings, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Settings`,
        token,
        method: "post",
        body: settings,
    });
};

const fetchDestinationsAsync$6 = async ({ recommenderApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
    });
};
const createDestinationAsync$6 = async ({ recommenderApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
const removeDestinationAsync$6 = async ({ recommenderApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};

const setTriggerAsync$6 = async ({ recommenderApiName, token, id, trigger, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
        token,
        method: "post",
        body: trigger,
    });
};
const fetchTriggerAsync$6 = async ({ recommenderApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/${recommenderApiName}/${id}/TriggerCollection`,
    });
};

const fetchLearningFeaturesAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
};
const setLearningFeaturesAsync$6 = async ({ recommenderApiName, token, id, useInternalId, featureIds, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { featureIds, useInternalId },
    });
};

const fetchLearningMetricsAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
    });
};
const setLearningMetricsAsync$6 = async ({ recommenderApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/recommenders/${recommenderApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};

const fetchReportImageBlobUrlAsync$6 = async ({ recommenderApiName, token, id, useInternalId, }) => {
    console.debug("fetching image for recommender");
    console.debug(`api/recommenders/${recommenderApiName}/${id}/ReportImage`);
    const axios = current();
    let response;
    try {
        response = await axios.get(`api/recommenders/${recommenderApiName}/${id}/ReportImage`, {
            headers: headers(token, null),
            responseType: "blob", // required for axios to understand response
        });
    }
    catch (ex) {
        console.error(ex);
        throw ex;
    }
    if (response.status >= 200 && response.status < 300) {
        return URL.createObjectURL(response.data);
    }
    else {
        handleErrorResponse(response);
    }
};

const recommenderApiName$2 = "ParameterSetRecommenders";
console.warn("Deprecation Notice: Parameter Set Recommenders are replaced by Parameter Set Campaigns.");
const fetchParameterSetRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        page,
    });
};
const fetchParameterSetRecommenderAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
const createParameterSetRecommenderAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/recommenders/ParameterSetRecommenders",
        token,
        method: "post",
        body: payload,
    });
};
const deleteParameterSetRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const fetchParameterSetRecommendationsAsync$1 = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/recommendations`,
        token,
        page,
        pageSize,
    });
};
const createLinkRegisteredModelAsync$4 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName: recommenderApiName$2,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$5 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const invokeParameterSetRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$4 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$4 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$4 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$5 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$4 = async ({ id, token }) => {
    return await fetchArgumentsAsync$5({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const setArgumentsAsync$5 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$5 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const createDestinationAsync$5 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$5 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$5 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
    });
};
const setTriggerAsync$5 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$5 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$5 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$4 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/ParameterSetRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$5 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName: recommenderApiName$2,
        id,
        token,
        useInternalId,
    });
};

var parameterSetRecommendersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchParameterSetRecommendersAsync: fetchParameterSetRecommendersAsync,
  fetchParameterSetRecommenderAsync: fetchParameterSetRecommenderAsync,
  createParameterSetRecommenderAsync: createParameterSetRecommenderAsync,
  deleteParameterSetRecommenderAsync: deleteParameterSetRecommenderAsync,
  fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync$1,
  createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$4,
  fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$5,
  invokeParameterSetRecommenderAsync: invokeParameterSetRecommenderAsync,
  fetchInvokationLogsAsync: fetchInvokationLogsAsync$4,
  fetchTargetVariablesAsync: fetchTargetVariablesAsync$4,
  createTargetVariableAsync: createTargetVariableAsync$4,
  setSettingsAsync: setSettingsAsync$5,
  fetchArgumentsAsync: fetchArgumentsAsync$4,
  setArgumentsAsync: setArgumentsAsync$5,
  fetchDestinationsAsync: fetchDestinationsAsync$5,
  createDestinationAsync: createDestinationAsync$5,
  removeDestinationAsync: removeDestinationAsync$5,
  fetchTriggerAsync: fetchTriggerAsync$5,
  setTriggerAsync: setTriggerAsync$5,
  fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$5,
  setLearningFeaturesAsync: setLearningFeaturesAsync$5,
  fetchLearningMetricsAsync: fetchLearningMetricsAsync$5,
  setLearningMetricsAsync: setLearningMetricsAsync$5,
  fetchStatisticsAsync: fetchStatisticsAsync$4,
  fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$5
});

const fetchLinkedRegisteredModelAsync$4 = async ({ campaignApiName, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
        token,
    });
};
const createLinkedRegisteredModelAsync = async ({ campaignApiName, token, id, modelId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ModelRegistration`,
        token,
        method: "post",
        body: { modelId },
    });
};

const fetchCampaignTargetVariableValuesAsync = async ({ campaignApiName, name, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        query: {
            name,
        },
    });
};
const createCampaignTargetVariableValueAsync = async ({ campaignApiName, targetVariableValue, token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TargetVariableValues`,
        token,
        method: "post",
        body: targetVariableValue,
    });
};

const fetchCampaignInvokationLogsAsync = async ({ campaignApiName, token, id, page, pageSize, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/InvokationLogs`,
        page,
        pageSize,
        token,
    });
};

const setArgumentsAsync$4 = async ({ campaignApiName, token, id, args, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "post",
        body: args,
    });
};
const fetchArgumentsAsync$3 = async ({ campaignApiName, token, id }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Arguments`,
        token,
        method: "get",
    });
};
const fetchChoosePromotionArgumentRulesAsync$2 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChoosePromotionArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChoosePromotionArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChoosePromotionArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const fetchChooseSegmentArgumentRulesAsync$2 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "get",
        query: {
            useInternalId,
        },
    });
};
const createChooseSegmentArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const updateChooseSegmentArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, rule, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ChooseSegmentArgumentRules/${ruleId}`,
        token,
        method: "post",
        body: rule,
        query: {
            useInternalId,
        },
    });
};
const deleteArgumentRuleAsync$2 = async ({ campaignApiName, token, id, useInternalId, ruleId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/ArgumentRules/${ruleId}`,
        token,
        method: "delete",
        query: {
            useInternalId,
        },
    });
};

const setSettingsAsync$4 = async ({ campaignApiName, token, id, settings, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Settings`,
        token,
        method: "post",
        body: settings,
    });
};

const fetchDestinationsAsync$4 = async ({ campaignApiName, token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
    });
};
const createDestinationAsync$4 = async ({ campaignApiName, token, id, destination, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/Destinations`,
        method: "post",
        body: destination,
    });
};
const removeDestinationAsync$4 = async ({ campaignApiName, token, id, destinationId, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/Destinations/${destinationId}`,
        token,
        method: "delete",
    });
};

const setTriggerAsync$4 = async ({ campaignApiName, token, id, trigger, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
        token,
        method: "post",
        body: trigger,
    });
};
const fetchTriggerAsync$4 = async ({ campaignApiName, token, id }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/${campaignApiName}/${id}/TriggerCollection`,
    });
};

const fetchLearningFeaturesAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    });
};
const setLearningFeaturesAsync$4 = async ({ campaignApiName, token, id, useInternalId, featureIds, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { featureIds, useInternalId },
    });
};

const fetchLearningMetricsAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
    });
};
const setLearningMetricsAsync$4 = async ({ campaignApiName, token, id, useInternalId, metricIds, }) => {
    return await executeFetch({
        path: `api/campaigns/${campaignApiName}/${id}/LearningFeatures`,
        token,
        method: "post",
        query: { useInternalId },
        body: { metricIds, useInternalId },
    });
};

const fetchReportImageBlobUrlAsync$4 = async ({ campaignApiName, token, id, useInternalId, }) => {
    console.debug("fetching image for recommender");
    console.debug(`api/campaigns/${campaignApiName}/${id}/ReportImage`);
    const axios = current();
    let response;
    try {
        response = await axios.get(`api/campaigns/${campaignApiName}/${id}/ReportImage`, {
            headers: headers(token, null),
            responseType: "blob", // required for axios to understand response
        });
    }
    catch (ex) {
        console.error(ex);
        throw ex;
    }
    if (response.status >= 200 && response.status < 300) {
        return URL.createObjectURL(response.data);
    }
    else {
        handleErrorResponse(response);
    }
};

const campaignApiName$1 = "ParameterSetCampaigns";
const fetchParameterSetCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        page,
    });
};
const fetchParameterSetCampaignAsync = async ({ token, id, searchTerm, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        query: {
            "q.term": searchTerm,
        },
    });
};
const createParameterSetCampaignAsync = async ({ token, payload, }) => {
    return await executeFetch({
        path: "api/campaigns/ParameterSetCampaigns",
        token,
        method: "post",
        body: payload,
    });
};
const deleteParameterSetCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}`,
        token,
        method: "delete",
    });
};
const fetchParameterSetRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/recommendations`,
        token,
        page,
        pageSize,
    });
};
const createLinkRegisteredModelAsync$3 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync({
        campaignApiName: campaignApiName$1,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$3 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const invokeParameterSetCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$3 = async ({ id, token, page, pageSize, }) => {
    return await fetchCampaignInvokationLogsAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$3 = async ({ id, token, name }) => {
    return await fetchCampaignTargetVariableValuesAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$3 = async ({ id, token, targetVariableValue, }) => {
    return await createCampaignTargetVariableValueAsync({
        campaignApiName: campaignApiName$1,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$3 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$2 = async ({ id, token }) => {
    return await fetchArgumentsAsync$3({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const setArgumentsAsync$3 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$3 = async ({ id, token }) => {
    return await fetchDestinationsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const createDestinationAsync$3 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$3 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$3 = async ({ id, token }) => {
    return await fetchTriggerAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
    });
};
const setTriggerAsync$3 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$3 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$3 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$3 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/ParameterSetCampaigns/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$3 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$4({
        campaignApiName: campaignApiName$1,
        id,
        token,
        useInternalId,
    });
};

var parameterSetCampaignsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchParameterSetCampaignsAsync: fetchParameterSetCampaignsAsync,
  fetchParameterSetCampaignAsync: fetchParameterSetCampaignAsync,
  createParameterSetCampaignAsync: createParameterSetCampaignAsync,
  deleteParameterSetCampaignAsync: deleteParameterSetCampaignAsync,
  fetchParameterSetRecommendationsAsync: fetchParameterSetRecommendationsAsync,
  createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$3,
  fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$3,
  invokeParameterSetCampaignAsync: invokeParameterSetCampaignAsync,
  fetchInvokationLogsAsync: fetchInvokationLogsAsync$3,
  fetchTargetVariablesAsync: fetchTargetVariablesAsync$3,
  createTargetVariableAsync: createTargetVariableAsync$3,
  setSettingsAsync: setSettingsAsync$3,
  fetchArgumentsAsync: fetchArgumentsAsync$2,
  setArgumentsAsync: setArgumentsAsync$3,
  fetchDestinationsAsync: fetchDestinationsAsync$3,
  createDestinationAsync: createDestinationAsync$3,
  removeDestinationAsync: removeDestinationAsync$3,
  fetchTriggerAsync: fetchTriggerAsync$3,
  setTriggerAsync: setTriggerAsync$3,
  fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$3,
  setLearningFeaturesAsync: setLearningFeaturesAsync$3,
  fetchLearningMetricsAsync: fetchLearningMetricsAsync$3,
  setLearningMetricsAsync: setLearningMetricsAsync$3,
  fetchStatisticsAsync: fetchStatisticsAsync$3,
  fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$3
});

const setMetadataAsync = async ({ token, metadata }) => {
    return await executeFetch({
        path: "api/profile/metadata",
        token,
        method: "post",
        body: metadata,
    });
};
const getMetadataAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/profile/metadata",
        token,
    });
};

var profileApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  setMetadataAsync: setMetadataAsync,
  getMetadataAsync: getMetadataAsync
});

const recommenderApiName$1 = "ItemsRecommenders";
console.warn("Deprecation Notice: Items Recommenders are replaced by Promotions Recommenders.");
const fetchItemsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/ItemsRecommenders",
        page,
    });
};
const fetchItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
    });
};
const fetchItemsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/ItemsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deleteItemsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const createItemsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/ItemsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchItemsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
    });
};
const addItemAsync = async ({ token, id, item }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items`,
        token,
        method: "post",
        body: item,
    });
};
const removeItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Items/${itemId}`,
        token,
        method: "post",
    });
};
const setBaselineItemAsync = async ({ token, id, itemId, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
        method: "post",
        body: { itemId },
    });
};
const setDefaultItemAsync = setBaselineItemAsync; // backwards compat
const getBaselineItemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/BaselineItem`,
        token,
    });
};
const getDefaultItemAsync = getBaselineItemAsync; // backwards compat
const createLinkRegisteredModelAsync$2 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName: recommenderApiName$1,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$2 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const invokeItemsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$2 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$2 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$2 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$2 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        settings,
    });
};
const setArgumentsAsync$2 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        args,
    });
};
const fetchDestinationsAsync$2 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const createDestinationAsync$2 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$2 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$2 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
    });
};
const setTriggerAsync$2 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$2 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$2 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$2 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/ItemsRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$2 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName: recommenderApiName$1,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync$2 = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/ItemsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};

var itemsRecommendersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchItemsRecommendersAsync: fetchItemsRecommendersAsync,
  fetchItemsRecommenderAsync: fetchItemsRecommenderAsync,
  fetchItemsRecommendationsAsync: fetchItemsRecommendationsAsync,
  deleteItemsRecommenderAsync: deleteItemsRecommenderAsync,
  createItemsRecommenderAsync: createItemsRecommenderAsync,
  fetchItemsAsync: fetchItemsAsync$1,
  addItemAsync: addItemAsync,
  removeItemAsync: removeItemAsync,
  setBaselineItemAsync: setBaselineItemAsync,
  setDefaultItemAsync: setDefaultItemAsync,
  getBaselineItemAsync: getBaselineItemAsync,
  getDefaultItemAsync: getDefaultItemAsync,
  createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$2,
  fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$2,
  invokeItemsRecommenderAsync: invokeItemsRecommenderAsync,
  fetchInvokationLogsAsync: fetchInvokationLogsAsync$2,
  fetchTargetVariablesAsync: fetchTargetVariablesAsync$2,
  createTargetVariableAsync: createTargetVariableAsync$2,
  setSettingsAsync: setSettingsAsync$2,
  setArgumentsAsync: setArgumentsAsync$2,
  fetchDestinationsAsync: fetchDestinationsAsync$2,
  createDestinationAsync: createDestinationAsync$2,
  removeDestinationAsync: removeDestinationAsync$2,
  fetchTriggerAsync: fetchTriggerAsync$2,
  setTriggerAsync: setTriggerAsync$2,
  fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$2,
  setLearningFeaturesAsync: setLearningFeaturesAsync$2,
  fetchLearningMetricsAsync: fetchLearningMetricsAsync$2,
  setLearningMetricsAsync: setLearningMetricsAsync$2,
  fetchStatisticsAsync: fetchStatisticsAsync$2,
  fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$2,
  fetchPerformanceAsync: fetchPerformanceAsync$2
});

const recommenderApiName = "PromotionsRecommenders";
console.warn("Deprecation Notice: Promotions Recommenders are replaced by Promotions Campaigns.");
const fetchPromotionsRecommendersAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/recommenders/PromotionsRecommenders",
        page,
    });
};
const fetchPromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
    });
};
const fetchPromotionsRecommendationsAsync$1 = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deletePromotionsRecommenderAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}`,
        token,
        method: "delete",
    });
};
const createPromotionsRecommenderAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/recommenders/PromotionsRecommenders",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchPromotionsAsync$2 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
    });
};
const fetchAudienceAsync$1 = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Audience`,
        token,
    });
};
const addPromotionAsync$1 = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
const removePromotionAsync$1 = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Promotions/${promotionId}`,
        token,
        method: "post",
    });
};
const setBaselinePromotionAsync$1 = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
const getBaselinePromotionAsync$1 = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/BaselinePromotion`,
        token,
    });
};
const createLinkRegisteredModelAsync$1 = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync$1({
        recommenderApiName,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync$1 = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const invokePromotionsRecommenderAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync$1 = async ({ id, token, page, pageSize, }) => {
    return await fetchRecommenderInvokationLogsAsync({
        recommenderApiName,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync$1 = async ({ id, token, name }) => {
    return await fetchRecommenderTargetVariableValuesAsync({
        recommenderApiName,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync$1 = async ({ id, token, targetVariableValue, }) => {
    return await createRecommenderTargetVariableValueAsync({
        recommenderApiName,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync$1 = async ({ id, token, settings, }) => {
    return await setSettingsAsync$6({
        recommenderApiName,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync$1 = async ({ id, token }) => {
    return await fetchArgumentsAsync$5({
        recommenderApiName,
        id,
        token,
    });
};
const setArgumentsAsync$1 = async ({ id, token, args, }) => {
    return await setArgumentsAsync$6({
        recommenderApiName,
        id,
        token,
        args,
    });
};
const createChoosePromotionArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, }) => {
    return await createChoosePromotionArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChoosePromotionArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChoosePromotionArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChoosePromotionArgumentRulesAsync$1 = async ({ id, useInternalId, token, }) => {
    return await fetchChoosePromotionArgumentRulesAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const createChooseSegmentArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, }) => {
    return await createChooseSegmentArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChooseSegmentArgumentRuleAsync$1 = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChooseSegmentArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChooseSegmentArgumentRulesAsync$1 = async ({ id, useInternalId, token, }) => {
    return await fetchChooseSegmentArgumentRulesAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const deleteArgumentRuleAsync$1 = async ({ id, useInternalId, token, ruleId, }) => {
    return await deleteArgumentRuleAsync$3({
        recommenderApiName,
        id,
        token,
        useInternalId,
        ruleId,
    });
};
const fetchDestinationsAsync$1 = async ({ id, token }) => {
    return await fetchDestinationsAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const createDestinationAsync$1 = async ({ id, token, destination, }) => {
    return await createDestinationAsync$6({
        recommenderApiName,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync$1 = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$6({
        recommenderApiName,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync$1 = async ({ id, token }) => {
    return await fetchTriggerAsync$6({
        recommenderApiName,
        id,
        token,
    });
};
const setTriggerAsync$1 = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$6({
        recommenderApiName,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync$1 = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync$1 = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync$1 = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync$1 = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$6({
        recommenderApiName,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync$1 = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
const fetchPromotionOptimiserAsync$1 = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/`,
    });
};
const setAllPromotionOptimiserWeightsAsync$1 = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
const setPromotionOptimiserWeightAsync$1 = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
const setUseOptimiserAsync$1 = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/recommenders/PromotionsRecommenders/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
const fetchRecommenderChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
    });
};
const addRecommenderChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
const removeRecommenderChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/recommenders/PromotionsRecommenders/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
const fetchPromotionsRecommendationAsync$1 = async ({ token, recommendationId, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/Recommendations/${recommendationId}`,
    });
};
const fetchOffersAsync$1 = async ({ token, page, pageSize, id, offerState, }) => {
    return await executeFetch({
        token,
        path: `api/recommenders/PromotionsRecommenders/${id}/Offers`,
        page,
        pageSize,
        query: { offerState },
    });
};

var promotionsRecommendersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchPromotionsRecommendersAsync: fetchPromotionsRecommendersAsync,
  fetchPromotionsRecommenderAsync: fetchPromotionsRecommenderAsync,
  fetchPromotionsRecommendationsAsync: fetchPromotionsRecommendationsAsync$1,
  deletePromotionsRecommenderAsync: deletePromotionsRecommenderAsync,
  createPromotionsRecommenderAsync: createPromotionsRecommenderAsync,
  fetchPromotionsAsync: fetchPromotionsAsync$2,
  fetchAudienceAsync: fetchAudienceAsync$1,
  addPromotionAsync: addPromotionAsync$1,
  removePromotionAsync: removePromotionAsync$1,
  setBaselinePromotionAsync: setBaselinePromotionAsync$1,
  getBaselinePromotionAsync: getBaselinePromotionAsync$1,
  createLinkRegisteredModelAsync: createLinkRegisteredModelAsync$1,
  fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync$1,
  invokePromotionsRecommenderAsync: invokePromotionsRecommenderAsync,
  fetchInvokationLogsAsync: fetchInvokationLogsAsync$1,
  fetchTargetVariablesAsync: fetchTargetVariablesAsync$1,
  createTargetVariableAsync: createTargetVariableAsync$1,
  setSettingsAsync: setSettingsAsync$1,
  fetchArgumentsAsync: fetchArgumentsAsync$1,
  setArgumentsAsync: setArgumentsAsync$1,
  createChoosePromotionArgumentRuleAsync: createChoosePromotionArgumentRuleAsync$1,
  updateChoosePromotionArgumentRuleAsync: updateChoosePromotionArgumentRuleAsync$1,
  fetchChoosePromotionArgumentRulesAsync: fetchChoosePromotionArgumentRulesAsync$1,
  createChooseSegmentArgumentRuleAsync: createChooseSegmentArgumentRuleAsync$1,
  updateChooseSegmentArgumentRuleAsync: updateChooseSegmentArgumentRuleAsync$1,
  fetchChooseSegmentArgumentRulesAsync: fetchChooseSegmentArgumentRulesAsync$1,
  deleteArgumentRuleAsync: deleteArgumentRuleAsync$1,
  fetchDestinationsAsync: fetchDestinationsAsync$1,
  createDestinationAsync: createDestinationAsync$1,
  removeDestinationAsync: removeDestinationAsync$1,
  fetchTriggerAsync: fetchTriggerAsync$1,
  setTriggerAsync: setTriggerAsync$1,
  fetchLearningFeaturesAsync: fetchLearningFeaturesAsync$1,
  setLearningFeaturesAsync: setLearningFeaturesAsync$1,
  fetchLearningMetricsAsync: fetchLearningMetricsAsync$1,
  setLearningMetricsAsync: setLearningMetricsAsync$1,
  fetchStatisticsAsync: fetchStatisticsAsync$1,
  fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync$1,
  fetchPerformanceAsync: fetchPerformanceAsync$1,
  fetchPromotionOptimiserAsync: fetchPromotionOptimiserAsync$1,
  setAllPromotionOptimiserWeightsAsync: setAllPromotionOptimiserWeightsAsync$1,
  setPromotionOptimiserWeightAsync: setPromotionOptimiserWeightAsync$1,
  setUseOptimiserAsync: setUseOptimiserAsync$1,
  fetchRecommenderChannelsAsync: fetchRecommenderChannelsAsync,
  addRecommenderChannelAsync: addRecommenderChannelAsync,
  removeRecommenderChannelAsync: removeRecommenderChannelAsync,
  fetchPromotionsRecommendationAsync: fetchPromotionsRecommendationAsync$1,
  fetchOffersAsync: fetchOffersAsync$1
});

const campaignApiName = "PromotionsCampaigns";
const fetchPromotionsCampaignsAsync = async ({ token, page, }) => {
    return await executeFetch({
        token,
        path: "api/campaigns/PromotionsCampaigns",
        page,
    });
};
const fetchPromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
    });
};
const fetchPromotionsRecommendationsAsync = async ({ token, page, pageSize, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Recommendations`,
        page,
        pageSize,
    });
};
const deletePromotionsCampaignAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}`,
        token,
        method: "delete",
    });
};
const createPromotionsCampaignAsync = async ({ token, payload, useInternalId, }) => {
    return await executeFetch({
        path: "api/campaigns/PromotionsCampaigns",
        token,
        method: "post",
        body: payload,
        query: { useInternalId },
    });
};
const fetchPromotionsAsync$1 = async ({ token, id }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
    });
};
const fetchAudienceAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Audience`,
        token,
    });
};
const addPromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions`,
        token,
        method: "post",
        body: promotion,
    });
};
const removePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Promotions/${promotionId}`,
        token,
        method: "delete",
    });
};
const setBaselinePromotionAsync = async ({ token, id, promotionId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
        method: "post",
        body: { promotionId },
    });
};
const getBaselinePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/BaselinePromotion`,
        token,
    });
};
const createLinkRegisteredModelAsync = async ({ token, id, modelId, }) => {
    return await createLinkedRegisteredModelAsync({
        campaignApiName,
        id,
        modelId,
        token,
    });
};
const fetchLinkedRegisteredModelAsync = async ({ token, id, }) => {
    return await fetchLinkedRegisteredModelAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const invokePromotionsCampaignAsync = async ({ token, id, input, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Invoke`,
        token,
        method: "post",
        body: input,
    });
};
const fetchInvokationLogsAsync = async ({ id, token, page, pageSize, }) => {
    return await fetchCampaignInvokationLogsAsync({
        campaignApiName,
        id,
        token,
        page,
        pageSize,
    });
};
const fetchTargetVariablesAsync = async ({ id, token, name }) => {
    return await fetchCampaignTargetVariableValuesAsync({
        campaignApiName,
        id,
        token,
        name,
    });
};
const createTargetVariableAsync = async ({ id, token, targetVariableValue, }) => {
    return await createCampaignTargetVariableValueAsync({
        campaignApiName,
        id,
        token,
        targetVariableValue,
    });
};
const setSettingsAsync = async ({ id, token, settings, }) => {
    return await setSettingsAsync$4({
        campaignApiName,
        id,
        token,
        settings,
    });
};
const fetchArgumentsAsync = async ({ id, token }) => {
    return await fetchArgumentsAsync$3({
        campaignApiName,
        id,
        token,
    });
};
const setArgumentsAsync = async ({ id, token, args, }) => {
    return await setArgumentsAsync$4({
        campaignApiName,
        id,
        token,
        args,
    });
};
const createChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await createChoosePromotionArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChoosePromotionArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChoosePromotionArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChoosePromotionArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await fetchChoosePromotionArgumentRulesAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const createChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, }) => {
    return await createChooseSegmentArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
    });
};
const updateChooseSegmentArgumentRuleAsync = async ({ id, useInternalId, token, rule, ruleId, }) => {
    return await updateChooseSegmentArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        rule,
        ruleId,
    });
};
const fetchChooseSegmentArgumentRulesAsync = async ({ id, useInternalId, token, }) => {
    return await fetchChooseSegmentArgumentRulesAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const deleteArgumentRuleAsync = async ({ id, useInternalId, token, ruleId, }) => {
    return await deleteArgumentRuleAsync$2({
        campaignApiName,
        id,
        token,
        useInternalId,
        ruleId,
    });
};
const fetchDestinationsAsync = async ({ id, token }) => {
    return await fetchDestinationsAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const createDestinationAsync = async ({ id, token, destination, }) => {
    return await createDestinationAsync$4({
        campaignApiName,
        id,
        token,
        destination,
    });
};
const removeDestinationAsync = async ({ id, token, destinationId, }) => {
    return await removeDestinationAsync$4({
        campaignApiName,
        id,
        token,
        destinationId,
    });
};
const fetchTriggerAsync = async ({ id, token }) => {
    return await fetchTriggerAsync$4({
        campaignApiName,
        id,
        token,
    });
};
const setTriggerAsync = async ({ id, token, trigger, }) => {
    return await setTriggerAsync$4({
        campaignApiName,
        id,
        token,
        trigger,
    });
};
const fetchLearningFeaturesAsync = async ({ id, token, useInternalId, }) => {
    return await fetchLearningFeaturesAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningFeaturesAsync = async ({ id, token, featureIds, useInternalId, }) => {
    return await setLearningFeaturesAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
        featureIds,
    });
};
const fetchLearningMetricsAsync = async ({ id, token, useInternalId, }) => {
    return await fetchLearningMetricsAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const setLearningMetricsAsync = async ({ id, token, metricIds, useInternalId, }) => {
    return await setLearningMetricsAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
        metricIds,
    });
};
const fetchStatisticsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Statistics`,
        token,
    });
};
const fetchReportImageBlobUrlAsync = async ({ id, token, useInternalId, }) => {
    return await fetchReportImageBlobUrlAsync$4({
        campaignApiName,
        id,
        token,
        useInternalId,
    });
};
const fetchPerformanceAsync = async ({ token, id, reportId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Performance/${reportId !== null && reportId !== void 0 ? reportId : "latest"}`,
    });
};
const fetchPromotionOptimiserAsync = async ({ token, useInternalId, id, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/`,
    });
};
const setAllPromotionOptimiserWeightsAsync = async ({ token, useInternalId, id, weights, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/`,
        method: "post",
        body: weights,
    });
};
const setPromotionOptimiserWeightAsync = async ({ token, useInternalId, id, weightId, weight, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/Optimiser/Weights/${weightId}`,
        method: "post",
        body: { weight },
    });
};
const setUseOptimiserAsync = async ({ token, useInternalId, id, useOptimiser, }) => {
    return await executeFetch({
        token,
        query: { useInternalId },
        path: `api/campaigns/PromotionsCampaigns/${id}/UseOptimiser`,
        method: "post",
        body: { useOptimiser },
    });
};
const fetchCampaignChannelsAsync = async ({ id, token, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
    });
};
const addCampaignChannelAsync = async ({ token, id, channel, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels`,
        token,
        method: "post",
        body: channel,
    });
};
const removeCampaignChannelAsync = async ({ id, token, channelId, }) => {
    return await executeFetch({
        path: `api/campaigns/PromotionsCampaigns/${id}/Channels/${channelId}`,
        token,
        method: "delete",
    });
};
const fetchPromotionsRecommendationAsync = async ({ token, recommendationId, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/Recommendations/${recommendationId}`,
    });
};
const fetchOffersAsync = async ({ token, page, pageSize, id, offerState, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/Offers`,
        page,
        pageSize,
        query: { offerState },
    });
};
const fetchARPOReportAsync = async ({ token, id, }) => {
    return await executeFetch({
        token,
        path: `api/campaigns/PromotionsCampaigns/${id}/ARPOReport`,
    });
};

var promotionsCampaignsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchPromotionsCampaignsAsync: fetchPromotionsCampaignsAsync,
  fetchPromotionsCampaignAsync: fetchPromotionsCampaignAsync,
  fetchPromotionsRecommendationsAsync: fetchPromotionsRecommendationsAsync,
  deletePromotionsCampaignAsync: deletePromotionsCampaignAsync,
  createPromotionsCampaignAsync: createPromotionsCampaignAsync,
  fetchPromotionsAsync: fetchPromotionsAsync$1,
  fetchAudienceAsync: fetchAudienceAsync,
  addPromotionAsync: addPromotionAsync,
  removePromotionAsync: removePromotionAsync,
  setBaselinePromotionAsync: setBaselinePromotionAsync,
  getBaselinePromotionAsync: getBaselinePromotionAsync,
  createLinkRegisteredModelAsync: createLinkRegisteredModelAsync,
  fetchLinkedRegisteredModelAsync: fetchLinkedRegisteredModelAsync,
  invokePromotionsCampaignAsync: invokePromotionsCampaignAsync,
  fetchInvokationLogsAsync: fetchInvokationLogsAsync,
  fetchTargetVariablesAsync: fetchTargetVariablesAsync,
  createTargetVariableAsync: createTargetVariableAsync,
  setSettingsAsync: setSettingsAsync,
  fetchArgumentsAsync: fetchArgumentsAsync,
  setArgumentsAsync: setArgumentsAsync,
  createChoosePromotionArgumentRuleAsync: createChoosePromotionArgumentRuleAsync,
  updateChoosePromotionArgumentRuleAsync: updateChoosePromotionArgumentRuleAsync,
  fetchChoosePromotionArgumentRulesAsync: fetchChoosePromotionArgumentRulesAsync,
  createChooseSegmentArgumentRuleAsync: createChooseSegmentArgumentRuleAsync,
  updateChooseSegmentArgumentRuleAsync: updateChooseSegmentArgumentRuleAsync,
  fetchChooseSegmentArgumentRulesAsync: fetchChooseSegmentArgumentRulesAsync,
  deleteArgumentRuleAsync: deleteArgumentRuleAsync,
  fetchDestinationsAsync: fetchDestinationsAsync,
  createDestinationAsync: createDestinationAsync,
  removeDestinationAsync: removeDestinationAsync,
  fetchTriggerAsync: fetchTriggerAsync,
  setTriggerAsync: setTriggerAsync,
  fetchLearningFeaturesAsync: fetchLearningFeaturesAsync,
  setLearningFeaturesAsync: setLearningFeaturesAsync,
  fetchLearningMetricsAsync: fetchLearningMetricsAsync,
  setLearningMetricsAsync: setLearningMetricsAsync,
  fetchStatisticsAsync: fetchStatisticsAsync,
  fetchReportImageBlobUrlAsync: fetchReportImageBlobUrlAsync,
  fetchPerformanceAsync: fetchPerformanceAsync,
  fetchPromotionOptimiserAsync: fetchPromotionOptimiserAsync,
  setAllPromotionOptimiserWeightsAsync: setAllPromotionOptimiserWeightsAsync,
  setPromotionOptimiserWeightAsync: setPromotionOptimiserWeightAsync,
  setUseOptimiserAsync: setUseOptimiserAsync,
  fetchCampaignChannelsAsync: fetchCampaignChannelsAsync,
  addCampaignChannelAsync: addCampaignChannelAsync,
  removeCampaignChannelAsync: removeCampaignChannelAsync,
  fetchPromotionsRecommendationAsync: fetchPromotionsRecommendationAsync,
  fetchOffersAsync: fetchOffersAsync,
  fetchARPOReportAsync: fetchARPOReportAsync
});

let authConfig = undefined; // caches this because it rarely change
const fetchAuth0ConfigurationAsync = async () => {
    if (!authConfig) {
        const result = await executeFetch({
            path: "api/reactConfig/auth0",
        });
        authConfig = result;
    }
    return authConfig;
};
let config = undefined;
const fetchConfigurationAsync = async () => {
    if (!config) {
        const result = await executeFetch({
            token: "",
            path: "api/reactConfig",
        });
        config = result;
    }
    return config;
};
const fetchHostingAsync$1 = async () => {
    return await executeFetch({
        path: "api/reactConfig/hosting",
        method: "get",
    });
};

var reactConfigApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchAuth0ConfigurationAsync: fetchAuth0ConfigurationAsync,
  fetchConfigurationAsync: fetchConfigurationAsync,
  fetchHostingAsync: fetchHostingAsync$1
});

const getPropertiesAsync$2 = async ({ api, token, id }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
    });
};
const setPropertiesAsync$2 = async ({ api, token, id, properties }) => {
    return await executeFetch({
        path: `api/${api}/${id}/Properties`,
        token,
        method: "post",
        body: properties,
    });
};

console.warn("Deprecation Notice: Recommendable Items are replaced by Promotions.");
const fetchItemsAsync = async ({ token, page, searchTerm, }) => {
    return await executeFetch({
        path: "api/RecommendableItems",
        token,
        page,
        query: {
            "q.term": searchTerm,
        },
    });
};
const fetchItemAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
    });
};
const createItemAsync = async ({ token, item, }) => {
    return await executeFetch({
        path: "api/RecommendableItems",
        token,
        method: "post",
        body: item,
    });
};
const updateItemAsync = async ({ token, id, item, }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
        method: "post",
        body: item,
    });
};
const deleteItemAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/RecommendableItems/${id}`,
        token,
        method: "delete",
    });
};
const getPropertiesAsync$1 = async ({ token, id }) => {
    return await getPropertiesAsync$2({
        token,
        id,
        api: "RecommendableItems",
    });
};
const setPropertiesAsync$1 = async ({ token, id, properties, }) => {
    return await setPropertiesAsync$2({
        token,
        id,
        properties,
        api: "RecommendableItems",
    });
};

var recommendableItemsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchItemsAsync: fetchItemsAsync,
  fetchItemAsync: fetchItemAsync,
  createItemAsync: createItemAsync,
  updateItemAsync: updateItemAsync,
  deleteItemAsync: deleteItemAsync,
  getPropertiesAsync: getPropertiesAsync$1,
  setPropertiesAsync: setPropertiesAsync$1
});

const fetchPromotionsAsync = async ({ token, page, searchTerm, promotionType, benefitType, weeksAgo, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.weeksAgo": weeksAgo,
            promotionType: promotionType,
            benefitType: benefitType,
        },
    });
};
const fetchPromotionAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
    });
};
const createPromotionAsync = async ({ token, promotion, }) => {
    return await executeFetch({
        path: "api/Promotions",
        token,
        method: "post",
        body: promotion,
    });
};
const updatePromotionAsync = async ({ token, id, promotion, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "post",
        body: promotion,
    });
};
const deletePromotionAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Promotions/${id}`,
        token,
        method: "delete",
    });
};
const getPropertiesAsync = async ({ token, id }) => {
    return await getPropertiesAsync$2({
        token,
        id,
        api: "Promotions",
    });
};
const setPropertiesAsync = async ({ token, id, properties, }) => {
    return await setPropertiesAsync$2({
        token,
        id,
        properties,
        api: "Promotions",
    });
};

var promotionsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchPromotionsAsync: fetchPromotionsAsync,
  fetchPromotionAsync: fetchPromotionAsync,
  createPromotionAsync: createPromotionAsync,
  updatePromotionAsync: updatePromotionAsync,
  deletePromotionAsync: deletePromotionAsync,
  getPropertiesAsync: getPropertiesAsync,
  setPropertiesAsync: setPropertiesAsync
});

const fetchReportsAsync = async ({ token }) => {
    return await executeFetch({
        path: "api/Reports",
        token,
    });
};
const downloadReportAsync = async ({ token, reportName }) => {
    return await executeFetch({
        path: "api/reports/download",
        token,
        query: {
            report: reportName,
        },
    });
};

var reportsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchReportsAsync: fetchReportsAsync,
  downloadReportAsync: downloadReportAsync
});

const fetchSegmentsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        page,
    });
};
const fetchSegmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/segments/${id}`,
        token,
    });
};
const createSegmentAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Segments",
        token,
        method: "post",
        body: payload,
    });
};
const deleteSegmentAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Segments/${id}`,
        token,
        method: "delete",
    });
};
const addCustomerAsync = async ({ token, id, customerId }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers/${customerId}`,
        token,
        method: "post",
    });
};
const removeCustomerAsync = async ({ token, id, customerId }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers/${customerId}`,
        token,
        method: "delete",
    });
};
const fetchSegmentCustomersAsync = async ({ token, page, id, searchTerm, weeksAgo, }) => {
    return await executeFetch({
        path: `api/Segments/${id}/Customers`,
        token,
        page,
        query: {
            "q.term": searchTerm,
            "q.weeksAgo": weeksAgo,
        },
    });
};
const fetchSegmentEnrolmentRulesAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules`,
        token,
    });
};
const addSegmentEnrolmentRuleAsync = async ({ token, id, payload }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules`,
        token,
        method: "post",
        body: payload,
    });
};
const removeSegmentEnrolmentRuleAsync = async ({ token, id, ruleId, }) => {
    return await executeFetch({
        path: `api/Segments/${id}/MetricEnrolmentRules/${ruleId}`,
        token,
        method: "delete",
    });
};

var segmentsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchSegmentsAsync: fetchSegmentsAsync,
  fetchSegmentAsync: fetchSegmentAsync,
  createSegmentAsync: createSegmentAsync,
  deleteSegmentAsync: deleteSegmentAsync,
  addCustomerAsync: addCustomerAsync,
  removeCustomerAsync: removeCustomerAsync,
  fetchSegmentCustomersAsync: fetchSegmentCustomersAsync,
  fetchSegmentEnrolmentRulesAsync: fetchSegmentEnrolmentRulesAsync,
  addSegmentEnrolmentRuleAsync: addSegmentEnrolmentRuleAsync,
  removeSegmentEnrolmentRuleAsync: removeSegmentEnrolmentRuleAsync
});

const fetchTouchpointsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        page,
    });
};
const fetchTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}`,
        token,
    });
};
const createTouchpointMetadataAsync = async ({ token, payload }) => {
    return await executeFetch({
        path: "api/Touchpoints",
        token,
        method: "post",
        body: payload,
    });
};
const fetchTrackedUserTouchpointsAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints`,
        token,
    });
};
const fetchTrackedUsersInTouchpointAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/touchpoints/${id}/trackedusers`,
        token,
    });
};
const createTrackedUserTouchpointAsync = async ({ token, id, touchpointCommonId, payload, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
        method: "post",
        body: payload,
    });
};
const fetchTrackedUserTouchpointValuesAsync = async ({ token, id, touchpointCommonId, }) => {
    return await executeFetch({
        path: `api/trackedusers/${id}/touchpoints/${touchpointCommonId}`,
        token,
    });
};

var touchpointsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchTouchpointsAsync: fetchTouchpointsAsync,
  fetchTouchpointAsync: fetchTouchpointAsync,
  createTouchpointMetadataAsync: createTouchpointMetadataAsync,
  fetchTrackedUserTouchpointsAsync: fetchTrackedUserTouchpointsAsync,
  fetchTrackedUsersInTouchpointAsync: fetchTrackedUsersInTouchpointAsync,
  createTrackedUserTouchpointAsync: createTrackedUserTouchpointAsync,
  fetchTrackedUserTouchpointValuesAsync: fetchTrackedUserTouchpointValuesAsync
});

// backwards compatible shim TrackedUser => Customer
const fetchTrackedUsersAsync = fetchCustomersAsync;
const updateMergePropertiesAsync = updateMergePropertiesAsync$1;
const fetchTrackedUserAsync = fetchCustomerAsync;
const fetchUniqueTrackedUserActionGroupsAsync = fetchUniqueCustomerActionGroupsAsync;
const fetchLatestRecommendationsAsync = fetchLatestRecommendationsAsync$1;
const fetchTrackedUserActionAsync = fetchCustomerActionAsync;
const uploadUserDataAsync = uploadUserDataAsync$1;
const createOrUpdateTrackedUserAsync = createOrUpdateCustomerAsync;
const fetchTrackedUsersActionsAsync = fetchCustomersActionsAsync;
const deleteCustomerAsync = deleteCustomerAsync$1;

var trackedUsersApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchTrackedUsersAsync: fetchTrackedUsersAsync,
  updateMergePropertiesAsync: updateMergePropertiesAsync,
  fetchTrackedUserAsync: fetchTrackedUserAsync,
  fetchUniqueTrackedUserActionGroupsAsync: fetchUniqueTrackedUserActionGroupsAsync,
  fetchLatestRecommendationsAsync: fetchLatestRecommendationsAsync,
  fetchTrackedUserActionAsync: fetchTrackedUserActionAsync,
  uploadUserDataAsync: uploadUserDataAsync,
  createOrUpdateTrackedUserAsync: createOrUpdateTrackedUserAsync,
  fetchTrackedUsersActionsAsync: fetchTrackedUsersActionsAsync,
  deleteCustomerAsync: deleteCustomerAsync
});

const fetchCurrentTenantAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current",
        token,
        method: "get",
    });
};
const fetchAccountAsync = async ({ token, id, }) => {
    return await executeFetch({
        path: `api/Tenants/${id}/Account`,
        token,
        method: "get",
    });
};
const fetchHostingAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/Tenants/Hosting",
        token,
        method: "get",
    });
};
const fetchCurrentTenantMembershipsAsync = async ({ token, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "get",
    });
};
const createTenantMembershipAsync = async ({ token, email, }) => {
    return await executeFetch({
        path: "api/tenants/current/memberships",
        token,
        method: "post",
        body: { email },
    });
};

var tenantsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchCurrentTenantAsync: fetchCurrentTenantAsync,
  fetchAccountAsync: fetchAccountAsync,
  fetchHostingAsync: fetchHostingAsync,
  fetchCurrentTenantMembershipsAsync: fetchCurrentTenantMembershipsAsync,
  createTenantMembershipAsync: createTenantMembershipAsync
});

const fetchRewardSelectorsAsync = async ({ token, page }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        page,
    });
};
const fetchRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        page,
    });
};
const deleteRewardSelectorAsync = async ({ token, id }) => {
    return await executeFetch({
        path: `api/RewardSelectors/${id}`,
        token,
        method: "delete",
    });
};
const createRewardSelectorAsync = async ({ token, entity }) => {
    return await executeFetch({
        path: "api/RewardSelectors",
        token,
        method: "post",
        body: entity,
    });
};

var rewardSelectorsApi = /*#__PURE__*/Object.freeze({
  __proto__: null,
  fetchRewardSelectorsAsync: fetchRewardSelectorsAsync,
  fetchRewardSelectorAsync: fetchRewardSelectorAsync,
  deleteRewardSelectorAsync: deleteRewardSelectorAsync,
  createRewardSelectorAsync: createRewardSelectorAsync
});

export { activityFeedApi as activityFeed, apiKeyApi as apiKeys, axiosInstance, businessesApi as businesses, channelsApi as channels, customersApi as customers, dataSummaryApi as dataSummary, deploymentApi as deployment, environmentsApi as environments, errorHandling, eventsApi as events, featureGeneratorsApi as featureGenerators, featuresApi as features, integratedSystemsApi as integratedSystems, itemsRecommendersApi as itemsRecommenders, metricGeneratorsApi as metricGenerators, metricsApi as metrics, modelRegistrationsApi as modelRegistrations, index as models, parameterSetCampaignsApi as parameterSetCampaigns, parameterSetRecommendersApi as parameterSetRecommenders, parametersApi as parameters, profileApi as profile, promotionsApi as promotions, promotionsCampaignsApi as promotionsCampaigns, promotionsRecommendersApi as promotionsRecommenders, reactConfigApi as reactConfig, recommendableItemsApi as recommendableItems, reportsApi as reports, rewardSelectorsApi as rewardSelectors, segmentsApi as segments, setBaseUrl, setDefaultApiKey, setDefaultEnvironmentId$1 as setDefaultEnvironmentId, setTenant, tenantsApi as tenants, touchpointsApi as touchpoints, trackedUsersApi as trackedUsers };
