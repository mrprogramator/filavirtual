angular.module('ClockApp', [])
    .controller('ClockController', function ($timeout) {
        self = this;
        self.shown = true;
        function refreshTime() {
            h = new Date().getHours();
            m = new Date().getMinutes();
            s = new Date().getSeconds();
            self.hh = checkTime(h);
            self.mm = checkTime(m);
            self.ss = checkTime(s);
            self.shown = !self.shown;
            console.log(h,m,s);
            if (self.shown)
                self.clockString =h + ":" + m;
            else
                self.clockString = h + "\t" + m;

            $timeout(function () {
                refreshTime()
            }, 1000);
        }

        function checkTime(i) {
            if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
            return i;
        }

        refreshTime();
    });