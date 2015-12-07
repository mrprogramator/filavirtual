angular.module('CatalogoApp')
    .controller('MesaTiposController', function ($http) {
        self = this;

        getMesa = function (id) {
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

        getTiposByMesa = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + id + '/tipo-atenciones'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getTipos = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/tipo-atenciones'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveMT = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + data.MesaId + '/tipo-atenciones/' + data.TipoId + '/create'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        removeMT = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/mesas/' + data.MesaId + '/tipo-atenciones/' + data.TipoId + '/delete'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function removeItemFromArray(item, array) {
            var index = array.indexOf(item);
            if (index >= 0) {
                array.splice(index, 1);
            }
        }

        self.items = [];

        this.addTipoToMesa = function (mesa, tipo) {
            var tipomesa = { MesaId: mesa.Id, Mesa: mesa, TipoId: tipo.Id, Tipo: tipo };
            self.items.push(tipomesa);

            removeItemFromArray(tipo, self.tipos);

            saveMT(tipomesa).then(function (promise) {
                self.response = promise.data;
            })
        }

        self.tipos = [];

        this.removeTipoFromMesa = function (tipomesa) {
            self.tipos.push(tipomesa.Tipo);

            removeItemFromArray(tipomesa, self.items);

            removeMT(tipomesa).then(function (promise) {
                self.response = promise.data;
            })
        }

        this.goBack = function (tab, id) {
            tab.templateUrl = 'filavirtual/mesas/' + id + '/detail/?ajax=1&_=' + Date.now();
        }

        this.init = function (tab) {
            console.log(tab.templateUrl);
            params = tab.templateUrl.split('/');
            id = params[params.indexOf("mesas") + 1];

            getMesa(id).then(function (promise) {
                self.item = promise.data;
                getTiposByMesa(id).then(function (promise) {
                    self.items = promise.data;
                    getTipos().then(function (promise2) {
                        self.tipos = promise2.data;
                        self.items.forEach(function (tipomesa) {

                            var tipos = self.tipos.filter(function (item) {
                                console.log(item, tipomesa);
                                return item.Id == tipomesa.TipoId
                            });

                            tipos.forEach(function (tipo) {
                                removeItemFromArray(tipo, self.tipos);
                            });
                        });
                    })
                })
            });


        }
    });