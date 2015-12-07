angular.module('TelevisorApp', [])
    .controller('TelevisorController', function ($http, $sce, $timeout, $location) {
        self = this;
        self.loading = true;
        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/puntos"));
        var bell = new Audio("http://www.orangefreesounds.com/wp-content/uploads/2015/01/Sound-of-a-doorbell.mp3");
        //var bell = new Audio(url + "/Resources/sound/door-bell.mp3");
        var connection = $.hubConnection(url);
        var mainHub = connection.createHubProxy('mainHub');
        self.video = url + "/Resources/video/beach.mp4";
        //self.video = $sce.trustAsResourceUrl("https://www.youtube.com/watch?v=QcIy9NiNbmo");

        connection.start().done(function () {
            console.log('connection started');
        })
        .fail(function (err) {
            console.log('cannot connect: ' + err);
        });
        mainHub.on('callTicket', function (ticket) {
            console.log('calling ticket', ticket);
            self.added(ticket);
        });

        mainHub.on('addTicketCalled', function () {
            console.log('adding ticket');
            self.refreshItems();
        })

        getPunto = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: url +'/puntos/' + puntoId
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        getAsignados = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: url + '/puntos/' + puntoId + '/atenciones/asignadas'
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        self.loadItems = function () {
            getAsignados(self.puntoId).then(function (promise) {
                self.asignados = promise.data;
                console.log(promise.data);
                self.loading = false;
            });
        }
        self.refreshItems = function () {
            getAsignados(self.puntoId).then(function (promise) {
                newInLine = promise.data[0];
                first = self.asignados[0];
                if (newInLine.NroTicket != first.NroTicket) {
                    self.asignados.unshift(promise.data[0]);
                    self.asignados.pop();
                }

                console.log(promise.data);
                self.loading = false;
            });
        }
        function blink(ticket, c, blinkT) {
            ++c;
            var item = $('#' + ticket);
            
            if (c == 6) {
                console.log('cancelling blink...');
                $timeout.cancel(blinkT);
                $('li').removeClass('called');
                item.removeClass('called');
                return;
            }
            
            console.log('item', item);
            if (item.attr('class') == undefined) {
                $('li').first().toggleClass('called');
            }
            else {
                console.log('get defined!!!!');
                item.toggleClass('called');
            }
            blinkT = $timeout(function () {
                blink(ticket, c, blinkT);
            }, 1000)

        }
        self.test = function (html) {
            console.log('html', html);
        }
        self.added = function (ticket) {
            bell.play();
            blink(ticket, 0);
        };

        self.addedH = function (ticket) {
            bell.play();
            blink(ticket, 0);
        }

        self.init = function () {
            param = $location.$$absUrl.split('/');
            puntoId = param[param.indexOf("puntos") + 1];
            getPunto(puntoId).then(function (promise) {
                self.punto = promise.data;
            })
            console.log('punto', puntoId);
            self.puntoId = puntoId;
            self.loadItems();
        }

        self.init();
    });