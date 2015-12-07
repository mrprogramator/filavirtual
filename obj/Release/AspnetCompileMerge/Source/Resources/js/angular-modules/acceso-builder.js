angular.module('CatalogoApp')
    .controller('AccesoBuilderController', function ($http) {
        self = this;
        getAccessByUser = function (user) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Acceso/GetAccessByUser/?id=' + user
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            })

            return promise;
        }

        getAccessByGroup = function (group) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/AccesoGrupo/GetAccessByGroup/?id=' + group
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            })

            return promise;
        }

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

        getAccessByProgram = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/Acceso/GetAccessByProgram/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getGroupAccessByProgram = function (id) {
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

        self.init = function () {
            var id = localStorage.getItem('user');
            getAccessByUser(id).then(function (promise) {
                self.userAccess = promise.data;
                console.log('uA', self.userAccess);
                getGroupsByUser(id).then(function (promise2) {
                    var groups = promise2.data;
                    console.log('groups', groups);
                    groups.forEach(function (group) {
                        getAccessByGroup(group.GrupoId).then(function (promise3) {
                            self.groupAccess = promise3.data;
                            console.log('gA', self.groupAccess);
                            allAccess = { byGroup: self.groupAccess, byUser: self.userAccess }
                        })
                    })
                })
            })
        }

        self.init();
    });