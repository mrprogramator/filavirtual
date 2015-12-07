angular.module('CatalogoApp')
    .controller('AccesoGrupoController', function ($http) {
        self = this;

        function getAccessByProgram(id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/AccesoGrupo/GetAccessByProgram/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function getGrupos() {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/grupos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function getProgram(id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/programas/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        this.save = function (acc) {
            console.log("acc", acc);
            var promise = $http({
                method: 'POST',
                url: 'catalogo/AccesoGrupo/Create/?programaId=' + acc.ProgramaId + '&grupoId=' + acc.GrupoId
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        this.update = function (acc) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/AccesoGrupo/Update',
                data: acc
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        this.remove = function (acc) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/AccesoGrupo/Remove/?programaId=' + acc.ProgramaId + '&grupoId=' + acc.GrupoId,
                data: acc
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        this.index = function (tab, programaId) {
            tab.templateUrl = 'catalogo/Programa/Access/?id=' + programaId + '&ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, acc) {
            tab.templateUrl = 'catalogo/AccesoGrupo/Detail/?programaId=' + acc.ProgramaId + '&grupoId=' + acc.GrupoId + '&ajax=1&_=' + Date.now();
        }

        function removeItemFromArray(item, array) {
            var index = array.indexOf(item);
            if (index >= 0) {
                array.splice(index, 1);
            }
        }

        this.addToAccess = function (user, program) {
            var acc = { Grupo: user, GrupoId: user.Id, Programa: program, ProgramaId: program.Id };
            if (self.items == undefined) {
                self.items = [acc];
                return;
            }

            self.items.push(acc);
            removeItemFromArray(user, self.grupos);

            self.save(acc).then(function (promise) {
                self.response = promise.data
            });
        }

        this.removeFromAccess = function (acc) {
            if (self.grupos == undefined) {
                self.grupos = [acc.Grupo];
                return;
            }

            self.grupos.push(acc.Grupo);
            removeItemFromArray(acc, self.items);

            self.remove(acc).then(function (promise) {
                self.response = promise.data
            });
        }

        this.goBack = function (tab, programaId) {
            tab.templateUrl = 'catalogo/programas/' + programaId + '/detail?ajax=1&_=' + Date.now();
        }

        this.init = function (tab) {
            var params = tab.templateUrl.split('/');
            var programa_id = params[params.indexOf('programas') + 1];

            getProgram(programa_id).then(function (promise) {
                prog = promise.data;
                self.item = prog;
                getAccessByProgram(prog.Id).then(function (promise) {
                    self.items = promise.data;
                    getGrupos().then(function (promise) {
                        self.grupos = promise.data;
                        console.log('GRUPOS', self.grupos);
                        self.items.forEach(function (acc) {

                            var groups = self.grupos.filter(function (item) {
                                return item.Id == acc.Grupo.Id
                            });

                            groups.forEach(function (group) {
                                removeItemFromArray(group, self.grupos);
                            });
                        });
                    })
                });
            })
        }
    });