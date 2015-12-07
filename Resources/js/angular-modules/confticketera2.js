angular.module('CatalogoApp')
    .controller('ConfTicketeraController', function ($http, $timeout) {
        self = this;

        self.loading = true;

        btnClasses = [
            'btn-success',
            'btn-danger',
            'btn-primary'
        ]
        self.i = 0;
        
        self.next = function () {
            console.log('next', self.i);
            self.i = self.i + 1;

            if (self.i >= btnClasses.length) {
                self.i = 0;
            }
        }

        self.getBtnClass = function (next) {
            console.log(btnClasses[self.i], self.i, next);
            console.log(self.btnClasses);
            self.next();
            t= self.i;
            var x = btnClasses[t].toString();
            return x;

        }
        getGrupos = function () {
            var promise = $http({
                method: 'POST',
                url: full + '/ticketeras/configs/grupos'
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
                url: full + '/ticketeras/'+ ticketeraId + '/configs/grupos/' + padreId + '/hijos'
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
            var promise = $http({
                method: 'POST',
                url: full + '/ticketeras/'+ticketeraId + '/configs'
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
                url: full + '/parametros/tipos'
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
                url: full + '/ticketeras/configs/create',
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
                url: full + '/ticketeras/configs/edit',
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
                url: full + '/ticketeras/configs/' + data.Id + '/delete',
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
            tab.templateUrl = full + "/puntos/" + puntoId + "/ticketeras/" + ticketeraId + "/configs/?ajax=1&_=" + Date.now();
        }

        self.volver = function (tab) {
            tab.templateUrl = tab.redirectUrl;
        }

        self.create = function (tab) {
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('?ajax','create/?ajax');
        }

        self.edit = function (tab, item) {
            if (item == undefined) {
                self.response = {result: false, value: 'Seleccione un item'}
                return;
            }
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
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('detail','edit');  
        }

        self.delete = function (tab, item) {
            if (item == undefined) {
                self.response = {result: false, value: 'Seleccione un item'}
                return;
            }
            tab.redirectUrl = tab.templateUrl;
            tab.templateUrl = tab.templateUrl.replace('?ajax',item.Id + '/delete/?ajax');
        }

        self.save = function (tab, data) {
            data.PuntoId = tab.templateUrl.split('/')[4];
            data.TicketeraId = tab.templateUrl.split('/')[6];
            
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
        
        self.init = function (ticketera) {
            self.ticketera = ticketera;
            getItems(self.ticketera.Id).then(function (promise) {
                self.items = promise.data;
                    console.log('ItEMS',self.items);
                    self.ticketera.grupos = promise.data.filter(function (p) {
                        return p.PadreId == undefined;
                    });

                    self.ticketera.grupos.forEach(function (grupo) {
                        getChildren(ticketera.Id, grupo.Id).then (function (promise2) {
                            grupo.hijos = promise2.data;
                            self.cargarExtremo(grupo);

                        });

                    });

                    promise.data
                        .filter(function (p) {
                                return p.PadreId == undefined
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

            getGrupos().then(function (promise) {
                self.padres = promise.data;
                self.padres.forEach(function (padre) {
                    getChildren(ticketera.Id, padre.Id).then (function (promise2) {
                        padre.hijos = promise2.data;
                        console.log('padres',self.padres);
                    });
                });
            });
            
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
        self.printTicket = function (conftick) {
            console.log("TICKETERA CONF: ", conftick);
            window.open( full + "/Ticketera/Ticket/?ajax=1&" + jQuery.param(conftick));
        }

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

        getGrupos().then(function (promise) {
            self.grupos = promise.data;
        });
    });