angular.module('CatalogoApp')
    .controller('TipoAtencionController', function ($scope, $http, $timeout) {
        self = this;

        self.loading = true;

        getTipoAtenciones = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros'
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
                return { "status": false }
            });

            return promise;
        }

        getParametro = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getGrupos = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/grupos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }
        
        getHijos = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/' + id + '/hijos/todos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveTipoAtencion = function (data) {
            var promise =  $http({
                method: 'POST',
                url: 'catalogo/parametros/create',
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

        updateTipoAtencion = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/edit',
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

        removeTipoAtencion = function (id) {
            return promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/' + id + '/delete'
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
            tab.templateUrl = 'catalogo/Parametro/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            if (self.selectedBranch == undefined) {
                tab.templateUrl = 'catalogo/parametros/create/?ajax=1&_=' + Date.now();
            }

            else {
                tab.templateUrl = 'catalogo/parametros/' + self.selectedBranch.id + '/add-child/?ajax=1&_=' + Date.now();
            }
        }

        this.edit = function (tab, id) {
            tab.templateUrl = 'catalogo/parametros/' + id + '/edit/?ajax=1&_=' + Date.now();
        }

        this.delete = function (tab, id) {
            console.log('id',id);
            tab.templateUrl = 'catalogo/parametros/' + id + '/delete/?ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.templateUrl = 'catalogo/parametros/' + id + '/detail/?ajax=1&_=' + Date.now();
        }

        this.save = function (tab, data) {
            saveTipoAtencion(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updateTipoAtencion(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removeTipoAtencion(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        self.createInit = function(tab) {
            var params = tab.templateUrl.split('/');

            if (params.indexOf('add-child') >= 0) {
                var padreId = params[ params.indexOf('add-child') - 1];
                self.item = { GrupoId: padreId };
                console.log('item with group', self.item);
            }
        }

        this.handleClick = function () {
            var self = this;
            $timeout(function () {
                self.selectedItem = self.items
                    .filter(function (item) {
                        return item.Id == self.selectedBranch.id;
                    })[0];
                console.log('selected', self.selectedBranch);
            }, 0);
        }
        
        getTipoAtenciones().then(function (promise) {
            self.items = promise.data;
            promise.data
                .filter(function (param) {
                    return param.GrupoId == "GRU";
                })
                .forEach(function (padre) {
                        fillTree(
                                promise.data,
                                my_tree, {
                                    id: padre.Id,
                                    label: padre.Nombre,
                                    children: []
                                })
                        });

            self.data = my_tree;
            console.log(my_tree);
            console.log('ITEMS',self.items);
            
            self.loading = false;
        });

        getEstados().then(function (promise) {
            self.estados = promise.data;
        })

        self.loadItem = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('parametros') + 1];
            console.log('id', id);
            getParametro(id).then(function (promise) {
                self.item = promise.data;
                console.log('item', self.item);
                self.loading = false;
            });
        }

        getGrupos().then( function (promise) {
            self.grupos = promise.data;
            self.grupos.push(undefined);
        });
        
        function isLeafOf(leaf) {
            return (leaf.GrupoId == this);
        }

        function fillTree(jsonResponse, tree, root) {
            jsonResponse.filter(isLeafOf, root.id).forEach(function (elem) {
                root.children.push({
                    id: elem.Id,
                    label: elem.Nombre,
                    children: []
                });
            });
            
            if (root.children == 0) {
                tree.push(root);
                return root;
            }

            root.children.forEach(function (branch) {
                branch = fillTree(jsonResponse, [], branch);
            });
            tree.push(root);
        }
        var my_tree = [];
    });