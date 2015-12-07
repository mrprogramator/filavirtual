angular.module('CatalogoApp')
    .controller('PuntoController', function ($http, $location) {
        self = this;

        self.loading = true;

        getPuntos = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function getPunto(id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }
        
        savePunto = function (data) {
            var promise =  $http({
                method: 'POST',
                url: 'filavirtual/Punto/Create',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        updatePunto = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Punto/Edit',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        removePunto = function (id) {
            return promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + id + '/delete'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        this.index = function (tab) {
            tab.templateUrl = 'filavirtual/Punto/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/Punto/Create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/puntos/' + id + '/edit?ajax=1&_=' + Date.now();
            console.log('tab',tab);
        }

        this.delete = function (tab, id) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/puntos/' + id + '/delete?ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/puntos/' + id + '/detail?ajax=1&_=' + Date.now();
        }

        this.ticketeras = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/puntos/' + id + '/ticketeras/?ajax=1&_=' + Date.now();
        }

        this.agentes = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/puntos/' + id + '/agentes/?ajax=1&_=' + Date.now();
        }

        this.volver = function (tab) {
            tab.templateUrl = tab.redirectUrl;
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('puntos') + 1];
            getPunto(id).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }

        this.save = function (tab, data) {
            savePunto(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updatePunto(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removePunto(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            });
        }

        getPuntos().then(function (promise) {
            self.items = promise.data;
            self.loading = false;
        });
    });