var cloud = _.extend(axios.create({
    baseURL: "https://api.sscms.com/v7",
    headers: {
        Authorization: "Bearer " + localStorage.getItem("ss_cloud_access_token")
    }
}), {
    host: "https://sscms.com",
    hostDl: "https://dl.sscms.com",
    hostDemo: "https://demo.sscms.com",
    hostStorage: "https://storage.sscms.com",
    getPluginIconUrl: function (t) {
        return t.icon ? t.success && !t.disabled ? t.icon : this.hostStorage + "/plugins/" + t.pluginId + "/logo" + t.icon.substring(t.icon.lastIndexOf(".")) : utils.getAssetsUrl("images/favicon.png")
    },
    getTemplatesUrl: function (t) {
        return this.host + "/templates/" + t
    },
    getPluginsUrl: function (t) {
        return this.host + "/plugins/" + t
    },
    getDocsUrl: function (t) {
        return this.host + "/docs/v7/" + t
    },
    getPlugins: function (t) {
        return this.post("cms/plugins", {
            word: t
        })
    },
    getPlugin: function (t, s) {
        return this.get("cms/plugins/" + t, {
            params: {
                version: s
            }
        }).catch(function (t) {
            if (!t.response || 404 !== t.response.status)
                throw t;
            utils.error("找不到资源，请重试或者检查计算机是否能够连接外网")
        })
    },
    getTemplates: function (t, s, e, n, r) {
        return this.get("cms/templates", {
            params: {
                page: t,
                word: s,
                tag: e,
                price: n,
                order: r
            }
        })
    },
    getTemplate: function (t) {
        return this.get("cms/templates/" + t)
    },
    getUpdates: function (t, s) {
        return this.post("cms/updates", {
            version: t,
            pluginIds: s
        })
    },
    compareVersion: function (t, s, e) {
        var n = (t || "").split("-")[0]
            , r = (s || "").split("-")[0]
            , o = e && e.lexicographical
            , i = e && e.zeroExtend
            , c = n.split(".")
            , u = r.split(".");
        function l(t) {
            return (o ? /^\d+[A-Za-z]*$/ : /^\d+$/).test(t)
        }
        if (!c.every(l) || !u.every(l))
            return NaN;
        if (i) {
            for (; c.length < u.length;)
                c.push("0");
            for (; u.length < c.length;)
                u.push("0")
        }
        o || (c = c.map(Number),
            u = u.map(Number));
        for (var a = 0; a < c.length; ++a) {
            if (u.length == a)
                return 1;
            if (c[a] != u[a])
                return c[a] > u[a] ? 1 : -1
        }
        return c.length != u.length ? -1 : 0
    }
});
