var $url = "/web/include/status"
    , $isAuthenticated = !1
    , data = {
        pageLoad: !1,
        isAuthenticated: !1,
        isUserMenu: !1,
        user: null,
        userName: null,
        displayName: null,
        avatarUrl: null,
        isVip: !1
    }
    , methods = {
        apiGet: function () {
            var e = this;
            $token ? cloud.get($url).then(function (t) {
                var n = t.data;
                $isAuthenticated = e.isAuthenticated = n.isAuthenticated,
                    n.user ? (e.user = n.user,
                        e.userName = n.user.userName,
                        e.displayName = n.user.displayName,
                        e.avatarUrl = n.user.avatarUrl,
                        e.isVip = n.user.isVip) : (sessionStorage.removeItem(ACCESS_TOKEN_NAME),
                            localStorage.removeItem(ACCESS_TOKEN_NAME))
            }).then(function () {
                e.pageLoad = !0
            }) : this.pageLoad = !0
        },
        getAvatarUrl: function () {
            return this.user.avatarUrl || "/sitefiles/assets/images/default_avatar.png"
        },
        openUserMenu: function () {
            var e = this;
            this.isUserMenu = !0,
                setTimeout(function () {
                    window.document.addEventListener("click", e.clickOutEvent)
                }, 10)
        },
        close: function () {
            this.isUserMenu = !1,
                window.document.removeEventListener("click", this.clickOutEvent)
        },
        menuClick: function () {
            event.stopPropagation()
        },
        clickOutEvent: function () {
            this.close()
        },
        getLoginUrl: function () {
            return "/member/login?return_url=" + encodeURIComponent(location.href)
        },
        getRegisterUrl: function () {
            return "/member/reg?return_url=" + encodeURIComponent(location.href)
        },
        getLogoutUrl: function () {
            return "/member/logout?return_url=" + encodeURIComponent(location.href)
        }
    }
    , $status = new Vue({
        el: "#status",
        data: data,
        methods: methods,
        created: function () {
            this.apiGet()
        }
    })
    , slideout = new Slideout({
        panel: document.getElementById("mainContent"),
        menu: document.getElementById("menuContent"),
        padding: 256,
        tolerance: 70
    });
document.querySelector(".menu-toggle-button").addEventListener("click", function () {
    slideout.toggle()
}),
    $(function () {
        $(window).scroll(function () {
            $(window).scrollTop() > 600 ? $(".backToTop").show() : $(".backToTop").hide()
        });
        var e = window.location.href + "";
        0 == e.indexOf("http://") && 0 == e.indexOf("localhost") && (window.location.href = e.replace("http://", "https://"))
    });
