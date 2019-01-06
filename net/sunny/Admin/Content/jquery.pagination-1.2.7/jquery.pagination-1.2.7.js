﻿/*! jQuery Pagination Plugin - v1.2.7 - 12/6/2015 | (c) 2015 Mricle , Licensed MIT , http://mricle.com/JqueryPagination */
!function (a) {
    "use strict";
    var d, e, f, g, h, i, j, k, l, b = {
        pageClicked: "pageClicked",
        jumpClicked: "jumpClicked",
        pageSizeChanged: "pageSizeChanged"
    }, c = function (c, d) {
        var f, g, e = {
            pageSize: 10,
            pageBtnCount: 11,
            showFirstLastBtn: !0,
            firstBtnText: null,
            lastBtnText: null,
            prevBtnText: "&laquo;",
            nextBtnText: "&raquo;",
            loadFirstPage: !0,
            remote: {
                url: null,
                params: null,
                callback: null,
                success: null,
                beforeSend: null,
                complete: null,
                pageIndexName: "pageIndex",
                pageSizeName: "pageSize",
                totalName: "total"
            },
            showInfo: !1,
            infoFormat: "{start} ~ {end} of {total} entires",
            showJump: !1,
            jumpBtnText: "Go",
            showPageSizes: !1,
            pageSizeItems: null,
            debug: !1
        };
        this.$element = a(c),
        this.$page = a('<ul class="m-pagination-page"></ul>'),
        this.$size = a('<div class="m-pagination-size"></div>'),
        this.$jump = a('<div class="m-pagination-jump"></div>'),
        this.$info = a('<div class="m-pagination-info"></div>'),
        this.options = a.extend(!0, {}, e, a.fn.page.defaults, d),
        this.total = this.options.total || 0,
        this.options.pageSizeItems = this.options.pageSizeItems || [5, 10, 15, 20],
        this.currentPageIndex = 0,
        this.currentPageSize = this.options.pageSize,
        this.pageCount = h(this.total, this.currentPageSize),
        null == this.options.remote.success && (this.options.remote.success = this.options.remote.callback),
        f = function (b) {
            var e, f, c = b, d = a('<select data-page-btn="size"></select>');
            for (e = 0; e < c.options.pageSizeItems.length; e++)
                d.append('<option value="' + c.options.pageSizeItems[e] + '">' + c.options.pageSizeItems[e] + "</option>");
            d.val(c.currentPageSize),
            c.$size.append(d),
            f = '<div class="m-pagination-group"><input type="text"><button data-page-btn="jump" type="button">' + c.options.jumpBtnText + "</button></div>",
            c.$jump.append(f),
            c.$jump.find("input").change(function () {
                j(this.value, c.pageCount) || (this.value = null)
            }),
            c.$element.append(c.$page.hide()),
            c.$element.append(c.$size.hide()),
            c.$element.append(c.$jump.hide()),
            c.$element.append(c.$info.hide()),
            c._remoteOrRedner(0),
            c.$element.on("click", {
                page: c
            }, function (a) {
                g(a)
            }).on("change", {
                page: c
            }, function (a) {
                g(a)
            })
        }
        ,
        g = function (c) {
            var f, g, h, d = c.data.page, e = a(c.target);
            "click" !== c.type || void 0 === e.data("pageIndex") || e.parent().hasClass("active") ? "click" === c.type && "jump" === e.data("pageBtn") ? (g = d.$jump.find("input").val(),
            j(g, d.pageCount) && (f = g - 1,
            d.$element.trigger(b.jumpClicked, f),
            d.debug("event[ jumpClicked ] : pageIndex = " + f),
            d._remoteOrRedner(f)),
            d.$jump.find("input").val(null)) : "change" === c.type && "size" === e.data("pageBtn") && (h = d.$size.find("select").val(),
            d.currentPageSize = h,
            d.$element.trigger(b.pageSizeChanged, h),
            d.debug("event[ pageSizeChanged ] : pageSize = " + h),
            d._remoteOrRedner(0)) : (f = a(c.target).data("pageIndex"),
            d.$element.trigger(b.pageClicked, f),
            d.debug("event[ pageClicked ] : pageIndex = " + f),
            d._remoteOrRedner(f))
        }
        ,
        "undefined" == typeof this.options.total && null === this.options.remote.url ? console && console.error("[init error] : the options must have the parameter of 'remote.url' or 'total'.") : "undefined" != typeof this.options.total || this.options.loadFirstPage ? f(this) : console && console.error("[init error] : if you don't remote the first page. you must set the options or 'total'.")
    };
    c.prototype = {
        _remoteOrRedner: function (a) {
            null != this.options.remote.url && (this.options.loadFirstPage || a > 0) ? this.remote(a) : this.renderPagination(a)
        },
        remote: function (b, c) {
            var e, f, d = this;
            (isNaN(parseInt(b)) || "object" == typeof b) && (c = b,
            b = null),
            isNaN(parseInt(b)) && (b = d.currentPageIndex),
            e = {},
            e[this.options.remote.pageIndexName] = b,
            e[this.options.remote.pageSizeName] = this.currentPageSize,
            this.options.remote.params = i(this.options.remote.params),
            c && (c = i(c),
            this.options.remote.params = a.extend({}, this.options.remote.params, c)),
            f = a.extend({}, this.options.remote.params, e),
            a.ajax({
                url: this.options.remote.url,
                dataType: "json",
                data: f,
                contentType: "application/Json",
                async: !1,
                beforeSend: function (a) {
                    "function" == typeof d.options.remote.beforeSend && d.options.remote.beforeSend(a)
                },
                complete: function (a, b) {
                    "function" == typeof d.options.remote.complete && d.options.remote.complete(a, b)
                },
                success: function (a) {
                    d.debug("ajax request : params = " + JSON.stringify(f), a);
                    var c = k(a, d.options.remote.totalName);
                    null == c || void 0 == c ? console && console.error("the response of totalName :  '" + d.options.remote.totalName + "'  not found") : (d._updateTotal(c),
                    "function" == typeof d.options.remote.success && d.options.remote.success(a, b),
                    d.renderPagination(b))
                }
            })
        },
        renderPagination: function (a) {
            this.currentPageIndex = a;
            var b = e(this.currentPageIndex, this.currentPageSize, this.total, this.options.pageBtnCount, this.options.firstBtnText, this.options.lastBtnText, this.options.prevBtnText, this.options.nextBtnText, this.options.showFirstLastBtn);
            this.$page.empty().append(b),
            this.$info.text(d(this.currentPageIndex, this.currentPageSize, this.total, this.options.infoFormat)),
            this.pageCount >= 1 ? (this.$page.show(),
            this.options.showPageSizes && this.$size.show(),
            this.options.showJump && this.$jump.show(),
            this.options.showInfo && this.$info.show()) : 1 == this.pageCount ? this.options.showInfo && this.$info.show() : (this.$page.hide(),
            this.$size.hide(),
            this.$jump.hide(),
            this.$info.hide())
        },
        _updateTotal: function (a) {
            this.total = a,
            this.pageCount = h(a, this.currentPageSize)
        },
        destroy: function () {
            this.$element.unbind().data("page", null).empty()
        },
        debug: function (a, b) {
            this.options.debug && console && (a && console.info(a),
            b && console.info(b))
        },
        setPage: function (a) {
            this.renderPagination(a);
        }
    },
    d = function (a, b, c, d) {
        var e = a * b + 1
          , f = (a + 1) * b;
        return f = f >= c ? c : f,
        d.replace("{start}", e).replace("{end}", f).replace("{total}", c)
    }
    ,
    e = function (a, b, c, d, e, i, j, k, l) {
        var m, n, o, p, q, r, s, t, u;
        return a = void 0 == a ? 1 : parseInt(a) + 1,
        m = h(c, b),
        n = [],
        d >= m ? n = f(1, m, a) : (o = g(e || 1, 0),
        p = g(i || m, m - 1),
        q = g(j, a - 2),
        r = g(k, a),
        s = (d - 1 - 4) / 2,
        l || (s += 1),
        t = (d + 1) / 2,
        u = m - (d + 1) / 2,
        s = -1 == s.toString().indexOf(".") ? s : s + .5,
        t = -1 == t.toString().indexOf(".") ? t : t + .5,
        u = -1 == u.toString().indexOf(".") ? u : u + .5,
        t >= a ? l ? (n = f(1, d - 2, a),
        n.push(r),
        n.push(p)) : (n = renderPagenderPage(1, d - 1, a),
        n.push(r)) : a > u ? l ? (n = f(m - d + 3, d - 2, a),
        n.unshift(q),
        n.unshift(o)) : (n = f(m - d + 2, d - 1, a),
        n.unshift(q)) : l ? (n = f(a - s, d - 4, a),
        n.unshift(q),
        n.push(r),
        n.unshift(o),
        n.push(p)) : (n = f(a - s, d - 2, a),
        n.unshift(q),
        n.push(r))),
        n
    }
    ,
    f = function (a, b, c) {
        var e, f, d = [];
        for (e = 0; b > e; e++)
            f = g(a, a - 1),
            a == c && f.addClass("active"),
            d.push(f),
            a++;
        return d
    }
    ,
    g = function (b, c) {
        return a("<li><a data-page-index='" + c + "'>" + b + "</a></li>")
    }
    ,
    h = function (a, b) {
        var d, c = 0;
        return a = parseInt(a),
        d = a / b,
        c = -1 != d.toString().indexOf(".") ? parseInt(d.toString().split(".")[0]) + 1 : d
    }
    ,
    i = function (a) {
        var c, d, e, b = {};
        if ("string" == typeof a)
            for (c = a.split("&"),
            d = 0; d < c.length; d++)
                e = c[d].split("="),
                b[e[0]] = decodeURIComponent(e[1]);
        else if (a instanceof Array)
            for (d = 0; d < a.length; d++)
                b[a[d].name] = decodeURIComponent(a[d].value);
        else
            "object" == typeof a && (b = a);
        return b
    }
    ,
    j = function (a, b) {
        var c = /^\+?[1-9][0-9]*$/;
        return c.test(a) && parseInt(a) <= parseInt(b)
    }
    ,
    k = function (a, b) {
        var f, c = b.split("."), d = a, e = null;
        for (f = 0; f < c.length; f++) {
            if (d = l(d, c[f]),
            !isNaN(parseInt(d))) {
                e = d;
                break
            }
            if (null == d)
                break
        }
        return e
    }
    ,
    l = function (a, b) {
        for (var c in a)
            if (c == b)
                return a[c];
        return null
    }
    ,
    a.fn.page = function (b) {
        var d = arguments;
        return this.each(function () {
            var g, h, e = a(this), f = e.data("page");
            f || "object" != typeof b && "undefined" != typeof b ? f && "string" == typeof b ? f[b].apply(f, Array.prototype.slice.call(d, 1)) : f || console && console.error("jQuery Pagination Plugin is uninitialized.") : (g = "object" == typeof b && b,
            h = e.data(),
            g = a.extend(g, h),
            e.data("page", f = new c(this, g)))
        })
    }
}(window.jQuery);
