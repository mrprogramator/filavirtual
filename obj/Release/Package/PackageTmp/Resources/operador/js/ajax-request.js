/*
This section contains ajax requests functions to the server
*/

//var full = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
var Id = 0;
var User = {Id:'N/A'};

function setAttId(id){
    Id = id;
}

function setUser(user) {
	console.log('setting user..',user);
	User = user;
}