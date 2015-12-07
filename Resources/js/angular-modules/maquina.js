angular.module('TicketeraApp', [])
    .controller('MaquinaController', function ($http, $location, $timeout) {
        CODGRU = 'TICGRU';
        TICKETNORMAL = 'NORM';

        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/puntos"));

        self = this;

        self.loading = true;
        
        getTicketera = function (puntoId, id) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/puntos/' + puntoId + '/ticketeras/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getPasswordTicketera = function () {
            var promise = $http({
                method: 'POST',
                url: url + '/catalogo/parametros/password-ticketera'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });
            console.log('password',promise);
            return promise;
        }

        getGrupos = function (id) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/ticketeras/' + id + '/configs/grupos'
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
                url: url + '/filavirtual/ticketeras/'+ ticketeraId + '/configs/grupos/' + padreId + '/hijos'
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
                url: url + '/filavirtual/ticketeras/' + ticketeraId + '/configs'
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
                url: url + '/catalogo/parametros/tipos'
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
                url: url + '/catalogo/parametros/tipo-atenciones'
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
                url: url + '/catalogo/parametros/tipos-ticket'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }
        
        self.togglePreferencialButtons = function () {
            self.ticketera.grupos.forEach(function (grupo) {
                grupo.hijos.forEach(function (hijo) {
                    if (hijo.TipoAtencionId != TICKETNORMAL) {
                        hijo.show = !hijo.show;
                        if (hijo.show) {
                            grupo.shownChildrenLength = grupo.shownChildrenLength + 1;
                        }
                        else {
                            grupo.shownChildrenLength = grupo.shownChildrenLength - 1;
                        }
                    }
                })
            })
        }

        self.init = function (tab) {
            if (tab == undefined) {
                console.log('loc',$location);
                param = $location.$$absUrl.split('/');
                puntoId = param[param.indexOf("puntos")+1];
                id = param[param.indexOf("ticketeras") + 1];
            }
            else {
                param = tab.templateUrl.split('/');
                console.log(param);
                puntoId = param[4];
                id = param[6];
            }

            getPasswordTicketera().then(function (promise) {
                self.correctPassword = promise.data.Valor;
            })

            getTicketera(puntoId, id).then(function (prom) {
                self.ticketera = prom.data;
                getItems(self.ticketera.Id).then(function (promise) {
                    self.items = promise.data;
                    console.log('ItEMS',self.items);
                    self.ticketera.grupos = promise.data.filter(function (p) {
                        return p.TipoId == CODGRU;
                    });

                    self.ticketera.grupos.forEach(function (grupo) {
                        grupo.shownChildrenLength = 0;
                        getChildren(self.ticketera.Id, grupo.Id).then (function (promise2) {
                            grupo.hijos = promise2.data;
                            grupo.hijos.forEach(function (hijo) {
                                if (hijo.TipoAtencionId == TICKETNORMAL) {
                                    grupo.shownChildrenLength = grupo.shownChildrenLength + 1;
                                    hijo.show = true;
                                }
                            });
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

        $('#confirmPreferencial').on('hidden.bs.modal', function (e) {
            self.password = "";
        });

        $('#errorAlert').on('close.bs.alert', function () {
        });

        self.closeAlert = function () {
            self.response = { result: true };
            console.log('RESPONSE ON CLOSE', self.response);

        }

        self.printTicket = function (conftick) {
            console.log("TICKETERA CONF: ", conftick);
            if (conftick == undefined) {
                if (self.selectedOption == undefined) {
                    return;
                }

                if (self.password != self.correctPassword) {
                    self.response = { result: false, value: "Clave Errónea. Intente de nuevo" }
                    console.log('RESPONSE ON WRONG ', self.response);
                    self.password = "";
                    return;
                }

                console.log('HOLA');
                conftick = self.selectedOption;
                var winpop = window.open(url + "/filavirtual/puntos/" + conftick.PuntoId + "/ticketeras/" + conftick.TicketeraId + "/ticket/" + conftick.Id + "/?ajax=1", "Ticket", "height=255,width=262");
                winpop.blur();
                window.focus();
                $('#confirmPreferencial').modal('hide');
            }
            else if (conftick.TipoAtencionId == TICKETNORMAL) {
                window.open(url + "/filavirtual/puntos/" + conftick.PuntoId + "/ticketeras/" + conftick.TicketeraId + "/ticket/" + conftick.Id + "/?ajax=1", "Ticket", "height=255,width=262");
                window.focus();
            }

            else {
                $('#confirmPreferencial').modal('show');
                self.selectedOption = conftick;
            }
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
        self.numbers = [];

        for (var i = 1; i <= 9; i++) {
            self.numbers.push(i);
        }
        self.password = "";
        console.log('NUMBERS',self.numbers);
        self.typePassword = function (n) {
            self.response = { result: true };
            console.log('hola', n , self.password);
            self.password = self.password + n;
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

        self.shown = true;
        function refreshTime() {
            h = new Date().getHours();
            m = new Date().getMinutes();
            s = new Date().getSeconds();
            self.hh = checkTime(h);
            self.mm = checkTime(m);
            self.ss = checkTime(s);
            self.shown = !self.shown;
            if (self.shown)
                self.clockString = self.hh + ":" + self.mm;
            else
                self.clockString = self.hh + "\t" + self.mm;
            $timeout(function () {
                refreshTime()
            }, 1000);
        }
        self.dateString = moment().lang("es").format("dddd D MMMM YYYY");

        function checkTime(i) {
            if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
            return i;
        }
        refreshTime();
    });