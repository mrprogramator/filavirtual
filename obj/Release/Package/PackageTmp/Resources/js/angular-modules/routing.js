angular.module('CatalogoApp')
    .controller('RoutingController', function () {

        this.response = { result: true, value: '#' };

        this.loadUrl = function (tab, url, data) {
            tab.templateUrl = url + jQuery.param(data);
        }

        this.save = function (item, url, tab) {
            this.response = $.ajax({
                type: 'POST',
                url: url,
                data: item,
                async: false
            }).responseJSON;

            if (this.response.result) {
                tab.templateUrl = this.response.value + '/?ajax=1&_=' + Date.now();
            }
        }

        this.update = function (item, url, tab) {
            this.response = $.ajax({
                type: 'POST',
                url: url,
                data: item,
                async: false
            }).responseJSON

            if (this.response.result) {
                tab.templateUrl = this.response.value + '/?ajax=1&_=' + Date.now();
            }
        }

        this.remove = function (id, url, tab) {
            this.response = $.ajax({
                type: 'POST',
                url: url + '/?id=' + id,
                async: false
            }).responseJSON

            if (this.response.result) {
                console.log(this.response);
                tab.templateUrl = this.response.value + '/?ajax=1&_=' + Date.now();
            }
        }

        this.loadUrl = function (tab, url, data) {
            tab.templateUrl = url + jQuery.param(data);
        }
    });