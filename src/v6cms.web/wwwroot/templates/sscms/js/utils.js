if (Object.defineProperty(Object.prototype, "getEntityValue", {
    value: function (e) {
        var t;
        for (t in this)
            if (t.toLowerCase() == e.toLowerCase())
                return this[t]
    }
}),
    window.swal && swal.mixin)
    var alert = swal.mixin({
        confirmButtonClass: "el-button el-button--primary",
        cancelButtonClass: "el-button el-button--default",
        buttonsStyling: !1
    });
var PER_PAGE = 30
    , DEFAULT_AVATAR_URL = "/sitefiles/assets/images/default_avatar.png"
    , $token = sessionStorage.getItem(ACCESS_TOKEN_NAME) || localStorage.getItem(ACCESS_TOKEN_NAME)
    , $api = axios.create({
        baseURL: $apiUrl,
        headers: {
            Authorization: "Bearer " + $token
        }
    })
    , utils = {
        init: function (e) {
            return _.assign({
                pageLoad: !1,
                loading: null
            }, e)
        },
        getQueryString: function (e, t) {
            var n = location.search.match(new RegExp("[?&]" + e + "=([^&]+)", "i"));
            return !n || n.length < 1 ? t || "" : decodeURIComponent(n[1])
        },
        getQueryStringList: function (e) {
            var t = utils.getQueryString(e);
            return t ? t.split(",") : []
        },
        getQueryBoolean: function (e) {
            var t = location.search.match(new RegExp("[?&]" + e + "=([^&]+)", "i"));
            return !(!t || t.length < 1 || "true" !== t[1] && "True" !== t[1])
        },
        getQueryInt: function (e, t) {
            var n = location.search.match(new RegExp("[?&]" + e + "=([^&]+)", "i"));
            return !n || n.length < 1 ? t || 0 : utils.toInt(n[1])
        },
        getQueryIntList: function (e) {
            var t = location.search.match(new RegExp("[?&]" + e + "=([^&]+)", "i"));
            return !t || t.length < 1 ? [] : _.map(t[1].split(","), function (e) {
                return utils.toInt(e)
            })
        },
        toInt: function (e) {
            return e ? "number" == typeof e ? e : parseInt(e, 10) || 0 : 0
        },
        formatDate: function (e) {
            var t = new Date(e)
                , n = "" + (t.getMonth() + 1)
                , s = "" + t.getDate()
                , a = t.getFullYear();
            return n.length < 2 && (n = "0" + n),
                s.length < 2 && (s = "0" + s),
                [a, n, s].join("-")
        },
        getQueryIntList: function (e) {
            var t = utils.getQueryString(e);
            return t ? _.map(t.split(","), function (e) {
                return parseInt(e, 10)
            }) : []
        },
        getIndexUrl: function (e) {
            var t = $rootUrl + "/";
            return e && (t += "?",
                _.forOwn(e, function (e, n) {
                    t += n + "=" + encodeURIComponent(e) + "&"
                }),
                t = t.substr(0, t.length - 1)),
                t
        },
        getRootUrl: function (e, t) {
            return utils.getPageUrl(null, e, t)
        },
        getAssetsUrl: function (e) {
            return "/sitefiles/assets/" + e
        },
        getCmsUrl: function (e, t) {
            return utils.getPageUrl("cms", e, t)
        },
        getWxUrl: function (e, t) {
            return utils.getPageUrl("wx", e, t)
        },
        getPluginsUrl: function (e, t) {
            return utils.getPageUrl("plugins", e, t)
        },
        getSettingsUrl: function (e, t) {
            return utils.getPageUrl("settings", e, t)
        },
        getCommonUrl: function (e, t) {
            return utils.getPageUrl("common", e, t)
        },
        getPageUrl: function (e, t, n) {
            var s = $rootUrl + "/";
            return s += e ? e + "/" + t + "/" : t + "/",
                n && (s += "?",
                    _.forOwn(n, function (e, t) {
                        s += t + "=" + encodeURIComponent(e) + "&"
                    }),
                    s = s.substr(0, s.length - 1)),
                s
        },
        getCountName: function (e) {
            return _.camelCase(e + "Count")
        },
        getExtendName: function (e, t) {
            return _.camelCase(t ? e + t : e)
        },
        pad: function (e) {
            for (var t = e + ""; t.length < 2;)
                t = "0" + t;
            return t
        },
        getUrl: function (e, t) {
            return t && (t.startsWith("/") || -1 != t.indexOf("://")) ? t : (e = _.trimEnd(e, "/")) + "/" + _.trimStart(_.trimStart(_.trimStart(t, "~"), "@"), "/")
        },
        getFriendlyDate: function (e) {
            "[object Date]" !== Object.prototype.toString.call(e) && (e = new Date(e));
            var t = Math.round((new Date - e) / 1e3);
            return t < 86400 ? utils.pad(e.getHours()) + ":" + utils.pad(e.getMinutes()) : t < 172800 ? "昨天 " + utils.pad(e.getHours()) + ":" + utils.pad(e.getMinutes()) : utils.pad(e.getMonth() + 1) + "月" + utils.pad(e.getDate()) + "日"
        },
        getRootVue: function () {
            return top.$vue || window.$vue
        },
        getTabVue: function (e) {
            var t = utils.getRootVue().tabs.find(function (t) {
                return t.name == e
            });
            return t ? top.document.getElementById("frm-" + t.name).contentWindow.$vue : null
        },
        getTabName: function () {
            return utils.getRootVue().tabName
        },
        openTab: function (e) {
            var t = utils.getRootVue();
            -1 !== t.tabs.findIndex(function (t) {
                return t.name == e
            }) && (t.tabName = e)
        },
        addTab: function (e, t) {
            var n = utils.getRootVue()
                , s = n.tabs.findIndex(function (e) {
                    return e.url == t
                })
                , a = null;
            -1 === s ? (a = {
                title: e,
                name: utils.uuid(),
                url: t
            },
                n.tabs.push(a)) : (a = n.tabs[s],
                    top.document.getElementById("frm-" + a.name).contentWindow.location.href = t),
                n.tabName = a.name
        },
        removeTab: function (e) {
            var t = utils.getRootVue();
            e || (e = t.tabName),
                t.tabName === e && (t.activeChildMenu = null,
                    t.tabs.forEach(function (n, s) {
                        if (n.name === e) {
                            var a = t.tabs[s + 1] || t.tabs[s - 1];
                            a && (t.tabName = a.name)
                        }
                    })),
                t.tabs = t.tabs.filter(function (t) {
                    return t.name !== e
                })
        },
        addQuery: function (e, t) {
            return e ? (e += -1 === e.indexOf("?") ? "?" : "&",
                _.forOwn(t, function (t, n) {
                    e += n + "=" + encodeURIComponent(t) + "&"
                }),
                e.substr(0, e.length - 1)) : ""
        },
        alertDelete: function (e) {
            return !!e && (alert({
                title: e.title,
                text: e.text,
                type: "warning",
                confirmButtonText: e.button || "删 除",
                confirmButtonClass: "el-button el-button--danger",
                cancelButtonClass: "el-button el-button--default",
                showCancelButton: !0,
                cancelButtonText: "取 消"
            }).then(function (t) {
                t.value && e.callback()
            }),
                !1)
        },
        alertSuccess: function (e) {
            return !!e && (alert({
                title: e.title,
                text: e.text,
                type: "success",
                confirmButtonText: e.button || "确 定",
                confirmButtonClass: "el-button el-button--primary",
                showCancelButton: !1
            }).then(function (t) {
                t.value && e.callback()
            }),
                !1)
        },
        alertWarning: function (e) {
            return !!e && (alert({
                title: e.title,
                text: e.text,
                type: "question",
                confirmButtonText: e.button || "确 认",
                confirmButtonClass: "el-button el-button--primary",
                cancelButtonClass: "el-button el-button--default",
                showCancelButton: !0,
                cancelButtonText: "取 消"
            }).then(function (t) {
                t.value && e.callback()
            }),
                !1)
        },
        getErrorMessage: function (e) {
            if (e.response && 500 === e.response.status)
                return JSON.stringify(e.response.data);
            var t = e.message;
            return e.response && e.response.data && (e.response.data.exceptionMessage ? t = e.response.data.exceptionMessage : e.response.data.message && (t = e.response.data.message)),
                t
        },
        uuid: function () {
            return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (e) {
                var t = 16 * Math.random() | 0;
                return ("x" == e ? t : 3 & t | 8).toString(16)
            })
        },
        notifySuccess: function (e) {
            utils.getRootVue().$notify.success({
                title: "成功",
                message: e
            })
        },
        notifyWarning: function (e) {
            utils.getRootVue().$notify.warning({
                title: "警告",
                message: e
            })
        },
        notifyInfo: function (e) {
            utils.getRootVue().$notify.info({
                title: "提示",
                message: e
            })
        },
        notifyError: function (e) {
            if (e) {
                var t;
                t = e.response ? utils.getErrorMessage(e) : "string" == typeof e ? e : e + "",
                    utils.getRootVue().$notify.error({
                        title: "错误",
                        message: t
                    })
            }
        },
        success: function (e) {
            utils.getRootVue().$message({
                type: "success",
                message: e,
                showIcon: !0
            })
        },
        error: function (e, t) {
            if (e)
                if (e.response) {
                    var n = utils.getErrorMessage(e);
                    if (e.response && 500 === e.response.status || t && t.redirect) {
                        var s = utils.uuid();
                        return "string" == typeof n ? sessionStorage.setItem(s, JSON.stringify({
                            message: n
                        })) : sessionStorage.setItem(s, n),
                            t && t.redirect ? void (location.href = utils.getRootUrl("error", {
                                uuid: s
                            })) : void top.utils.openLayer({
                                url: utils.getRootUrl("error", {
                                    uuid: s
                                })
                            })
                    }
                    utils.getRootVue().$message({
                        type: "error",
                        message: n,
                        showIcon: !0
                    })
                } else
                    "string" == typeof e ? t && t.redirect ? (s = utils.uuid(),
                        sessionStorage.setItem(s, JSON.stringify({
                            message: e
                        })),
                        location.href = utils.getRootUrl("error", {
                            uuid: s
                        })) : utils.getRootVue().$message({
                            type: "error",
                            message: e,
                            showIcon: !0
                        }) : "object" == typeof e && utils.getRootVue().$message({
                            type: "error",
                            message: e + "",
                            showIcon: !0
                        })
        },
        loading: function (e, t) {
            t ? e.pageLoad && (e.loading = e.$loading({
                text: "页面加载中"
            })) : e.loading ? e.loading.close() : e.pageLoad = !0
        },
        scrollTop: function () {
            document.documentElement.scrollTop = document.body.scrollTop = 0
        },
        closeLayer: function (e) {
            return e ? parent.location.reload() : parent.layer.closeAll(),
                !1
        },
        openLayer: function (e) {
            if (!e || !e.url)
                return !1;
            e.width || (e.width = $(window).width() - 50),
                e.height || (e.height = $(window).height() - 50);
            var t = layer.open({
                type: 2,
                btn: null,
                title: e.title,
                area: [e.width + "px", e.height + "px"],
                maxmin: !e.max,
                resize: !e.max,
                shadeClose: !0,
                content: e.url
            });
            return e.max && layer.full(t),
                !1
        },
        contains: function (e, t) {
            return e && t && -1 !== e.indexOf(t)
        },
        validateMobile: function (e, t, n) {
            t ? /^1[3|4|5|7|8][0-9]\d{8}$/.test(t) ? n() : n(new Error(e.message || "字段必须是有效的手机号码")) : n()
        },
        validateInt: function (e, t, n) {
            t ? /^[-]?\d+$/.test(t) ? n() : n(new Error(e.message || "字段必须是有效的数字值")) : n()
        },
        getForm: function (e, t) {
            for (var n = _.assign({}, t), s = 0; s < e.length; s++) {
                var a = e[s]
                    , r = _.lowerFirst(a.attributeName);
                "TextEditor" === a.inputType ? setTimeout(function () {
                    var e = UE.getEditor(a.attributeName, {
                        allowDivTransToP: !1,
                        maximumWords: 99999999
                    });
                    e.attributeName = a.attributeName,
                        e.ready(function () {
                            e.addListener("contentChange", function () {
                                $this.form[this.attributeName] = this.getContent()
                            })
                        })
                }, 100) : "CheckBox" !== a.inputType && "SelectMultiple" !== a.inputType || n[r] && Array.isArray(n[r]) || (n[r] = [])
            }
            return n
        },
        getRules: function (e) {
            var t = [{
                required: "字段为必填项"
            }, {
                email: "字段必须是有效的电子邮件"
            }, {
                mobile: "字段必须是有效的手机号码"
            }, {
                url: "字段必须是有效的url"
            }, {
                alpha: "字段只能包含英文字母"
            }, {
                alphaDash: "字段只能包含英文字母、数字、破折号或下划线"
            }, {
                alphaNum: "字段只能包含英文字母或数字"
            }, {
                alphaSpaces: "字段只能包含英文字母或空格"
            }, {
                creditCard: "字段必须是有效的信用卡"
            }, {
                between: "字段必须有一个以最小值和最大值为界的数值"
            }, {
                decimal: "字段必须是数字，并且可能包含指定数量的小数点"
            }, {
                digits: "字段必须是整数，并且具有指定的位数"
            }, {
                included: "字段必须具有指定列表中的值"
            }, {
                excluded: "字段不能具有指定列表中的值"
            }, {
                max: "字段不能超过指定的长度"
            }, {
                maxValue: "字段必须是数值，并且不能大于指定的值"
            }, {
                min: "字段不能低于指定的长度"
            }, {
                minValue: "字段必须是数值，并且不能小于指定的值"
            }, {
                regex: "字段必须匹配指定的正则表达式"
            }, {
                chinese: "字段必须是中文"
            }, {
                currency: "字段必须是货币格式"
            }, {
                zip: "字段必须是邮政编码"
            }, {
                idCard: "字段必须是身份证号码"
            }];
            if (e) {
                for (var n = [], s = 0; s < e.length; s++) {
                    var a = e[s]
                        , r = _.camelCase(a.type);
                    if ("required" === r)
                        n.push({
                            required: !0,
                            message: a.message || t.required
                        });
                    else if ("email" === r)
                        n.push({
                            type: "email",
                            message: a.message || t.email
                        });
                    else if ("mobile" === r)
                        n.push({
                            validator: utils.validateMobile,
                            message: a.message || t.mobile
                        });
                    else if ("url" === r)
                        n.push({
                            type: "url",
                            message: a.message || t.url
                        });
                    else if ("alpha" === r)
                        n.push({
                            type: "alpha",
                            message: a.message || t.alpha
                        });
                    else if ("alphaDash" === r)
                        n.push({
                            type: "alphaDash",
                            message: a.message || t.alphaDash
                        });
                    else if ("alphaNum" === r)
                        n.push({
                            type: "alphaNum",
                            message: a.message || t.alphaNum
                        });
                    else if ("alphaSpaces" === r)
                        n.push({
                            type: "alphaSpaces",
                            message: a.message || t.alphaSpaces
                        });
                    else if ("creditCard" === r)
                        n.push({
                            type: "creditCard",
                            message: a.message || t.creditCard
                        });
                    else if ("between" === r)
                        n.push({
                            type: "between",
                            message: a.message || t.between
                        });
                    else if ("decimal" === r)
                        n.push({
                            type: "decimal",
                            message: a.message || t.decimal
                        });
                    else if ("digits" === r)
                        n.push({
                            type: "digits",
                            message: a.message || t.digits
                        });
                    else if ("included" === r)
                        n.push({
                            type: "included",
                            message: a.message || t.included
                        });
                    else if ("excluded" === r)
                        n.push({
                            type: "excluded",
                            message: a.message || t.excluded
                        });
                    else if ("max" === r)
                        n.push({
                            type: "max",
                            message: a.message || t.max
                        });
                    else if ("maxValue" === r)
                        n.push({
                            type: "maxValue",
                            message: a.message || t.maxValue
                        });
                    else if ("min" === r)
                        n.push({
                            type: "min",
                            message: a.message || t.min
                        });
                    else if ("minValue" === r)
                        n.push({
                            type: "minValue",
                            message: a.message || t.minValue
                        });
                    else if ("regex" === r && a.value) {
                        var i = new RegExp(a.value, "ig")
                            , u = a.message || t.regex;
                        n.push({
                            validator: function (e, t, n) {
                                t ? i.test(t) ? n() : n(new Error(u)) : n()
                            },
                            message: u
                        })
                    } else
                        "chinese" === r ? n.push({
                            type: "chinese",
                            message: a.message || t.chinese
                        }) : "currency" === r ? n.push({
                            type: "currency",
                            message: a.message || t.currency
                        }) : "zip" === r ? n.push({
                            type: "zip",
                            message: a.message || t.zip
                        }) : "idCard" === r && n.push({
                            type: "idCard",
                            message: a.message || t.idCard
                        })
                }
                return n
            }
            return null
        }
    };
