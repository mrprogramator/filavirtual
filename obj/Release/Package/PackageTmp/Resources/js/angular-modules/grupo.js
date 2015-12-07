angular.module('CatalogoApp')
    .controller('GrupoController', function ($http) {
        self = this;
        self.loading = true;
        getGrupos = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false }
            });

            return promise;
        }

        getGrupo = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }


        getEstados = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/estados'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false }
            });

            return promise;
        }

        saveGroup = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos/create',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false }
            });

            return promise;
        }

        updateGroup = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos/edit',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false }
            });

            return promise;
        }

        removeGroup = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos/' + id + '/delete'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false }
            });

            return promise;
        }

        getGrupos().then(function (promise) {
            self.items = promise.data;
            self.loading = false;
        });
        getEstados().then(function (promise) {
            self.estados = promise.data;
        });

        this.index = function (tab) {
            tab.templateUrl = 'catalogo/Grupo/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.templateUrl = 'catalogo/grupos/create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.templateUrl = 'catalogo/grupos/' + id + '/edit/?&ajax=1&_=' + Date.now();
        }

        self.delete = function (tab, id) {
            tab.templateUrl = 'catalogo/grupos/' + id + '/delete/?&ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.templateUrl = 'catalogo/grupos/' + id + '/detail/?&ajax=1&_=' + Date.now();
        }

        this.users = function (tab, id) {
            tab.templateUrl = 'catalogo/grupos/' + id + '/usuarios/?&ajax=1&_=' + Date.now();
        }

        this.save = function (tab, data) {
            saveGroup(data).then(function (promise) {
                self.response = promise.data;
                
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updateGroup(data).then(function (promise) {
                self.response = promise.data;

                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removeGroup(id).then(function (promise) {
                self.response = promise.data;
                
                if (self.response.result) {
                    self.index(tab);
                }
            })
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('grupos') + 1];

            getGrupo(id).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }
    });