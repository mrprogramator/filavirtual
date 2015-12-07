var full = getFullUrl();

var app = angular.module('CatalogoApp');

app.controller('TreeviewController', function ($http, $timeout, $location) {


    self = this;
    this.data = [];
    this.loading = true;

    function isLeafOf(leaf) {
        return (leaf.PadreId == this);
    }

    function fillTree(jsonResponse, tree, root) {
        var hasAcc = false;
        jsonResponse.filter(isLeafOf, root.id).forEach(function (elem) {
            root.children.push({
                id: elem.Id,
                label: elem.Nombre,
                templateUrl: ((elem.Url == undefined || elem.Url == '') ? undefined : elem.Url + '?ajax=1'),
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

    getPrograms = function () {
        var promise = $http({ method: 'POST', url: $location.$$absUrl + 'catalogo/programas' })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false };
            });

        return promise;
    }

    getAccessByUser = function (user) {
        var promise = $http({
            method: 'POST',
            url: $location.$$absUrl + 'Acceso/GetAccessByUser/?id=' + user
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
            url: $location.$$absUrl + 'catalogo/AccesoGrupo/GetAccessByGroup/?id=' + group
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
            url: $location.$$absUrl + 'catalogo/UsuarioGrupo/GetGroupsByUser/?id=' + id
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
            url: $location.$$absUrl + 'catalogo/Acceso/GetAccessByProgram/?id=' + id
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
            url: $location.$$absUrl + 'catalogo/AccesoGrupo/GetAccessByProgram/?id=' + id
        })
            .success(function (data, status, headers, config) {
                return data;
            })
            .error(function (data, status, headers, config) {
                return { "status": false }
            });

        return promise;
    }

    cleanProgs = function (progs, acc) {
        var clean_progs = [];
        acc.forEach(function (a) {
            progs.forEach(function (prog) {
                if (a.ProgramaId == prog.Id) {
                    console.log('entro', a);
                    if (a.CheckSel) {
                        clean_progs.unshift(prog);
                    }
                }
            })
        })
        
        clean_progs.sort(function (p1, p2) {
            return p1.Orden - p2.Orden
        });
        
        return clean_progs;
    }

    joinAccess = function (user, group) {
        user.forEach(function (ua) {
            group.forEach(function (ga) {
                if (ua.ProgramaId == ga.ProgramaId) {
                    ua.CheckSel = ua.CheckSel || ga.CheckSel;
                    ua.CheckIns = ua.CheckIns || ga.CheckIns;
                    ua.CheckMod = ua.CheckMod || ga.CheckMod;
                    ua.CheckEli = ua.CheckEli || ga.CheckEli;
                    ua.CheckImp = ua.CheckImp || ga.CheckImp;
                    group.remove(ga);
                }
            })
        })

        group.forEach(function (ga) {
            var newAcc = {
                ProgramaId: ga.ProgramaId,
                CheckSel: ga.CheckSel,
                CheckIns: ga.CheckIns,
                CheckMod: ga.CheckMod,
                CheckEli: ga.CheckEli,
                CheckImp: ga.CheckImp
            }
            user.push(newAcc);
        })
    }

    self.init = function () {
        var id = localStorage.getItem('user');
        getAccessByUser(id).then(function (promise) {
            self.userAccess = promise.data;
            console.log('uA', self.userAccess);
            allAccess.byUser = self.userAccess;
            getGroupsByUser(id).then(function (promise2) {
                var groups = promise2.data;
                console.log('groups', groups);
                if (groups.length == 0) {
                    getPrograms().then(function (promise) {
                        progs = promise.data;
                        progs = cleanProgs(progs, self.userAccess);
                        console.log('progs', progs);
                        self.loading = false;
                        progs
                            .filter(function (p) {
                                return p.PadreId == undefined
                            })
                            .forEach(function (s) {
                                fillTree(
                                progs,
                                my_tree, {
                                    id: s.Id,
                                    label: s.Nombre,
                                    children: []
                                })
                            });
                        return true;
                    });

                    self.data = my_tree;
                    return;
                }
                groups.forEach(function (group) {
                    getAccessByGroup(group.GrupoId).then(function (promise3) {
                        self.groupAccess = promise3.data;
                        console.log('gA', self.groupAccess);
                        allAccess.byGroup = self.groupAccess;
                        getPrograms().then(function (promise) {
                            progs = promise.data;
                            joinAccess(self.userAccess, self.groupAccess);
                            progs = cleanProgs(progs, self.userAccess);
                            console.log('progs on groupSide', progs);
                            self.loading = false;
                            progs
                            .filter(function (p) {
                                return p.PadreId == undefined
                            })
                            .forEach(function (s) {
                                fillTree(
                                progs,
                                my_tree, {
                                    id: s.Id,
                                    label: s.Nombre,
                                    children: []
                                })
                            });
                            return true;
                        });

                        self.data = my_tree;
                    })
                })
            })
        })
    }

    this.selectedBranch = null;

    this.handleClick = function (tabctrl) {
        var self = this;
        $timeout(function () {
            tabctrl.addTab(self.selectedBranch.label, self.selectedBranch.templateUrl);
        }, 0);
    };

    var my_tree = [];
    self.init();
});
