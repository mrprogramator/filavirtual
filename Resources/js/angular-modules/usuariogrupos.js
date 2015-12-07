angular.module('CatalogoApp')
    .controller('UsuarioGruposController', function ($http) {
        self = this;

        getGroupsByUser = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/UsuarioGrupo/GetGroupsByUser/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getUser = function (id) {
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

        getGroups = function (id) {
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

        saveUG = function (acc) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/UsuarioGrupo/Create/?usuarioId=' + acc.UsuarioId + '&grupoId=' + acc.GrupoId
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        removeUG = function (acc) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/UsuarioGrupo/Remove/?usuarioId=' + acc.UsuarioId + '&grupoId=' + acc.GrupoId
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

        this.addGroupToUser = function (user, group) {
            var usugru = { Usuario: user, UsuarioId: user.Id, Grupo: group, GrupoId: group.Id };
            if (self.items == undefined) {
                self.items = [usugru];
            }
            else {
                self.items.push(usugru);
            }

            removeItemFromArray(group, self.grupos);

            saveUG(usugru).then(function (promise) {
                self.response = promise.data;
            })
        }

        this.removeGroupFromUser = function (usugru) {
            if (self.grupos == undefined) {
                self.grupos = [usugru.Grupo];
            }
            else {
                self.grupos.push(usugru.Grupo);

            }

            removeItemFromArray(usugru, self.items);

            removeUG(usugru).then(function (promise) {
                self.response = promise.data;
            })
        }

        this.goBack = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('usuarios') + 1];

            tab.templateUrl = 'catalogo/usuarios/' + id + '/detail?ajax=1&_=' + Date.now();
        }

        this.init = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('usuarios') + 1];
            getUser(id).then(function (promise) {
                self.item = promise.data;

                getGroupsByUser(id).then(function (promise) {
                    self.items = promise.data;
                    getGroups().then(function (promise2) {
                        self.grupos = promise2.data;
                        self.items.forEach(function (usugru) {

                            var groups = self.grupos.filter(function (item) {
                                console.log(item, usugru);
                                return item.Id == usugru.Grupo.Id
                            });

                            groups.forEach(function (group) {
                                removeItemFromArray(group, self.grupos);
                            });
                        });
                    })
                })
            })
            
        }
    });