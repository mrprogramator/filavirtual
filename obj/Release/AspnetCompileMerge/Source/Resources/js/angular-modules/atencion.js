angular.module('CatalogoApp')
    .controller('AtencionController', function ($http, $sce, $location) {
        self = this;

        self.loading = true;

        getAtencion = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getAtenciones = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getAtencionesRange = function (inicio, fin, nroAgente) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/' + inicio + '/' + fin + '/' + nroAgente
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getAtencionesByTicket = function (inicio, fin, nroTicket, nroAgente) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/tickets/' + nroTicket + '/' + inicio + '/' + fin + '/' + nroAgente
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getPuntos = function () {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Punto/GetPuntos'
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
                url: 'catalogo/Parametro/GetEstadosAtencion'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        saveAtencion = function (data) {
            var promise =  $http({
                method: 'POST',
                url: 'filavirtual/Atencion/Create',
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

        updateAtencion = function (data) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Atencion/Edit',
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

        removeAtencion = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/Atencion/Remove/?id=' + id
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        getAudio = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/' + id + '/audio'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        downloadAudio = function (id) {
            var promise = $http({
                method: 'POST',
                url: 'filavirtual/atenciones/' + id + '/audio/download'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

            return promise;
        }

        function downloadURI(uri, name) {
            console.log('downloading audio...');
            var link = document.createElement("a");
            link.download = name;
            link.href = uri;
            link.click();
        }
        HTMLElement.prototype.click = function () {
            var evt = this.ownerDocument.createEvent('MouseEvents');
            evt.initMouseEvent('click', true, true, this.ownerDocument.defaultView, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
            this.dispatchEvent(evt);
        }
        getTransacciones = function (id) {
            return promise = $http({
                method: 'POST',
                url: 'filavirtual/DetalleAtencion/GetTransacciones/?atencionId=' + id
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
            tab.templateUrl = 'filavirtual/Atencion/?ajax=1&_=' + Date.now();
        }

        this.create = function (tab) {
            tab.templateUrl = 'filavirtual/Atencion/Create/?ajax=1&_=' + Date.now();
        }

        this.edit = function (tab, id) {
            tab.templateUrl = 'filavirtual/Atencion/Edit/?id=' + id + '&ajax=1&_=' + Date.now();
        }

        this.delete = function (tab, id) {
            tab.templateUrl = 'Atencion/Delete/?id=' + id + '&ajax=1&_=' + Date.now();
        }

        this.detail = function (tab, id) {
            tab.templateUrl = 'filavirtual/atenciones/' + id + '/detail?ajax=1&_=' + Date.now();
        }

        this.transacciones = function (tab, id) {
            tab.templateUrl = 'filavirtual/atenciones/' + id + '/transacciones?ajax=1&_=' + Date.now();
        }

        this.save = function (tab, data) {
            saveAtencion(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.update = function (tab, data) {
            updateAtencion(data).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.remove = function (tab, id) {
            removeAtencion(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.index(tab);
                }
            });
        }

        this.detailInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var atencion_id = params[params.indexOf('atenciones') + 1];
            getAtencion(atencion_id).then(function (promise) {
                self.item = promise.data;
                self.item.url = self.getAudioURL(self.item);
                self.loading = false;
                fechaStr = self.item.FechaEmision;
                fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
                self.item.fechaString = new Date(fechaUnicode).toLocaleString();
                getTransacciones(self.item.Id).then(function (promise2){
                    self.item.transacciones = promise2.data;
                    console.log('trans',self.item.transacciones);
                })
            });

            return self.item;
        }

        self.show = function () {
            $("#eyeSpan").toggleClass('glyphicon-eye-open').toggleClass('glyphicon-eye-close');
            $("#transacTable").toggleClass('esconder').toggleClass('');

            console.log('it works');
        }
        
        convertToDate = function (fechaStr) {
			fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
			return new Date(fechaUnicode);
        }

        self.detalleInit = function (tab) {
            var params = tab.templateUrl.split('/');
            var atencion_id = params[params.indexOf('atenciones') + 1];

            getAtencion(atencion_id).then(function (promise) {
                self.item = promise.data;
                self.item.url = self.getAudioURL(self.item);
                self.loading = false;
                getTransacciones(self.item.Id).then(function (promise2){
                    self.item.transacciones = promise2.data;
                    self.item.transacciones.forEach(function (trans) {
                        var index = self.item.transacciones.indexOf(trans);
                        trans.Fecha = convertToDate(trans.Fecha);
                        trans.FechaFin = convertToDate(trans.FechaFin);
                        trans.Tiempo = trans.FechaFin - trans.Fecha;
                        /*if (index == 0)
                            trans.Fecha = convertToDate(trans.Fecha);
                        console.log('index', index);
                        if (index >= self.item.transacciones.length - 1) {
                            console.log('item', self.item);
                            self.item.FechaFin = convertToDate(self.item.FechaFin);
                            trans.Tiempo = self.item.FechaFin - trans.Fecha;
                            console.log('t', trans.Tiempo);
                            return;
                        }
                        self.item.transacciones[index+1].Fecha = convertToDate(self.item.transacciones[index+1].Fecha);

                        trans.Tiempo = self.item.transacciones[index+1].Fecha - trans.Fecha - 1000;
                        */
                        console.log('t', trans.Tiempo); 
                    })
                    console.log('trans',self.item.transacciones);
                })
            })
        }

        self.download = function (id) {
            downloadURI(self.item.url,self.item.LogUsr +'_' + self.item.NroTicket + '_' + self.item.fechaString + '.ogg');
            /*downloadAudio(id).then(function (promise) {
                self.response = promise.data;
                if (self.response.result) {
                    self.info = { state: true, message:'Audio exportado con éxito' }
                }
            });*/
        }
        

        this.getAudioURL = function (item) {
            return 'api/AtencionApi/GetRecording/' + item.Id;
        };

        getAtenciones().then(function (promise) {
            self.items = promise.data;
            console.log(self.items[0]);
            self.items.forEach( function (item) {
                fechaStr = item.FechaEmision;
				fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
				item.FechaEmision = new Date(fechaUnicode);

				fechaStr = item.FechaLlamado;
				fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
				item.FechaLlamado = new Date(fechaUnicode);
            });

            self.loading = false;
        });
        getPuntos().then(function (promise) {
            self.puntos = promise.data;
        });

        getEstados().then(function (promise) {
            self.estados = promise.data;
        });

        self.search = function () {
            inicio = $('#dTPInicio').val();

            inicio = inicio.split("/")[2] + '-' + inicio.split("/")[1] + '-' + inicio.split("/")[0];



            fin = $('#dTPFin').val();

            fin = fin.split("/")[2] + '-' + fin.split("/")[1] + '-' + fin.split("/")[0];



            nroAgente = $('#nroAgente').val();

            nroTicket = $('#nroTicket').val();
            
            self.loading = true;
            self.items = [];
                
            if (nroTicket == "") {
                getAtencionesRange(inicio, fin, nroAgente).then(function (promise) {
                    self.items = promise.data;
                    self.items.forEach( function (item) {
                        fechaStr = item.FechaEmision;
					    fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					    item.FechaEmision = new Date(fechaUnicode);

					    fechaStr = item.FechaLlamado;
					    fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					    item.FechaLlamado = new Date(fechaUnicode);
                    });
                    self.loading = false;
                })
                return;
            }

            getAtencionesByTicket(inicio,fin,nroTicket,nroAgente).then(function (promise) {
                self.items = promise.data;
                self.items.forEach( function (item) {
                    fechaStr = item.FechaEmision;
					fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					item.FechaEmision = new Date(fechaUnicode);

					fechaStr = item.FechaLlamado;
					fechaUnicode = fechaStr.substring(6, fechaStr.length - 2) * 1;
					item.FechaLlamado = new Date(fechaUnicode);
                });
                self.loading = false;
            })


            
        }
    });