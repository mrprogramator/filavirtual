angular.module("CatalogoApp")
    .controller("ConfTickController", function ($http, $location) {
        var self = this;

        self.printTicket = function (conftick) {
            console.log("TICKETERA CONF: ", conftick);
            window.open(full + "/Ticketera/Ticket/?ajax=1&" + jQuery.param(conftick));
        }

        self.init = function (id) {
            //this.ticketId = $location.search().ticketId;
            this.ticketId = id;
            console.log("id", self.ticketId);
            this.grupos = [];


            this.ticketera = getTicketera(self.ticketId).responseJSON[0];
            console.log("TICK", this.ticketera);
            this.cargarGrupos = function () {
                var grupos = getGrupoConf(self.ticketId).responseJSON;
                console.log(grupos);
                console.log(self.ticketId);
                grupos.forEach(function (group) {
                    group.children = getChildGroup(group.Id, self.ticketId).responseJSON;
                    console.log(group.children);
                });

                self.grupos = grupos;

            };

            this.cargarExtremos = function () {
                var grupos = self.grupos;
                console.log(grupos);
                grupos.forEach(function (grupo) {
                    grupo.firstChild = grupo.children.shift();
                    grupo.lastChild = grupo.children.pop();
                    console.log('groupchildren', grupo.children);
                });
            };

            this.cargarGrupos();
            this.cargarExtremos();
            if (this.ticketId === undefined) {
                self.ticketera = { Descripcion: "No se ha especificado la ticketera" };
            }

        };
    });