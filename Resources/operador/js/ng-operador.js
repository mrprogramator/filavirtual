deps = ['angularBootstrapNavTree'];
deps.push('ngAnimate');
var app = angular.module('OperadorApp', deps);
INACTIVO    = 0;
ACTIVO      = 1;
AUSENTE     = 2;
AGEACT = "estage1";
AGEINA = "estage2";

app.controller("OperadorController", function ($http, $location, $timeout,  $cacheFactory, $window, $scope, $compile) {
    url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/operador"));
    var connection = $.hubConnection(url);
    var mainHub = connection.createHubProxy('mainHub');
    console.log('URL', url);

    connection.start().done(function () {
        console.log('connection started');
        mainHub.invoke('joinGroup', self.agente.PuntoId);
    })
    .fail(function (err) {
      console.log('cannot connect: ' + err);
    });

    getFila = function () {
        var promise = $http({ 
            method: 'POST', 
            url: url + '/filavirtual/mesas/'+ self.agente.nroAgente +'/fila' 
        })
        .success(function (data, status, headers, config) {
                return data;
        })
        .error(function (data, status, headers, config) {
                return { "status": false };
        });

        return promise;
    }

    getHijos = function (id) {
        var promise = $http({
            method: 'POST',
            url: url + '/catalogo/parametros/' + id + '/hijos'
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });

        return promise;
    }

    getTiempoLlamaout = function () {
        var promise = $http({
            method: 'POST',
            url: url + '/catalogo/parametros/tiempo-llamaut'
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });

        return promise;
    }

    getAtendidos = function (nroAgente) {
            var promise = $http({ 
                method:'POST', 
                url: url + "/filavirtual/mesas/" + nroAgente + "/atenciones/atendidas" 
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
    }

    getTiposAtencion = function (id) {
            var promise = $http({ 
                method:'POST', 
                url: url + '/catalogo/parametros/' + id + '/hijos' 
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
    }

    pullFirstInLine = function (nroAgente) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/mesas/"+ nroAgente + "/fila/pull"       
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });
        return promise;
    }

    abandonarTicket = function (id) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/fila/" + id + "/abandonar"
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });
        return promise;
    }

    recuperarTicket = function (nroAgente, id) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/mesas/" + nroAgente + "/fila/" + id + "/recuperar"
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });
        return promise;
    }

    createAttention = function (first) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/atenciones/create",
            data: first
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });

        return promise;
    }

    finishAttention = function (atencion) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/atenciones/"+ atencion.Id + "/finalizar",
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    setDtAtencion = function (id) {
        console.log('id',id);
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/detalle-atenciones/create",
            data: {AtencionId: self.atencion.Id, ServicioId: id, Observaciones: '' }
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    changeDtAtencion = function (x) {
        console.log("detalleT", self.detalleT);
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/detalle-atenciones/update",
            data: x
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false };
        });

        return promise;
    }

    updateDtAtencion = function (data) {
        console.log("detalleT",self.detalleT);
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/detalle-atenciones/update",
            data: data
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    getExcuses = function () {
        var promise = $http({
            method: "POST",
            url: url + "/catalogo/parametros/ausencias-agente",
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    getMesa = function (id) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/mesas/" + id,
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }
  
    registerState = function (id) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/agentes/" + self.agente.nroAgente + "/estados/"+ id +"/registrar",
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    registerExcuse = function (id, excuse_id) {
        console.log('E X C U S E ID', excuse_id);
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/agentes/" + self.agente.nroAgente + "/estados/" + id + "/motivos/" + excuse_id + "/registrar",
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }

    finalizeState = function (id) {
        var promise = $http({
            method: "POST",
            url: url + "/filavirtual/agentes/estados/" + id + "/finalizar",
        })
        .success(function (data, stauts, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return {"status": false};
        });

        return promise;
    }     

    function startTimeReverse(h, m, s, callback) {
        //console.log('starting $timeout');
        if (m == 0 && h > 0)
        {
          h = h - 1;
          m = 59;
        }
        if (s == 0 && m > 0)
        {
          m = m - 1;
          s = 59;
        }
        s = s - 1;
        self.hs = checkTime(h);
        self.ms = checkTime(m);
        self.ss = checkTime(s);
        if ( h<=0 && m<=0 && s<0){
          //console.log('clock stopped');
          callback();
          return;
        }
        //console.log(self.hs,":",self.ms,":",self.ss)
        document.getElementById('clock').innerHTML = self.hs+":"+self.ms+":"+self.ss;
        self.promise = $timeout( function () {
            startTimeReverse(h,m,s,callback)
        },1000);
    }

    function startTime(h, m, s) {
        //console.log('starting $timeout');
        s = s + 1;
        if (s>59) {
            s = 0;
            m = m + 1;
        }

        if (m>59)
        {
            m = 0;
            h = h + 1;
        }
        self.shs = checkTime(h);
        self.sms = checkTime(m);
        self.sss = checkTime(s);
        
        //$(".clock")[0].innerHTML = self.shs+":"+self.sms+":"+self.sss;
        self.promise2 = $timeout( function () {
            startTime(h,m,s)
        },1000);
    }

    function checkTime(i) {
        if (i<10) {i = "0" + i};  // add zero in front of numbers < 10
        return i;
    }

    var self = this;
    self.activo = ACTIVO;

    
    self.client = {NroTicket:'N/A'}


    self.autenicated = true;
    self.atendiendo = false;

    self.response = {result: false, value: 'obteniendo datos...'}    
    
    self.refreshLine = function () {
        getFila().then(function (promise) {
            if (self.fila != undefined) {
                var oldLength = self.fila.length;
            }
            self.fila = promise.data;
            if (oldLength == 0 && !self.atendiendo && self.first == undefined) {
                self.hasCalled = false;
                timeoutCallTicket(self.tllamaut * 1000);
            }
        });
        getAtendidos(self.agente.nroAgente).then(function (promise) {
            self.pasados = promise.data;
        });
    }

    mainHub.on('update', function () {
        console.log('ACTUALIZA LA FILA !!!!!!');
        self.refreshLine();
    });

    self.activar = function () {
        console.log('activando');
        self.activo = ACTIVO;

        if (self.estado != undefined) {
            finalizeState(self.estado.Id).then(function (promise) {
                self.response = promise.data;
                self.hasCalled = false;
                timeoutCallTicket(self.tllamaut * 1000);
            });
        }

        registerState(AGEACT).then(function (promise) {
            self.response = promise.data;
            if (self.response.result == true) {
                self.estado = self.response.value;
                localStorage.setItem('estado', self.estado.Id);
            }            
        });
        
        $timeout.cancel(self.promise);
    }

    self.abandonar = function () {
        $('#skipConfirmModal').modal('hide');
        if (self.first != null) {
            abandonarTicket(self.first.Id).then(function (promise) {
                mainHub.invoke("removeTicket", self.agente.PuntoId, self.first);
                self.response = promise.data;
                self.first = null;
                self.info = {status: false};
                self.hasCalled = false;
                timeoutCallTicket(self.tllamaut * 1000);
            });
        }
    }

    self.desactivar = function (excuse){
        self.activo = AUSENTE;
        
        if (self.estado != undefined) {
            console.log('estado',self.estado);
            finalizeState(self.estado.Id).then(function (promise) {
                self.response = promise.data;
            });
        }

        registerExcuse(AGEINA, excuse.Id).then(function (promise) {
            self.response = promise.data;
            if (self.response.result == true) {
                self.estado = self.response.value;
                localStorage.setItem('estado', self.estado.Id);
            }
        });

        startTimeReverse(0,parseInt(excuse.Valor),0,self.inactivar);
    }

    self.endInit = function () {
            self.response.result=true;
    }

    self.inactivar = function () {
        console.log('inactivando');
        self.activo = INACTIVO;
        $timeout.cancel(self.promise);
    }

    self.rellamar = function() {
        if (self.first == null)
        {
            self.response.result = false;
            self.response.value = "No se ha llamado al ticket";
            return;
        }
        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        console.log('rellamando');
        mainHub.invoke("callTicket", self.agente.PuntoId, self.first);
    }

    self.siguiente = function () {
       
        if (self.waiting) {
            self.response = { result: false, value: 'Ya se ha llamado al ticket, espere un momento' }
            return;
        }
        if (self.atendiendo) {
            self.response = {result:false, value:'No llame al siguiente ticket hasta finalizar la atención'}
            return;
        }

        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        self.waiting = true;
        pullFirstInLine(self.agente.nroAgente).then(function (promise) {
            self.response = promise.data;
            console.log(self.response);
            mainHub.invoke("removeTicket", self.agente.PuntoId, self.first);
            self.waiting = false;
            if (!self.response.result) {
                return;
            }

            self.first = self.response.value;
            console.log('serv',self.first);
            getTiposAtencion(self.first.ServicioId).then(function (promise) {
                self.transacciones = promise.data;
                self.Servicio = promise.data[0].Grupo.Nombre;
                
                self.loadTreeView(self.first.ServicioId);

                self.response = { result: true }    
            })
            mainHub.invoke("addTicketCalled", self.agente.PuntoId, self.first);
            self.info = { status:true, value:'Se ha llamado al ticket ' + self.first.NroTicket }
        })
    }

    self.iniciarTransaccion = function (x) {

        if (x == undefined) {
            self.response = { result: false, value: 'No ha seleccionado ningún detalle de atencion' }
            return;
        }

        if (x.children.length > 0) {
            self.transInfo = {
                status: true,
                value: 'Debe seleccionar una opción sin subniveles',
                img: url + '/Resources/img/manejo-trans.png'

            }
            $("#messageModal").modal('show');
            return;
        }

        $('#transaccionModal').modal('hide');

        console.log('detalle', x.id);
        setDtAtencion(x.id).then(function (promise) {
            self.response = promise.data;

            if (self.response.result) {
                self.detalleT = self.response.value;
                console.log('dtatt:', self.response.value);

                self.transInit = false;

            }
            else {
                console.log('Error al guardar detalle.', self.response);
            }
        });
    }

    self.recuperar = function (ticket) {
        if (self.atendiendo) {
            self.response = {result:false, value:'No llame al ticket hasta finalizar la atención'}
            return;
        }

        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        recuperarTicket(self.agente.nroAgente, ticket.Id).then(function (promise) {
            self.first = promise.data.value;
            self.first.nroAgente = self.agente.nroAgente;

            getTiposAtencion(self.first.ServicioId).then(function (promise) {
                self.transacciones = promise.data;
                self.Servicio = promise.data[0].Grupo.Nombre;
                self.loadTreeView(self.first.ServicioId);
                self.response = { result: true }
            })
            self.response = { result: false, value: 'Se ha recuperado el ticket ' + ticket.NroTicket }
            self.info = { status: true, value: 'Se ha recuperado el ticket ' + ticket.NroTicket }
        })
        
    }

    self.llamar = function () {
        mainHub.invoke("refreshAll");
        if (self.hasCalled) {
            self.response = { result: false, value: 'Ya se ha llamado al ticket, espere un momento' }
            return;
        }

        self.hasCalled = true;
        if (self.atendiendo){
            self.response = {result:false, value:'No llame al siguiente ticket hasta finalizar la atención'}
            return;
        }
        
        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        console.log('llamar() and first = ', self.first);
        if (self.first != undefined){
            console.log('first no es undefined');
            self.info = {status: true, value: 'Ya se ha llamado al ticket ' + self.first.NroTicket }
            console.log('info',self.info);
            return;
        }
        console.log('going to the next()');
        self.siguiente();
        $timeout.cancel(self.promise3);
    }

    self.atender = function () {
        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        if (self.atendiendo || self.first == undefined){
            console.log('first es undefined en atender, no se atiende');
            return;
        }
        self.info = {status: false}
        self.atendiendo = true;
        self.transInit = true;
        createAttention(self.first).then(function (promise) {
            self.response = promise.data;

            if (self.response.result){
                self.client = self.first;
                self.atencion = self.response.value;
                setAttId(self.atencion.Id);
            }
            else{
                self.atendiendo = false;
            }
        });

        startTime(0,0,0);
    }

    self.dtSelected = {};

    self.handleDtSelected = function (att) {
        if (!self.atendiendo){
            console.log('el operador no está atendiendo !!!. No se puede registrar la transacción');
            return;
        }
        self.dtSelected = att;
        console.log('dtSelected:',self.dtSelected);
    }
    
    addComboBox = function (childrenObjectName) {
        var modelName = makeid();

        var transaccionName = makeid();

        var transContainer = document.getElementById('trans-container');
        transContainer.innerHTML = transContainer.innerHTML + "<select id=" + modelName + " ng-change=\"operador.changeDtAt(" + modelName + ", \'" + modelName+ "\')\" class='col-xs-4 form-control' ng-model='" + modelName +".Id' required>" +
                                                                  "<option ng-repeat='" + transaccionName + " in " + childrenObjectName +"' value='{{" + transaccionName + ".Id}}'>" + 
                                                                        "{{" + transaccionName + ".Nombre}}" + 
                                                                    "</option>" +
                                                            "</select>"
        $compile(document.getElementById('trans-container'))($scope);
    }

    function makeid() {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 5; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }
    
    self.setDtAt = function (x, varname) {
        console.log('HOLAA', x, varname);
        if (x == undefined) {
            self.response = { result: false, value: 'No ha seleccionado ningún detalle de atencion'}
            return;
        }
        console.log('detalle',x.Id);
        setDtAtencion(x.Id).then(function (promise){
            self.response = promise.data;

            if (self.response.result){
                self.detalleT = self.response.value;
                self.response = {result: false, value: 'Se ha registrado la transacción'};
                console.log('dtatt:', self.response.value);
                getHijos(x.Id).then(function (promise2) {
                    x.hijos = promise2.data;

                    if (x.hijos.length > 0) {
                        addComboBox(varname + '.hijos');
                    }

                    console.log('HIJOS DE ' + x.Id, x.hijos);
                })

                self.transInit = false;
                
            }
            else {
                console.log('Error al guardar detalle.',self.response);
            }
        });
    }

    self.updateDtAt = function () {
        if (self.detalleT == undefined) {
            console.log('detalleT undefined');
            return;
        }

        console.log('D E T A L L E   D E  A T E N C I O N', self.detalleT);
        self.detalleT.Observaciones = self.Observaciones;
        self.Observaciones = "";
        self.selectedBranch = null;
        updateDtAtencion(self.detalleT).then(function (promise) {
            self.response = promise.data;
            self.canFinalize = true;
            self.transInit = true;
        });
    }

    self.changeDtAt = function (x, varname) {
        if (x == undefined) {
            return;
        }

        self.detalleT.ServicioId = x.Id;
        updateDtAtencion().then(function (promise) {
            self.response = promise.data;
            self.canFinalize = true;
            self.transInit = true;
            console.log('HOLAAAA RESP FROM CHANGE', self.response);
            if (self.response.result) {
                self.detalleT = self.response.value;
                self.info = { status: true, value: 'Se ha cambiado la transacción' };
                console.log('dtatt:', self.response.value);
                getHijos(x.Id).then(function (promise2) {
                    x.hijos = promise2.data;

                    if (x.hijos.length > 0) {
                        addComboBox(varname + '.hijos');
                    }

                    console.log('HIJOS DE ' + x.Id, x.hijos);
                })
            }
        });
    }

    self.finalizar = function() {


        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }
        if (!self.atendiendo){
            return;
        }

        if (!self.canFinalize) {
            return;
        }
        
        self.dtSelected = undefined;
        self.Servicio = undefined;
        
        self.atendiendo = false;
        finishAttention(self.atencion).then(function (promise) {
            self.response = promise.data;
            self.client = {NroTicket:'N/A'}
            self.atencion = {}
            self.transInit = false;
            self.first = undefined;
            self.canFinalize = false;
            self.hasCalled = false;
            self.refreshLine();
        });
        $timeout.cancel(self.promise2);

        mainHub.invoke("removeTicket", self.agente.PuntoId, self.first);

        timeoutCallTicket(self.tllamaut*1000);
    }

    function timeoutCallTicket(t) {
        self.promise3 = $timeout(function () {
            self.llamar();
        }, t);

    }

    self.closeAlert = function() {
        if (self.activo != ACTIVO) {
            console.log("operador no está activo!!!");
            return;
        }

        self.response = { result: true }
    }

    self.logout = function () {
        localStorage.removeItem('user');
        self.info = { status: true, value: 'Cerrando sesión...' }
        if (self.estado != undefined) {
            finalizeState(self.estado.Id).then(function (promise) {
                self.response = promise.data;
            });
        }
        else {
            console.log('ESTADO NULO!');
        }
        self.checkUser();
    }

    self.checkUser = function () {
        self.myUser = localStorage.getItem('user');
        self.myLog = localStorage.getItem('log');
        console.log(self.myUser);
        if (self.myUser == undefined) {
            self.autenicated = false;
            console.log('loc', $location.absUrl());
            var landingUrl = $location.absUrl().toString();
            landingUrl.replace("index","login");
            console.log('user not aut', landingUrl);
            $window.location.href = 'operador/login';
            return;
        }
        else {
            self.agente = { nroAgente: self.myUser, logUsr: self.myLog }
            console.log('user aut');
            self.autenicated = true;
        }
    }

    getExcuses().then(function (promise) {
        self.excuses = promise.data;
    })

    self.setInitialState = function () {
        registerState(AGEACT).then(function (promise) {
            if (promise.data.result == true) {
                self.estado = promise.data.value;
                localStorage.setItem('estado',self.estado.Id);    
            }
        });
        self.estado = { Id: self.myState }
    }

    getTiempoLlamaout().then(function (promise) {
        self.tllamaut = promise.data.Valor;
        timeoutCallTicket(self.tllamaut * 1000);
    })

    self.checkUser();
    self.setInitialState();
    self.refreshLine();
    self.endInit();
    console.log(self.response);
    



    getMesa(self.agente.nroAgente).then( function (promise) {
        self.agente.PuntoId = promise.data.PuntoId;
        console.log("AGENTE", self.agente);
    });

    self.handleClose = function () {
        if (!self.activo) {
            return;
        }
        localStorage.removeItem('user');
        self.info = {status: true, value: 'Finalizando el programa...'}
        if (self.estado != undefined) {
            finalizeState(self.estado.Id).then(function (promise) {
                self.response = promise.data;
                window.close();
            });
        }
        else {
            console.log('ESTADO NULO!');
            window.close();
        }
    }

    //TREEVIEW

    self.treeLoading = true;

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

    getTransacciones = function () {
        var promise = $http({
            method: 'POST',
            url: url + '/catalogo/parametros/vigentes'
        })
        .success(function (data, status, headers, config) {
            return data;
        })
        .error(function (data, status, headers, config) {
            return { "status": false }
        });

        return promise;
    }

    self.handleClick = function () {
        var self = this;
        $timeout(function () {
            self.selectedItem = self.items
                .filter(function (item) {
                    return item.Id == self.selectedBranch.id;
                })[0];
            console.log('selected', self.selectedBranch);
        }, 0);
    }

    self.loadTreeView = function (servicioId) {
        my_tree = [];
        console.log('SERVICIO ID', servicioId);
        getTransacciones().then(function (promise) {
            self.items = promise.data;
            console.log('transacciones', self.items);
            promise.data
                .filter(function (param) {
                    return param.GrupoId == servicioId;
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
            console.log('Cómo que no hay tree data? :', my_tree);
            console.log('ITEMS', self.items);

            self.treeLoading = false;
            console.log('LOADING......', self.treeLoading);
        });
    }
});
