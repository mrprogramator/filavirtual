angular.module('CatalogoApp')
    .controller('GrupoUsuariosController', function ($http) {
        self = this;

        getUsersByGroup = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/UsuarioGrupo/GetUsersByGroup/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
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

        this.addUserToGroup = function (user, group) {
            var usugru = { Usuario: user, UsuarioId: user.Id, Grupo: group, GrupoId: group.Id };
            if (self.items == undefined) {
                self.items = [usugru];
            }
            else {
                self.items.push(usugru);
            }

            removeItemFromArray(user, self.usuarios);

            saveUG(usugru).then(function (promise) {
                self.response = promise.data;
            })
        }

        this.removeUserFromGroup = function (usugru) {
            if (self.usuarios == undefined) {
                self.usuarios = [usugru.Grupo];
            }
            else {
                self.usuarios.push(usugru.Usuario);

            }

            removeItemFromArray(usugru, self.items);

            removeUG(usugru).then(function (promise) {
                self.response = promise.data;
            })
        }

        this.goBack = function (tab, grupoId) {
            tab.templateUrl = 'catalogo/grupos/' + grupoId + '/detail?ajax=1&_=' + Date.now();
        }

        this.init = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('grupos') + 1]

            getGrupo(id).then(function (promise) {
                self.item = promise.data;

                getUsersByGroup(id).then(function (promise) {
                    self.items = promise.data;

                    getUsuarios().then(function (promise2) {
                        self.usuarios = promise2.data;
                        console.log('USUARIOS', self.usuarios);
                        self.items.forEach(function (usugru) {

                            var users = self.usuarios.filter(function (item) {
                                return item.Id == usugru.Usuario.Id
                            });

                            users.forEach(function (user) {
                                removeItemFromArray(user, self.usuarios);
                            });
                        });
                    });
                })
            })
        }
    });