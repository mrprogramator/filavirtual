angular.module('CatalogoApp')
    .controller('TicketeraController', function ($http, $location) {
        self = this;
        self.loading = true;

        getTicketeras = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'filavirtual/puntos/' + puntoId + '/ticketeras'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        getTicketera = function (puntoId, ticketeraId) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'filavirtual/puntos/' + puntoId + '/ticketeras/' + ticketeraId
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }


        getPuntos = function () {
            var promise = $http({ method: 'POST', url: $location.$$absUrl + 'Punto/GetPuntos' })
                .success(function (data, status, headers, config) {
                    return data;
                })
                .error(function (data, status, headers, config) {
                    return { "status": false };
                });

            return promise;
        }

        getPunto = function (id) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'filavirtual/puntos/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getData = function () {
            var promise = $http({ method: 'POST', url: $location.$$absUrl + 'Ticketera/GetTicketeras' })
                .success(function (data, status, headers, config) {
                    return data;
                })
                .error(function (data, status, headers, config) {
                    return { "status": false };
                });

            return promise;
        }

        getPuntos().then(function (promise) {
            self.puntos = promise.data;
        });

        saveTicketera = function (data) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'ticketeras/create',
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

        updateTicketera = function (data) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'filavirtual/ticketeras/edit',
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

        removeTicketera = function (item) {
            var promise = $http({
                method: 'POST',
                url: $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/delete'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        self.volver = function (tab) {
            console.log('volviendo...', tab);
            prev = tab.pila.pop();
            tab.redirectUrl = prev.redirectUrl;
            tab.templateUrl = prev.templateUrl;
        }

        self.create = function (tab) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/ticketeras/create/?ajax=1&_=' + Date.now();
        }

        self.edit = function (tab, item) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/edit/?ajax=1&_=' + Date.now();
        }

        self.delete = function (tab, item) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/delete/?ajax=1&_=' + Date.now();
        }

        self.configs = function (tab, item) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/configs/?ajax=1&_=' + Date.now();
        }

        self.detail = function (tab, item) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/detail/?ajax=1&_=' + Date.now();
        }

        self.preview = function (tab, item) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = $location.$$absUrl + 'filavirtual/puntos/' + item.PuntoId + '/ticketeras/' + item.Id + '/preview/?ajax=1&_=' + Date.now();
        }

        self.save = function (tab, item) {
            saveTicketera(item).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            })
        }

        self.update = function (tab, item) {
            updateTicketera(item).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            })
        }

        self.remove = function (tab, item) {
            removeTicketera(item).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            })
        }

        self.init = function (puntoId) {
            console.log('puntoId', puntoId);
            if (puntoId == null) {
                getData().then(function (promise) {
                    self.items = promise.data;
                    console.log(self.items);
                    self.loading = false;
                });
                return;
            }

            getPunto(puntoId).then(function (promise) {
                self.punto = promise.data;
            })

            self.loading = true;
            self.items = [];
            getTicketeras(puntoId).then(function (promise) {
                console.log('promise data', promise.data);
                self.items = promise.data;
                self.loading = false;
            });
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var puntoId = params[params.indexOf('puntos') + 1];
            var ticketeraId = params[params.indexOf('ticketeras') + 1];

            getTicketera(puntoId, ticketeraId).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }
    });