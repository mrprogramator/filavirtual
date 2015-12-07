angular.module('CatalogoApp')
    .controller('ConfTicketeraController', function ($http, $timeout, $location) {
        CODGRU = 'TICGRU';

        self = this;

        self.loading = true;
        
        getTicketera = function (puntoId, id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/puntos/' + puntoId + '/ticketeras/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getGrupos = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/ticketeras/' + id + '/configs/grupos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function getConfig(id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/configs/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getChildren = function (ticketeraId, padreId) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/ticketeras/' + ticketeraId + '/configs/grupos/' + padreId + '/hijos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getItems = function (ticketeraId) {
            console.log('getItems', ticketeraId);
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/ticketeras/' + ticketeraId + '/configs'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }
        
        getTipos = function () {
            
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/tipos'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getTipoAtenciones = function () {
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

        getTiposTicket = function () {
            var promise = $http({
                method: 'POST',
                url: 'catalogo/parametros/tipos-ticket'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveConfig = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/ticketeras/configs/create',
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

        editConfig = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/ticketeras/configs/edit',
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

        deleteConfig = function (data) {
            var promise = $http({
                method: 'POST',
                url:'filavirtual/ticketeras/configs/' + data.Id + '/delete',
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }
        self.index = function (tab, puntoId, ticketeraId) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = "filavirtual/puntos/" + puntoId + "/ticketeras/" + ticketeraId + "/configs/?ajax=1&_=" + Date.now();
        }

        self.volver = function (tab) {
            prev = tab.pila.pop();
            tab.redirectUrl = prev.redirectUrl;
            tab.templateUrl = prev.templateUrl;
        }

        self.create = function (tab) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('?ajax','create/?ajax');
        }

        self.edit = function (tab, item) {
            if (item == undefined) {
                self.response = {result: false, value: 'Seleccione un item'}
                return;
            }
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('?ajax',item.Id + '/edit/?ajax');
        }

        self.detail = function (tab, item) {
            if (item == undefined) {
                self.response = {result: false, value: 'Seleccione un item'}
                return;
            }
            tab.templateUrl = tab.templateUrl.replace('?ajax',item.Id + '/detail/?ajax');
        }

        self.editFromDetail = function (tab) {
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('detail','edit');  
        }

        self.delete = function (tab, item) {
            if (item == undefined) {
                self.response = {result: false, value: 'Seleccione un item'}
                return;
            }
            tab.pila.push({redirectUrl: tab.redirectUrl, templateUrl: tab.templateUrl});
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('?ajax',item.Id + '/delete/?ajax');
        }

        self.save = function (tab, data) {
            data.PuntoId = param[param.indexOf("puntos") + 1];
            data.TicketeraId = param[param.indexOf("ticketeras") + 1];
            
            saveConfig(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.volver(tab);
                }
            });
        }

        self.update = function (tab, data) {
            editConfig(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    tab.templateUrl = tab.templateUrl.replace("/" + data.Id + "/edit","");
                }
            });
        }

        self.remove = function (tab, data) {
            console.log('data to delete', data);
            deleteConfig(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                console.log('delete url before', tab.templateUrl);
                    tab.templateUrl = tab.templateUrl.replace("/" + data.Id + "/delete","");
                }
            })
        }
        
        self.cancel = function (tab) {
            console.log('tab',tab.templateUrl);
            tab.templateUrl = tab.templateUrl.replace("/create","");
        };
        
        self.init = function (tab) {
            param = tab.templateUrl.split('/');
            puntoId = param[param.indexOf("puntos") + 1];
            id = param[param.indexOf("ticketeras") + 1];

            getTicketera(puntoId, id).then(function (prom) {
                self.ticketera = prom.data;
                getItems(self.ticketera.Id).then(function (promise) {
                    self.items = promise.data;
                    console.log('ItEMS',self.items);
                    self.ticketera.grupos = promise.data.filter(function (p) {
                        return p.TipoId == CODGRU;
                    });
                    self.ticketera.grupos.forEach(function (grupo) {
                        getChildren(self.ticketera.Id, grupo.Id).then (function (promise2) {
                            grupo.hijos = promise2.data;
                            //self.cargarExtremo(grupo);

                        });

                    });

                    promise.data
                        .filter(function (p) {
                                return p.TipoId == CODGRU;
                        })
                        .forEach(function (padre) {
                            fillTree(
                                promise.data,
                                my_tree, {
                                    id: padre.Id,
                                    label: padre.Descripcion,
                                    children: []
                                })
                        });

                        self.data = my_tree;
                        console.log(my_tree);
                        console.log('ITEMS',self.items);
                        self.loading = false;
                    });
                     getGrupos(self.ticketera.Id).then(function (promise) {
                        self.padres = promise.data;
                        self.padres.forEach(function (padre) {
                            /*getChildren(self.ticketera.Id, padre.Id).then (function (promise2) {
                                padre.hijos = promise2.data;
                                console.log('padres',self.padres);
                            });*/
                        });
                    });
                    
                    getTipoAtenciones().then(function (promise) {
                        self.tipoAtenciones = promise.data;
                    });
            })
        }

        self.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var id = params[params.indexOf('configs') + 1];

            getConfig(id).then(function (promise) {
                self.item = promise.data;
                self.loading = false;
            })
        }


        self.cargarExtremo = function (grupo) {
            grupo.firstChildren = [];
            grupo.lastChildren = [];
            while(grupo.hijos.length > 0) {
                firstChild = grupo.hijos.shift();
                if(firstChild != undefined) {
                    grupo.firstChildren.push(firstChild);
                }

                lastChild = grupo.hijos.pop();
                if(lastChild != undefined) {
                    grupo.lastChildren.push(lastChild);
                }
            }
            console.log('groupchildren', grupo.hijos);
        };

        function isLeafOf(leaf) {
            return (leaf.PadreId == this);
        }

        function fillTree(jsonResponse, tree, root) {
            var hasAcc = false;
            jsonResponse.filter(isLeafOf, root.id).forEach(function (elem) {
                root.children.push({
                    id: elem.Id,
                    label: elem.Descripcion,
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

        getTipos().then(function (promise) {
            self.tipos = promise.data;
            console.log('tipos',self.tipos);
        });

        getTiposTicket().then(function (promise) {
            self.tiposTicket = promise.data;
            console.log('tipos de ticket', self.tiposTicket);
        })

        self.paintButtons = function () {
            $(".div-success a").addClass("btn-success");
            $(".div-info a").addClass("btn-default");
            $(".div-danger a").addClass("btn-success");
        }
    });