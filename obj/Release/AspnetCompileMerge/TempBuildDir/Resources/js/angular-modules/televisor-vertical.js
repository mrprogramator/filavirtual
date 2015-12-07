angular.module('TelevisorApp', [])
    .controller('TelevisorController', function ($scope, $http, $sce, $timeout, $location) {

        url = $location.$$absUrl.substr(0, $location.$$absUrl.indexOf("/puntos"));

        //var bell = new Audio("http://www.orangefreesounds.com/wp-content/uploads/2015/01/Sound-of-a-doorbell.mp3");


        var connection = $.hubConnection(url);

        var mainHub = connection.createHubProxy('mainHub');

        $scope.video = url + "/Resources/video/beach.mp4";
        //$scope.video = $sce.trustAsResourceUrl("https://www.youtube.com/watch?v=QcIy9NiNbmo");

        connection.start().done(function () {
            console.log('connection started');
            mainHub.invoke("joinGroup", $scope.puntoId);
        })
        .fail(function (err) {
            console.log('cannot connect: ' + err);
        });

        mainHub.on('callTicket', function (ticket) {
            console.log('calling ticket', ticket);
            $scope.refreshItems(ticket);
        });

        mainHub.on('addTicketCalled', function (ticket) {
            console.log('adding ticket', ticket);
            $scope.refreshItems(ticket);
        });

        mainHub.on('removeTicket', function (ticket) {
            console.log('removing ticket', ticket, 'asignados', $scope.asignados);
            x = $scope.asignados.filter(function (item) {
                return item.NroTicket == ticket.NroTicket
                    && item.AgenteId == ticket.AgenteId;
            })[0];
            removeItemFromArray(x, $scope.asignados);


        });

        function removeItemFromArray(item, array) {
            var index = array.indexOf(item);
            console.log('index', index);
            if (index >= 0) {
                array.splice(index, 1);
            }
            console.log('after:asignados', $scope.asignados);
        }

        getPunto = function (puntoId) {
            var promise = $http({
                method: 'POST',
                url: url + '/filavirtual/puntos/' + puntoId
            })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

            return promise;
        }

        $scope.asignados = [];
        $scope.refreshItems = function (ticket) {

            if ($scope.asignados.length > 4) {
                $scope.asignados.pop();
            }

            var existente = $scope.asignados.filter(function (item) {
                return item.NroTicket == ticket.NroTicket
                    && item.AgenteId == ticket.AgenteId;
            })[0];

            console.log('existente', existente);
            if (existente == undefined) {
                $scope.asignados.unshift(ticket);
            }

            $scope.added(ticket.NroTicket + ticket.AgenteId);
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

            if (item.attr('class') == undefined) {
                $('li').first().toggleClass('called');
            }
            else {
                item.toggleClass('called');
            }
            blinkT = $timeout(function () {
                blink(ticket, c, blinkT);
            }, 1000)

        }

        $scope.added = function (ticket) {
            var bell = new Audio(url + "/Resources/sound/door-bell.ogg");
            bell.play();
            blink(ticket, 0);
        };

        $scope.init = function () {

        }

        param = $location.$$absUrl.split('/');
        puntoId = param[param.indexOf("puntos") + 1];
        getPunto(puntoId).then(function (promise) {
            $scope.punto = promise.data;
        })
        console.log('punto', puntoId);
        $scope.puntoId = puntoId;
    });