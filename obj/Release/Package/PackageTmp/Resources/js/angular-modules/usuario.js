angular.module('CatalogoApp')
    .controller('UsuarioController', function ($http) {
        self = this;

        self.loading = true;

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

        getUsuario = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/usuarios/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveUser = function (data) {
            var promise =  $http({
                method: 'POST',
                url: 'catalogo/Usuario/Create',
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

        updateUser = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Usuario/Edit',
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

        removeUser = function (id) {
            return promise = $http({
                method: 'POST',
                url: 'catalogo/Usuario/Remove/?id=' + id
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
            tab.templateUrl = 'catalogo/Usuario/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.templateUrl = 'catalogo/Usuario/Create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.templateUrl = 'catalogo/usuarios/' + id + '/edit?ajax=1&_=' + Date.now();
        }

        this.deleteUser = function (tab, id) {
            tab.templateUrl = 'catalogo/usuarios/' + id + '/delete?ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.templateUrl = 'catalogo/usuarios/' + id + '/detail?ajax=1&_=' + Date.now();
        }

        this.groups = function (tab, id) {
            tab.templateUrl = 'catalogo/usuarios/' + id + '/grupos?ajax=1&_=' + Date.now();
        }


        this.save = function (tab, data) {
            saveUser(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updateUser(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removeUser(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('usuarios') + 1];

            getUsuario(id).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }

        getUsuarios().then(function (promise) {
            self.items = promise.data;
            self.loading = false;
        });
    });