angular.module('CatalogoApp')
    .controller('AgenteController', function ($http, $location) {
        self = this;

        self.loading = true;

        getAgentes = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Agente/GetAgentes'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function getMesa(id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getPunto = function (id) {
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

        getAgentesByPunto = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/agentes'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }


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

        getUsuarios = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/usuarios'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveAgente = function (data) {
            var promise =  $http({
                method: 'POST',
                url: 'filavirtual/Agente/Create',
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

        updateAgente = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Agente/Edit',
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

        removeAgente = function (id) {
            return promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + id + '/delete'
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
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/Agente/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/Agente/Create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/mesas/' + id + '/edit?ajax=1&_=' + Date.now();
        }

        this.delete = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/mesas/' + id + '/delete?ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/mesas/' + id + '/detail/?ajax=1&_=' + Date.now();
        }

        this.tipos = function (tab, id) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = 'filavirtual/mesas/' + id + '/tipo-atenciones/?ajax=1&_=' + Date.now();
        }

        this.save = function (tab, data) {
            saveAgente(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updateAgente(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removeAgente(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        self.volver = function (tab, id) {
            prev = tab.pila.pop();
            tab.redirectUrl = prev.redirectUrl;
            tab.templateUrl = prev.templateUrl;
        }
        
        self.init = function (puntoId) {
            if (puntoId == null) {
                getAgentes().then(function (promise) {
                    self.items = promise.data;
                    console.log('agentes', self.items);
                    self.loading = false;
                });
                return; 
            }

            getPunto(puntoId).then(function (promise) {
                self.punto = promise.data;
            });

            getAgentesByPunto(puntoId).then(function (promise) {
                self.items = promise.data;
                self.loading = false;
            });
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('mesas') + 1]

            getMesa(id).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }

        getPuntos().then(function (promise) {
            self.puntos = promise.data;
        });

        getUsuarios().then(function (promise) {
            self.usuarios = promise.data;
        });
    });