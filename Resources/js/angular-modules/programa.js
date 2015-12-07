angular.module('CatalogoApp')
    .controller('ProgramaController', function ($http) {
        self = this;
        self.loading = true;
        getPrograms = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/programas'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

            return promise;
        }

        getProgram = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/programas/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            })

            return promise;
        }

        getTipoPrograms = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Parametro/GetTipoProgramas'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

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
                return {"status": false}
            })

            return promise;
        }

        getAccessByProgram = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'Acceso/GetAccessByProgram/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

            return promise;
        }
        
        saveProgram = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Programa/Create',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

            return promise;
        }

        updateProgram = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Programa/Edit',
                data: data
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

            return promise;
        }

        removeProgram = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/programas/' + id + '/delete'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return {"status": false}
            })

            return promise;
        }

        this.index = function (tab) {
            tab.templateUrl = 'catalogo/Programa/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.templateUrl = 'catalogo/Programa/Create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.templateUrl = 'catalogo/programas/' + id + '/edit?ajax=1&_=' + Date.now();
        }

        this.delete = function (tab, id) {
            tab.templateUrl = 'catalogo/programas/' + id + '/delete?ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.templateUrl = 'catalogo/programas/' + id + '/detail?ajax=1&_=' + Date.now();
        }

        this.access = function (tab, id){
            tab.templateUrl = 'catalogo/programas/' + id + '/accesos?ajax=1&_=' + Date.now();
        }

        this.accessgroup = function (tab, id) {
            tab.templateUrl = 'catalogo/programas/' + id + '/accesos-grupos?ajax=1&_=' + Date.now();
        }

        this.save = function (tab, data) {
            saveProgram(data).then(function (promise) {
                self.response = promise.data;

                if (self.response.result) {
                    tab.templateUrl = self.response.value + '/?ajax=1&_=' + Date.now();
                }
            })
        }

        this.update = function (tab, data) {
            updateProgram(data).then(function (promise) {
                self.response = promise.data;

                if (self.response.result) {
                    self.index(tab);
                }
            })
        }

        this.remove = function (tab, id) {
            removeProgram(id).then(function (promise) {
                self.response = promise.data;

                if (self.response.result) {
                    self.index(tab);
                }
            })
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split("/");
            var programa_id = params[params.indexOf("programas") + 1];
            getProgram(programa_id).then(function (promise) {
                self.item = promise.data;
                console.log(self.item);
                self.loading = false;
            });
        }
        
        getPrograms().then(function (promise) {
            self.items = promise.data;
            self.loading = false;
        })

        getPrograms().then(function (promise) {
            self.padres = promise.data;
            self.padres.push(undefined);
        })

        getTipoPrograms().then(function (promise) {
            self.tipos = promise.data;
        })
        
        getEstados().then(function (promise) {
            self.estados = promise.data;
        })

        this.loadAccess = function (item){
            return item;
        }

        
    });